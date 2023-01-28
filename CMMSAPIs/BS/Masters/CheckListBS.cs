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
        Task<List<CMCheckList>> GetCheckList(int facility_id, int type);
        Task<CMDefaultResponse> CreateChecklist(CMCreateCheckList request);
        Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request);
        Task<CMDefaultResponse> DeleteChecklist(int id);

    }
    public class CheckListBS : ICheckListBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public CheckListBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMCheckList>> GetCheckList(int facility_id, int type)
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

        public async Task<CMDefaultResponse> DeleteChecklist(int id)
        {
            try
            {
                using (var repos = new CheckListRepository(getDB))
                {
                    return await repos.DeleteChecklist(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
