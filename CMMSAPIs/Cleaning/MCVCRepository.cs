using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.MCVCRepository
{

    public class MCVCRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public cleaningType moduleType = 0;
        public static string measure = "";
        public MCVCRepository(MYSQLDBHelper sqlDBHelper, cleaningType type) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);

            // Set moduleType based on the passed cleaningType
            moduleType = type;

            // Adjust the measure based on cleaningType
            if (type == cleaningType.ModuleCleaning)
            {
                //moduleType = (int)cleaningType.ModuleCleaning;
                measure = "moduleQuantity";
            }
            else if (type == cleaningType.Vegetation)
            {
                //moduleType = (int)cleaningType.Vegetation;
                measure = "area";
            }

        }




        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>()
        {
            { (int)CMMS.CMMS_Status.MC_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.MC_PLAN_SUBMITTED, "Waiting for Approval" },
            { (int)CMMS.CMMS_Status.MC_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.MC_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.MC_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.MC_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.MC_TASK_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW,"PTW_Linked" },
            { (int)CMMS.CMMS_Status.MC_TASK_END_APPROVED,"Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_END_REJECTED,"Reject" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED,"Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT,"Reject" },
            { (int)CMMS.CMMS_Status.MC_TASK_RESCHEDULED,"Reschedule" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED,"Abandoned Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED,"Abandoned Rejected" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED, "Waiting for Approval" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.VEG_TASK_STARTED, "Started" },
            { (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED, "Close - Waiting for Approval" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED, "Abandon - Waiting for Approval" },
            { (int)CMMS.CMMS_Status.VEG_TASK_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_TASK_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW, "PTW Linked" },
            { (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED, "Schedule - Approved" },
            { (int)CMMS.CMMS_Status.VEG_TASK_END_REJECTED, "Schedule - Reject" },
            { (int)CMMS.CMMS_Status.VEG_TASK_UPDATED, "Updated" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ASSIGNED , "Reassigned" },
            { (int)CMMS.CMMS_Status.EQUIP_CLEANED, "Cleaned" },
            { (int)CMMS.CMMS_Status.EQUIP_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.EQUIP_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED, "Abandoned Rejected" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED, "Abandoned Approved" }
        };
        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan planObj)
        {
            string retValue = "";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue = String.Format("MC{0} Created by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue = String.Format("MC{0} Rejected by {1} ", planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue = String.Format("MC{0} Approved by {1} ", planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue = String.Format("MC{0} Deleted by {1} ", planObj.planId, planObj.deletedbyName);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue = String.Format("MC{0} Updated by {1} ", planObj.planId, planObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                    retValue = String.Format("VC{0} Created by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue = String.Format("VC{0} Rejected by {1} ", planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue = String.Format("VC{0} Approved by {1} ", planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue = String.Format("VC{0} Deleted by {1} ", planObj.planId, planObj.deletedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                    retValue = String.Format("VC{0} Updated by {1} ", planObj.planId, planObj.updatedbyName);
                    break;
                default:
                    retValue = String.Format(" No status for VC{0} ", planObj.planId);
                    break;
            }
            return retValue;

        }
        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecution executionObj)
        {
            string retValue = "";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                    retValue = String.Format("MC{0} Scheduled by {1}", executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue = String.Format("MC{0} Started by {1}", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue = String.Format("MC{0} Closed by {1}", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue = String.Format("MC{0} Abandoned by {1}", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue = String.Format("MC{0} Abandoned Rejected by {1}", executionObj.executionId, executionObj.abandonRejectedByName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue = String.Format("MC{0} Abandoned Approved by {1}", executionObj.executionId, executionObj.abandonApprovedByName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue = String.Format("MC{0} Closed by {1}", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue = String.Format("MC{0} Task Rejected by {1}", executionObj.executionId, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue = String.Format("MC{0} Task Approved by {1}", executionObj.executionId, executionObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue = String.Format("MC{0} End Approved by {1}", executionObj.executionId, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue = String.Format("MC{0} End Rejected by {1}", executionObj.executionId, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ASSIGNED:
                    retValue = String.Format("MC{0} Assigned to {1}", executionObj.executionId, executionObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                    retValue = String.Format("VE{0} Rescheduled to {1}", executionObj.executionId, executionObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("VE{0} Scheduled by {1}", executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue = String.Format("VE{0} Started by {1}", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("VE{0} Closed by {1}", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("VE{0} Abandoned by {1}", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue = String.Format("VE{0} Abandoned Rejected by {1}", executionObj.executionId, executionObj.abandonRejectedByName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue = String.Format("VE{0} Abandoned Approved by {1}", executionObj.executionId, executionObj.abandonApprovedByName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue = String.Format("VE{0} Task Closed by {1}", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue = String.Format("VE{0} Task Rejected by {1}", executionObj.executionId, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue = String.Format("VE{0} Task Approved by {1}", executionObj.executionId, executionObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue = String.Format("VE{0} End Approved by {1}", executionObj.executionId, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue = String.Format("VE{0} End Rejected by {1}", executionObj.executionId, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue = String.Format("VE{0} Assigned to {1}", executionObj.executionId, executionObj.assignedTo);
                    break;
                default:
                    retValue = String.Format("Unsupported status {0} for VEG {1}", notificationID, executionObj.executionId);
                    break;
            }
            return retValue;

        }



        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecutionSchedule scheduleObj)
        {
            string retValue = "";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue = String.Format("SCH{0} Started by {1}", scheduleObj.scheduleId, scheduleObj.startedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue = String.Format("SCH{0} Closed by {1}", scheduleObj.scheduleId, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue = String.Format("SCH{0} Abandoned by {1}", scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue = String.Format("SCH{0} Close Approved by {1}", scheduleObj.scheduleId, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue = String.Format("SCH{0} Close Rejected by {1}", scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue = String.Format("SCH{0} Close Rejected by {1}", scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;


                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue = String.Format("PTW{0} Linked with SCH{1} of MCT{2}", scheduleObj.permit_id, scheduleObj.scheduleId, scheduleObj.executionId);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue = String.Format("SCH{0} Started by {1}", scheduleObj.scheduleId, scheduleObj.startedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("SCH{0} Closed by {1}", scheduleObj.scheduleId, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("SCH{0} Abandoned by {1}", scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue = String.Format("SCH{0} Rejected by {1}", scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                /*case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue = String.Format("PTW{0} Linked with SCH{1} of MCT{1}", scheduleObj.permit_id, scheduleObj.scheduleId, scheduleObj.executionId);
                    break;*/
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue = String.Format("SCH{0} Close Approved by {1}", scheduleObj.scheduleId, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue = String.Format("SCH{0} Close Rejected by {1}", scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("SCH{0} Scheduled by {1}", scheduleObj.scheduleId, scheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue = String.Format("SCH{0} Updated by {1}", scheduleObj.scheduleId, scheduleObj.updatedbyName);
                    break;

                default:
                    retValue = String.Format("No status for SCH{0} at {1)", scheduleObj.scheduleId, scheduleObj.facilityidName);
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


        internal async Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone, string startDate, string endDate)        //Pending : add date range and status filter
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery1 = $"SELECT mc.planId, mc.status, mc.frequencyId, mc.assignedTo as assignedToId, mc.startDate, " +
            $"mc.durationDays as noOfCleaningDays, mc.facilityId, mc.title, " +
            $"CONCAT(createdBy.firstName, ' ', createdBy.lastName) as createdBy, mc.createdAt, " +
            $"CONCAT(approvedbyName.firstName, ' ', approvedbyName.lastName) as approvedByName, mc.approvedAt, " +
            $"freq.name as frequency, CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assignedTo, " +
            $"mc.durationDays, {statusOut} as status_short " +
            $"FROM cleaning_plan as mc " +
            $"LEFT JOIN Frequency as freq ON freq.id = mc.frequencyId " +
            $"LEFT JOIN users as assignedTo ON assignedTo.id = mc.assignedTo " +
            $"LEFT JOIN users as createdBy ON createdBy.id = mc.createdById " +
            $"LEFT JOIN users as approvedbyName ON approvedbyName.id = mc.approvedById ";

            if (facilityId > 0)
            {
                myQuery1 += $" WHERE mc.facilityId = {facilityId} ";

                if ((int)moduleType > 0)
                {
                    myQuery1 += " AND mc.moduleType = " + (int)moduleType;
                }

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    DateTime start = DateTime.Parse(startDate);
                    DateTime end = DateTime.Parse(endDate);

                    // Check if the start date is earlier than the end date
                    if (DateTime.Compare(start, end) < 0)
                    {
                        string formattedStart = start.ToString("yyyy-MM-dd");
                        string formattedEnd = end.ToString("yyyy-MM-dd");

                        myQuery1 += $" AND DATE_FORMAT(mc.createdAt,'%Y-%m-%d') BETWEEN '{formattedStart}' AND '{formattedEnd}'";
                    }
                    else
                    {
                        throw new ArgumentException("Start date must be earlier than end date");
                    }
                }

                /*if (!string.IsNullOrEmpty(statusOut))
                {
                    // Assuming `statusOut` is a comma-separated list like '350, 351, 352'
                    myQuery1 += $" AND mc.status IN ({statusOut})";
                }*/
            }
            else
            {
                throw new ArgumentException($"Facility ID <{facilityId}> cannot be empty or 0");
            }





            List<CMMCPlan> _ViewMCPlanList = await Context.GetData<CMMCPlan>(myQuery1).ConfigureAwait(false);
            foreach (var mclist in _ViewMCPlanList)
            {
                if (mclist != null && mclist.approvedAt != null)
                    mclist.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, mclist.approvedAt);
                if (mclist != null && mclist.createdAt != null)
                    mclist.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, mclist.createdAt);
                if (mclist != null && mclist.startDate != null)
                    mclist.startDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, mclist.startDate);
            }
            return _ViewMCPlanList;

        }

        internal async Task<List<CMMCTaskList>> GetTaskList(string facilityId, string facilitytimeZone, string startDate, string endDate, bool selfView, int userId)         //Pending : add date range and status filter
        {
            string statusOut = "CASE ";
            //Scheduled Qnty	Actual Qnty	Deviation	Machine type	Time taken	Remarks
            //SUM(css.moduleQuantity) as  Scheduled_Qnty,sub2.no_of_cleaned as Actual_Qnty,( SUM(css.moduleQuantity) - sub2.no_of_cleaned)  as Deviation
            //CASE  WHEN mc.abandonedAt IS NOT NULL THEN TIMESTAMPDIFF(MINUTE, mc.startDate, mc.abandonedAt)  ELSE TIMESTAMPDIFF(MINUTE, mc.startDate, mc.endedAt)
            //END as Time_taken , mc.reasonForAbandon as Remark
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery1 = $"select mc.id as executionId ,mp.title,mc.planId,mc.status,mc.abandonedAt as Abondond_done_date," +
                              $" CONCAT(assignedToUser.firstName, assignedToUser.lastName) as responsibility , mc.startDate as scheduledDate, mc.endedAt as doneDate, " +
                              $" SUM(css.area) as  Scheduled_Qnty ,sub2.no_of_cleaned as Actual_Qnty, ( SUM(css.area) - sub2.no_of_cleaned)  as Deviation , Case When mc.abandonedById >0 THEN 'yes' ELSE 'no' END as Abondend , " +
                              $" CASE  WHEN mc.abandonedAt IS NOT NULL THEN TIMESTAMPDIFF(MINUTE, mc.abandonedAt, mc.startDate)  ELSE TIMESTAMPDIFF(MINUTE,mc.endedAt, mc.startDate) END as Time_taken ,  mc.reasonForAbandon as Remark,  " +
                              $" mc.prevTaskDoneDate as lastDoneDate,freq.name as frequency,mc.noOfDays, {statusOut} as status_short" +
                              $" from cleaning_execution as mc left join cleaning_plan as mp on mp.planId = mc.planId " +
                              $" LEFT JOIN (SELECT executionId, SUM(area) AS no_of_cleaned FROM cleaning_execution_items where cleanedById>0 GROUP BY executionId) sub2 ON mc.id = sub2.executionId " +
                              $"LEFT join cleaning_execution_items as css on css.executionId = mc.id " +
                              $"LEFT JOIN Frequency as freq on freq.id = mp.frequencyId " +
                              $"LEFT JOIN users as assignedToUser ON assignedToUser.id = mc.assignedTo " +
                              $"LEFT JOIN users as approvedBy ON approvedBy.id = mc.approvedByID" +
                              $" where mc.moduleType = {(int)moduleType} ";


            if (!string.IsNullOrEmpty(facilityId))
            {
                // Assume facilityId is a comma-delimited list like "1,2,3"
                myQuery1 += $" and mc.facilityId IN ({facilityId}) group by mc.id";
            }

            if (selfView)
            {
                // Add assignedToId condition along with user.id and created_user.id
                myQuery1 += $" AND mc.assignedTo = {userId}";
            }


            List<CMMCTaskList> _ViewMCTaskList = await Context.GetData<CMMCTaskList>(myQuery1).ConfigureAwait(false);
            foreach (var ViewMCTaskList in _ViewMCTaskList)
            {
                if (ViewMCTaskList != null && ViewMCTaskList.doneDate != null)
                    ViewMCTaskList.doneDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.doneDate);
                if (ViewMCTaskList != null && ViewMCTaskList.scheduledDate != null)
                    ViewMCTaskList.scheduledDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.scheduledDate);
                if (ViewMCTaskList != null && ViewMCTaskList.lastDoneDate != null)
                    ViewMCTaskList.lastDoneDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.lastDoneDate);
                if (ViewMCTaskList != null && ViewMCTaskList.Abondond_done_date != null)
                    ViewMCTaskList.Abondond_done_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.Abondond_done_date);
            }
            return _ViewMCTaskList;
        }

        internal async Task<CMMCPlan> GetPlanDetails(int planId, string facilitytimeZone)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN plan.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string planQuery = $"select plan.planId,plan.title,plan.startDate , plan.description, plan.frequencyId,plan.assignedTo as assignedToId ,plan.approvedById,plan.createdById,plan.facilityId,f.name as siteName, CONCAT(createdBy.firstName, ' ' ,createdBy.lastName) as createdBy , plan.createdAt,freq.name as frequency, " +
                 $" plan.durationDays as noOfCleaningDays, facility.name, plan.approvedAt as approvedAt, CONCAT(updatedBy.firstName, ' ' ,updatedBy.lastName) as updatedbyName, CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assignedTo,plan.status,{statusOut} as status_short," +
                 $"CONCAT(approvedBy.firstName,' ' , approvedBy.lastName) as approvedbyName ," +
                 $"CONCAT(approvedBy.firstName,' ' ,approvedBy.lastName) as rejectedbyName ," +
                 $"CONCAT(deletedBy.firstName,' ' ,deletedBy.lastName) as deletedbyName , facility.name AS facilityidName, " +
                 $" plan.cleaningType as cleaningType ,CASE plan.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' when 3 then 'Robotic' else 'Wet 'end as cleaningTypeName  from cleaning_plan as plan " +
                 $" LEFT JOIN Frequency as freq on freq.id = plan.frequencyId " +
                 $" LEFT JOIN users as createdBy ON createdBy.id = plan.createdById " +
                 $" LEFT JOIN users as deletedBy ON deletedBy.id = plan.deleted_by_id " +
                 $" LEFT JOIN users as approvedBy ON approvedBy.id = plan.approvedById " +
                 $"LEFT JOIN facilities as f on f.id=plan.facilityId " +
                 $"LEFT JOIN users AS updatedBy ON plan.updatedById = updatedBy.id  " +
                 $"LEFT JOIN facilities as facility ON facility.id = plan.facilityId " +
                 $"LEFT JOIN users as assignedTo ON assignedTo.id = plan.assignedTo where plan.planId = {planId}  ;";

            List<CMMCPlan> _ViewMCPlan = await Context.GetData<CMMCPlan>(planQuery).ConfigureAwait(false);

            string measures = ",count(distinct assets.parentId ) as Invs,count(item.assetId) as smbs ";

            if (moduleType == cleaningType.Vegetation)
            {
                measures += ", sum(assets.area) as area ";
            }
            else
            {
                measures += ",sum(assets.moduleQuantity) as scheduledModules,cp.cleaningType as cleaningType ,CASE cp.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' when 3 then 'Robotic' else 'Wet 'end as cleaningTypeName";
            }

            string scheduleQuery = $"select schedule.scheduleId, schedule.plannedDay as cleaningDay {measures} from cleaning_plan_schedules as schedule  LEFT JOIN cleaning_plan_items as item on schedule.scheduleId = item.scheduleId   LEFT JOIN cleaning_plan as cp on schedule.planId = cp.planId LEFT   JOIN assets on assets.id = item.assetId where schedule.planId={planId} group by schedule.scheduleId ORDER BY schedule.scheduleId ASC";


            List<CMMCSchedule> _Schedules = await Context.GetData<CMMCSchedule>(scheduleQuery).ConfigureAwait(false);

            string equipmentQuery = $"select assets.id,assets.parentId as parentId,parent.name as parentName, assets.name as equipmentName,assets.moduleQuantity as moduleQuantity,assets.area ,item.plannedDay as noOfPlanDay  from cleaning_plan_items as item  LEFT JOIN assets  on assets.id = item.assetId left join assets as parent on assets.parentId = parent.id where item.planId={planId} ORDER BY item.plannedDay ASC ";

            List<CMMCEquipmentDetails> _Equipments = await Context.GetData<CMMCEquipmentDetails>(equipmentQuery).ConfigureAwait(false);

            foreach (CMMCSchedule Schedules in _Schedules)
            {
                Schedules.equipments = new List<CMMCEquipmentDetails>();
                foreach (CMMCEquipmentDetails equip in _Equipments)
                {
                    if (Schedules.cleaningDay == equip.noOfPlanDay)
                    {
                        Schedules.equipments.Add(equip);
                    }
                }
            }

            _ViewMCPlan[0].schedules = _Schedules;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewMCPlan[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.VEGETATION_PLAN, _Status_long, _ViewMCPlan[0]);
            _ViewMCPlan[0].status_long = _longStatus;

            foreach (var list in _ViewMCPlan)
            {
                DateTime a = list.startDate;
                if (list != null && list.approvedAt != null)
                    list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.approvedAt);
                if (list != null && list.createdAt != null)
                    list.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.createdAt);
                if (list != null && list.startDate != null)
                    list.startDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.startDate);
            }

            return _ViewMCPlan[0];
        }

        internal virtual async Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytimeZone)
        {

            string status = "";
            status = $" case WHEN task.status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} THEN 1 ELSE 0 END as isPending , " +
                $"case WHEN task.status = {(int)CMMS.CMMS_Status.EQUIP_CLEANED} THEN 1 ELSE 0 END as isCleaned," +
                $"case WHEN task.status = {(int)CMMS.CMMS_Status.EQUIP_ABANDONED} THEN 1 ELSE 0 END as isAbandoned , ";

            string smbQuery = $"select task.assetId as smbId, assets.name as smbName , assets.parentId as parentId , task.{measure}, {status} " +
                " task.plannedDate AS scheduledAt, task.cleanedAt AS cleanedAt, task.abandonedAt AS abandonedAt" +
                $", plannedDay as scheduledDay, executionDay as executedDay from cleaning_execution_items as task left join assets on assets.id = task.assetId where task.executionId ={taskId}";

            List<CMSMB> smbs = await Context.GetData<CMSMB>(smbQuery).ConfigureAwait(false);
            string invQuery = $"Select id as invId , name as invName from assets where id in (Select  parent.id as id from cleaning_execution_items as task left join  assets on assets.id = task.assetId left join assets as parent on parent.id = assets.parentId where task.executionId ={taskId} group by assets.parentId)";
            List<CMMCTaskEquipmentList> invs = await Context.GetData<CMMCTaskEquipmentList>(invQuery).ConfigureAwait(false);
            foreach (CMMCTaskEquipmentList inv in invs)
            {
                inv.moduleQuantity = 0;
                inv.area = 0;
                foreach (CMSMB smb in smbs)
                {
                    if (inv.invId == smb.parentId)
                    {
                        inv.moduleQuantity += smb.moduleQuantity;
                        inv.area += smb.area;
                        inv?.smbs.Add(smb);
                    }
                }

            }
            foreach (var smb in smbs)
            {
                if (smb != null && smb.abandonedAt != null)
                    smb.abandonedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, smb.abandonedAt);
                if (smb != null && smb.cleanedAt != null)
                    smb.cleanedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, smb.cleanedAt);
                if (smb != null && smb.scheduledAt != null)
                    smb.scheduledAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, smb.scheduledAt);
            }
            return invs;
        }

        internal virtual async Task<List<CMMCEquipmentList>> GetEquipmentList(int facility_Id)
        {
            string filter = "";
            List<CMMCEquipmentList> invs = new List<CMMCEquipmentList>();

            if (facility_Id > 0)
            {
                string invQuery = $"SELECT assets.id AS invId, assets.name AS invName, assets.moduleQuantity FROM assets JOIN assetcategories ON assets.categoryId = assetcategories.id WHERE assetcategories.name=\"Inverter\" and facilityId={facility_Id}  ";


                invs = await Context.GetData<CMMCEquipmentList>(invQuery).ConfigureAwait(false);


                string smbQuery = $"SELECT assets.id as smbId, assets.name as smbName , assets.parentId, assets.moduleQuantity ,assets.area from assets  join assetcategories on  assets.categoryId =assetcategories.id where assetcategories.name = \"SMB\" and facilityId={facility_Id}  ";

                List<CMPlanSMB> smbs = await Context.GetData<CMPlanSMB>(smbQuery).ConfigureAwait(false);

                //List<CMSMB> invSmbs = new List<CMSMB>;

                foreach (CMMCEquipmentList inv in invs)
                {
                    inv.moduleQuantity = 0;
                    inv.area = 0;

                    foreach (CMPlanSMB smb in smbs)
                    {
                        if (inv.invId == smb.parentId)
                        {
                            inv.moduleQuantity += smb.moduleQuantity;
                            inv.area += smb.area;
                            inv?.smbs.Add(smb);
                        }
                    }
                }
            }
            return invs;
        }

        internal async Task<CMMCExecution> GetExecutionDetails(int excutionId, string facilitytimeZone)
        {

            string statusEx = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusEx += $"WHEN ex.status = {status.Key} THEN '{status.Value}' ";
            }
            statusEx += $"ELSE 'Invalid Status' END";

            string statusSc = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusSc += $"WHEN schedule.status = {status.Key} THEN '{status.Value}' ";
            }
            statusSc += $"ELSE 'Invalid Status' END";

            string statusEquip = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusEquip += $"WHEN item.status = {status.Key} THEN '{status.Value}' ";
            }
            statusEquip += $"ELSE 'Invalid Status' END";

            string executionQuery = $"select ex.id as executionId,ex.status ,ex.startDate as scheduledDate,ex.assignedTo as assignedToId, CONCAT(assignedToUser.firstName, assignedToUser.lastName) as assignedTo ,F.name as site_name , " +
               $"plan.title, CONCAT(createdBy.firstName, ' ' , createdBy.lastName) as plannedBy ,plan.createdAt as plannedAt,freq.name as frequency, ex.executedById as startedById, CONCAT(startedByUser.firstName, ' ' ,startedByUser.lastName) as startedBy ," +
               $" ex.executionStartedAt as startedAt , ex.rejectedById, CONCAT(rejectedByUser.firstName, ' ', rejectedByUser.lastName) as rejectedbyName,ex.rejectedAt,ex.approvedById, CONCAT(approvedByUser.firstName,' ', approvedByUser.lastName) as approvedbyName,ex.approvedAt,  {statusEx} as status_short, " +
               $" CONCAT(endedByUser.firstName, ' ', endedByUser.lastName) as endedBy, ex.endedAt , ex.abandonApprovedAt, ex.abandonRejectedAt, ex.endedById as endedById, f.name as facilityidName, " +
               $" CONCAT(abandonedUser.firstName,' ', abandonedUser.lastName) as abandonedBy,ex.abandonedAt, ex.abandonedById, ex.end_approved_at, ex.end_rejected_at, " +
               $" CONCAT(updatedByUser.firstName,' ', updatedByUser.lastName) as updatedBy, ex.updatedById as updatedById, " +
               $" CONCAT(endapprovedUser.firstName,' ', endapprovedUser.lastName) as endapprovedbyName, ex.end_approved_id as endapprovedbyId, " +
               $" CONCAT(endrejectedUser.firstName,' ', endrejectedUser.lastName) as endrejectedbyName, ex.end_rejected_id as endrejectedbyId, " +
               $" CONCAT(abandonApprovedUser.firstName,' ', abandonApprovedUser.lastName) as abandonApprovedByName, ex.abandonApprovedBy as abandonApprovedById, " +
               $" CONCAT(abandonRejectedUser.firstName,' ', abandonRejectedUser.lastName) as abandonRejectedByName, ex.abandonRejectedBy as abandonRejectedById " +
               $" from cleaning_execution as ex LEFT JOIN cleaning_plan as plan on ex.planId = plan.planId " +
               $" LEFT JOIN Frequency as freq on freq.id = plan.frequencyId " +
               $" left join facilities as F on F.id = ex.facilityId  " +
               $" LEFT JOIN users as createdBy ON createdBy.id = plan.createdById " +
               $" LEFT JOIN users as rejectedByUser ON rejectedByUser.id = ex.rejectedById " +
               $" LEFT JOIN users as approvedByUser ON approvedByUser.id = ex.approvedById " +
               $" LEFT JOIN users as endapprovedUser ON endapprovedUser.id = ex.end_approved_id " +
               $" LEFT JOIN users as endrejectedUser ON endrejectedUser.id = ex.end_rejected_id " +
               $" LEFT JOIN users as startedByUser ON startedByUser.id = ex.executedById " +
               $" LEFT JOIN users as endedByUser ON endedByUser.id = ex.endedById" +
               $" LEFT JOIN users as updatedByUser ON updatedByUser.id = ex.updatedById" +
               $" LEFT JOIN users as abandonedUser ON abandonedUser.id = ex.abandonedById" +
               $" LEFT JOIN users as abandonApprovedUser ON abandonApprovedUser.id = ex.abandonApprovedBy " +
               $" LEFT JOIN users as abandonRejectedUser ON abandonRejectedUser.id = ex.abandonRejectedBy " +
               $" LEFT JOIN users as assignedToUser ON assignedToUser.id = ex.assignedTo where ex.id={excutionId};";


            List<CMMCExecution> _ViewExecution = await Context.GetData<CMMCExecution>(executionQuery).ConfigureAwait(false);

            string scheduleQuery = $"select schedule.scheduleId as scheduleId ,schedule.status ,schedule.executionId, schedule.actualDay as cleaningDay ,schedule.startedAt as start_date,schedule.endedAt as end_date, permit.startDate as startDate, CASE when permit.startDate <  now() then 1 else 0 END as tbt_start, " +
                                  $" cp.cleaningType ,CASE cp.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' when 3 then 'Robotic'  else 'Wet '  end as cleaningTypeName, SUM({measure}) as scheduled , " +
                                  $" permit.id as permit_id,permit.code as  permit_code, schedule.rejectedById, CONCAT(rejectedByUser.firstName, rejectedByUser.lastName) as rejectedBy, schedule.rejectedAt,schedule.approvedById, CONCAT(approvedByUser.firstName, approvedByUser.lastName) as approvedBy, schedule.approvedAt, " +
                                  $" Case when permit.TBT_Done_By is null or permit.TBT_Done_By = 0 then 0 else 1 end ptw_tbt_done,permit.status as ptw_status ," +
                                  $" SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_CLEANED} THEN {measure} ELSE 0 END) as cleaned , SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_ABANDONED} THEN {measure} ELSE 0 END) as abandoned , " +
                                  $" SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} THEN {measure} ELSE 0 END) as pending ,schedule.remark_of_schedule as remark_of_schedule ,schedule.waterUsed, schedule.remark as remark ,{statusSc} as status_short from cleaning_execution_schedules as schedule " +
                                  $" left join cleaning_execution_items as item on schedule.scheduleId = item.scheduleId " +
                                  $" left join permits as permit on permit.id = schedule.ptw_id " +
                                  $" LEFT JOIN users as rejectedByUser ON rejectedByUser.id = schedule.rejectedById " +
                                  $" LEFT JOIN users as approvedByUser ON approvedByUser.id = schedule.approvedById " +
                                  $" left join cleaning_plan as cp on schedule.planId= cp.planId " +
                                  $" where schedule.executionId = {excutionId} group by schedule.scheduleId;";

            List<CMMCExecutionSchedule> _ViewSchedule = await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);



            _ViewExecution[0].noOfDays = _ViewSchedule.Count;
            _ViewExecution[0].schedules = _ViewSchedule;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewExecution[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.MC_TASK, _Status_long, _ViewExecution[0]);
            _ViewExecution[0].status_long = _longStatus;
            foreach (var item in _ViewSchedule)
            {
                CMMS.CMMS_Status ptw_status1 = (CMMS.CMMS_Status)(item.ptw_status);
                item.status_short_ptw = Status_PTW((int)ptw_status1);
            }

            foreach (var view in _ViewExecution)
            {
                if (view != null && view.abandonedAt != null)
                    view.abandonedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.abandonedAt);
                if (view != null && view.plannedAt != null)
                    view.plannedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.plannedAt);
                if (view != null && view.scheduledDate != null)
                    view.scheduledDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.scheduledDate);
                if (view != null && view.startedAt != null)
                    view.startedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.startedAt);

            }
            return _ViewExecution[0];
        }

        internal async Task<CMMCExecutionSchedule> GetScheduleDetails(int scheduleId, string facilitytimeZone)
        {

            string statusEx = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusEx += $"WHEN ex.status = {status.Key} THEN '{status.Value}' ";
            }
            statusEx += $"ELSE 'Invalid Status' END";

            string statusSc = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusSc += $"WHEN schedule.status = {status.Key} THEN '{status.Value}' ";
            }
            statusSc += $"ELSE 'Invalid Status' END";

            string statusEquip = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusEquip += $"WHEN item.status = {status.Key} THEN '{status.Value}' ";
            }
            statusEquip += $"ELSE 'Invalid Status' END";


            string scheduleQuery = $"select schedule.scheduleId as scheduleId ,schedule.status ,schedule.executionId, schedule.moduleType, schedule.actualDay as cleaningDay ,schedule.startedAt as start_date,schedule.endedAt as end_date  , " +
                                              $" cp.cleaningType ,CASE cp.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' when 3 then 'Robotic'  else 'Wet '  end as cleaningTypeName, SUM({measure}) as scheduled , " +
                                              $" permit.id as permit_id,permit.code as  permit_code , schedule.rejectedById, CONCAT(rejectedByUser.firstName, ' ',rejectedByUser.lastName) as rejectedBy,schedule.rejectedAt, schedule.approvedById, CONCAT(approvedByUser.firstName, ' ',approvedByUser.lastName) as approvedBy, schedule.createdbyId, CONCAT(createdByUser.firstName, createdByUser.lastName) as createdbyName, schedule.startedById, CONCAT(startedByUser.firstName, ' ',startedByUser.lastName) as startedbyName ," +
                                              $" schedule.approvedAt, schedule.abandonedAt, schedule.updatedAt, title.title AS title, description.description AS description, schedule.endedById, CONCAT(endedByUser.firstName, ' ',endedByUser.lastName) as endedbyName, schedule.abandonedById, CONCAT(abandonedByUser.firstName, ' ',abandonedByUser.lastName) as abandonedbyName, schedule.updatedById, CONCAT(updatedByUser.firstName, ' ',updatedByUser.lastName) as updatedbyName," +
                                              $" Case when permit.TBT_Done_By is null or permit.TBT_Done_By = 0 then 0 else 1 end ptw_tbt_done,permit.status as ptw_status , f.name AS facilityidName, " +
                                              $" SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_CLEANED} THEN {measure} ELSE 0 END) as cleaned , SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_ABANDONED} THEN {measure} ELSE 0 END) as abandoned , " +    //Pending : why why measure/area is as cleaned
                                              $" SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} THEN {measure} ELSE 0 END) as pending ,schedule.remark_of_schedule as remark_of_schedule ,schedule.waterUsed, schedule.remark as remark ,{statusSc} as status_short from cleaning_execution_schedules as schedule " +
                                              $" left join cleaning_execution_items as item on schedule.scheduleId = item.scheduleId " +
                                              $" left join permits as permit on permit.id = schedule.ptw_id " +
                                              $" LEFT JOIN users as rejectedByUser ON rejectedByUser.id = schedule.rejectedById " +
                                              $" LEFT JOIN users as approvedByUser ON approvedByUser.id = schedule.approvedById " +
                                              $" LEFT JOIN users as createdByUser ON createdByUser.id = schedule.createdbyId " +
                                              $" LEFT JOIN users as startedByUser ON startedByUser.id = schedule.startedById " +
                                              $" LEFT JOIN users as endedByUser ON endedByUser.id = schedule.endedById " +
                                              $" LEFT JOIN users as abandonedByUser ON abandonedByUser.id = schedule.abandonedById " +
                                              $" LEFT JOIN users as updatedByUser ON updatedByUser.id = schedule.updatedById " +
                                              $" left join cleaning_plan as cp on schedule.planId= cp.planId " +
                                              $" left join cleaning_plan as title on schedule.planId= title.planId " +
                                              $" LEFT JOIN cleaning_execution t ON schedule.executionId = t.id  " +
                                              $" LEFT JOIN  facilities f ON t.facilityId = f.id " +
                                              $" left join cleaning_plan as description on schedule.planId= description.planId " +
                                              $" where schedule.scheduleId = {scheduleId} group by schedule.scheduleId;";

            List<CMMCExecutionSchedule> _ViewSchedule = await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);





            CMMS.CMMS_Status _Status_long_Schedule = (CMMS.CMMS_Status)(_ViewSchedule[0].status);

            try
            {
                string _longStatus_Schedule = getLongStatus(CMMS.CMMS_Modules.MC_EXECUTION, _Status_long_Schedule, _ViewSchedule[0]);
                _ViewSchedule[0].status_long_schedule = _longStatus_Schedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            foreach (var item in _ViewSchedule)
            {
                CMMS.CMMS_Status ptw_status1 = (CMMS.CMMS_Status)(item.ptw_status);
                item.status_short_ptw = Status_PTW((int)ptw_status1);
            }

            return _ViewSchedule[0];
        }



        internal async Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId, string facilitytimeZone)
        {
            int planIds = 0;
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.MC_PLAN;
            int status = (int)CMMS.CMMS_Status.MC_PLAN_SUBMITTED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_PLAN;
                status = (int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED;
            }

            foreach (CMMCPlan plan in request)
            {
                int cleaningType = plan.cleaningType;
                string startDate = "NULL";

                /* if (plan.startDate != null && plan.startDate != Convert.ToDateTime("01-01-0001 00:00:00"))
                 {
                     startDate = " '" + plan.startDate.ToString("yyyy-MM-dd hh:MM:ss")+"' ";

                 }*/
                string qry = "insert into `cleaning_plan` (`moduleType`,`facilityId`,`title`,`description`, `durationDays`,`frequencyId`,cleaningType,`startDate`,`assignedTo`,`status`,`createdById`,`createdAt`, `status_updated_at`) VALUES " +
                            $"({(int)moduleType},'{plan.facilityId}','{plan.title}','{plan.description}','{plan.noOfCleaningDays}','{plan.frequencyId}',{plan.cleaningType},'{plan.startDate}','{plan.assignedToId}',{status},'{userId}','{UtilsRepository.GetUTCTime()}','{UtilsRepository.GetUTCTime()}');" +
                             "SELECT LAST_INSERT_ID() as id ;";

                List<CMMCPlan> planQry = await Context.GetData<CMMCPlan>(qry).ConfigureAwait(false);

                var planId = Convert.ToInt16(planQry[0].id.ToString());

                string scheduleQry = " ";
                string equipmentQry = $"insert into `cleaning_plan_items` (`planId`,`moduleType`,`scheduleId`,`assetId`,`plannedDay`,`createdById`,`createdAt`) VALUES ";

                if (plan.schedules.Count > 0)
                {

                    foreach (var schedule in plan.schedules)
                    {
                        //cleaningType = schedule.cleaningType;

                        //if (moduleType == cleaningType.Vegetation)
                        //    cleaningType = 0;

                        scheduleQry = "insert into `cleaning_plan_schedules` (`planId`,`moduleType`,cleaningType,`plannedDay`,`createdById`,`createdAt`,`status_updated_at` ) VALUES ";
                        scheduleQry += $"({planId},{(int)moduleType},{cleaningType},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}', '{UtilsRepository.GetUTCTime()}');" +
                                           $"SELECT LAST_INSERT_ID() as id ;";

                        List<CMMCSchedule> schedule_ = await Context.GetData<CMMCSchedule>(scheduleQry).ConfigureAwait(false);
                        var scheduleId = Convert.ToInt16(schedule_[0].id.ToString());

                        if (schedule.equipments.Count > 0)
                        {
                            foreach (var equipment in schedule.equipments)
                            {
                                equipmentQry += $"({planId},{(int)moduleType},{scheduleId},{equipment.id},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}'),";
                            }

                        }

                    }

                    if (plan.schedules[0].equipments.Count > 0)
                    {
                        equipmentQry = equipmentQry.Substring(0, equipmentQry.Length - 1);

                        equipmentQry += $"; update cleaning_plan_items left join assets on cleaning_plan_items.assetId = assets.id set cleaning_plan_items.{measure} = assets.{measure} where planId ={planId};";

                        await Context.GetData<CMMCPlan>(equipmentQry).ConfigureAwait(false);
                    }
                }

                planIds = planId;
                await _utilsRepo.AddHistoryLog(module, planId, 0, 0, "Plan Created", (CMMS.CMMS_Status)status, userId);
                try
                {
                    CMMCPlan _ViewPlanList = await GetPlanDetails(planId, facilitytimeZone);
                    await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewPlanList);

                }
                catch (Exception e)
                {
                    // response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Failed to send Vegetation Notification");
                    Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");
                }

                //var notificationResponse = await CMMSNotification.sendNotification(CMMS.CMMS_Modules.VEGETATION_PLAN, (CMMS.CMMS_Status)status, new int[] { userId }, plan);
            }

            CMDefaultResponse response = new CMDefaultResponse(planIds, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Created Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> requests, int userId, string facilitytimeZone)
        {
            int planId = 0;
            int planIds = 0;
            //int cleaningType1;
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.MC_PLAN;
            int status = (int)CMMS.CMMS_Status.MC_PLAN_UPDATED; //Pending why status updated? status should not changei in update

            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_PLAN;
                status = (int)CMMS.CMMS_Status.VEG_PLAN_UPDATED;
            }

            foreach (CMMCPlan request in requests)
            {
                int rid = request.planId;

                string Query = "UPDATE cleaning_plan SET ";
                if (request.title != null && request.title != "")
                    Query += $"title = '{request.title}', ";
                if (request.assignedToId > 0)
                    Query += $"assignedTo = '{request.assignedToId}', ";
                if (request.description != null && request.description != "")
                    Query += $"description = '{request.description}', ";
                if (request.frequencyId > 0)
                    Query += $"frequencyId = {request.frequencyId}, ";
                if (request.cleaningType > 0)
                    Query += $"cleaningType = {request.cleaningType}, ";

                Query += $"startDate = '{request.startDate}', ";
                if (request.noOfCleaningDays > 0)
                {
                    Query += $"durationDays = {request.noOfCleaningDays}, ";
                }
                if (moduleType == cleaningType.ModuleCleaning)
                {
                    if (request.resubmit == 1)
                    {
                        Query += $"status = {(int)CMMS.CMMS_Status.MC_PLAN_SUBMITTED}, ";
                    }
                }

                else if (moduleType == cleaningType.Vegetation)
                {
                    if (request.resubmit == 1)
                    {
                        Query += $"status = {(int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED}, ";
                    }
                }


                Query += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedById = {userId} WHERE planId = {request.planId};";

                await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);
                string myQuery12 = $"Delete from cleaning_plan_items where planId= {request.planId};";
                await Context.ExecuteNonQry<int>(myQuery12).ConfigureAwait(false);
                planId = request.planId;
                string myQuery = "";
                string scheduleQry = " ";
                string equipmentQry = $"insert into `cleaning_plan_items` (`planId`,`moduleType`,`scheduleId`,`assetId`,`plannedDay`,`createdById`,`createdAt`) VALUES ";

                if (request.schedules.Count > 0)
                {

                    foreach (var schedule in request.schedules)
                    {
                        //cleaningType1 = schedule.cleaningType;


                        /*
                        scheduleQry = "insert into `cleaning_plan_schedules` (`planId`,`moduleType`,cleaningType,`plannedDay`,`createdById`,`createdAt`) VALUES ";
                         scheduleQry += $"({request.planId},{(int)moduleType},{request.cleaningType},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}');" +
                                            $"SELECT LAST_INSERT_ID() as id ;";

                         List<CMMCSchedule> schedule_ = await Context.GetData<CMMCSchedule>(scheduleQry).ConfigureAwait(false);
                         var scheduleId = Convert.ToInt16(schedule_[0].id.ToString());*/
                        bool check = true;
                        int schedule_Id = 0;
                        if (schedule.scheduleId > 0)
                        {
                            myQuery += $"UPDATE cleaning_plan_schedules SET updatedAt = '{UtilsRepository.GetUTCTime()}', updatedById = {userId}, plannedDay = {schedule.cleaningDay}  " +
                                       $"WHERE  planId = {request.planId} AND scheduleId = {schedule.scheduleId};";

                        }
                        else
                        {
                            string myQuery1 = $"INSERT INTO cleaning_plan_schedules (planId,plannedDay,moduleType,cleaningType, updatedAt, updatedById, createdAt, createdById) " +
                                              $"VALUES ({request.planId}, {schedule.cleaningDay},{(int)moduleType},{request.cleaningType}, '{UtilsRepository.GetUTCTime()}', {userId}, '{UtilsRepository.GetUTCTime()}', {userId});" +
                                              $"SELECT LAST_INSERT_ID();";
                            DataTable dt = await Context.FetchData(myQuery1).ConfigureAwait(false);
                            schedule_Id = Convert.ToInt32(dt.Rows[0][0]);
                            check = false;
                        }

                        // Delete existing cleaning plan items for this scheduleId

                        myQuery += $"DELETE FROM cleaning_plan_items WHERE scheduleId = {schedule.scheduleId};";

                        int sc_id = 0;
                        if (check)
                        {
                            sc_id = schedule.scheduleId;
                        }
                        else
                        {
                            sc_id = schedule_Id;
                        }

                        if (schedule.equipments.Count > 0)
                        {
                            foreach (var equipment in schedule.equipments)
                            {
                                equipmentQry += $"({request.planId},{(int)moduleType},{sc_id},{equipment.id},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}'),";
                            }

                        }
                    }

                    if (request.schedules[0].equipments.Count > 0)
                    {
                        equipmentQry = equipmentQry.Substring(0, equipmentQry.Length - 1);

                        equipmentQry += $"; update cleaning_plan_items left join assets on cleaning_plan_items.assetId = assets.id set cleaning_plan_items.{measure} = assets.{measure} where planId ={request.planId};";

                        await Context.GetData<CMMCPlan>(equipmentQry).ConfigureAwait(false);
                    }
                }

                if (moduleType == cleaningType.ModuleCleaning)
                {
                    if (request.resubmit == 1)
                    {

                        await _utilsRepo.AddHistoryLog(module, rid, 0, 0, "Plan Updated Successfully", CMMS.CMMS_Status.MC_PLAN_SUBMITTED, userId);

                        return new CMDefaultResponse(rid, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully");

                    }

                }
                else if (moduleType == cleaningType.Vegetation)
                {
                    await _utilsRepo.AddHistoryLog(module, rid, 0, 0, "Plan Updated Successfully", CMMS.CMMS_Status.VEG_PLAN_SUBMITTED, userId);

                    return new CMDefaultResponse(rid, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully");
                }
                //var notificationResponse = await CMMSNotification.sendNotification(CMMS.CMMS_Modules.VEGETATION_PLAN, CMMS.CMMS_Status.UPDATED, new int[] { userId });

                if (moduleType == cleaningType.ModuleCleaning)
                {
                    await _utilsRepo.AddHistoryLog(module, request.planId, 0, 0, "Plan Updated", CMMS.CMMS_Status.MC_PLAN_DRAFT, userId);
                }
                else if (moduleType == cleaningType.Vegetation)
                {
                    await _utilsRepo.AddHistoryLog(module, request.planId, 0, 0, "Plan Updated", CMMS.CMMS_Status.VEG_PLAN_DRAFT, userId);
                }
                try
                {
                    CMMCPlan _ViewPlanList = await GetPlanDetails(planId, facilitytimeZone);
                    await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewPlanList);
                }

                catch (Exception ex)
                {
                    // response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Failed to send Vegetation Notification");
                    Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
                }
            }
            CMDefaultResponse response = new CMDefaultResponse(planId, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully ");
            return response;
        }

        internal async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userID, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.MC_PLAN;
            int status = (int)CMMS.CMMS_Status.MC_PLAN_APPROVED;
            int task_schedule_status = (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED;
            CMMS.CMMS_Modules task_module = CMMS.CMMS_Modules.MC_TASK;

            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_PLAN;
                status = (int)CMMS.CMMS_Status.VEG_PLAN_APPROVED;

                task_schedule_status = (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED;
                task_module = CMMS.CMMS_Modules.VEGETATION_TASK;
            }


            string approveQuery = $"Update cleaning_plan set isApproved = 1,status= {status} ,approvedById={userID}, ApprovalReason='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where planId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);


            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status)  " +
               $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId,durationDays as noOfDays ,startDate,assignedTo," +
               $"{task_schedule_status} as status " +
               $"from cleaning_plan where planId = {request.id}; " +
               $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int taskid = Convert.ToInt32(dt3.Rows[0][0]);
            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,moduleType,cleaningType,status) " +
                            $"select {taskid} as executionId,planId as planId, plannedDay,{(int)moduleType},cleaningType,{task_schedule_status} as status from cleaning_plan_schedules where planId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);
            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,`status`) SELECT '{taskid}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay - 1  DAY) as plannedDate,item.plannedDay,schedule.createdById,schedule.createdAt,{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_plan_items as item join cleaning_execution_schedules as schedule on item.planId = schedule.planId and item.plannedDay = schedule.actualDay where schedule.executionId = {taskid}";

            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(module, request.id, task_module, taskid, request.comment, (CMMS.CMMS_Status)status, userID);
            //            await _utilsRepo.AddHistoryLog(task_module, taskid, 0, 0, "Execution scheduled", (CMMS.CMMS_Status)status, userID);
            /*
                        if (moduleType == cleaningType.ModuleCleaning)
                        {
                            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status)  " +
                                          $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId,durationDays as noOfDays ,startDate,assignedTo," +
                                          $"{(int)CMMS.CMMS_Status.MC_TASK_SCHEDULED} as status " +
                                          $"from cleaning_plan where planId = {request.id}; " +
                                          $"SELECT LAST_INSERT_ID(); ";

                            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                            int taskid = Convert.ToInt32(dt3.Rows[0][0]);
                            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,moduleType,cleaningType,status) " +
                                            $"select {taskid} as executionId,planId as planId, plannedDay,{(int)moduleType},cleaningType,{(int)CMMS.CMMS_Status.MC_TASK_SCHEDULED} as status from cleaning_plan_schedules where planId = {request.id}";
                            await Context.ExecuteNonQry<int>(scheduleQry);
                            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,`status`) SELECT '{taskid}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay - 1  DAY) as plannedDate,item.plannedDay,schedule.createdById,schedule.createdAt,{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_plan_items as item join cleaning_execution_schedules as schedule on item.planId = schedule.planId and item.plannedDay = schedule.actualDay where schedule.executionId = {taskid}";

                            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_TASK, taskid, 0, 0, "Execution scheduled", (CMMS.CMMS_Status)status, userID);

                        }
                        else if(moduleType == cleaningType.Vegetation)
                        {
                            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status)  " +
                                      $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId,durationDays as noOfDays ,startDate,assignedTo," +
                                      $"{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED} as status " +
                                      $"from cleaning_plan where planId = {request.id}; " +
                                      $"SELECT LAST_INSERT_ID(); ";

                            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                            int taskid = Convert.ToInt32(dt3.Rows[0][0]);
                            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,moduleType,cleaningType,status) " +
                                            $"select {taskid} as executionId,planId as planId, plannedDay,{(int)moduleType},cleaningType,{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED} as status from cleaning_plan_schedules where planId = {request.id}";
                            await Context.ExecuteNonQry<int>(scheduleQry);
                            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,`status`) SELECT '{taskid}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay - 1  DAY) as plannedDate,item.plannedDay,schedule.createdById,schedule.createdAt,{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_plan_items as item join cleaning_execution_schedules as schedule on item.planId = schedule.planId and item.plannedDay = schedule.actualDay where schedule.executionId = {taskid}";
                            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, taskid, 0, 0, "Execution scheduled", (CMMS.CMMS_Status)status, userID);


                        }
            */

            try
            {
                CMMCPlan _ViewPlanList = await GetPlanDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewPlanList);
            }
            catch (Exception e)
            {
                // response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Failed to send Vegetation Notification");
                Console.WriteLine($"Failed to send {(CMMS.CMMS_Status)status} Notification: {e.Message}");
            }


            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Approved");
            return response;

        }

        internal async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userID, string facilitytimeZone)
        {

            int status;
            CMMS.CMMS_Modules module;

            module = CMMS.CMMS_Modules.MC_PLAN;
            status = (int)CMMS.CMMS_Status.MC_PLAN_REJECTED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_PLAN;
                status = (int)CMMS.CMMS_Status.VEG_PLAN_REJECTED;
            }
            string approveQuery = $"Update cleaning_plan set isApproved = 2,status= {status},approvedById={userID},ApprovalReason='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where planId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);
            try
            {
                CMMCPlan _ViewPlanList = await GetPlanDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewPlanList);
            }

            catch (Exception e)
            {
                Console.WriteLine($"Failed to send {(CMMS.CMMS_Status)status} Vegetation Notification: {e.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePlan(int planid, int userID, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_PLAN;
            status = (int)CMMS.CMMS_Status.MC_PLAN_DELETED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_PLAN;
                status = (int)CMMS.CMMS_Status.VEG_PLAN_DELETED;
            }

            string deleteQuery = $"UPDATE cleaning_plan SET deleted_by_id  = {userID}, status = {status}, status_updated_at = '{UtilsRepository.GetUTCTime()}' WHERE planid = {planid};";
            await Context.ExecuteNonQry<int>(deleteQuery).ConfigureAwait(false);

            try
            {
                CMMCPlan _ViewPlanList = await GetPlanDetails(planid, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewPlanList);
            }

            catch (Exception e)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");
            }
            string approveQuery = $"Delete from cleaning_plan where planId ={planid};Delete from cleaning_plan_schedules where planId ={planid};Delete from cleaning_plan_items where planId ={planid}"; ;
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, planid, 0, 0, "Plan Deleted", (CMMS.CMMS_Status)status, userID);

            CMDefaultResponse response = new CMDefaultResponse(planid, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Deleted");
            return response;
        }


        internal async Task<CMDefaultResponse> ApproveEndExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            //comment remark='{request.comment}'
            int taskid = 0;

            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.MC_TASK;
            int status = (int)CMMS.CMMS_Status.MC_TASK_END_APPROVED;
            int task_schedule_status = (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED;

            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED;

                task_schedule_status = (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED;
            }

            string approveQuery = $"Update cleaning_execution set status= {status} ,end_approved_id={userID}, status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                $" end_approved_at='{UtilsRepository.GetUTCTime()}',rescheduled = 1 where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string statusQry = $"SELECT status FROM cleaning_execution  WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status1 = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status1 != (CMMS.CMMS_Status)status)
                return new CMRescheduleApprovalResponse(0, request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a End Vegetation Task can be Approved ");

            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status,prevTaskId,prevTaskDoneDate)  " +
             $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId,noOfDays as noOfDays , " +
             $"Case WHEN cleaning_execution.frequencyId in(4,5,6) THEN DATE_ADD(startDate,INTERVAL freq.months MONTH) WHEN  cleaning_execution.frequencyId=7 THEN DATE_ADD(startDate,INTERVAL 1 YEAR)  else DATE_ADD(startDate, INTERVAL freq.days DAY) end as startDate,assignedTo , " +
             $"{(int)task_schedule_status} as status,{request.id} as prevTaskId, '{UtilsRepository.GetUTCTime()}' as prevTaskDoneDate " +
             $" from cleaning_execution left join frequency as freq on cleaning_execution.frequencyId= freq.id " +
             $" where cleaning_execution.id = {request.id}; " +
             $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            taskid = Convert.ToInt32(dt3.Rows[0][0]);
            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,moduleType,actualDay,cleaningType,status) " +
                           $"select {taskid} as executionId,planId as planId,{(int)moduleType},actualDay,cleaningType,{(int)task_schedule_status} as status from cleaning_execution_schedules where cleaning_execution_schedules.executionId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            /*
            if (moduleType == cleaningType.ModuleCleaning)
            {
                string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status,prevTaskId,prevTaskDoneDate)  " +
                             $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId,noOfDays as noOfDays , " +
                             $"Case WHEN cleaning_execution.frequencyId in(4,5,6) THEN DATE_ADD(startDate,INTERVAL freq.months MONTH) WHEN  cleaning_execution.frequencyId=7 THEN DATE_ADD(startDate,INTERVAL 1 YEAR)  else DATE_ADD(startDate, INTERVAL freq.days DAY) end as startDate,assignedTo , " +
                             $"{(int)CMMS.CMMS_Status.MC_TASK_SCHEDULED} as status,{request.id} as prevTaskId, '{UtilsRepository.GetUTCTime()}' as prevTaskDoneDate " +
                             $" from cleaning_execution left join frequency as freq on cleaning_execution.frequencyId= freq.id " +
                             $" where cleaning_execution.id = {request.id}; " +
                             $"SELECT LAST_INSERT_ID(); ";
                DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                taskid = Convert.ToInt32(dt3.Rows[0][0]);
                string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,moduleType,actualDay,cleaningType,status) " +
                               $"select {taskid} as executionId,planId as planId,{(int)moduleType},actualDay,cleaningType,{(int)CMMS.CMMS_Status.MC_TASK_SCHEDULED} as status from cleaning_execution_schedules where cleaning_execution_schedules.executionId = {request.id}";
                 await Context.ExecuteNonQry<int>(scheduleQry);
            }
            else if(moduleType == cleaningType.Vegetation)
            {
                string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status,prevTaskId,prevTaskDoneDate)  " +
                            $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId,noOfDays as noOfDays , " +
                            $"Case WHEN cleaning_execution.frequencyId in(4,5,6) THEN DATE_ADD(startDate,INTERVAL freq.months MONTH) WHEN  cleaning_execution.frequencyId=7 THEN DATE_ADD(startDate,INTERVAL 1 YEAR)  else DATE_ADD(startDate, INTERVAL freq.days DAY) end as startDate,assignedTo , " +
                            $"{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED} as status,{request.id} as prevTaskId, '{UtilsRepository.GetUTCTime()}' as prevTaskDoneDate " +
                            $" from cleaning_execution left join frequency as freq on cleaning_execution.frequencyId= freq.id " +
                            $" where cleaning_execution.id = {request.id}; " +
                            $"SELECT LAST_INSERT_ID(); ";
                DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                taskid = Convert.ToInt32(dt3.Rows[0][0]);
                string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,moduleType,actualDay,cleaningType,status) " +
                               $"select {taskid} as executionId,planId as planId,{(int)moduleType},actualDay,cleaningType,{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED} as status from cleaning_execution_schedules where cleaning_execution_schedules.executionId = {request.id}";
                await Context.ExecuteNonQry<int>(scheduleQry);
            }
            */
            //for clone of item
            string scheduleOld = $"SELECT scheduleId as id FROM cleaning_execution_schedules where executionId = {taskid}";
            List<ApproveMC> new_sc_ = await Context.GetData<ApproveMC>(scheduleOld);
            string scheduleNew = $"SELECT scheduleId as schedule_id FROM cleaning_execution_schedules where executionId = {request.id}";
            List<ApproveMC> old_sc_ = await Context.GetData<ApproveMC>(scheduleNew);
            for (int i = 0; i < old_sc_.Count; i++)
            {
                string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,`status`) " +
               $"SELECT '{taskid}' as executionId,item.moduleType,{new_sc_[i].id}, item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay - 1  DAY) as plannedDate, " +
               $"item.plannedDay,item.createdById,item.createdAt,  " +
               $"{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_execution_items as item  " +
               $"join cleaning_execution as schedule on item.executionId = schedule.id  where item.executionId= {request.id} and item.scheduleId={old_sc_[i].schedule_id}";

                var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);
            }

            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send {(CMMS.CMMS_Status)status} Vegetation Notification: {ex.Message}");
            }
            CMDefaultResponse response = new CMRescheduleApprovalResponse(taskid, request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution End Approved To Vegetation Task {taskid}");

            return response;

        }


        internal async Task<CMDefaultResponse> EndExecution(int executionId, int userId, string facilitytimeZone)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.MC_TASK;
            int status = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED; ;
            }



            string Query = $"Update cleaning_execution set status = {status},endedById={userId},endedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}'  where id = {executionId}; ";
            //$"Update cleaning_execution_schedules set status = {status} where scheduleId = {scheduleId}";

            int retVal = await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);



            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(module, executionId, 0, 0, "Execution Completed", (CMMS.CMMS_Status)status, userId);

            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(executionId, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }



            CMDefaultResponse response = new CMDefaultResponse(executionId, CMMS.RETRUNSTATUS.SUCCESS, $"Task Execution Completed");
            return response;
        }

        internal async Task<CMDefaultResponse> StartExecution(int executionId, int userId, string facilitytimeZone)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_STARTED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_STARTED;
            }

            string Query = $"Update cleaning_execution set status = {status},executedById={userId},executionStartedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {executionId}; ";

            int retVal = await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);


            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(module, executionId, 0, 0, "Execution Started", (CMMS.CMMS_Status)status, userId);

            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(executionId, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);

            }

            catch (Exception e)
            {
                // response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Failed to send Vegetation Notification");

            }


            CMDefaultResponse response = new CMDefaultResponse(executionId, CMMS.RETRUNSTATUS.SUCCESS, $"Task Execution started");
            return response;
        }

        internal async Task<CMDefaultResponse> ReAssignTask(int task_id, int assign_to, int userID, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            int theStatus;
            int retVal = 0;


            module = CMMS.CMMS_Modules.MC_TASK;
            theStatus = (int)CMMS.CMMS_Status.MC_TASK_ASSIGNED;
            //CMMS.CMMS_Status taskStatus = CMMS.CMMS_Status.MC_TASK_SCHEDULED;
            CMDefaultResponse response;

            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                theStatus = (int)CMMS.CMMS_Status.VEG_TASK_ASSIGNED;
                //CMMS.CMMS_Status taskStatus = CMMS.CMMS_Status.VEG_TASK_SCHEDULED;
            }


            string statusQry = $"SELECT status FROM cleaning_execution WHERE id = {task_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            //prndong. write common code
            if (moduleType == cleaningType.ModuleCleaning)
            {
                if (status != CMMS.CMMS_Status.MC_TASK_RESCHEDULED && status != CMMS.CMMS_Status.MC_PLAN_APPROVED && status != CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW)
                {
                    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Only Scheduled Tasks can be assigned");
                }

                string myQuery = "UPDATE cleaning_execution SET " +
                               $"assignedTo = {assign_to}, " +
                               $"status = {(int)CMMS.CMMS_Status.MC_TASK_ASSIGNED}, " +
                               $"updatedAt = '{UtilsRepository.GetUTCTime()}', " +
                               $"updatedById = {userID}, " +
                               $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                               $"WHERE id = {task_id} ;";
                retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            }

            else if (moduleType == cleaningType.Vegetation)
            {
                if (status != CMMS.CMMS_Status.VEG_TASK_RESCHEDULED && status != CMMS.CMMS_Status.VEG_PLAN_APPROVED && status != CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW)
                {
                    return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Only Scheduled Tasks can be assigned");
                }
                string myQuery = "UPDATE cleaning_execution SET " +
                              $"assignedTo = {assign_to}, " +
                              $"status = {(int)CMMS.CMMS_Status.VEG_TASK_ASSIGNED}, " +
                              $"updatedAt = '{UtilsRepository.GetUTCTime()}', " +
                              $"updatedById = {userID},  " +
                              $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                              $"WHERE id = {task_id} ;";
                retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            }
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            response = new CMDefaultResponse();

            if (moduleType == cleaningType.ModuleCleaning)
            {
                if (status == CMMS.CMMS_Status.MC_TASK_SCHEDULED)
                {
                    response = new CMDefaultResponse(task_id, retCode, $"MC Task Assigned To user Id {assign_to}");
                }
                else
                {
                    response = new CMDefaultResponse(task_id, retCode, $"MC Task Reassigned To user Id {assign_to}");
                }
            }
            else if (moduleType == cleaningType.Vegetation)
            {
                if (status == CMMS.CMMS_Status.VEG_TASK_SCHEDULED)
                {
                    response = new CMDefaultResponse(task_id, retCode, $"Vegetation Task Assigned To user Id {assign_to}");
                }
                else
                {
                    response = new CMDefaultResponse(task_id, retCode, $"Vegetation Task Reassigned To user Id {assign_to}");
                }
            }

            await _utilsRepo.AddHistoryLog(module, task_id, 0, 0, $" Task Assigned to user_Id {assign_to}", (CMMS.CMMS_Status)status, userID);
            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(task_id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;
            }
            string Query = $"Update cleaning_execution set status = {status},abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,reasonForAbandon = '{request.comment}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id};";
            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);
            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);
            }

            catch (Exception e)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> ApproveAbandonExecution(CMApproval request, int userId, string facilitytimeZone)
        {

            int notStatus = (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED;

            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED;

            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED;
                notStatus = (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED;
            }


            string Query = $"Update cleaning_execution set status = {status},abandonApprovedBy={userId},abandonApprovedAt='{UtilsRepository.GetUTCTime()}', reasonForAbandon = '{request.comment}',  status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id};";
            Query += $" Update cleaning_execution_schedules set status = {status} where executionId = {request.id} and  status  IN ( {notStatus} ) ;";
            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);
            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> RejectAbandonExecution(CMApproval request, int userId, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED;

            int notStatus = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED;
                notStatus = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            }

            // Determine status and module based on moduleType

            string Query = $"Update cleaning_execution set status = {status},abandonRejectedBy={userId},abandonRejectedAt='{UtilsRepository.GetUTCTime()}' ,reasonForAbandon = '{request.comment}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id};";

            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);

            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);
            }

            catch (Exception e)
            {
                Console.WriteLine($"Failed to send {(CMMS.CMMS_Status)status} Vegetation Notification: {e.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Task Abandon rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectEndExecution(ApproveMC request, int userID, string facilitytimeZone)
        {

            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_END_REJECTED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_END_REJECTED;
            }
            string approveQuery = $"Update cleaning_execution set status= {status},end_rejected_id={userID},executionRejectedRemarks='{request.comment}', end_rejected_at='{UtilsRepository.GetUTCTime()}', executionRejectedAt = '{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, request.execution_id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);



            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewTaskList);
            }

            catch (Exception e)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution End Rejected");
            return response;

        }

        internal async Task<CMDefaultResponse> RejectExecution(CMApproval request, int userID, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_REJECTED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_REJECTED;
            }
            string approveQuery = $"Update cleaning_execution set status= {status},rejectedById={userID},remark='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);
            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> ApproveExecution(CMApproval request, int userID, string facilitytimeZone)
        {

            int status;
            int status2;

            CMMS.CMMS_Modules module;

            module = CMMS.CMMS_Modules.MC_TASK;
            status = (int)CMMS.CMMS_Status.MC_TASK_APPROVED;
            status2 = (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_TASK;
                status = (int)CMMS.CMMS_Status.VEG_TASK_APPROVED;
                status2 = (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED;
            }




            string approveQuery = $"Update cleaning_execution set status= {status} ,approvedById={userID},rescheduled = 1, remark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status,prevTaskId,prevTaskDoneDate)  " +
                              $"select planId as planId,{(int)moduleType} as moduleType,facilityId,frequencyId, noOfDays ,DATE_ADD(startDate, INTERVAL freq.days DAY),assignedTo," +
                              $"{status2} as status, {request.id} as prevTaskId, '{UtilsRepository.GetUTCTime()}' as prevTaskDoneDate " +
                              $" from cleaning_execution left join frequency as freq on cleaning_execution.frequencyId = freq.id where  cleaning_execution.id = {request.id}; " +
                              $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int taskid = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,moduleType,cleaningType,status) " +
                                $"select {taskid} as executionId,planId as planId, actualDay,{(int)moduleType},cleaningType,{status2} as status from cleaning_execution_schedules where executionId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,status) SELECT '{taskid}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval schedule.actualDay DAY) as plannedDate,schedule.actualDay,schedule.createdById,schedule.createdAt ,{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_execution_items as item join cleaning_execution_schedules as schedule on item.executionId = schedule.executionId and item.plannedDay = schedule.actualDay where schedule.executionId = {taskid}";

            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            await _utilsRepo.AddHistoryLog(module, taskid, 0, 0, "Execution scheduled", (CMMS.CMMS_Status)status2, userID);
            //pending : incorrect module



            try
            {
                CMMCExecution _ViewTaskList = await GetExecutionDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }


            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Approved");
            return response;


        }


        internal async Task<CMDefaultResponse> EndScheduleExecution(int scheduleId, int userId, string facilitytimeZone)
        {
            int status;
            CMMS.CMMS_Modules module;

            module = CMMS.CMMS_Modules.MC_EXECUTION;
            status = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
                status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED; ;
            }

            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},endedById={userId},endedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}'  where scheduleId = {scheduleId}; ";
            //$"Update cleaning_execution_items set status = {status} where scheduleId = {scheduleId}";

            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, scheduleId, 0, 0, "schedule Execution Completed", (CMMS.CMMS_Status)status, userId);
            try
            {
                CMMCExecutionSchedule _ViewTaskList = await GetScheduleDetails(scheduleId, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");

            }



            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Completed");
            return response;
        }


        internal async Task<CMDefaultResponse> RejectScheduleExecution(ApproveMC request, int userId, string facilitytimeZone)
        {

            int status;

            CMMS.CMMS_Modules module;

            module = CMMS.CMMS_Modules.MC_EXECUTION;
            status = (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
                status = (int)CMMS.CMMS_Status.VEG_TASK_END_REJECTED;
            }



            string approveQuery = $"Update cleaning_execution_schedules set status= {status},rejectedById={userId},remark='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where scheduleId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            if (moduleType == cleaningType.ModuleCleaning)
            {

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_TASK, request.schedule_id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);
            }

            else if (moduleType == cleaningType.Vegetation)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.schedule_id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);
            }

            try
            {
                CMMCExecutionSchedule _ViewTaskList = await GetScheduleDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Schedule Rejected");
            return response;

        }

        internal async Task<CMDefaultResponse> ApproveScheduleExecution(ApproveMC request, int userID, string facilitytimeZone)
        {

            int status;
            CMMS.CMMS_Modules module;

            module = CMMS.CMMS_Modules.MC_EXECUTION;
            status = (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
                status = (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED;
            }

            string approveQuery = $"Update cleaning_execution_schedules set status= {status} ,approvedById={userID}, remark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where  scheduleId= {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            try
            {
                CMMCExecutionSchedule _ViewTaskList = await GetScheduleDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userID }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Schedule Approved");
            return response;

        }

        internal async Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_EXECUTION;
            status = (int)CMMS.CMMS_Status.MC_TASK_STARTED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
                status = (int)CMMS.CMMS_Status.VEG_TASK_STARTED;
            }



            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},startedById={userId},startedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where scheduleId = {scheduleId}; ";


            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(module, scheduleId, 0, 0, "schedule Started", (CMMS.CMMS_Status)status, userId);
            try
            {
                CMMCExecutionSchedule _ViewTaskList = await GetScheduleDetails(scheduleId, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);

            }

            catch (Exception e)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {e.Message}");
            }


            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule started Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateScheduleExecution(CMMCGetScheduleExecution request, int userId, string facilitytimeZone)
        {
            int status1 = (int)CMMS.CMMS_Status.EQUIP_CLEANED;
            int abandonStatus1 = (int)CMMS.CMMS_Status.EQUIP_ABANDONED;

            string waterUsedUpdate = $"waterUsed ={request.waterUsed},";

            CMMS.CMMS_Modules module;
            int status;

            module = CMMS.CMMS_Modules.MC_EXECUTION;
            status = (int)CMMS.CMMS_Status.MC_TASK_UPDATED;


            // Determine status and module based on moduleType
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
                status = (int)CMMS.CMMS_Status.VEG_TASK_UPDATED;
                waterUsedUpdate = "";
            }



            string scheduleQuery = $"Update cleaning_execution_schedules set {waterUsedUpdate} updatedById={userId},remark_of_schedule='{request.remark}',updatedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where scheduleId = {request.scheduleId};";
            //  $" Update cleaning_execution_items set status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} where scheduleId = {request.scheduleId} ;";

            int val = await Context.ExecuteNonQry<int>(scheduleQuery).ConfigureAwait(false);

            if (request?.cleanedEquipmentIds?.Length > 0)
            {
                string equipIds = (request?.cleanedEquipmentIds?.Length > 0 ? " " + string.Join(" , ", request.cleanedEquipmentIds) + " " : string.Empty);

                string cleanedQuery = $"Update cleaning_execution_items set status = {status1},executionDay={request.cleaningDay},cleanedById={userId},cleanedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds}); ";

                int val2 = await Context.ExecuteNonQry<int>(cleanedQuery).ConfigureAwait(false);
            }

            if (request.abandonedEquipmentIds.Length > 0)
            {
                string equipIds2 = string.Join(" , ", request.abandonedEquipmentIds);

                string abandoneQuery = $"Update cleaning_execution_items set status = {abandonStatus1},executionDay={request.cleaningDay},abandonedById={userId},abandonedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds2}); ";
                int val3 = await Context.ExecuteNonQry<int>(abandoneQuery).ConfigureAwait(false);
            }
            await _utilsRepo.AddHistoryLog(module, request.scheduleId, 0, 0, "schedule Updated", (CMMS.CMMS_Status)status, userId);
            try
            {
                CMMCExecutionSchedule _ViewTaskList = await GetScheduleDetails(request.scheduleId, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewTaskList);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }
            CMDefaultResponse response = new CMDefaultResponse(request.scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Updated Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> AbandonSchedule(CMApproval request, int userId, string facilitytimeZone)
        {
            CMMS.CMMS_Modules module;
            module = CMMS.CMMS_Modules.MC_EXECUTION;

            int status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED;
            int equipstatus = (int)CMMS.CMMS_Status.EQUIP_ABANDONED;
            int notStatus = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;

            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;
                notStatus = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            }

            string Query = $"Update cleaning_execution_schedules set status = {status}, abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,remark = '{request.comment}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where scheduleId = {request.id} ;" +
                 $"Update cleaning_execution_items set status = {equipstatus}, abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}'  where scheduleId = {request.id} and  status NOT IN ( {notStatus} );";


            int val = await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);


            await _utilsRepo.AddHistoryLog(module, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);
            try
            {


                CMMCExecutionSchedule _ViewSchedule = await GetScheduleDetails(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(module, (CMMS.CMMS_Status)status, new[] { userId }, _ViewSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Vegetation Notification: {ex.Message}");
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> LinkPermitToMCVC(int scheduleId, int permit_id, int userId, string facilitytimeZone)
        {
            /*
            * Primary Table - PMSchedule
            * Set the required fields in primary table for linling permit to MC
            * Code goes here
           */
            CMMS.CMMS_Modules module;
            int theStatus;

            module = CMMS.CMMS_Modules.MC_EXECUTION;
            theStatus = (int)CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW;
            if (moduleType == cleaningType.Vegetation)
            {
                module = CMMS.CMMS_Modules.VEGETATION_EXECUTION;
            }

            CMDefaultResponse response;
            string statusQry = $"SELECT  status,ifnull(ptw_id,0) ptw_id FROM cleaning_execution_schedules WHERE  scheduleId= {scheduleId}";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            int ptw_id = Convert.ToInt32(dt1.Rows[0][1]);


            if (ptw_id > 0)
            {
                return new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.FAILURE, "Permit is already Linked to scheduled");
            }
            string permitQuery = "SELECT ptw.id as ptw_id, ptw.code as ptw_code, ptw.title as ptw_title, ptw.status as ptw_status " +
                                    "FROM " +
                                        "permits as ptw " +
                                    $"WHERE ptw.id = {permit_id} ;";
            List<ScheduleLinkedPermit> permit = await Context.GetData<ScheduleLinkedPermit>(permitQuery).ConfigureAwait(false);
            if (permit.Count == 0)
                return new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.FAILURE, $"Permit {permit_id} does not exist.");
            string myQuery = "UPDATE cleaning_execution_schedules  SET " +
                                $"ptw_id = {permit[0].ptw_id}, " +
                                $"status = {(int)CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW}, " +
                                $"updatedAt = '{UtilsRepository.GetUTCTime()}', " +
                                $"updatedById = '{userId}', " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE scheduleId = {scheduleId};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            if (moduleType == cleaningType.ModuleCleaning)
            {
                await _utilsRepo.AddHistoryLog(module, scheduleId, CMMS.CMMS_Modules.PTW, permit_id, "PTW linked to MC", CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW, userId);
            }
            if (moduleType == cleaningType.Vegetation)
            {
                await _utilsRepo.AddHistoryLog(module, scheduleId, CMMS.CMMS_Modules.PTW, permit_id, "PTW linked to VC", CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW, userId);
            }

            try
            {
                CMMCExecutionSchedule _ViewTaskList = await GetScheduleDetails(scheduleId, facilitytimeZone);
                await CMMSNotification.sendNotification(module, CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW, new[] { userId }, _ViewTaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Permit {permit_id} linked to  SCH{scheduleId} Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> CompleteExecution(CMMCExecution request, int userId)
        {
            return null;
        }
    }
}
