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
        Task<List<SMMaster>> GetAssetTypeList();
        Task<List<SMMaster>> AddAssetType();
        Task<List<SMMaster>> UpdateAssetType();
        Task<List<SMMaster>> DeleteAssetType();

        Task<List<SMMaster>> GetAssetCategoryList();
        Task<List<SMMaster>> AddAssetCategory();
        Task<List<SMMaster>> UpdateAssetCategory();
        Task<List<SMMaster>> DeleteAssetCategory();

        Task<List<SMMaster>> GetUnitMeasurementList();
        Task<List<SMMaster>> AddUnitMeasurement();
        Task<List<SMMaster>> UpdateUnitMeasurement();
        Task<List<SMMaster>> DeleteUnitMeasurement();

        Task<List<SMMaster>> GetAssetMasterList();
        Task<List<SMMaster>> AddAssetMaster();
        Task<List<SMMaster>> UpdateAssetMaster();
        Task<List<SMMaster>> DeleteAssetMaster();

    }

    public class SMMasterBS : ISMMasterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public SMMasterBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }


        public async Task<List<SMMaster>> GetAssetTypeList()
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

        public async Task<List<SMMaster>> AddAssetType()
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

        public async Task<List<SMMaster>> UpdateAssetType()
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

        public async Task<List<SMMaster>> DeleteAssetType()
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

        public async Task<List<SMMaster>> GetAssetCategoryList()
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

        public async Task<List<SMMaster>> AddAssetCategory()
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

        public async Task<List<SMMaster>> UpdateAssetCategory()
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

        public async Task<List<SMMaster>> DeleteAssetCategory()
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

        public async Task<List<SMMaster>> GetUnitMeasurementList()
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

        public async Task<List<SMMaster>> AddUnitMeasurement()
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

        public async Task<List<SMMaster>> UpdateUnitMeasurement()
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

        public async Task<List<SMMaster>> DeleteUnitMeasurement()
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

        public async Task<List<SMMaster>> GetAssetMasterList()
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

        public async Task<List<SMMaster>> AddAssetMaster()
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

        public async Task<List<SMMaster>> UpdateAssetMaster()
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

        public async Task<List<SMMaster>> DeleteAssetMaster()
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
