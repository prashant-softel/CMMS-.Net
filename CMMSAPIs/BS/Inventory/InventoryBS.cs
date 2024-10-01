using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Inventory;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Inventory
{
    public interface IInventoryBS
    {
        Task<List<CMInventoryList>> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds, string facilitytime);
        Task<CMViewInventory> GetInventoryDetails(int id, string facilitytime);
        Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request, int userID, string facilitytimeZone);
        Task<CMImportFileResponse> ImportInventories(int file_id, int facility_id, int userID);
        Task<CMDefaultResponse> UpdateInventory(CMAddInventory request, int userID);
        Task<CMDefaultResponse> DeleteInventory(int id, int userID);
        Task<CMDefaultResponse> DeleteInventoryByFacilityId(int facilityId, int userID);
        Task<List<CMInventoryTypeList>> GetInventoryTypeList();
        Task<CMDefaultResponse> AddInventoryType(CMInventoryTypeList request, int userID);
        Task<CMDefaultResponse> UpdateInventoryType(CMInventoryTypeList request, int userID);
        Task<CMDefaultResponse> DeleteInventoryType(int id);
        Task<List<CMInventoryStatusList>> GetInventoryStatusList();
        Task<CMDefaultResponse> AddInventoryStatus(CMInventoryStatusList request, int userID);
        Task<CMDefaultResponse> UpdateInventoryStatus(CMInventoryStatusList request, int userID);
        Task<CMDefaultResponse> DeleteInventoryStatus(int id);
        Task<List<CMInventoryCategoryList>> GetInventoryCategoryList();
        Task<CMDefaultResponse> AddInventoryCategory(CMInventoryCategoryList request, int userID);
        Task<CMDefaultResponse> UpdateInventoryCategory(CMInventoryCategoryList request, int userID);
        Task<CMDefaultResponse> DeleteInventoryCategory(int id);
        Task<List<CMDefaultList>> GetWarrantyTypeList();
        Task<List<CMDefaultList>> GetWarrantyUsageTermList();
        Task<List<CMWarrantyCertificate>> GetWarrantyCertificate(string facility_id, DateTime from_date, DateTime to_date);
        Task<List<CMCalibrationAssets>> GetCalibrationList(string facilityId, string facilitytime);

        Task<CMDefaultResponse> SetParentAsset(int parentID, int childID, int userID);
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

        public async Task<List<CMInventoryList>> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds, string facilitytime)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryList(facilityId, linkedToBlockId, status, categoryIds, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMViewInventory> GetInventoryDetails(int id, string facilitytime)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetInventoryDetails(id, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.AddInventory(request, userID, facilitytimeZone);

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
                    return await repos.UpdateInventries(request, userID);

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

        public async Task<CMDefaultResponse> DeleteInventoryByFacilityId(int facilityId, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.DeleteInventoryByFacilityId(facilityId, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMImportFileResponse> ImportInventories(int file_id, int facility_id, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.ImportInventories(file_id, facility_id, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetParentAsset(int parentID, int childID, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.SetParentAsset(parentID, childID, userID);
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

        public async Task<CMDefaultResponse> AddInventoryType(CMInventoryTypeList request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.AddInventoryType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateInventoryType(CMInventoryTypeList request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.UpdateInventoryType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteInventoryType(int id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.DeleteInventoryType(id);
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

        public async Task<CMDefaultResponse> AddInventoryCategory(CMInventoryCategoryList request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.AddInventoryCategory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateInventoryCategory(CMInventoryCategoryList request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.UpdateInventoryCategory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteInventoryCategory(int id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.DeleteInventoryCategory(id);
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

        public async Task<CMDefaultResponse> AddInventoryStatus(CMInventoryStatusList request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.AddInventoryStatus(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateInventoryStatus(CMInventoryStatusList request, int userID)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.UpdateInventoryStatus(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteInventoryStatus(int id)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.DeleteInventoryStatus(id);
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
                    return await repos.GetWarrantyUsageTermList


                        ();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMWarrantyCertificate>> GetWarrantyCertificate(string facility_id, DateTime from_date, DateTime to_date)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetWarrantyCertificate(facility_id, from_date, to_date);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMCalibrationAssets>> GetCalibrationList(string facilityId, string facilitytime)
        {
            try
            {
                using (var repos = new InventoryRepository(getDB, _environment))
                {
                    return await repos.GetCalibrationList(facilityId, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
