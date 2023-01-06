using CMMSAPIs.Helper;
using CMMSAPIs.Repositories.Inventory;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Inventory
{
    public interface IInventoryBS
    {
        Task<List<CMInventoryList>> GetInventoryList(int facility_id);
        Task<CMViewInventory> ViewInventory(int id);
        Task<CMDefaultResponse> AddInventory(CMAddInventory request);
        Task<CMDefaultResponse> UpdateInventory(CMAddInventory request);
        Task<CMDefaultResponse> DeleteInventory(int id);
        Task<List<KeyValuePairs>> GetInventoryTypeList();
        Task<List<KeyValuePairs>> GetInventoryStatusList();


    }
    public class InventoryBS : IInventoryBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public InventoryBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMInventoryList>> GetInventoryList(int facility_id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.GetInventoryList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMViewInventory> ViewInventory(int id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.ViewInventory(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddInventory(CMAddInventory request)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.AddInventory(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateInventory(CMAddInventory request)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.UpdateInventory(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteInventory(int id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.DeleteInventory(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<KeyValuePairs>> GetInventoryTypeList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.GetInventoryTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<KeyValuePairs>> GetInventoryStatusList()
        {
            try
            {
                using (var repos = new InventoryRepository(getDB))
                {
                    return await repos.GetInventoryStatusList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
