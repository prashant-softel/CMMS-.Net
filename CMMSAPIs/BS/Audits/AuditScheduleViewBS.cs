using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Audits;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Audits
{
    public interface IAuditScheduleViewBS
    {
        Task<List<CMAuditScheduleList>> GetAuditScheduleViewList(CMAuditListFilter request);
        Task<CMAuditScheduleDetail> GetAuditScheduleDetail(int audit_id);
        Task<CMDefaultResponse> ExecuteAuditSchedule(CMExecuteAuditSchedule request);
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

        public async Task<CMDefaultResponse> ExecuteAuditSchedule(CMExecuteAuditSchedule request)
        {
            try
            {
                using (var repos = new AuditScheduleViewRepository(getDB))
                {
                    return await repos.ExecuteAuditSchedule(request);
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
