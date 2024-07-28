using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.SM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.SM
{
    public interface IMRSBS
    {
        Task<CMDefaultResponse> CreateMRS(CMMRS request, int UserID);
        Task<CMDefaultResponse> updateMRS(CMMRS request, int UserID);
        Task<List<CMMRSList>> getMRSList(int facility_ID, int emp_id, DateTime toDate, DateTime fromDate, int status, string facilitytime);
        Task<List<CMMRSListByModule>> getMRSListByModule(int jobId, int pmId, string facilitytime);
        Task<List<CMMRSItems>> getMRSItems(int ID, string facilitytime);
        Task<List<CMMRSItemsBeforeIssue>> getMRSItemsBeforeIssue(int ID, string facilitytime);
        Task<List<CMMRSItemsBeforeIssue>> getMRSItemsWithCode(int ID, string facilitytime);
        Task<CMMRSList> getMRSDetails(int ID, string facilitytime);
        Task<CMMRSReturnList> getReturnDataByID(int ID, string facilitytime);
        Task<CMMRSAssetTypeList> getAssetTypeByItemID(int ItemID);
        Task<CMDefaultResponse> ReturnMRS(CMMRS request, int UserID);
        Task<CMDefaultResponse> CreateReturnFaultyMRS(CMMRS request, int UserID);
        Task<CMDefaultResponse> UpdateReturnMRS(CMMRS request, int UserID);
        Task<CMDefaultResponse> mrsApproval(CMMrsApproval request, int userId);
        Task<CMDefaultResponse> mrsReject(CMApproval request, int userId);
        Task<CMDefaultResponse> ApproveMRSReturn(CMApproval request, int UserID);
        Task<CMDefaultResponse> RejectMRSReturn(CMApproval request, int UserID);
        void UpdateAssetStatus(int assetItemID, int status);
        Task<CMMRS> getLastTemplateData(int ID);
        Task<List<CMAssetItem>> GetAssetItems(int facility_ID, bool isGroupByCode = false);
        Task<CMDefaultResponse> MRSIssue(CMMRS request, int UserID);
        Task<CMDefaultResponse> ApproveMRSIssue(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectMRSIssue(CMApproval request, int userId);
        Task<List<CMMRSList>> GetMRSReturnList(int facility_ID, bool self_view, int userID, string facilitytime);
        Task<CMDefaultResponse> TransactionDetails(List<CMTransferItems> request);
        Task<CMDefaultResponse> updateUsedQty(List<CMTransferItems> request);
        Task<CMIssuedAssetItems> getIssuedAssetItems(int id);
        Task<List<CMPlantStockOpeningResponse_MRSRetrun>> getMRSReturnStockItems(int mrs_id);
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

        public async Task<List<CMMRSList>> getMRSList(int facility_ID, int emp_id, DateTime toDate, DateTime fromDate, int status, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSList(facility_ID, emp_id, toDate, fromDate, status, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSListByModule>> getMRSListByModule(int jobId, int pmId, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSListByModule(jobId, pmId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSItems>> getMRSItems(int ID, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItems(ID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSItemsBeforeIssue>> getMRSItemsBeforeIssue(int ID, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItemsBeforeIssue(ID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMRSItemsBeforeIssue>> getMRSItemsWithCode(int ID, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItemsWithCode(ID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMMRSList> getMRSDetails(int ID, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSDetails(ID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMMRSReturnList> getReturnDataByID(int ID, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getReturnDataByID(ID, facilitytime);
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

        public async Task<CMDefaultResponse> CreateReturnFaultyMRS(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.CreateReturnFaultyMRS(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateReturnMRS(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.UpdateReturnMRS(request, UserID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> mrsApproval(CMMrsApproval request, int userId)
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

        public async Task<CMDefaultResponse> MRSIssue(CMMRS request, int UserID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.MRSIssue(request, UserID);
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
        public async Task<List<CMMRSList>> GetMRSReturnList(int facility_ID, bool self_view, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.GetMRSReturnList(facility_ID, self_view, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> TransactionDetails(List<CMTransferItems> requestList)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {

                    CMDefaultResponse response = new CMDefaultResponse();
                    foreach (var request in requestList)
                    {
                        var result = await repos.TransferMaterialInTransaction(request);
                        //if (result)
                        //{
                        //    response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.SUCCESS, "Item transferred.");
                        //}
                        //else
                        //{
                        //    response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Item failed to transfer.");
                        //}
                        if (result == 0)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.SUCCESS, "Item transferred.");
                        }
                        else if (result == 1)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Item requesting more than available quantity.");
                        }
                        else if (result == 2)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "MRSItemId ( " + request.mrsItemID + " ) not found.");
                        }
                        else if (result == 3)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Exception occured during quantity updation.");
                        }
                        else if (result == 4)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "MRS Id is 0.");
                        }
                        else if (result == 5)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Issued qantity 0.");
                        }
                        else if (result == 6)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Existing qty same as updating quantity.");
                        }
                        else if (result == 7)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "No old transaction. 0 quantity not updated.");
                        }
                        else
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Unknown error code <" + result + ">. Pl contact tech team.");
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> updateUsedQty(List<CMTransferItems> requestList)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {

                    CMDefaultResponse response = new CMDefaultResponse();
                    foreach (var request in requestList)
                    {
                        var result = await repos.updateUsedQty(request.facilityID, request.fromActorID, request.fromActorType, request.toActorID, request.toActorType, request.assetItemID, request.qty, request.refType, request.refID, request.remarks, request.mrsID, 0, 0, request.mrsItemID);
                        if (result == 0)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.SUCCESS, "Item updated.");
                        }
                        else if (result == 1)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Item requesting more than available quantity.");
                        }
                        else if (result == 2)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "MRS Id ( " + request.mrsItemID + " ) not found.");
                        }
                        else if (result == 3)
                        {
                            response = new CMDefaultResponse(request.mrsID, CMMS.RETRUNSTATUS.FAILURE, "Exception occured during quantity updation.");
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMIssuedAssetItems> getIssuedAssetItems(int id)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getIssuedAssetItems(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMPlantStockOpeningResponse_MRSRetrun>> getMRSReturnStockItems(int mrs_id)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSReturnStockItems(mrs_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
