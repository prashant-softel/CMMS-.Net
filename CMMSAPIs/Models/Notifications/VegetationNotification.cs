using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using System;

namespace CMMSAPIs.Models.Notifications
{
    internal class VegetationNotification : CMMSNotification
    {
        int VegId;
        CMMCPlan planObj;
        CMMCExecution executionObj;
        CMMCExecutionSchedule scheduleObj;

        public VegetationNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan vegPlanObj) : base(moduleID, notificationID)
        {
            planObj = vegPlanObj;
           
        }

        public VegetationNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecution vegTaskObj) : base(moduleID, notificationID)
        {
           
            executionObj = vegTaskObj;
        }
        public VegetationNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecutionSchedule vegScheduleObj) : base(moduleID, notificationID)
        {
            scheduleObj = vegScheduleObj;
        }




        override protected string getEMSubject(params object[] args)
        {
            int plan_id = planObj.planId;
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    retValue += String.Format("Vegetation Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                    retValue = String.Format("{0} Vegetation{1}-Created by {2}", planObj.facilityidName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Rejected by {2}", planObj.facilityidName, planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Approved by {2}", planObj.facilityidName, planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                    retValue += String.Format("{0} Vegetation{1}-Updated by {2}", planObj.facilityidName, planObj.planId, planObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue += String.Format("{0} Vegetation{1}-Deleted by {2}", planObj.facilityidName, planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue += String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("{0} Vegetation{1}-Started by {2}", executionObj.facilityidName, executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("{0} Vegetation{1}-Closed by {2}", executionObj.facilityidName, executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("Vegetation Task <{0}> Closed  by {1} ", executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                    retValue += String.Format("Vegetation Task <{0}>  Linked by {1} ", executionObj.id, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-End Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue += String.Format("{0} Vegetation{1}-Assigned to {2}", executionObj.facilityidName, executionObj.id, executionObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-End Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} Vegetation{1}-Updated by {2}", executionObj.facilityidName, executionObj.id, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_STARTED:
                    retValue += String.Format("{0} Vegetation{1}-Started by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.startedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Approved by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_UPDATED:
                    retValue += String.Format("{0} Vegetation{1}-Updated by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_END_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Closed Rejected by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_ABANDONED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_COMPLETED:
                    retValue += String.Format("{0} Vegetation{1}-Closed by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("{0} Vegetation{1}-Permit Linked to {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.scheduleId);
                    break;

                    







                default:
                    retValue += String.Format("Vegetation  <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
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
                case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    retValue = String.Format("Vegetation Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                    retValue = String.Format("{0} Vegetation{1}-Created by {2}", planObj.facilityidName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue = String.Format("{0} Vegetation{1}-Rejected by {2}", planObj.facilityidName, planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Approved by {2}", planObj.facilityidName, planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                    retValue += String.Format("{0} Vegetation{1}-Updated by {2}", planObj.facilityidName, planObj.planId, planObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue = String.Format("{0} Vegetation{1}-Deleted by {2}", planObj.facilityidName, planObj.planId);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("{0} Vegetation{1}-Started by {2}", executionObj.facilityidName, executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("{0} Vegetation{1}-Closed by {2}", executionObj.facilityidName, executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("{0} Vegetation{1}-Closed by {2}", executionObj.facilityidName, executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("{0} Vegetation{1}-Abandoned by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                    retValue += String.Format("Vegetation Task <{0}>  Linked by {1} ", executionObj.id, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue += String.Format("{0} Vegetation{1}-Assigned to {2}", executionObj.facilityidName, executionObj.id, executionObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-End Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-End Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} Vegetation{1}-Updated by {2}", executionObj.facilityidName, executionObj.id, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("{0} Vegetation{1}-Permit Linked to {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.scheduleId);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_STARTED:
                    retValue += String.Format("{0} Vegetation{1}-Started by {2}", scheduleObj.facilityidName, executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_APPROVED:
                    retValue += String.Format("{0} Vegetation{1}-Approved by {2}", scheduleObj.facilityidName, executionObj.id, executionObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_UPDATED:
                    retValue += String.Format("{0} Vegetation{1}-Updated by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_END_REJECTED:
                    retValue += String.Format("{0} Vegetation{1}-Closed Rejected by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_ABANDONED:
                    retValue += String.Format("{0} Vegetation{1}-Abandoned  by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_COMPLETED:
                    retValue += String.Format("{0} Vegetation{1}-Closed  by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.endedbyName);
                    break;

                    
                default:
                    if (planObj != null && planObj.planId != 0)
                    {
                        retValue += String.Format("Vegetation Plan <{0}> Undefined status {1} ", planObj.planId, m_notificationID);
                    }
                    else if (executionObj != null && executionObj.executionId != 0)
                    {
                        retValue += String.Format("Vegetation Task <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
                    }
                    else if (scheduleObj != null && scheduleObj.scheduleId != 0)
                    {
                        retValue += String.Format("Vegetation Schedule <{0}> Undefined status {1} ", scheduleObj.scheduleId, m_notificationID);
                    }

                        break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            if (planObj != null && planObj.planId != 0)
            {
                // Display plan status
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", planObj.status_long);

                // Table for plan details
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
                retValue += String.Format(template, "VC ID", planObj.planId);
                retValue += String.Format(template, "Status", planObj.status_short);
                retValue += String.Format(template, "Vegetation Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Created By", planObj.createdBy);

                // Switch case for different notification IDs
                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                        // Close the table when no additional rows are needed
                        retValue += "</table><br>";
                        break;

                    case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", planObj.rejectedbyName);
                        break;

                    case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", planObj.approvedbyName);
                        break;

                    case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                        retValue += String.Format(templateEnd, "Updated By", planObj.updatedbyName);
                        break;

                    case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                        retValue += String.Format(templateEnd, "Deleted By", planObj.deletedBy);
                        break;

                    default:
                        retValue += String.Format("MC Task <{0}> Undefined status {1}", executionObj.id, m_notificationID);
                        break;
                }

                // Display schedules if available
                if (planObj.schedules.Count > 0)
                {
                    // Display the 'Selected Material' header centered
                    retValue += "<div style='text-align: center; margin-top: 20px;'><h4>Schedules</h4></div>";

                    // Table for schedules
                    retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
                    retValue += "<tr>";
                    retValue += "<th>SR.No</th>";
                    retValue += "<th>Cleaning Day</th>";
                    retValue += "<th>No. Of SMBs</th>";
                    retValue += "<th>No. Of Inverters</th>";
                    retValue += "<th>Grass Cutting Area</th>";
                    retValue += "</tr>";

                    int i = 0;
                    foreach (var item in planObj.schedules)
                    {
                        retValue += "<tr>";
                        retValue += String.Format("<td>{0}</td>", ++i);
                        retValue += String.Format("<td>{0}</td>", item.cleaningDay);
                        retValue += String.Format("<td>{0}</td>", item.smbs);
                        retValue += String.Format("<td>{0}</td>", item.Invs);
                        retValue += String.Format("<td>{0}</td>", item.area);
                        retValue += "</tr>";  // Ensure each row is properly closed
                    }

                    retValue += "</table><br><br>";
                }
            }



            else if (executionObj != null && executionObj.executionId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", executionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Execution ID", executionObj.executionId);
                retValue += String.Format(template, "Status", executionObj.status_short);
                retValue += String.Format(template, "Vegetation Execution Title", executionObj.title);
                retValue += String.Format(template, "Frequency", executionObj.frequency);
                retValue += String.Format(template, "Started By", executionObj.startedBy);

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.VEG_TASK_STARTED:
                        retValue += String.Format(templateEnd, "Vegetation Task started By", executionObj.startedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                        retValue += String.Format(templateEnd, "Vegetation Task Completed By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ENDED:
                        retValue += String.Format(templateEnd, "Schedule Ended By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                        retValue += String.Format(template, "Abandoned By ", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                        retValue += String.Format(templateEnd, "Vegetation Task Abandoned Rejected  By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                        retValue += String.Format(templateEnd, "Vegetation Task Abandoned Rejected  By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                        retValue += String.Format(templateEnd, "Vegetation Task Approved By", executionObj.approvedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                        retValue += String.Format(templateEnd, "Vegetation Task Rejected By", executionObj.rejectedbyName);
                        break;
                    case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "Vegetation Linked  By", executionObj.schedules);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                        retValue += String.Format(templateEnd, "Vegetation Task End Approved By", executionObj.endapprovedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                        retValue += String.Format(templateEnd, "Vegetation Task End Rejected By", executionObj.endrejectedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                        retValue += String.Format(templateEnd, "Vegetation Task Updated By", executionObj.updatedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                        retValue += String.Format(templateEnd, "Vegetation Task Assigned to", executionObj.assignedTo);
                        break;

                }
            }
            else if (scheduleObj != null && scheduleObj.scheduleId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", scheduleObj.status_long_schedule);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Schedule ID", "MCS" + scheduleObj.id);
                retValue += String.Format(template, "Status", scheduleObj.status_short);
                retValue += String.Format(template, "Vegetation Schedule Title", scheduleObj.title);
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
                    case CMMS.CMMS_Status.VEG_EXECUTION_STARTED:
                        retValue = String.Format(templateEnd, "Vegetation Execution Started By ", scheduleObj.startedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_EXECUTION_APPROVED:
                        retValue = String.Format(templateEnd, "Vegetation Execution Approved By ", scheduleObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_EXECUTION_UPDATED:
                        retValue = String.Format(templateEnd, "Vegetation Execution Updated By ", scheduleObj.updatedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_EXECUTION_END_REJECTED:
                        retValue = String.Format(templateEnd, "Vegetation Execution Closed Rejected By ", scheduleObj.rejectedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_EXECUTION_ABANDONED:
                        retValue = String.Format(templateEnd, "Vegetation Execution Abandoned By ", scheduleObj.abandonedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_EXECUTION_COMPLETED:
                        retValue = String.Format(templateEnd, "Vegetation Execution Ended By ", scheduleObj.endedbyName);
                        break;

                    default:
                        break;

                }

            }
            return retValue;
        }


    }
}
