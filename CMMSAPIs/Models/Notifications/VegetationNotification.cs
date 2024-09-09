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
                    retValue = String.Format("{0} VE{1}-Created by {2}", planObj.facilityidName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue += String.Format("{0} VE{1} Rejected by {2}", planObj.facilityidName, planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue += String.Format("{0} VE{1} Approved by {2}", planObj.facilityidName, planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                    retValue += String.Format("{0} VE{1} Updated by {2}", planObj.facilityidName, planObj.planId, planObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue += String.Format("{0} VE{1} Deleted by {2}", planObj.facilityidName, planObj.planId, planObj.deletedBy);
                    break;


                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue += String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("{0} VE{1} Started by {2}", executionObj.facilityidName, executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("{0} VE{1} Closed by {2}", executionObj.facilityidName, executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("Vegetation Task <{0}> Closed  by {1} ", executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                    retValue += String.Format("Vegetation Task <{0}>  Linked by {1} ", executionObj.id, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("{0} VE{1} Abandoned by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} VE{1} Abandoned Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} VE{1} Abandoned Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("{0} VE{1} Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("{0} VE{1} Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} VE{1} End Approved by {2}", executionObj.facilityidName, executionObj.id, executionObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue += String.Format("{0} VE{1} Assigned to {2}", executionObj.facilityidName, executionObj.id, executionObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} VE{1} End Rejected by {2}", executionObj.facilityidName, executionObj.id, executionObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} VE{1} Updated by {2}", executionObj.facilityidName, executionObj.id, executionObj.updatedBy);
                    break;


                case CMMS.CMMS_Status.VEG_EXECUTION_STARTED:
                    retValue += String.Format("{0} VE{1} Started by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.startedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_APPROVED:
                    retValue += String.Format("{0} VE{1} Approved by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_UPDATED:
                    retValue += String.Format("{0} VE{1} Updated by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_END_REJECTED:
                    retValue += String.Format("{0} VE{1} Closed Rejected by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_ABANDONED:
                    retValue += String.Format("{0} VE{1} Abandoned by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_EXECUTION_COMPLETED:
                    retValue += String.Format("{0} VE{1} Closed by {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("{0} VE{1} Permit Linked to {2}", scheduleObj.facilityidName, scheduleObj.id, scheduleObj.scheduleId);
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
                    retValue = String.Format("{0} VE{1} Created by {2}", planObj.facilityidName, planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue = String.Format("{0} VE{1} Rejected by {2}", planObj.facilityidName, planObj.planId, planObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue += String.Format("{0} VE{1} Approved by {2}", planObj.facilityidName, planObj.planId, planObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                    retValue += String.Format("{0} VE{1} Updated by {2}", planObj.facilityidName, planObj.planId, planObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue = String.Format("{0} VE{1} Deleted by {2}", planObj.facilityidName, planObj.planId, planObj.deletedbyName);
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
                // Display plan status without extra line breaks
                retValue = String.Format("<h3 style='text-align: center;'><b style='color:#31576D'>Status: </b>{0}</h3>", planObj.status_long + " at " + planObj.facilityidName);

                // Table for plan details
                retValue += "<table style='width: 50%; margin: 20px auto; border-collapse: collapse;' border='1'>";
                retValue += String.Format(template, "ID", "VC" + planObj.planId);
                retValue += String.Format(template, "Status", planObj.status_short);
                retValue += String.Format(template, "VC Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Planning By", planObj.createdBy);
                retValue += String.Format(template, "Site Name", planObj.facilityidName);
                retValue += String.Format(template, "Assigned To", planObj.assignedTo);
                retValue += String.Format(template, "Planning Date and Time", planObj.createdAt);
                retValue += String.Format(template, "Start Date", planObj.startDate);

                // Handle different notification statuses
                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                        retValue += "</table>";
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
                        retValue += String.Format(templateEnd, "Deleted By", planObj.deletedbyName);
                        break;
                    default:
                        retValue += String.Format("VC Task <{0}> Undefined status {1}", executionObj.id, m_notificationID);
                        break;
                }

                // Display schedules if available
                if (planObj.schedules.Count > 0)
                {
                    retValue += "<div style='text-align: center; margin-top: 20px;'><h4>Schedules</h4></div>";

                    // Table for schedules
                    retValue += "<table style='width: 80%; margin: 0 auto; border-collapse: collapse;' border='1'>";
                    retValue += "<tr><th>SR.No</th><th>Cleaning Day</th><th>No. Of SMBs</th><th>No. Of Inverters</th><th>Grass Cutting Area</th></tr>";

                    int i = 0;
                    foreach (var item in planObj.schedules)
                    {
                        retValue += "<tr>";
                        retValue += String.Format("<td>{0}</td>", ++i);
                        retValue += String.Format("<td>{0}</td>", item.cleaningDay);
                        retValue += String.Format("<td>{0}</td>", item.smbs);
                        retValue += String.Format("<td>{0}</td>", item.Invs);
                        retValue += String.Format("<td>{0}</td>", item.area);
                        retValue += "</tr>";
                    }

                    retValue += "</table><br>";
                }
            }

            return retValue;
        }



    }
}
