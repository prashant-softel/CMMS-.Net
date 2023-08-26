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
        Task<CMDefaultResponse> CreateMRS(CMMRS request, int UserID);        
        Task<CMDefaultResponse> updateMRS(CMMRS request, int UserID);        
        Task<List<CMMRSList>> getMRSList(int facility_ID, int emp_id, DateTime toDate, DateTime fromDate, int status);
        Task<List<CMMRSListByModule>> CMMRSListByModule(int jobId, int pmId);
        Task<List<CMMRSItems>> getMRSItems(int ID);        
        Task<List<CMMRSItemsBeforeIssue>> getMRSItemsBeforeIssue(int ID);        
        Task<List<CMMRSItemsBeforeIssue>> getMRSItemsWithCode(int ID);        
        Task<CMMRSList> getMRSDetails(int ID);        
        Task<CMMRSList> getReturnDataByID(int ID);        
        Task<CMMRSAssetTypeList> getAssetTypeByItemID(int ItemID);
        Task<CMDefaultResponse> ReturnMRS(CMMRS request, int UserID);        
        Task<CMDefaultResponse> mrsApproval(CMApproval request, int userId);        
        Task<CMDefaultResponse> mrsReject(CMApproval request, int userId);        
        Task<CMDefaultResponse> ApproveMRSReturn(CMApproval request, int UserID);
        Task<CMDefaultResponse> RejectMRSReturn(CMApproval request, int UserID);
        void UpdateAssetStatus(int assetItemID, int status);
        Task<CMMRS> getLastTemplateData(int ID);
        Task<List<CMAssetItem>> GetAssetItems(int facility_ID, bool isGroupByCode = false);
        Task<CMDefaultResponse> CreateMRSIssue(CMMRS request, int UserID);
        Task<CMDefaultResponse> ApproveMRSIssue(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectMRSIssue(CMApproval request, int userId);
        Task<List<CMMRSList>> GetMRSReturnList(int facility_ID, int emp_id);
    }
    public class MRSBS : IMRSBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public MRSBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<CMDefaultResponse> CreateMRS(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.CreateMRS(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> updateMRS(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.updateMRS(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSList>> getMRSList(int facility_ID, int emp_id, DateTime toDate, DateTime fromDate, int status)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSList(facility_ID, emp_id, toDate, fromDate, status);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public  async Task<List<CMMRSListByModule>> CMMRSListByModule(int jobId, int pmId)
        { 
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.CMMRSListByModule( jobId, pmId);
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

        public async Task<CMMRSList> getMRSDetails(int ID)
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
        public async Task<CMMRSList> getReturnDataByID(int ID)
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
        public async Task<CMMRSAssetTypeList> getAssetTypeByItemID(int ItemID)
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

        public async Task<CMDefaultResponse> ReturnMRS(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.ReturnMRS(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<CMDefaultResponse> mrsApproval(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsApproval(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> mrsReject(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsReject(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveMRSReturn(CMApproval request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.ApproveMRSReturn(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectMRSReturn(CMApproval request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.RejectMRSReturn(request, UserID);
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

        public async Task<List<CMAssetItem>> GetAssetItems(int facility_ID, bool isGroupByCode = false)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.GetAssetItems(facility_ID, isGroupByCode);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateMRSIssue(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.CreateMRSIssue(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveMRSIssue(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.ApproveMRSIssue(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectMRSIssue(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.RejectMRSIssue(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMRSList>> GetMRSReturnList(int facility_ID, int emp_id)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.GetMRSReturnList(facility_ID, emp_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
