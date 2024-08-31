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
        Task<List<CMWCList>> GetWCList(int facilityId, string startDate, string endDate, int statusId, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> CreateWC(List<CMWCCreate> request, int userID, string facilitytimeZone);
        Task<CMWCDetail> GetWCDetails(int wc_id, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> UpdateWC(CMWCCreate request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> ApproveWC(CMApproval request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> RejectWC(CMApproval request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> ClosedWC(CMApproval request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> updateWCimages(filesforwc request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> ApprovedClosedWC(CMApproval request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> RejectClosedWC(CMApproval request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> CancelWC(CMApproval request, int userID, string facilitytimeZone);


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

        public async Task<List<CMWCList>> GetWCList(int facilityId, string startDate, string endDate, int statusId, int userID, string facilitytimeZone)
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

        public async Task<CMDefaultResponse> CreateWC(List<CMWCCreate> request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.CreateWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMWCDetail> GetWCDetails(int id, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.GetWCDetails(id, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateWC(CMWCCreate request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.UpdateWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveWC(CMApproval request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ApproveWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectWC(CMApproval request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.RejectWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ClosedWC(CMApproval request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ClosedWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> updateWCimages(filesforwc request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.updateWCimages(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovedClosedWC(CMApproval request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.ApprovedClosedWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectClosedWC(CMApproval request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.RejectClosedWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CancelWC(CMApproval request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new WCRepository(getDB))
                {
                    return await repos.CancelWC(request, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
