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
    public interface IMRSBS
    {
        Task<CMDefaultResponse> CreateMRS(CMMRS request);        
        Task<List<CMMRSList>> getMRSList(int plant_ID, int emp_id, DateTime toDate, DateTime fromDate, int status);        
        Task<List<CMMRSItems>> getMRSItems(int ID);        
        Task<List<CMMRSItemsBeforeIssue>> getMRSItemsBeforeIssue(int ID);        
        Task<List<CMMRSItemsBeforeIssue>> getMRSItemsWithCode(int ID);        
        Task<List<CMMRS>> getMRSDetails(int ID);        
        Task<List<CMRETURNMRSDATA>> getReturnDataByID(int ID);        
        Task<List<CMMRS>> getAssetTypeByItemID(int ItemID);
        Task<CMDefaultResponse> mrsReturn(CMMRS request);        
        Task<CMDefaultResponse> mrsApproval(CMMRS request);        
        Task<CMDefaultResponse> mrsReturnApproval(CMMRS request);
        void UpdateAssetStatus(int assetItemID, int status);
        Task<CMMRS> getLastTemplateData(int ID);
        Task<List<CMAssetItem>> GetAssetItems(int plantID, bool isGroupByCode = false);
    }
    public class MRSBS : IMRSBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public MRSBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<CMDefaultResponse> CreateMRS(CMMRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.CreateMRS(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSList>> getMRSList(int plant_ID, int emp_id, DateTime toDate, DateTime fromDate, int status)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSList(plant_ID, emp_id, toDate, fromDate, status);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<CMMRSItems>> getMRSItems(int ID)
           {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItems(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<CMMRSItemsBeforeIssue>> getMRSItemsBeforeIssue(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItemsBeforeIssue(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSItemsBeforeIssue>> getMRSItemsWithCode(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItemsWithCode(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRS>> getMRSDetails(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSDetails(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMRETURNMRSDATA>> getReturnDataByID(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getReturnDataByID(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMRS>> getAssetTypeByItemID(int ItemID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getAssetTypeByItemID(ItemID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> mrsReturn(CMMRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsReturn(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<CMDefaultResponse> mrsApproval(CMMRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsApproval(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<CMDefaultResponse> mrsReturnApproval(CMMRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsReturnApproval(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void UpdateAssetStatus(int assetItemID, int status)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                     repos.UpdateAssetStatus(assetItemID, status);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMMRS> getLastTemplateData(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                   return await repos.getLastTemplateData(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMAssetItem>> GetAssetItems(int plantID, bool isGroupByCode = false)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.GetAssetItems(plantID, isGroupByCode);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }

}
