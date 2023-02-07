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
        Task<List<CMViewInventory>> ViewInventory(int id);
        Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request);
        Task<CMDefaultResponse> UpdateInventory(CMAddInventory request);
        Task<CMDefaultResponse> DeleteInventory(int id);
        Task<List<CMInventoryTypeList>> GetInventoryTypeList();
        Task<List<CMInventoryStatusList>> GetInventoryStatusList();


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

        public async Task<List<CMViewInventory>> ViewInventory(int id)
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

        public async Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request)
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

        public async Task<List<CMInventoryTypeList>> GetInventoryTypeList()
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

        public async Task<List<CMInventoryStatusList>> GetInventoryStatusList()
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
