using CMMSAPIs.Helper;
using CMMSAPIs.Repositories.Inventory;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CMMSAPIs.BS.Inventory
{
    public interface IInventoryBS
    {
        Task<List<CMInventoryList>> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds);
        Task<List<CMViewInventory>> GetInventoryDetails(int id);
        Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request, int userID);
        Task<List<CMDefaultResponse>> ImportInventories(int file_id, int userID);
        Task<CMDefaultResponse> UpdateInventory(CMAddInventory request, int userID);
        Task<CMDefaultResponse> DeleteInventory(int id, int userID);
        Task<List<CMInventoryTypeList>> GetInventoryTypeList();
        Task<List<CMInventoryStatusList>> GetInventoryStatusList();
        Task<List<CMInventoryCategoryList>> GetInventoryCategoryList();
        Task<List<CMDefaultList>> GetWarrantyTypeList();
        Task<List<CMDefaultList>> GetWarrantyUsageTermList();
        Task<CMDefaultResponse> SetParentAsset(CMSetParentAsset parent_child_group, int userID);
    }
    public class InventoryBS : IInventoryBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;
        public InventoryBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

        public async Task<List<CMInventoryList>> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryList(facilityId, linkedToBlockId, status, categoryIds);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMViewInventory>> GetInventoryDetails(int id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryDetails(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.AddInventory(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateInventory(CMAddInventory request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.UpdateInventory(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteInventory(int id, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.DeleteInventory(id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> ImportInventories(int file_id, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.ImportInventories(file_id, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetParentAsset(CMSetParentAsset parent_child_group, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.SetParentAsset(parent_child_group, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CMInventoryTypeList>> GetInventoryTypeList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMInventoryCategoryList>> GetInventoryCategoryList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryCategoryList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMInventoryStatusList>> GetInventoryStatusList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryStatusList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultList>> GetWarrantyTypeList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetWarrantyTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultList>> GetWarrantyUsageTermList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetWarrantyUsageTermList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
