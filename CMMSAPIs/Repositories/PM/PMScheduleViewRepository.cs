using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.PM
{
    public class PMScheduleViewRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private PMRepository _pmScheduleRepo;
        public PMScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _pmScheduleRepo = new PMRepository(sqlDBHelper);
        }
        Dictionary<CMMS.CMMS_Status, string> statusList = new Dictionary<CMMS.CMMS_Status, string>()
        {
            { CMMS.CMMS_Status.PM_START, "PM Started" },
            { CMMS.CMMS_Status.PM_UPDATE, "PM Updated" },
            { CMMS.CMMS_Status.PM_SUBMIT, "PM Submitted" },
            { CMMS.CMMS_Status.PM_LINK_PTW, "PM Linked to PTW" },
            { CMMS.CMMS_Status.PM_APPROVE, "PM Approved" },
            { CMMS.CMMS_Status.PM_REJECT, "PM Rejected" },
            { CMMS.CMMS_Status.PM_COMPLETED, "PM Completed" },
            { CMMS.CMMS_Status.PM_CANCELLED, "PM Cancelled" },
            { CMMS.CMMS_Status.PM_PTW_TIMEOUT, "Permit Timed Out" },
            { CMMS.CMMS_Status.PM_DELETED, "PM Deleted" }
        };
        internal async Task<List<CMPMScheduleView>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */
            string statusQry = "CASE ";
            foreach (KeyValuePair<CMMS.CMMS_Status, string> status in statusList)
                statusQry += $"WHEN {(int)status.Key} THEN '{status.Value}' ";
            statusQry += "ELSE 'Unknown Status' END";
            string myQuery = $"SELECT id, PM_Maintenance_Order_Number as maintenance_order_number, PM_Schedule_date as schedule_date, PM_Schedule_Completed_date as completed_date, Asset_Name as equipment_name, Asset_Category_name as category_name, PM_Frequecy_Name as frequency_name, PM_Schedule_Emp_name as assigned_to_name, PTW_id as permit_id, {statusQry} as status_name " + 
                                "FROM pm_schedule ";
            if(facility_id > 0)
            {
                myQuery += $"WHERE Facility_id = {facility_id} ";
                if(start_date != null && end_date != null)
                {
                    if (start_date > end_date)
                        throw new ArgumentException("Start date should be earlier than end date");
                    string start = ((DateTime)start_date).ToString("yyyy'-'MM'-'dd");
                    string end = ((DateTime)end_date).ToString("yyyy'-'MM'-'dd");
                    myQuery += $"AND PM_Schedule_date >= '{start}' AND PM_Schedule_date <= '{end}'";
                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            List<CMPMScheduleView> scheduleViewList = await Context.GetData<CMPMScheduleView>(myQuery).ConfigureAwait(false);
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
            if (status != CMMS.CMMS_Status.PM_PTW_TIMEOUT && status != CMMS.CMMS_Status.PM_LINK_PTW && status != CMMS.CMMS_Status.PM_SUBMIT)
                throw new ArgumentException("Only a PM schedule that has not been executed can be cancelled.");
            string myQuery = "UPDATE pm_schedule SET " + 
                                $"PM_Schedule_cancel_by_id = {userID}, " +  
                                $"PM_Schedule_cancel_date = '{UtilsRepository.GetUTCTime()}', " + 
                                $"PM_Schedule_cancel_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CANCELLED} " + 
                                $"WHERE id = {request.id} AND status IN ({(int)CMMS.CMMS_Status.PM_LINK_PTW},{(int)CMMS.CMMS_Status.PM_SUBMIT},{(int)CMMS.CMMS_Status.PM_PTW_TIMEOUT});";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if(retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, "PM Schedule Cancelled", CMMS.CMMS_Status.PM_CANCELLED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Schedule cancelled successfully");
            return response;
        }

        internal async Task<CMPMScheduleViewDetail> GetPMTaskDetail(int schedule_id)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Other supporting tables - Facility, Asset, AssetCategory, Users
             * Read All properties mention in model and return list
             * Code goes here
            */
            if (schedule_id <= 0)
                throw new ArgumentException("Invalid Schedule ID");
            string statusQry = "CASE ";
            foreach (KeyValuePair<CMMS.CMMS_Status, string> status in statusList)
                statusQry += $"WHEN {(int)status.Key} THEN '{status.Value}' ";
            statusQry += "ELSE 'Unknown Status' END";
            string myQuery1 = $"SELECT id, PM_Maintenance_Order_Number as maintenance_order_number, PM_Schedule_date as schedule_date, PM_Schedule_Completed_date as completed_date, Asset_Name as equipment_name, Asset_Category_name as category_name, PM_Frequecy_Name as frequency_name, PM_Schedule_Emp_name as assigned_to_name, PTW_id as permit_id, {statusQry} as status_name, Facility_Name as facility_name " +
                                $"FROM pm_schedule WHERE id = {schedule_id};";
            List<CMPMScheduleViewDetail> scheduleViewDetail = await Context.GetData<CMPMScheduleViewDetail>(myQuery1).ConfigureAwait(false);
            string myQuery2 = "SELECT Check_Point_id as check_point_id, Check_Point_Name as check_point_name, Check_Point_Requirement as requirement, PM_Schedule_Observation as observation, job_created as is_job_created, custom_checkpoint as is_custom_check_point, file_required as is_file_required " + 
                                $"FROM pm_execution WHERE PM_Schedule_Id = {schedule_id};";
            List<ScheduleCheckList> scheduleCheckList = await Context.GetData<ScheduleCheckList>(myQuery2).ConfigureAwait(false);
            if (scheduleCheckList.Count == 0)
                throw new MissingMemberException("PM Schedule not found");
            List<CMLog> log = await _utilsRepo.GetHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule_id);
            scheduleViewDetail[0].schedule_check_list = scheduleCheckList;
            scheduleViewDetail[0].history_log = log;
            return scheduleViewDetail[0];
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
                                    "LEFT JOIN " +
                                        "permitlotoassets as loto on ptw.LOTOId = loto.id " +
                                    $"WHERE ptw.id = {permit_id} AND ptw.facilityId = {scheduleData[0].facility_id} AND loto.Loto_Asset_id = {scheduleData[0].asset_id};";
            List<ScheduleLinkedPermit> permit = await Context.GetData<ScheduleLinkedPermit>(permitQuery).ConfigureAwait(false);
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PTW_id = {permit[0].ptw_id}, " +
                                $"PTW_Code = '{permit[0].ptw_code}', " +
                                $"PTW_Ttitle = '{permit[0].ptw_title}', " +
                                $"PTW_by_id = {userID}, " +
                                $"PTW_Status = {permit[0].status}, " +
                                $"PTW_Attached_At = '{UtilsRepository.GetUTCTime()}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_LINK_PTW} " +
                                $"WHERE id = {schedule_id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule_id, 0, 0, "PTW linked to PM", CMMS.CMMS_Status.PM_LINK_PTW, userID);
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
            if (status != CMMS.CMMS_Status.PM_START)
                throw new ArgumentException("Execution must be in progress to add a custom checkpoint");
            string myQuery = "INSERT INTO pm_execution (PM_Schedule_Id, PM_Schedule_Code, Check_Point_Name, custom_checkpoint, file_required, Status, Check_Point_Requirement) " +
                                $"VALUES ({request.schedule_id}, 'PMSCH{request.schedule_id}', '{request.check_point_name}', 1, {request.is_document_required}, 1, '{request.requirement}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt2 = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.CHECKPOINTS, 0, "Custom Checkpoint added", CMMS.CMMS_Status.PM_UPDATE, userID);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, $"Custom checkpoint added successfully to PM Schedule PMSCH{request.schedule_id}");
        }

        internal async Task<CMDefaultResponse> SetPMTask(int schedule_id, int userID)
        {
            /*
             * Primary Table - PMExecution
             * Add all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            CMDefaultResponse response;
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {schedule_id}";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_LINK_PTW)
            {
                throw new FieldAccessException("Cannot start execution due to following reasons:" +
                    "\n 1. Permit is not linked to PM" +
                    "\n 2. Permit has been expired" +
                    "\n 3. Execution has already been started" +
                    "\n 4. Execution is completed");
            }
            string getParamsQry = "SELECT id as schedule_id, PM_Frequecy_id as frequency_id, Facility_id as facility_id, Asset_Category_id as category_id, Asset_id as asset_id, PM_Schedule_date as schedule_date " +
                                $"FROM pm_schedule WHERE id = {schedule_id};";
            List<ScheduleIDData> schedule_details = await Context.GetData<ScheduleIDData>(getParamsQry).ConfigureAwait(false);
            string checkpointsQuery = "SELECT checkpoint.id, checkpoint.check_point, checkpoint.check_list_id as checklist_id, checkpoint.requirement, checkpoint.is_document_required, checkpoint.status " +
                                        "FROM checkpoint " +
                                        "JOIN checklist_mapping as map ON map.checklist_id=checkpoint.check_list_id " +
                                        "JOIN checklist_number as checklist ON checklist.id=map.checklist_id " +
                                        $"WHERE map.facility_id = {schedule_details[0].facility_id} AND map.category_id = {schedule_details[0].category_id} AND checklist.frequency_id = {schedule_details[0].frequency_id};";
            List<CMCreateCheckPoint> checkpointList = await Context.GetData<CMCreateCheckPoint>(checkpointsQuery).ConfigureAwait(false);
            if(checkpointList.Count == 0)
            {
                response = new CMDefaultResponse(schedule_id, CMMS.RETRUNSTATUS.FAILURE, "No checklist or checkpoints found");
            }
            else
            {
                List<int> idList = new List<int>();
                foreach(CMCreateCheckPoint checkpoint in checkpointList)
                {
                    string executeQuery = "INSERT INTO pm_execution(PM_Schedule_Id, PM_Schedule_Code, Check_Point_id, Check_Point_Name, " +
                                            "Check_Point_Code, Status, Check_Point_Requirement) VALUES " + 
                                            $"({schedule_id}, 'PMSCH{schedule_id}', {checkpoint.id}, " +
                                            $"'{checkpoint.check_point}', 'CP{checkpoint.id}', 1, '{checkpoint.requirement}'); " +
                                            $"SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(executeQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dt2.Rows[0][0]);
                    idList.Add(id);
                }
                string startQry = $"UPDATE pm_schedule SET PM_Execution_Started_by_id = {userID}, PM_Execution_Started_date = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.PM_START} WHERE id = {schedule_id};";
                await Context.ExecuteNonQry<int>(startQry).ConfigureAwait(false);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule_id, CMMS.CMMS_Modules.INVENTORY, schedule_details[0].asset_id, "PM Execution Started", CMMS.CMMS_Status.PM_START, userID);
                response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"Execution PMSCH{schedule_id} Started Successfully");
            }
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
            string statusQry = $"SELECT status FROM pm_schedule WHERE id = {request.schedule_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_START)
                throw new ArgumentException("Execution must be in progress to modify execution details");
            string executeQuery = $"SELECT id FROM pm_execution WHERE PM_Schedule_Id = {request.schedule_id};";
            DataTable dt = await Context.FetchData(executeQuery).ConfigureAwait(false);
            List<int> executeIds = dt.GetColumn<int>("id");
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
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
                            updateQry += $"PM_Schedule_Observation_update_by = {userID}, " +
                                        $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}', " +
                                        $"PM_Schedule_Observation = '{schedule_detail.observation}' ";
                            message = "Observation Updated";
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Updated Successfully");
                        }
                        updateQry += $"WHERE id = {schedule_detail.execution_id};";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, message, CMMS.CMMS_Status.PM_UPDATE, userID);
                        responseList.Add(response);
                        changeFlag++;
                    }
                    if(schedule_detail.job_create == 1 && execution_details[0].job_create == 0)
                    {
                        string updateQry = $"UPDATE pm_execution SET job_created = 1 WHERE id = {schedule_detail.execution_id};";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, "Job Created for PM", CMMS.CMMS_Status.PM_UPDATE, userID);
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Created for PM Successfully");
                        responseList.Add(response);
                        changeFlag++;
                    }
                    if (schedule_detail.pm_files.Count != 0)
                    {
                        foreach(var file in schedule_detail.pm_files)
                        {
                            string insertFile = "INSERT INTO pm_schedule_files(PM_Schedule_id, PM_Schedule_Code, PM_Execution_id, File_id, PM_Event, " +
                                                "File_Discription, File_added_by, File_added_date, File_Server_id, File_Server_Path) VALUES " +
                                                $"({request.schedule_id}, 'PMSCH{request.schedule_id}', {schedule_detail.execution_id}, {file.file_id}, " +
                                                $"{(int)file.pm_event}, '{file.file_desc}', {userID}, '{UtilsRepository.GetUTCTime()}', 1, " +
                                                $"'http://cms_test.com/' ); SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(insertFile).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt2.Rows[0][0]);
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
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"{schedule_detail.pm_files.Count} file(s) attached to PMSCH{request.schedule_id}", CMMS.CMMS_Status.PM_UPDATE, userID);
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"{schedule_detail.pm_files.Count} file(s) attached to PM Successfully");
                        responseList.Add(response);
                        changeFlag++;
                    }
                    if(changeFlag == 0)
                    {
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, "No changes");
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
            if (status != CMMS.CMMS_Status.PM_START)
                throw new ArgumentException("Only a PM schedule under execution can be closed");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PM_Schedule_Completed_by_id = {userID}, " +
                                $"PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Schedule_Complete_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_COMPLETED} " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.PM_START};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, "PM Schedule Closed", CMMS.CMMS_Status.PM_COMPLETED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Schedule Closed successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> ApprovePMTaskExecution(CMApproval request, int userID)
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
                throw new ArgumentException("Only a closed PM schedule can be Approved");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PM_Schedule_Completed_by_id = {userID}, " +
                                $"PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Schedule_Complete_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_APPROVE} ," +
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
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, $"PM Schedule Approved to PMSCH{createResponse[0].id[0]}", CMMS.CMMS_Status.PM_APPROVE, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, $"PM Schedule Approved to PMSCH{createResponse[0].id[0]}");
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
                throw new ArgumentException("Only a closed PM schedule can be Rejected");
            string myQuery = "UPDATE pm_schedule SET " +
                                $"PM_Schedule_Completed_by_id = {userID}, " +
                                $"PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}', " +
                                $"PM_Schedule_Complete_Recomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_REJECT} " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.PM_COMPLETED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.id, 0, 0, "PM Schedule Rejected", CMMS.CMMS_Status.PM_REJECT, userID);
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
