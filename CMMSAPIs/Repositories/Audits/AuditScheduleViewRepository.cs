using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Audits
{
    public class AuditScheduleViewRepository : GenericRepository
    {
        public AuditScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMAuditScheduleList>> GetAuditScheduleViewList(CMAuditListFilter request)
        {
            /*
             * Primary Table - AuditSchedule
             * Supporting tables - Users, Facility
             * Check the CMAuditScheduleList models properties and return 
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMAuditScheduleDetail> GetAuditScheduleDetail(int audit_id)
        {
            /*
             * Primary Table - AuditSchedule, AuditExecution
             * Supporting tables - Users, Facility, history, job
             * Check the CMAuditScheduleDetail models properties and return
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ExecuteAuditSchedule(CMExecuteAuditSchedule request)
        {
            /*
             * Primary Table -  AuditExecution, AuditSchedule
             * Supporting tables - Users, Facility, history, job
             * Check the CMExecuteAuditSchedule models properties and insert into AuditExecution
             * This function will be common for start Execution, Update, submit for Approval. 
             * If its start execution then it perform insert else update
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApproveAuditSchedule(CMApproval request)
        {
            /*
             * Primary Table - AuditSchedule
             * Read the reques and Update the to primary table
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectAuditSchedule(CMApproval request)
        {
            /*
             * Primary Table - AuditSchedule
             * Read the reques and Update the to primary table
             * Your Code goes here
            */
            return null;
        }
    }
}
