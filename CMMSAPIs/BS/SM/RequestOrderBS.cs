﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.SM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.SM
{
    public interface IRequestOrderBS
    {
        Task<List<CMCreateRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate, string facilitytime);
        Task<List<CMCreateRequestOrderGET>> GetRODetailsByID(string IDs, string facilitytime);
        Task<CMDefaultResponse> CreateRequestOrder(CMCreateRequestOrder request, int userID, string facilityTimeZone);
        Task<CMDefaultResponse> UpdateRequestOrder(CMCreateRequestOrder request, int userID, string facilityTimeZone);
        Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID, string facilityTimeZone);
        Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userID, string facilityTimeZone);
        Task<CMDefaultResponse> RejectRequestOrder(CMApproval request, int userID, string facilityTimeZone);
        Task<CMDefaultResponse> CloseRequestOrder(CMApproval request, int userID, string facilityTimeZone);
    }
    public class RequestOrderBS : IRequestOrderBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public RequestOrderBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }
        public async Task<List<CMCreateRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate, string facilitytime)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.GetRequestOrderList(facilityID, fromDate, toDate, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMCreateRequestOrderGET>> GetRODetailsByID(string IDs, string facilitytime)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.GetRODetailsByID(IDs, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateRequestOrder(CMCreateRequestOrder request, int userID, string facilityTimeZone)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.CreateRequestOrder(request, userID, facilityTimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateRequestOrder(CMCreateRequestOrder request, int userID, string facilityTimeZone)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.UpdateRequestOrder(request, userID, facilityTimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID, string facilityTimeZone)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.DeleteRequestOrder(request, userID, facilityTimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseRequestOrder(CMApproval request, int userID, string facilityTimeZone)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.CloseRequestOrder(request, userID, facilityTimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userID, string facilityTimeZone)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.ApproveRequestOrder(request, userID, facilityTimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectRequestOrder(CMApproval request, int userID, string facilityTimeZone)
        {
            try
            {
                using (var repos = new RequestOrderRepository(getDB))
                {
                    return await repos.RejectRequestOrder(request, userID, facilityTimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
