using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.CleaningRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Cleaning
{
    public interface VegBS
    {
        public Task<CMDefaultResponse> AbandonExecutionVegetation(CMApproval request, int userId);
        public Task<CMDefaultResponse> ApproveExecutionVegetation(ApproveMC request, int userId);
        public Task<CMDefaultResponse> StartExecutionVegetation(int executionId, int userId);
        public Task<CMDefaultResponse> StartScheduleExecutionVegetation(int scheduleId, int userId);
        public Task<CMDefaultResponse> EndExecutionVegetation(int executionId, int userId);
        public Task<CMDefaultResponse> LinkPermitToVegetation(int task_id, int permit_id, int userId);
        public Task<CMDefaultResponse> RejectExecutionVegetation(CMApproval request, int userId);
        public Task<CMDefaultResponse> RejectEndExecutionVegetation(ApproveMC request, int userId);
        public Task<CMDefaultResponse> AbandonScheduleVegetation(CMApproval request, int userId);
        public Task<CMDefaultResponse> CompleteExecutionVegetation(CMMCExecution request, int userId);
        public Task<CMDefaultResponse> ApproveScheduleExecutionVegetation(ApproveMC request, int userId);
        public Task<CMDefaultResponse> RejectAbandonExecutionVegetation(CMApproval request, int userId);
        public Task<CMDefaultResponse> ApproveAbandonExecutionVegetation(CMApproval request, int userId);
        public Task<CMDefaultResponse> EndScheduleExecutionVegetation(int scheduleId, int userId);
        public Task<CMDefaultResponse> ReAssignMcTaskVegetation(int task_id, int assign_to, int userID);
        public Task<CMDefaultResponse> UpdateScheduleExecutionVegetation(CMMCGetScheduleExecution request, int userId);
        public Task<CMDefaultResponse> ApproveEndExecutionVegetation(ApproveMC request, int userId);
        public Task<CMDefaultResponse> RejectScheduleExecutionVegetation(ApproveMC request, int userId);
        public Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytimeZone);
        public Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId);
        public Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId);
        public Task<CMMCPlan> GetPlanDetails(int planId, string facilitytimeZone);
        public Task<List<CMMCEquipmentList>> GetVegEquipmentList(int facilityId);
        public Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytimeZone);
        public Task<CMMCExecution> GetExecutionDetails(int executionId, string facilitytimeZone);
        //Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone);
        // Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone);
    }

    public class vegetaion : VegBS
    {

        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        private VegetationRepository repos;
        private CleaningRepository repo;
        private CMMS.cleaningType module;

        public vegetaion(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;

        }

        internal int setModuleType(CMMS.cleaningType vegetation)
        {

            if (CMMS.cleaningType.Vegetation == module)
            {
                repos = new VegetationRepository(getDB);
            }
            return 1;
        }


        public async Task<CMDefaultResponse> AbandonExecutionVegetation(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.AbandonExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartExecutionVegetation(int executionId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.StartExecutionVegetation(executionId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartScheduleExecutionVegetation(int scheduleId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.StartScheduleExecutionVegetation(scheduleId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EndExecutionVegetation(int executionId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.EndExecutionVegetation(executionId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkPermitToVegetation(int task_id, int permit_id, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.LinkPermitToVegetation(task_id, permit_id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AbandonScheduleVegetation(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.AbandonScheduleVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CompleteExecutionVegetation(CMMCExecution request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.CompleteExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EndScheduleExecutionVegetation(int scheduleId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.EndScheduleExecutionVegetation(scheduleId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ReAssignMcTaskVegetation(int task_id, int assign_to, int userID)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ReAssignMcTaskVegetation(task_id, assign_to, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateScheduleExecutionVegetation(CMMCGetScheduleExecution request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.UpdateScheduleExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveEndExecutionVegetation(ApproveMC request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ApproveEndExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveScheduleExecutionVegetation(ApproveMC request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ApproveScheduleExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectAbandonExecutionVegetation(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectAbandonExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveExecutionVegetation(ApproveMC request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ApproveEndExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectEndExecutionVegetation(ApproveMC request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectEndExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> ApproveAbandonExecutionVegetation(CMApproval request, int userId)
        {


            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ApproveAbandonExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectExecutionVegetation(CMApproval request, int userId)
        {

            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectExecutionVegetation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> RejectScheduleExecutionVegetation(ApproveMC request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectScheduleExecutionVegetation(request, userId);
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
                using (var repos = new VegetationRepository(getDB))
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
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.UpdatePlan(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMMCPlan> GetPlanDetails(int planId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.GetPlanDetails(planId, facilitytimeZone);
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
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.GetEquipmentList(facilityId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.GetTaskEquipmentList(taskId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.GetTaskList(facilityId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMMCExecution> GetExecutionDetails(int executionId, string facilitytimeZone)

        {
            try
            {
                // using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.GetExecutionDetails(executionId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        internal async Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.GetPlanList(facilityId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ApprovePlan(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectPlan(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> DeletePlan(int planId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.DeletePlan(planId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> StartExecution(int planId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.StartExecutionVegetation(planId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
