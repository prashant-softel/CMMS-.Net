using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Repositories.Jobs;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Permits;
using static iTextSharp.text.pdf.AcroFields;

namespace CMMSAPIs.Repositories.PM
{
    public class PMScheduleViewRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private PMRepository _pmScheduleRepo;
        private JobRepository _jobRepo;
        public PMScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _pmScheduleRepo = new PMRepository(sqlDBHelper);
            _jobRepo = new JobRepository(sqlDBHelper);
        }
        Dictionary<CMMS.CMMS_Status, string> statusList = new Dictionary<CMMS.CMMS_Status, string>()
        {
            { CMMS.CMMS_Status.PM_SUBMIT, "PM Submitted" },
            { CMMS.CMMS_Status.PM_LINK_PTW, "PM Linked to PTW" },
            { CMMS.CMMS_Status.PM_START, "PM Started" },
            { CMMS.CMMS_Status.PM_COMPLETED, "PM Completed" },
            { CMMS.CMMS_Status.PM_REJECTED, "PM Rejected" },
            { CMMS.CMMS_Status.PM_APPROVED, "PM Approved" },
            { CMMS.CMMS_Status.PM_CANCELLED, "PM Cancelled" },
            { CMMS.CMMS_Status.PM_DELETED, "PM Deleted" },
            { CMMS.CMMS_Status.PM_UPDATED, "PM Updated" }
        };

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;
            retValue = "";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_SCHEDULED:
                    retValue = "Scheduled"; break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = "Assigned"; break;
                case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                    retValue = "Linked To PTW"; break;
                case CMMS.CMMS_Status.PM_START:
                    retValue = "Started"; break;
                case CMMS.CMMS_Status.PM_COMPLETED:
                    retValue = "Waiting for Approval"; break;
                case CMMS.CMMS_Status.PM_REJECTED:
                    retValue = "Rejected"; break;
                case CMMS.CMMS_Status.PM_APPROVED:
                    retValue = "Approved"; break;
                case CMMS.CMMS_Status.PM_CANCELLED:
                    retValue = "Cancelled"; break;
                case CMMS.CMMS_Status.PM_DELETED:
                    retValue = "Deleted"; break;
                case CMMS.CMMS_Status.PM_UPDATED:
                    retValue = "Updated"; break;
                default:
                    break;
            }
            return retValue;

        }
        internal async Task<List<CMPMTaskList>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds)
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

            string myQuery = $"SELECT pm_task.id,pm_task.category_id,cat.name as category_name,  CONCAT('PMTASK',pm_task.id) as task_code,pm_plan.plan_name as plan_title,pm_task.facility_id, pm_task.frequency_id as frequency_id, freq.name as frequency_name, pm_task.plan_date as due_date,prev_task_done_date as done_date, CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assigned_to_name, pm_task.PTW_id as permit_id, CONCAT('PTW',pm_task.PTW_id) as permit_code, PM_task.status " +
                               "FROM pm_task " +
                               $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                               $"left join pm_plan  on pm_task.plan_id = pm_plan.id " +
                               $"left join assetcategories as cat  on pm_task.category_id = cat.id " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id where 1 ";

           // myQuery += (frequencyIds.Length > 0 ? " AND freq.id IN ( '" + string.Join("' , '", frequencyIds) + "' )" : string.Empty);

            List<CMPMTaskList> scheduleViewList = new List<CMPMTaskList> ();
            if (facility_id > 0)
            {
                myQuery += $" and pm_task.Facility_id = {facility_id} ";
                if (start_date == null)
                    start_date = DateTime.UtcNow;
                if (end_date == null)
                    end_date = DateTime.UtcNow;
                if (start_date > end_date)
                    throw new ArgumentException("Start date should be earlier than end date");
                string start = ((DateTime)start_date).ToString("yyyy'-'MM'-'dd");
                myQuery += $"AND pm_task.plan_date >= '{start}' ";
                string end = ((DateTime)end_date).ToString("yyyy'-'MM'-'dd");
                myQuery += $"AND pm_task.plan_date <= '{end}' ";
                //if (categories.Count > 0)
                //{
                //    string catList = string.Join(", ", categories);
                //    myQuery += $"AND Asset_Category_id in ({catList}) ";
                //}
                if (frequencyIds != "" || frequencyIds != null)
                {
                    
                    myQuery += $"AND pm_task.frequency_id in ({frequencyIds}) ";
                }

                if (categoryIds != "" || categoryIds != null)
                {

                    myQuery += $"AND pm_task.category_id in ({categoryIds}) ";
                }
                scheduleViewList = await Context.GetData<CMPMTaskList>(myQuery).ConfigureAwait(false);


                foreach (var task in scheduleViewList)
                {
                    CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(task.status);
                    string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
                    task.status_short = _shortStatus;
                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            
            return scheduleViewList;
        }

        

        internal async Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Delete the requested id from primary table
             * Code goes here
            */
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_LINK_PTW && status != CMMS.CMMS_Status.PM_SUBMIT)
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a PM schedule that has not been executed can be cancelled.");
            string myQuery = "UPDATE pm_schedule SET " + 
                                $"PM_Schedule_cancel_by_id = {userID}, " +  
                                $"PM_Schedule_cancel_date = '{UtilsRepository.GetUTCTime()}', " + 
                                $"PM_Schedule_cancel_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CANCELLED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " + 
                                $"WHERE id = {request.id} AND status IN ({(int)CMMS.CMMS_Status.PM_LINK_PTW},{(int)CMMS.CMMS_Status.PM_SUBMIT});";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if(retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, "PM Schedule Cancelled", CMMS.CMMS_Status.PM_CANCELLED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Schedule cancelled successfully");
            return response;
        }

        internal async Task<CMPMTaskView> GetPMTaskDetail(int task_id)
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

            //string myQuery1 = $"SELECT id, PM_Maintenance_Order_Number as maintenance_order_number, PM_Schedule_date as schedule_date, PM_Schedule_Completed_date as completed_date, Asset_id as equipment_id, Asset_Name as equipment_name, Asset_Category_id as category_id, Asset_Category_name as category_name, PM_Frequecy_id as frequency_id, PM_Frequecy_Name as frequency_name, PM_Schedule_Emp_name as assigned_to_name, PTW_id as permit_id, status, {statusQry} as status_name, Facility_id as facility_id, Facility_Name as facility_name " +
            //                    $"FROM pm_schedule WHERE id = {schedule_id};";

            string myQuery = $"SELECT pm_task.id, CONCAT('PMTASK',pm_task.id) as task_code,pm_task.category_id,cat.name as category_name, pm_plan.plan_name as plan_title, pm_task.facility_id, frequency_id as frequency_id, freq.name as frequency_name, pm_task.plan_date as due_date,prev_task_done_date as done_date, CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assigned_to_name, pm_task.PTW_id as permit_id, CONCAT('PTW',pm_task.PTW_id) as permit_code, PM_task.status, {statusQry} as status_short " +
                               "FROM pm_task " +
                               $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                               $"left join pm_plan  on pm_task.plan_id = pm_plan.id " +
                               $"left join assetcategories as cat  on pm_task.category_id = cat.id " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id where pm_task.id = {task_id} ";

            List<CMPMTaskView> taskViewDetail = await Context.GetData<CMPMTaskView>(myQuery).ConfigureAwait(false);

            if (taskViewDetail.Count == 0)
                throw new MissingMemberException("PM Task not found");

            string myQuery2 = $"SELECT pm_schedule.id as schedule_id, assets.name as asset_name, checklist.checklist_number as checklist_name from pm_schedule " +
                $"left join assets on pm_schedule.asset_id = assets.id " +
                $"left join checklist_number as checklist on pm_schedule.checklist_id = checklist.id " +
                $"where task_id = {task_id};";

            List<CMPMScheduleExecutionDetail> checklist_collection = await Context.GetData<CMPMScheduleExecutionDetail>(myQuery2).ConfigureAwait(false);


            //string myQuery2 = $"SELECT DISTINCT checklist.id, checklist.checklist_number AS name FROM pm_execution " + 
            //                    $"JOIN checkpoint on pm_execution.Check_Point_id = checkpoint.id " + 
            //                    $"JOIN checklist_number as checklist ON checklist.id = checkpoint.check_list_id " +
            //                    $"WHERE pm_execution.PM_Schedule_Id = {schedule_id};";

            //List<CMDefaultList> checklist_collection = await Context.GetData<CMDefaultList>(myQuery2).ConfigureAwait(false);
            foreach (var schedule in checklist_collection)
            {
                string myQuery3 = "SELECT pm_execution.id as execution_id,checkpoint.type as check_point_type ,Check_Point_id as check_point_id, Check_Point_Name as check_point_name, Check_Point_Requirement as requirement, PM_Schedule_Observation as observation, job_created as is_job_created, linked_job_id, custom_checkpoint as is_custom_check_point, file_required as is_file_required " +
                                    $"FROM pm_execution " +
                                    $"left join checkpoint on checkpoint.id = pm_execution.Check_Point_id WHERE PM_Schedule_Id = {schedule.schedule_id}  ;";

                List<ScheduleCheckList> scheduleCheckList = await Context.GetData<ScheduleCheckList>(myQuery3).ConfigureAwait(false);

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
                    jobStatusQry += $"WHEN jobs.status={(int)jobStatus} THEN '{JobRepository.getShortStatus(CMMS.CMMS_Modules.JOB, jobStatus)}' ";
                jobStatusQry += "ELSE 'Invalid Status' END ";

                string myQuery4 = $"SELECT jobs.id as job_id, jobs.title as job_title, jobs.description as job_description, CASE WHEN jobs.createdAt = '0000-00-00 00:00:00' THEN NULL ELSE jobs.createdAt END as job_date, {jobStatusQry} as job_status " +
                                    $"FROM jobs " +
                                    $"JOIN pm_execution ON jobs.id = pm_execution.linked_job_id " +
                                    $"WHERE pm_execution.PM_Schedule_Id  = {schedule.schedule_id};";
                List<ScheduleLinkJob> linked_jobs = await Context.GetData<ScheduleLinkJob>(myQuery4).ConfigureAwait(false);
                List<CMLog> log = await _utilsRepo.GetHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id);

                //if (checklist_collection.Count > 0)
                //{
                //    taskViewDetail[0].checklist_id = checklist_collection[0].id;
                //    taskViewDetail[0].checklist_name = checklist_collection[0].name;
                //}
                //taskViewDetail[0].schedule_check_points = scheduleCheckList;
                //taskViewDetail[0].history_log = log;

                schedule.schedule_link_job = linked_jobs;
                schedule.checklist_observation = scheduleCheckList;
            }
            taskViewDetail[0].schedules = checklist_collection;

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(taskViewDetail[0].status);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
            taskViewDetail[0].status_short = _shortStatus;

            return taskViewDetail[0];
        }

        internal async Task<CMDefaultResponse> LinkPermitToPMTask(int schedule_id, int permit_id, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Set the required fields in primary table for linling permit to PM
             * Code goes here
            */
            string scheduleQuery = "SELECT PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
                                    $"FROM pm_schedule WHERE id = {schedule_id};";
            List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(scheduleQuery).ConfigureAwait(false);
            string permitQuery = "SELECT ptw.id as ptw_id, ptw.code as ptw_code, ptw.title as ptw_title, ptw.status as ptw_status " + 
                                    "FROM " + 
                                        "permits as ptw " +
                                    $"WHERE ptw.id = {permit_id} AND ptw.facilityId = {scheduleData[0].facility_id};";
            List<ScheduleLinkedPermit> permit = await Context.GetData<ScheduleLinkedPermit>(permitQuery).ConfigureAwait(false);
            if (permit.Count == 0)
                return new CMDefaultResponse(schedule_id, CMMS.RETRUNSTATUS.FAILURE, $"Permit {permit_id} does not exist.");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PTW_id = {permit[0].ptw_id}, " +
                                $"PTW_Code = '{permit[0].ptw_code}', " +
                                $"PTW_Ttitle = '{permit[0].ptw_title}', " +
                                $"PTW_by_id = {userID}, " +
                                $"PTW_Status = {permit[0].status}, " +
                                $"PTW_Attached_At = '{UtilsRepository.GetUTCTime()}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_LINK_PTW}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {schedule_id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule_id, CMMS.CMMS_Modules.PTW, permit_id, "PTW linked to PM", CMMS.CMMS_Status.PM_LINK_PTW, userID);
            CMDefaultResponse response = new CMDefaultResponse(schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Permit {permit_id} linked to schedule {schedule_id}");
            return response;
        }
        internal async Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Add a custom checkpoint
             * Code goes here
            */
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.schedule_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_START && status != CMMS.CMMS_Status.PM_REJECTED)
                return new CMDefaultResponse(request.schedule_id, CMMS.RETRUNSTATUS.FAILURE, "Execution must be rejected or in progress to add a custom checkpoint");
            string myQuery = "INSERT INTO pm_execution (PM_Schedule_Id, PM_Schedule_Code, Check_Point_Name, custom_checkpoint, file_required, Status, Check_Point_Requirement) " +
                                $"VALUES ({request.schedule_id}, 'PMSCH{request.schedule_id}', '{request.check_point_name}', 1, {request.is_document_required}, 1, '{request.requirement}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt2 = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.CHECKPOINTS, 0, "Custom Checkpoint added", CMMS.CMMS_Status.PM_UPDATED, userID);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, $"Custom checkpoint added successfully to PM Schedule PMSCH{request.schedule_id}");
        }

        internal async Task<CMDefaultResponse> StartPMTask(int task_id, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Add all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            List<int> idList = new List<int>();
            CMDefaultResponse response;
            string statusQry = $"SELECT status FROM pm_task WHERE id = {task_id}";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            //if (status != CMMS.CMMS_Status.PM_LINK_PTW)
            //{
            //    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution due to following reasons:" +
            //        "\n 1. Permit is not linked to PM" +
            //        "\n 2. Permit has been expired" +
            //        "\n 3. Execution has already been started" +
            //        "\n 4. Execution is completed");
            //}
            string getParamsQry = "SELECT pm_schedule.task_id , pm_schedule.id as schedule_id, plan.frequency_id as frequency_id, plan.facility_id as facility_id, assets.categoryId as category_id, Asset_id as asset_id, PM_Schedule_date as schedule_date " +
                                $"FROM pm_schedule "+
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
                                            $"WHERE map.facility_id IN ({schedule.facility_id}) AND map.category_id IN ( {schedule.category_id}) AND checklist.frequency_id IN ({schedule.frequency_id});";

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
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, task_id, 0, 0, $"PM Execution Started of assets ", CMMS.CMMS_Status.PM_START, userID);

                response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"Execution PMTASK{task_id} Started Successfully");
            
            return response;
        }
        
        internal async Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID)
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
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.schedule_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_START && status != CMMS.CMMS_Status.PM_REJECTED)
            {
                responseList.Add(new CMDefaultResponse(request.schedule_id, CMMS.RETRUNSTATUS.FAILURE, 
                    "Execution must be rejected or in progress to modify execution details"));
                return responseList;
            }
            string executeQuery = $"SELECT id FROM pm_execution WHERE PM_Schedule_Id = {request.schedule_id};";
            DataTable dt = await Context.FetchData(executeQuery).ConfigureAwait(false);
            List<int> executeIds = dt.GetColumn<int>("id");
            foreach(var schedule_detail in request.add_observations)
            {
                CMDefaultResponse response;
                if(executeIds.Contains(schedule_detail.execution_id))
                {
                    int changeFlag = 0;
                    string myQuery1 = "SELECT id as execution_id, PM_Schedule_Observation as observation, job_created as job_create " +
                                        $"FROM pm_execution WHERE id = {schedule_detail.execution_id};";
                    List<AddObservation> execution_details = await Context.GetData<AddObservation>(myQuery1).ConfigureAwait(false);
                    if(schedule_detail.observation != null && schedule_detail.observation != "" && !schedule_detail.observation.Equals(execution_details[0].observation))
                    {
                        string updateQry = "UPDATE pm_execution SET ";
                        string message;
                        if(execution_details[0].observation == null || execution_details[0].observation == "")
                        {
                            updateQry += $"PM_Schedule_Observation_added_by = {userID}, " +
                                        $"PM_Schedule_Observation_add_date = '{UtilsRepository.GetUTCTime()}', " +
                                        $"PM_Schedule_Observation = '{schedule_detail.observation}' ";
                            message = "Observation Added";
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Added Successfully");
                        }
                        else
                        {
                            updateQry += $"PM_Schedule_Observation = '{schedule_detail.observation}', ";
                            updateQry += $"PM_Schedule_Observation_update_by = {userID}, " +
                                        $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' " ;
                            message = "Observation Updated";
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Updated Successfully");
                        }
                        updateQry += $"WHERE id = {schedule_detail.execution_id};";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, message, CMMS.CMMS_Status.PM_UPDATED, userID);
                        responseList.Add(response);
                        changeFlag++;
                    }
                    if(schedule_detail.job_create == 1 && execution_details[0].job_create == 0)
                    {

                        string facilityQry = $"SELECT facilities.id as block, CASE WHEN facilities.parentId=0 THEN facilities.id ELSE facilities.parentId END AS parent " +
                                            $"FROM facilities JOIN pm_schedule ON pm_schedule.Block_Id=facilities.id " +
                                            $"WHERE pm_schedule.id = {request.schedule_id};";
                        DataTable dtFacility = await Context.FetchData(facilityQry).ConfigureAwait(false);
                        string titleDescQry = $"SELECT CONCAT('PMSCH{request.schedule_id}\\r\\n', Check_Point_Name, ': ', Check_Point_Requirement) as title, " +
                                                $"PM_Schedule_Observation as description FROM pm_execution WHERE id = {execution_details[0].execution_id};";
                        DataTable dtTitleDesc = await Context.FetchData(titleDescQry).ConfigureAwait(false);
                        string assetsPMQry = $"SELECT Asset_id FROM pm_schedule WHERE id = {request.schedule_id};";
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
                        CMDefaultResponse jobResp = await _jobRepo.CreateNewJob(newJob,userID);
                        string updateQry = $"UPDATE pm_execution SET job_created = 1, linked_job_id = {jobResp.id[0]} WHERE id = {schedule_detail.execution_id};";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"Job {jobResp.id[0]} Created for PM", CMMS.CMMS_Status.PM_UPDATED, userID);
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
                                                            $"WHERE PM_Schedule_id = {request.schedule_id} " +
                                                            $"AND PM_Execution_id = {schedule_detail.execution_id} " +
                                                            $"AND PM_Event = {(int)file.pm_event}";
                                DataTable dt2 = await Context.FetchData(checkEventFiles).ConfigureAwait(false);

                                if (dt2.Rows.Count > 0)
                                {
                                    string deleteQry = "DELETE FROM pm_schedule_files " +
                                                            $"WHERE PM_Schedule_id = {request.schedule_id} " +
                                                            $"AND PM_Execution_id = {schedule_detail.execution_id} " +
                                                            $"AND PM_Event = {(int)file.pm_event}";
                                    await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                                }
                                string insertFile = "INSERT INTO pm_schedule_files(PM_Schedule_id, PM_Schedule_Code, PM_Execution_id, File_id, PM_Event, " +
                                                    "File_Discription, File_added_by, File_added_date, File_Server_id, File_Server_Path) VALUES " +
                                                    $"({request.schedule_id}, 'PMSCH{request.schedule_id}', {schedule_detail.execution_id}, {file.file_id}, " +
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
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"{schedule_detail.pm_files.Count} file(s) attached to PMSCH{request.schedule_id}", CMMS.CMMS_Status.PM_UPDATED, userID);
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"{schedule_detail.pm_files.Count} file(s) attached to PM Successfully");
                            responseList.Add(response);
                            changeFlag++;
                        }
                    }
                    if(changeFlag == 0)
                    {
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "No changes");
                        responseList.Add(response);
                    }
                }
                else
                {
                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id} not associated with PMSCH{request.schedule_id}");
                    responseList.Add(response);
                }
            }
            return responseList;
        }

        internal async Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for completion
             * Code goes here
            */
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_START && status != CMMS.CMMS_Status.PM_REJECTED)
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a rejected PM schedule or one under execution can be closed");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PM_Schedule_Completed_by_id = {userID}, " +
                                $"PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Schedule_Complete_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_COMPLETED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.id} AND status IN ({(int)CMMS.CMMS_Status.PM_START}, {(int)CMMS.CMMS_Status.PM_REJECTED});";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, "PM Schedule Closed", CMMS.CMMS_Status.PM_COMPLETED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Schedule Closed successfully");
            return response;
        }

        internal async Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for approval
             * Code goes here
            */
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_COMPLETED)
                return new CMRescheduleApprovalResponse(0, request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a closed PM schedule can be Approved");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PM_Schedule_Completed_by_id = {userID}, " +
                                $"PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Schedule_Complete_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_APPROVED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Rescheduled = 1 " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.PM_COMPLETED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string scheduleQuery = "SELECT PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
                                    $"FROM pm_schedule WHERE id = {request.id};";
            List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(scheduleQuery).ConfigureAwait(false);
            scheduleData[0].schedule_date = UtilsRepository.Reschedule(scheduleData[0].schedule_date, scheduleData[0].frequency_id);
            CMSetScheduleData newData = CreateScheduleData(scheduleData[0]);
            List<CMDefaultResponse> createResponse = await _pmScheduleRepo.SetScheduleData(newData, userID);
            string prevScheduleQry = $"UPDATE pm_schedule SET Prev_Schedule_id = {request.id} WHERE id = {createResponse[0].id[0]}";
            await Context.ExecuteNonQry<int>(prevScheduleQry).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, $"PM Schedule Approved to PMSCH{createResponse[0].id[0]}", CMMS.CMMS_Status.PM_APPROVED, userID);
            //CMPMScheduleViewDetail _PMList = await GetPMTaskDetail(request.id);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_APPROVED, _PMList);

            CMRescheduleApprovalResponse response = new CMRescheduleApprovalResponse(createResponse[0].id[0], request.id, retCode, $"PM Schedule Approved to PMSCH{createResponse[0].id[0]}");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table as Rejection
             * Code goes here
            */

            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_COMPLETED)
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a closed PM schedule can be Rejected");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PM_Schedule_Completed_by_id = {userID}, " +
                                $"PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Schedule_Complete_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_REJECTED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.PM_COMPLETED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, "PM Schedule Rejected", CMMS.CMMS_Status.PM_REJECTED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Schedule Rejected");
            return response;
        }
        private CMSetScheduleData CreateScheduleData(ScheduleIDData scheduleData)
        {
            CMSetScheduleData schData = new CMSetScheduleData();
            schData.facility_id = scheduleData.facility_id;
            schData.asset_schedules = new List<CMScheduleData>();
            CMScheduleData asset_schedule = new CMScheduleData();
            asset_schedule.asset_id = scheduleData.asset_id;
            asset_schedule.asset_name = null;
            asset_schedule.category_id = 0;
            asset_schedule.category_name = null;
            asset_schedule.frequency_dates = new List<ScheduleFrequencyData>();
            ScheduleFrequencyData schedule = new ScheduleFrequencyData();
            schedule.frequency_name = null;
            schedule.frequency_id = scheduleData.frequency_id;
            schedule.schedule_id = 0;
            schedule.schedule_date = scheduleData.schedule_date;
            asset_schedule.frequency_dates.Add(schedule);
            schData.asset_schedules.Add(asset_schedule);
            return schData;
        }
    }

}
