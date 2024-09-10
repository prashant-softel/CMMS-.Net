using System;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;

namespace CMMSAPIs.Models.Notifications
{
    internal class MCNotification : CMMSNotification
    {
        int VegId;
        CMMCPlan planObj;
        CMMCExecution executionObj;
        CMMCExecutionSchedule scheduleObj;


        public MCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan mcPlanObj) : base(moduleID, notificationID)
        {
            planObj = mcPlanObj;

        }

        public MCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecution mcTaskObj) : base(moduleID, notificationID)
        {
            executionObj = mcTaskObj;
        }

        public MCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecutionSchedule mcScheduleObj) : base(moduleID, notificationID)
        {
            scheduleObj = mcScheduleObj;
        }



        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue += String.Format("{0} MC{1} Created by {2}", planObj.facilityidName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("{0} MC{1} Rejected by {2}", planObj.facilityidName, planObj.planId, planObj.rejectedbyName);
                    break;
                default:
                    retValue += String.Format("MC<{0}> Undefined status {1} ", planObj.id, m_notificationID);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }

        override protected string getSubject(params object[] args)
        {

            string retValue = "";


            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                    retValue += String.Format("MCP<{0}> Drafted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue += String.Format("{0} MC{1} Created by {2}", planObj.facilityidName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("{0} MC{1} Rejected by {2}", planObj.facilityidName, planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("{0} MC{1} Approved by {2}", planObj.facilityidName, planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("{0} MC{1} Deleted by {2}", planObj.facilityidName, planObj.planId, planObj.deletedbyName);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("{0} MC{1} Updated by {2}", planObj.facilityidName, planObj.planId, planObj.updatedbyName);
                    break;

                /*             case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                                 retValue += String.Format("{0} PTW{1} Linked With SCH{2} of MCT{3}", scheduleObj.facilityidName, scheduleObj.permit_id, scheduleObj.scheduleId, scheduleObj.executionId);
                                 break;*/

                default:
                    retValue += String.Format("MC<{0}> Undefined status {1} ", planObj.id, m_notificationID);
                    break;
            }
            return retValue;

        }



        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            if (planObj != null && planObj.planId != 0)
            {
                // Wrap the status and table in a div to control layout
                retValue = "<div style='width: 100%; text-align: center; margin-bottom: 20px;'>";

                // Status on top
                retValue += String.Format("<h3><b style='color:#31576D'>Status:</b> {0}</h3>", planObj.status_long + " at " + planObj.facilityidName);

                // Close div to ensure the status is on top
                retValue += "</div>";

                // Table for plan details
                retValue += "<div style='width: 100%;'>";
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
                retValue += String.Format(template, "ID", "MCP" + planObj.planId);
                retValue += String.Format(template, "Status", planObj.status_short);
                retValue += String.Format(template, "MC Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Planning By", planObj.createdBy);
                retValue += String.Format(template, "Site Name", planObj.facilityidName);
                retValue += String.Format(template, "Assigned To", planObj.assignedTo);
                retValue += String.Format(template, "Planning Date and Time ", planObj.createdAt);
                retValue += String.Format(template, "Start Date ", planObj.startDate);

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                        retValue += "</table>";
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                        retValue += "</table>";
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", planObj.approvedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", planObj.approvedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_DELETED:
                        retValue += String.Format(templateEnd, "Deleted By", planObj.deletedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                        retValue += String.Format(templateEnd, "Updated By", planObj.updatedbyName);
                        break;
                    default:
                        break;
                }

                if (planObj.schedules.Count > 0)
                {
                    // Schedule section
                    retValue += "<div style='text-align: center; margin-top: 20px;'>";
                    retValue += "<h4>Scheduled</h4>";
                    retValue += "</div>";
                    retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 0px;' border='1'>";
                    retValue += "<tr>";
                    retValue += "<th>SR.No</th>";
                    retValue += "<th>Cleaning Day</th>";
                    retValue += "<th>No. Of Inverters</th>";
                    retValue += "<th>No. Of SMBs</th>";
                    retValue += "<th>No.of Modules</th>";
                    retValue += "<th>Type</th>";

                    int i = 0;
                    foreach (var item in planObj.schedules)
                    {
                        retValue += "<tr>";
                        retValue += String.Format("<td>{0}</td>", ++i);
                        retValue += String.Format("<td>{0}</td>", item.cleaningDay);
                        retValue += String.Format("<td>{0}</td>", item.Invs);
                        retValue += String.Format("<td>{0}</td>", item.smbs);
                        retValue += String.Format("<td>{0}</td>", item.ScheduledModules);
                        retValue += String.Format("<td>{0}</td>", item.cleaningTypeName);
                    }
                    retValue += "</table><br><br>";
                }

                retValue += "</div>";  // Close the wrapping div for layout
            }

        
    
            else if (executionObj != null && executionObj.executionId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", executionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Execution ID", "MCT" + executionObj.id);
                retValue += String.Format(template, "Status", executionObj.status_short);
                retValue += String.Format(template, "MC Execution Title", executionObj.title);
                retValue += String.Format(template, "Frequency", executionObj.frequency);
                retValue += String.Format(template, "Scheduled At", executionObj.scheduledDate);
                if (executionObj.startedById > 0)
                {
                    retValue += String.Format(template, "Started By", executionObj.startedBy + " at " + executionObj.startedAt);
                }
                if (executionObj.endedById > 0)
                {
                    retValue += String.Format(template, "Ended By", executionObj.endedBy + " at " + executionObj.endedAt);
                }

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.MC_TASK_STARTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.MC_TASK_APPROVED:
                        retValue += String.Format(templateEnd, "MC Task Schedule Approved By", executionObj.approvedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ENDED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Ended By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Completed By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_UPDATED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Updated By", executionObj.updatedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Abandoned By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_REJECTED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Rejected By", executionObj.rejectedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Abandoned Rejected By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                        retValue += String.Format(templateEnd, "MC Task Scheduled Abandoned Approved By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                        retValue += String.Format(templateEnd, "MC Task Schedule End Approved By", executionObj.endapprovedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                        retValue += String.Format(templateEnd, "MC Task Schedule End Rejected By", executionObj.endrejectedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ASSIGNED:
                        retValue += String.Format(templateEnd, "MC Task Schedule Assigned to", executionObj.assignedTo);
                        break;
                    default:
                        break;

                }

            }
            else if (scheduleObj != null && scheduleObj.scheduleId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", scheduleObj.status_long_schedule);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Schedule ID", "MCS" + scheduleObj.id);
                retValue += String.Format(template, "Status", scheduleObj.status_short);
                retValue += String.Format(template, "MC Schedule Title", scheduleObj.title);
                retValue += String.Format(template, "Description", scheduleObj.description);
                retValue += String.Format(template, "Started By", scheduleObj.startedbyName);
                retValue += String.Format(template, "Started At", scheduleObj.start_date);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                        retValue += "</table>"; break;

                    case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                         retValue += String.Format(templateEnd, "Permit ID Linked to Schedule ID ", scheduleObj.scheduleId);
                         break;
                    case CMMS.CMMS_Status.MC_EXECUTION_STARTED:
                        retValue = String.Format(templateEnd, "MC Execution Started By ", scheduleObj.startedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_EXECUTION_CLOSED:
                        retValue = String.Format(templateEnd, "MC Execution Ended By ", scheduleObj.endedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_EXECUTION_UPDATED:
                        retValue = String.Format(templateEnd, "MC Execution Updated By ", scheduleObj.updatedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_EXECUTION_ABANDONED:
                        retValue = String.Format(templateEnd, "MC Execution Abandoned Approved By ", scheduleObj.abandonedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_EXECUTION_ABANDONED_REJECTED:
                        retValue = String.Format(templateEnd, "MC Execution Abandoned Rejected By ", scheduleObj.abandonedbyName);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                        retValue += String.Format(templateEnd, "MC Execution Approved By ", scheduleObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                        retValue += String.Format(templateEnd, "MC Execution Approved By ", scheduleObj.rejectedBy);
                        break;
                    default:
                        break;

                }

            }
            return retValue;
        }


    }
}
