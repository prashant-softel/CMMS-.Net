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
        public Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId);
        public Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userId);
        public Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId);
        public Task<CMDefaultResponse> DeletePlan(int planId, int userId);
        public Task<CMDefaultResponse> EndExecution(int executionId, int userId);
        Task<CMDefaultResponse> LinkPermitToVegetation(int task_id, int permit_id, int userId);
        public Task<CMDefaultResponse> RejectExecution(CMApproval request, int userId);
        public Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId);
        public Task<CMDefaultResponse> StartExecution(int executionId, int userId);
        public Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId);
        public Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId);
        public Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userId);
        public Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId);
    }

    public class vegetaion : VegBS
    {

        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        private CleaningRepository repos;

        public vegetaion(DatabaseProvider dbProvider)
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

        public async Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId)
        {

            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.AbandonExecution(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.ApproveExecution(request, userId);
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

        public async Task<CMDefaultResponse> DeletePlan(int planId, int userId)
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

        public async Task<CMDefaultResponse> EndExecution(int executionId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.EndExecution(executionId, userId);
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

        public async Task<CMDefaultResponse> RejectExecution(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectExecution(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId)
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

        public async Task<CMDefaultResponse> StartExecution(int executionId, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.StartExecution(executionId, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartMCExecution(int id, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.StartExecutionVegetation(id, userId);
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
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.StartScheduleExecution(scheduleId, userId);
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

        public async Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.RejectEndExecution(request, userId);
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
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.AbandonSchedule(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<CMDefaultResponse> CompleteExecution(CMMCExecution request, int userId)
        {
            try
            {
                using (var repos = new VegetationRepository(getDB))
                {
                    return await repos.CompleteExecution(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
