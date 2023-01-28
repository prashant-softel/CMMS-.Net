using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Masters
{
    public interface ICheckListMapBS
    {
        Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type);
        Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request);
        Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request);
    }
    public class CheckListMapBS : ICheckListMapBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public CheckListMapBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type)
        {
            try
            {
                using (var repos = new CheckListMapRepository(getDB))
                {
                    return await repos.GetCheckListMap(facility_id, type);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request)
        {
            try
            {
                using (var repos = new CheckListMapRepository(getDB))
                {
                    return await repos.CreateCheckListMap(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request)
        {
            try
            {
                using (var repos = new CheckListMapRepository(getDB))
                {
                    return await repos.UpdateCheckListMap(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
