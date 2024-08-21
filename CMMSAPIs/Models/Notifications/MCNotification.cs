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


        public MCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan mcPlanObj, CMMCExecution mcTaskObj) : base(moduleID, notificationID)
        {
            planObj = mcPlanObj;
            executionObj = mcTaskObj;
        }

    /*    public MCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecution mcTaskObj) : base(moduleID, notificationID)
        {
            executionObj = mcTaskObj;
        }*/
        /* override protected string getEMSubject(params object[] args)
         {

             string retValue = "My job subject";

             switch (m_notificationID)
             {
                 case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                     retValue += String.Format("Vegetation Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                     break;
                 case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                     retValue += String.Format("Vegetation Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                     break;
                 case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                     retValue += String.Format("Vegetation Plan <{0}> Rejected by {1} ", planObj.planId, planObj.approvedBy);
                     break;
                 case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                     retValue += String.Format("Vegetation Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
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
                     retValue += String.Format("Vegetation Task <{0}> Execution Completed by {1} ", executionObj.id, executionObj.startedBy);
                     break;
                 case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                     retValue += String.Format("Vegetation Task <{0}>  Execution Abandoned by {1} ", executionObj.id, executionObj.abandonedBy);
                     break;
                 default:
                     retValue += String.Format("Vegetation Task <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
                     break;
             }
             retValue += $" for {m_delayDays} days";
             return retValue;

         }*/

        override protected string getEMSubject(params object[] args)
        {

            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                    retValue += String.Format("MC Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue += String.Format("MC Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("MC Plan <{0}> Rejected by {1} ", planObj.planId, planObj.id);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("MC Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("MC Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("MC Plan <{0}> Updated started by {1} ", planObj.planId, planObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                    retValue += String.Format("MC Task <{0}> Scheduled by {1} ", executionObj.executionId, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("MC Task <{0}> Started by {1} ", executionObj.executionId, executionObj.startedAt);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("MC Task <{0}> Ended by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("MC Task <{0}> Completed by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("MC Task <{0}> Abandon by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("MC Task <{0}> Approved started by {1} ", executionObj.executionId, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("MC Task <{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("MC Task <{0}> Updated by {1} ", executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("MC Task <{0}> Abandoned Rejected by {1} ", executionObj.executionId, executionObj.abandonedBy, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("MC Task <{0}> Abandoned Approved by {1} ", executionObj.executionId, executionObj.abandonedBy, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("MC Task <{0}> Deleted by {1} ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("MC Task <{0}> Approved started by {1} ", executionObj.executionId, executionObj.endedBy, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("MC Task <{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue += String.Format("MC Task <{0}> Scheduled Approvved by {1} ", executionObj.executionId, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue += String.Format("MC Task <{0}> Scheduled Rejected by {1} ", executionObj.executionId, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                    retValue += String.Format("MC Task <{0}> Rescheduled by {1} ", executionObj.executionId, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.RESCHEDULED_TASK:
                    retValue += String.Format("MC Task <{0}> Rescheduled Task started by {1} ", executionObj.executionId, executionObj.schedules);
                    break;
                default:
                    retValue += String.Format("MC Task <{0}> Undefined status {1} ", executionObj.id, m_notificationID);
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
                case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                    retValue += String.Format("MC Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue += String.Format("MC Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("MC Plan <{0}> Rejected by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("MC Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("MC Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("MC Task <{0}> Execution started by {1} ", planObj.planId, planObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                    retValue += String.Format("MC Task <{0}> Scheduled by {1} ", executionObj.executionId, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("MC Task <{0}> Started by {1} ", executionObj.executionId, executionObj.startedAt);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("MC Task <{0}> Ended by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("MC Task <{0}> Completed by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("MC Task <{0}> Abandon by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("MC Task <{0}> Approved started by {1} ", executionObj.executionId, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("MC Task <{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("MC Task <{0}> Updated by {1} ", executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("MC Task <{0}> Abandoned Rejected by {1} ", executionObj.executionId, executionObj.abandonedBy, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("MC Task <{0}> Abandoned Approved by {1} ", executionObj.executionId, executionObj.abandonedBy, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("MC Task <{0}> Deleted by {1} ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("MC Task <{0}> Approved started by {1} ", executionObj.executionId, executionObj.endedBy, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("MC Task <{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue += String.Format("MC Task <{0}> Scheduled Approvved by {1} ", executionObj.executionId, executionObj.approvedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue += String.Format("MC Task <{0}> Scheduled Rejected by {1} ", executionObj.executionId, executionObj.rejectedById);
                    break;
                case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                    retValue += String.Format("MC Task <{0}> Rescheduled by {1} ", executionObj.executionId, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.RESCHEDULED_TASK:
                    retValue += String.Format("MC Task <{0}> Rescheduled Task started by {1} ", executionObj.executionId, executionObj.schedules);
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
                retValue += String.Format(template, "MC Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Created By", planObj.createdBy);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", planObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", planObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.MC_PLAN_DELETED:
                        retValue += String.Format(templateEnd, "Deleted By", planObj.deletedBy);
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
                retValue += String.Format(template, "Execution ID", executionObj.planId);
                retValue += String.Format(template, "Status", executionObj.status_short);
                retValue += String.Format(template, "MC Execution Title", executionObj.title);
                retValue += String.Format(template, "Frequency", executionObj.frequency);
                retValue += String.Format(template, "Started By", executionObj.startedBy);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.MC_TASK_STARTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.MC_TASK_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", executionObj.approvedById);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ENDED:
                        retValue += String.Format(templateEnd, "Ended By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                        retValue += String.Format(templateEnd, "Completed By", executionObj.endedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_UPDATED:
                        retValue += String.Format(templateEnd, "Updated By", executionObj.updatedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                        retValue += String.Format(templateEnd, "Abandoned By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", executionObj.rejectedById);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                        retValue += String.Format(templateEnd, "Abandoned By", executionObj.abandonedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", executionObj.approvedById);
                        break;
                    /*case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "Lnked By", executionObj.l);
                        break;*/
                    case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                        retValue += String.Format(templateEnd, "End Approved By", executionObj.approvedById);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", executionObj.rejectedById);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", executionObj.approvedById);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                        retValue += String.Format(templateEnd, "ScheduleRejected By", executionObj.rejectedById);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                        retValue += String.Format(templateEnd, "Updated By", executionObj.schedules);
                        break;
                    case CMMS.CMMS_Status.MC_ASSIGNED:
                        retValue += String.Format(templateEnd, "Assigned to", executionObj.assignedTo);
                        break;
                    case CMMS.CMMS_Status.RESCHEDULED_TASK:
                        retValue += String.Format(templateEnd, "Deleted By", executionObj.schedules);
                        break;
                    default:
                        break;

                }

            }
            return retValue;
        }


    }
}
