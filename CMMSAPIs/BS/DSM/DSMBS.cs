using CMMSAPIs.Helper;
using CMMSAPIs.Models.DSM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.DSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.DSM
{
    public interface IDSMBS
    {
        Task<List<CMDSMData>> getDSMData(CMDSMFilter request);
       
    }
    public class DSMBS : IDSMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public DSMBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMDSMData>> getDSMData(CMDSMFilter request)
        {
            try
            {
                using (var repos = new DSMRepository(getDB))
                {
                    return await repos.getDSMData(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //    public async Task<CMEscalationResponse> Escalate(CMMS.CMMS_Modules module, int id)
        //    {
        //        try
        //        {
        //            using (var repos = new DSMRepository(getDB))
        //            {
        //                return await repos.Escalate(module, id);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //    public async Task<List<CMEscalationLog>> ShowEscalationLog(CMMS.CMMS_Modules module, int module_ref_id)
        //    {
        //        try
        //        {
        //            using (var repos = new DSMRepository(getDB))
        //            {
        //                return await repos.ShowEscalationLog(module, module_ref_id);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}
        }
    }
        
