using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.MCVCRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Cleaning
{
    public interface IMCVCBS
    {
        public int setModuleType(CMMS.cleaningType module);

        //List
        public Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone, string startDate, string endDate); /// DONE ///
        public Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytimeZone, string startDate, string endDate); /// DONE ///
        public Task<CMMCPlan> GetPlanDetails(int planId, string facilitytimeZone);
        // public Task<CMMCPlan> GetPlanDetailsSummary(int planId, CMMCPlanSummary request);
        public Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytimeZone);
        public Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId);
        public Task<CMMCExecution> GetExecutionDetails(int executionId, string facilitytimeZone);
        public Task<CMMCExecutionSchedule> GetScheduleDetails(int scheduleId, string facilitytimeZone);

        //Plan
        public Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> DeletePlan(int planId, int userId, string facilitytimeZone);


        //Task
        public Task<CMDefaultResponse> ApproveEndExecution(CMApproval request, int userId, string facilitytime);
        public Task<CMDefaultResponse> EndExecution(int executionId, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> StartExecution(int executionId, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> ReAssignTask(int task_id, int assign_to, int userID, string facilitytimeZone);
        public Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> ApproveAbandonExecution(CMApproval request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> RejectAbandonExecution(CMApproval request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> RejectExecution(CMApproval request, int userId, string facilitytimeZone);


        //Schedule
        public Task<CMDefaultResponse> EndScheduleExecution(int scheduleId, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> RejectScheduleExecution(ApproveMC request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> ApproveScheduleExecution(ApproveMC request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId, string facilitytimeZone);
        public Task<CMDefaultResponse> LinkPermitToMCVC(int task_id, int permit_id, int userId, string facilitytimeZone);

        // public Task<CMDefaultResponse> CompleteExecution(CMMCExecution request, int userId);

    }

    public class MCVCBS : IMCVCBS
    {

        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();

        private MCVCRepository repos;
        /*private CleaningRepository repo;*/
        private CMMS.cleaningType module;

        public MCVCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;

        }

        public int setModuleType(CMMS.cleaningType module)
        {
            this.module = module;

            /* if (CMMS.cleaningType.ModuleCleaning == module)
             {
                 repos = new MCVCRepository(getDB);
             }
             if (CMMS.cleaningType.Vegetation == module)
             {
                 repos = new MCVCRepository(getDB);

             }*/
            return 1;
        }

        public async Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone, string startDate, string endDate)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetPlanList(facilityId, facilitytimeZone, startDate, endDate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytimeZone, string startDate, string endDate)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetTaskList(facilityId, facilitytimeZone, startDate, endDate);
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
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetPlanDetails(planId, facilitytime);
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
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetTaskEquipmentList(taskId, facilitytimeZone);
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
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetEquipmentList(facilityId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMMCExecutionSchedule> GetScheduleDetails(int scheduleId, string facilitytime)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetScheduleDetails(scheduleId, facilitytime);
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
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.GetExecutionDetails(executionId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }


        /*     public async Task<CMMCPlan> GetPlanDetailsSummary(int planId, CMMCPlanSummary request)
             {
                 try
                 {
                    using (var repos = new MCVCRepository(getDB, module))
                     {
                         //return await repos.GetPlanDetailsSummary(planId,  request);
                     }
                 }
                 catch (Exception ex)
                 {
                     throw;
                 }
             }*/




        public async Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId, string facilitytime)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.CreatePlan(request, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.UpdatePlan(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.ApprovePlan(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.RejectPlan(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePlan(int planId, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.DeletePlan(planId, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveEndExecution(CMApproval request, int userId, string facilitytime)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {

                    return await repos.ApproveEndExecution(request, userId, facilitytime);
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
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.EndExecution(executionId, userId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartExecution(int executionId, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.StartExecution(executionId, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ReAssignTask(int task_id, int assign_to, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.ReAssignTask(task_id, assign_to, userID, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.AbandonExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveAbandonExecution(CMApproval request, int userId, string facilitytimeZone)
        {


            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.ApproveAbandonExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectAbandonExecution(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.RejectAbandonExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.RejectEndExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> RejectExecution(CMApproval request, int userId, string facilitytimeZone)
        {

            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.RejectExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.ApproveExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<CMDefaultResponse> EndScheduleExecution(int executionId, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.EndScheduleExecution(executionId, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<CMDefaultResponse> RejectScheduleExecution(ApproveMC request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.RejectScheduleExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> ApproveScheduleExecution(ApproveMC request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.ApproveScheduleExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.StartScheduleExecution(scheduleId, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.UpdateScheduleExecution(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.AbandonSchedule(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkPermitToMCVC(int task_id, int permit_id, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MCVCRepository(getDB, module))
                {
                    return await repos.LinkPermitToMCVC(task_id, permit_id, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /*  public async Task<CMDefaultResponse> CompleteExecutionVegetation(CMMCExecution request, int userId)
          {
              try
              {
                 using (var repos = new MCVCRepository(getDB, module))
                  {
                      return await repos.CompleteExecution(request, userId);
                  }
              }
              catch (Exception ex)
              {
                  throw;
              }
          }*/


    }


}




