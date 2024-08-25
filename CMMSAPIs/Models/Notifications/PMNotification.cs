using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class PMNotification : CMMSNotification
    {
        int m_PMId;
        CMPMScheduleViewDetail m_pmscheduleObj;
        CMPMPlanDetail m_pmPlanObj;
        CMPMTaskView m_pmExecutionObj;

        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMTaskView executionObj) : base(moduleID, notificationID)
        {
            m_pmExecutionObj = executionObj;
            m_PMId = m_pmExecutionObj.id;
        }

        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMPlanDetail pmObj) : base(moduleID, notificationID)
        {
            m_pmPlanObj = pmObj;
            m_PMId = m_pmPlanObj.plan_id;
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "Subject";
            if (m_pmPlanObj != null)
            {
                m_PMId = m_pmPlanObj.plan_id;
            }
           

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_START:     //Created                  
                    retValue = String.Format("Preventive Order{0}> Task  Started <{1}> By", m_pmPlanObj.task_id, m_pmPlanObj.started_by_name);
                    break;
                case CMMS.CMMS_Status.PM_UPDATED:     //Assigned
                    retValue = String.Format("Preventive Order <{0}> Task Updated <{1}> By", m_pmExecutionObj.id, m_pmscheduleObj.updated_by);
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:     //Linked
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Submitted By", m_pmExecutionObj.id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_LINK_PTW:     //Closed
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Linked to PTW <{2}>", m_pmExecutionObj.id, m_pmscheduleObj.equipment_name, m_pmscheduleObj.permit_id);
                    break;
                case CMMS.CMMS_Status.PM_COMPLETED:     //Cancelled
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Completed By", m_pmExecutionObj.id);
                    break;
                case CMMS.CMMS_Status.PM_APPROVED:     //Closed
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Approved", m_pmExecutionObj.id, m_pmscheduleObj.equipment_name);
                    break;
                case CMMS.CMMS_Status.PM_REJECTED:     //Cancelled
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Rejected", m_pmExecutionObj.id, m_pmPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED:     //Closed
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Cancelled By", m_pmExecutionObj.id, m_pmExecutionObj.cancelled_by_name);
                    break;
                case CMMS.CMMS_Status.PM_DELETED:     //Cancelled
                    retValue = String.Format("Preventive Order <{0}> Task <{1}> Deleted", m_pmExecutionObj.id, m_pmscheduleObj.equipment_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_CREATED:     //Cancelled
                    retValue = String.Format("PM Plan <{0}>  <{1}> Created By", m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DRAFT:     //Cancelled
                    retValue = String.Format("PM  <{0}> Task  Drafted By <{1}>", m_pmExecutionObj.id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                    retValue = String.Format("PM Plan <{0}> Updated By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    retValue = String.Format("PM Plan <{0}> Deleted By <{1}> ", m_pmExecutionObj.id);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = String.Format("Preventive Order <{0}> Approved By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = String.Format("Preventive Order <{0}> Rejected By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
                    retValue = String.Format("PM  <{0}> Task  Drafted By <{1}>", m_pmExecutionObj.id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = String.Format("PM Plan <{0}> Updated By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                    retValue = String.Format("PM Plan <{0}> Deleted By <{1}> ", m_pmExecutionObj.id);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                    retValue = String.Format("Preventive Order <{0}> Approved By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                    retValue = String.Format("Preventive Order <{0}> Rejected By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.approved_close_by_name);
                    break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = String.Format("Preventive Order <{0}> Rejected By <{1}> ", m_pmExecutionObj.id);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            if (m_pmPlanObj != null && m_pmPlanObj.plan_id != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_pmPlanObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Plan ID", m_pmPlanObj.plan_id);
                retValue += String.Format(template, "Status", m_pmPlanObj.started_by_name);
                retValue += String.Format(template, "Plan Name", m_pmPlanObj.plan_name);
                retValue += String.Format(template, "Responsible Person", m_pmPlanObj.assigned_to_name);
                // retValue += String.Format(template, "Check List", m_pmPlanObj.name);
                retValue += String.Format(template, "Frequency", m_pmPlanObj.plan_freq_name);
                //retValue += String.Format(template, "Last Done Date", m_pmPlanObj.);
                //retValue += String.Format(template, "Scheduled Date", m_pmPlanObj.schedule_date);

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_PLAN_DRAFT:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.PM_PLAN_CREATED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                        retValue += String.Format(templateEnd, "PM Plan Updated", m_pmPlanObj.updated_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "PM Plan Approved", m_pmPlanObj.approved_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "PM Plan Rejected", m_pmPlanObj.rejected_by_name);
                        break;
                    default:
                        break;

                }

            }
            else if (m_pmExecutionObj != null && m_pmExecutionObj.id != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_pmExecutionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Task ID", m_pmExecutionObj.id);
                retValue += String.Format(template, "Status", m_pmExecutionObj.status);
               // retValue += String.Format(template, "Task Name", m_pmExecutionObj.);
                retValue += String.Format(template, "Responsible Person", m_pmExecutionObj.assigned_to_name);
                retValue += String.Format(template, "Started By", m_pmExecutionObj.started_by_name);
                retValue += String.Format(template, "Frequency", m_pmExecutionObj.frequency_name);
                //retValue += String.Format(template, "Last Done Date", m_pmPlanObj.);
                //retValue += String.Format(template, "Scheduled Date", m_pmPlanObj.schedule_date);

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.PM_ASSIGNED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "PM Task Linked By", m_pmExecutionObj.updated_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_LINK_PTW:
                        retValue += String.Format(templateEnd, "PM Task Linked", m_pmExecutionObj.ptw_tbt_done);
                        break;
                    case CMMS.CMMS_Status.PM_START:
                        retValue += String.Format(templateEnd, "PM Task Started By", m_pmExecutionObj.started_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_COMPLETED:
                        retValue += String.Format(templateEnd, "PM Task Completed By", m_pmExecutionObj.closed_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Rejected By", m_pmExecutionObj.rejected_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Approved By", m_pmExecutionObj.approved_by);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Closed Rejected By", m_pmExecutionObj.rejected_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Close Approved By", m_pmExecutionObj.approved_by);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled By", m_pmExecutionObj.cancelledbyName);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled Rejected By", m_pmExecutionObj.cancelledrejectedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled Approved", m_pmExecutionObj.rejected_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_DELETED:
                        retValue += String.Format(templateEnd, "PM Task Deleted", m_pmExecutionObj.rejected_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_UPDATED:
                        retValue += String.Format(templateEnd, "PM Task Updated", m_pmExecutionObj.rejected_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_SUBMIT:
                        retValue += String.Format(templateEnd, "PM Task Submited", m_pmExecutionObj.rejected_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_TASK_DELETED:
                        retValue += String.Format(templateEnd, "PM Task Close Approved", m_pmExecutionObj.rejected_by_name);
                        break;
                    default:
                        break;

                }


            }

            return retValue;
        }


    }


          /*  retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_pmscheduleObj.status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", m_pmscheduleObj.id);
            retValue += String.Format(template, "Status", m_pmscheduleObj.status_short);
            retValue += String.Format(template, "Equipment Name", m_pmscheduleObj.equipment_name);
            retValue += String.Format(template, "Responsible Person", m_pmscheduleObj.assigned_to_name);
            retValue += String.Format(template, "Check List", m_pmscheduleObj.checklist_name);
            retValue += String.Format(template, "Frequency", m_pmscheduleObj.frequency_name);
            retValue += String.Format(template, "Last Done Date", m_pmscheduleObj.last_done_date);
            retValue += String.Format(template, "Scheduled Date", m_pmscheduleObj.schedule_date);*/


           /* if (m_pmscheduleObj.schedule_link_job.Count != 0)
            {
                retValue += String.Format(template, "Associated Jobs", m_pmscheduleObj.schedule_link_job);
            }
            if (string.IsNullOrEmpty(m_pmscheduleObj.permit_code))
            {
                retValue += String.Format(template, "Permit ID", m_pmscheduleObj.permit_id);
            }
*/
            /*switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_START:
                    retValue += String.Format(template, "Started By", m_pmscheduleObj.started_by);
                    retValue += String.Format(templateEnd, "Started At", m_pmscheduleObj.started_at);
                    break;
                case CMMS.CMMS_Status.PM_UPDATED:
                    retValue += String.Format(template, "Updated By", m_pmscheduleObj.updated_by);
                    retValue += String.Format(templateEnd, "Updated At", m_pmscheduleObj.updated_at);
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:
                    retValue += String.Format(template, "Submitted By", m_pmscheduleObj.submitted_by);
                    retValue += String.Format(templateEnd, "Submitted At", m_pmscheduleObj.submitted_at);
                    break;
                case CMMS.CMMS_Status.PM_LINK_PTW:
                    retValue += "</table>"; break;
                case CMMS.CMMS_Status.PM_COMPLETED:
                    retValue += String.Format(templateEnd, "Completed At", m_pmscheduleObj.completed_at);
                    break;
                case CMMS.CMMS_Status.PM_APPROVED:
                    retValue += String.Format(template, "Approved By", m_pmscheduleObj.approved_by);
                    retValue += String.Format(templateEnd, "Approved At", m_pmscheduleObj.approved_at);
                    break;
                case CMMS.CMMS_Status.PM_REJECTED:
                    retValue += String.Format(template, "Rejected By", m_pmscheduleObj.rejected_by);
                    retValue += String.Format(templateEnd, "Rejected At", m_pmscheduleObj.rejected_at);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED:
                    retValue += String.Format(template, "Cancelled By", m_pmscheduleObj.cancelled_by);
                    retValue += String.Format(templateEnd, "Cancelled At", m_pmscheduleObj.cancelled_at);
                    break;
                case CMMS.CMMS_Status.PM_DELETED:
                    retValue += String.Format(template, "Deleted By", m_pmscheduleObj.deleted_by);
                    retValue += String.Format(templateEnd, "Deleted At", m_pmscheduleObj.deleted_at); break;
                //case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
                //    retValue = String.Format("Maintenance Order <{0}> Cancelled", m_pmscheduleObj.maintenance_order_number);
                //    break;
                default:
                    break;
            }

            return retValue;
        }*/


    }

