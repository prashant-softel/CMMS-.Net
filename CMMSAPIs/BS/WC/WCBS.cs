using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.WC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.WC
{
    public interface IWCBS
    {
        Task<List<CMWCList>> GetWCList(int facilityId, string startDate, string endDate, int statusId);
        Task<CMDefaultResponse> CreateWC(List<CMWCCreate> request, int userID);
        Task<CMWCDetail> GetWCDetails(int wc_id);
        Task<CMDefaultResponse> UpdateWC(CMWCCreate request, int userID);
        Task<CMDefaultResponse> ApproveWC(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectWC(CMApproval request, int userID);
        Task<CMDefaultResponse> ClosedWC(CMApproval request, int userID);
        Task<CMDefaultResponse> updateWCimages(filesforwc request, int userID);
        Task<CMDefaultResponse> ApprovedClosedWC(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectClosedWC(CMApproval request, int userID);


        //  Add those methods here and in WCcontroller

    }
    public class WCBS : IWCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public WCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMWCList>> GetWCList(int facilityId, string startDate, string endDate, int statusId)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.GetWCList(facilityId, startDate, endDate, statusId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateWC(List<CMWCCreate> request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.CreateWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMWCDetail> GetWCDetails(int id)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.GetWCDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateWC(CMWCCreate request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.UpdateWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveWC(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ApproveWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectWC(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.RejectWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ClosedWC(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ClosedWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> updateWCimages(filesforwc request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.updateWCimages(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovedClosedWC(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ApprovedClosedWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectClosedWC(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.RejectClosedWC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
