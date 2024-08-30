using CMMSAPIs.Helper;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Notifications
{
    public class GONotification : CMMSNotification
    {
        int m_GOId;
        CMGOMaster m_GOObj;
        public GONotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMGOMaster GOObj) : base(moduleID, notificationID)
        {
            m_GOObj = GOObj;
            m_GOId = m_GOObj.Id;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";
            m_GOId = m_GOObj.Id;

            switch (m_notificationID)
            {

                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue += String.Format("GO{0} Submitted By{1}", m_GOObj.Id, m_GOObj.submitted_by_name);
                    break; 
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue += String.Format("GO{0} Rejected By {1}", m_GOObj.Id, m_GOObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue += String.Format("GO{0} Approved By {1}", m_GOObj.Id, m_GOObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue += String.Format("GO{0} Receive Request Submitted By {1}", m_GOObj.Id, m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue += String.Format("GO{0} Receive Request Rejected By {1}", m_GOObj.Id, m_GOObj.receive_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue += String.Format("GO{0} Receive Request Approved By {1}", m_GOObj.Id, m_GOObj.receive_approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue += String.Format("GO{0} Closed By {1}", m_GOObj.Id, m_GOObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DELETED:
                    retValue += String.Format("GO{0} Deleted By {1}", m_GOObj.Id, m_GOObj.deleted_by_name);
                    break;

            }
            retValue += $" for {m_delayDays} days";
            return retValue;
        }

        override protected string getSubject(params object[] args)
        {
            string retValue = "My Job Card subject";
            m_GOId = m_GOObj.Id;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue = String.Format("GO{0} Submitted By {1}", m_GOObj.Id, m_GOObj.submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue = String.Format("GO{0} Rejected By {1}", m_GOObj.Id, m_GOObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue = String.Format("GO{0} Approved By {1}", m_GOObj.Id, m_GOObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue = String.Format("GO{0} Receive Request Submitted By {1}", m_GOObj.Id, m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue = String.Format("GO{0} Receive Request Rejected By {1}", m_GOObj.Id, m_GOObj.receive_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue = String.Format("GO{0} Receive Request Approved By {1}", m_GOObj.Id, m_GOObj.receive_approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue = String.Format("GO{0} Closed By {1}", m_GOObj.Id, m_GOObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DELETED:
                    retValue = String.Format("GO{0} Deleted By {1}", m_GOObj.Id, m_GOObj.deleted_by_name);
                    break;
            }
            return retValue;
        }
        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_GOObj.status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", "GO"+m_GOObj.Id ?? "N/A");
            retValue += String.Format(template, "Vendor Name", m_GOObj.vendor_name ?? "N/A");
            retValue += String.Format(template, "PO No.", m_GOObj.po_no);
            retValue += String.Format(template, "Amount", m_GOObj.amount+m_GOObj.currency);
            retValue += String.Format(template, "PO Date", m_GOObj.po_date);
            retValue += String.Format(template, "Number Of Packages Received", m_GOObj.no_pkg_received);
            retValue += String.Format(template, "Vehicle Number", m_GOObj.vehicle_no);
            retValue += String.Format(template, "GIR No.", m_GOObj.gir_no);
            retValue += String.Format(template, "Challan No.", m_GOObj.challan_no ?? "N/A");
            retValue += String.Format(template, "Challan Date", m_GOObj.challan_date);
            retValue += String.Format(template, "RO ID", m_GOObj.purchaseID);
            retValue += String.Format(template, "Ordered By", m_GOObj.order_by_type);
            retValue += String.Format(template, "LR No.", m_GOObj.lr_no);

            if (!string.IsNullOrEmpty(m_GOObj.submitted_by_name))
            {
                retValue += String.Format(template, "Submitted By", m_GOObj.submitted_by_name);
                retValue += String.Format(template, "Submitted At", m_GOObj.submitted_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.closed_by_name))
            {
                retValue += String.Format(template, "Closed By", m_GOObj.closed_by_name);
                retValue += String.Format(template, "Closed At", m_GOObj.closed_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.deleted_by_name))
            {
                retValue += String.Format(template, "Deleted By", m_GOObj.deleted_by_name);
                retValue += String.Format(template, "Deleted At", m_GOObj.deleted_at);
            }         
            if (!string.IsNullOrEmpty(m_GOObj.rejected_by_name))
            {
                retValue += String.Format(template, "Rejected By", m_GOObj.rejected_by_name);
                retValue += String.Format(template, "Rejected At", m_GOObj.rejected_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.approved_by_name))
            {
                retValue += String.Format(template, "Approved By", m_GOObj.approved_by_name);
                retValue += String.Format(template, "Approved At", m_GOObj.approved_at);
            }         
            if (!string.IsNullOrEmpty(m_GOObj.receive_submitted_by_name))
            {
                retValue += String.Format(template, "Receive Request Submitted By", m_GOObj.receive_submitted_by_name);
                retValue += String.Format(template, "Receive Request Submitted At", m_GOObj.receive_submitted_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.receive_rejected_by_name))
            {
                retValue += String.Format(template, "Receive Request Rejected By", m_GOObj.receive_rejected_by_name);
                retValue += String.Format(template, "Receive Request Rejected At", m_GOObj.receive_rejected_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.receive_approved_by_name))
            {
                retValue += String.Format(template, "Receive Request Approved By", m_GOObj.receive_approved_by_name);
                retValue += String.Format(template, "Receive Request Approved At", m_GOObj.receive_approved_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.closed_by_name))
            {
                retValue += String.Format(template, "Closed By", m_GOObj.closed_by_name);
                retValue += String.Format(template, "Closed At", m_GOObj.closed_at);
            }
            if (!string.IsNullOrEmpty(m_GOObj.deleted_by_name))
            {
                retValue += String.Format(template, "Deleted By", m_GOObj.deleted_by_name);
                retValue += String.Format(template, "Deleted At", m_GOObj.deleted_at);
            }

            retValue += "</table><br><br>";

            // GO Items Table
            retValue += "<h4>GO Items</h4>";
            retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
            retValue += "<tr>";
            retValue += "<th>Item Name</th>";
            retValue += "<th>Ordered Quantity</th>";
            retValue += "<th>Received Quantity</th>";
            retValue += "<th>Accepted Quantity</th>";
            retValue += "<th>Damaged Quantity</th>";
            retValue += "<th>Lost Quantity</th>";
            retValue += "</tr>";

            foreach (var item in m_GOObj.GODetails)
            {
                retValue += "<tr>";
                retValue += String.Format("<td>{0}</td>", item.assetItem_Name);
                retValue += String.Format("<td>{0}</td>", item.ordered_qty);
                retValue += String.Format("<td>{0}</td>", item.received_qty);
                retValue += String.Format("<td>{0}</td>", item.accepted_qty);
                retValue += String.Format("<td>{0}</td>", item.damaged_qty);
                retValue += String.Format("<td>{0}</td>", item.lost_qty);
                retValue += "</tr>";
            }
            retValue += "</table><br><br>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue += "</table>";
                    break;
                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue += String.Format(templateEnd, "Submitted By", m_GOObj.submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue += String.Format(templateEnd, "Closed By", m_GOObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DELETED:
                    retValue += String.Format(templateEnd, "Deleted By", m_GOObj.deleted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue += String.Format(templateEnd, "Rejected By", m_GOObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue += String.Format(templateEnd, "Approved By", m_GOObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue += String.Format(templateEnd, "Recieved Request Submitted By", m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue += String.Format(templateEnd, "Received Request Rejected By", m_GOObj.receive_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue += String.Format(templateEnd, "Received Request Approved By", m_GOObj.receive_approved_by_name);
                    break;
                default:
                    break;
            }
            return retValue;
        }
    }
}
