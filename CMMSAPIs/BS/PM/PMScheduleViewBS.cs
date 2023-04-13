using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.PM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.PM
{
    public interface IPMScheduleViewBS
    {
        Task<List<CMPMScheduleView>> GetScheduleViewList(int facility_id, DateTime? start_date, DateTime? end_date);
        Task<CMDefaultResponse> CancelPMScheduleView(CMApproval request, int userID);
        Task<CMPMScheduleViewDetail> GetPMScheduleViewDetail(int schedule_id);
        Task<CMDefaultResponse> SetPMScheduleView(CMPMScheduleExecution request);
        Task<CMDefaultResponse> UpdatePMScheduleExecution(CMPMScheduleExecution request);
        Task<CMDefaultResponse> ApprovePMScheduleExecution(CMApproval request);
        Task<CMDefaultResponse> RejectPMScheduleExecution(CMApproval request);


    }
    public class PMScheduleViewBS : IPMScheduleViewBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public PMScheduleViewBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMPMScheduleView>> GetScheduleViewList(int facility_id, DateTime? start_date, DateTime? end_date)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetScheduleViewList(facility_id, start_date, end_date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CancelPMScheduleView(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.CancelPMScheduleView(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMPMScheduleViewDetail> GetPMScheduleViewDetail(int schedule_id)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMScheduleViewDetail(schedule_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetPMScheduleView(CMPMScheduleExecution request)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.SetPMScheduleView(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdatePMScheduleExecution(CMPMScheduleExecution request)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.UpdatePMScheduleExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovePMScheduleExecution(CMApproval request)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.ApprovePMScheduleExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPMScheduleExecution(CMApproval request)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.RejectPMScheduleExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
