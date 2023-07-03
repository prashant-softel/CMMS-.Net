using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.EscalationMatrix;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Models.PM;
using System.Reflection;
using System.Runtime.CompilerServices;
using CMMSAPIs.Models.Notifications;
using static CMMSAPIs.Helper.CMMS;
using System.Linq;

namespace CMMSAPIs.Repositories.EscalationMatrix
{
    public class EscalationMatrixRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public EscalationMatrixRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<CMDefaultResponse> InsertEscalationMatrixData(CMEscalationMatrixModel request, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */

            CMMS_Status EnumValue = (CMMS_Status)Enum.Parse(typeof(CMMS_Status), request.Status);
            int StatusID = (int)EnumValue;

            string mainQuery = $"INSERT INTO escalationmatrix" +
                               $"(JobID, Status, " +
                               $"createdBy, CreatedAt, updatedBy," +
                               $"updatedAt, isActive)" +
                               $"VALUES" +
                                $"('{request.Module}', '{StatusID}', " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', null, " +
                                $"null, 1); SELECT LAST_INSERT_ID();";

            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            var LevelList = request.EscalationLevelList.OrderBy(item => item.Level).ToList();
            for (var i=0;i< LevelList.Count; i++)
            {
               
                    
                        string InsertQuery = $"INSERT INTO escalationlevelmapping" +
                       $"(Escalation_id,NoOfDays,Levels)" +
                       $"VALUES" +
                        $"('{id}'," +
                        $"{LevelList[i].NoOfDays}, '{LevelList[i].Level}'" +
                        $"); select 1 Dummay;";
                        DataTable dt = await Context.FetchData(InsertQuery).ConfigureAwait(false);
                 
            }


            //await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Data saved successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateEscalationMatrixStatus(CMEscalationMatrixModel request, int userID)
        {
            int isActive = 0;
            if (request.Status.ToLower().Contains("closed"))
            {
                isActive = 0;
            }
            else
            {
                isActive= 1;
            }
            CMMS_Status EnumValue = (CMMS_Status)Enum.Parse(typeof(CMMS_Status), request.Status);
            int StatusID = (int)EnumValue;
            string updateQ = "update escalationmatrix set Status = "+ StatusID + ", updatedBy = "+userID+", updatedAt='"+ UtilsRepository.GetUTCTime() + "', isActive = "+ isActive + " where Id = "+request.Id+";";
            var result = await Context.ExecuteNonQry<int>(updateQ);

            string updateQueryForLevel = "update escalationlevelmapping set IsDone = 1 where Escalation_id = "+request.Id+" and Levels = "+request.Levels+";";
            var resultForLevel = await Context.ExecuteNonQry<int>(updateQueryForLevel);

            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Data updated successfully.");
            return response;
        }

        internal async Task<int> getEscalationLevel()
        {
            //string myQuery = "select * from (\r\nSELECT  DATEDIFF(NOW(), EM.CreatedAt) AS DayDifference, em.Status as Status_ID, EL.NoOfDays,el.Levels\r\nFROM escalationmatrix EM \r\nINNER JOIN escalationlevelmapping EL ON EM.id = EL.Escalation_id\r\nWHERE EM.isActive = 1) a\r\nwhere a.DayDifference = a.NoOfDays;";

            string myQuery = "select * from (\r\nSELECT EM.Id,EM.JobID, EM.createdBy, EM.CreatedAt, EM.updatedBy, EM.updatedAt, DATEDIFF(NOW(), EM.CreatedAt) AS DayDifference, em.Status as Status_ID, EL.NoOfDays,el.Levels, SUM(EL.NoOfDays) OVER (PARTITION BY EL.Escalation_id ORDER BY EL.id) EscalatedDays\r\nFROM escalationmatrix EM \r\nINNER JOIN escalationlevelmapping EL ON EM.id = EL.Escalation_id\r\nWHERE EM.isActive = 1 and EL.IsDone = 0 ) a\r\nwhere a.DayDifference = a.EscalatedDays;\r\n";

            string escalationLevel = "select * from escalationlevel";
            DataTable dt = await Context.FetchData(escalationLevel).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
 

            try
            {
                List<CMEscalationMatrixModel> _EscalatedList = await Context.GetData<CMEscalationMatrixModel>(myQuery).ConfigureAwait(false);

                for (var i = 0; i < _EscalatedList.Count; i++)
                {
                    List<CMEscalationMail> RoleWithMailList = new List<CMEscalationMail>();
                    if (_EscalatedList[i].Levels > 0)
                    {
                        for(var j=0; j< dt.Rows.Count; j++)
                        {
                            int level = Convert.ToInt32(dt.Rows[j]["Id"]); 
                            if(level >= _EscalatedList[i].Levels)
                            {
                                CMEscalationMail item = new CMEscalationMail();
                                item.Level = level;
                                item.Role = Convert.ToString(dt.Rows[j]["Role"]);
                                item.MailTo = Convert.ToString(dt.Rows[j]["MailTo"]);
                                RoleWithMailList.Add(item);
                            }

                        }

                    }
                    _EscalatedList[i].EscalationMail = RoleWithMailList;
                    CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_EscalatedList[i].Status_ID);
                    string _shortStatus = getShortStatus(CMMS.CMMS_Modules.Escalation_Matrix, _Status);
                    _EscalatedList[i].Status = _shortStatus;
                }

                if (_EscalatedList.Count != 0)
                {
                    CMMSNotification.sendNotification(CMMS.CMMS_Modules.Escalation_Matrix, CMMS.CMMS_Status.JC_OPENED, _EscalatedList[1]);
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
            
            
            return 1;
        }
        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue = "";

            return retValue;

        }
    }
}
