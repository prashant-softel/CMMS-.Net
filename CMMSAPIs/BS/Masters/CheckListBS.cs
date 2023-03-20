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
        Task<CMDefaultResponse> CreateChecklist(List<CMCreateCheckList> request, int userID);      
        Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request, int userID);
        Task<CMDefaultResponse> DeleteChecklist(int id);
        Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type);
        Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request);      
        Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request);
        Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id);
        Task<CMDefaultResponse> CreateCheckPoint(List<CMCreateCheckPoint> request, int userID);
        Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request, int userID);
        Task<CMDefaultResponse> DeleteCheckPoint(int id);
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateChecklist(List<CMCreateCheckList> request, int userID)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.CreateChecklist(request, userID);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request, int userID)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.UpdateCheckList(request, userID);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteChecklist(int id)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.DeleteChecklist(id);

                }
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateCheckPoint(List<CMCreateCheckPoint> request, int userID)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.CreateCheckPoint(request, userID);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request, int userID)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.UpdateCheckPoint(request, userID);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteCheckPoint(int id)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.DeleteCheckPoint(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
