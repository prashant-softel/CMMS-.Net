using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
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
        public PMScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        Dictionary<CMMS.CMMS_Status, string> statusList = new Dictionary<CMMS.CMMS_Status, string>()
        {
            { CMMS.CMMS_Status.PM_START, "PM Started" },
            { CMMS.CMMS_Status.PM_UPDATE, "PM Updated" },
            { CMMS.CMMS_Status.PM_SUBMIT, "PM Submitted" },
            { CMMS.CMMS_Status.PM_APPROVE, "PM Approved" },
            { CMMS.CMMS_Status.PM_REJECT, "PM Rejected" },
            { CMMS.CMMS_Status.PM_COMPLETED, "PM Completed" },
            { CMMS.CMMS_Status.PM_CANCELLED, "PM Cancelled" },
            { CMMS.CMMS_Status.PM_PTW_TIMEOUT, "Permit Timed Out" }
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
            string userQuery = $"SELECT " +
                                $"u.id, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, mobileNumber as contact_no, r.name as role_name " +
                               $"FROM " +
                                $"Users as u " +
                               $"LEFT JOIN " +
                                $"UserRoles as r ON u.roleId = r.id " + 
                               $"WHERE u.id = {userID}";
            List<CMUser> user = await Context.GetData<CMUser>(userQuery).ConfigureAwait(false);
            string myQuery = "UPDATE pm_schedule SET " + 
                                $"PM_Schedule_cancel_by_id = {userID}, " + 
                                $"PM_Schedule_cancel_by_Name = '{user[0].full_name}', " + 
                                $"PM_Schedule_cancel_by_Code = 'USER{userID}', " + 
                                $"PM_Schedule_cancel_by_Code = '{UtilsRepository.GetUTCTime()}', " + 
                                $"PM_Schedule_cancel_status = 1, " + 
                                $"PM_Schedule_cancel_Reccomendations = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CANCELLED} " + 
                                $"WHERE id = {request.id} AND status in (172,173);";
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
            string myQuery2 = "SELECT Check_Point_id as check_point_id, Check_Point_Name as check_point_name, Check_Point_Requirement as requirement, PM_Schedule_Observation as observation, job_created as is_job_created, custom_checkpoint as is_custom_check_point " + 
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
        internal async Task<CMDefaultResponse> AddCustomCheckpoint(CMPMScheduleExecution request)
        {
            /*
             * Primary Table - PMExecution
             * Add a custom checkpoint
             * Code goes here
            */

            return null;
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
                string startQry = $"UPDATE pm_schedule SET PM_Execution_Started_by_id = {userID}, PM_Execution_Started_date = '{UtilsRepository.GetUTCTime()}' WHERE id = {schedule_id};";
                await Context.ExecuteNonQry<int>(startQry).ConfigureAwait(false);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule_id, CMMS.CMMS_Modules.INVENTORY, schedule_details[0].asset_id, "PM Execution Started", CMMS.CMMS_Status.PM_START, userID);
                response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"Execution PMSCH{schedule_id} Started Successfully");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> UpdatePMTaskExecution(CMPMScheduleExecution request)
        {
            /*
             * Primary Table - PMExecution
             * Update all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApprovePMTaskExecution(CMApproval request)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for approval
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table as Rejection
             * Code goes here
            */
            return null;
        }
    }

}
