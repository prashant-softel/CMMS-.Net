using CMMSAPIs.Helper;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.EM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.EM
{
    public interface IEMBS
    {
        Task<CMDefaultResponse> SetEscalationMatrix(List<CMSetMasterEM> request, int userID);
        Task<List<CMSetMasterEM>> GetEscalationMatrix(CMMS.CMMS_Modules module);
        Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules module, int id); // change to run escalation
        Task<List<CMEscalationLog>> ShowEscalationLog(CMMS.CMMS_Modules module, int module_ref_id);
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
        public async Task<List<CMSetMasterEM>> GetEscalationMatrix(CMMS.CMMS_Modules module)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.GetEscalationMatrix(module);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules module, int id)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.Escalate(module, id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMEscalationLog>> ShowEscalationLog(CMMS.CMMS_Modules module, int module_ref_id)
        {
            try
            {
                using (var repos = new EMRepository(getDB))
                {
                    return await repos.ShowEscalationLog(module, module_ref_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
        
