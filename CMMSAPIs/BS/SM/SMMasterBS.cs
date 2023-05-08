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
        Task<List<CMSMMaster>> GetAssetTypeList(int ID);
        Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID);
        Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID);
        Task<CMDefaultResponse> DeleteAssetType(int Id, int userID);

        Task<List<ItemCategory>> GetAssetCategoryList(int ID);
        Task<CMDefaultResponse> AddAssetCategory(ItemCategory request, int userID);
        Task<CMDefaultResponse> UpdateAssetCategory(ItemCategory request, int userID);
        Task<CMDefaultResponse> DeleteAssetCategory(int acID, int userID);

        Task<List<UnitMeasurement>> GetUnitMeasurementList(int ID);
        Task<CMDefaultResponse> AddUnitMeasurement(UnitMeasurement request, int userID);
        Task<CMDefaultResponse> UpdateUnitMeasurement(UnitMeasurement request, int userID);
        Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID);

        Task<List<CMSMMaster>> GetAssetMasterList(int ID);
        Task<CMDefaultResponse> AddAssetMaster(CMSMMaster request, AssetMasterFiles fileData, int UserID);
        Task<CMDefaultResponse> UpdateAssetMaster(CMSMMaster request, AssetMasterFiles fileData, int UserID);
        Task<CMDefaultResponse> DeleteAssetMaster(CMSMMaster request, int UserID);

    }

    public class SMMasterBS : ISMMasterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public SMMasterBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }


        public async Task<List<CMSMMaster>> GetAssetTypeList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetTypeList(ID);
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

        public async Task<List<ItemCategory>> GetAssetCategoryList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetCategoryList(ID);
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

        public async Task<List<UnitMeasurement>> GetUnitMeasurementList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetUnitMeasurementList(ID);
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

        public async Task<List<CMSMMaster>> GetAssetMasterList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetMasterList(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddAssetMaster(CMSMMaster request, AssetMasterFiles fileData, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetMaster(request, fileData, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetMaster(CMSMMaster request, AssetMasterFiles fileData,int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetMaster(request, fileData, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetMaster(CMSMMaster request, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetMaster(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
