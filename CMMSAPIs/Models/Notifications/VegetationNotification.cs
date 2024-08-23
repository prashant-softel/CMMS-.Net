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


        public VegetationNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan vegPlanObj, CMMCExecution vegTaskObj) : base(moduleID, notificationID)
        {
            planObj = vegPlanObj;
            executionObj = vegTaskObj;
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
                    retValue += String.Format("Vegetation Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue += String.Format("Vegetation Plan <{0}> Rejected by {1} ", planObj.planId, planObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue += String.Format("Vegetation Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_UPDATED:
                    retValue += String.Format("Vegetation Plan <{0}> Updated by {1} ", planObj.planId, planObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue += String.Format("Vegetation Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue += String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("Vegetation Task <{0}> Execution Completed by {1} ", executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("Vegetation Task <{0}> Ended  by {1} ", executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                    retValue += String.Format("Vegetation Task <{0}>  Linked by {1} ", executionObj.id, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("Vegetation Task <{0}>  Execution Abandoned by {1} ", executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("Vegetation Task Abandoned Rejected <{0}>  by {1} ", executionObj.id, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("Vegetation Task Abandoned Approved <{0}>  by {1} ", executionObj.id, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("Vegetation Task <{0}>  Approved by {1} ", executionObj.id, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("Vegetation Task  Rejected <{0}>  by {1} ", executionObj.id, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("Vegetation Task End Approved <{0}>  by {1} ", executionObj.id, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("Vegetation Task End Rejected <{0}>  by {1} ", executionObj.id, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("Vegetation Task Updated <{0}>  by {1} ", executionObj.id, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue += String.Format("Vegetation Task Assigned <{0}>  to {1} ", executionObj.id, executionObj.assignedTo);
                    break;
                default:
                    retValue += String.Format("Vegetation Task <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    retValue = String.Format("Vegetation Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                    retValue = String.Format("Vegetation Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue = String.Format("Vegetation Plan <{0}> Rejected by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue = String.Format("Vegetation Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue = String.Format("Vegetation Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("Vegetation Task <{0}> Execution Completed by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("Vegetation Task <{0}> Ended  by {1} ", executionObj.id, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("Vegetation Task <{0}>  Execution Abandoned by {1} ", executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("Vegetation Task Rejected <{0}>  by {1} ", executionObj.id, executionObj.abandonedBy, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("Vegetation Task Approved <{0}>  by {1} ", executionObj.id, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("Vegetation Task <{0}>  Approved by {1} ", executionObj.id, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                    retValue += String.Format("Vegetation Task <{0}>  Linked by {1} ", executionObj.id, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("Vegetation Task Rejected <{0}>  by {1} ", executionObj.id, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("Vegetation Task END Approved <{0}>  by {1} ", executionObj.id, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("Vegetation Task END Rejected <{0}>  by {1} ", executionObj.id, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("Vegetation Task Updated <{0}>  by {1} ", executionObj.id, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue += String.Format("Vegetation Task Assigned <{0}>  to {1} ", executionObj.id, executionObj.assignedTo);
                    break;
                default:
                    retValue += String.Format("Vegetation Task <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
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
                retValue += String.Format(template, "ID", planObj.planId);
                retValue += String.Format(template, "Status", planObj.status_short);
                retValue += String.Format(template, "Vegetation Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Created By", planObj.createdBy);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", planObj.rejectedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", planObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                        retValue += String.Format(templateEnd, "Deleted By", planObj.deletedBy);
                        break;
                    default:
                        retValue += String.Format("MC Task <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
                        break;
                }
            }
            else if (executionObj != null && executionObj.executionId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", executionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Execution ID", executionObj.executionId);
                retValue += String.Format(template, "Status", executionObj.status_short);
                retValue += String.Format(template, "Vegetation Plan Title", executionObj.title);
                retValue += String.Format(template, "Frequency", executionObj.frequency);
                retValue += String.Format(template, "started By", executionObj.startedBy);

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.VEG_TASK_STARTED:
                        retValue += String.Format(templateEnd, "Schedule started By", executionObj.startedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                        retValue += String.Format(templateEnd, "Schedule Completed By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ENDED:
                        retValue += String.Format(templateEnd, "Schedule Ended By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                        retValue += String.Format(template, "Abandoned By", executionObj.abandonedBy);
                        retValue += String.Format(templateEnd, "Reason For Abandon", executionObj.status_short);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                        retValue += String.Format(templateEnd, "Schedule Abandoned  By", executionObj.rejectedById);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                        retValue += String.Format(templateEnd, "Schedule Abandoned Approved By", executionObj.approvedById);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                        retValue += String.Format(templateEnd, "Schedule Approved By", executionObj.approvedById);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                        retValue += String.Format(templateEnd, "Schedule Rejected By", executionObj.rejectedById);
                        break;
                    case CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "Vegetation Linked  By", executionObj.schedules);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                        retValue += String.Format(templateEnd, "Schedule End Approved By", executionObj.approvedById);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                        retValue += String.Format(templateEnd, "Schedule End Rejected By", executionObj.rejectedById);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                        retValue += String.Format(templateEnd, "Schedule Ended Updated By", executionObj.updatedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                        retValue += String.Format(templateEnd, "Schedule Assigned to", executionObj.assignedTo);
                        break;
                }
            }
            return retValue;
        }


    }
}
