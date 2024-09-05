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
                case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                    retValue += String.Format("MCP<{0}> Drafted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Created by {2}", planObj.facilityidbyName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Rejected by {2}", planObj.facilityidbyName, planObj.planId, planObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Approved by {2}", planObj.facilityidbyName, planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Deleted", planObj.facilityidbyName, planObj.planId);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Updated by {2}", planObj.facilityidbyName, planObj.planId, planObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Started by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Closed by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Closed by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Abandoned by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.abandonedBy); 
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Approved by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Rejected by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Updated by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Abandoned Rejected by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Abandoned Approved by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("Permit ID<{0}> Linked With Schedule ID{1} ", scheduleObj.permit_id, scheduleObj.scheduleId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-End Approved by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-End Rejected by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Approved by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue += String.Format("{0} Module-Cleaning{1}-Rejected by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_STARTED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Started by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.createdbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_CLOSED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Closed by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_UPDATED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Updated by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_ABANDONED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Abandoned Approved by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_ABANDONED_REJECTED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Abandoned Rejected by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.MC_ASSIGNED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Assigned to {2}", executionObj.facilityidbyName, executionObj.executionId, executionObj.assignedTo);
                    break;
                default:
                    retValue += String.Format("MCT<{0}> Undefined status {1} ", executionObj.id, m_notificationID);
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
                    retValue = String.Format("{0} Module-Cleaning{1}-Created by {2}", planObj.facilityidbyName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Rejected by {2}", planObj.facilityidbyName, planObj.planId, planObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Approved by {2}", planObj.facilityidbyName, planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Deleted", planObj.facilityidbyName, planObj.planId);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Updated by {2}", planObj.facilityidbyName, planObj.planId, planObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Started by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Closed by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Closed by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Abandoned by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Approved by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Rejected by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Updated by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Abandoned Rejected by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Abandoned Approved by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("Permit ID<{0}> Linked With Schedule ID{1} ", scheduleObj.permit_id, scheduleObj.scheduleId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-End Approved by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("{0} Module-Cleaning{1}-End Rejected by {2}", planObj.facilityidbyName, executionObj.executionId, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue += String.Format("{0} Module-Cleaning{1}-Approved by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue += String.Format("{0} Module-Cleaning{1}-Rejected by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_STARTED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Started by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.createdbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_CLOSED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Closed by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_UPDATED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Updated by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_ABANDONED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Abandoned Approved by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.MC_EXECUTION_ABANDONED_REJECTED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Abandoned Rejected by {2}", scheduleObj.facilityidbyName, scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.MC_ASSIGNED:
                    retValue = String.Format("{0} Module-Cleaning{1}-Assigned to {2}", executionObj.facilityidbyName, executionObj.executionId, executionObj.assignedTo);
                    break;
            }
            return retValue;

        }



        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            if (planObj != null && planObj.planId != 0)
            {

                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", planObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "ID", "MCP" + planObj.planId);
                retValue += String.Format(template, "Status", planObj.status_short);
                retValue += String.Format(template, "MC Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Created By", planObj.createdBy);
                retValue += String.Format(template, "Assigned To", planObj.assignedTo);
                retValue += "</table><br>";

                if (planObj.schedules.Count > 0) 
                {
                    retValue += "<h4>Selected Material</h4>";
                    retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
                    retValue += "<tr>";
                    retValue += "<th>SR.No</th>";
                    retValue += "<th>Cleaning Day</th>";
                    retValue += "<th>No. Of Inverters</th>";
                    retValue += "<th>No. Of SMBs</th>";
                    retValue += "<th>No.of Modules</th>";
                    retValue += "<th>cleaningTypeName</th>";
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


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", planObj.rejectedBy);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", planObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_DELETED:
                        retValue += String.Format(templateEnd, "MC Plan Deleted ", planObj.planId);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                        retValue += String.Format(templateEnd, "Updated By", planObj.updatedBy);
                        break;
                    default:
                        break;
                }
            }
            else if (executionObj != null && executionObj.executionId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", executionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Execution ID", "MCT" + executionObj.id);
                retValue += String.Format(template, "Status", executionObj.status_short);
                retValue += String.Format(template, "MC Execution Title", executionObj.title);
                retValue += String.Format(template, "Frequency", executionObj.frequency);
                retValue += String.Format(template, "Started By", executionObj.startedBy);
                retValue += String.Format(template, "Started At", executionObj.startDate);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.MC_TASK_STARTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.MC_TASK_APPROVED:
                        retValue += String.Format(templateEnd, "MC Task Schedule Approved By", executionObj.approvedBy);
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
                        retValue += String.Format(templateEnd, "MC Task Scheduled Rejected By", executionObj.rejectedBy);
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
                    case CMMS.CMMS_Status.MC_ASSIGNED:
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
                retValue += String.Format(template, "Started By", scheduleObj.start_date);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                        retValue += "</table>"; break;

                    case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                         retValue += String.Format(templateEnd, "Permit ID Linked to Schedule ID ", scheduleObj.scheduleId);
                         break;
                    case CMMS.CMMS_Status.MC_EXECUTION_STARTED:
                        retValue = String.Format(templateEnd, "MC Execution Started By ", scheduleObj.executedBy);
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
