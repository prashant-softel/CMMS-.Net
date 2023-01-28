using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Masters
{
    public interface ICheckPointBS
    {
        Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id);
        Task<CMDefaultResponse> CreateCheckPoint(CMCreateCheckPoint request);
        Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request);
        Task<CMDefaultResponse> DeleteCheckPoint(int id);

    }
    public class CheckPointBS : ICheckPointBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public CheckPointBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id)
        {
            try
            {
                using (var repos = new CheckPointRepository(getDB))
                {
                    return await repos.GetCheckPointList(checklist_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateCheckPoint(CMCreateCheckPoint request)
        {
            try
            {
                using (var repos = new CheckPointRepository(getDB))
                {
                    return await repos.CreateCheckPoint(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request)
        {
            try
            {
                using (var repos = new CheckPointRepository(getDB))
                {
                    return await repos.UpdateCheckPoint(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteCheckPoint(int id)
        {
            try
            {
                using (var repos = new CheckPointRepository(getDB))
                {
                    return await repos.DeleteCheckPoint(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
