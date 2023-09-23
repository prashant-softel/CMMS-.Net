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
        Task<List<CMPMTaskList>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, int[] frequencyIds);
        Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID);
        Task<CMPMScheduleViewDetail> GetPMTaskDetail(int schedule_id);
        Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID);
        Task<CMDefaultResponse> SetPMTask(int schedule_id, int userID);
        Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID);
        Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID);
        Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID);
        Task<CMDefaultResponse> LinkPermitToPMTask(int schedule_id, int permit_id, int userID);

    }
    public class PMScheduleViewBS : IPMScheduleViewBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public PMScheduleViewBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMPMTaskList>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, int[] frequencyIds)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskList(facility_id, start_date, end_date, frequencyIds);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.CancelPMTask(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMPMScheduleViewDetail> GetPMTaskDetail(int schedule_id)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskDetail(schedule_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkPermitToPMTask(int schedule_id, int permit_id, int userID)
        {
            try
            {
                using(var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.LinkPermitToPMTask(schedule_id, permit_id, userID);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.AddCustomCheckpoint(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetPMTask(int schedule_id, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.SetPMTask(schedule_id, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.UpdatePMTaskExecution(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.ClosePMTaskExecution(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.ApprovePMTaskExecution(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.RejectPMTaskExecution(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
