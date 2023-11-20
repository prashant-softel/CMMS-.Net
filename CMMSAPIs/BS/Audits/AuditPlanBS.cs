using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Audits;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Audits
{
    public interface IAuditPlanBS
    {
        Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate);
        Task<CMAuditPlanList> GetAuditPlanByID(int id);
        Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request);
        Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request);
        Task<CMDefaultResponse> DeleteAuditPlan(int audit_plan_id);
        Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId);
        Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId);
    }
    public class AuditPlanBS : IAuditPlanBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public AuditPlanBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetAuditPlanList(facility_id, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMAuditPlanList> GetAuditPlanByID(int id)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.GetAuditPlanByID(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request)
        {
            try
            {
                using (var repos = new AuditPlanRepository(getDB))
                {
                    return await repos.CreateAuditPlan(request);

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
    }
}
