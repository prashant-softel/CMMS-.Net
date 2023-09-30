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
        Task<List<CMPMTaskList>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds);
        Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID);
        Task<CMPMTaskView> GetPMTaskDetail(int task_id);
        Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID);
        Task<CMDefaultResponse> StartPMTask(int task_id, int userID);
        Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID);
        Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID);
        Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID);
        Task<CMDefaultResponse> LinkPermitToPMTask(int task_id, int permit_id, int userID);
        Task<CMDefaultResponse> AssignPMTask(int task_id, int assign_to, int userID);
        Task<List<CMDefaultResponse>> UpdatePMScheduleExecution(CMPMExecutionDetail request, int userID);
        Task<CMPMScheduleExecutionDetail> GetPMTaskScheduleDetail(int task_id, int schedule_id);
        Task<List<CMDefaultResponse>> cloneSchedule(int facility_id,int task_id, int from_schedule_id, int to_schedule_id,int userID);

    }
    public class PMScheduleViewBS : IPMScheduleViewBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public PMScheduleViewBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMPMTaskList>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskList(facility_id, start_date, end_date, frequencyIds, categoryIds);
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

        public async Task<CMPMTaskView> GetPMTaskDetail(int task_id)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskDetail(task_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkPermitToPMTask(int task_id, int permit_id, int userID)
        {
            try
            {
                using(var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.LinkPermitToPMTask(task_id, permit_id, userID);
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

        public async Task<CMDefaultResponse> StartPMTask(int task_id, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.StartPMTask(task_id, userID);
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

        public async Task<CMDefaultResponse> AssignPMTask(int task_id, int assign_to, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.AssignPMTask(task_id, assign_to, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMPMScheduleExecutionDetail> GetPMTaskScheduleDetail(int task_id, int schedule_id)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskScheduleDetail(task_id, schedule_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResponse>> UpdatePMScheduleExecution(CMPMExecutionDetail request, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.UpdatePMScheduleExecution(request, userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResponse>> cloneSchedule(int facility_id, int task_id, int from_schedule_id, int to_schedule_id, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.cloneSchedule( facility_id,  task_id,  from_schedule_id,  to_schedule_id,  userID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
