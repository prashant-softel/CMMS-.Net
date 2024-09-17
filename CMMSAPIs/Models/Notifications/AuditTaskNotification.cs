using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Notifications
{
    public class AuditTasknotification : CMMSNotification
    {
        CMPMTaskView m_AuditObj;

        public AuditTasknotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMTaskView AuditObj) : base(moduleID, notificationID)
        {
            m_AuditObj = AuditObj;
            m_module_ref_id = m_AuditObj.id;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {

                case CMMS.CMMS_Status.AUDIT_START:
                    retValue += String.Format("{0} Audit{1} Started By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.started_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP:
                    retValue += String.Format("{0} AUD{1} Skip By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.skip_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_REJECT:
                    retValue += String.Format("{0} AUD{1} Skip Rejected By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.skip_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_APPROVED:
                    retValue += String.Format("{0} AUD{1} Skip Approved By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.skip_approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED:
                    retValue += String.Format("{0} AUD{1} Closed By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED:
                    retValue += String.Format("{0} AUD{1} Close Approved By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.closedApprovedByName);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_REJECT:
                    retValue += String.Format("{0} AUD{1} Close Rejected By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.AUDIT_EXECUTED:
                    retValue += String.Format("{0} AUD{1} Executed By {2}", m_AuditObj.facility_name, m_AuditObj.plan_id, m_AuditObj);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue += String.Format("{0} AUD{1} Linked To PTW By {2}", m_AuditObj.facility_name, m_AuditObj.plan_id, m_AuditObj);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;
        }

        override protected string getSubject(params object[] args)
        {
            string retValue = "My Job Card subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_START:
                    retValue = String.Format("{0} Audit{1} Started By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.started_by_name);     
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP:
                    retValue = String.Format("{0} AUD{1} Skip By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.skip_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_REJECT:
                    retValue = String.Format("{0} AUD{1} Skip Rejected By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.skip_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_APPROVED:
                    retValue = String.Format("{0} AUD{1} Skip Approved By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.skip_approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED:
                    retValue = String.Format("{0} AUD{1} Closed By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED:
                    retValue = String.Format("{0} AUD{1} Close Approved By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.closedApprovedByName);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_REJECT:
                    retValue = String.Format("{0} AUD{1} Close Rejected By {2}", m_AuditObj.facility_name, m_AuditObj.id, m_AuditObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.AUDIT_EXECUTED:
                    retValue = String.Format("{0} AUD{1} Executed By {2}", m_AuditObj.facility_name, m_AuditObj.plan_id, m_AuditObj);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("{0} AUD{1} Linked To PTW By {2}", m_AuditObj.facility_name, m_AuditObj.plan_id, m_AuditObj);
                    break;
            }
            return retValue;
        }
        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";


                retValue = String.Format("<h3 style='text-align:center;'><b style='color:#31576D'>Status : </b>{0}</h3><br>", m_AuditObj.status_long + " At " + m_AuditObj.facility_name);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Audit Task ID", "AUT" + m_AuditObj.id);
                retValue += String.Format(template, "Plan Title", m_AuditObj.plan_title);
                retValue += String.Format(template, "Frequency", m_AuditObj.frequency_name);
                retValue += String.Format(template, "checklist", m_AuditObj.checklist_name);
                retValue += String.Format(template, "Schedule Date", m_AuditObj.Schedule_Date);
                retValue += String.Format(template, "Last Done Date", m_AuditObj.last_done_date);
                retValue += String.Format(template, "Due Date", m_AuditObj.due_date);
                retValue += String.Format(template, "Assigned To", m_AuditObj.assigned_to_name);
                retValue += String.Format(template, "Done Date", m_AuditObj.done_date);
            


            if (!string.IsNullOrEmpty(m_AuditObj.started_by_name))
            {
                retValue += String.Format(template, "Started By", m_AuditObj.started_by_name + " at " + m_AuditObj.started_at);
            }
            if(!string.IsNullOrEmpty(m_AuditObj.skip_by_name))
            {
                retValue += String.Format(template, "Skip By", m_AuditObj.skip_by_name + " at " + m_AuditObj.skip_date);
            }
            if (!string.IsNullOrEmpty(m_AuditObj.skip_by_name) && !string.IsNullOrEmpty(m_AuditObj.skip_rejected_by_name))
            {
                retValue += String.Format(template, "Skip Rejected By", m_AuditObj.skip_rejected_by_name + " at " + m_AuditObj.Skip_rejected_Date);
            }
            if (!string.IsNullOrEmpty(m_AuditObj.skip_approved_by_name))
            {
                retValue += String.Format(template, "Skip Approved By", m_AuditObj.skip_approved_by_name + " at " + m_AuditObj.skip_approved_at);
            }
            if (!string.IsNullOrEmpty(m_AuditObj.closed_by_name))
            {
                retValue += String.Format(template, "Closed By", m_AuditObj.closed_by_name + " at " + m_AuditObj.closed_at);
            }
            if (!string.IsNullOrEmpty(m_AuditObj.closeRejectedbyName))
            {
                retValue += String.Format(template, "Close Rejected By", m_AuditObj.closeRejectedbyName + " at " + m_AuditObj.rejected_at);
            }
            if (!string.IsNullOrEmpty(m_AuditObj.closedApprovedByName))
            {
                retValue += String.Format(template, "Close Approved By", m_AuditObj.closedApprovedByName + " at " + m_AuditObj.approved_at);
            }


            retValue += "</table><br><br>";

            retValue += "<div style='text-align:center;'>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_START:
                    retValue += String.Format(templateEnd, "Started By", m_AuditObj.started_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP:
                    retValue += String.Format(templateEnd, "Skipped By : ", m_AuditObj.skip_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_REJECT:
                    retValue += String.Format(templateEnd, "Skip Rejected By : ", m_AuditObj.skip_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_SKIP_APPROVED:
                    retValue += String.Format(templateEnd, "Skip Approved By : ", m_AuditObj.skip_approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED:
                    retValue += String.Format(templateEnd, "Closed By : ", m_AuditObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED:
                    retValue += String.Format(templateEnd, "Close Approved By : ", m_AuditObj.closedApprovedByName);
                    break;
                case CMMS.CMMS_Status.AUDIT_CLOSED_REJECT:
                    retValue += String.Format(templateEnd, "Close Rejected By : ", m_AuditObj.closeRejectedbyName);
                    break;
                case CMMS.CMMS_Status.AUDIT_EXECUTED:
                    retValue += String.Format(templateEnd, "Executed By : ", m_AuditObj);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue += String.Format(templateEnd, "PTW Linked To Audit By : ", m_AuditObj);
                    break;
                default:
                    break;
            }

            return retValue;
        }

    }
}
