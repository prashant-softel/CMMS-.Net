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

namespace CMMSAPIs.Repositories.EscalationMatrix
{
    public class EscalationMatrixRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public EscalationMatrixRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<CMDefaultResponse> InsertEscalationMatrixData(EscalationMatrixModel request, int userID)
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
            for (var i=0;i< request.EscalationLevelList.Count; i++)
            {
               
                    
                        string InsertQuery = $"INSERT INTO escalationlevelmapping" +
                       $"(Escalation_id,NoOfDays,Levels)" +
                       $"VALUES" +
                        $"('{id}'," +
                        $"{request.EscalationLevelList[i].NoOfDays}, '{request.EscalationLevelList[i].Level}'" +
                        $"); select 1 Dummay;";
                        DataTable dt = await Context.FetchData(InsertQuery).ConfigureAwait(false);
                 
            }


            //await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Data saved successfully.");
            return response;
        }

        internal async Task<int> getEscalationLevel()
        {
            string myQuery = "select * from (\r\nSELECT  DATEDIFF(NOW(), EM.CreatedAt) AS DayDifference, EM.Module, em.Status, EL.NoOfDays,el.Levels\r\nFROM escalationmatrix EM \r\nINNER JOIN escalationlevel EL ON EM.Module_Id = EL.Module_Id\r\nWHERE EM.isActive = 1) a\r\nwhere a.DayDifference = a.NoOfDays";
            try
            {
                List<EscalationMatrixModel> _EscalatedList = await Context.GetData<EscalationMatrixModel>(myQuery).ConfigureAwait(false);
                if (_EscalatedList.Count != 0)
                {
                    CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_OPENED, _EscalatedList[1]);
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
            
            
            return 1;
        }
    }
}
