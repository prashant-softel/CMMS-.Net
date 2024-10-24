using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.PM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.PM
{
    public interface IPMScheduleViewBS
    {
        Task<List<CMPMTaskList>> GetPMTaskList(string facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds, int userID, bool self_view, string facilitytime);
        Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID, string facilitytime);
        Task<CMPMTaskView> GetPMTaskDetail(int task_id, string facilitytime);
        Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID, string facilitytime);
        Task<CMDefaultResponse> StartPMTask(int task_id, int userID, string facilitytime);
        Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID, string facilitytime);
        Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> CancelRejectedPMTaskExecution(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> CancelApprovedPMTaskExecution(CMApproval request, int userID, string facilitytime);
        Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> LinkPermitToPMTask(int task_id, int permit_id, int userID, string facilitytime);
        Task<CMDefaultResponse> AssignPMTask(int task_id, int assign_to, int userID, string facilitytime);
        Task<List<CMDefaultResponse>> UpdatePMScheduleExecution(CMPMExecutionDetail request, int userID, string facilitytime);
        Task<CMPMScheduleExecutionDetail> GetPMTaskScheduleDetail(int task_id, int schedule_id, string facilitytime);
        Task<List<CMDefaultResponse>> cloneSchedule(int facility_id, int task_id, int from_schedule_id, int to_schedule_id, int cloneJobs, int userID);
        Task<List<AssetList>> getAssetListForClone(int task_id, int schedule_id);
        Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id, string facilitytime);
        Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID, int task_id, int schedule_id, string facilitytime);
        Task<CMDefaultResponse> DeletePMTask(CMApproval request, int userID, string facilitytime);

    }
    public class PMScheduleViewBS : IPMScheduleViewBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public PMScheduleViewBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMPMTaskList>> GetPMTaskList(string facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds, int userID, bool self_view, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskList(facility_id, start_date, end_date, frequencyIds, categoryIds, userID, self_view, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.CancelPMTask(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMPMTaskView> GetPMTaskDetail(int task_id, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskDetail(task_id, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkPermitToPMTask(int task_id, int permit_id, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.LinkPermitToPMTask(task_id, permit_id, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.AddCustomCheckpoint(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartPMTask(int task_id, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.StartPMTask(task_id, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.UpdatePMTaskExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.ClosePMTaskExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CancelRejectedPMTaskExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.CancelRejectedPMTaskExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CancelApprovedPMTaskExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.CancelApprovedPMTaskExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.ApprovePMTaskExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.RejectPMTaskExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AssignPMTask(int task_id, int assign_to, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.AssignPMTask(task_id, assign_to, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMPMScheduleExecutionDetail> GetPMTaskScheduleDetail(int task_id, int schedule_id, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetPMTaskScheduleDetail(task_id, schedule_id, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResponse>> UpdatePMScheduleExecution(CMPMExecutionDetail request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.UpdatePMScheduleExecution(request, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResponse>> cloneSchedule(int facility_id, int task_id, int from_schedule_id, int to_schedule_id, int cloneJobs, int userID)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.cloneSchedule(facility_id, task_id, from_schedule_id, to_schedule_id, cloneJobs, userID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AssetList>> getAssetListForClone(int task_id, int schedule_id)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.getAssetListForClone(task_id, schedule_id);


                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.GetScheduleData(facility_id, category_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID, int task_id, int schedule_id, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.SetScheduleData(request, userID, task_id, schedule_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePMTask(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMScheduleViewRepository(getDB))
                {
                    return await repos.DeletePMTask(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
