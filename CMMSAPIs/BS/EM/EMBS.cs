using CMMSAPIs.Helper;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.EM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.EM
{
    public interface IEMBS
    {
        Task<CMDefaultResponse> SetEscalationMatrix(List<CMSetMasterEM> request, int userID);

        Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules moduleId, int statusId, int userID, string facilitytimeZone); // change to run escalation
        Task<List<CMEscalationLog>> GetEscalationLog(CMMS.CMMS_Modules module, int module_ref_id, int userID, string facilitytimeZone);
        Task<List<CMSetMasterEM>> GetEscalationMatrixbystatusId(CMMS.CMMS_Modules module, int status_id);
        Task<List<GetEcMatrix>> GetEscalationMatrixList(CMMS.CMMS_Modules module);
    }
    public class EMBS : IEMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public EMBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }
        public async Task<CMDefaultResponse> SetEscalationMatrix(List<CMSetMasterEM> request, int userID)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.SetEscalationMatrix(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<GetEcMatrix>> GetEscalationMatrixList(CMMS.CMMS_Modules module)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.GetEscalationMatrixList(module);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMSetMasterEM>> GetEscalationMatrixbystatusId(CMMS.CMMS_Modules module, int status_id)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.GetEscalationMatrixbystatusId(module, status_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules moduleId, int statusId, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.Escalate_2(moduleId, statusId, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMEscalationLog>> GetEscalationLog(CMMS.CMMS_Modules module, int module_ref_id, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.GetEscalationLog(module, module_ref_id, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

