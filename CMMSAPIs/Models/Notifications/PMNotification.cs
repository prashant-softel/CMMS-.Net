using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.BS.Grievance;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using System;

namespace CMMSAPIs.Models.Notifications
{
    internal class PMNotification : CMMSNotification
    {
        CMPMScheduleExecutionDetail m_pmscheduleObj;
        CMPMPlanDetail m_pmPlanObj;
        CMPMTaskView m_pmExecutionObj;

        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMTaskView executionObj) : base(moduleID, notificationID)
        {
            m_pmExecutionObj = executionObj;
            m_module_ref_id = m_pmExecutionObj.id;
        }

        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMPlanDetail pmObj) : base(moduleID, notificationID)
        {
            m_pmPlanObj = pmObj;
            m_module_ref_id = m_pmPlanObj.plan_id;
        }

        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMScheduleExecutionDetail pmscheduleObj) : base(moduleID, notificationID)
        {
            m_pmscheduleObj = pmscheduleObj;
            m_module_ref_id = m_pmscheduleObj.schedule_id;
        }

        override protected string getEMSubject(params object[] args)
        {

            string retValue = "ESCALATION: ";
            if (m_pmPlanObj != null)
            {
                m_module_ref_id = m_pmPlanObj.plan_id;
            }


            switch (m_notificationID)
            {

                case CMMS.CMMS_Status.PM_PLAN_CREATED:     //Cancelled
                    retValue = String.Format("{0} PM{1} Created by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                    retValue = String.Format("{0} PM{1} Updated By {2} ", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    retValue = String.Format("{0} PM{1} Deleted By {2} ", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.deleted_by_name); 
                    break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = String.Format("{0} PM{1} Approved by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = String.Format("{0} PM{1} Rejected by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.rejected_by_name);
                    break;



                case CMMS.CMMS_Status.PM_LINK_PTW:     //Closed
                    retValue = String.Format("{0} PTW{1} Linked by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.permit_id, m_pmExecutionObj.status_updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:     //Closed
                    retValue = String.Format("{0} PM{1} Close Approved by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.closedApprovedByName);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:     //Cancelled
                    retValue = String.Format("{0} PM{1} Close Rejected by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED:     //Closed
                    retValue = String.Format("{0} PM{1} Created by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.cancelledbyName);
                    break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = String.Format("{0} PM{1} Assigned to {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.assigned_to_name);
                    break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = String.Format("{0} PM{1} Deleted By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.deletedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CLOSED:
                    retValue = String.Format("{0} PM{1} Closed By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                    retValue = String.Format("{0} PM{1} Cancelled Approved By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.cancelledapprovedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue = String.Format("{0} PM{1} Cancelled Rejected By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.cancelledrejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_START:     //Created                  
                    retValue = String.Format("{0} PM{1} Started  By {2}", m_pmscheduleObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.createdbyName);
                    break;
                case CMMS.CMMS_Status.PM_TASK_UPDATED:     //Created                  
                    retValue = String.Format("{0} PM{1} Updated  By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.updated_by_name);
                    break;



                case CMMS.CMMS_Status.PM_UPDATED:     //Assigned
                    retValue = String.Format("{0} PMs{1} Updated by {2}", m_pmscheduleObj.facilityidbyName, m_pmscheduleObj.schedule_id, m_pmscheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:     //Linked
                    retValue = String.Format("{0} PMS{1} Submitted by {2}", m_pmscheduleObj.facilityidbyName, m_pmscheduleObj.schedule_id, m_pmscheduleObj.submittedByName);
                    break;
                /*case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
                    retValue = String.Format("PMS<{0}> Scheduled By <{1}> ", m_pmscheduleObj.schedule_id, m_pmscheduleObj.createdbyName);
                    break;*/
                
                case CMMS.CMMS_Status.PM_DELETED:     //Cancelled
                    retValue = String.Format("PMS<{0}> Schedule Deleted", m_pmscheduleObj.schedule_id);
                    break;
                default:
                    if (m_pmPlanObj != null && m_pmPlanObj.plan_id != 0)
                    {
                        retValue += String.Format("PMP{0} Undefined status {1}", m_pmPlanObj.plan_id, m_notificationID);
                    }
                    else if (m_pmExecutionObj != null && m_pmExecutionObj.id != 0)
                    {
                        retValue += String.Format("PM{0} Undefined status {1}", m_pmExecutionObj.id, m_notificationID);
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


            switch (m_notificationID)
            {

                case CMMS.CMMS_Status.PM_PLAN_CREATED:     //Cancelled
                    retValue = String.Format("{0} PM{1} Created by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                    retValue = String.Format("{0} PM{1} Updated By <{1}> ", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    retValue = String.Format("{0} PM{1} Deleted by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.deleted_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = String.Format("{0} PM{1} Approved by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = String.Format("{0} PM{1} Rejected by {2}", m_pmPlanObj.facilityidbyName, m_pmPlanObj.plan_id, m_pmPlanObj.rejected_by_name);
                    break;



                case CMMS.CMMS_Status.PM_LINK_PTW:     //Closed
                    retValue = String.Format("{0} PTW{1} Linked by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.permit_id, m_pmExecutionObj.status_updated_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:     //Closed
                    retValue = String.Format("{0} PM{1} Close Approved by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.closedApprovedByName);
                    break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:     //Cancelled
                    retValue = String.Format("{0} PM{1} Close Rejected by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED:     //Closed
                    retValue = String.Format("{0} PM{1} Cancelled by {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.cancelledbyName);
                    break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = String.Format("{0} PM{1} Assigned to {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.assigned_to_name);
                    break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = String.Format("{0} PM{1} Deleted By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.deletedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CLOSED:
                    retValue = String.Format("{0} PM{1} Closed By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                    retValue = String.Format("{0} PM{1} Cancelled Approved By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.cancelledapprovedbyName);
                    break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue = String.Format("{0} PM{1} Cancelled Rejected By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.cancelledrejectedbyName);
                    break;
                case CMMS.CMMS_Status.PM_START:     //Created                  
                    retValue = String.Format("{0} PM{1} Started  By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.createdbyName);
                    break;
                case CMMS.CMMS_Status.PM_TASK_UPDATED:     //Created                  
                    retValue = String.Format("{0} PM{1} Updated  By {2}", m_pmExecutionObj.facilityidbyName, m_pmExecutionObj.id, m_pmExecutionObj.updated_by_name);
                    break;

                    



                case CMMS.CMMS_Status.PM_UPDATED:     //Assigned
                    retValue = String.Format("{0} PM{1} Updated by {2}", m_pmscheduleObj.facilityidbyName, m_pmscheduleObj.schedule_id, m_pmscheduleObj.updatedbyName);
                    break;
                case CMMS.CMMS_Status.PM_SUBMIT:     //Linked
                    retValue = String.Format("{0} PM{1} Submitted by {2}", m_pmscheduleObj.facilityidbyName, m_pmscheduleObj.schedule_id, m_pmscheduleObj.submittedByName);
                    break;

                case CMMS.CMMS_Status.PM_DELETED:     //Cancelled
                    retValue = String.Format("PM<{0}> Schedule Deleted", m_pmscheduleObj.schedule_id);
                    break;

                default:
                    if (m_pmPlanObj != null && m_pmPlanObj.plan_id != 0)
                    {
                        retValue += String.Format("PMP{0} Undefined status {1}", m_pmPlanObj.plan_id, m_notificationID);
                    }
                    else if (m_pmExecutionObj != null && m_pmExecutionObj.id != 0)
                    {
                        retValue += String.Format("PM{0} Undefined status {1}", m_pmExecutionObj.id, m_notificationID);
                    }
                    else if (m_pmscheduleObj != null && m_pmscheduleObj.schedule_id != 0)
                    {
                        retValue += String.Format("PM{0} Undefined status {1}", m_pmscheduleObj.schedule_id, m_notificationID);
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
                retValue += String.Format(template, "Status", m_pmPlanObj.created_by_name);
                if(m_pmPlanObj.plan_date!=null)
                {
                    retValue += String.Format(template, "Date Created", m_pmPlanObj.plan_date);
                }

                if (m_pmPlanObj.updated_at != null && m_pmPlanObj.updated_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "Updated At", m_pmPlanObj.updated_at);
                }


                retValue += String.Format(template, "Status", m_pmPlanObj.status_short);
                retValue += String.Format(template, "Plan Name", m_pmPlanObj.plan_name);
                retValue += String.Format(template, "Responsible Person", m_pmPlanObj.assigned_to_name);
                retValue += String.Format(template, "Category Name", m_pmPlanObj.category_name);
                retValue += String.Format(template, "Frequency", m_pmPlanObj.plan_freq_name);
                retValue += String.Format(template, "Plan Date", m_pmPlanObj.plan_date);


                if (m_pmPlanObj.rejected_at != null && m_pmPlanObj.rejected_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "Rejected At", m_pmPlanObj.rejected_at);
                }

                if (m_pmPlanObj.approved_at != null && m_pmPlanObj.approved_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "Approved At", m_pmPlanObj.approved_at);
                }



                //retValue += String.Format(template, "Scheduled Date", m_pmPlanObj.schedule_date);



                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_PLAN_DRAFT:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.PM_PLAN_CREATED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                        retValue += String.Format(templateEnd, "PM Plan Updated By", m_pmPlanObj.updated_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "PM Plan Approved By", m_pmPlanObj.approved_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "PM Plan Rejected By", m_pmPlanObj.rejected_by_name);
                        break;
                    default:
                        break;

                }

                if (m_pmPlanObj.mapAssetChecklist.Count > 0)
                {
                    retValue += "<h4>Assets</h4>";
                    retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
                    retValue += "<tr>";
                    retValue += "<th>SR.No</th>";
                    retValue += "<th>Name</th>";
                    retValue += "<th>Parent Name</th>";
                    retValue += "<th>CheckList Name</th>";
                    ;
                    int i = 0;
                    foreach (var item in m_pmPlanObj.mapAssetChecklist)
                    {
                        retValue += "<tr>";
                        retValue += String.Format("<td>{0}</td>", ++i);
                        retValue += String.Format("<td>{0}</td>", item.name);
                        retValue += String.Format("<td>{0}</td>", item.parentName);
                        retValue += String.Format("<td>{0}</td>", item.checklist_name);
                        
                    }
                    retValue += "</table><br><br>";

                }

            }
            else if (m_pmExecutionObj != null && m_pmExecutionObj.id != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_pmExecutionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Task ID ", "PMT" + m_pmExecutionObj.id);
                retValue += String.Format(template, "Status ", m_pmExecutionObj.status_short);
                retValue += String.Format(template, "Responsible Person ", m_pmExecutionObj.assigned_to_name);
                retValue += String.Format(template, "Started By ", m_pmExecutionObj.started_by_name);
                retValue += String.Format(template, "Frequency ", m_pmExecutionObj.frequency_name);
                retValue += String.Format(template, "Title", m_pmExecutionObj.title);
                retValue += String.Format(template, "Category Name ", m_pmExecutionObj.categoryName);
                retValue += String.Format(template, "PM Task Submitted By", m_pmExecutionObj.createdbyName);

                

                if (m_pmExecutionObj.cancelled_at!=null && m_pmExecutionObj.cancelled_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "PM Task Cancelled At", m_pmExecutionObj.cancelled_at);

                }

                if (m_pmExecutionObj.updated_at != null && m_pmExecutionObj.updated_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "PM Task Updated At", m_pmExecutionObj.updated_at);

                }

                if (m_pmExecutionObj.approved_at != null && m_pmExecutionObj.approved_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "PM Task Approved At", m_pmExecutionObj.approved_at);

                }

                if (m_pmExecutionObj.closed_at != null && m_pmExecutionObj.closed_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "PM Task Closed At", m_pmExecutionObj.closed_at);

                }
                if (m_pmExecutionObj.started_at != null && m_pmExecutionObj.started_at != DateTime.MinValue)
                {
                    retValue += String.Format(template, "PM Task Started At", m_pmExecutionObj.started_at);

                }


                





                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_TASK_UPDATED:     //Assigned
                        retValue += String.Format(templateEnd,"PM Task Updated By ", m_pmExecutionObj.updated_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_TASK_DELETED:
                        retValue += String.Format(templateEnd, "PM Task Deleted By", m_pmExecutionObj.deletedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_LINK_PTW:
                        retValue += String.Format(templateEnd, "PM Task Linked By", m_pmExecutionObj.status_updated_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Closed By", m_pmExecutionObj.closedApprovedByName);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled By", m_pmExecutionObj.cancelledbyName);
                        break;
                    case CMMS.CMMS_Status.PM_ASSIGNED:
                        retValue += String.Format(templateEnd, "PM Task Assigned To", m_pmExecutionObj.assigned_to_name);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSED:
                        retValue += String.Format(templateEnd, "PM Task Closed By", m_pmExecutionObj.closed_by_name);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled Approved By", m_pmExecutionObj.cancelledapprovedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled Rejected By", m_pmExecutionObj.cancelledrejectedbyName);
                        break;
                    case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                        retValue += String.Format(templateEnd, "PM Task Cancelled Rejected By", m_pmExecutionObj.closeRejectedbyName);
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
                retValue += String.Format(template, "Frequency", m_pmscheduleObj.PM_Frequecy_Name);
                retValue += String.Format(template, "Scheduled Date", m_pmscheduleObj.PM_Schedule_date);



                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PM_SCHEDULED:
                        retValue += "</table>"; 
                        break;
                    case CMMS.CMMS_Status.PM_ASSIGNED:
                        retValue += "</table>";
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
                        break;

                }
            }

            return retValue;
        }


    }



}















