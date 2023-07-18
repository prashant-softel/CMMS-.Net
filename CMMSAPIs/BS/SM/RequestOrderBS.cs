using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.BS.Facility;

namespace CMMSAPIs.BS.SM
{
    public interface IRequestOrderBS
    {
        Task<List<CMRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate);
        Task<CMRequestOrder> GetRODetailsByID(int id);
        Task<CMDefaultResponse> CreateRequestOrder(CMRequestOrder request, int userID);
        Task<CMDefaultResponse> UpdateRO(CMRequestOrder request, int userID);
        Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID);
        Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request, int userID);
    }
        public class RequestOrderBS : IRequestOrderBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public RequestOrderBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }
        public async Task<List<CMRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.GetRequestOrderList(facilityID, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMRequestOrder> GetRODetailsByID(int id)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.GetRODetailsByID(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateRequestOrder(CMRequestOrder request, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.CreateRequestOrder(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateRO(CMRequestOrder request, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.UpdateRO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.DeleteRequestOrder(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.ApproveRequestOrder(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.RejectGoodsOrder(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
