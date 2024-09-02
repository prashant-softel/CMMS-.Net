﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.BS.Grievance;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class PMNotification : CMMSNotification
    {
        int m_PMId;
        CMPMScheduleExecutionDetail m_pmscheduleObj;
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

        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMScheduleExecutionDetail pmscheduleObj) : base(moduleID, notificationID)
        {
            m_pmscheduleObj = pmscheduleObj;
            m_PMId = m_pmscheduleObj.schedule_id;
        }

        override protected string getEMSubject(params object[] args)
        {

            string retValue = "ESCALATION: ";
            if (m_pmPlanObj != null)
            {
                m_PMId = m_pmPlanObj.plan_id;
            }


            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_START:     //Created                  
                    retValue = String.Format("PMS<{0}> Schedule Started By <{1}>", m_pmscheduleObj.schedule_id, m_pmscheduleObj.PM_Execution_Started_by_name);
                    break;
                case CMMS.CMMS_Status.PM_UPDATED:     //Assigned
                    retValue = String.Format("PMS<{0}> Schedule Updated By <{1}>", m_pmscheduleObj.schedule_id, m_pmscheduleObj.PM_Schedule_updated_by);
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:     //Linked
                    retValue = String.Format("PMS<{0}> Schedule Submitted By <{1}>", m_pmscheduleObj.schedule_id, m_pmscheduleObj.submittedByName);
                    break;
                case CMMS.CMMS_Status.PM_LINK_PTW:     //Closed
                    retValue = String.Format("PMT<{0}> Task Linked to PTW", m_pmExecutionObj.id);
                    break;
                case CMMS.CMMS_Status.PM_CLOSED:     //Cancelled
                    retValue = String.Format("PMT<{0}> Task Closed By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:     //Closed
                    retValue = String.Format("PMT<{0}> Task Approved By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:     //Cancelled
                    retValue = String.Format("PMT<{0}> Task Rejected By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED:     //Closed
                    retValue = String.Format("PMT<{0}> Task Cancelled By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.cancelled_by_name);
                    break;
                case CMMS.CMMS_Status.PM_DELETED:     //Cancelled
                    retValue = String.Format("PMS<{0}> Schedule Deleted", m_pmscheduleObj.schedule_id);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_CREATED:     //Cancelled
                    retValue = String.Format("PMP<{0}> Plan Created By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DRAFT:     //Cancelled
                    retValue = String.Format("PMT<{0}> Task  Drafted By <{1}>", m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                    retValue = String.Format("PMP<{0}> Plan Updated By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    retValue = String.Format("PMP<{0}> Plan Deleted ", m_pmPlanObj.plan_id);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = String.Format("PMP<{0}> Plan Approved By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = String.Format("PMP<{0}> Plan Rejected By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
                    retValue = String.Format("PMS<{0}> Scheduled By <{1}> ", m_pmscheduleObj.schedule_id, m_pmscheduleObj.createdbyName);
                    break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = String.Format("PMT<{0}> Task Assigned to <{1}> ", m_pmExecutionObj.id, m_pmExecutionObj.assigned_to_name);
                    break;
                /*case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                    retValue = String.Format("Preventive Task <{0}> Close Rejected By <{1}> ", m_pmExecutionObj.id, m_pmExecutionObj.cl);
                    break;*/
                /* case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                     retValue = String.Format("Preventive Order <{0}> Rejected By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.approved_close_by_name);
                     break;*/
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = String.Format("PMT<{0}> Task Deleted By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.deletedbyName);
                    break;
                default:
                    if (m_pmPlanObj != null && m_pmPlanObj.plan_id != 0)
                    {
                        retValue += String.Format("PMP{0} Undefined status {1}", m_pmPlanObj.plan_id, m_notificationID);
                    }
                    else if (m_pmExecutionObj != null && m_pmExecutionObj.id != 0)
                    {
                        retValue += String.Format("PMT{0} Undefined status {1}", m_pmExecutionObj.id, m_notificationID);
                    }
                    else if (m_pmscheduleObj != null && m_pmscheduleObj.schedule_id != 0)
                    {
                        retValue += String.Format("PMS{0} Undefined status {1}", m_pmscheduleObj.schedule_id, m_notificationID);
                    }
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "";
            if (m_pmPlanObj != null)
            {
                m_PMId = m_pmPlanObj.plan_id;
            }


            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_START:     //Created                  
                    retValue = String.Format("PMT<{0}> Task Started By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.started_by_name);
                    break;
                case CMMS.CMMS_Status.PM_UPDATED:     //Assigned
                    retValue = String.Format("PMS<{0}> Schedule Updated By <{1}>", m_pmscheduleObj.schedule_id, m_pmscheduleObj.PM_Schedule_updated_by);
                    break;
                case CMMS.CMMS_Status.PM_TASK_UPDATED:     //Assigned
                    retValue = String.Format("PMT<{0}> Task Updated By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:     //Linked
                    retValue = String.Format("PMS<{0}> Schedule Submitted By <{1}>", m_pmscheduleObj.schedule_id, m_pmscheduleObj.submittedByName);
                    break;
                case CMMS.CMMS_Status.PM_LINK_PTW:     //Closed
                    retValue = String.Format("PMT<{0}> Task Linked to Permit ID {1}", m_pmExecutionObj.id, m_pmExecutionObj.permit_id);
                    break;
                case CMMS.CMMS_Status.PM_CLOSED:     //Cancelled
                    retValue = String.Format("PMT<{0}> Task Closed By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:     //Closed
                    retValue = String.Format("PMT<{0}> Task Approved By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:     //Cancelled
                    retValue = String.Format("PMT<{0}> Task Rejected By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED:     //Closed
                    retValue = String.Format("PMT<{0}> Task Cancelled By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.cancelled_by_name);
                    break;
                case CMMS.CMMS_Status.PM_DELETED:     //Cancelled
                    retValue = String.Format("PMS<{0}> Schedule Deleted", m_pmscheduleObj.schedule_id);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_CREATED:     //Cancelled
                    retValue = String.Format("PMP<{0}> Plan Created By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DRAFT:     //Cancelled
                    retValue = String.Format("PMT<{0}> Task  Drafted By <{1}>", m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                    retValue = String.Format("PMP<{0}> Plan Updated By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    retValue = String.Format("PMP<{0}> Plan Deleted ", m_pmPlanObj.plan_id);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = String.Format("PMP<{0}> Plan Approved By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = String.Format("PMP<{0}> Plan Rejected By <{1}> ", m_pmPlanObj.plan_id, m_pmPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
                    retValue = String.Format("PMS<{0}> Scheduled By <{1}> ", m_pmscheduleObj.schedule_id, m_pmscheduleObj.createdbyName);
                    break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = String.Format("PMT<{0}> Task Assigned to <{1}> ", m_pmExecutionObj.id, m_pmExecutionObj.assigned_to_name);
                    break;
                    /*case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                        retValue = String.Format("Preventive Task <{0}> Close Rejected By <{1}> ", m_pmExecutionObj.id, m_pmExecutionObj.cl);
                        break;*/
                    /* case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                         retValue = String.Format("Preventive Order <{0}> Rejected By <{1}> ", m_pmExecutionObj.id, m_pmPlanObj.approved_close_by_name);
                         break;*/
                    case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                       retValue = String.Format("PMT<{0}> Cancelled Approved By <{1}> ", m_pmExecutionObj.id, m_pmExecutionObj.cancelledapprovedbyName);
                       break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue = String.Format("PMT<{0}> Cancelled Rejected By <{1}> ", m_pmExecutionObj.id, m_pmExecutionObj.cancelledrejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = String.Format("PMT<{0}> Task Deleted By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.deletedbyName);
                    break;
                default:
                    if (m_pmPlanObj != null && m_pmPlanObj.plan_id != 0)
                    {
                        retValue += String.Format("PMP{0} Undefined status {1}", m_pmPlanObj.plan_id, m_notificationID);
                    }
                    else if (m_pmExecutionObj != null && m_pmExecutionObj.id != 0)
                    {
                        retValue += String.Format("PMT{0} Undefined status {1}", m_pmExecutionObj.id, m_notificationID);
                    }
                    else if (m_pmscheduleObj != null && m_pmscheduleObj.schedule_id != 0)
                    {
                        retValue += String.Format("PMS{0} Undefined status {1}", m_pmscheduleObj.schedule_id, m_notificationID);
                    }
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
                retValue += String.Format(template, "Plan ID", "PMP" + m_pmPlanObj.plan_id);
                retValue += String.Format(template, "Status", m_pmPlanObj.status_short);
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
                retValue += String.Format(template, "Task ID ", "PMT" + m_pmExecutionObj.id);
                retValue += String.Format(template, "Status ", m_pmExecutionObj.status_short);
                // retValue += String.Format(template, "Task Name", m_pmExecutionObj.);
                retValue += String.Format(template, "Responsible Person ", m_pmExecutionObj.assigned_to_name);
                retValue += String.Format(template, "Started By ", m_pmExecutionObj.started_by_name);
                retValue += String.Format(template, "Frequency ", m_pmExecutionObj.frequency_name);
                //retValue += String.Format(template, "Last Done Date", m_pmPlanObj.);
                //retValue += String.Format(template, "Scheduled Date", m_pmPlanObj.schedule_date);
                retValue += String.Format(templateEnd, "PM Task Submited By", m_pmExecutionObj.createdbyName);
                retValue += String.Format(templateEnd, "PM Task Linked By", m_pmExecutionObj.ptw_tbt_done);
                if (m_pmExecutionObj.started_by_id > 0)
                {
                    retValue += String.Format(templateEnd, "PM Task Started By", m_pmExecutionObj.started_by_name);
                }

                if (m_pmExecutionObj.closed_by > 0)
                {
                    retValue += String.Format(templateEnd, "PM Task Closed By", m_pmExecutionObj.closed_by_name);
                    retValue += String.Format(templateEnd, "PM Task Closed Rejected By", m_pmExecutionObj.closeRejectedbyName);
                    retValue += String.Format(templateEnd, "PM Task Close Approved By", m_pmExecutionObj.approved_by_name);
                }
                

                if (m_pmExecutionObj.cancelled_by > 0)
                {
                    retValue += String.Format(templateEnd, "PM Task Cancelled By", m_pmExecutionObj.cancelled_by_name);
                    retValue += String.Format(templateEnd, "PM Task Cancelled Rejected By", m_pmExecutionObj.cancelledrejectedbyName);
                    retValue += String.Format(templateEnd, "PM Task Cancelled Approved", m_pmExecutionObj.cancelledapprovedbyName);
                }

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_TASK_UPDATED:     //Assigned
                        retValue = String.Format(templateEnd,"PMT<{0}> Task Updated By <{1}>", m_pmExecutionObj.id, m_pmExecutionObj.updated_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_TASK_DELETED:
                        retValue += String.Format(templateEnd, "PM Task Deleted By", m_pmExecutionObj.deletedbyName);
                        break;
                    default:
                        break;

                }
             
            }
            else if (m_pmscheduleObj != null && m_pmscheduleObj.schedule_id != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_pmscheduleObj.status_long);
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "ID", "PMS" + m_pmscheduleObj.schedule_id);
                retValue += String.Format(template, "Status", m_pmscheduleObj.status_short);
                //retValue += String.Format(template, "Responsible Person", m_pmscheduleObj.);
                //retValue += String.Format(template, "Check List", m_pmscheduleObj.);
                retValue += String.Format(template, "Frequency", m_pmscheduleObj.PM_Frequecy_Name);
                retValue += String.Format(template, "Scheduled Date", m_pmscheduleObj.PM_Schedule_date);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.PM_ASSIGNED:
                        retValue += "</table>"; break;
                        break;
                    /*case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                        retValue += String.Format(templateEnd, "PM Task Linked By", m_pmExecutionObj.updated_by_name);
                        break;*/
                    /* case CMMS.CMMS_Status.PM_START:
                         retValue += String.Format(templateEnd, "PM Task Started By", m_pmscheduleObj.started_by_name);
                         break;*/
                    case CMMS.CMMS_Status.PM_CLOSED:
                        retValue += String.Format(templateEnd, "PM Task Schedule Closed By", m_pmscheduleObj.completedBy_name);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Schedule Rejected By", m_pmscheduleObj.rejectedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Schedule Approved By", m_pmscheduleObj.approvedbyName);
                        break;
                   /* case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Closed Rejected By", m_pmscheduleObj.re);
                        break;
                    /case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Close Approved By", m_pmExecutionObj.approved_by);
                        break;*/
                    /*case CMMS.CMMS_Status.PM_CANCELLED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled By", m_pmscheduleObj.ca);
                        break;*/
                    case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                        retValue += String.Format(templateEnd, "PM Schedule Cancelled Rejected By ", m_pmscheduleObj.cancelledrejectedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                        retValue += String.Format(templateEnd, "PM Schedule Cancelled Approved By ", m_pmscheduleObj.cancelledapprovedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_DELETED:
                        retValue += String.Format(templateEnd, "PM Schedule Deleted for ID ", m_pmscheduleObj.schedule_id);
                        break;
                    case CMMS.CMMS_Status.PM_UPDATED:
                        retValue += String.Format(templateEnd, "PM Schedule Updated By", m_pmscheduleObj.PM_Schedule_updated_by);
                        break;
                    case CMMS.CMMS_Status.PM_START:
                        retValue += String.Format(templateEnd, "PM Schedule Started By ", m_pmscheduleObj.PM_Execution_Started_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_SUBMIT:
                        retValue += String.Format(templateEnd, "PM Schedule Submitted By ", m_pmscheduleObj.submittedByName);
                        break;
                    default:
                        retValue += String.Format(templateEnd, "PM Schedule Submitted By ", m_pmscheduleObj.submittedByName);
                        break;

                }
            }

            return retValue;
        }


    }



}















