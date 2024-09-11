using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Notifications
{
    public class MRSNotification : CMMSNotification
    {
        CMMRSList m_MRSObj;

        public MRSNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMRSList MRSObj) : base(moduleID, notificationID)
        {
            m_MRSObj = MRSObj;
            m_module_ref_id = m_MRSObj.ID;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue += String.Format("MRS{0} MRS{1} Requested By {2}.", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.requested_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue += String.Format("MRS{0} Request Rejected By {1}", m_MRSObj.ID, m_MRSObj.request_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue += String.Format("MRS{0} Request Approved By {1}", m_MRSObj.ID, m_MRSObj.approver_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED:
                    retValue += String.Format("MRS{0} Issued By {1}", m_MRSObj.ID, m_MRSObj.issued_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED:
                    retValue += String.Format("MRS{0} Issue Rejected By {1}", m_MRSObj.ID, m_MRSObj.issue_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED:
                    retValue += String.Format("MRS{0} Issue Approved By {1}", m_MRSObj.ID, m_MRSObj.issue_appoved_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_RETURN:
                    retValue += String.Format("MRS{0} Return By {1}", m_MRSObj.ID, m_MRSObj.issue_appoved_by_name);
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
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue = String.Format("{0} MRS{1} Requested By {2}.", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.requested_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue = String.Format("{0} MRS{1} Request Rejected By {2}", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.request_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue = String.Format("{0} MRS{1} Request Approved By {2}", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.approver_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED:
                    retValue = String.Format("{0} MRS{1} Issued By {2}", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.issued_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED:
                    retValue = String.Format("{0} MRS{1} Issue Rejected By {2}", m_MRSObj.facilityName, m_MRSObj.ID,  m_MRSObj.issue_rejected_by_name); 
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED:
                    retValue = String.Format("{0} MRS{1} Issue Approved By {2}", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.issue_appoved_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_RETURN:
                    retValue = String.Format("{0} MRS{1} Return By {2}", m_MRSObj.facilityName, m_MRSObj.ID, m_MRSObj.issue_appoved_by_name);
                    break;

            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args) 
        {
            string retValue = "";

            retValue = String.Format("<table style='width: 50%; margin: 0 auto; border-collapse: collapse; border-spacing: 10px'><tr><td style='white-space: nowrap;'><h3><b style='color:#31576D'>Status : </b>{0}</h3></td></tr></table>", m_MRSObj.status_long + " at " + m_MRSObj.facilityName);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            
            retValue += String.Format(template, "ID", "MRS" + m_MRSObj.ID);
            retValue += String.Format(template, "Status", m_MRSObj.status_short);
            retValue += String.Format(template, "Activity", m_MRSObj.activity);
            retValue += String.Format(template, "Where Used", $"{m_MRSObj.whereUsedTypeName} {m_MRSObj.whereUsedRefID}");
            if (m_MRSObj.approved_by_emp_ID > 0)
            {
                retValue += String.Format(template, "Approval By", m_MRSObj.approver_name + " at " + m_MRSObj.approval_date);
                retValue += String.Format(template, "Approval Status", m_MRSObj.approval_status);
                retValue += String.Format(template, "Approval Comment", m_MRSObj.approval_comment);
            }
            retValue += String.Format(template, "Remarks", m_MRSObj.remarks  ?? "N/A");

            if (m_MRSObj.requested_by_emp_ID > 0)
            {
                retValue += String.Format(template, "Requested By", m_MRSObj.requested_by_name + " at " + m_MRSObj.requestd_date);
            }
            if (!string.IsNullOrEmpty(m_MRSObj.request_rejected_by_name))
            {
                retValue += String.Format(template, "Request Rejected By", m_MRSObj.request_rejected_by_name + " at " + m_MRSObj.request_rejected_at);
            }
            if (!string.IsNullOrEmpty(m_MRSObj.approver_name))
            {
                retValue += String.Format(template, "Request Approved By", m_MRSObj.approver_name + " at " + m_MRSObj.approval_date);
            }
            if (!string.IsNullOrEmpty(m_MRSObj.issued_name))
            {
                retValue += String.Format(template, "Issued By", m_MRSObj.issued_name + " at " + m_MRSObj.issuedAt);
            }
            if (m_MRSObj.issue_approved_by_emp_ID > 0)
            {
                retValue += String.Format(template, "Issue Approved  By", m_MRSObj.issue_appoved_by_name + " at " + m_MRSObj.issue_approved_date);
            }
            if (m_MRSObj.issue_rejected_by_emp_ID > 0)
            {
                retValue += String.Format(template, "Issue Rejected By", m_MRSObj.issue_rejected_by_name + " at " + m_MRSObj.issue_rejected_date);
            }

            retValue += "</table><br><br>";

            // MRS Items Table
            retValue += "<h4 style='text-align:center;'>Materials</h4>";
            retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
            retValue += "<tr>";
            retValue += "<th>Material Name</th>";
            retValue += "<th>Asset Type</th>";
            retValue += "<th>Available Quantity</th>";
            retValue += "<th>Requested Quantity</th>";
            retValue += "<th>Issued Quantity</th>";            
            retValue += "<th>Consumed Quantity</th>";
            retValue += "</tr>";

            foreach (var item in m_MRSObj.CMMRSItems)
            {
                retValue += "<tr>";
                retValue += String.Format("<td>{0}</td>", item.asset_name);
                retValue += String.Format("<td>{0}</td>", item.asset_type);
                retValue += String.Format("<td>{0}</td>", item.available_qty);
                retValue += String.Format("<td>{0}</td>", item.requested_qty);
                retValue += String.Format("<td>{0}</td>", item.issued_qty);
                retValue += String.Format("<td>{0}</td>", item.used_qty);
                retValue += "</tr>";
            }

            retValue += "</table><br><br>";

            retValue += "<div style='text-align:center;'>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue += String.Format(templateEnd, "Requested By", m_MRSObj.requested_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue += String.Format(templateEnd, "Request Rejected By",m_MRSObj.request_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue += String.Format(templateEnd, "Request Approved By",m_MRSObj.approver_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED:
                    retValue += String.Format(templateEnd, "Issued By", m_MRSObj.issued_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED:
                    retValue += String.Format(templateEnd, "Issue Rejected By", m_MRSObj.issue_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED:
                    retValue += String.Format(templateEnd, "Issue Approved By", m_MRSObj.issue_appoved_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_RETURN:
                    retValue += String.Format(templateEnd, "Issue Approved By", m_MRSObj.issue_appoved_by_name);
                    break;
                default:
                    break;
            }

            return retValue;
        }
    }
}
