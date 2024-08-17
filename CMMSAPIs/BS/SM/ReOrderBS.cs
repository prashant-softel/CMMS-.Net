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
        Task<List<CMReOrder>> GetReorderDataByID(int assetID, int plantID,string faciltytime);
        Task<List<CMReOrder>> getReorderAssetsData(int plantID, string faciltytime);
        Task<List<CMReOrderItems>> getReorderItems(int plantID);
        Task<CMDefaultResponse> submitReorderForm(CMReOrder request);
        Task<CMDefaultResponse> updateReorderData(CMReOrder request);
        Task<CMDefaultResponse> reorderAssets(CMReOrder request);
    }
        public class ReOrderBS: IReOrderBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public ReOrderBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMReOrder>> GetReorderDataByID(int assetID, int plantID,string facilitytime)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.GetReorderDataByID(assetID, plantID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> submitReorderForm(CMReOrder request)
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
        
        public async Task<CMDefaultResponse> updateReorderData(CMReOrder request)
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

        public async Task<List<CMReOrder>> getReorderAssetsData(int plantID, string faciltytime)
        {
            try
            {
                using (var repos = new ReOrderRepository(getDB))
                {
                    return await repos.getReorderAssetsData(plantID,  faciltytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMReOrderItems>> getReorderItems(int plantID)
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
        public async Task<CMDefaultResponse> reorderAssets(CMReOrder request)
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
