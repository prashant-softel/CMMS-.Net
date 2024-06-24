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
        public Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId);
        public Task<CMMCPlan> GetPlanDetails(int planId, string facilitytime);
        public Task<CMMCPlanSummary> GetPlanDetailsSummary(int planId, CMMCPlanSummary request);

        public Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId);
        public Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId);
        public Task<CMDefaultResponse> DeletePlan(int planId, int userId);
        public Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId);
        public Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytime);
        public Task<CMDefaultResponse> StartExecution(int executionId, int userId);
        public Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId);
        public Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution schedule, int userId);
        public Task<CMMCExecutionSchedule> GetScheduleExecutionSummary(CMMCGetScheduleExecution schedule, string facilitytime);
        public Task<CMMCExecution> GetExecutionDetails(int id, string facilitytime);
        public Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId);
        public Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId);
        public Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId);
        public Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytime);
        public Task<List<CMMCEquipmentList>> GetVegEquipmentList(int facilityId);
        public Task<CMDefaultResponse> EndScheduleExecution(int scheduleId, int userId);
        public Task<CMDefaultResponse> EndExecution(int executionId, int userId);
        public Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userID);
        public Task<CMDefaultResponse> RejectExecution(CMApproval request, int userID);
        // public Task<List<CMMCTaskEquipmentList>> GetVegTaskEquipmentList(int taskId, string facilitytime);

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
                repos = new VegetationRepository(getDB);

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
        public async Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.CreatePlan(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.UpdatePlan(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userID)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.ApprovePlan(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userID)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.RejectPlan(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePlan(int planId, int userID)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.DeletePlan(planId, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartExecution(int executionId, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.StartExecution(executionId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.StartScheduleExecution(scheduleId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution schedule, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.UpdateScheduleExecution(schedule, userId);
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
        public async Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.AbandonExecution(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.AbandonSchedule(request, userId);
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

        public async Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userID)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.ApproveExecution(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectExecution(CMApproval request, int userID)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.RejectExecution(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EndScheduleExecution(int scheduleId, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.EndScheduleExecution(scheduleId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EndExecution(int executionId, int userId)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.EndExecution(executionId, userId);
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
