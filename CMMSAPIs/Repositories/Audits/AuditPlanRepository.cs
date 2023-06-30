using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            CMDefaultResponse response = null;
            string SelectQ = "select * from st_audit where plan_number = '"+request.plan_number+"'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if(auditPlanList != null && auditPlanList.Count > 0 ) {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.FAILURE, "Audit plan with plan number : "+request.plan_number+" already exists.");
            }
            else
            {
                string InsertQ = $"insert into st_audit(plan_number, Facility_id, Audit_Added_date, Status, Auditee_Emp_ID, Auditor_Emp_ID) " +
                                $"values('{request.plan_number}', {request.Facility_id}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', {request.Status}, {request.auditee_id}, {request.auditor_id})";
                await Context.ExecuteNonQry<int>(InsertQ);
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + request.plan_number + " created successfully.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select * from st_audit where plan_number = '" + request.plan_number + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set plan_number = '{request.plan_number}', " +
                 $"Facility_id = {request.Facility_id}, " +
                 $"Audit_Added_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                 $"Status = {request.Status}, " +
                 $"Auditee_Emp_ID = {request.auditee_id}, " +
                 $"Auditor_Emp_ID = {request.auditor_id} " +
                 $"where ID = {request.id}";
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + request.plan_number + " updated successfully.");
            }
            else
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.FAILURE, "Audit plan with plan number : " + request.plan_number + " does not exists to update.");
            }

            return response;
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
