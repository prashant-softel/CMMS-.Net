using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Facility;
using CMMSAPIs.Models.Facility;

namespace CMMSAPIs.BS.Facility
{
    public interface IFacilityBS
    {
        Task<List<CMFacilityList>> GetFacilityList(int userID);
        Task<List<CMFacilityList>> GetBlockList(int parent_id);
        Task<CMFacilityDetails> GetFacilityDetails(int id);
        Task<CMDefaultResponse> CreateNewFacility(CMCreateFacility request, int userID);
        Task<CMDefaultResponse> UpdateFacility(CMCreateFacility request, int userID);
        Task<CMDefaultResponse> DeleteFacility(int facility_id);
        Task<CMDefaultResponse> CreateNewBlock(CMCreateBlock request, int userID);
        Task<CMDefaultResponse> UpdateBlock(CMCreateBlock request, int userID);
        Task<CMDefaultResponse> DeleteBlock(int block_id);
    }

    public class FacilityBs : IFacilityBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public FacilityBs(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMFacilityList>> GetFacilityList(int userID)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.GetFacilityList(userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CMFacilityList>> GetBlockList(int parent_id)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.GetBlockList(parent_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMFacilityDetails> GetFacilityDetails(int id)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.GetFacilityDetails(id);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateNewFacility(CMCreateFacility request, int userID)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.CreateNewFacility(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateFacility(CMCreateFacility request, int userID)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.UpdateFacility(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteFacility(int facility_id)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.DeleteFacility(facility_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateNewBlock(CMCreateBlock request, int userID)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.CreateNewBlock(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateBlock(CMCreateBlock request, int userID)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.UpdateBlock(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteBlock(int block_id)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.DeleteBlock(block_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
