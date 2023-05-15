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
    public interface IReOrderBS
    {
        Task<List<ReOrder>> GetReorderDataByID(int assetID, int plantID);
        Task<List<ReOrder>> getReorderAssetsData(int plantID);
        Task<List<ReOrderItems>> getReorderItems(int plantID);
        Task<CMDefaultResponse> submitReorderForm(ReOrder request);
        Task<CMDefaultResponse> updateReorderData(ReOrder request);
        Task<CMDefaultResponse> reorderAssets(ReOrder request);
    }
        public class ReOrderBS: IReOrderBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public ReOrderBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<ReOrder>> GetReorderDataByID(int assetID, int plantID)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.GetReorderDataByID(assetID, plantID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> submitReorderForm(ReOrder request)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.submitReorderForm(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<CMDefaultResponse> updateReorderData(ReOrder request)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.updateReorderData(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ReOrder>> getReorderAssetsData(int plantID)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.getReorderAssetsData(plantID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ReOrderItems>> getReorderItems(int plantID)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.getReorderItems(plantID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> reorderAssets(ReOrder request)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.reorderAssets(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
