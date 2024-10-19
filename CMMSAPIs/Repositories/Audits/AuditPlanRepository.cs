using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Audits
{
    public class AuditPlanRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public AuditPlanRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        Dictionary<CMMS.CMMS_Status, string> statusList = new Dictionary<CMMS.CMMS_Status, string>()
        {
            { CMMS.CMMS_Status.AUDIT_SCHEDULE, "Scheduled" },
            { CMMS.CMMS_Status.AUDIT_START, "Started" },
            { CMMS.CMMS_Status.AUDIT_DELETED, "Deleted" },
            { CMMS.CMMS_Status.AUDIT_APPROVED, "Approved" },
            { CMMS.CMMS_Status.AUDIT_REJECTED, "Rejected" },
            { CMMS.CMMS_Status.AUDIT_CLOSED, "Closed" },
            { CMMS.CMMS_Status.AUDIT_SKIP, "Skip" },
            { CMMS.CMMS_Status.AUDIT_SKIP_APPROVED, "Skip Approved" },
            { CMMS.CMMS_Status.AUDIT_SKIP_REJECT, "Skip Reject" },
            { CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED, "Closed Approved" },
            { CMMS.CMMS_Status.AUDIT_CLOSED_REJECT, "Closed Reject" },
            { CMMS.CMMS_Status.AUDIT_EXECUTED, "Executed" }
        };

        internal async Task<List<CMAuditPlanList>> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate, string facilitytimeZone, int module_type_id)
        {

            string filter = "Where (DATE(st.Audit_Added_date) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(st.Audit_Added_date) <= '" + toDate.ToString("yyyy-MM-dd") + "')";
            filter = filter + " and st.Facility_id = " + facility_id + "";

            if (module_type_id > 0)
            {
                filter = filter + " and st.module_type_id = " + module_type_id + "";
            }
            string SelectQ = "select st.id,title,  f.name as facility_name, concat(au.firstName, ' ', au.lastName)  Auditee_Emp_Name, " +
                "concat(u.firstName, ' ', u.lastName) Auditor_Emp_Name,checklist_number as checklist_name, frequency.name as frequency_name,st.frequency,st.checklist_id, st.status, case when st.frequency = 0 then 'False' else 'True' end as FrequencyApplicable " +
                " ,st.Description,st.Schedule_Date,module_type_id as Module_Type_id from st_audit st " +
                "inner join facilities f ON st.Facility_id = f.id " +
                "left join users au on au.id = st.Auditee_Emp_ID " +
                "left join checklist_number checklist_number on checklist_number.id = st.Checklist_id " +
                "left join frequency frequency on frequency.id = st.Frequency " +
                "left join users u on u.id = st.Auditor_Emp_ID  " + filter;

            List<CMAuditPlanList> auditPlanList = await Context.GetData<CMAuditPlanList>(SelectQ).ConfigureAwait(false);

            for (var i = 0; i < auditPlanList.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(auditPlanList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                auditPlanList[i].short_status = _shortStatus;
            }
            foreach (var list in auditPlanList)
            {
                list.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.created_at);
                list.Schedule_Date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.Schedule_Date);

            }


            return auditPlanList;
        }

        internal async Task<CMAuditPlanList> GetAuditPlanByID(int id, string facilitytimeZone)
        {

            string filter = " where st.id = " + id + "";
            string SelectQ = "select st.id,title,  f.name as facility_name, concat(au.firstName, ' ', au.lastName)  Auditee_Emp_Name,  " +
                             "st.max_score as max_score , concat(u.firstName, ' ', u.lastName) Auditor_Emp_Name , st.frequency, st.status, " +
                             "case when st.frequency = 0 then 'False' else 'True' end as FrequencyApplicable, st.Description,st.Schedule_Date," +
                             " st.checklist_id, " +
                             " checklist_number as checklist_name, frequency.name as frequency_name, st.created_at, " +
                             "concat(created.firstName, ' ', created.lastName) created_by, st.approved_Date, concat(ct.firstName, ' ', ct.lastName) approved_by_name, " +
                             "st.module_type_id as Module_Type_id, case when st.module_type_id = 1 then 'PM'  when st.module_type_id = 2 then 'HOTO'  when st.module_type_id = 3 then 'Audit' \r\n  when st.module_type_id = 4 then 'MIS' end as  Module_Type,   assignedTo,Employees,  case when is_PTW = 1 then 'True' else 'False' end is_PTW" +
                             " from st_audit st " +
                             "inner join facilities f ON st.Facility_id = f.id " +
                             "left join users au on au.id = st.Auditee_Emp_ID " +
                             "left join users u on u.id = st.Auditor_Emp_ID " +
                             "left join checklist_number checklist_number on checklist_number.id = st.Checklist_id " +
                             "left join frequency frequency on frequency.id = st.Frequency " +
                             "left join users ct on ct.id = st.approved_by " +
                             "left join users created on created.id = st.created_by   " + filter;

            List<CMAuditPlanList> auditPlanList = await Context.GetData<CMAuditPlanList>(SelectQ).ConfigureAwait(false);
            if (auditPlanList[0].Module_Type_id == (int)CMMS.checklist_type.Evaluation)
            {
                auditPlanList[0].map_checklist = await GetEvaluationChecklistByID(id, facilitytimeZone);
            }
            for (var i = 0; i < auditPlanList.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(auditPlanList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                auditPlanList[i].short_status = _shortStatus;
            }
            foreach (var list in auditPlanList)
            {
                list.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.created_at);
                list.Schedule_Date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.Schedule_Date);
            }

            return auditPlanList[0];
        }

        internal async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request, int userID, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            int InsertedValue = 0;

            int status = 0;
            status = (int)CMMS.CMMS_Status.AUDIT_SCHEDULE;

            string SelectQ = "select id from st_audit where title = '" + request.plan_number + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                response = new CMDefaultResponse(auditPlanList[0].id, CMMS.RETRUNSTATUS.FAILURE, "Audit plan with plan number : " + request.title + " already exists.");
            }
            else
            {

                string InsertQ = $"insert into st_audit(title, Facility_id, Audit_Added_date, Status, Auditee_Emp_ID, Auditor_Emp_ID, Frequency, Description, Schedule_Date, Checklist_id, created_by, created_at,assignedTo,Employees,is_PTW,module_type_id,max_score) " +
                                $"values('{request.plan_number}', {request.Facility_id}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', {status}, {request.auditee_id}, {request.auditor_id}, {request.ApplyFrequency},'{request.Description}','{request.Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}', {request.Checklist_id}, {userID},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{request.assignedTo}','{string.Join(", ", request.Employees)}', {request.is_PTW},{request.Module_Type_id},{request.max_score}) ; SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(InsertQ).ConfigureAwait(false);
                InsertedValue = Convert.ToInt32(dt2.Rows[0][0]);

                response = new CMDefaultResponse(InsertedValue, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + request.title + " created successfully.");

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, InsertedValue, 0, 0, " Audit plan schduled ", CMMS.CMMS_Status.AUDIT_SCHEDULE, userID);

                CMPMPlanDetail _AuditList = await GetAuditPlanDetail(InsertedValue, facilitytimeZone);

                _ = await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_PLAN, CMMS.CMMS_Status.AUDIT_SCHEDULE, new[] { userID }, _AuditList);

            }

            // for Evaluation we are setting checklist map here

            if (request.Module_Type_id == (int)CMMS.checklist_type.Evaluation && request.map_checklist.Count > 0)
            {
                string mapChecklistQry = "INSERT INTO evalution_checklist_map(evalution_plan_id, checklist_id, weightage,ptw_req,comments,created_by,created_at) VALUES ";
                foreach (var map in request.map_checklist)
                {
                    mapChecklistQry += $"({InsertedValue}, {map.checklist_id}, {map.weightage},{map.ptw_req},'{map.comment}',{userID},'{UtilsRepository.GetUTCTime()}'), ";
                }
                if (mapChecklistQry.Length > 0)
                {
                    mapChecklistQry = mapChecklistQry.TrimEnd(',', ' ') + ";";
                }
                await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
                response = new CMDefaultResponse(InsertedValue, CMMS.RETRUNSTATUS.SUCCESS, "Plan with plan number : " + request.title + " created successfully.");
            }

            return response;
        }

        internal async Task<List<CMEvaluationAuditList>> GetEvaluationChecklistByID(int id, string facilitytimeZone)
        {

            string SelectQ = "select e.id as map_checlist,evalution_plan_id ,checklist_id as id,ck.checklist_number as checklist_number, weightage,comments,e.ptw_req as ptw_required,e.title as title ," +
                " CONCAT(createdByUser.firstName, ' ', createdByUser.lastName) AS createdByName, " +
                " CONCAT(updatedByUser.firstName, ' ', updatedByUser.lastName) AS updated_by_name,e.created_at as createdAt,e.updated_at as updatedAt " +
                " from evalution_checklist_map e" +
                 " LEFT JOIN checklist_number AS ck ON ck.id = e.checklist_id" +
                " LEFT JOIN users AS createdByUser ON createdByUser.id = e.created_by" +
                " LEFT JOIN users AS updatedByUser ON updatedByUser.id = e.updated_by " +
                " where evalution_plan_id = " + id + "; ";

            List<CMEvaluationAuditList> list = await Context.GetData<CMEvaluationAuditList>(SelectQ).ConfigureAwait(false);
            foreach (var item in list)
            {
                item.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, item.createdAt);
                item.updatedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, item.updatedAt);
            }
            return list;
        }

        internal async Task<CMDefaultResponse> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set title = '{request.title}', " +
                 $"Facility_id = {request.Facility_id}, " +
                 $"Audit_Added_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                 $"Frequency = {request.ApplyFrequency}, " +
                 $"Checklist_id = {request.Checklist_id}, " +
                 $"Description = '{request.Description}', " +
                 $"assignedTo = '{request.assignedTo}', " +
                 $"is_PTW = {request.is_PTW}, " +
                 $"module_type_id = {request.Module_Type_id}, " +
                 $"Schedule_Date = '{request.Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                foreach (var map in request.map_checklist)
                {
                    if (request.Module_Type_id == (int)CMMS.checklist_type.Evaluation && request.map_checklist.Count > 0)
                    {
                        string mapChecklistQry = $"Update  evalution_checklist_map SET  weightage ={map.weightage},comments='{map.comment}'  ,  " +
                                                  $"updated_by ={0},updated_at='{UtilsRepository.GetUTCTime()}',  " +
                                                  $"ptw_req={map.ptw_req} where checklist_id={map.checklist_id} ";
                        await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
                    }
                }
            }
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id,module_type_id as Module_Type_id  from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);
            int status = 0;
            status = (int)CMMS.CMMS_Status.AUDIT_DELETED;
            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set Status = '{status}'," +
                 $"deleted_by = {userId}, " +
                 $"deleted_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                CMPMPlanDetail _AuditList = await GetAuditPlanDetail(request.id, facilitytimeZone);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].title + " deleted.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, " Audit plan deleted ", CMMS.CMMS_Status.AUDIT_DELETED);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_PLAN, CMMS.CMMS_Status.AUDIT_DELETED, new[] { userId }, _AuditList);


            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Plan does not exists to delete.");
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
                case CMMS.CMMS_Status.AUDIT_START:
                    retValue = "Started";
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP:
                    retValue = "Skip";
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_REJECT:
                    retValue = "Skip Rejected";
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_APPROVED:
                    retValue = "Skip Approved";
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_REJECT:
                    retValue = "Closed Rejected";
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED:
                    retValue = "Closed Approved";
                    break;
                case CMMS.CMMS_Status.AUDIT_EXECUTED:
                    retValue = "Executed";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = "Audit link with permit";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }

        internal async Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id, Facility_id,Frequency as ApplyFrequency, Schedule_Date,Auditor_Emp_ID as auditor_id,assignedTo,Module_Type_id from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);
            int status = 0;

            status = (int)CMMS.CMMS_Status.AUDIT_APPROVED;

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set Status = '{status}' ," +
                 $"approved_by = {userId}, " +
                 $"approved_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                 $"approved_Comment = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);



                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].title + " approved successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, CMMS.CMMS_Modules.AUDIT_PLAN, 0, request.comment, CMMS.CMMS_Status.AUDIT_APPROVED);
                CMPMPlanDetail _AuditList = await GetAuditPlanDetail(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_PLAN, CMMS.CMMS_Status.AUDIT_APPROVED, new[] { userId }, _AuditList);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to approve.");
            }

            for (var i = 0; i < auditPlanList.Count; i++)
            {

                string entryInTask = $" INSERT INTO pm_task (plan_id, category_id, facility_id, frequency_id, plan_date, prev_task_done_date, closed_at, " +
                    $" assigned_to_audit, PTW_id, status) VALUES " +
                    $" ({auditPlanList[i].id},0,{auditPlanList[i].Facility_id},{auditPlanList[i].ApplyFrequency} ,  " +
                    $" '{auditPlanList[i].Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}',null,null,'{auditPlanList[i].assignedTo}' ,  " +
                    $" 0,{status});SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(entryInTask).ConfigureAwait(false);
                int task_id = Convert.ToInt32(dt2.Rows[0][0]);

                if (auditPlanList[0].Module_Type_id != (int)CMMS.checklist_type.Evaluation)
                {
                    string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                    $"select {task_id} as task_id,id as plan_id, 0 as Asset_id, Checklist_id  as checklist_id,Schedule_Date    as PM_Schedule_date,{(int)CMMS.CMMS_Status.AUDIT_SCHEDULE} as status from st_audit  where id = {request.id}";
                    await Context.ExecuteNonQry<int>(scheduleQry);

                    string setCodeNameQuery = "UPDATE pm_schedule " +
                                                "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                                "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                                "PM_Schedule_Number = CONCAT('SCH',id), " +
                                                "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
                    await Context.ExecuteNonQry<int>(setCodeNameQuery);
                }
            }
            return response;
        }
        internal async Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id,Module_Type_id from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);
            int status = 0;
            status = (int)CMMS.CMMS_Status.AUDIT_REJECTED;

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set Status = '{status}' , rejected_by = {userId}, rejected_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', rejected_Comment = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);



                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].title + " rejected successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_REJECTED, userId);
                CMPMPlanDetail _AuditList = await GetAuditPlanDetail(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_PLAN, CMMS.CMMS_Status.AUDIT_REJECTED, new[] { userId }, _AuditList);
            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Plan does not exists to reject.");
            }

            return response;
        }
        internal async Task<CMDefaultResponse> CreateAuditPlan(CMPMPlanDetail pm_plan, int userID)
        {
            int status = pm_plan.isDraft > 0 ? (int)CMMS.CMMS_Status.PM_PLAN_DRAFT : (int)CMMS.CMMS_Status.PM_PLAN_CREATED;

            string checklistIDsQry = $"SELECT id FROM checklist_number WHERE facility_id = {pm_plan.facility_id} " +
                                        $"AND asset_category_id = {pm_plan.category_id} AND frequency_id = {pm_plan.plan_freq_id} " +
                                        $"AND checklist_type = 1; ";
            DataTable dt1 = await Context.FetchData(checklistIDsQry).ConfigureAwait(false);
            List<int> checklistIDs = dt1.GetColumn<int>("id");
            string assetIDsQry = $"SELECT id FROM assets WHERE facilityId = {pm_plan.facility_id} AND categoryId = {pm_plan.category_id}; ";
            DataTable dt2 = await Context.FetchData(assetIDsQry).ConfigureAwait(false);
            List<int> assetIDs = dt2.GetColumn<int>("id");
            List<int> invalidChecklists = new List<int>();
            List<int> invalidAssets = new List<int>();
            foreach (var map in pm_plan.mapAssetChecklist)
            {
                if (!checklistIDs.Contains(map.checklist_id))
                    invalidChecklists.Add(map.checklist_id);
                if (!assetIDs.Contains(map.id))
                    invalidAssets.Add(map.id);
            }
            //if (invalidChecklists.Count > 0 || invalidAssets.Count > 0)
            //    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG,
            //        $"{invalidChecklists.Count} invalid checklists [{string.Join(',', invalidChecklists)}] " +
            //        $"and {invalidAssets.Count} invalid assets [{string.Join(',', invalidAssets)}] linked");

            string addPlanQry = $"INSERT INTO pm_plan(plan_name, facility_id, category_id, frequency_id, " +
                                $"status, plan_date,assigned_to, created_by, created_at, updated_by, updated_at, type_id) VALUES " +
                                $"('{pm_plan.title}', {pm_plan.facility_id}, {pm_plan.category_id}, {pm_plan.plan_freq_id}, " +
                                $"{status}, '{pm_plan.plan_date.ToString("yyyy-MM-dd")}',{pm_plan.assigned_to_id}, " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', {userID}, '{UtilsRepository.GetUTCTime()}', {pm_plan.type_id}); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(addPlanQry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId, typeId) VALUES ";
            foreach (var map in pm_plan.mapAssetChecklist)
            {
                mapChecklistQry += $"({id}, {map.id}, {map.checklist_id}, {pm_plan.type_id}), ";
            }
            mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
            await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, id, 0, 0, "PM Plan added", CMMS.CMMS_Status.PM_PLAN_CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Plan added successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> StartAuditTask(int task_id, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution
             * Add all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            List<int> idList = new List<int>();
            int plan_id = 0;
            int Module_Type_id = 0;
            CMDefaultResponse response;
            string statusQry = $"SELECT pm_task.status,permit.endDate, permit.status as ptw_status,plan_id, " +
                $"(select Module_Type_id from st_audit where id = plan_id) as Module_Type_id FROM pm_task " +
                               $"left join permits as permit on pm_task.PTW_id = permit.id " +
                $"WHERE pm_task.id = {task_id}";


            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            if (dt1.Rows.Count > 0)
            {
                plan_id = Convert.ToInt32(dt1.Rows[0][3]);
                Module_Type_id = Convert.ToInt32(dt1.Rows[0][4]);
            }
            //CMMS.CMMS_Status ptw_status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][2]);

            // string updateQ = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.AUDIT_APPROVED} WHERE id = {task_id};";
            // await Context.ExecuteNonQry<int>(updateQ).ConfigureAwait(false);

            //if (ptw_status == CMMS.CMMS_Status.PTW_APPROVED)
            //{
            //    string updateQ = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.PM_APPROVED} WHERE id = {task_id};";
            //    await Context.ExecuteNonQry<int>(updateQ).ConfigureAwait(false);
            //}
            //else
            //{
            //    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution due to following reasons:" +
            // " \n 1. Permit is not linked to PM" +
            // "\n 2. Permit is not Approved" +
            // " \n 3. Execution has already been started" +
            // " \n 4. Execution is completed");
            //}

            //CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            //if (status !=  CMMS.CMMS_Status.PM_APPROVED)
            //{
            //    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution due to following reasons:" +
            //        " \n 1. Permit is not linked to PM" +
            //        "\n 2. Permit is not Approved" +
            //        " \n 3. Execution has already been started" +
            //        " \n 4. Execution is completed");
            //}
            //DateTime expDate = Convert.ToDateTime(dt1.Rows[0]["endDate"]);

            //if (expDate <= Convert.ToDateTime(UtilsRepository.GetUTCTime()))
            //{
            //    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution : Permit has been expired");
            //}

            string getParamsQry = "SELECT checklist_id, pm_schedule.task_id , pm_schedule.id as schedule_id, plan.frequency_id as frequency_id, plan.facility_id as facility_id, assets.categoryId as category_id, Asset_id as asset_id, PM_Schedule_date as schedule_date " +
                                $"FROM pm_schedule " +
                                $"left join pm_plan as plan on pm_schedule.plan_id = plan.id " +
                                $"left join assets on pm_schedule.Asset_id = assets.id " +
                                $"left join frequency as freq on plan.frequency_id = freq.id where  task_id = {task_id} ";

            List<ScheduleIDData> schedule_details = await Context.GetData<ScheduleIDData>(getParamsQry).ConfigureAwait(false);

            foreach (ScheduleIDData schedule in schedule_details)
            {

                //string facility_ids = string.Join(",", schedule_details.Select(item => $"{item.facility_id}"));
                //string frequency_ids = string.Join(",", schedule_details.Select(item => $"{item.frequency_id}"));
                //string category_ids = string.Join(",", schedule_details.Select(item => $"{item.category_id}"));


                string checkpointsQuery = "SELECT checkpoint.id, checkpoint.check_point, checkpoint.check_list_id as checklist_id, checkpoint.requirement, checkpoint.is_document_required,checkpoint.status " +
                                            "FROM checkpoint " +
                                            "left JOIN checklist_mapping as map ON map.checklist_id = checkpoint.check_list_id " +
                                            "left JOIN checklist_number as checklist ON checklist.id = map.checklist_id " +
                                            //$"WHERE map.facility_id IN ({schedule.facility_id}) AND map.category_id IN ( {schedule.category_id}) AND checklist.frequency_id IN ({schedule.frequency_id});";
                                            $"WHERE checkpoint.check_list_id in ({schedule.checklist_id});";

                List<CMCreateCheckPoint> checkpointList = await Context.GetData<CMCreateCheckPoint>(checkpointsQuery).ConfigureAwait(false);
                if (checkpointList.Count == 0)
                {
                    response = new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "No checklist or checkpoints found");
                }
                else
                {
                    if (Module_Type_id != 5)
                    {
                        foreach (CMCreateCheckPoint checkpoint in checkpointList)
                        {
                            string executeQuery = "INSERT INTO pm_execution(task_id,PM_Schedule_Id, PM_Schedule_Code, Check_Point_id, Check_Point_Name, " +
                                                    "Check_Point_Code, Status, Check_Point_Requirement) VALUES " +
                                                    $"({task_id}, {schedule.schedule_id} ,'PMSCH{schedule.schedule_id}', {checkpoint.id}, " +
                                                    $"'{checkpoint.check_point}', 'CP{checkpoint.id}', 1, '{checkpoint.requirement}'); " +
                                                    $"SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(executeQuery).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt2.Rows[0][0]);
                            idList.Add(id);
                        }
                    }
                    string startQry = $"UPDATE pm_schedule SET PM_Execution_Started_by_id = {userID}, PM_Execution_Started_date = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.PM_START}, status_updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {schedule.schedule_id};";
                    await Context.ExecuteNonQry<int>(startQry).ConfigureAwait(false);
                }
            }


            string startQry2 = $"UPDATE pm_task SET started_by = {userID}, started_at = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.AUDIT_START} WHERE id = {task_id};";
            await Context.ExecuteNonQry<int>(startQry2).ConfigureAwait(false);

            string startQry3 = $"UPDATE st_audit SET  status = {(int)CMMS.CMMS_Status.AUDIT_START} WHERE id = {plan_id};";
            await Context.ExecuteNonQry<int>(startQry3).ConfigureAwait(false);



            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_TASK, task_id, 0, 0, $"Audit Execution Started of assets ", CMMS.CMMS_Status.AUDIT_START, userID);

            CMPMTaskView _AuditList = await GetTaskDetail(task_id, facilitytimeZone);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_TASK, CMMS.CMMS_Status.AUDIT_START, new[] { userID }, _AuditList);

            response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"Execution AuditTask{task_id} Started Successfully");



            return response;
        }

        internal async Task<List<CMDefaultResponse>> UpdateAuditTaskExecution(CMPMExecutionDetail request, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Add or update comment
             * Code goes here
            */
            //string getParamsQry = "SELECT pm_schedule.id as schedule_id, pm_schedule.PM_Frequecy_id as frequency_id, pm_schedule.Facility_id as facility_id, assets.blockId as block_id, pm_schedule.Asset_Category_id as category_id, pm_schedule.Asset_id as asset_id, pm_schedule.PM_Schedule_date as schedule_date " +
            //                        $"FROM pm_schedule " +
            //                        $"JOIN assets ON pm_schedule.Asset_id = assets.id " +
            //                        $"WHERE id = {request.schedule_id};";
            //List<ScheduleIDData> schedule_details = await Context.GetData<ScheduleIDData>(getParamsQry).ConfigureAwait(false);.
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string statusQry = $"SELECT status FROM pm_task WHERE id = {request.task_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status == CMMS.CMMS_Status.PM_SCHEDULED || status == CMMS.CMMS_Status.PM_CLOSE_REJECTED)
            {
                responseList.Add(new CMDefaultResponse(request.task_id, CMMS.RETRUNSTATUS.FAILURE,
                    "Execution must be rejected or in progress to modify execution details"));
                return responseList;
            }
            foreach (var schedule in request.schedules)
            {

                string executeQuery = $"SELECT id FROM pm_execution WHERE PM_Schedule_Id = {schedule.schedule_id};";
                DataTable dt = await Context.FetchData(executeQuery).ConfigureAwait(false);
                List<int> executeIds = dt.GetColumn<int>("id");

                foreach (var schedule_detail in schedule.add_observations)
                {
                    CMDefaultResponse response;
                    if (executeIds.Contains(schedule_detail.execution_id))
                    {
                        int changeFlag = 0;
                        string myQuery1 = "SELECT id as execution_id, PM_Schedule_Observation as observation, job_created as job_create " +
                                            $"FROM pm_execution WHERE id = {schedule_detail.execution_id};";
                        List<AddObservation> execution_details = await Context.GetData<AddObservation>(myQuery1).ConfigureAwait(false);

                        string myQuery2 = $"SELECT checkpoint.type from pm_execution left join checkpoint on pm_execution.check_point_id = checkpoint.id WHERE pm_execution.id = {schedule_detail.execution_id};";

                        DataTable dtType = await Context.FetchData(myQuery2).ConfigureAwait(false);
                        string CPtypeValue = "";
                        if (Convert.ToInt32(dtType.Rows[0][0]) == 0)
                        {
                            CPtypeValue = $" , `text` = '{schedule_detail.text}' ";

                        }

                        else if (Convert.ToInt32(dtType.Rows[0][0]) == 1)
                        {
                            //CPtypeValue = $" , boolean = {schedule_detail.boolean} ";
                            CPtypeValue = $" , boolean = {Convert.ToInt32(schedule_detail.text)} ";

                        }
                        else if (Convert.ToInt32(dtType.Rows[0][0]) == 2)
                        {
                            //CPtypeValue = $" , `range` = {schedule_detail.range} ";
                            CPtypeValue = $" , `range` = {Convert.ToInt32(schedule_detail.text)} ";

                        }

                        CPtypeValue = CPtypeValue + $" , is_ok = {schedule_detail.is_ok} ";
                        if (schedule_detail.observation != null || !schedule_detail.observation.Equals(execution_details[0].observation))
                        {
                            string updateQry = "UPDATE pm_execution SET ";
                            string message;
                            if (execution_details[0].observation == null || execution_details[0].observation == "")
                            {
                                updateQry += $"PM_Schedule_Observation_added_by = {userID}, " +
                                            $"PM_Schedule_Observation_add_date = '{UtilsRepository.GetUTCTime()}', " +
                                            $"PM_Schedule_Observation = '{schedule_detail.observation}' {CPtypeValue} ";
                                message = "CMObservation Added";
                                response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "CMObservation Added Successfully");
                            }
                            else
                            {
                                updateQry += $"PM_Schedule_Observation = '{schedule_detail.observation}', ";
                                updateQry += $"PM_Schedule_Observation_update_by = {userID}, " +
                                            $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' {CPtypeValue} ";
                                message = "CMObservation Updated";
                                response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "CMObservation Updated Successfully");
                            }
                            updateQry += $"WHERE id = {schedule_detail.execution_id};";
                            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, message, CMMS.CMMS_Status.PM_UPDATED, userID);
                            responseList.Add(response);
                            changeFlag++;
                        }
                        if (schedule_detail.job_create == 1 && execution_details[0].job_create == 0)
                        {

                            string facilityQry = $"SELECT pm_task.facility_id as block, CASE WHEN facilities.parentId=0 THEN facilities.id ELSE facilities.parentId END AS parent " +
                                                $"FROM pm_schedule Left join pm_task on pm_task.id = pm_schedule.task_id LEFT JOIN facilities ON pm_task.facility_id=facilities.id " +
                                                $"WHERE pm_schedule.id = {schedule.schedule_id}  group by pm_task.id ;";
                            DataTable dtFacility = await Context.FetchData(facilityQry).ConfigureAwait(false);
                            string titleDescQry = $"SELECT CONCAT('PMSCH{schedule.schedule_id}', Check_Point_Name, ': ', Check_Point_Requirement) as title, " +
                                                    $"PM_Schedule_Observation as description FROM pm_execution WHERE id = {schedule_detail.execution_id};";
                            DataTable dtTitleDesc = await Context.FetchData(titleDescQry).ConfigureAwait(false);
                            string assetsPMQry = $"SELECT Asset_id FROM pm_schedule WHERE id = {schedule.schedule_id};";
                            DataTable dtPMAssets = await Context.FetchData(assetsPMQry).ConfigureAwait(false);
                            CMCreateJob newJob = new CMCreateJob()
                            {
                                title = dtTitleDesc.GetColumn<string>("title")[0],
                                description = dtTitleDesc.GetColumn<string>("description")[0],
                                facility_id = dtFacility.GetColumn<int>("parent")[0],
                                block_id = dtFacility.GetColumn<int>("block")[0],
                                breakdown_time = DateTime.UtcNow,
                                AssetsIds = dtPMAssets.GetColumn<int>("Asset_id"),
                                jobType = CMMS.CMMS_JobType.PreventiveMaintenance
                            };
                            CMDefaultResponse jobResp = await CreateNewJob(newJob, userID);
                            string updateQry = $"UPDATE pm_execution SET job_created = 1, linked_job_id = {jobResp.id[0]} WHERE id = {schedule_detail.execution_id};";
                            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"Job {jobResp.id[0]} Created for PM", CMMS.CMMS_Status.PM_UPDATED, userID);
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {jobResp.id[0]} Created for PM Successfully");
                            responseList.Add(response);
                            changeFlag++;
                        }
                        if (schedule_detail.pm_files != null)
                        {
                            if (schedule_detail.pm_files.Count != 0)
                            {
                                foreach (var file in schedule_detail.pm_files)
                                {
                                    string checkEventFiles = $"SELECT * FROM pm_schedule_files " +
                                                                $"WHERE PM_Schedule_id = {schedule.schedule_id} " +
                                                                $"AND PM_Execution_id = {schedule_detail.execution_id} " +
                                                                $"AND PM_Event = {(int)file.pm_event}";
                                    DataTable dt2 = await Context.FetchData(checkEventFiles).ConfigureAwait(false);

                                    if (dt2.Rows.Count > 0)
                                    {
                                        string deleteQry = "DELETE FROM pm_schedule_files " +
                                                                $"WHERE PM_Schedule_id = {schedule.schedule_id} " +
                                                                $"AND PM_Execution_id = {schedule_detail.execution_id} " +
                                                                $"AND PM_Event = {(int)file.pm_event}";
                                        await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                                    }
                                    string insertFile = "INSERT INTO pm_schedule_files(PM_Schedule_id, PM_Schedule_Code, PM_Execution_id, File_id, PM_Event, " +
                                                        "File_Discription, File_added_by, File_added_date, File_Server_id, File_Server_Path) VALUES " +
                                                        $"({schedule.schedule_id}, 'PMSCH{schedule.schedule_id}', {schedule_detail.execution_id}, {file.file_id}, " +
                                                        $"{(int)file.pm_event}, '{file.file_desc}', {userID}, '{UtilsRepository.GetUTCTime()}', 1, " +
                                                        $"'http://cms_test.com/' ); SELECT LAST_INSERT_ID();";
                                    DataTable dt3 = await Context.FetchData(insertFile).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt3.Rows[0][0]);
                                    string otherDetailsQry = "UPDATE pm_schedule_files as pmf " +
                                                                "JOIN uploadedfiles as f ON pmf.File_id = f.id " +
                                                                "JOIN pm_schedule as pms ON pms.id = pmf.PM_Schedule_id " +
                                                                "JOIN pm_execution as pme ON pme.id = pmf.PM_Execution_id " +
                                                             "SET " +
                                                                "pmf.PM_Schedule_Title = pms.PM_Schedule_Name, " +
                                                                "pmf.Check_Point_id = pme.Check_Point_id, " +
                                                                "pmf.Check_Point_Code = pme.Check_Point_Code, " +
                                                                "pmf.Check_Point_Name = pme.Check_Point_Name, " +
                                                                "pmf.File_Name = SUBSTRING_INDEX(f.file_path, '\\\\', -1), " +
                                                                "pmf.File_Path = f.file_path, " +
                                                                "pmf.File_Type_name = f.file_type, " +
                                                                "pmf.File_Size = f.file_size, " +
                                                                "pmf.File_Size_Units = f.file_size_units, " +
                                                                "pmf.File_Size_bytes = f.file_size_bytes, " +
                                                                "pmf.File_Type_ext = SUBSTRING_INDEX(f.file_path, '.', -1) " +
                                                             $"WHERE pmf.id = {id};";
                                    await Context.ExecuteNonQry<int>(otherDetailsQry).ConfigureAwait(false);
                                }
                                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"{schedule_detail.pm_files.Count} file(s) attached to PMSCH{schedule.schedule_id}", CMMS.CMMS_Status.PM_UPDATED, userID);
                                response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"{schedule_detail.pm_files.Count} file(s) attached to PM Successfully");
                                responseList.Add(response);
                                changeFlag++;
                            }
                        }
                        if (changeFlag == 0)
                        {
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "No changes");
                            responseList.Add(response);
                        }
                    }
                    else
                    {
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id} not associated with PMSCH{schedule.schedule_id}");
                        responseList.Add(response);
                    }
                }

            }
            string taskQry = $"update pm_task set updated_by = {userID},updated_at = '{UtilsRepository.GetUTCTime()}', update_remarks = '{request.comment}' WHERE id = {request.task_id};";
            int retVal = await Context.ExecuteNonQry<int>(taskQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_TASK, request.task_id, 0, 0, string.IsNullOrEmpty(request.comment) ? "Audit Task Updated" : request.comment, CMMS.CMMS_Status.PM_UPDATED, userID);

            CMDefaultResponse responseResult = new CMDefaultResponse(request.task_id, retCode, "Audit Task Updated successfully");
            responseList.Add(responseResult);

            return responseList;
        }

        internal async Task<CMDefaultResponse> CreateNewJob(CMCreateJob request, int userId)
        {
            /*
             * Job basic details will go to Job table
             * Job associated assets and category will go in JobMappingAssets table (one to many relation)
             * Job associated work type will go in JobAssociatedWorkTypes
             * return value will be inserted record. Use GetJobDetail() function
            */
            //if(request.AssetsIds.Count > 10)
            //    throw new 
            /*Your code goes here*/
            int status = ((int)CMMS.CMMS_Status.JOB_CREATED);
            if (request.assigned_id > 0)
            {
                status = ((int)CMMS.CMMS_Status.JOB_ASSIGNED);
            }
            //int created_by = Utils.UtilsRepository.GetUserID();
            if (request.jobType == null)
                request.jobType = CMMS.CMMS_JobType.BreakdownMaintenance;
            string qryJobBasic = "insert into jobs(facilityId, blockId, title, description, statusUpdatedAt, createdAt, createdBy, breakdownTime, JobType, status, assignedId, linkedPermit) values" +
            $"({request.facility_id}, {request.block_id}, '{request.title}', '{request.description}', '{UtilsRepository.GetUTCTime()}','{UtilsRepository.GetUTCTime()}',{userId},'{request.breakdown_time.ToString("yyyy-MM-dd HH:mm:ss")}',{(int)request.jobType},{status},{request.assigned_id},{(request.permit_id == null ? 0 : request.permit_id)})";
            qryJobBasic = qryJobBasic + ";" + "select LAST_INSERT_ID(); ";

            DataTable dt = await Context.FetchData(qryJobBasic).ConfigureAwait(false);
            int newJobID = Convert.ToInt32(dt.Rows[0][0]);

            //List<CMJobView> newJob = await Context.GetData<CMJobView>(qry).ConfigureAwait(false);
            //            int newJobID = newJob[0].id;
            if (request.AssetsIds == null)
                request.AssetsIds = new List<int>();

            foreach (var data in request.AssetsIds)
            {
                string qryAssetsIds = $"insert into jobmappingassets(jobId, assetId ) values ({newJobID}, {data});";
                await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            }
            string setCat = $"UPDATE jobmappingassets, assets SET jobmappingassets.categoryId = assets.categoryId WHERE jobmappingassets.assetId = assets.id;";
            await Context.ExecuteNonQry<int>(setCat).ConfigureAwait(false);
            if (request.WorkType_Ids == null)
                request.WorkType_Ids = new List<int>();

            foreach (var data in request.WorkType_Ids)
            {
                string qryCategoryIds = $"insert into jobassociatedworktypes(jobId, workTypeId ) value ( {newJobID}, {data} );";
                await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
            }


            CMJobView _ViewJobList = await GetJobDetails(newJobID, "");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Created", CMMS.CMMS_Status.JOB_CREATED, userId);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CREATED, new[] { userId }, _ViewJobList);

            string strJobStatusMsg = $"Job {newJobID} Created";
            if (_ViewJobList.assigned_id > 0)
            {
                strJobStatusMsg = $"Job {newJobID} Created and Assigned to " + _ViewJobList.assigned_name;

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Assigned", CMMS.CMMS_Status.JOB_ASSIGNED, userId);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_ASSIGNED, new[] { userId }, _ViewJobList);
            }

            // File Upload code for JOB
            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={newJobID} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            CMDefaultResponse response = new CMDefaultResponse(newJobID, CMMS.RETRUNSTATUS.SUCCESS, strJobStatusMsg);

            return response;
        }

        internal async Task<CMJobView> GetJobDetails(int job_id, string facilitytimeZone)
        {
            /*
             * Fetch data from Job table and joins these table for relationship using ids Users, Assets, AssetCategory, Facility
             * id and it string value should be there in list
            */

            /*Your code goes here*/

            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as facility_id, facilities.name as facility_name, blocks.id as block_id, blocks.name as block_name, job.status as status, job.createdAt as created_at,created_user.id as created_by_id, CONCAT(created_user.firstName, created_user.lastName) as created_by_name, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, job.title as job_title, job.description as job_description, job.breakdownTime as breakdown_time, ptw.id as current_ptw_id, ptw.title as current_ptw_title, ptw.description as current_ptw_desc, jc.id as latestJCid, jc.JC_Status as latestJCStatus, jc.JC_Approved as latestJCApproval " +
                                      "FROM " +
                                            "jobs as job " +
                                      "LEFT JOIN " +
                                            "jobcards as jc ON job.latestJC = jc.id " +
                                      " LEFT JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "LEFT JOIN " +
                                            "facilities as blocks ON job.blockId = blocks.id " +
                                      "LEFT JOIN " +
                                            "permits as ptw ON job.linkedPermit = ptw.id " +
                                      "LEFT JOIN " +
                                            "users as created_user ON created_user.id = job.createdby " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId " +
                                      "WHERE job.id = " + job_id;
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, (CMMS.CMMS_Status)_ViewJobList[0].status);
            _ViewJobList[0].status_short = _shortStatus;
            _ViewJobList[0].breakdown_type = $"{(CMMS.CMMS_JobType)_ViewJobList[0].job_type}";

            //get equipmentCat list
            string myQuery1 = "SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat " +
                "JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMequipmentCatList> _equipmentCatList = await Context.GetData<CMequipmentCatList>(myQuery1).ConfigureAwait(false);

            //get workingArea_name list 
            string myQuery2 = "SELECT asset.id as workingArea_id, asset.name as workingArea_name FROM assets as asset " +
             "JOIN jobmappingassets as mapAssets ON mapAssets.assetId  =  asset.id  JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMworkingAreaNameList> _WorkingAreaNameList = await Context.GetData<CMworkingAreaNameList>(myQuery2).ConfigureAwait(false);

            //get Associated permits
            string myQuery3 = "SELECT ptw.id as permitId,ptw.status as ptwStatus ,ptw.title as title, ptw.code as sitePermitNo, ptw.startDate, ptw.endDate,  CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, ptw.status as ptwStatus FROM permits as ptw JOIN jobs as job ON ptw.id = job.linkedPermit " +
                "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                "LEFT JOIN st_jc_files as jobCard ON jobCard.JC_id = ptw.id " +
            "WHERE job.id= " + job_id;
            List<CMAssociatedPermitList> _AssociatedpermitList = await Context.GetData<CMAssociatedPermitList>(myQuery3).ConfigureAwait(false);
            if (_AssociatedpermitList.Count > 0)
            {
                _AssociatedpermitList[0].ptwStatus_short = "Linked";    //temp till JOIN is made
            }
            string myQuery4 = "SELECT workType.id AS workTypeId, workType.workTypeName as workTypeName FROM jobs AS job " +
                "left JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
                "left JOIN assetcategories AS asset_cat ON mapAssets.categoryId = asset_cat.id " +
                "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
                "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
                $"WHERE job.id = {job_id} ";
            List<CMWorkType> _WorkType = await Context.GetData<CMWorkType>(myQuery4).ConfigureAwait(false);

            string myQuery5 = "SELECT tools.id as toolId, tools.assetName as toolName FROM jobs AS job " +
                "JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
                "JOIN assets ON mapAssets.assetId = assets.id " +
                "JOIN assetcategories AS asset_cat ON assets.categoryId = asset_cat.id " +
                "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
                "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
                "LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId=workType.id " +
                "LEFT JOIN worktypemasterassets AS tools ON tools.id=mapTools.ToolId " +
                $"WHERE job.id = {job_id} GROUP BY tools.id";
            List<CMWorkTypeTool> _Tools = await Context.GetData<CMWorkTypeTool>(myQuery5).ConfigureAwait(false);

            if (_ViewJobList[0].current_ptw_id == 0)
            {
                _ViewJobList[0].latestJCStatusShort = "Permit not linked";
            }
            else if (_ViewJobList[0].latestJCid != 0)
            {
                //if permit status is not yet approved
                if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                {
                    _ViewJobList[0].latestJCStatusShort = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_ViewJobList[0].latestJCStatus, (CMMS.ApprovalStatus)_ViewJobList[0].latestJCApproval);
                }
                else if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - rejected";
                }
                else
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - Waiting For Approval";
                }
            }
            else
            {
                if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                {

                    _ViewJobList[0].latestJCStatusShort = "Permit - Approved";
                }
                else if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - rejected";
                }
                else
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - Pending";
                }
            }

            _ViewJobList[0].equipment_cat_list = _equipmentCatList;
            _ViewJobList[0].working_area_name_list = _WorkingAreaNameList;
            _ViewJobList[0].associated_permit_list = _AssociatedpermitList;
            _ViewJobList[0].work_type_list = _WorkType;
            _ViewJobList[0].tools_required_list = _Tools;
            //add worktype and tools ka collection
            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewJobList[0].status + 100);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOB, _Status_long, _ViewJobList[0]);
            _ViewJobList[0].status_long = _longStatus;

            foreach (var vlt in _ViewJobList)
            {
                if (vlt.breakdown_time != null && vlt.breakdown_time != null)
                    vlt.breakdown_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)vlt.breakdown_time);
                if (vlt.Job_closed_on != null && vlt.Job_closed_on != null)
                    vlt.Job_closed_on = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, vlt.Job_closed_on);
                if (vlt.created_at != null && vlt.created_at != null)
                    vlt.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)vlt.created_at);




            }
            return _ViewJobList[0];
        }
        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJobView jobObj)
        {
            string retValue = "Job";
            int jobId = jobObj.id;

            switch (notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                                                       //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    string desc = jobObj.job_description;
                    if (string.IsNullOrEmpty(jobObj.assigned_name))
                    {
                        retValue = String.Format("Job {0} created by", jobObj.created_by_name);
                    }
                    else
                    {
                        retValue = String.Format("Job {0} Created by and Assigned to", jobObj.created_by_name, jobObj.assigned_name);
                    }
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = String.Format("Job <{0}> assigned to <{1}>", jobObj.job_title, jobObj.assigned_name);
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = String.Format("Job <{0}> linked to PTW <{1}>", jobObj.job_title, jobObj.current_ptw_id);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = String.Format("Job <{0}> closed", jobObj.job_title);
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = String.Format("Job <{0}> Cancelled", jobObj.job_title);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userId)
        {

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }
            string approveQuery = $"Update pm_plan set status = {(int)CMMS.CMMS_Status.AUDIT_APPROVED}, approved_at = '{UtilsRepository.GetUTCTime()}', " +
                $" remarks = '{request.comment}',  " +
                $" approved_by = {userId}" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string mainQuery = $"INSERT INTO pm_task(plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to,status)  " +
                               $"select id as plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to," +
                               $"CASE WHEN assigned_to = '' or assigned_to IS NULL THEN {(int)CMMS.CMMS_Status.PM_SCHEDULED} " +
                               $"ELSE {(int)CMMS.CMMS_Status.PM_ASSIGNED} END as status " +
                               $"from pm_plan where id = {request.id}; " +
                               $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                $"select {id} as task_id,planId as plan_id, assetId as Asset_id, checklistId as checklist_id,PP.plan_date  as PM_Schedule_date,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status from pmplanassetchecklist  P inner join pm_plan PP on PP.Id = P.planId where planId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "Audit Plan Approved " : request.comment, CMMS.CMMS_Status.AUDIT_APPROVED, userId);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.APPROVED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "PM Plan Approved Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userId)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string approveQuery = $"Update pm_plan set status = {(int)CMMS.CMMS_Status.AUDIT_REJECTED} , " +
                $"remarks = '{request.comment}',  " +
                $"rejected_by = {userId}, rejected_at = '{UtilsRepository.GetUTCTime()}' " +
                $" where id = {request.id} ";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Plan Rejected " : request.comment, CMMS.CMMS_Status.PM_PLAN_REJECTED, userId);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "PM Plan Rejected Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePlan(int planId, int userID)
        {
            string approveQuery = $"update pm_plan set status_id = 0 where id = {planId}; ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, planId, 0, 0, $"PM Plan Deleted by user {userID}", CMMS.CMMS_Status.PM_PLAN_DELETED);

            CMDefaultResponse response = new CMDefaultResponse(planId, CMMS.RETRUNSTATUS.SUCCESS, $" PM Plan Deleted");
            return response;
        }

        internal async Task<List<CMPMPlanList>> GetAuditPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytimeZone)
        {
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            string planListQry = $"SELECT plan.id as plan_id, plan.plan_name, plan.status as status_id, statuses.statusName as status_short, plan.plan_date, " +
                                    $"facilities.id as facility_id, facilities.name as facility_name, category.id as category_id, category.name as category_name, " +
                                    $"frequency.id as plan_freq_id, frequency.name as plan_freq_name, createdBy.id as created_by_id, " +
                                    $"CONCAT(createdBy.firstName, ' ', createdBy.lastName) as created_by_name, plan.created_at,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assigned_to_name, " +
                                    $"updatedBy.id as updated_by_id, CONCAT(updatedBy.firstName, ' ', updatedBy.lastName) as updated_by_name, plan.updated_at " +
                                    $"FROM pm_plan as plan " +
                                    $"LEFT JOIN statuses ON plan.status = statuses.softwareId " +
                                    $"JOIN facilities ON plan.facility_id = facilities.id " +
                                    $"LEFT JOIN assetcategories as category ON plan.category_id = category.id " +
                                    $"LEFT JOIN frequency ON plan.frequency_id = frequency.id " +
                                    $"LEFT JOIN users as createdBy ON createdBy.id = plan.created_by " +
                                    $"LEFT JOIN users as updatedBy ON updatedBy.id = plan.updated_by " +
                                    $"LEFT JOIN users as assignedTo ON assignedTo.id = plan.assigned_to " +
                                    $"WHERE facilities.id = {facility_id}  ";

            if (category_id != null && category_id != "")
                planListQry += $"AND category.id IN ( {category_id} )";
            if (frequency_id != null && category_id != "")
                planListQry += $"AND frequency.id IN ( {frequency_id} )";
            if (start_date != null)
                planListQry += $"AND plan.plan_date >= '{((DateTime)start_date).ToString("yyyy-MM-dd")}' ";
            if (end_date != null)
                planListQry += $"AND plan.plan_date <= '{((DateTime)end_date).ToString("yyyy-MM-dd")}' ";
            planListQry += $";";

            List<CMPMPlanList> plan_list = await Context.GetData<CMPMPlanList>(planListQry).ConfigureAwait(false);

            foreach (var plan in plan_list)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(plan.status_id);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                plan.status_short = _shortStatus;
            }
            foreach (var list in plan_list)
            {
                if (list != null && list.approved_at != null)
                    list.approved_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.approved_at);
                if (list.created_at != null && list.created_at != null)
                    list.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.created_at);
                if (list.plan_date != null && list.plan_date != null)
                    list.plan_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.plan_date);
                if (list.rejected_at != null && list.rejected_at != null)
                    list.rejected_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.rejected_at);
                if (list.updated_at != null && list.updated_at != null)
                    list.updated_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.updated_at);

            }
            return plan_list;
        }

        internal async Task<CMPMPlanDetail> GetAuditPlanDetail(int id, string facilitytimeZone)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            string planListQry = $"SELECT plan.id as plan_id, plan.title title, plan.status as status_id, statuses.statusName as status_short, plan.Audit_Added_date plan_date,plan.description, plan.checklist_id,Employees, " +
                " checklist_number as checklist_name,plan.Schedule_Date,plan.max_score, " +
                $" facilities.id as facility_id, facilities.name as facility_name," +
                $" frequency.id as plan_freq_id, frequency.name as plan_freq_name, createdBy.id as created_by_id, " +
                $" CONCAT(createdBy.firstName, ' ', createdBy.lastName) as created_by_name, plan.created_at,approvedBy.id as approved_by_id," +
                $" CONCAT(approvedBy.firstName, ' ', approvedBy.lastName) as approved_by_name, plan.approved_Date approved_at, rejectedBy.id as rejected_by_id, " +
                $" CONCAT(rejectedBy.firstName, ' ', rejectedBy.lastName) as rejected_by_name, plan.rejected_Date rejected_at," +
                $" CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assigned_to_name," +
                $" CONCAT(rejected_close_by.firstName, ' ', rejected_close_by.lastName) as rejected_close_by_name," +
                $" CONCAT(approved_close_by.firstName, ' ', approved_close_by.lastName) as approved_close_by_name," +
                $" CONCAT(close_by.firstName, ' ', close_by.lastName) as close_by_name, " +
                $" CONCAT(deleted_by.firstName, ' ', deleted_by.lastName) as deleted_by_name,plan.deleted_date, " +
                $" close_comment, close_Date,approved_close_Date,rejected_close_Date" +
                $" FROM st_audit as plan " +
                $" LEFT JOIN statuses ON plan.status = statuses.softwareId " +
                $" JOIN facilities ON plan.facility_id = facilities.id " +
                $" LEFT JOIN frequency ON plan.Frequency = frequency.id " +
                $" LEFT JOIN users as createdBy ON createdBy.id = plan.created_by " +
                $" LEFT JOIN users as approvedBy ON approvedBy.id = plan.approved_by " +
                $" LEFT JOIN checklist_number checklist_number on checklist_number.id = plan.Checklist_id " +
                $" LEFT JOIN users as rejectedBy ON rejectedBy.id = plan.rejected_by " +
                $" LEFT JOIN users as assignedTo ON assignedTo.id = plan.Auditor_Emp_ID " +
                $" LEFT JOIN users as rejected_close_by ON rejected_close_by.id = plan.rejected_close_by " +
                $" LEFT JOIN users as approved_close_by ON approved_close_by.id = plan.approved_close_by " +
                $" LEFT JOIN users as close_by ON close_by.id = plan.close_by " +
                $" LEFT JOIN users as deleted_by ON deleted_by.id = plan.deleted_by " +
                $"WHERE plan.id = {id} ";

            List<CMPMPlanDetail> planDetails = await Context.GetData<CMPMPlanDetail>(planListQry).ConfigureAwait(false);

            if (planDetails.Count == 0)
                return null;

            //string assetChecklistsQry = $"SELECT assets.id as asset_id, assets.name as asset_name, parent.id as parent_id, parent.name as parent_name, assets.moduleQuantity as module_qty, checklist.id as checklist_id, checklist.checklist_number as checklist_name " +
            //                            $"FROM pmplanassetchecklist as planmap " +
            //                            $"LEFT JOIN assets ON assets.id = planmap.assetId " +
            //                            $"LEFT JOIN assets as parent ON assets.parentId = parent.id " +
            //                            $"LEFT JOIN checklist_number as checklist ON checklist.id = planmap.checklistId " +
            //                            $"WHERE planmap.planId = {id};";

            string assetChecklistsQry = $"SELECT checklist.id as checklist_id, checklist.checklist_number as checklist_name " +
                $" FROM st_audit as planmap " +
                $" LEFT JOIN checklist_number as checklist ON checklist.id = planmap.Checklist_id " +
                $" WHERE planmap.id = {id};";
            List<AssetCheckList> assetCheckLists = await Context.GetData<AssetCheckList>(assetChecklistsQry).ConfigureAwait(false);
            planDetails[0].mapAssetChecklist = assetCheckLists;

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(planDetails[0].status_id);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
            planDetails[0].status_short = _shortStatus;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(planDetails[0].status_id);

            CMPMTaskView m_AuditObj = new CMPMTaskView();
            string _longStatus = getLongStatus_Details(CMMS.CMMS_Modules.AUDIT_PLAN, _Status_long, planDetails[0], m_AuditObj);
            planDetails[0].status_long = _longStatus;
            foreach (var pd in planDetails)
            {
                if (pd != null && pd.approved_at != null)
                    pd.approved_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, pd.approved_at);
                if (pd != null && pd.created_at != null)
                    pd.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, pd.created_at);
                if (pd != null && pd.plan_date != null)
                    pd.plan_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, pd.plan_date);
                if (pd != null && pd.rejected_at != null)
                    pd.rejected_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, pd.rejected_at);



            }
            return planDetails[0];
        }
        internal string getLongStatus_Details(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMPlanDetail PlanObj, CMPMTaskView m_AuditObj)
        {
            string retValue = " ";


            switch (notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_SCHEDULE:
                    retValue = String.Format("AUD{0} Created by {1}", PlanObj.plan_id, PlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_START:
                    retValue = String.Format("AUD{0} submitted by {1}", PlanObj.plan_id, PlanObj.started_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_REJECTED:
                    retValue = String.Format("AUD{0} Rejected by {1}", PlanObj.plan_id, PlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_APPROVED:
                    retValue = String.Format("AUD{0} Approved by {1}", PlanObj.plan_id, PlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_DELETED:
                    retValue = String.Format("AUD{0} Deleted by {1}", PlanObj.plan_id, PlanObj.deleted_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED:
                    retValue = String.Format("AUD{0} Closed by {1}", PlanObj.plan_id, PlanObj.close_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP:
                    retValue = String.Format("AUD{0} Skip by {1}", m_AuditObj.id, m_AuditObj.skip_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_REJECT:
                    retValue = String.Format("AUD{0} Skip Rejected by {1}", PlanObj.plan_id);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_APPROVED:
                    retValue = String.Format("AUD{0} Skip Approved by {1}", PlanObj.plan_id);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED:
                    retValue = String.Format("AUD{0} Close Approved by {1}", PlanObj.plan_id, PlanObj.approved_close_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_REJECT:
                    retValue = String.Format("AUD{0} Close Rejected by {1}", PlanObj.plan_id, PlanObj.rejected_close_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_EXECUTED:
                    retValue = String.Format("AUD{0} Executed by {1}", PlanObj.plan_id);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("AUD{0} Linked With Permit by {1}", PlanObj.plan_id);
                    break;
                default:
                    break;
            }
            return retValue;

        }


        internal async Task<List<CMPMTaskList>> GetTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string facilitytimeZone, int module_type_id)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */

            //string statusQry = "CASE ";
            //foreach (KeyValuePair<CMMS.CMMS_Status, string> status in statusList)
            //    statusQry += $"WHEN pm_schedule.status = {(int)status.Key} THEN '{status.Value}' ";
            //statusQry += "ELSE 'Unknown Status' END";

            string myQuery = $"SELECT pm_task.id,pm_task.category_id,'' as category_name,  CONCAT('AuditTASK',pm_task.id) as task_code, " +
                              $"st_audit.title as plan_title,pm_task.facility_id, pm_task.frequency_id as frequency_id, " +
                              $"freq.name as frequency_name, pm_task.plan_date as due_date,prev_task_done_date as last_done_date, " +
                              $"closed_at as done_date, CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assigned_to_name, " +
                              $"pm_task.PTW_id as permit_id, CONCAT('PTW',pm_task.PTW_id) as permit_code, st_audit.status as status_plan, " +
                              $"pm_task.Status status  " +
                              "FROM pm_task " +
                               $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                               $"left join st_audit  on pm_task.plan_id = st_audit.id " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id Where  module_type_id = " + module_type_id + "  AND (parent_task_id = 0 OR parent_task_id IS NULL) ";
            // myQuery += (frequencyIds.Length > 0 ? " AND freq.id IN ( '" + string.Join("' , '", frequencyIds) + "' )" : string.Empty);

            List<CMPMTaskList> scheduleViewList = new List<CMPMTaskList>();
            if (facility_id > 0)
            {
                myQuery += $" and pm_task.Facility_id = {facility_id} ";
                if (start_date != null && end_date != null)
                {
                    string start = ((DateTime)start_date).ToString("yyyy'-'MM'-'dd");
                    myQuery += $"AND pm_task.plan_date >= '{start}' ";
                    string end = ((DateTime)end_date).ToString("yyyy'-'MM'-'dd");
                    myQuery += $"AND pm_task.plan_date <= '{end}' ";
                }
                //if (categories.Count > 0)
                //{
                //    string catList = string.Join(", ", categories);
                //    myQuery += $"AND Asset_Category_id in ({catList}) ";
                //}
                if (frequencyIds != "" && frequencyIds != null)
                {

                    myQuery += $"AND pm_task.frequency_id in ({frequencyIds}) ";
                }
                scheduleViewList = await Context.GetData<CMPMTaskList>(myQuery).ConfigureAwait(false);
                foreach (var task in scheduleViewList)
                {
                    if (task.status == (int)CMMS.CMMS_Status.PM_LINKED_TO_PTW)
                    {
                        if (task.ptw_status == (int)CMMS.CMMS_Status.PTW_APPROVED)
                        {
                            string startQry2 = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.AUDIT_APPROVED} WHERE id = {task.id};";
                            await Context.ExecuteNonQry<int>(startQry2).ConfigureAwait(false);
                        }
                        CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(task.ptw_status);
                        task.status_short = "Audit - " + getShortStatus(CMMS.CMMS_Modules.AUDIT_SCHEDULE, _Status);
                    }
                    else
                    {
                        CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(task.status);
                        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                        task.status_short = _shortStatus;
                    }
                    CMMS.CMMS_Status _Status_plan = (CMMS.CMMS_Status)(task.status_plan);
                    string _shortStatus_plan = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status_plan);
                    task.status_plan_short = _shortStatus_plan;

                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }

            return scheduleViewList;
        }


        internal async Task<CMPMTaskView> GetTaskDetail(int task_id, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Other supporting tables - Facility, Asset, AssetCategory, Users
             * Read All properties mention in model and return list
             * Code goes here
            */
            if (task_id <= 0)
                throw new ArgumentException("Invalid Task ID");

            string statusQry = "CASE ";
            foreach (KeyValuePair<CMMS.CMMS_Status, string> status in statusList)
                statusQry += $"WHEN pm_task.Status = {(int)status.Key} THEN '{status.Value}' ";
            statusQry += "ELSE 'Unknown Status' END ";

            string eventQry = "CASE ";
            foreach (CMMS.CMMS_Events _event in Enum.GetValues(typeof(CMMS.CMMS_Events)))
            {
                eventQry += $"WHEN pm_schedule_files.PM_Event = {(int)_event} THEN '{_event}' ";
            }
            eventQry += "ELSE 'Unknown Event' END ";

            //string myQuery1 = $"SELECT id, PM_Maintenance_Order_Number as maintenance_order_number, PM_Schedule_date as schedule_date,
            //PM_Schedule_Completed_date as completed_date, Asset_id as equipment_id, Asset_Name as equipment_name,
            //Asset_Category_id as category_id, Asset_Category_name as category_name, PM_Frequecy_id as frequency_id,
            //PM_Frequecy_Name as frequency_name, PM_Schedule_Emp_name as assigned_to_name, PTW_id as permit_id, status,
            //{statusQry} as status_name, Facility_id as facility_id, Facility_Name as facility_name " +
            //                    $"FROM pm_schedule WHERE id = {schedule_id};";
            //  COALESCE(pm_task.title, st_audit.title) AS plan_title1,
            string myQuery = $"SELECT pm_plan.Schedule_Date,pm_task.parent_task_id,pm_plan.title as plan_title, pm_task.id,pm_plan.id as plan_id, " +
                             $"CONCAT('AUDITTASK',pm_task.id) as task_code,  " +
                            $"pm_task.category_id,cat.name as category_name, pm_task.facility_id, pm_task.frequency_id as frequency_id, freq.name as frequency_name, pm_task.plan_date as due_date,   " +
                            $"prev_task_done_date as done_date, pm_task.assigned_to_audit  as assigned_to_name, CONCAT(closedBy.firstName,' ',closedBy.lastName)  as closed_by_name, pm_task.closed_at , " +
                $"CONCAT(approvedBy.firstName,' ',approvedBy.lastName)  as closedApprovedByName, pm_task.approved_at ," +
                $"facilities.name as facility_name,pm_plan.max_score," +
                $"CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assignedTo," +
                $"CONCAT(rejectedBy.firstName,' ',rejectedBy.lastName)  as closeRejectedbyName,checklist_number as checklist_name, pm_task.rejected_at ," +
                $"CONCAT(cancelledBy.firstName,' ',cancelledBy.lastName)  as cancelled_by_name, pm_task.cancelled_at , pm_task.rejected_at ," +
                $"CONCAT(startedBy.firstName,' ',startedBy.lastName)  as started_by_name, pm_task.started_at , pm_task.PTW_id as permit_id, " +
                $"CONCAT(skip_by.firstName,' ',skip_by.lastName)  as skip_by_name, pm_task.skip_date, " +
                $"CONCAT(Skip_rejected_by.firstName,' ',Skip_rejected_by.lastName)  as skip_rejected_by_name, pm_task.Skip_rejected_Date, " +
                $"CONCAT(skip_approved_by.firstName,' ',skip_approved_by.lastName)  as skip_approved_by_name, pm_task.skip_approved_at, " +
                $"CONCAT('PTW',pm_task.PTW_id) as permit_code,permit.status as ptw_status,ptype.title as  permit_type, " +
                $"pm_task.Status as status, {statusQry} as status_short " + ",  " +
                               "CONCAT(tbtDone.firstName,' ',tbtDone.lastName)  as tbt_by_name, Case when permit.TBT_Done_By is null  then 0 else  permit.TBT_Done_By end ptw_tbt_done, pm_plan.Employees employee_list,case when pm_plan.is_PTW = 1 then 'True' else 'False' end is_PTW" +
                               " FROM pm_task " +
                               $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                               $"left join users as closedBy on pm_task.closed_by = closedBy.id " +
                               $"left join users as approvedBy on pm_task.approved_by = approvedBy.id " +
                               $"left join users as rejectedBy on pm_task.rejected_by = rejectedBy.id " +
                               $"left join users as cancelledBy on pm_task.cancelled_by = cancelledBy.id " +
                               $"left join users as startedBy on pm_task.started_by = startedBy.id " +
                               $"left join users as skip_by on pm_task.skip_by = skip_by.id " +
                               $"left join users as Skip_rejected_by on pm_task.Skip_rejected_by = Skip_rejected_by.id " +
                               $"left join users as skip_approved_by on pm_task.skip_approved_by = skip_approved_by.id " +
                               $"left join permits as permit on pm_task.PTW_id = permit.id " +
                               $"left join permittypelists as ptype on ptype.id = permit.typeId " +
                               $"left join st_audit as pm_plan  on pm_task.plan_id = pm_plan.id " +
                               $"left join assetcategories as cat  on pm_task.category_id = cat.id " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id " +
                               $"  left join users as tbtDone on permit.TBT_Done_By = tbtDone.id " +
                               $" JOIN facilities ON pm_task.facility_id = facilities.id " +
                               $" LEFT JOIN checklist_number checklist_number on checklist_number.id = pm_plan.Checklist_id " +
                               $" where pm_task.id = {task_id}  ";

            List<CMPMTaskView> taskViewDetail = await Context.GetData<CMPMTaskView>(myQuery).ConfigureAwait(false);

            for (var i = 0; i < taskViewDetail.Count; i++)
            {
                taskViewDetail[i].Employees = taskViewDetail[i].employee_list.Split(',').ToList();
            }

            if (taskViewDetail.Count == 0)
                throw new MissingMemberException("PM Task not found");

            string myQuery2 = $"SELECT pm_schedule.id as schedule_id,assets.id as assetsID, assets.name as asset_name, " +
                $"checklist.checklist_number as checklist_name from pm_schedule " +
                $"left join assets on pm_schedule.asset_id = assets.id " +
                $"left join checklist_number as checklist on pm_schedule.checklist_id = checklist.id " +
                $"where task_id = {task_id} and Asset_id=0;";

            List<CMPMScheduleExecutionDetail> checklist_collection = await Context.GetData<CMPMScheduleExecutionDetail>(myQuery2).ConfigureAwait(false);
            taskViewDetail[0].Material_consumption = new List<Materialconsumption>();
            //string myQuery2 = $"SELECT DISTINCT checklist.id, checklist.checklist_number AS name FROM pm_execution " + 
            //                    $"JOIN checkpoint on pm_execution.Check_Point_id = checkpoint.id " + 
            //                    $"JOIN checklist_number as checklist ON checklist.id = checkpoint.check_list_id " +
            //                    $"WHERE pm_execution.PM_Schedule_Id = {schedule_id};";

            //List<CMDefaultList> checklist_collection = await Context.GetData<CMDefaultList>(myQuery2).ConfigureAwait(false);
            List<SubEvalutionTask> Sub_task = new List<SubEvalutionTask>();

            string task1 = "SELECT   COALESCE(pm_task.title, pme.checkpoint_title) AS title, pm_task.id as subtask_id, ck.id as  id ,e.evalution_plan_id as evalution_plan_id, " +
                           "Case when permit.TBT_Done_By is null or  permit.TBT_Done_By =0 then 0 else 1  end ptw_tbt_done,  " +
                           $" pm_task.assigned_to as assign_to, pm_task.plan_date as  schedule_date, " +
                           $" CASE when permit.startDate <  now() then 1 else 0 END as  tbt_start, " +
                           $"pm_task.PTW_id as permit_id, ef.id as schdule_id,  " +
                           $"CONCAT('PTW',pm_task.PTW_id) as permit_code ,permit.status as ptw_status,  " +
                           $" permit.physicalIsolation as isolation ," +
                           $" pm_task.Status as status_of,{statusQry} as status_short , " +
                           $" CONCAT(usr.firstName,' ',usr.lastName)   as assign_name,ck.checklist_number as checklist_number, " +
                           $" ef.ptw_req as ptw_required ,  " +
                           $"CASE WHEN  ef.ptw_req=1 THEN 'YES'  else 'NO' END as is_ptw  " +
                           $"FROM  pm_task as pm_task " +
                           $"left join st_audit as pm_plan  on pm_task.plan_id = pm_plan.id " +
                           $"LEFT join  pm_execution as pme on pm_task.id=pme.task_id  " +
                           $"LEFT join  checkpoint as ckp on pme.Check_Point_id=ckp.id " +
                           $"Left join evalution_checklist_map as e on e.evalution_plan_id = pm_task.plan_id  " +
                           $"left join permits as permit on pm_task.PTW_id = permit.id " +
                           $"LEFT join  checklist_number as ck on ck.id=ckp.check_list_id " +
                           $"LEFT JOIN evalution_checklist_map AS ef ON ef.evalution_plan_id = pm_task.plan_id AND ck.id = ef.checklist_id " +
                           $"LEFT join  users as usr on usr.id=pm_task.assigned_to " +
                           $" WHERE pm_task.parent_task_id= {task_id} group by pm_task.id;";
            Sub_task = await Context.GetData<SubEvalutionTask>(task1).ConfigureAwait(false);

            if (Sub_task.Count > 0)
            {
                foreach (var data in Sub_task)
                {
                    if (Sub_task.Count > 0)
                    {
                        if (data.ptw_status > 0)
                        {
                            data.status_short_ptw_sub = Status_PTW(data.ptw_status);
                        }
                        if (data.status_of > 0)
                        {
                            CMMS.CMMS_Status _Status1 = (CMMS.CMMS_Status)(data.status_of);
                            string _shortStatus1 = getShortStatus(CMMS.CMMS_Modules.AUDIT_SCHEDULE, _Status1);
                            data.status_short_sub = _shortStatus1;
                        }
                    }
                }
            }
            List<CMPMScheduleExecutionDetail> checklist_collection_SubTask = new List<CMPMScheduleExecutionDetail>();
            if (Sub_task.Count > 0)
            {
                foreach (var data in Sub_task)
                {
                    string myQuery21 = $"SELECT pm_task.id as subtask_id, ef.id as schedule_id FROM  pm_task as pm_task   " +
                                       $"LEFT join  pm_execution as pme on pm_task.id=pme.task_id " +
                                       $"LEFT join  checkpoint as ckp on pme.Check_Point_id=ckp.id  " +
                                       $"Left join evalution_checklist_map as e on e.evalution_plan_id = pm_task.plan_id  " +
                                       $"LEFT join  checklist_number as ck on ck.id=ckp.check_list_id  " +
                                       $"LEFT JOIN evalution_checklist_map AS ef ON ef.evalution_plan_id = pm_task.plan_id AND ck.id = ef.checklist_id " +
                                       $" WHERE pm_task.parent_task_id= {task_id} group by pm_task.id;";
                    /* string myQuery21 = $"SELECT cmp.id as schedule_id ," +
                        $"checklist.checklist_number as checklist_name from evalution_checklist_map as cmp " +
                        $"left join checklist_number as checklist on cmp.checklist_id = checklist.id " +
                        $"where cmp.evalution_plan_id = {data.evalution_plan_id} and ;";*/

                    checklist_collection_SubTask = await Context.GetData<CMPMScheduleExecutionDetail>(myQuery21).ConfigureAwait(false);
                }
            }
            dynamic type;
            bool check;
            if (checklist_collection_SubTask.Count > 0)
            {
                type = checklist_collection_SubTask;
            }
            else
            {
                type = checklist_collection;
            }
            List<ScheduleCheckList> scheduleCheckList = new List<ScheduleCheckList>();
            foreach (var schedule in type)
            {

                string myQuery3 = "SELECT checkpoint.min_range,checkpoint.max_range,is_ok as cp_ok, boolean as type_bool, failure_weightage as failure_waightage,type as check_point_type,pm_execution.range as type_range, pm_execution.text as type_text,pm_execution.id as execution_id,checkpoint.type as check_point_type ,pm_execution.range as type_range,pm_execution.text as type_text,pm_execution.is_ok as type_bool, Check_Point_id as check_point_id, Check_Point_Name as check_point_name, Check_Point_Requirement as requirement, PM_Schedule_Observation as observation, job_created as is_job_created, linked_job_id, custom_checkpoint as is_custom_check_point, file_required as is_file_required " +
                                    $"FROM pm_execution " +
                                    $"left join checkpoint on checkpoint.id = pm_execution.Check_Point_id WHERE PM_Schedule_Id = {schedule.schedule_id}  ";
                if (type == checklist_collection_SubTask)
                {
                    myQuery3 += $" AND task_id = {schedule.subtask_id}";
                }

                scheduleCheckList = await Context.GetData<ScheduleCheckList>(myQuery3).ConfigureAwait(false);

                if (scheduleCheckList.Count > 0)
                {
                    foreach (ScheduleCheckList scheduleCheckPoint in scheduleCheckList)
                    {
                        string fileQry = $"SELECT {eventQry} AS _event, File_Path as file_path, File_Discription as file_description " +
                                            $"FROM pm_schedule_files WHERE PM_Execution_id = {scheduleCheckPoint.execution_id}; ";
                        List<ScheduleFiles> fileList = await Context.GetData<ScheduleFiles>(fileQry).ConfigureAwait(false);
                        scheduleCheckPoint.files = fileList;
                    }
                }

                string jobStatusQry = "CASE ";
                for (CMMS.CMMS_Status jobStatus = CMMS.CMMS_Status.JOB_CREATED; jobStatus <= CMMS.CMMS_Status.JOB_UPDATED; jobStatus++)
                    jobStatusQry += $"WHEN jobs.status={(int)jobStatus} THEN '' ";
                jobStatusQry += "ELSE 'Invalid Status' END ";

                string myQuery4 = $"SELECT jobs.id as job_id, jobs.title as job_title, jobs.description as job_description, CASE WHEN jobs.createdAt = '0000-00-00 00:00:00' THEN NULL ELSE jobs.createdAt END as job_date, {jobStatusQry} as job_status " +
                                    $"FROM jobs " +
                                    $"JOIN pm_execution ON jobs.id = pm_execution.linked_job_id " +
                                    $"WHERE pm_execution.PM_Schedule_Id  = {schedule.schedule_id};";
                try
                {
                    List<ScheduleLinkJob> linked_jobs = await Context.GetData<ScheduleLinkJob>(myQuery4).ConfigureAwait(false);
                    List<CMLog> log = await _utilsRepo.GetHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, "");
                    schedule.schedule_link_job = linked_jobs;

                }
                catch (Exception e)
                {

                }
                //if (checklist_collection.Count > 0)
                //{
                //    taskViewDetail[0].checklist_id = checklist_collection[0].id;
                //    taskViewDetail[0].checklist_name = checklist_collection[0].name;
                //}
                //taskViewDetail[0].schedule_check_points = scheduleCheckList;
                //taskViewDetail[0].history_log = log; 

                schedule.checklist_observation = scheduleCheckList;
            }
            taskViewDetail[0].schedules = checklist_collection;

            taskViewDetail[0].map_checklist = await GetEvaluationChecklistByID(taskViewDetail[0].plan_id, facilitytimeZone);
            taskViewDetail[0].Sub_PmTask = Sub_task;
            if (checklist_collection_SubTask.Count > 0)
            {
                foreach (var data in taskViewDetail[0].Sub_PmTask)
                {

                    var filteredSubSchedules = checklist_collection_SubTask
                        .Where(s => s.subtask_id == data.subtask_id)
                        .ToList();
                    data.Sub_schedules = filteredSubSchedules;
                }
            }
            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(taskViewDetail[0].status);
            //if (_Status != CMMS.CMMS_Status.AUDIT_APPROVED)
            //{
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_SCHEDULE, _Status);
            taskViewDetail[0].status_short = _shortStatus;

            string _longStatus = getLongStatus_taskview(CMMS.CMMS_Modules.AUDIT_SCHEDULE, _Status, taskViewDetail[0]);
            taskViewDetail[0].status_long = _longStatus;

            CMMS.CMMS_Status ptw_status = (CMMS.CMMS_Status)(taskViewDetail[0].ptw_status);
            string _longStatus_PTW = getLongStatus_taskview(CMMS.CMMS_Modules.AUDIT_SCHEDULE, ptw_status, taskViewDetail[0]);

            //}
            taskViewDetail[0].status_short_ptw = Status_PTW(taskViewDetail[0].ptw_status);
            foreach (var task in taskViewDetail)
            {
                if (task != null && task.approved_at != null)
                    task.approved_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, task.approved_at);
                if (task != null && task.cancelled_at != null)
                    task.cancelled_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, task.cancelled_at);
                if (task != null && task.closed_at != null)
                    task.closed_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, task.closed_at);
                if (task != null && task.done_date != null)
                    task.done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)task.done_date);
                if (task != null && task.due_date != null)
                    task.due_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)task.due_date);
                if (task != null && task.last_done_date != null)
                    task.last_done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)task.last_done_date);
                if (task != null && task.due_date != null)
                    task.due_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)task.due_date);
                if (task != null && task.rejected_at != null)
                    task.rejected_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, task.rejected_at);
                if (task != null && task.started_at != null)
                    task.started_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, task.started_at);
                if (task != null && task.updated_at != null)
                    task.updated_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, task.updated_at);
            }

            return taskViewDetail[0];
        }

        internal string getLongStatus_taskview(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMTaskView Obj)
        {
            string retValue = " ";


            switch (notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_SCHEDULE:
                    retValue = $"Audit Scheduled "; break;
                case CMMS.CMMS_Status.AUDIT_START:
                    retValue = String.Format("AUD{0} Satrted by {1}", Obj.id, Obj.started_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_DELETED:
                    retValue = $"Audit Deleted "; break;
                case CMMS.CMMS_Status.AUDIT_APPROVED:
                    retValue = $"Audit Approved By {Obj.approved_by_name}";
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP:
                    retValue = String.Format("AUD{0} Skip by {1}", Obj.id, Obj.skip_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_REJECT:
                    retValue = String.Format("AUD{0} Skip Rejected by {1}", Obj.id, Obj.skip_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_APPROVED:
                    retValue = String.Format("AUD{0} Skip Approved by {1}", Obj.id, Obj.skip_approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_REJECTED:
                    retValue = $"Audit Rejected By {Obj.closeRejectedbyName}"; break;
                case CMMS.CMMS_Status.AUDIT_CLOSED:
                    retValue = String.Format("AUD{0} Closed by {1}", Obj.id, Obj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED:
                    retValue = String.Format("AUD{0} Close Approved by {1}", Obj.id, Obj.closedApprovedByName);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_REJECT:
                    retValue = String.Format("AUD{0} Close Rejected by {1}", Obj.id, Obj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = $"Audit linked with permit By {Obj.closed_by_name}";
                    break;
                default:
                    break;
            }
            return retValue;

        }

        public static string Status_PTW(int statusID)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    statusName = "Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    statusName = "Issued";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    statusName = "Rejected By Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    statusName = "Approved";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    statusName = "Rejected By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    statusName = "Closed";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    statusName = "Cancelled BY Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    statusName = "Cancelled By HSE";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    statusName = "Cancelled By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    statusName = "Cancelled";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    statusName = "Cancel Request Rejected";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    statusName = "Cancelled";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    statusName = "Requested for Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    statusName = "Approved Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    statusName = "Rejected Extension";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    statusName = "Linked to Job";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    statusName = "Linked to PM";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    statusName = "Linked to Audit";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    statusName = "Linked to HOTO";
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    statusName = "Expired";
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    statusName = "Updated";
                    break;
                default:
                    statusName = "Invalid";
                    break;
            }
            return statusName;
        }

        internal async Task<CMDefaultResponse> CreateAuditSkip(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from pm_task where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update pm_task " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_SKIP}'," +
                 $"Skip_by = {userId}, " +
                 $"Skip_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan task id : " + auditPlanList[0].id + " skipped successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_SKIP);
                //CMPMPlanDetail _AuditList = await GetAuditPlanDetail(request.id, facilitytimeZone);
                CMPMTaskView _AuditList = await GetTaskDetail(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_TASK, CMMS.CMMS_Status.AUDIT_SKIP, new[] { userId }, _AuditList);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exist to skip.");
            }



            return response;
        }

        internal async Task<CMDefaultResponse> RejectAuditSkip(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id  from pm_task where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update pm_task " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_SKIP_REJECT}'," +
                 $"Skip_rejected_by = {userId}, " +
                 $"Skip_rejected_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with task id : " + auditPlanList[0].id + " skip rejected.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_SKIP_REJECT);

                CMPMTaskView _AuditList = await GetTaskDetail(request.id, facilitytimeZone);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_TASK, CMMS.CMMS_Status.AUDIT_SKIP_REJECT, new[] { userId }, _AuditList);


            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to skip.");
            }

            return response;
        }
        internal async Task<CMDefaultResponse> ApproveAuditSkip(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select plan_id id, Facility_id,frequency_id as ApplyFrequency,plan_date as Schedule_Date,assigned_to_audit as assignedTo,frequency.months, frequency.days  from pm_task  JOIN frequency ON pm_task.frequency_id = frequency.id where pm_task.ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);
            DateTime total_date = DateTime.Now;
            DataTable dt = await Context.FetchData(SelectQ).ConfigureAwait(false);

            if (dt.Rows.Count > 0)
            {
                DateTime nextdate = Convert.ToDateTime(dt.Rows[0]["Schedule_Date"]);
                int months = Convert.ToInt32(dt.Rows[0]["months"]);
                int fdays = Convert.ToInt32(dt.Rows[0]["days"]);

                total_date = months == 0 ? nextdate.AddDays(fdays) : nextdate.AddMonths(months);

            }

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update pm_task " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_SKIP_APPROVED}' , skip_approved_by = {userId}, skip_approved_at = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', approve_remarks = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan task : " + auditPlanList[0].title + " skip approved successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_SKIP_APPROVED);

                CMPMTaskView _AuditList = await GetTaskDetail(request.id, facilitytimeZone);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_TASK, CMMS.CMMS_Status.AUDIT_SKIP_APPROVED, new[] { userId }, _AuditList);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to skip.");
            }
            for (var i = 0; i < auditPlanList.Count; i++)
            {
                string entryInTask = $" INSERT INTO pm_task (plan_id, category_id, facility_id, frequency_id, prev_task_done_date,plan_date, closed_at, " +
                    $"assigned_to_audit, PTW_id, status) VALUES " +
                    $" ({auditPlanList[i].id},0,{auditPlanList[i].Facility_id},{auditPlanList[i].ApplyFrequency},'{auditPlanList[i].Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}','{total_date.ToString("yyyy-MM-dd HH:mm:ss")}',null,'{auditPlanList[i].assignedTo}',0,{(int)CMMS.CMMS_Status.AUDIT_SCHEDULE});SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(entryInTask).ConfigureAwait(false);
                int task_id = Convert.ToInt32(dt2.Rows[0][0]);

                string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                $"select {task_id} as task_id,id as plan_id, 0 as Asset_id, Checklist_id  as checklist_id,Schedule_Date    as PM_Schedule_date,{(int)CMMS.CMMS_Status.AUDIT_SCHEDULE} as status from st_audit  where id = {auditPlanList[i].id}";
                await Context.ExecuteNonQry<int>(scheduleQry);

                string setCodeNameQuery = "UPDATE pm_schedule " +
                                            "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                            "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                            "PM_Schedule_Number = CONCAT('SCH',id), " +
                                            "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
                await Context.ExecuteNonQry<int>(setCodeNameQuery);
            }
            return response;
        }
        internal async Task<CMDefaultResponse> CloseAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            //string SelectQ = "select id from st_audit where ID = '" + request.id + "'";
            //List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            string SelectQ = "select pm_task.id,st.module_type_id as module_type_id from pm_task Left join st_audit st  on st.id=pm_task.plan_id where pm_task.ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {

                string UpdateQ = $"update pm_task " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_CLOSED}' , closed_by = {userId}, closed_at = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', close_remarks = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                int task_id = auditPlanList[0].id;
                response = new CMDefaultResponse(request.id, task_id, CMMS.RETRUNSTATUS.SUCCESS, "Audit task with id : " + auditPlanList[0].id + " closed successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_CLOSED, userId);

                CMPMPlanDetail _AuditList = await GetAuditPlanDetail(request.id, facilitytimeZone);

                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_PLAN, CMMS.CMMS_Status.AUDIT_CLOSED, new[] { userId }, _AuditList);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to approve close audit.");
            }

            return response;
        }
        internal async Task<CMDefaultResponse> RejectCloseAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select pm_task.id,st.module_type_id as module_type_id from pm_task Left join st_audit st  on st.id=pm_task.plan_id where pm_task.ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {

                string UpdateQ = $"update pm_task " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_CLOSED_REJECT}' , rejected_by = {userId}, rejected_at = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', reject_remarks = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].title + " close rejected.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_CLOSED_REJECT, userId);

                CMPMTaskView _AuditList = await GetTaskDetail(request.id, facilitytimeZone);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_TASK, CMMS.CMMS_Status.AUDIT_CLOSED_REJECT, new[] { userId }, _AuditList);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to audit close reject.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> ApproveClosedAuditPlan(CMApproval request, int userId, string facilitytimeZone)
        {
            CMDefaultResponse response = null;

            string SelectQ = "select plan_id id,st.module_type_id as module_type_id, pm_task.Facility_id,frequency_id as ApplyFrequency,plan_date as Schedule_Date,assigned_to_audit as assignedTo,   " +
                             "frequency.months, frequency.days  from pm_task " +
                             "Left join st_audit st  on st.id=pm_task.plan_id " +
                             " JOIN frequency ON pm_task.frequency_id = frequency.id where pm_task.ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            DateTime total_date = DateTime.Now;
            DataTable dt = await Context.FetchData(SelectQ).ConfigureAwait(false);

            if (dt.Rows.Count > 0)
            {
                DateTime nextdate = Convert.ToDateTime(dt.Rows[0]["Schedule_Date"]);
                int months = Convert.ToInt32(dt.Rows[0]["months"]);
                int fdays = Convert.ToInt32(dt.Rows[0]["days"]);

                total_date = months == 0 ? nextdate.AddDays(fdays) : nextdate.AddMonths(months);

            }

            if (auditPlanList != null && auditPlanList.Count > 0)
            {

                string UpdateQ = $"update pm_task " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED}' , approved_by = {userId}, approved_at = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', close_comment = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with task id : " + auditPlanList[0].id + " close approved successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED, userId);

                CMPMTaskView _AuditList = await GetTaskDetail(request.id, facilitytimeZone);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_TASK, CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED, new[] { userId }, _AuditList);


                for (var i = 0; i < auditPlanList.Count; i++)
                {
                    string entryInTask = $" INSERT INTO pm_task (plan_id, category_id, facility_id, frequency_id,  prev_task_done_date,plan_date, closed_at, " +
                     $"assigned_to_audit, PTW_id, status) VALUES " +
                     $" ({auditPlanList[i].id},0,{auditPlanList[i].Facility_id},{auditPlanList[i].ApplyFrequency},'{auditPlanList[i].Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}','{total_date.ToString("yyyy-MM-dd HH:mm:ss")}',null,'{auditPlanList[i].assignedTo}',0,{(int)CMMS.CMMS_Status.AUDIT_SCHEDULE});SELECT LAST_INSERT_ID();";

                    DataTable dt2 = await Context.FetchData(entryInTask).ConfigureAwait(false);
                    int task_id = Convert.ToInt32(dt2.Rows[0][0]);

                    string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                    $"select {task_id} as task_id,id as plan_id, 0 as Asset_id, Checklist_id  as checklist_id,Schedule_Date    as PM_Schedule_date,{(int)CMMS.CMMS_Status.AUDIT_SCHEDULE} as status from st_audit  where id = {auditPlanList[i].id}";
                    await Context.ExecuteNonQry<int>(scheduleQry);

                    string setCodeNameQuery = "UPDATE pm_schedule " +
                                                "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                                "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                                "PM_Schedule_Number = CONCAT('SCH',id), " +
                                                "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
                    await Context.ExecuteNonQry<int>(setCodeNameQuery);
                }

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to approve close audit.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> AuditLinkToPermit(int audit_id, int ptw_id, int updatedBy, string facilitytimeZone)
        {

            string updateQry_task = $"update pm_task set ptw_id = {ptw_id},status = {(int)CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT}, updated_at = '{UtilsRepository.GetUTCTime()}', updated_by = {updatedBy}  where id =  {audit_id};";
            int retVal_task = await Context.ExecuteNonQry<int>(updateQry_task).ConfigureAwait(false);
            //select plan_id as id from pm_task where id = (select Distinct task_id from pm_execution where id = '1887');
            string SelectQ = $"SELECT plan_id AS id FROM pm_task where  id ={audit_id}";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);


            string updateQry = $"update st_audit set Audit_update_by_id = {updatedBy},status = {(int)CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT}, Audit_update_date = '{UtilsRepository.GetUTCTime()}', linked_permit_id = {ptw_id}  where id =  {auditPlanList[0].id};";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, audit_id, CMMS.CMMS_Modules.AUDIT_PLAN, ptw_id, $"Permit <{ptw_id}> Assigned to Audit <{audit_id}>", CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT, updatedBy);

            CMPMPlanDetail _AuditList = await GetAuditPlanDetail(audit_id, facilitytimeZone);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.AUDIT_PLAN, CMMS.CMMS_Status.AUDIT_REJECTED, new[] { ptw_id }, _AuditList);


            CMDefaultResponse response = new CMDefaultResponse(audit_id, CMMS.RETRUNSTATUS.SUCCESS, $"Evalution <{audit_id}> Linked To Permit <{ptw_id}> ");

            return response;
        }
        internal async Task<CMDefaultResponse> AssignAuditTask(int task_id, int assign_to, int userID)
        {
            string statusQry = $"SELECT status FROM pm_task   WHERE id = {task_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            string assignqry = $"SELECT CONCAT(firstName, ' ', lastName) as assigned_to FROM users WHERE id = {assign_to};";
            DataTable dt2 = await Context.FetchData(assignqry).ConfigureAwait(false);
            string assigned_to = Convert.ToString(dt2.Rows[0][0]);

            if (status != CMMS.CMMS_Status.AUDIT_SCHEDULE && status != CMMS.CMMS_Status.PM_ASSIGNED && status != CMMS.CMMS_Status.PM_LINKED_TO_PTW)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Only Scheduled Tasks can be assigned and Not yet started and Rejected Tasks can be Reassigned ");
            }

            string myQuery = "UPDATE pm_task SET " +
                                $"assigned_to_audit =' {assigned_to}' , " +
                                $"status = {(int)CMMS.CMMS_Status.AUDIT_APPROVED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID}  " +
                                $"WHERE id = {task_id} ;";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            CMDefaultResponse response = new CMDefaultResponse();


            if (status == CMMS.CMMS_Status.AUDIT_SCHEDULE)
            {
                response = new CMDefaultResponse(task_id, retCode, $"Audit Task Assigned To user Id {assign_to}");
            }
            else
            {
                response = new CMDefaultResponse(task_id, retCode, $"Audit Task Reassigned To user Id {assign_to}");
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_EXECUTION, task_id, 0, 0, $"Audit Task Assigned to user Id {assign_to}", CMMS.CMMS_Status.AUDIT_APPROVED, userID);
            return response;
        }

        internal async Task<CMDefaultResponse> CreateSubTaskForChecklist(CMCreateAuditPlan auditPlanList, int userID)
        {
            int task_id = auditPlanList.task_id;
            int plan_id = auditPlanList.plan_id;
            CMDefaultResponse response = new CMDefaultResponse();
            int parent_task_id = 0;

            // check task is created or not

            string check_task_is_created_Q = $" select id from pm_task where plan_id = {plan_id} ";
            DataTable dt = await Context.FetchData(check_task_is_created_Q).ConfigureAwait(false);
            if (dt.Rows.Count > 0)
            {
                parent_task_id = task_id;
            }
            else
            {
                parent_task_id = 0;
            }

            string scheduleDate = auditPlanList.Schedule_Date.ToString("yyyy-MM-dd");
            string getParamsQry = $"SELECT plan.id,plan.Audit_Added_date,evalution_checklist_map.checklist_id,  " +
                                  $"evalution_checklist_map.id as schedule_id " +
                                  $"FROM evalution_checklist_map " +
                                  $"left join st_audit as plan on evalution_checklist_map.evalution_plan_id = plan.id " +
                                  $"where plan.id = {plan_id}";

            List<ScheduleIDData> schedule_details = await Context.GetData<ScheduleIDData>(getParamsQry).ConfigureAwait(false);
            foreach (CMEvaluationAudit schedule in auditPlanList.map_checklist)
            {
                if (schedule.subtask_id == 0)
                {
                    if (parent_task_id > 0)
                    {
                        string entryInTask = $" INSERT INTO pm_task (plan_id, category_id, facility_id, frequency_id, plan_date,  " +
                         $" assigned_to_audit, PTW_id, status,parent_task_id,assigned_to)  " +
                         $"  select   plan_id, category_id, facility_id, frequency_id,' {schedule.schedule_date.ToString("yyyy-MM-dd")}' ,  " +
                         $" assigned_to_audit, PTW_id, '{(int)CMMS.CMMS_Status.AUDIT_APPROVED}',{parent_task_id},{schedule.assign_to}  " +
                         $" from pm_task where id = {task_id};SELECT LAST_INSERT_ID();";
                        DataTable dt2 = await Context.FetchData(entryInTask).ConfigureAwait(false);
                        task_id = Convert.ToInt32(dt2.Rows[0][0]);
                    }

                    string checkpointsQuery = "SELECT checkpoint.id, checkpoint.check_point, checkpoint.check_list_id as checklist_id, checkpoint.requirement, checkpoint.is_document_required,checkpoint.status " +
                                                "FROM checkpoint " +
                                                "left JOIN checklist_mapping as map ON map.checklist_id = checkpoint.check_list_id " +
                                                "left JOIN checklist_number as checklist ON checklist.id = map.checklist_id " +
                                                $"WHERE checkpoint.check_list_id in ({schedule.checklist_id});";
                    List<CMCreateCheckPoint> checkpointList = await Context.GetData<CMCreateCheckPoint>(checkpointsQuery).ConfigureAwait(false);
                    if (checkpointList.Count == 0)
                    {
                        response = new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "No checklist or checkpoints found");
                        return response;
                    }
                    else
                    {
                        foreach (CMCreateCheckPoint checkpoint in checkpointList)
                        {
                            string executeQuery = "INSERT INTO pm_execution(task_id,PM_Schedule_Id, PM_Schedule_Code, Check_Point_id, Check_Point_Name, " +
                                                    "Check_Point_Code, Status, assign_to_evalution,PM_Schedule_Observation_add_date,Check_Point_Requirement,checkpoint_title) VALUES " +
                                                    $"({task_id}, {schedule_details[0].schedule_id} ,'PMSCH{schedule_details[0].schedule_id}', {checkpoint.id}, " +
                                                    $"'{checkpoint.check_point}', 'CP{checkpoint.id}', 1,{checkpoint.assign_to},'{checkpoint.schedule_date}','{checkpoint.requirement}','{schedule.title}'); " +
                                                    $"SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(executeQuery).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt2.Rows[0][0]);

                        }
                        string startQry = $"UPDATE pm_schedule SET PM_Execution_Started_by_id = {userID}, " +
                                           $"PM_Execution_Started_date = '{UtilsRepository.GetUTCTime()}', " +
                                          $"status = {(int)CMMS.CMMS_Status.PM_START}, status_updated_at = '{UtilsRepository.GetUTCTime()}'" +
                                          $" WHERE id = {schedule_details[0].schedule_id};";
                        await Context.ExecuteNonQry<int>(startQry).ConfigureAwait(false);

                    }

                }
                else
                {
                    string startQry1 = $"UPDATE pm_task SET  assigned_to={schedule.assign_to}, title='{schedule.title}',plan_date='{schedule.schedule_date}' " +
                                 $"WHERE id = {schedule.subtask_id};";
                    await Context.ExecuteNonQry<int>(startQry1).ConfigureAwait(false);
                }

            }
            response = new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.SUCCESS, "Evaluation plan with sub task id : " + task_id + " created successfully.");

            return response;
        }
    }
}
