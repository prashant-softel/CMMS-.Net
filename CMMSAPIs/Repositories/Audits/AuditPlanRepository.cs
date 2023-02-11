using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Audits
{
    public class AuditPlanRepository : GenericRepository
    {
        public AuditPlanRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMAuditPlan>> GetAuditPlanList(int facility_id)
        {
            /*
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request)
        {
            /*
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            /*
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> DeleteAuditPlan(int audit_plan_id)
        {
            /*
             * Your Code goes here
            */
            return null;
        }
    }
}
