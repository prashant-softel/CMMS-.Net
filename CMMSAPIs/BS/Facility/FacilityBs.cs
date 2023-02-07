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
        Task<List<CMFacilityList>> GetFacilityList(int facility_id);
        Task<List<CMFacilityList>> GetFacilityList();
        Task<List<CMFacilityDetails>> GetFacilityDetails(int id);
        Task<int> CreateNewFacility(CMCreateFacility request);
        Task<int> UpdateFacility(CMUpdateFacility request);
        Task<CMDefaultResponse> DeleteFacility(int facility_id);
    }

    public class FacilityBs : IFacilityBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public FacilityBs(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMFacilityList>> GetFacilityList(int facility_id)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.GetFacilityList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMFacilityList>> GetFacilityList()
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.GetFacilityList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMFacilityDetails>> GetFacilityDetails(int id)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.GetFacilityDetails(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> CreateNewFacility(CMCreateFacility request)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.CreateNewFacility(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdateFacility(CMUpdateFacility request)
        {
            try
            {
                using (var repos = new FacilityRepository(getDB))
                {
                    return await repos.UpdateFacility(request);
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
