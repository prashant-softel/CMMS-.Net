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
                    retValue += String.Format("MCP<{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("MCP<{0}> Rejected by {1} ", planObj.planId, planObj.id);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("MCP<{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("MCP<{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("MCP<{0}> Updated by {1} ", planObj.planId, planObj.updatedBy);
                    break;
 /*               case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                    retValue += String.Format("MCT<{0}> Scheduled by {1} ", executionObj.executionId, executionObj.schedules);
                    break;*/
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("MCT<{0}> Started by {1} ", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("MCT<{0}> Ended by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("MCT<{0}> Completed by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("MCT<{0}> Abandoned by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("MCT<{0}> Approved  by {1} ", executionObj.executionId, executionObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("MCT<{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("MCT<{0}> Updated by {1} ", executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("MCT<{0}> Abandoned Rejected by {1} ", executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("MCT<{0}> Abandoned Approved by {1} ", executionObj.executionId, executionObj);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("Permit ID<{0}> Linked With Schedule ID{1} ", scheduleObj.permit_id, scheduleObj.scheduleId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("MCT<{0}> Approved started by {1} ", executionObj.executionId, executionObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("MCT<{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue += String.Format("MCT<{0}> Scheduled Approvved by {1} ");
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue += String.Format("MCT<{0}> Scheduled Rejected by {1} ");
                    break;
                case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                    retValue += String.Format("MCT<{0}> Rescheduled by {1} " );
                    break;
                case CMMS.CMMS_Status.RESCHEDULED_TASK:
                    retValue += String.Format("MCT<{0}> Rescheduled Task started by {1} ");
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
                    retValue += String.Format("MCP<{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue += String.Format("MCP<{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue += String.Format("MCP<{0}> Rejected by {1} ", planObj.planId, planObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue += String.Format("MCP<{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue += String.Format("MCP<{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_UPDATED:
                    retValue += String.Format("MCT<{0}> Execution started by {1} ", planObj.planId, planObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                    retValue += String.Format("MCT<{0}> Scheduled by {1} ", executionObj.executionId, executionObj.schedules);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("MCT<{0}> Started by {1} ", executionObj.executionId, executionObj.startedAt);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("MCT<{0}> Ended by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("MCT<{0}> Completed by {1} ", executionObj.executionId, executionObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("MCT<{0}> Abandoned by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("MCT<{0}> Approved  by {1} ", executionObj.executionId, executionObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("MCT<{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("MCT<{0}> Updated by {1} ", executionObj.executionId, executionObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("MCT<{0}> Abandoned Rejected by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("MCT<{0}> Abandoned Approved by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("PermitId<{0}> Linked to Schedule ID {1} ", scheduleObj.permit_id, scheduleObj.scheduleId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("MCT<{0}> Approved started by {1} ", executionObj.executionId, executionObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("MCT<{0}> Rejected by {1} ", executionObj.executionId, executionObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                    retValue += String.Format("MCT<{0}> Scheduled Approved by {1} ");
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue += String.Format("MCT<{0}> Scheduled Rejected by {1} ");
                    break;
                case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                    retValue += String.Format("MCT<{0}> Rescheduled by {1} ");
                    break;
                case CMMS.CMMS_Status.RESCHEDULED_TASK:
                    retValue += String.Format("MCT<{0}> Rescheduled  by {1} ");
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
                retValue += String.Format(template, "Execution ID", "MCT" + executionObj.id);
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
                   /* case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "MC Task Schedule Linked By", executionObj.schedules);
                        break;*/
                    case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                        retValue += String.Format(templateEnd, "MC Task Schedule End Approved By", executionObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                        retValue += String.Format(templateEnd, "MC Task Schedule End Rejected By", executionObj.rejectedBy);
                        break;
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED:
                        retValue += String.Format(templateEnd, "MC Task Schedule Approved By");
                        break;
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                        retValue += String.Format(templateEnd, "MC Task Schedule Rejected By");
                        break;
                    case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                        retValue += String.Format(templateEnd, "MC Task Rescheduled By");
                        break;
                    case CMMS.CMMS_Status.MC_ASSIGNED:
                        retValue += String.Format(templateEnd, "MC Task Schedule Assigned to", executionObj.assignedTo);
                        break;
                    case CMMS.CMMS_Status.RESCHEDULED_TASK:
                        retValue += String.Format(templateEnd, "MC Task Rescheduled  by");
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
                // += String.Format(template, "MC Execution Title", scheduleObj.);
               // retValue += String.Format(template, "Frequency", scheduleObj.frequency);
                retValue += String.Format(template, "Started By", scheduleObj.start_date);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                   /* case CMMS.CMMS_Status.MC_TASK_STARTED:
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
                        break;*/
                    case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                         retValue += String.Format(templateEnd, "Permit ID Linked to Schedule ID", scheduleObj.scheduleId);
                         break;
                    /*case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "Permit ID Linked to Schedule ID", scheduleObj.scheduleId);
                        break;*/
                        
                    /*case CMMS.CMMS_Status.RESCHEDULED_TASK:
                        retValue += String.Format(templateEnd, "MC Task Rescheduled  by");
                        break;*/
                    default:
                        break;

                }

            }
            return retValue;
        }


    }
}
