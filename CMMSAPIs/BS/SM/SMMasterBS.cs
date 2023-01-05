using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.SM;

namespace CMMSAPIs.BS.SM
{
    public interface ISMMasterBS
    {
        Task<List<SMMasterModel>> GetAssetTypeList();
        Task<List<SMMasterModel>> AddAssetType();
        Task<List<SMMasterModel>> UpdateAssetType();
        Task<List<SMMasterModel>> DeleteAssetType();
        Task<List<SMMasterModel>> GetAssetCategoryList();
        Task<List<SMMasterModel>> AddAssetCategory();
        Task<List<SMMasterModel>> UpdateAssetCategory();
        Task<List<SMMasterModel>> DeleteAssetCategory();
        Task<List<SMMasterModel>> GetUnitMeasurementList();
        Task<List<SMMasterModel>> AddUnitMeasurement();
        Task<List<SMMasterModel>> UpdateUnitMeasurement();
        Task<List<SMMasterModel>> DeleteUnitMeasurement();
        Task<List<AssetsMaster>> GetAssetMasterList();        
        Task<int> AddAssetMaster(SMAssetMaster request);
        Task<int> UpdateAssetMaster(SMAssetMaster request);
        Task<int> DeleteAssetMaster(int id);

    }

    public class SMMasterBS : ISMMasterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public SMMasterBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }


        public async Task<List<SMMasterModel>> GetAssetTypeList()
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

        public async Task<List<SMMasterModel>> AddAssetType()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> UpdateAssetType()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> DeleteAssetType()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> GetAssetCategoryList()
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

        public async Task<List<SMMasterModel>> AddAssetCategory()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetCategory();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> UpdateAssetCategory()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetCategory();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> DeleteAssetCategory()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetCategory();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> GetUnitMeasurementList()
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

        public async Task<List<SMMasterModel>> AddUnitMeasurement()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddUnitMeasurement();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> UpdateUnitMeasurement()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateUnitMeasurement();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SMMasterModel>> DeleteUnitMeasurement()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteUnitMeasurement();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<AssetsMaster>> GetAssetMasterList()
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

        
        public async Task<int> AddAssetMaster(SMAssetMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetMaster(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> UpdateAssetMaster(SMAssetMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetMaster(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> DeleteAssetMaster(int id)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetMaster(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
