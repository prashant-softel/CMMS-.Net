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
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Repositories.Audits
{
    public class AuditPlanRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public AuditPlanRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

            _utilsRepo = new UtilsRepository(sqlDBHelper);
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
                "concat(u.firstName, ' ', u.lastName) Auditor_Emp_Name , st.frequency, st.status, case when st.frequency = 0 then 'False' else 'True' end as FrequencyApplicable, st.Description,st.Schedule_Date, st.checklist_id, " +
                " checklist_number as checklist_name, frequency.name as frequency_name, st.created_at, concat(created.firstName, ' ', created.lastName) created_by" +
                " from st_audit st " +
                "inner join facilities f ON st.Facility_id = f.id " +
                "left join users au on au.id = st.Auditee_Emp_ID " +
                "left join users u on u.id = st.Auditor_Emp_ID " +
                " left join checklist_number checklist_number on checklist_number.id = st.Checklist_id " +
                "left join frequency frequency on frequency.id = st.Frequency " +
          
                "left join users created on created.id = st.created_by   " + filter;

            List<CMAuditPlanList> auditPlanList = await Context.GetData<CMAuditPlanList>(SelectQ).ConfigureAwait(false);
            for (var i = 0; i < auditPlanList.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(auditPlanList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.AUDIT_PLAN, _Status);
                auditPlanList[i].short_status = _shortStatus;
            }
            return auditPlanList[0];
        }

        internal async Task<CMDefaultResponse> CreateAuditPlan(CMCreateAuditPlan request, int userID)
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
                string InsertQ = $"insert into st_audit(plan_number, Facility_id, Audit_Added_date, Status, Auditee_Emp_ID, Auditor_Emp_ID, Frequency, Description, Schedule_Date, Checklist_id, created_by, created_at) " +
                                $"values('{request.plan_number}', {request.Facility_id}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', {((int)CMMS.CMMS_Status.AUDIT_SCHEDULE)}, {request.auditee_id}, {request.auditor_id}, {request.ApplyFrequency},'{request.Description}','{request.Schedule_Date.ToString("yyyy-MM-dd HH:mm:ss")}', {request.Checklist_id}, {userID},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}') ; SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(InsertQ).ConfigureAwait(false);
                InsertedValue = Convert.ToInt32(dt2.Rows[0][0]);
                response = new CMDefaultResponse(InsertedValue, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + request.plan_number + " created successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, " Audit plan schduled ", CMMS.CMMS_Status.AUDIT_SCHEDULE);

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
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, audit_plan_id, 0, 0, " Audit plan deleted ", CMMS.CMMS_Status.AUDIT_DELETED);

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

        internal async Task<CMDefaultResponse> ApproveAuditPlan(CMApproval request, int userId)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_APPROVED}' , approved_by = {userId}, approved_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', approved_Comment = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].plan_number + " approved successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_APPROVED);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to approve.");
            }

            return response;
        }
        internal async Task<CMDefaultResponse> RejectAuditPlan(CMApproval request, int userId)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from st_audit where ID = '" + request.id + "'";
            List<CMCreateAuditPlan> auditPlanList = await Context.GetData<CMCreateAuditPlan>(SelectQ).ConfigureAwait(false);

            if (auditPlanList != null && auditPlanList.Count > 0)
            {
                string UpdateQ = $"update st_audit " +
                 $"set Status = '{(int)CMMS.CMMS_Status.AUDIT_REJECTED}' , rejected_by = {userId}, rejected_Date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', rejected_Comment = '{request.comment}'" +
                 $"where ID = {request.id}";
                var result = await Context.ExecuteNonQry<int>(UpdateQ);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Audit plan with plan number : " + auditPlanList[0].plan_number + " rejected successfully.");
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.AUDIT_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.AUDIT_REJECTED);

            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Audit plan does not exists to reject.");
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
                if (!assetIDs.Contains(map.asset_id))
                    invalidAssets.Add(map.asset_id);
            }
            //if (invalidChecklists.Count > 0 || invalidAssets.Count > 0)
            //    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG,
            //        $"{invalidChecklists.Count} invalid checklists [{string.Join(',', invalidChecklists)}] " +
            //        $"and {invalidAssets.Count} invalid assets [{string.Join(',', invalidAssets)}] linked");

            string addPlanQry = $"INSERT INTO pm_plan(plan_name, facility_id, category_id, frequency_id, " +
                                $"status, plan_date,assigned_to, created_by, created_at, updated_by, updated_at, type_id) VALUES " +
                                $"('{pm_plan.plan_name}', {pm_plan.facility_id}, {pm_plan.category_id}, {pm_plan.plan_freq_id}, " +
                                $"{status}, '{pm_plan.plan_date.ToString("yyyy-MM-dd")}',{pm_plan.assigned_to_id}, " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', {userID}, '{UtilsRepository.GetUTCTime()}', {pm_plan.type_id}); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(addPlanQry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId, typeId) VALUES ";
            foreach (var map in pm_plan.mapAssetChecklist)
            {
                mapChecklistQry += $"({id}, {map.asset_id}, {map.checklist_id}, {pm_plan.type_id}), ";
            }
            mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
            await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, id, 0, 0, "PM Plan added", CMMS.CMMS_Status.PM_PLAN_CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Plan added successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> StartAuditTask(int task_id, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Add all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            List<int> idList = new List<int>();
            CMDefaultResponse response;
            string statusQry = $"SELECT pm_task.status,permit.endDate, permit.status as ptw_status FROM pm_task " +
                               $"left join permits as permit on pm_task.PTW_id = permit.id " +
                $"WHERE pm_task.id = {task_id}";

            // List<CMPMTaskList> taskDetails = await Context.GetData<CMPMTaskList>(statusQry).ConfigureAwait(false);
            //DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            //CMMS.CMMS_Status ptw_status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][2]);

            string updateQ = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.AUDIT_APPROVED} WHERE id = {task_id};";
            await Context.ExecuteNonQry<int>(updateQ).ConfigureAwait(false);

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
                    string startQry = $"UPDATE pm_schedule SET PM_Execution_Started_by_id = {userID}, PM_Execution_Started_date = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.PM_START}, status_updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {schedule.schedule_id};";
                    await Context.ExecuteNonQry<int>(startQry).ConfigureAwait(false);
                }
            }
            string startQry2 = $"UPDATE pm_task SET started_by = {userID}, started_at = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.PM_START} WHERE id = {task_id};";
            await Context.ExecuteNonQry<int>(startQry2).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, task_id, 0, 0, $"PM Execution Started of assets ", CMMS.CMMS_Status.PM_START, userID);

            response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"Execution PMTASK{task_id} Started Successfully");

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
            if (status == CMMS.CMMS_Status.PM_SCHEDULED || status == CMMS.CMMS_Status.PM_REJECTED)
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
                                message = "Observation Added";
                                response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Added Successfully");
                            }
                            else
                            {
                                updateQry += $"PM_Schedule_Observation = '{schedule_detail.observation}', ";
                                updateQry += $"PM_Schedule_Observation_update_by = {userID}, " +
                                            $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' {CPtypeValue} ";
                                message = "Observation Updated";
                                response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Updated Successfully");
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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.task_id, 0, 0, string.IsNullOrEmpty(request.comment) ? "Audit Task Updated" : request.comment, CMMS.CMMS_Status.PM_UPDATED, userID);

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


            CMJobView _ViewJobList = await GetJobDetails(newJobID);

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

        internal async Task<CMJobView> GetJobDetails(int job_id)
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


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Plan Approved " : request.comment, CMMS.CMMS_Status.PM_PLAN_APPROVED);

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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Plan Rejected " : request.comment, CMMS.CMMS_Status.PM_PLAN_REJECTED);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "PM Plan Rejected Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePlan(int planId, int userID)
        {
            string approveQuery = $"update pm_plan set status_id = 0 where id = {planId}; ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, planId, 0, 0, $"PM Plan Deleted by user {userID}", CMMS.CMMS_Status.PM_PLAN_DELETED);

            CMDefaultResponse response = new CMDefaultResponse(planId, CMMS.RETRUNSTATUS.SUCCESS, $" PM Plan Deleted");
            return response;
        }
    }
}
