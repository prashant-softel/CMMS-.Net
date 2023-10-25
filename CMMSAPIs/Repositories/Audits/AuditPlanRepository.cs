using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Audits
{
    public class AuditPlanRepository : GenericRepository
    {
        public AuditPlanRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate)
        {

            string filter = "Where (DATE(st.Audit_Added_date) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(st.Audit_Added_date) <= '" + toDate.ToString("yyyy-MM-dd") + "')";
            filter = filter + " and st.Facility_id = " + facility_id + "";

            string SelectQ = "select st.id,plan_number,  f.name as facility_name, concat(au.firstName, ' ', au.lastName)  Auditee_Emp_Name, " +
                "concat(u.firstName, ' ', u.lastName) Auditor_Emp_Name , st.frequency, st.status, case when st.frequency = 0 then 'False' else 'True' end as FrequencyApplicable " +
                " ,st.Description,st.Schedule_Date from st_audit st " +
                "inner join facilities f ON st.Facility_id = f.id " +
                "left join users au on au.id = st.Auditee_Emp_ID " +
                "left join users u on u.id = st.Auditor_Emp_ID  " + filter;

            List<CMAuditPlanList> auditPlanList = await Context.GetData<CMAuditPlanList>(SelectQ).ConfigureAwait(false);

            for (var i = 0; i < auditPlanList.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(auditPlanList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                auditPlanList[i].short_status = _shortStatus;
            }


            return auditPlanList;
        }

        internal async Task<CMAuditPlanList> GetAuditPlanByID(int id)
        {

            string filter = " where st.id = " + id + "";


            string SelectQ = "select st.id,plan_number,  f.name as facility_name, concat(au.firstName, ' ', au.lastName)  Auditee_Emp_Name, " +
                "concat(u.firstName, ' ', u.lastName) Auditor_Emp_Name , st.frequency, st.status, case when st.frequency = 0 then 'False' else 'True' end as FrequencyApplicable, st.Description,st.Schedule_Date, st.checklist_id " +
                "from st_audit st " +
                "inner join facilities f ON st.Facility_id = f.id " +
                "left join users au on au.id = st.Auditee_Emp_ID " +
                "left join users u on u.id = st.Auditor_Emp_ID  " + filter;

            List<CMAuditPlanList> auditPlanList = await Context.GetData<CMAuditPlanList>(SelectQ).ConfigureAwait(false);
            for (var i = 0; i < auditPlanList.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(auditPlanList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                auditPlanList[i].short_status = _shortStatus;
            }
            return auditPlanList[0];
        }

        internal async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request)
        {
            CMDefaultResponse response = null;
            int InsertedValue = 0;
            string SelectQ = "select id from st_audit where plan_number = '" + request.plan_number + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                response = new CMDefaultResponse(auditPlanList[0].id, CMMS.RETRUNSTATUS.FAILURE, "Audit plan with plan number : " + request.plan_number + " already exists.");
            }
            else
            {
                string InsertQ = $"insert into st_audit(plan_number, Facility_id, Audit_Added_date, Status, Auditee_Emp_ID, Auditor_Emp_ID, Frequency, Description, Schedule_Date, Checklist_id) " +
                                $"values('{request.plan_number}', {request.Facility_id}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', {((int)CMMS.CMMS_Status.AUDIT_SCHEDULE)}, {request.auditee_id}, {request.auditor_id}, {request.ApplyFrequency},'{request.Description}','{request.Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}', {request.Checklist_id}) ; SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(InsertQ).ConfigureAwait(false);
                InsertedValue = Convert.ToInt32(dt2.Rows[0][0]);
                response = new CMDefaultResponse(InsertedValue, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + request.plan_number + " created successfully.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set plan_number = '{request.plan_number}', " +
                 $"Facility_id = {request.Facility_id}, " +
                 $"Audit_Added_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                 $"Auditee_Emp_ID = {request.auditee_id}, " +
                 $"Auditor_Emp_ID = {request.auditor_id}, " +
                 $"Frequency = {request.ApplyFrequency}, " +
                 $"Checklist_id = {request.Checklist_id}, " +
                 $"Description = '{request.Description}', " +
                 $"Schedule_Date = '{request.Schedule_Date}' " +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + request.plan_number + " updated successfully.");
            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to update.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteAuditPlan(int audit_plan_id)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from st_audit where ID = '" + audit_plan_id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_DELETED}' " +
                 $"where ID = {audit_plan_id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(audit_plan_id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].plan_number + " deleted.");
            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to delete.");
            }

            return response;
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_APPROVED:
                    retValue = "Approved";
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED:
                    retValue = "Closed";
                    break;
                case CMMS.CMMS_Status.AUDIT_DELETED:
                    retValue = "Deleted";
                    break;
                case CMMS.CMMS_Status.AUDIT_REJECTED:
                    retValue = "Rejected";
                    break;
                case CMMS.CMMS_Status.AUDIT_SCHEDULE:
                    retValue = "Schedule";
                    break;
                case CMMS.CMMS_Status.AUDIT_STARTED:
                    retValue = "Started";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }

    }
}
