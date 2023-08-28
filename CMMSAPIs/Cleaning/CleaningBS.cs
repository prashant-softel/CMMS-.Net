using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.CleaningRepository;
using CMMSAPIs.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.BS.Cleaning
{
    public interface ICleaningBS
    {
        public Task<List<CMMCPlan>> GetPlanList(int facilityId);
        public Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId);
        public Task<CMMCPlan> GetPlanDetails(int planId);
        public Task<CMMCPlanSummary> GetPlanDetailsSummary(int planId, CMMCPlanSummary request);

        public Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId);
        public Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId);
        public Task<CMDefaultResponse> DeletePlan(int planId, int userId);
        public Task<CMDefaultResponse> UpdatePlan(CMMCPlan request, int userId);
        public Task<List<CMMCTaskList>> GetTaskList(int facilityId);
        public Task<CMDefaultResponse> StartExecution(int planId, int userId);
        public Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId);
        public Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution schedule, int userId);
        public Task<CMMCExecutionSchedule> GetScheduleExecutionSummary(CMMCGetScheduleExecution schedule);
        public Task<CMMCExecution> GetExecutionDetails(int id);
        public Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId);
        public Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId);
        public Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId);
        public Task<List<CMVegEquipmentList>> GetVegEquipmentList(int facilityId);
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

        public int setModuleType(CMMS.cleaningType module) {

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
        public async Task<List<CMMCPlan>> GetPlanList(int facilityId)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetPlanList(facilityId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMCTaskList>> GetTaskList(int facility)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetTaskList(facility);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMMCPlan> GetPlanDetails(int planId)
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetPlanDetails(planId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMMCPlanSummary> GetPlanDetailsSummary(int planId, CMMCPlanSummary request )
        {
            try
            {
                //using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetPlanDetailsSummary(planId,request);
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
        public async Task<CMDefaultResponse> UpdatePlan(CMMCPlan request, int userId)
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

        public async Task<CMDefaultResponse> StartExecution(int planId, int userId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.StartExecution(planId, userId);
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
        public async Task<CMMCExecutionSchedule> GetScheduleExecutionSummary(CMMCGetScheduleExecution schedule)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetScheduleExecutionSummary(schedule);
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

        public async Task<CMMCExecution> GetExecutionDetails(int executionId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetExecutionDetails(executionId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId)
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

        public async Task<List<CMVegEquipmentList>> GetVegEquipmentList(int facilityId)
        {
            try
            {
                // using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetVegEquipmentList(facilityId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }




    }
}
