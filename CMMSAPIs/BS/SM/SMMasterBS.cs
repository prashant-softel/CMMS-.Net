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

        Task<List<SMAssetMaster>> GetAssetMasterList();
        /*        Task<int> AddAssetMaster(string aasetCode, string assetName, string assetType, string cat_name, string description, string unitMeasurement, string approvalRequired);*/
        Task<int> AddAssetMaster(SMAssetMaster request);
        Task<int> UpdateAssetMaster(SMAssetMaster request);
        Task<int> DeleteAssetMaster(SMAssetMaster request);

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

        public async Task<List<SMAssetMaster>> GetAssetMasterList()
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

        /*        public async Task<int> AddAssetMaster(string aasetCode, string assetName, string assetType, string cat_name, string description, string unitMeasurement, string approvalRequired)
                {
                    try
                    {
                        using (var repos = new SMMasterRepository(getDB))
                        {
                            return await repos.AddAssetMaster(aasetCode, assetName, assetType, cat_name, description, unitMeasurement, approvalRequired);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }*/
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

        public async Task<int> DeleteAssetMaster(SMAssetMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetMaster(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
