using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.PM
{
    public class PMScheduleViewRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private PMRepository _pmScheduleRepo;
        private JobRepository _jobRepo;
        public static IWebHostEnvironment _environment;

        public PMScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _pmScheduleRepo = new PMRepository(sqlDBHelper, _environment);
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
                    retValue = "Close - Waiting for Approval"; break;
                case CMMS.CMMS_Status.PM_REJECTED:
                    retValue = "Rejected"; break;
                case CMMS.CMMS_Status.PM_APPROVED:
                    retValue = "Approved"; break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                    retValue = "Closed - Rejected"; break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                    retValue = "Closed - Approved"; break;
                case CMMS.CMMS_Status.PM_CANCELLED:
                    retValue = "Cancelled"; break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue = "Cancelled - Rejected"; break;
                case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                    retValue = "Cancelled - Approved"; break;
                case CMMS.CMMS_Status.PM_DELETED:
                    retValue = "Deleted"; break;
                case CMMS.CMMS_Status.PM_UPDATED:
                    retValue = "Updated"; break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = "Deleted"; break;
                default:
                    break;
            }
            return retValue;

        }

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMScheduleExecutionDetail Obj)
        {
            string retValue = " ";


            switch (notificationID)
            {
                case CMMS.CMMS_Status.PM_SCHEDULED:
                    retValue += String.Format("PMS{0} Schedule </p>", Obj.schedule_id); break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue += String.Format("PMS{0} Assigned </p>", Obj.schedule_id);
                    break;
                case CMMS.CMMS_Status.PM_COMPLETED:
                    retValue += String.Format("PMS{0} Completed By {1} </p>", Obj.schedule_id, Obj.completedBy_name); ;
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:
                    retValue += String.Format("PMS{0} Submitted By {1} </p>", Obj.schedule_id, Obj.submittedByName);
                    break;
                case CMMS.CMMS_Status.PM_START:
                    retValue += String.Format("PMS{0} Started By {1} </p>", Obj.schedule_id, Obj.PM_Execution_Started_by_name);
                    break;
                case CMMS.CMMS_Status.PM_REJECTED:
                    retValue += String.Format("PMS{0} Rejected By {1} </p>", Obj.schedule_id, Obj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_APPROVED:
                    retValue += String.Format("PMS{0} Approved By {1} </p>", Obj.schedule_id, Obj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue += String.Format("PMS{0} Cancelled Rejected By {1} </p>", Obj.schedule_id,Obj.cancelledrejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                    retValue += String.Format("PMS{0} Cancelled Approved By {1} </p>", Obj.schedule_id, Obj.cancelledapprovedbyName);
                    break;
                case CMMS.CMMS_Status.PM_DELETED:
                    retValue += String.Format("PMS{0} Deleted </p>", Obj.schedule_id);
                    break;
                case CMMS.CMMS_Status.PM_UPDATED:
                    retValue += String.Format("PMS{0} Updated By {1} </p>", Obj.schedule_id,Obj.PM_Schedule_updated_by);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMTaskView Obj)
        {
            string retValue = " ";


            switch (notificationID)
            {
                case CMMS.CMMS_Status.PM_SCHEDULED:
                    retValue = $"PM Task Scheduled "; break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = $"PM Task Assigned To {Obj.assigned_to_name} "; break;
                case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                    retValue = $"Permit {Obj.id} Linked To Permit Id {Obj.permit_id} "; break;
                case CMMS.CMMS_Status.PM_START:
                    retValue = $"PM Task Started By {Obj.started_by_name}"; break;
                case CMMS.CMMS_Status.PM_COMPLETED:
                    retValue = $"PM Task Closed By {Obj.closed_by_name} "; break;
                case CMMS.CMMS_Status.PM_REJECTED:
                    retValue = $"PM Task Rejected By {Obj.rejected_by_name} "; break;
                case CMMS.CMMS_Status.PM_APPROVED:
                    retValue = $"PM Task Approved By {Obj.approved_by}"; break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                    retValue = $"PM Task Closed Rejected By {Obj.closeRejectedbyName}"; break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                    retValue = $"PM Task Closed Approved By {Obj.closedApprovedByName}"; break;
                case CMMS.CMMS_Status.PM_CANCELLED:
                    retValue = $"PM Task Cancelled By {Obj.cancelled_by_name} "; break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue = $"PM Task Cancelled Rejected By {Obj.cancelledrejectedbyName}"; break;
                case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                    retValue = $"PM Task cancelled Approved By {Obj.cancelledapprovedbyName}"; break;
                case CMMS.CMMS_Status.PM_UPDATED:
                    retValue = $"PM Task Updated By {Obj.updated_by_name} "; break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = $"PM Task Deleted By {Obj.deletedbyName} "; break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMPMTaskList>> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds, int userID, bool self_view, string facilitytimeZone)
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
            //  
            string myQuery = $"SELECT pm_task.id,pm_plan.id as plan_id, pm_task.category_id,cat.name as category_name,  CONCAT('PMTASK',pm_task.id) as task_code,pm_plan.plan_name as plan_title,pm_task.facility_id, pm_task.frequency_id as frequency_id, freq.name as frequency_name, pm_task.plan_date as due_date,prev_task_done_date as last_done_date, closed_at as done_date, CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assigned_to_name, pm_task.PTW_id as permit_id, CONCAT('PTW',pm_task.PTW_id) as permit_code,permit.status as ptw_status, PM_task.status " +
                               ", f.name as Site_name,pm_task.started_at as start_date,pm_task.closed_at as close_time,CONCAT(isotak.firstName,isotak.lastName) as Isolation_taken,permitType.title as permit_type FROM pm_task " +
                               $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                               $"left join pm_plan  on pm_task.plan_id = pm_plan.id " +
                               $"left join assetcategories as cat  on pm_task.category_id = cat.id " +
                               $"left join permits as permit on pm_task.PTW_id = permit.id " +
                               "left join facilities as f on f.id=pm_task.facility_id " +
                               "LEFT JOIN permittypelists as permitType ON permitType.id = permit.typeId  " +
                               "LEFT JOIN users as isotak ON isotak.id = permit.physicalIsolation " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id where 1 ";

            // myQuery += (frequencyIds.Length > 0 ? " AND freq.id IN ( '" + string.Join("' , '", frequencyIds) + "' )" : string.Empty);

            List<CMPMTaskList> scheduleViewList = new List<CMPMTaskList>();
            if (facility_id > 0)
            {
                myQuery += $" and pm_task.Facility_id = {facility_id} and pm_task.category_id!=0 ";
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

                if (categoryIds != "" && categoryIds != null)
                {

                    myQuery += $"AND pm_task.category_id in ({categoryIds}) ";
                }
                if (self_view)
                    myQuery += $"AND (pm_task.assigned_to={userID});";

                scheduleViewList = await Context.GetData<CMPMTaskList>(myQuery).ConfigureAwait(false);


                foreach (var task in scheduleViewList)
                {
                    if (task.status == (int)CMMS.CMMS_Status.PM_LINKED_TO_PTW)
                    {
                        if (task.ptw_status == (int)CMMS.CMMS_Status.PTW_APPROVED)
                        {
                            string startQry2 = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.PM_APPROVED} WHERE id = {task.id};";
                            await Context.ExecuteNonQry<int>(startQry2).ConfigureAwait(false);
                        }
                        task.status_short = "Permit - " + PermitRepository.getShortStatus(task.ptw_status);
                    }
                    else
                    {
                        CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(task.status);
                        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
                        task.status_short = _shortStatus;
                    }

                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            foreach (var detail in scheduleViewList)
            {
                if (detail != null && detail.due_date != null)
                {
                    detail.due_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.due_date);
                }
                if (detail != null && detail.done_date != null)
                {
                    detail.done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.done_date);
                }
                if (detail != null && detail.last_done_date != null)
                {
                    detail.last_done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.last_done_date);
                }
            }

            return scheduleViewList;
        }



        internal async Task<CMDefaultResponse> CancelPMTask(CMApproval request, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMSchedule
             * Delete the requested id from primary table
             * Code goes here
            */
            int retVal = 0;
            string statusQry = $"SELECT status FROM pm_task WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);


            if (status != CMMS.CMMS_Status.RESCHEDULED_TASK && status != CMMS.CMMS_Status.PM_CLOSE_APPROVED)
            {
                // return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a PM Task that has not been executed can be cancelled.");
                string myQuery = "UPDATE pm_task SET " +
                                    $"cancelled_by = {userID}, " +
                                    $"cancelled_at = '{UtilsRepository.GetUTCTime()}', " +
                                    $"cancel_remarks = '{request.comment}', " +
                                    $"status = {(int)CMMS.CMMS_Status.PM_CANCELLED}, " +
                                    $"rescheduled = 1 , " +
                                    $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                    $"status_updated_by = {userID} " +
                                    $"WHERE id = {request.id} ";

                retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            }
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            string mainQuery = $"INSERT INTO pm_task(plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to,status,prev_task_id,prev_task_done_date)  " +
                             $"select plan_id as plan_id,facility_id,category_id,frequency_id,CASE when  pm_task.frequency_id in( 4,5,6)  then\r\nDATE_ADD(plan_date, INTERVAL freq.months MONTH) \r\n  WHEN pm_task.frequency_id = 7 THEN \r\n DATE_ADD(plan_date, INTERVAL 1 YEAR)\r\nelse DATE_ADD(plan_date, INTERVAL freq.days DAY) end as plan_date,assigned_to,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status,{request.id} as prev_task_id, '{UtilsRepository.GetUTCTime()}' as prev_task_done_date from pm_task " +
                             $"left join frequency as freq on pm_task.frequency_id = freq.id where pm_task.id = {request.id}; " +
                             $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                $"select {id} as task_id, plan_id, Asset_id, checklist_id, PM_Schedule_date,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status from pm_schedule where task_id = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            
            
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Task Cancelled" : request.comment, CMMS.CMMS_Status.PM_CANCELLED, userID);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_CANCELLED, new[] { userID }, _PMTaskList);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Task cancelled successfully");
            return response;
        }

        internal async Task<CMPMTaskView> GetPMTaskDetail(int task_id, string facilitytimeZone)
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

            string myQuery = $"SELECT pm_task.id, CONCAT('PMTASK',pm_task.id) as task_code, pm_plan.id as plan_id,facilities.name as site_name, pm_task.category_id,cat.name as category_name, pm_plan.plan_name as plan_title, pm_task.facility_id, pm_task.frequency_id as frequency_id, freq.name as frequency_name, pm_task.plan_date as due_date,closed_at as done_date, CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assigned_to_name, CONCAT(closedBy.firstName,' ',closedBy.lastName)  as closed_by_name, pm_task.closed_at , CONCAT(approvedBy.firstName,' ',approvedBy.lastName)  as approved_by_name, pm_task.approved_at ,CONCAT(rejectedBy.firstName,' ',rejectedBy.lastName)  as rejected_by_name, pm_task.rejected_at ,CONCAT(cancelledBy.firstName,' ',cancelledBy.lastName)  as cancelled_by_name, pm_task.cancelled_at , pm_task.rejected_at ,CONCAT(startedBy.firstName,' ',startedBy.lastName)  as started_by_name, pm_task.started_at , pm_task.PTW_id as permit_id, CONCAT('PTW',pm_task.PTW_id) as permit_code,permit.status as ptw_status, PM_task.status, {statusQry} as status_short, " +
                               "  CONCAT(tbtDone.firstName,' ',tbtDone.lastName)  as tbt_by_name, Case when permit.TBT_Done_By is null or  permit.TBT_Done_By =0 then 0 else 1 end ptw_tbt_done " +
                               " , permittypelists.title as permit_type,ptwu.id as Employee_ID,CONCAT(ptwu.firstName,ptwu.lastName) as Employee_name,bus.name as Company, " +
                               " passt.name as Isolated_equipments,CONCAT(tbtDone.firstName,' ',tbtDone.lastName) as TBT_conducted_by_name,permit.TBT_Done_At as TBT_done_time,permit.startDate Start_time,permit.description as workdescription ,pm_task.close_remarks as new_remark   ," +
                               " permit.status as status_PTW, CONCAT(isotak.firstName, ' ',isotak.lastName) as Isolation_taken, " +
                               " CONCAT(cancelledrejectedBy.firstName,' ',cancelledrejectedBy.lastName)  as cancelledrejectedbyName, " +
                               " CONCAT(completedBy.firstName,' ',completedBy.lastName)  as completedbyName, " +
                               " CONCAT(cancelledapprovedBy.firstName,' ',cancelledapprovedBy.lastName)  as cancelledapprovedbyName, " +
                               " CONCAT(createdBy.firstName,' ',createdBy.lastName)  as createdbyName, " +
                               " CONCAT(deletedBy.firstName,' ',deletedBy.lastName)  as deletedbyName,  CONCAT(closeRejected.firstName,' ', closeRejected.lastName) AS closeRejectedbyName, pm_task.status " +
                               " FROM pm_task " +
                               $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                               $"left join users as closedBy on pm_task.closed_by = closedBy.id " +
                               $"left join users as approvedBy on pm_task.approved_by = approvedBy.id " +
                               $"left join users as rejectedBy on pm_task.rejected_by = rejectedBy.id " +
                               $"left join users as cancelledBy on pm_task.cancelled_by = cancelledBy.id " +
                               $"left join users as cancelledrejectedBy on pm_task.cancel_rejected_by = cancelledrejectedBy.id " +
                               $"left join users as completedBy on completedBy.id = pm_task.completedById " +
                               $"left join users as cancelledapprovedBy on cancelledapprovedBy.id = pm_task.cancel_approved_by " +
                               $"left join users as createdBy on createdBy.id = pm_task.createdById " + 
                               $"left join users as closed on pm_task.closed_by = closed.id " +
                               $"left join users as startedBy on pm_task.started_by = startedBy.id " +
                               $"left join users as deletedBy on pm_task.deletedById = deletedBy.id " +
                               $"left join permits as permit on pm_task.PTW_id = permit.id " +
                               $"left join pm_plan  on pm_task.plan_id = pm_plan.id " +
                               $"left join facilities  on pm_task.facility_id =facilities.id " +
                               $"left join assetcategories as cat  on pm_task.category_id = cat.id " +
                               $"Left join users as ptwu on permit.issuedById = ptwu.id " +
                               $"LEFT join  assets as passt on permit.physicalIsoEquips = passt.id  " +
                               $"Left join users as isotak on permit.physicalIsolation = isotak.id  " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id " +
                               $"left join users as tbtDone on permit.TBT_Done_By = tbtDone.id " +
                               $"left join business as bus on bus.id = ptwu.companyId " +
                               $"left join permittypelists on permittypelists.id = permit.typeId  " +
                               $"LEFT JOIN users AS closeRejected ON pm_task.close_rejected_by_id = closeRejected.id " +
                               $" where pm_task.id = {task_id} ";

            List<CMPMTaskView> taskViewDetail = await Context.GetData<CMPMTaskView>(myQuery).ConfigureAwait(false);
            string Materialconsumption = "SELECT sam.ID as Material_ID,sam.asset_name as  Material_name,smi.asset_item_ID as Equipment_ID, " +
                    "smtype.asset_type as  Material_type, smi.used_qty,smi.issued_qty" +
                    " FROM smassetmasters sam LEFT JOIN smrsitems smi ON sam.ID = smi.mrs_ID " +
                    "Left join smmrs as smm on smm.id=smi.mrs_ID " +
                    "Left join smassettypes as smtype on smtype.ID=sam.asset_type_ID " +
                    "left join smassetitems sai on sai.assetMasterID =  sam.id " +
                    $"WHERE  smm.whereUsedRefID={task_id};";
            List<Materialconsumption> Material = await Context.GetData<Materialconsumption>(Materialconsumption).ConfigureAwait(false);
            if (taskViewDetail.Count == 0)
                throw new MissingMemberException("PM Task not found");

            string myQuery2 = $"SELECT pm_schedule.id as schedule_id,assets.id as assetsID,assets.name as asset_name, " +
                $"PM_Schedule_Completed_by_id as completedBy_id,  CONCAT(users.firstName, users.lastName) as   completedBy_name , asst.name as categoryname, " +
                $" checklist.checklist_number as checklist_name from pm_schedule " +
                $" left join assets on pm_schedule.asset_id = assets.id " +
                $" left join pm_task on pm_task.id = pm_schedule.task_id " +
                $" left join assetcategories as asst on asst.id = pm_task.category_id " +
                $" left join users on pm_schedule.PM_Schedule_Completed_by_id = users.id " +
                $" left join checklist_number as checklist on pm_schedule.checklist_id = checklist.id " +
                $" where task_id = {task_id};";

            List<CMPMScheduleExecutionDetail> checklist_collection = await Context.GetData<CMPMScheduleExecutionDetail>(myQuery2).ConfigureAwait(false);


            //string myQuery2 = $"SELECT DISTINCT checklist.id, checklist.checklist_number AS name FROM pm_execution " + 
            //                    $"JOIN checkpoint on pm_execution.Check_Point_id = checkpoint.id " + 
            //                    $"JOIN checklist_number as checklist ON checklist.id = checkpoint.check_list_id " +
            //                    $"WHERE pm_execution.PM_Schedule_Id = {schedule_id};";

            //List<CMDefaultList> checklist_collection = await Context.GetData<CMDefaultList>(myQuery2).ConfigureAwait(false);
            foreach (var schedule in checklist_collection)
            {
                string myQuery3 = "SELECT checkpoint.min_range,checkpoint.max_range,is_ok as cp_ok, boolean as type_bool, failure_weightage as failure_waightage,type as check_point_type,pm_execution.range as type_range, pm_execution.text as type_text,pm_execution.id as execution_id,checkpoint.type as check_point_type ,pm_execution.range as type_range,pm_execution.text as type_text,pm_execution.is_ok as type_bool, Check_Point_id as check_point_id, Check_Point_Name as check_point_name, Check_Point_Requirement as requirement, PM_Schedule_Observation as observation, job_created as is_job_created, linked_job_id, custom_checkpoint as is_custom_check_point, file_required as is_file_required " +
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

                string myQuery4 = $"SELECT jobs.id as job_id, jobs.title as job_title, jobs.description as job_description, " +
                                  $"CASE WHEN jobs.createdAt = '0000-00-00 00:00:00' THEN NULL ELSE jobs.createdAt END as job_date, " +
                                  $"{jobStatusQry}   AS job_status,  a.assetName AS Tool_name,SUM(a.id) AS No_of_tools  " +
                                    $"FROM jobs " +
                                    $"JOIN pm_execution ON jobs.id = pm_execution.linked_job_id " +
                                    $"LEFT JOIN jobassociatedworktypes  jas on jas.jobId = jobs.id " +
                                    $"LEFT JOIN worktypemasterassets as a ON a.workTypeId = jas.workTypeId " +
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
            taskViewDetail[0].Material_consumption = Material;

            if (taskViewDetail[0].status == (int)CMMS.CMMS_Status.PM_LINKED_TO_PTW)
            {
                if (taskViewDetail[0].ptw_status == (int)CMMS.CMMS_Status.PTW_APPROVED)
                {
                    string startQry2 = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.PM_APPROVED} WHERE id = {taskViewDetail[0].id};";
                    await Context.ExecuteNonQry<int>(startQry2).ConfigureAwait(false);
                }
                taskViewDetail[0].status_short = "Permit - " + PermitRepository.getShortStatus(taskViewDetail[0].ptw_status);
                taskViewDetail[0].status_short = PermitRepository.LongStatus(taskViewDetail[0].ptw_status, null);
                string _shortStatus_PTW = Status_PTW(taskViewDetail[0].ptw_status);
                taskViewDetail[0].status_short_ptw = _shortStatus_PTW;
            }
            else
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(taskViewDetail[0].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_TASK, _Status);
                taskViewDetail[0].status_short = _shortStatus;

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.PM_TASK, _Status, taskViewDetail[0]);
                taskViewDetail[0].status_long = _longStatus;



                string _shortStatus_PTW = Status_PTW(taskViewDetail[0].ptw_status);
                taskViewDetail[0].status_short_ptw = _shortStatus_PTW;

            }
            foreach (var detail in taskViewDetail)
            {
                if (detail != null && detail.done_date != null)
                {
                    detail.done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.done_date);
                }
                if (detail != null && detail.last_done_date != null)
                {
                    detail.last_done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.last_done_date);
                }
                if (detail != null && detail.approved_at != null)
                {
                    detail.approved_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.approved_at);
                }
                if (detail != null && detail.cancelled_at != null)
                {
                    detail.cancelled_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.cancelled_at);
                }

                if (detail != null && detail.closed_at != null)
                {
                    detail.closed_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.closed_at);
                }
                if (detail != null && detail.due_date != null)
                {
                    detail.due_date = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.due_date);
                }
                if (detail != null && detail.rejected_at != null)
                {
                    detail.rejected_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.rejected_at);
                }
                if (detail != null && detail.started_at != null)
                {
                    detail.started_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.started_at);
                }
                if (detail != null && detail.updated_at != null)
                {
                    detail.updated_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.updated_at);
                }
            }
            return taskViewDetail[0];
        }

        internal async Task<CMDefaultResponse> LinkPermitToPMTask(int task_id, int permit_id, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMSchedule
             * Set the required fields in primary table for linling permit to PM
             * Code goes here
            */
            //string scheduleQuery = "SELECT PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
            //                        $"FROM pm_task WHERE id = {task_id};";
            //List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(scheduleQuery).ConfigureAwait(false);

            CMDefaultResponse response;
            string statusQry = $"SELECT status,ifnull(assigned_to,0) assigned_to, ifnull(ptw_id,0) ptw_id FROM pm_task WHERE id = {task_id}";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            int assigned_to = Convert.ToInt32(dt1.Rows[0][1]);
            int ptw_id = Convert.ToInt32(dt1.Rows[0][2]);
            if (assigned_to <= 0)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "PM Task is Not Assigned.");
            }

            if (ptw_id > 0)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "PM Task is already Linked to Permit");
            }


            string permitQuery = "SELECT ptw.id as ptw_id, ptw.code as ptw_code, ptw.title as ptw_title, ptw.status as ptw_status " +
                                    "FROM " +
                                        "permits as ptw " +
                                    $"WHERE ptw.id = {permit_id} ;";
            List<ScheduleLinkedPermit> permit = await Context.GetData<ScheduleLinkedPermit>(permitQuery).ConfigureAwait(false);
            if (permit.Count == 0)
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, $"Permit {permit_id} does not exist.");
            string myQuery = "UPDATE pm_task SET " +
                                $"ptw_id = {permit[0].ptw_id}, " +
                                $"status = {(int)CMMS.CMMS_Status.PM_LINKED_TO_PTW}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = '{userID}' " +
                                $"WHERE id = {task_id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, task_id, CMMS.CMMS_Modules.PTW, permit_id, "PTW linked to PM", CMMS.CMMS_Status.PM_LINK_PTW, userID);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(task_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_LINK_PTW, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            response = new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.SUCCESS, $"Permit {permit_id} linked to Task {task_id}");

            return response;
        }
        internal async Task<CMDefaultResponse> AddCustomCheckpoint(CMCustomCheckPoint request, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution
             * Add a custom checkpoint
             * Code goes here
            */
            string statusQry = $"SELECT task_id, status FROM pm_schedule WHERE id = {request.schedule_id};";
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
            try
            {
                CMPMScheduleExecutionDetail PMTaskSchedule = await GetPMTaskScheduleDetail(request.task_id, request.schedule_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_UPDATED, new[] { userID }, PMTaskSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Schedule Notification: {ex.Message}");
            }
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, $"Custom checkpoint added successfully to PM Schedule PMSCH{request.schedule_id}");
        }

        internal async Task<CMDefaultResponse> StartPMTask(int task_id, int userID, string facilitytimeZone)
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
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status ptw_status = new CMMS.CMMS_Status();
            if (dt1.Rows[0][2].ToString() != "")
            {
                ptw_status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][2]);
            }


            if (ptw_status == CMMS.CMMS_Status.PTW_APPROVED)
            {
                string updateQ = $"UPDATE pm_task SET  status = {(int)CMMS.CMMS_Status.PM_APPROVED} WHERE id = {task_id};";
                await Context.ExecuteNonQry<int>(updateQ).ConfigureAwait(false);
            }
            else
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution due to following reasons:" +
             " \n 1. Permit is not linked to PM" +
             "\n 2. Permit is not Approved" +
             " \n 3. Execution has already been started" +
             " \n 4. Execution is completed");
            }

            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            //if (status !=  CMMS.CMMS_Status.PM_APPROVED)
            //{
            //    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution due to following reasons:" +
            //        " \n 1. Permit is not linked to PM" +
            //        "\n 2. Permit is not Approved" +
            //        " \n 3. Execution has already been started" +
            //        " \n 4. Execution is completed");
            //}
            DateTime expDate = Convert.ToDateTime(dt1.Rows[0]["endDate"]);

            if (expDate <= Convert.ToDateTime(UtilsRepository.GetUTCTime()))
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Cannot start execution : Permit has been expired");
            }

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
                                                $"\"{checkpoint.check_point}\", 'CP{checkpoint.id}', 1, '{checkpoint.requirement}'); " +
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
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(task_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_START, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"Execution PMTASK{task_id} Started Successfully");

            return response;
        }

        internal async Task<List<CMDefaultResponse>> UpdatePMTaskExecution(CMPMExecutionDetail request, int userID, string facilitytimeZone)
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
            if (request.schedules != null)
            {
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
                                try
                                {
                                    CPtypeValue = $" , `text` = '{schedule_detail.text}' ";
                                }
                                catch (Exception ex)
                                {
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id} checkpoint of type requires text.;");
                                    responseList.Add(response);
                                    return responseList;
                                }

                            }

                            else if (Convert.ToInt32(dtType.Rows[0][0]) == 1) // 1 is boolean
                            {
                                try
                                {
                                    //CPtypeValue = $" , boolean = {schedule_detail.boolean} ";
                                    CPtypeValue = $" , boolean = {Convert.ToInt32(schedule_detail.text)} ";
                                }
                                catch (Exception ex)
                                {
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id}  checkpoint of type requires boolean but passed string.");
                                    responseList.Add(response);
                                    return responseList;
                                }

                            }
                            else if (Convert.ToInt32(dtType.Rows[0][0]) == 2) //type ==2 is Range
                            {

                                //CPtypeValue = $" , `range` = {schedule_detail.range} ";
                                try
                                {
                                    CPtypeValue = $" , `range` = {Convert.ToInt32(schedule_detail.text)} ";
                                }
                                catch (Exception ex)
                                {
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id} checkpoint of type requires range but passed string.");
                                    responseList.Add(response);
                                    return responseList;
                                }


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
                                CMDefaultResponse jobResp = await _jobRepo.CreateNewJob(newJob, userID);
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

                    string logHistoryQ = "select  pe.Check_Point_id,pe.Check_Point_Code, ps.Asset_id,asset.name, ps.checklist_id, cn.checklist_number " +
                        " from  pm_schedule ps  " +
                        " left join pm_execution pe on pe.pm_schedule_id = ps.id  " +
                        " left join assets asset on asset.id = ps.Asset_id " +
                        " left join checklist_number cn on cn.id =  ps.checklist_id " +
                        " where pe.id = " + schedule.schedule_id + ";";
                    string checkpointName = "";
                    string assetName = "";
                    string checklistName = "";
                    DataTable dtHistoryLog = await Context.FetchData(logHistoryQ).ConfigureAwait(false);
                    if (dtHistoryLog.Rows.Count > 0)
                    {
                        checkpointName = Convert.ToString(dtHistoryLog.Rows[0][1]);
                        assetName = Convert.ToString(dtHistoryLog.Rows[0][3]);
                        checklistName = Convert.ToString(dtHistoryLog.Rows[0][5]);
                    }
                    string messageLogForHistory = "PM Task Updated : for schedule id: " + schedule.schedule_id + ", asset : " + assetName + ", checkpoint: " + checkpointName + ", checklist : " + checklistName + "";
                    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.task_id, 0, 0, messageLogForHistory, CMMS.CMMS_Status.PM_UPDATED, userID);

                }
            }
            string taskQry = $"update pm_task set updated_by = {userID},updated_at = '{UtilsRepository.GetUTCTime()}', update_remarks = '{request.comment}' WHERE id = {request.task_id};";
            int retVal = await Context.ExecuteNonQry<int>(taskQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.task_id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Task Updated" : request.comment, CMMS.CMMS_Status.PM_UPDATED, userID);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.task_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_TASK_UPDATED, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            CMDefaultResponse responseResult = new CMDefaultResponse(request.task_id, retCode, "PM Task Updated successfully");
            responseList.Add(responseResult);

            return responseList;
        }

        internal async Task<CMDefaultResponse> ClosePMTaskExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for completion
             * Code goes here
            */
            string statusQry = $"SELECT status FROM pm_task WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            //if (status != CMMS.CMMS_Status.PM_START && status != CMMS.CMMS_Status.PM_REJECTED)
            //    return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a rejected PM Task or one under execution can be closed");

            string myQuery = "UPDATE pm_task SET " +
                                $"closed_by = {userID}, " +
                                $"closed_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"close_remarks = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_COMPLETED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID} " +
                                $"WHERE id = {request.id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Task Close Requested " : request.comment, CMMS.CMMS_Status.PM_COMPLETED, userID);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_COMPLETED, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Task Close Requested successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> CancelRejectedPMTaskExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            string myQuery = "UPDATE pm_task SET " +
                                $"cancel_rejected_by = {userID}, " +
                                $"cancel_rejected_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"cancel_remarks = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CANCELLED_REJECTED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID} " +
                                $"WHERE id = {request.id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Task Cancel Rejected " : request.comment, CMMS.CMMS_Status.PM_CANCELLED_REJECTED, userID);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_CANCELLED_REJECTED, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Task Cancel Rejected successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> CancelApprovedPMTaskExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            string myQuery = "UPDATE pm_task SET " +
                                $"cancel_approved_by = {userID}, " +
                                $"cancel_approved_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"approve_remarks = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CANCELLED_APPROVED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID} " +
                                $"WHERE id = {request.id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Task Cancel Rejected " : request.comment, CMMS.CMMS_Status.PM_CANCELLED_APPROVED, userID);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_CANCELLED_APPROVED, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Task Cancel Rejected successfully.");
            return response;
        }

        internal async Task<CMRescheduleApprovalResponse> ApprovePMTaskExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for approval
             * Code goes here
            */
            string statusQry = $"SELECT status FROM pm_task WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.PM_COMPLETED)
                return new CMRescheduleApprovalResponse(0, request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a closed PM Task can be Approved ");

            string myQuery = "UPDATE pm_task SET " +
                                $"approved_by = {userID}, " +
                                $"approved_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"approve_remarks = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CLOSE_APPROVED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID} , " +
                                $"rescheduled = 1 " +
                                $"WHERE id = {request.id};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            //string scheduleQuery = "SELECT plan_date as schedule_date, facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
            //                        $"FROM pm_task WHERE id = {request.id};";
            //List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(scheduleQuery).ConfigureAwait(false);
            //scheduleData[0].schedule_date = UtilsRepository.Reschedule(scheduleData[0].schedule_date, scheduleData[0].frequency_id);
            //CMSetScheduleData newData = CreateScheduleData(scheduleData[0]);
            //List<CMDefaultResponse> createResponse = await _pmScheduleRepo.SetScheduleData(newData, userID);
            //string prevScheduleQry = $"UPDATE pm_schedule SET Prev_Schedule_id = {request.id} WHERE id = {createResponse[0].id[0]}";
            //await Context.ExecuteNonQry<int>(prevScheduleQry).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            string mainQuery = $"INSERT INTO pm_task(plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to,status,prev_task_id,prev_task_done_date)  " +
                               $"select plan_id as plan_id,facility_id,category_id,frequency_id,CASE when  pm_task.frequency_id in( 4,5,6)  then\r\nDATE_ADD(plan_date, INTERVAL freq.months MONTH) \r\n  WHEN pm_task.frequency_id = 7 THEN \r\n DATE_ADD(plan_date, INTERVAL 1 YEAR)\r\nelse DATE_ADD(plan_date, INTERVAL freq.days DAY) end as plan_date,assigned_to,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status,{request.id} as prev_task_id, '{UtilsRepository.GetUTCTime()}' as prev_task_done_date from pm_task " +
                               $"left join frequency as freq on pm_task.frequency_id = freq.id where pm_task.id = {request.id}; " +
                               $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                $"select {id} as task_id, plan_id, Asset_id, checklist_id, PM_Schedule_date,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status from pm_schedule where task_id = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);

            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? $"PM Task Close Approved" : request.comment, CMMS.CMMS_Status.PM_APPROVED, userID);
            //CMPMScheduleViewDetail _PMList = await GetPMTaskDetail(request.id);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_APPROVED, _PMList);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_APPROVED, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            CMRescheduleApprovalResponse response = new CMRescheduleApprovalResponse(id, request.id, retCode, $"PM Task Close Approved to PMTASK{id}");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectPMTaskExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table as Rejection
             * Code goes here
            */

            string statusQry = $"SELECT status FROM pm_task WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            if (status != CMMS.CMMS_Status.PM_COMPLETED)
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a closed PM Task can be Rejected");

            string myQuery = "UPDATE pm_task SET " +
                                $"rejected_by = {userID}, " +
                                $"rejected_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"reject_remarks = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.PM_CLOSE_REJECTED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID}  " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.PM_COMPLETED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Task Close Rejected" : request.comment, CMMS.CMMS_Status.PM_REJECTED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, retCode, "PM Task Close Rejected");
            try
            {
                CMPMTaskView _PMPlanList = await GetPMTaskDetail(request.id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_REJECTED, new[] { userID }, _PMPlanList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            
            return response;
        }

        internal async Task<CMDefaultResponse> AssignPMTask(int task_id, int assign_to, int userID, string facilitytimeZone)
        {
            string statusQry = $"SELECT status FROM pm_task WHERE id = {task_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            if (status != CMMS.CMMS_Status.PM_SCHEDULED && status != CMMS.CMMS_Status.PM_ASSIGNED && status != CMMS.CMMS_Status.PM_LINKED_TO_PTW)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Only Scheduled Tasks can be assigned and Not yet started and Rejected Tasks can be Reassigned ");
            }

            string myQuery = "UPDATE pm_task SET " +
                                $"assigned_to = {assign_to}, " +
                                $"status = {(int)CMMS.CMMS_Status.PM_ASSIGNED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"status_updated_by = {userID}  " +
                                $"WHERE id = {task_id} ;";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            CMDefaultResponse response = new CMDefaultResponse();


            if (status == CMMS.CMMS_Status.PM_SCHEDULED)
            {
                response = new CMDefaultResponse(task_id, retCode, $"PM Task Assigned To user Id {assign_to}");
            }
            else
            {
                response = new CMDefaultResponse(task_id, retCode, $"PM Task Reassigned To user Id {assign_to}");
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, task_id, 0, 0, $"PM Task Assigned to user Id {assign_to}", CMMS.CMMS_Status.PM_ASSIGNED, userID);
            try
            {
                CMPMTaskView _PMTask = await GetPMTaskDetail(task_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_ASSIGNED, new[] { userID }, _PMTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }
            

            return response;
        }
        //private CMSetScheduleData CreateScheduleData(ScheduleIDData scheduleData)
        //{
        //    CMSetScheduleData schData = new CMSetScheduleData();
        //    schData.facility_id = scheduleData.facility_id;
        //    schData.asset_schedules = new List<CMScheduleData>();
        //    CMScheduleData asset_schedule = new CMScheduleData();
        //    asset_schedule.asset_id = scheduleData.asset_id;
        //    asset_schedule.asset_name = null;
        //    asset_schedule.category_id = 0;
        //    asset_schedule.category_name = null;
        //    asset_schedule.frequency_dates = new List<ScheduleFrequencyData>();
        //    ScheduleFrequencyData schedule = new ScheduleFrequencyData();
        //    schedule.frequency_name = null;
        //    schedule.frequency_id = scheduleData.frequency_id;
        //    schedule.schedule_id = 0;
        //    schedule.schedule_date = scheduleData.schedule_date;
        //    asset_schedule.frequency_dates.Add(schedule);
        //    schData.asset_schedules.Add(asset_schedule);
        //    return schData;
        //}
        internal async Task<List<CMDefaultResponse>> UpdatePMScheduleExecution(CMPMExecutionDetail request, int userID, string facilitytimeZone)
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
            if (status != CMMS.CMMS_Status.PM_START && status != CMMS.CMMS_Status.PM_REJECTED)
            {
                responseList.Add(new CMDefaultResponse(request.task_id, CMMS.RETRUNSTATUS.FAILURE,
                    "Task Execution must be rejected or in progress to modify execution details"));
                return responseList;
            }

            CMPMScheduleObservation schedule = new CMPMScheduleObservation();
            schedule = request.schedules[0];


            string executeQuery1 = $"SELECT id FROM pm_schedule WHERE id = {schedule.schedule_id} and task_id = {request.task_id};";

            DataTable dtSch = await Context.FetchData(executeQuery1).ConfigureAwait(false);

            if (dtSch.Rows.Count == 0)
                throw new MissingMemberException($"PM Schedule PMSCH{schedule.schedule_id} associated with PM Task PMTASK{request.task_id} not found");

            string executeQuery = $"SELECT id FROM pm_execution WHERE PM_Schedule_Id = {schedule.schedule_id} and task_id = {request.task_id};";

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
                    object valueFromDatabase = dtType.Rows[0][0];
                    string CPtypeValue = "";
                    /*    if (Convert.ToInt32(dtType.Rows[0][0]) == 0)
                     {
                         CPtypeValue = $" , `text` = '{schedule_detail.text}' ";
                     }
                     else if (Convert.ToInt32(dtType.Rows[0][0]) == 1)
                     {
                         CPtypeValue = $" , `text` = {schedule_detail.text} ";
                     }
                     else if (Convert.ToInt32(dtType.Rows[0][0]) == 2)
                     {
                         CPtypeValue = $" , `text` = {schedule_detail.text} ";
                     }*/
                    if (valueFromDatabase != DBNull.Value)
                    {
                        int typeValue = Convert.ToInt32(valueFromDatabase);

                        // Now use typeValue in your logic
                        if (typeValue == 0)
                        {
                            CPtypeValue = $" , `text` = '{schedule_detail.text}' ";
                        }
                        else if (typeValue == 1 || typeValue == 2)
                        {
                            CPtypeValue = $" , `text` = {schedule_detail.text} ";
                        }
                    }
                    CPtypeValue = CPtypeValue + $" , is_ok = {schedule_detail.cp_ok} ";
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
                        try
                        {
                            CMPMScheduleExecutionDetail PMTaskSchedule = await GetPMTaskScheduleDetail(request.task_id, schedule.schedule_id, facilitytimeZone);
                            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_UPDATED, new[] { userID }, PMTaskSchedule);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to send Schedule Notification: {ex.Message}");
                        }
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
                        CMDefaultResponse jobResp = await _jobRepo.CreateNewJob(newJob, userID);
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
                           /* CMPMTaskView _PMTask = await GetPMTaskDetail(request.task_id, facilitytimeZone);
                            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_UPDATED, new[] { userID }, _PMTask);
*/
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
                    else
                    {
                        string pmScheduleUpdate = $"UPDATE pm_schedule as pms " +
                                                  $"SET pms.PM_Schedule_Completed_by_id = {userID}, " +
                                                  $"pms.PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}' " +
                                                  $"WHERE pms.id = {schedule.schedule_id};";
                        await Context.ExecuteNonQry<int>(pmScheduleUpdate).ConfigureAwait(false);

                        CMDefaultResponse responseSchedule = new CMDefaultResponse(schedule.schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Updated Successfully.");

                        responseList.Add(responseSchedule);
                    }
                }
                else
                {
                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id} is not associated with PMSCH{schedule.schedule_id}");
                    responseList.Add(response);
                }
            }

            //string taskQry = $"update pm_task set updated_by = {userID},updated_at = '{UtilsRepository.GetUTCTime()}', update_remarks = '{request.comment}' WHERE id = {request.task_id};";
            //int val = await Context.ExecuteNonQry<int>(taskQry).ConfigureAwait(false);
            //CMDefaultResponse responseSchedule = new CMDefaultResponse(schedule.schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Updated Successfully.");

            //responseList.Add(responseSchedule);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.task_id, 0, 0, $"PM Task {request.task_id} updated.", CMMS.CMMS_Status.PM_UPDATED, userID);
            /*  CMPMTaskView _PMList = await GetPMTaskDetail(request.task_id, facilitytimeZone);
              CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_UPDATED, new[] { userID }, _PMList);*/
            try
            {
                CMPMScheduleExecutionDetail PMTaskSchedule = await GetPMTaskScheduleDetail(request.task_id, schedule.schedule_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_UPDATED, new[] { userID }, PMTaskSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Schedule Notification: {ex.Message}");
            }

            return responseList;
        }

        internal async Task<CMPMScheduleExecutionDetail> GetPMTaskScheduleDetail(int task_id, int schedule_id, string facilitytimeZone)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Other supporting tables - Facility, Asset, AssetCategory, Users
             * Read All properties mention in model and return list
             * Code goes here
            */
            if (task_id <= 0 || schedule_id <= 0)
                throw new ArgumentException("Invalid Task ID or Schedule ID");


            string eventQry = "CASE ";
            foreach (CMMS.CMMS_Events _event in Enum.GetValues(typeof(CMMS.CMMS_Events)))
            {
                eventQry += $"WHEN pm_schedule_files.PM_Event = {(int)_event} THEN '{_event}' ";
            }
            eventQry += "ELSE 'Unknown Event' END ";

            //string myQuery1 = $"SELECT id, PM_Maintenance_Order_Number as maintenance_order_number, PM_Schedule_date as schedule_date, PM_Schedule_Completed_date as completed_date, Asset_id as equipment_id, Asset_Name as equipment_name, Asset_Category_id as category_id, Asset_Category_name as category_name, PM_Frequecy_id as frequency_id, PM_Frequecy_Name as frequency_name, PM_Schedule_Emp_name as assigned_to_name, PTW_id as permit_id, status, {statusQry} as status_name, Facility_id as facility_id, Facility_Name as facility_name " +
            //                    $"FROM pm_schedule WHERE id = {schedule_id};";

            //string myQuery = $"select status from pm_task where pm_task.id = {task_id} ";

            //List<CMPMTaskView> taskViewDetail = await Context.GetData<CMPMTaskView>(myQuery).ConfigureAwait(false);

            //if (taskViewDetail.Count == 0)
            //    throw new MissingMemberException("PM Task not found");

            string myQuery2 = $"SELECT pm_schedule.id as schedule_id, assets.name as asset_name, checklist.checklist_number as checklist_name, " +
                $" CONCAT(startedBy.firstName, ' ' ,startedBy.lastName) as PM_Execution_Started_by_name, CONCAT(updatedBy.firstName, ' ' ,updatedBy.lastName) as updatedbyName, " +
                $" CONCAT(createdBy.firstName, ' ' ,createdBy.lastName) as createdbyName, pm_schedule.Status AS status, PM_Schedule_date, PM_Frequecy_Name, CONCAT(rejectedBy.firstName, ' ' ,rejectedBy.lastName) as rejectedbyName, " +
                $"CONCAT(approvedBy.firstName, ' ' , approvedBy.lastName) as approvedbyName, CONCAT(cancelledrejected.firstName, ' ' , cancelledrejected.lastName) as cancelledrejectedbyName, " +
                $"CONCAT(cancelledapproved.firstName, ' ' , cancelledapproved.lastName) as cancelledapprovedbyName, PM_Schedule_updated_by, " +
                $"CONCAT(submittedBy.firstName, ' ' , submittedBy.lastName) as submittedByName, " +
                $"CONCAT(completedbyName.firstName, ' ' , completedbyName.lastName) as completedBy_name" +
                $" from pm_schedule "  + 
                $"left join assets on pm_schedule.asset_id = assets.id " +
                $"left join checklist_number as checklist on pm_schedule.checklist_id = checklist.id " +
                $"left join users AS startedBy ON startedBy.id = pm_schedule.PM_Execution_Started_by_id "+
                $"left join users AS updatedBy ON updatedBy.id = pm_schedule.PM_Schedule_updated_by "+
                $"left join users AS createdBy ON createdBy.id = pm_schedule.createdById " +
                $"left join users AS rejectedBy ON rejectedBy.id = pm_schedule.PM_Schedule_Rejected_by_id " +
                $"left join users AS approvedBy ON approvedBy.id = pm_schedule.PM_Schedule_Approved_by_id " + 
                $"left join users AS cancelledrejected ON cancelledrejected.id = pm_schedule.PM_Schedule_cancel_by_id " +
                $"left join users AS cancelledapproved ON cancelledapproved.id = pm_schedule.PM_Schedule_Approved_by_id " +
                $"left join users AS submittedBy ON submittedBy.id = pm_schedule.submittedById " +
                $"left join users AS completedbyName ON completedbyName.id = pm_schedule.PM_Schedule_Completed_by_id " +
                $"where pm_schedule.id = {schedule_id} and task_id = {task_id};"; 

            List<CMPMScheduleExecutionDetail> scheduleDetails = await Context.GetData<CMPMScheduleExecutionDetail>(myQuery2).ConfigureAwait(false);


            if (scheduleDetails.Count == 0)
                throw new MissingMemberException($"PM Schedule PMSCH{schedule_id} associated with PM Task PMTASK{task_id} not found");



            //string myQuery2 = $"SELECT DISTINCT checklist.id, checklist.checklist_number AS name FROM pm_execution " + 
            //                    $"JOIN checkpoint on pm_execution.Check_Point_id = checkpoint.id " + 
            //                    $"JOIN checklist_number as checklist ON checklist.id = checkpoint.check_list_id " +
            //                    $"WHERE pm_execution.PM_Schedule_Id = {schedule_id};";

            //List<CMDefaultList> checklist_collection = await Context.GetData<CMDefaultList>(myQuery2).ConfigureAwait(false);

            string myQuery3 = "SELECT pm_execution.id as execution_id,checkpoint.type as check_point_type ,pm_execution.range as type_range,pm_execution.text as type_text,pm_execution.is_ok as type_bool, Check_Point_id as check_point_id, Check_Point_Name as check_point_name, Check_Point_Requirement as requirement, PM_Schedule_Observation as observation, job_created as is_job_created, linked_job_id, custom_checkpoint as is_custom_check_point, file_required as is_file_required " +
                                $"FROM pm_execution " +
                                $"left join checkpoint on checkpoint.id = pm_execution.Check_Point_id WHERE PM_Schedule_Id = {schedule_id}  ;";

            List<ScheduleCheckList> scheduleCheckList = await Context.GetData<ScheduleCheckList>(myQuery3).ConfigureAwait(false);

            if (scheduleCheckList.Count > 0)
            {
                foreach (ScheduleCheckList scheduleCheckPoint in scheduleCheckList)
                {
                    string fileQry = $"SELECT {eventQry} AS _event,File_id as file_id, File_Path as file_path, File_Discription as file_description " +
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
                                $"WHERE pm_execution.PM_Schedule_Id  = {schedule_id};";
            List<ScheduleLinkJob> linked_jobs = await Context.GetData<ScheduleLinkJob>(myQuery4).ConfigureAwait(false);
            List<CMLog> log = await _utilsRepo.GetHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule_id, "");

            //if (checklist_collection.Count > 0)
            //{
            //    taskViewDetail[0].checklist_id = checklist_collection[0].id;
            //    taskViewDetail[0].checklist_name = checklist_collection[0].name;
            //}
            //taskViewDetail[0].schedule_check_points = scheduleCheckList;
            //taskViewDetail[0].history_log = log; 

            scheduleDetails[0].schedule_link_job = linked_jobs;
            scheduleDetails[0].checklist_observation = scheduleCheckList;


            //CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(taskViewDetail[0].status);
            //string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_SCHEDULE, _Status);
            //taskViewDetail[0].status_short = _shortStatus;

            //string _longStatus = getLongStatus(CMMS.CMMS_Modules.PM_SCHEDULE, _Status, taskViewDetail[0]);
            //taskViewDetail[0].status_long = _longStatus;
            foreach (var detail in linked_jobs)
            {
                if (detail != null && detail.job_date != null)
                    detail.job_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.job_date);
            }

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(scheduleDetails[0].status);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_SCHEDULE, _Status);
            scheduleDetails[0].status_short = _shortStatus;

            
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.PM_SCHEDULE, _Status, scheduleDetails[0]);
            scheduleDetails[0].status_long = _longStatus;

            return scheduleDetails[0];
        }
        internal async Task<List<CMDefaultResponse>> cloneSchedule(int facility_id, int task_id, int from_schedule_id, int to_schedule_id, int cloneJobs, int userID)
        {
            try
            {
                string statusQry = $"SELECT checklist_id FROM pm_schedule WHERE task_id = {task_id} and id IN ({from_schedule_id},{to_schedule_id});";
                DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);

                if (dt1.Rows.Count == 0)
                    throw new MissingMemberException("Schedule Id(s) not Found");

                if (Convert.ToInt32(dt1.Rows[0][0]) != Convert.ToInt32(dt1.Rows[1][0]))
                    throw new MissingMemberException("Checklists Should be same.");

                CMPMScheduleExecutionDetail from_schedule = await GetPMTaskScheduleDetail(task_id, from_schedule_id, "");

                string executeQuery = $"SELECT Check_Point_id FROM pm_execution WHERE PM_Schedule_Id = {from_schedule_id} and task_id = {task_id} ;";
                DataTable dt = await Context.FetchData(executeQuery).ConfigureAwait(false);
                List<int> checkpointIds = dt.GetColumn<int>("Check_Point_id");

                List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();

                foreach (ScheduleCheckList schedule_detail in from_schedule.checklist_observation)
                {
                    CMDefaultResponse response;
                    int executionId = 0;
                    if (checkpointIds.Contains(schedule_detail.check_point_id))
                    {
                        int changeFlag = 0;
                        string myQuery1 = "SELECT id as execution_id, PM_Schedule_Observation as observation, job_created as job_create " +
                                            $"FROM pm_execution WHERE  PM_Schedule_Id = {to_schedule_id} and Check_Point_id ={schedule_detail.check_point_id} and task_id = {task_id};";

                        List<AddObservation> execution_details = await Context.GetData<AddObservation>(myQuery1).ConfigureAwait(false);
                        executionId = execution_details[0].execution_id;
                        string myQuery2 = $"SELECT checkpoint.type from pm_execution left join checkpoint on pm_execution.check_point_id = checkpoint.id WHERE pm_execution.id = {executionId} ;";

                        DataTable dtType = await Context.FetchData(myQuery2).ConfigureAwait(false);
                        string CPtypeValue = "";
                        if (Convert.ToInt32(dtType.Rows[0][0]) == 0)
                        {
                            CPtypeValue = $" , `text` = '{schedule_detail.type_text}' ";

                        }
                        else if (Convert.ToInt32(dtType.Rows[0][0]) == 1)
                        {
                            CPtypeValue = $" , is_ok = {schedule_detail.type_bool} ";

                        }
                        else if (Convert.ToInt32(dtType.Rows[0][0]) == 2)
                        {
                            CPtypeValue = $" , `range` = {schedule_detail.type_range} ";

                        }
                        if (schedule_detail != null && schedule_detail.observation != null)
                        {
                            if (!schedule_detail.observation.Equals(execution_details[0].observation))
                            {
                                string updateQry = "UPDATE pm_execution SET ";
                                updateQry += $"PM_Schedule_Observation = '{schedule_detail.observation}', " +
                                            $"is_ok = '{schedule_detail.type_bool}', ";
                                /*+
                                            $"text = '{schedule_detail.type_text}', " +
                                            $"boolean = '{schedule_detail.type_bool}', " +
                                            $"range = '{schedule_detail.type_range}'";*/
                                string message;
                                if (execution_details[0].observation == null || execution_details[0].observation == "")
                                {
                                    updateQry += $"PM_Schedule_Observation_added_by = {userID}, " +
                                                $"PM_Schedule_Observation_add_date = '{UtilsRepository.GetUTCTime()}' {CPtypeValue} ";
                                    message = "Observation Added";
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Added Successfully");
                                }
                                else
                                {
                                    updateQry += $"PM_Schedule_Observation_update_by = {userID}, " +
                                                $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' {CPtypeValue} ";
                                    message = "Observation Updated";
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Updated Successfully");
                                }
                                updateQry += $"WHERE id = {executionId};";

                                /*
                                   if (!schedule_detail.observation.Equals(execution_details[0].observation))
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
                                   updateQry += $"WHERE id = {executionId};";*/
                                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, to_schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, message, CMMS.CMMS_Status.PM_UPDATED, userID);
                                responseList.Add(response);
                                changeFlag++;
                            }
                            if (cloneJobs == 1)
                            {
                                if (schedule_detail.linked_job_id > 0 && execution_details[0].job_create == 0)
                                {

                                    string facilityQry = $"SELECT pm_task.facility_id as block, CASE WHEN facilities.parentId=0 THEN facilities.id ELSE facilities.parentId END AS parent " +
                                                        $"FROM pm_schedule Left join pm_task on pm_task.id = pm_schedule.task_id LEFT JOIN facilities ON pm_task.facility_id=facilities.id " +
                                                        $"WHERE pm_schedule.id = {to_schedule_id}  group by pm_task.id ;";
                                    DataTable dtFacility = await Context.FetchData(facilityQry).ConfigureAwait(false);
                                    string titleDescQry = $"SELECT CONCAT('PMSCH{to_schedule_id}', Check_Point_Name, ': ', Check_Point_Requirement) as title, " +
                                                            $"PM_Schedule_Observation as description FROM pm_execution WHERE id = {executionId};";
                                    DataTable dtTitleDesc = await Context.FetchData(titleDescQry).ConfigureAwait(false);
                                    string assetsPMQry = $"SELECT Asset_id FROM pm_schedule WHERE id = {to_schedule_id};";
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
                                    CMDefaultResponse jobResp = await _jobRepo.CreateNewJob(newJob, userID);
                                    string updateQry = $"UPDATE pm_execution SET job_created = 1, linked_job_id = {jobResp.id[0]} WHERE id = {executionId};";
                                    await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                                    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, to_schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"Job {jobResp.id[0]} Created for PM", CMMS.CMMS_Status.PM_UPDATED, userID);
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {jobResp.id[0]} Created for PM Successfully");
                                    responseList.Add(response);
                                    changeFlag++;
                                }
                            }
                            else
                            {
                                //job creatiojn skipped
                            }

                            if (schedule_detail.files != null)
                            {
                                if (schedule_detail.files.Count != 0)
                                {
                                    foreach (var file in schedule_detail.files)
                                    {
                                        string _eventStr = "";
                                        _eventStr += (file._event == "BEFORE" ? "1" : string.Empty);
                                        _eventStr += (file._event == "DURING" ? "2" : string.Empty);
                                        _eventStr += (file._event == "AFTER" ? "3" : string.Empty);
                                        DataTable dt2 = new DataTable();
                                        if (_eventStr != "")
                                        {
                                            string checkEventFiles = $"SELECT * FROM pm_schedule_files " +
                                                                        $"WHERE PM_Schedule_id = {to_schedule_id} " +
                                                                        $"AND PM_Execution_id = {execution_details[0].execution_id} " +
                                                                        $"AND PM_Event = {_eventStr}";
                                            dt2 = await Context.FetchData(checkEventFiles).ConfigureAwait(false);
                                        }
                                        if (dt2.Rows.Count > 0)
                                        {
                                            string deleteQry = "DELETE FROM pm_schedule_files " +
                                                                    $"WHERE PM_Schedule_id = {to_schedule_id} " +
                                                                    $"AND PM_Execution_id = {execution_details[0].execution_id} " +
                                                                    $"AND PM_Event = {_eventStr}";
                                            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                                        }
                                        int PM_Event = _eventStr == "" ? 0 : Convert.ToInt32(_eventStr);
                                        string insertFile = "INSERT INTO pm_schedule_files(PM_Schedule_id, PM_Schedule_Code, PM_Execution_id,Check_Point_id, File_id, PM_Event, " +
                                                            "File_Discription, File_added_by, File_added_date, File_Server_id, File_Server_Path) VALUES " +
                                                            $"({to_schedule_id}, 'PMSCH{to_schedule_id}', {execution_details[0].execution_id},{schedule_detail.check_point_id}, {file.file_id}, " +
                                                            $"{PM_Event}, '{file.file_description}', {userID}, '{UtilsRepository.GetUTCTime()}', 1, " +
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
                                    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, to_schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, execution_details[0].execution_id, $"{schedule_detail.files.Count} file(s) attached to PMSCH{to_schedule_id}", CMMS.CMMS_Status.PM_UPDATED, userID);
                                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"{schedule_detail.files.Count} file(s) attached to PM Successfully");
                                    responseList.Add(response);
                                    changeFlag++;
                                }
                            }
                            if (changeFlag == 0)
                            {
                                response = new CMDefaultResponse(execution_details[0].execution_id, CMMS.RETRUNSTATUS.SUCCESS, "No changes");
                                responseList.Add(response);
                            }
                            else
                            {
                                string pmScheduleUpdate = $"UPDATE pm_schedule as pms " +
                                                      $"SET pms.PM_Schedule_Completed_by_id = {userID}, " +
                                                      $"pms.PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}' " +
                                                      $"WHERE pms.id = {to_schedule_id};";
                                await Context.ExecuteNonQry<int>(pmScheduleUpdate).ConfigureAwait(false);

                                CMDefaultResponse responseSchedule = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Updated Successfully.");

                                responseList.Add(responseSchedule);
                            }
                        }

                    }
                    else
                    {
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Checkpoint ID {executionId} not associated with PMSCH{to_schedule_id}");
                        responseList.Add(response);
                    }
                }

                //CMDefaultResponse response = new CMDefaultResponse();
                CMPMScheduleExecutionDetail to_schedule = await GetPMTaskScheduleDetail(task_id, from_schedule_id, "");


                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, to_schedule_id, 0, 0, $"PM Task {task_id} schedule {to_schedule_id} is cloned from {from_schedule_id}", CMMS.CMMS_Status.PM_ASSIGNED, userID);
                return responseList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal async Task<List<AssetList>> getAssetListForClone(int task_id, int schedule_id)
        {
            string qry = $"SELECT pm_schedule.id as schedule_id, assets.name as asset_name,checklist.checklist_number as checklist_name  FROM pm_schedule " +
                $"left join assets on pm_schedule.asset_id = assets.id " +
                $"left join checklist_number as checklist on pm_schedule.checklist_id = checklist.id " +
                $" WHERE task_id = {task_id} and checklist_id = (SELECT checklist_id FROM pm_schedule WHERE pm_schedule.id = {schedule_id}) ;";

            List<AssetList> list = await Context.GetData<AssetList>(qry).ConfigureAwait(false);

            if (list.Count == 0)
                throw new MissingMemberException($"Schedule Id {schedule_id} not Found");

            return list;
        }

        internal async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id, string facilitytimeZone)
        {
            /*
             * Primary Table - PMSchedule
             * Read All properties mention in model and return 
             * Code goes here
            */

            string myQuery = "SELECT assets.id as asset_id, assets.name as asset_name, category.id as category_id, category.name as category_name, block.id as block_id, block.name as block_name " +
                                "FROM assets " +
                                "LEFT JOIN assetcategories as category ON assets.categoryId = category.id " +
                                "LEFT JOIN facilities as block ON assets.blockId = block.id ";
            if (facility_id > 0)
            {
                myQuery += " WHERE assets.facilityId = " + facility_id;
                if (category_id > 0)
                {
                    myQuery += " AND category.id = " + category_id;
                }
                else
                {
                    throw new ArgumentException("Invalid Category ID");
                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            myQuery += " ORDER BY assets.id ASC;";
            List<CMScheduleData> _scheduleList = await Context.GetData<CMScheduleData>(myQuery).ConfigureAwait(false);
            foreach (CMScheduleData schedule in _scheduleList)
            {
                string query2 = "SELECT a.id as schedule_id, frequency.id as frequency_id, frequency.name as frequency_name, " +
                                    "a.PM_Schedule_date as schedule_date FROM pm_schedule as a " +
                                    "RIGHT JOIN frequency ON a.PM_Frequecy_id = frequency.id " +
                                    "WHERE a.PM_Schedule_date = (SELECT CASE WHEN MAX(b.PM_Schedule_date) IS NOT NULL THEN MAX(b.PM_Schedule_date) ELSE NULL END AS schdate " +
                                    "FROM pm_schedule as b WHERE a.Asset_id = b.Asset_id AND a.PM_Frequecy_id = b.PM_Frequecy_id AND b.status NOT IN " +
                                    $"({(int)CMMS.CMMS_Status.PM_CANCELLED}, {(int)CMMS.CMMS_Status.PM_APPROVED}) AND " +
                                    $"PM_Rescheduled = 0) AND a.Asset_id = {schedule.asset_id} GROUP BY frequency.id ORDER BY frequency.id;";
                List<ScheduleFrequencyData> _freqData = await Context.GetData<ScheduleFrequencyData>(query2).ConfigureAwait(false);
                schedule.frequency_dates = _freqData;
                foreach (var detail in _freqData)
                {
                    detail.schedule_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.schedule_date);

                }


            }


            return _scheduleList;
        }


        internal async Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID, int task_id, int schedule_id, string facilitytimeZone)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string myQuery1 = $"SELECT id, CONCAT(firstName,' ',lastName) as full_name, loginId as user_name, mobileNumber as contact_no " +
                                        $"FROM users WHERE id = {userID};";
            List<CMUser> user = await Context.GetData<CMUser>(myQuery1).ConfigureAwait(false);
            string myQuery2 = $"SELECT id, name, address, city, state, country, zipcode as pin FROM facilities WHERE id = {request.facility_id};";
            List<CMFacility> facility = await Context.GetData<CMFacility>(myQuery2).ConfigureAwait(false);
            foreach (var asset_schedule in request.asset_schedules)
            {
                CMDefaultResponse response = null;
                string myQuery3 = $"SELECT id, facilityId, categoryId, name FROM assets WHERE id = {asset_schedule.asset_id};";
                List<CMAsset> asset = await Context.GetData<CMAsset>(myQuery3).ConfigureAwait(false);
                if (asset[0].facilityId != request.facility_id)
                {
                    response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG, $"Facility ID {request.facility_id} does not include Asset ID {asset_schedule.asset_id}");
                    responseList.Add(response);
                    continue;
                }
                string myQuery4 = $"SELECT serialNumber, blockId FROM assets WHERE id = {asset_schedule.asset_id};";
                DataTable dt = await Context.FetchData(myQuery4).ConfigureAwait(false);
                string serialNumber = Convert.ToString(dt.Rows[0]["serialNumber"]);
                string blockId = Convert.ToString(dt.Rows[0]["blockId"]);
                string myQuery5 = $"SELECT id, name, description FROM assetcategories WHERE id = {asset[0].categoryId};";
                List<CMAssetCategory> category = await Context.GetData<CMAssetCategory>(myQuery5).ConfigureAwait(false);
                if (category.Count == 0)
                {
                    CMAssetCategory cat = new CMAssetCategory();
                    cat.id = 0;
                    cat.name = "Others";
                    cat.description = "Others";
                    category.Add(cat);
                }
                foreach (var frequency_schedule in asset_schedule.frequency_dates)
                {
                    string myQuery6 = $"SELECT * FROM frequency WHERE id = {frequency_schedule.frequency_id};";
                    List<CMFrequency> frequency = await Context.GetData<CMFrequency>(myQuery6).ConfigureAwait(false);
                    string myQuery7 = "SELECT id as schedule_id, PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
                                        $"FROM pm_schedule WHERE Asset_id = {asset_schedule.asset_id} AND PM_Frequecy_id = {frequency_schedule.frequency_id} " +
                                        $"AND status NOT IN ({(int)CMMS.CMMS_Status.PM_CANCELLED}) AND PM_Rescheduled = 0;";
                    List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(myQuery7).ConfigureAwait(false);
                    if (scheduleData.Count > 0)
                    {
                        if (frequency_schedule.schedule_date != null)
                        {
                            string updateQry = $"UPDATE pm_schedule SET PM_Schedule_date = '{((DateTime)frequency_schedule.schedule_date).ToString("yyyy'-'MM'-'dd")}', " +
                                                $"PM_Schedule_updated_by = {userID}, PM_Schedule_updated_date = '{UtilsRepository.GetUTCTime()}' " +
                                                $"WHERE id = {scheduleData[0].schedule_id};";
                            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, scheduleData[0].schedule_id, 0, 0, "PM Schedule Details Updated", CMMS.CMMS_Status.PM_UPDATED, userID);
                            response = new CMDefaultResponse(scheduleData[0].schedule_id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule date updated successfully");
                            responseList.Add(response);
                        }
                        else
                        {
                            string deleteQry = $"DELETE FROM pm_schedule WHERE id = {scheduleData[0].schedule_id};";
                            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, scheduleData[0].schedule_id, 0, 0, "PM Schedule Deleted", CMMS.CMMS_Status.PM_DELETED, userID);
                            response = new CMDefaultResponse(scheduleData[0].schedule_id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule data deleted due to empty date");
                            responseList.Add(response);
                        }
                    }
                    else
                    {
                        if (frequency_schedule.schedule_date != null)
                        {
                            string mainQuery = $"INSERT INTO pm_schedule(PM_Schedule_Date, PM_Frequecy_Name, PM_Frequecy_id, PM_Frequecy_Code, " +
                               $"Facility_id, Facility_Name, Facility_Code, Block_Id, Block_Code, Asset_Category_id, Asset_Category_Code, Asset_Category_name, " +
                               $"Asset_id, Asset_Code, Asset_Name, PM_Schedule_User_id, PM_Schedule_User_Name, PM_Schedule_Emp_id, PM_Schedule_Emp_name, createdById, " +
                               $"PM_Schedule_created_date, Asset_Sno, status, status_updated_at, submittedById) VALUES " +
                               $"('{((DateTime)frequency_schedule.schedule_date).ToString("yyyy'-'MM'-'dd")}', '{frequency[0].name}', {frequency[0].id}, 'FRC{frequency[0].id}', " +
                               $"{facility[0].id}, '{facility[0].name}', 'FAC{facility[0].id + 1000}', {blockId}, 'BLOCK{blockId}', {category[0].id}, 'AC{category[0].id + 1000}', '{category[0].name}', " +
                               $"{asset[0].id}, 'INV{asset[0].id}', '{asset[0].name}', {user[0].id}, '{user[0].full_name}', {user[0].id}, '{user[0].full_name}', {userID}, " +
                               $"'{UtilsRepository.GetUTCTime()}', '{serialNumber}', {(int)CMMS.CMMS_Status.PM_SUBMIT}, '{UtilsRepository.GetUTCTime()}', {userID}); SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt2.Rows[0][0]);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.PM_SUBMIT, userID);
                            response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "PM Schedule data inserted successfully");
                            responseList.Add(response);
                        }
                        else
                        {
                            response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG, "Date cannot be null");
                            responseList.Add(response);
                        }
                    }

                  

                }
            }

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);

            try
            {
                CMPMScheduleExecutionDetail PMTaskSchedule = await GetPMTaskScheduleDetail(task_id, schedule_id, facilitytimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_SCHEDULE, CMMS.CMMS_Status.PM_SUBMIT, new[] { userID }, PMTaskSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Schedule Notification: {ex.Message}");
            }
            return responseList;
        }

        internal async Task<CMDefaultResponse> DeletePMTask(CMApproval request, int userID, string facilityTimeZone)
        {
            string deleteQuery = $"UPDATE pm_task SET status = {(int)CMMS.CMMS_Status.PM_TASK_DELETED}, deletedById = {userID} WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(deleteQuery).ConfigureAwait(false);
            try
            {
                CMPMTaskView _PMTaskList = await GetPMTaskDetail(request.id, facilityTimeZone);
                CMMSNotification.sendNotification(CMMS.CMMS_Modules.PM_TASK, CMMS.CMMS_Status.PM_TASK_DELETED, new[] { userID }, _PMTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Task Notification: {ex.Message}");
            }

            CMDefaultResponse response = null;
            string mrsQ = "select status,id from smmrs where whereUsedRefID = " + request.id + ";";
            DataTable dt_insert = await Context.FetchData(mrsQ).ConfigureAwait(false);

            string taskQ = "select ptw_id from pm_task where id = " + request.id + ";";
            DataTable dt_task = await Context.FetchData(taskQ).ConfigureAwait(false);

            int mrs_status = 0;
            int mrs_id = 0;
            int permit_id = 0;
            if (dt_insert.Rows.Count > 0)
            {
                mrs_status = Convert.ToInt32(dt_insert.Rows[0][0]);
                mrs_id = Convert.ToInt32(dt_insert.Rows[0][1]);
            }

            if (dt_task.Rows.Count > 0)
            {
                if (Convert.ToString(dt_task.Rows[0][0]) != "")
                {
                    permit_id = Convert.ToInt32(dt_insert.Rows[0][0]);
                }

            }

            if ((CMMS.CMMS_Status)mrs_status == CMMS.CMMS_Status.MRS_REQUEST_ISSUED)
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, $" PM Plan With MRS " + mrs_id + " Is Issued. Please Return MRS To Proceed Further.");
                return response;
            }

            string approveQuery = $"update pm_task set status = {(int)CMMS.CMMS_Status.PM_TASK_DELETED}, deletedById = {userID},  update_remarks = '{request.comment}' where id = {request.id}; ";
            approveQuery = approveQuery + $" update smmrs set status = {(int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED}, issue_rejected_comment = '{request.comment}', issue_rejected_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' where id = {mrs_id}; ";
            approveQuery = approveQuery + $" update permits set rejectReason = '{request.comment}', rejectStatus = 1, status = {(int)CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER}, status_updated_at = '{UtilsRepository.GetUTCTime()}', rejectedDate ='{UtilsRepository.GetUTCTime()}', rejectedById = {userID}  where id = {permit_id};";
            //string approveQuery = $"update pm_task set status = {(int)CMMS.CMMS_Status.PM_TASK_DELETED} where id = {request.id}; ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PM_TASK_DELETED);
          
            response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" PM Task Deleted With MRS : " + mrs_id + "");
            return response;
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
    }

}
