using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.CleaningRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Cleaning
{
    public interface ICleaningBS
    {
        public Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytime);
        public Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId, string facilitytime);
        public Task<CMMCPlan> GetPlanDetails(int planId, string facilitytime);
        public Task<CMMCPlanSummary> GetPlanDetailsSummary(int planId, CMMCPlanSummary request);
        public Task<CMDefaultResponse> ApproveEndExecution(ApproveMC request, int userId, string facilitytime);
        public Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userId, string facilitytime);
        public Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> DeletePlan(int planId, int userId, string facilitytime);
        public Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId, string facilitytime);
        public Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytime);
        public Task<CMDefaultResponse> StartExecution(int executionId, int userId, string facilitytime);
        public Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId, string facilitytime);
        public Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution schedule, int userId, string facilitytime);
        public Task<CMMCExecutionSchedule> GetScheduleExecutionSummary(CMMCGetScheduleExecution schedule, string facilitytime);
        public Task<CMMCExecution> GetExecutionDetails(int id, string facilitytime);
        public Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> RejectAbandonExecution(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> ApproveAbandonExecution(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId, string facilitytime);
        public Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId);
        public Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytime);
        public Task<List<CMMCEquipmentList>> GetVegEquipmentList(int facilityId);
        public Task<CMDefaultResponse> EndScheduleExecution(int scheduleId, int userId, string facilitytime);
        public Task<CMDefaultResponse> EndExecution(int executionId, int userId, string facilitytime);
        public Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userID, string facilitytime);
        public Task<CMDefaultResponse> RejectExecution(CMApproval request, int userID, string facilitytime);
        // public Task<List<CMMCTaskEquipmentList>> GetVegTaskEquipmentList(int taskId, string facilitytime);
        public Task<CMDefaultResponse> LinkPermitToModuleCleaning(int task_id, int permit_id, int userId, string facilitytime);

    }
    public class CleaningBS : ICleaningBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        private CleaningRepository repos;

        public CleaningBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;

        }

        public int setModuleType(CMMS.cleaningType module)
        {

            if (CMMS.cleaningType.ModuleCleaning == module)
            {
                repos = new MCRepository(getDB);
            }
            if (CMMS.cleaningType.Vegetation == module)
            {
                repos = new CleaningRepository(getDB);

            }
            return 1;
        }
        public async Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {

                    return await repos.GetPlanList(facilityId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMCTaskList>> GetTaskList(int facility, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetTaskList(facility, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMMCPlan> GetPlanDetails(int planId, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetPlanDetails(planId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMMCPlanSummary> GetPlanDetailsSummary(int planId, CMMCPlanSummary request)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetPlanDetailsSummary(planId, request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.CreatePlan(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.UpdatePlan(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.ApprovePlan(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.RejectPlan(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePlan(int planId, int userID, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.DeletePlan(planId, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartExecution(int executionId, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.StartExecution(executionId, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.StartScheduleExecution(scheduleId, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution schedule, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.UpdateScheduleExecution(schedule, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMMCExecutionSchedule> GetScheduleExecutionSummary(CMMCGetScheduleExecution schedule, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetScheduleExecutionSummary(schedule, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.AbandonExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectAbandonExecution(CMApproval request, int userId, string facilitytime)
        {
            try
            {

                {
                    return await repos.RejectAbandonExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveAbandonExecution(CMApproval request, int userId, string facilitytime)
        {
            try
            {

                {
                    return await repos.ApproveAbandonExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.AbandonSchedule(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMMCExecution> GetExecutionDetails(int executionId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetExecutionDetails(executionId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMCEquipmentList>> GetEquipmentList(int facility_Id)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetEquipmentList(facility_Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetTaskEquipmentList(taskId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMCEquipmentList>> GetVegEquipmentList(int facilityId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetEquipmentList(facilityId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.ApproveExecution(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectExecution(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.RejectExecution(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EndScheduleExecution(int scheduleId, int userId, string facilitytime)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.EndScheduleExecution(scheduleId, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EndExecution(int executionId, int userId, string facilitytime)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.EndExecution(executionId, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkPermitToModuleCleaning(int task_id, int permit_id, int userId, string facilitytime)
        {

            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.LinkPermitToModuleCleaning(task_id, permit_id, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMDefaultResponse> ApproveEndExecution(ApproveMC request, int userId, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {

                    return await repos.ApproveEndExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userId, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {

                    return await repos.RejectEndExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> ApproveScheduleExecution(ApproveMC request, int userId, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {

                    return await repos.ApproveScheduleExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        internal async Task<CMDefaultResponse> RejectScheduleExecution(ApproveMC request, int userId, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {

                    return await repos.RejectScheduleExecution(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> ReAssignMcTask(int task_id, int assign_to, int userID, string facilitytime)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {

                    return await repos.ReAssignMcTask(task_id, assign_to, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public async Task<List<CMMCTaskEquipmentList>> GetVegitationTaskEquipmentList(int taskId, string facilitytime)
        //{
        //    try
        //    {
        //        // using (var repos = new MCRepository(getDB))
        //        {
        //            return await repos.GetVegitationTaskEquipmentList(taskId, facilitytime);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}