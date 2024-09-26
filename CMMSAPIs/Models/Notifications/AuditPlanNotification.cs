using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Notifications
{
    public class AuditPlannotification : CMMSNotification
    {

        CMPMPlanDetail m_AuditPlanObj;

        public AuditPlannotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMPlanDetail AuditPlanObj) : base(moduleID, notificationID)
        {
            m_AuditPlanObj = AuditPlanObj;
            m_module_ref_id = m_AuditPlanObj.plan_id;
        }
        
        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_SCHEDULE:
                    retValue += String.Format("{0} AUD{1} Created By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_REJECTED:
                    retValue += String.Format("{0} AUD{1} Rejected By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_APPROVED:
                    retValue += String.Format("{0} AUD{1} Approved By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_DELETED:
                    retValue += String.Format("{0} AUD{1} Deleted By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.deleted_by_name);
                    break;

            }
            retValue += $" for {m_delayDays} days";
            return retValue;
        }

        protected override string getURL(params object[] args)
        {
            return $"{m_baseURL}/viewAuditPlan/{m_module_ref_id}";
        }

        override protected string getSubject(params object[] args)
        {
            string retValue = "My Job Card subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_SCHEDULE:
                    retValue = String.Format("{0} AUD{1} Created By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_REJECTED:
                    retValue = String.Format("{0} AUD{1} Rejected By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_APPROVED:
                    retValue = String.Format("{0} AUD{1} Approved By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_DELETED:
                    retValue = String.Format("{0} AUD{1} Deleted By {2}", m_AuditPlanObj.facility_name, m_AuditPlanObj.plan_id, m_AuditPlanObj.deleted_by_name);
                    break;
            }
            return retValue;
        }
        override protected string getHTMLBody(params object[] args)
        {
                string retValue = "";

                retValue = String.Format("<h3 style='text-align:center;'><b style='color:#31576D'>Status : </b>{0}</h3><br>", m_AuditPlanObj.status_long + " At " + m_AuditPlanObj.facility_name);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Audit ID", "AUD" + m_AuditPlanObj.plan_id);
                retValue += String.Format(template, "Plan Title", m_AuditPlanObj.plan_name);
                retValue += String.Format(template, "Frequency", m_AuditPlanObj.plan_freq_name);
                retValue += String.Format(template, "checklist", m_AuditPlanObj.checklist_name);
                retValue += String.Format(template, "Schedule Date", m_AuditPlanObj.schedule_Date);
                retValue += String.Format(template, "Description", m_AuditPlanObj.description);
                retValue += String.Format(template, "Assigned To", m_AuditPlanObj.assigned_to_name);
                retValue += String.Format(template, "Employees", m_AuditPlanObj.Employees);
            
                if (m_AuditPlanObj.created_by_id > 0)
                {
                    retValue += String.Format(template, "Created By", m_AuditPlanObj.created_by_name + " at " + m_AuditPlanObj.created_at);
                }
                if (!string.IsNullOrEmpty(m_AuditPlanObj.rejected_by_name))
                {
                    retValue += String.Format(template, "Rejected By", m_AuditPlanObj.rejected_by_name + " at " + m_AuditPlanObj.rejected_at);
                }
                if (!string.IsNullOrEmpty(m_AuditPlanObj.approved_by_name))
                {
                    retValue += String.Format(template, "Approved By", m_AuditPlanObj.approved_by_name + " at " + m_AuditPlanObj.approved_at);
                }
                if (!string.IsNullOrEmpty(m_AuditPlanObj.deleted_by_name))
                {
                    retValue += String.Format(template, "Deleted By", m_AuditPlanObj.deleted_by_name + " at " + m_AuditPlanObj.deleted_Date);
                }
            

            retValue += "</table><br><br>";

            retValue += "<div style='text-align:center;'>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.AUDIT_SCHEDULE:
                    retValue += String.Format(templateEnd, "Created By : ", m_AuditPlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_REJECTED:
                    retValue += String.Format(templateEnd, "Rejected By : ", m_AuditPlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_APPROVED:
                    retValue += String.Format(templateEnd, "Approved By : ", m_AuditPlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.AUDIT_DELETED:
                    retValue += String.Format(templateEnd, "Deleted By : ", m_AuditPlanObj.deleted_by_name);
                    break;
                default:
                    break;
            }

            return retValue;
        }

    }
}
