using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Audits;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Audits
{
    public interface IAuditScheduleViewBS
    {
        Task<List<CMAuditScheduleList>> GetAuditScheduleViewList(CMAuditListFilter request);
        Task<CMAuditScheduleDetail> GetAuditScheduleDetail(int audit_id);
        Task<List<CMDefaultResponse>> ExecuteAuditSchedule(CMPMExecutionDetail request, int userID);
        Task<CMDefaultResponse> ApproveAuditSchedule(CMApproval request);
        Task<CMDefaultResponse> RejectAuditSchedule(CMApproval request);
    }
    public class AuditScheduleViewBS : IAuditScheduleViewBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public AuditScheduleViewBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMAuditScheduleList>> GetAuditScheduleViewList(CMAuditListFilter request)
        {
            try
            {
                using (var repos = new AuditScheduleViewRepository(getDB))
                {
                    return await repos.GetAuditScheduleViewList(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMAuditScheduleDetail> GetAuditScheduleDetail(int audit_id)
        {
            try
            {
                using (var repos = new AuditScheduleViewRepository(getDB))
                {
                    return await repos.GetAuditScheduleDetail(audit_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> ExecuteAuditSchedule(CMPMExecutionDetail request, int userID)
        {
            try
            {
                using (var repos = new AuditScheduleViewRepository(getDB))
                {
                    return await repos.ExecuteAuditSchedule(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveAuditSchedule(CMApproval request)
        {
            try
            {
                using (var repos = new AuditScheduleViewRepository(getDB))
                {
                    return await repos.ApproveAuditSchedule(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectAuditSchedule(CMApproval request)
        {
            try
            {
                using (var repos = new AuditScheduleViewRepository(getDB))
                {
                    return await repos.RejectAuditSchedule(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
