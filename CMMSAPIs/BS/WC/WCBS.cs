using CMMSAPIs.Helper;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.WC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.WC
{
    public interface IWCBS
    {
        Task<CMWCList> GetWCList(int facility_id);
        Task<CMDefaultResponse> CreateWC(CMWCCreate request);
        Task<CMWCDetail> ViewWC(int wc_id);

    }
    public class WCBS : IWCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public WCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<CMWCList> GetWCList(int facility_id)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.GetWCList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateWC(CMWCCreate request)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.CreateWC(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMWCDetail> ViewWC(int wc_id)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ViewWC(wc_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
