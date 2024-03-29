using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Audits;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Audits
{
    public interface IAuditPlanBS
    {
        Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate,string facilitytime);
        Task<CMAuditPlanList> GetAuditPlanByID(int id,string facilitytime);
        Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request, int userID);
        Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request);
        Task<CMDefaultResponse> DeleteAuditPlan(int audit_plan_id);
        Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId);
        Task<CMDefaultResponse> CreateAuditPlan(CMPMPlanDetail pm_plan, int userID);
        Task<CMDefaultResponse> StartAuditTask(int task_id, int userID);
        Task<List<CMDefaultResponse>> UpdateAuditTaskExecution(CMPMExecutionDetail request, int userID);
        Task<CMDefaultResponse> DeletePlan(int planId, int userID);
        Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId);
        Task<List<CMPMPlanList>> GetPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytime);
        Task<CMPMPlanDetail> GetPlanDetail(int id, string facilitytime);
        Task<List<CMPMTaskList>> GetTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string facilitytime);
        Task<CMPMTaskView> GetTaskDetail(int task_id,string facilitytime);

    }
    public class AuditPlanBS : IAuditPlanBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public AuditPlanBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate, string facilitytime)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetAuditPlanList(facility_id, fromDate, toDate, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMAuditPlanList> GetAuditPlanByID(int id, string facilitytime)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetAuditPlanByID(id, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request, int userID)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CreateAuditPlan(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.UpdateAuditPlan(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteAuditPlan(int audit_plan_id)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.DeleteAuditPlan(audit_plan_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.ApproveAuditPlan(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.RejectAuditPlan(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateAuditPlan(CMPMPlanDetail plan, int userID)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CreateAuditPlan(plan, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> StartAuditTask(int task_id, int userID)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.StartAuditTask(task_id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> UpdateAuditTaskExecution(CMPMExecutionDetail request, int userID)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.UpdateAuditTaskExecution(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.ApprovePlan(request, userId);

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
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.RejectPlan(request, userId);

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
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.DeletePlan(planId, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMPMPlanList>> GetPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytime)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    
                    return await repos.GetAuditPlanList(facility_id, category_id, frequency_id, start_date, end_date, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMPMPlanDetail> GetPlanDetail(int id,string facilitytime)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetAuditPlanDetail(id, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMPMTaskList>> GetTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string facilitytime)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetTaskList(facility_id, start_date, end_date, frequencyIds, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMPMTaskView> GetTaskDetail(int task_id,string facilitytime)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetTaskDetail(task_id, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
