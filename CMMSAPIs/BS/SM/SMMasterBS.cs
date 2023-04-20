using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;

namespace CMMSAPIs.BS.SM
{
    public interface ISMMasterBS
    {
        Task<List<CMSMMaster>> GetAssetTypeList();
        Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID);
        Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID);
        Task<CMDefaultResponse> DeleteAssetType(int Id, int userID);

        Task<List<ItemCategory>> GetAssetCategoryList();
        Task<CMDefaultResponse> AddAssetCategory(ItemCategory request, int userID);
        Task<CMDefaultResponse> UpdateAssetCategory(ItemCategory request, int userID);
        Task<CMDefaultResponse> DeleteAssetCategory(int acID, int userID);

        Task<List<UnitMeasurement>> GetUnitMeasurementList();
        Task<CMDefaultResponse> AddUnitMeasurement(UnitMeasurement request, int userID);
        Task<CMDefaultResponse> UpdateUnitMeasurement(UnitMeasurement request, int userID);
        Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID);

        Task<List<CMSMMaster>> GetAssetMasterList();
        Task<List<CMSMMaster>> AddAssetMaster();
        Task<List<CMSMMaster>> UpdateAssetMaster();
        Task<List<CMSMMaster>> DeleteAssetMaster();

    }

    public class SMMasterBS : ISMMasterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public SMMasterBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }


        public async Task<List<CMSMMaster>> GetAssetTypeList()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetTypeList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetType(int Id, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetType(Id, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ItemCategory>> GetAssetCategoryList()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetCategoryList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddAssetCategory(ItemCategory request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetCategory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetCategory(ItemCategory request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetCategory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetCategory(int acID, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetCategory(acID, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UnitMeasurement>> GetUnitMeasurementList()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetUnitMeasurementList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddUnitMeasurement(UnitMeasurement request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddUnitMeasurement(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateUnitMeasurement(UnitMeasurement request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateUnitMeasurement(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteUnitMeasurement(umID, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSMMaster>> GetAssetMasterList()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetMasterList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSMMaster>> AddAssetMaster()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetMaster();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSMMaster>> UpdateAssetMaster()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetMaster();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSMMaster>> DeleteAssetMaster()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetMaster();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
