using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Masters
{
    public interface ICheckListBS
    {
        Task<List<CMCheckList>> GetCheckList(int facility_id, string type);
        Task<CMDefaultResponse> CreateChecklist(CMCreateCheckList request);      
        Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request);
        Task<CMDefaultResponse> DeleteChecklist(CMCreateCheckList request);
        Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type);
        Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request);      
        Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request);
        Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id);
        Task<CMDefaultResponse> CreateCheckPoint(CMCreateCheckPoint request);
        Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request);
        Task<CMDefaultResponse> DeleteCheckPoint(CMCreateCheckPoint request);
    }
    public class CheckListBS : ICheckListBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public CheckListBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMCheckList>> GetCheckList(int facility_id, string type)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.GetCheckList(facility_id, type);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateChecklist(CMCreateCheckList request)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.CreateChecklist(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.UpdateCheckList(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteChecklist(CMCreateCheckList request)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.DeleteChecklist(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
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
                using (var repos = new CheckListRepository(getDB))
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
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.UpdateCheckListMap(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
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
                using (var repos = new CheckListRepository(getDB))
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
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.UpdateCheckPoint(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteCheckPoint(CMCreateCheckPoint request)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.DeleteCheckPoint(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
