using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Repositories.EM
{
    public class EMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public EMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        public async Task<CMDefaultResponse> SetEscalationMatrix(List<CMSetMasterEM> request, int userID)
        {
            /*
             * Setting escalation matrix for each module and its every status
             * Master method where matrix is stored in EscalationMatrix table
             */
            List<int> idList = new List<int>();
            foreach(CMSetMasterEM emModule in request)
            {
                foreach(CMMasterEM emStatus in emModule.status_escalation)
                {
                    string deleteEscalation = $"DELETE FROM escalationmatrix WHERE moduleId = {emModule.module_id} AND statusId = {emStatus.status_id};";
                    await Context.ExecuteNonQry<int>(deleteEscalation).ConfigureAwait(false);
                    foreach (CMEscalation escalation in emStatus.escalation)
                    {
                        string addEscalation = "INSERT INTO escalationmatrix (moduleId, statusId, roleId, days, createdBy, createdAt) " +
                                                $"VALUES ({emModule.module_id}, {emStatus.status_id}, {escalation.role_id}, " +
                                                $"{escalation.days}, {userID}, '{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID(); ";
                        DataTable dt = await Context.FetchData(addEscalation).ConfigureAwait(false);
                        int id = Convert.ToInt32(dt.Rows[0][0]);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.ESCALATION_MATRIX, id, 0, 0, "Escalation Details Added", CMMS.CMMS_Status.CREATED, userID);
                        idList.Add(id);
                    }
                }
            }
            return new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, "Escalation Details Added");
        }
        public async Task<CMSetMasterEM> GetEscalationMatrix(CMMS.CMMS_Modules module)
        {
            /*
             * Gets the escalation matrix from EscalationMatrix table
             */
            if (module == 0)
                throw new ArgumentException("Module number is required");
            CMSetMasterEM matrix = new CMSetMasterEM()
            {
                module_id = (int)module,
                module_name = $"{module}"
            };
            string statusCase = "CASE ";
            foreach(CMMS.CMMS_Status status in Enum.GetValues(typeof(CMMS.CMMS_Status)))
            {
                statusCase += $"WHEN statusId = {(int)status} THEN '{status}' ";
            }
            statusCase += "ELSE 'Invalid' END";
            string getStatusList = $"SELECT statusId as status_id, {statusCase} as status_name FROM escalationmatrix WHERE moduleId = {matrix.module_id} GROUP BY statusId;";
            List<CMMasterEM> statusList = await Context.GetData<CMMasterEM>(getStatusList).ConfigureAwait(false);
            foreach(CMMasterEM status in statusList)
            {
                string escalationQry = $"SELECT days, role.id as role_id, role.name as role_name FROM escalationmatrix " +
                                        $"LEFT JOIN userroles as role ON role.id = escalationmatrix.roleId " +
                                        $"WHERE statusId = {status.status_id};";
                List<CMEscalation> escalation = await Context.GetData<CMEscalation>(escalationQry).ConfigureAwait(false);
                status.escalation = escalation;
            }
            matrix.status_escalation = statusList;
            return matrix;
        }
        public async Task<CMDefaultResponse> Escalate(CMMS.CMMS_Modules module, int module_ref_id)
        {
            /*
             * Checks the current UTC time with the time since status was last updated
             * Performs escalation process as per escalation matrix
             * Will put escalation details in escalation table
             * Also sends notification to all users under the plant
             * 
             */

            return null;
        }
        public async Task<CMEscalationLog> ShowEscalationLog(CMMS.CMMS_Modules module, int module_ref_id)
        {
            /*
             * Returns the escalation log for the reference ID of module
             */
            return null;
        }
    }
}
