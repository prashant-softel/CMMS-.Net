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
    public interface ISMMasterBS
    {
        Task<List<CMAssetTypes>> GetAssetTypeList(int ID);
        Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID);
        Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID);
        Task<CMDefaultResponse> DeleteAssetType(int Id, int userID);

        Task<List<CMItemCategory>> GetMaterialCategoryList(int ID);
        Task<CMDefaultResponse> AddMaterialCategory(CMItemCategory request, int userID);
        Task<CMDefaultResponse> UpdateMaterialCategory(CMItemCategory request, int userID);
        Task<CMDefaultResponse> DeleteMaterialCategory(int acID, int userID);

        Task<List<CMUnitMeasurement>> GetUnitMeasurementList(int ID);
        Task<CMDefaultResponse> AddUnitMeasurement(CMUnitMeasurement request, int userID);
        Task<CMDefaultResponse> UpdateUnitMeasurement(CMUnitMeasurement request, int userID);
        Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID);

        Task<List<CMASSETMASTERLIST>> GetAssetMasterList(int ID);
        Task<CMDefaultResponse> AddAssetMaster(CMSMMaster request, CMAssetMasterFiles fileData, int UserID);
        Task<CMDefaultResponse> UpdateAssetMaster(CMSMMaster request, CMAssetMasterFiles fileData, int UserID);
        Task<CMDefaultResponse> DeleteAssetMaster(CMSMMaster request, int UserID);
        Task<List<CMGETASSETDATALIST>> GetAssetDataList(int facility_id);
        Task<List<CMVendorList>> GetVendorList();
        Task<CMAssetBySerialNo> GetAssetBySerialNo(string serial_number);
        Task<List<CMPaidBy>> GetPaidByList(int ID);
        Task<CMDefaultResponse> AddPaidBy(CMPaidBy request, int UserID);
        Task<CMDefaultResponse> UpdatePaidBy(CMPaidBy request, int UserID);
        Task<CMDefaultResponse> DeletePaidBy(CMPaidBy request, int UserID);
        Task<CMImportFileResponse> ImportMaterialFile(int file_id, int facility_id, int userID);
      
    }

    public class SMMasterBS : ISMMasterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public SMMasterBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }


        public async Task<List<CMAssetTypes>> GetAssetTypeList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetTypeList(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetType(int Id, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetType(Id, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMItemCategory>> GetMaterialCategoryList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetMaterialCategoryList(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddMaterialCategory(CMItemCategory request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddMaterialCategory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateMaterialCategory(CMItemCategory request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateMaterialCategory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteMaterialCategory(int acID, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteMaterialCategory(acID, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMUnitMeasurement>> GetUnitMeasurementList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetUnitMeasurementList(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddUnitMeasurement(CMUnitMeasurement request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddUnitMeasurement(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateUnitMeasurement(CMUnitMeasurement request, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateUnitMeasurement(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteUnitMeasurement(umID, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMASSETMASTERLIST>> GetAssetMasterList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetMasterList(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddAssetMaster(CMSMMaster request, CMAssetMasterFiles fileData, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddAssetMaster(request, fileData, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAssetMaster(CMSMMaster request, CMAssetMasterFiles fileData,int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdateAssetMaster(request, fileData, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAssetMaster(CMSMMaster request, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeleteAssetMaster(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMGETASSETDATALIST>> GetAssetDataList(int facility_id)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetDataList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMVendorList>> GetVendorList()
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetVendorList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMAssetBySerialNo> GetAssetBySerialNo(string serial_number)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetAssetBySerialNo(serial_number);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
         public async Task<List<CMPaidBy>> GetPaidByList(int ID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.GetPaidByList(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddPaidBy(CMPaidBy request, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.AddPaidBy(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdatePaidBy(CMPaidBy request, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.UpdatePaidBy(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePaidBy(CMPaidBy request, int UserID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.DeletePaidBy(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMImportFileResponse> ImportMaterialFile(int file_id, int facility_id, int userID)
        {
            try
            {
                using (var repos = new SMMasterRepository(getDB))
                {
                    return await repos.ImportMaterialFile(file_id, facility_id, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}
