using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.BS.SM
{
    public interface ISMMasterBS
    {
        Task<List<CMSMMaster>> GetAssetTypeList();
        Task<CMDefaultResponse> AddAssetType(CMSMMaster request);
        Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request);
        Task<CMDefaultResponse> DeleteAssetType(int id);

        Task<List<CMSMMaster>> GetAssetCategoryList();
        Task<CMDefaultResponse> AddAssetCategory(CMSMMaster request);
        Task<CMDefaultResponse> UpdateAssetCategory(CMSMMaster request);
        Task<CMDefaultResponse> DeleteAssetCategory(int id);

        Task<List<CMSMUnitMaster>> GetUnitMeasurementList();
        Task<CMDefaultResponse> AddUnitMeasurement(CMSMUnitMaster request);
        Task<CMDefaultResponse> UpdateUnitMeasurement(CMSMUnitMaster request);
        Task<CMDefaultResponse> DeleteUnitMeasurement(int id);

        Task<List<CMSMAssetMaster>> GetAssetMasterList();
        Task<CMDefaultResponse> AddAssetMaster(CMSMAssetMaster request);
        Task<CMDefaultResponse> UpdateAssetMaster(CMSMAssetMaster request);
        Task<CMDefaultResponse> DeleteAssetMaster(int id);

        /* Stock List */
        Task<List<CMStock>> GetStockList(int facility_id);
        Task<CMDefaultResponse> UpdateStockList(List<CMStock> request);
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

        public async Task<CMDefaultResponse> AddAssetType(CMSMMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetType(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetType(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetType(int id)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetType(id);
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

        public async Task<CMDefaultResponse> AddAssetCategory(CMSMMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetCategory(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetCategory(CMSMMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetCategory(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetCategory(int id)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetCategory(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSMUnitMaster>> GetUnitMeasurementList()
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

        public async Task<CMDefaultResponse> AddUnitMeasurement(CMSMUnitMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddUnitMeasurement(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateUnitMeasurement(CMSMUnitMaster request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateUnitMeasurement(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteUnitMeasurement(int id)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteUnitMeasurement(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSMAssetMaster>> GetAssetMasterList()
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

        public async Task<CMDefaultResponse> AddAssetMaster(CMSMAssetMaster request)
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

        public async Task<CMDefaultResponse> UpdateAssetMaster(CMSMAssetMaster request)
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

        public async Task<CMDefaultResponse> DeleteAssetMaster(int id)
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

        /* Stock List */

        public async Task<List<CMStock>> GetStockList(int facility_id)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetStockList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateStockList(List<CMStock> request)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateStockList(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
