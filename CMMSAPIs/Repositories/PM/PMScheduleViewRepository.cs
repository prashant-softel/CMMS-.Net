using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.Utils;
using System;
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
        Dictionary<int, string> statusList = new Dictionary<int, string>()
        {
            { 171, "PM Started" },
            { 172, "PM Saved" },
            { 173, "PM Submitted" },
            { 174, "PM Approved" },
            { 175, "PM Rejected" },
            { 176, "PM Completed" },
            { 177, "Permit Timed Out" }
        };
        internal async Task<List<CMPMScheduleView>> GetScheduleViewList(int facility_id, DateTime? start_date, DateTime? end_date)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */
            string statusQry = "CASE ";
            foreach (KeyValuePair<int, string> status in statusList)
                statusQry += $"WHEN {status.Key} THEN '{status.Value}' ";
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

        internal async Task<CMDefaultResponse> CancelPMScheduleView(CMApproval request, int userID)
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

        internal async Task<CMPMScheduleViewDetail> GetPMScheduleViewDetail(int schedule_id)
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
            foreach (KeyValuePair<int, string> status in statusList)
                statusQry += $"WHEN {status.Key} THEN '{status.Value}' ";
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

        internal async Task<CMDefaultResponse> SetPMScheduleView(CMPMScheduleExecution request)
        {
            /*
             * Primary Table - PMExecution
             * Add all the details present in CMPMScheduleExecution model
             * Code goes here
            */

            return null;
        }

        internal async Task<CMDefaultResponse> UpdatePMScheduleExecution(CMPMScheduleExecution request)
        {
            /*
             * Primary Table - PMExecution
             * Update all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApprovePMScheduleExecution(CMApproval request)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for approval
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectPMScheduleExecution(CMApproval request)
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
