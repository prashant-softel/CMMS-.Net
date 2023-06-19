using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.BS.SM
{
    public interface IRequestOrderBS
    {
        Task<List<CMRequestOrder>> GetRequestOrderList(int plantID, DateTime fromDate, DateTime toDate);
        Task<CMDefaultResponse> CreateRequestOrder(CMRequestOrder request, int userID);
        Task<CMDefaultResponse> UpdateGO(CMRequestOrder request, int userID);
        Task<CMDefaultResponse> DeleteRequestOrder(int RO_ID, int userID);
        Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request);
        Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request);
    }
        public class RequestOrderBS : IRequestOrderBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public RequestOrderBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }
        public async Task<List<CMRequestOrder>> GetRequestOrderList(int plantID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.GetRequestOrderList(plantID, fromDate, toDate);

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
        public async Task<CMDefaultResponse> UpdateGO(CMRequestOrder request, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.UpdateGO(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteRequestOrder(int RO_ID, int userID)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.DeleteRequestOrder(RO_ID, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.ApproveRequestOrder(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.RejectGoodsOrder(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
