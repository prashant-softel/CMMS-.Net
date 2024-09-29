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
        Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate,string facilitytime, int module_type_id);
        Task<CMAuditPlanList> GetAuditPlanByID(int id,string facilitytime);
        Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request, int userID, string facilitytimeZone);
        Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request);
        Task<CMDefaultResponse> DeleteAuditPlan(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> CreateAuditPlan(CMPMPlanDetail pm_plan, int userID);
        Task<CMDefaultResponse> StartAuditTask(int task_id, int userID, string facilitytimeZone);
        Task<List<CMDefaultResponse>> UpdateAuditTaskExecution(CMPMExecutionDetail request, int userID);
        Task<CMDefaultResponse> DeletePlan(int planId, int userID);
        Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId);
        Task<List<CMPMPlanList>> GetPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytime);
        Task<CMPMPlanDetail> GetPlanDetail(int id, string facilitytime);
        Task<List<CMPMTaskList>> GetTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string facilitytime, int module_type_id);
        Task<CMPMTaskView> GetTaskDetail(int task_id,string facilitytime);
        Task<CMDefaultResponse> CreateAuditSkip(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> RejectAuditSkip(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> ApproveAuditSkip(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> CloseAuditPlan(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> RejectCloseAuditPlan(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> ApproveClosedAuditPlan(CMApproval request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> AuditLinkToPermit(int audit_id, int ptw_id, int updatedBy, string facilitytimeZone);
        Task<CMDefaultResponse> AssignAuditTask(int task_id, int assign_to, int userID);
        Task<CMDefaultResponse> CreateSubTaskForEvaluation(int task_id, List<CMCreateAuditPlan> auditPlanList, int userID);
    }
    public class AuditPlanBS : IAuditPlanBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public AuditPlanBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate, string facilitytime, int module_type_id)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetAuditPlanList(facility_id, fromDate, toDate, facilitytime, module_type_id);

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

        public async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CreateAuditPlan(request, userID, facilitytimeZone);

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

        public async Task<CMDefaultResponse> DeleteAuditPlan(CMApproval request,int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.DeleteAuditPlan(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.ApproveAuditPlan(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.RejectAuditPlan(request, userId, facilitytimeZone);

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
        public async Task<CMDefaultResponse> StartAuditTask(int task_id, int userID, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.StartAuditTask(task_id, userID, facilitytimeZone);

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

        public async Task<List<CMPMTaskList>> GetTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string facilitytime, int module_type_id)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetTaskList(facility_id, start_date, end_date, frequencyIds, facilitytime, module_type_id);

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

        public async Task<CMDefaultResponse> CreateAuditSkip(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CreateAuditSkip(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectAuditSkip(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.RejectAuditSkip(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveAuditSkip(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.ApproveAuditSkip(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CloseAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CloseAuditPlan(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectCloseAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.RejectCloseAuditPlan(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveClosedAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.ApproveClosedAuditPlan(request, userId, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AuditLinkToPermit(int audit_id, int ptw_id, int updatedBy, string facilitytimeZone)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.AuditLinkToPermit(audit_id, ptw_id, updatedBy, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AssignAuditTask(int task_id, int assign_to, int userID)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.AssignAuditTask(task_id, assign_to, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateSubTaskForEvaluation(int task_id, List<CMCreateAuditPlan> auditPlanList, int userID)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CreateSubTaskForEvaluation(task_id, auditPlanList, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
