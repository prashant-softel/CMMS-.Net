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
        Task<List<CMSMMaster>> GetAssetTypeList();
        Task<List<CMSMMaster>> AddAssetType();
        Task<List<CMSMMaster>> UpdateAssetType();
        Task<List<CMSMMaster>> DeleteAssetType();

        Task<List<CMSMMaster>> GetAssetCategoryList();
        Task<List<CMSMMaster>> AddAssetCategory();
        Task<List<CMSMMaster>> UpdateAssetCategory();
        Task<List<CMSMMaster>> DeleteAssetCategory();

        Task<List<CMSMMaster>> GetUnitMeasurementList();
        Task<List<CMSMMaster>> AddUnitMeasurement();
        Task<List<CMSMMaster>> UpdateUnitMeasurement();
        Task<List<CMSMMaster>> DeleteUnitMeasurement();

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

        public async Task<List<CMSMMaster>> AddAssetType()
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

        public async Task<List<CMSMMaster>> UpdateAssetType()
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

        public async Task<List<CMSMMaster>> DeleteAssetType()
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

        public async Task<List<CMSMMaster>> GetAssetCategoryList()
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

        public async Task<List<CMSMMaster>> AddAssetCategory()
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

        public async Task<List<CMSMMaster>> UpdateAssetCategory()
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

        public async Task<List<CMSMMaster>> DeleteAssetCategory()
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

        public async Task<List<CMSMMaster>> GetUnitMeasurementList()
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

        public async Task<List<CMSMMaster>> AddUnitMeasurement()
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

        public async Task<List<CMSMMaster>> UpdateUnitMeasurement()
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

        public async Task<List<CMSMMaster>> DeleteUnitMeasurement()
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
