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
        Task<List<CMGO>> GetGOList(int plantID, DateTime fromDate, DateTime toDate);
        Task<List<CMGO>> GetGOItemByID(int id);
        Task<List<CMGO>> GetAssetCodeDetails(int asset_code);
        Task<CMDefaultResponse> CreateGO(CMGO request, int userID);
        Task<CMDefaultResponse> UpdateGO(CMGO request, int userID);
        Task<CMDefaultResponse> DeleteGO(int GOid, int userID);
        Task<CMDefaultResponse> WithdrawGO(CMGO request, int userID);
        Task<CMDefaultResponse> GOApproval(CMGO request, int userID);
        Task<List<PurchaseData>> GetPurchaseData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type);
        Task<List<CMGO>> getPurchaseDetailsByID(int id);
        Task<CMDefaultResponse> SubmitPurchaseData(SubmitPurchaseData request);
    }

    public class GOBS : IGOBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public GOBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMGO>> GetGOList(int plantID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetGOList(plantID, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<CMGO>> GetGOItemByID(int id)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetGOItemByID(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMGO>> GetAssetCodeDetails(int asset_code)
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

        public async Task<CMDefaultResponse> CreateGO(CMGO request, int userID)
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

        public async Task<CMDefaultResponse> UpdateGO(CMGO request, int userID)
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

        public async Task<CMDefaultResponse> DeleteGO(int GOid, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.DeleteGO(GOid, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> WithdrawGO(CMGO request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.WithdrawGO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> GOApproval(CMGO request, int userID)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GOApproval(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PurchaseData>> GetPurchaseData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetPurchaseData(plantID, empRole, fromDate, toDate, status, order_type);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMGO>> getPurchaseDetailsByID(int id)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.getPurchaseDetailsByID(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> SubmitPurchaseData(SubmitPurchaseData request)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.SubmitPurchaseData(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
