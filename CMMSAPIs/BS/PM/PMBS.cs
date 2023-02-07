using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.PM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.PM
{
    public interface IPMBS
    {
        Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id);
        Task<CMDefaultResponse> SetScheduleData(List<CMSetScheduleData> request);

    }
    public class PMBS : IPMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public PMBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id)
        {
            try
            {
                using (var repos = new PMRepository(getDB))
                {
                    return await repos.GetScheduleData(facility_id, category_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetScheduleData(List<CMSetScheduleData> request)
        {
            try
            {
                using (var repos = new PMRepository(getDB))
                {
                    return await repos.SetScheduleData(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
