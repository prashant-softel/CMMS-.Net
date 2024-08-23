using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using MailKit.Search;

namespace CMMSAPIs.BS
{
    public interface IGOBS
    {
        Task<List<CMGOListByFilter>> GetGOList(int facility_id, DateTime fromDate, DateTime toDate, int Status, string facilitytime);
        Task<CMGoodsOrderList> GetGOItemByID(int id,string facilitytime);
        Task<List<CMGoodsOrderList>> GetAssetCodeDetails(int asset_code);
        Task<CMDefaultResponse> CreateGO(CMGoodsOrderList request, int userID);
        Task<CMDefaultResponse> UpdateGO(CMGoodsOrderList request, int userID);
        Task<CMDefaultResponse> DeleteGO(CMApproval request, int userID);
        Task<CMDefaultResponse> CloseGO(CMGoodsOrderList request, int userID);
        Task<CMDefaultResponse> ApproveGO(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectGO(CMApproval request, int userId);
        Task<List<CMPURCHASEDATA>> GetGoodsOrderData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type,string facilitytime);
        Task<CMGOMaster> GetGODetailsByID(int id);
        Task<CMDefaultResponse> SubmitPurchaseData(CMSUBMITPURCHASEDATA request, int userId);
        Task<List<CMGOListByFilter>> GetSubmitPurchaseOrderList(int facility_id, DateTime fromDate, DateTime toDate, int Status, string facilitytime);

        Task<CMDefaultResponse> UpdateGOReceive(CMGoodsOrderList request, int userID);
        Task<CMDefaultResponse> ApproveGOReceive(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectGOReceive(CMApproval request, int userId);
        Task<CMDefaultResponse> CloseRO(CMGoodsOrderList request, int userID);
    }

    public class GOBS : IGOBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public GOBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMGOListByFilter>> GetGOList(int facility_id, DateTime fromDate, DateTime toDate, int Status, string facilitytime)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                   int is_purchaseorder = 0;
                    return await repos.GetGOList(facility_id, fromDate, toDate, is_purchaseorder,  facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMGoodsOrderList> GetGOItemByID(int id, string facilitytime)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetGOItemByID(id, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMGoodsOrderList>> GetAssetCodeDetails(int asset_code)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetAssetCodeDetails(asset_code);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateGO(CMGoodsOrderList request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.CreateGO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateGO(CMGoodsOrderList request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.UpdateGO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteGO(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.DeleteGO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CloseGO(CMGoodsOrderList request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.CloseGO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveGO(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.ApproveGoodsOrder(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectGO(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.RejectGoodsOrder(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMPURCHASEDATA>> GetGoodsOrderData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type, string facilitytime)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetGoodsOrderData(plantID, empRole, fromDate, toDate, status, order_type,  facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMGOMaster> GetGODetailsByID(int id)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetGODetailsByID(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> SubmitPurchaseData(CMSUBMITPURCHASEDATA request, int userId)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.SubmitPurchaseData(request, userId) ;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

                public async Task<List<CMGOListByFilter>> GetSubmitPurchaseOrderList(int facility_id, DateTime fromDate, DateTime toDate, int Status, string facilitytime)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    int is_purchaseorder = 1;
                    return await repos.GetGOList(facility_id, fromDate, toDate, is_purchaseorder,  facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateGOReceive(CMGoodsOrderList request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.UpdateGOReceive(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveGOReceive(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.ApproveGoodsOrderReceive(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectGOReceive(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.RejectGoodsOrderReceive(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseRO(CMGoodsOrderList request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.CloseRO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
