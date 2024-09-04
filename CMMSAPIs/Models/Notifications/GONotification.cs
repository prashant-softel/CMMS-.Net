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
                    retValue += String.Format("{0} GO{1} Submitted By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.submitted_by_name);
                    break; 
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue += String.Format("{0} GO{1} Rejected By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue += String.Format("{0} GO{1} Approved By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue += String.Format("{0} GO{1} Updated By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.go_updated_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue += String.Format("{0} GO{1} Receive Request Submitted By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue += String.Format("{0} GO{1} Receive Request Rejected By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.receive_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue += String.Format("{0} GO{1} Receive Request Approved By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.receive_approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue += String.Format("{0} GO{1} Closed By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DELETED:
                    retValue += String.Format("{0} GO{1} Deleted By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.deleted_by_name);
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
                    retValue = String.Format("{0} GO{1} Submitted By {2}",m_GOObj.facilityName, m_GOObj.Id, m_GOObj.submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue = String.Format("{0} GO{1} Rejected By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue = String.Format("{0} GO{1} Approved By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue = String.Format("{0} GO{1} Updated By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.go_updated_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue = String.Format("{0} GO{1} Receive Request Submitted By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue = String.Format("{0} GO{1} Receive Request Rejected By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.receive_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue = String.Format("{0} GO{1} Receive Request Approved By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.receive_approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue = String.Format("{0} GO{1} Closed By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DELETED:
                    retValue = String.Format("{0} GO{1} Deleted By {2}", m_GOObj.facilityName, m_GOObj.Id, m_GOObj.deleted_by_name);
                    break;
            }
            return retValue;
        }
        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status : </b>{0}</h3><br>", m_GOObj.status_long);

            
            retValue += "<div style='display: flex; flex-direction: column; align-items: center;'>";

            bool check1 = !string.IsNullOrEmpty(m_GOObj.receive_submitted_by_name);
            bool check2 = !string.IsNullOrEmpty(m_GOObj.receive_rejected_by_name);
            bool check3 = !string.IsNullOrEmpty(m_GOObj.receive_approved_by_name);
            bool check4 = !string.IsNullOrEmpty(m_GOObj.closed_by_name);
            bool check5 = string.IsNullOrEmpty(m_GOObj.go_updated_by_name);

            if ((check1 || check2 || check3 || check4) && check5)
            {
                
                retValue += "<div style='display: flex; justify-content: space-between; width: 100%; max-width: 1200px;'>";

                // Table 1
                retValue += "<div style='width: 48%;'>";
                retValue += String.Format("<table style='width: 100%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
                retValue += String.Format(template, "Vendor", m_GOObj.vendor_name ?? "N/A");
                retValue += String.Format(template, "PO No.", m_GOObj.po_no);
                retValue += String.Format(template, "Amount", $"{m_GOObj.amount} [{m_GOObj.currency}]");
                retValue += String.Format(template, "Invoice No.", m_GOObj.challan_no);
                retValue += String.Format(template, "Delivery Challan", m_GOObj.freight);
                retValue += String.Format(template, "Count Of Packages Received", m_GOObj.no_pkg_received);
                retValue += String.Format(template, "Vehicle No.", m_GOObj.vehicle_no);
                retValue += String.Format(template, "GIR No.", m_GOObj.condition_pkg_received);
                retValue += String.Format(template, "Freight", m_GOObj.freight_value);
                retValue += "</table>";
                retValue += "</div>";

                // Table 2
                retValue += "<div style='width: 48%;'>";
                retValue += String.Format("<table style='width: 100%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
                retValue += String.Format(template, "PO Date", m_GOObj.po_date);
                retValue += String.Format(template, "Currency", m_GOObj.currency);
                retValue += String.Format(template, "GRN No.", m_GOObj.gir_no);
                retValue += String.Format(template, "Invoice Date", m_GOObj.purchaseDate);
                retValue += String.Format(template, "Delivery Challan Date", m_GOObj.challan_date);
                retValue += String.Format(template, "Material Receive Date", m_GOObj.receivedAt);
                retValue += String.Format(template, "LR No.", m_GOObj.lr_no);
                retValue += String.Format(template, "E-way Bill", m_GOObj.job_ref);
                retValue += String.Format(template, "Inspection Report", m_GOObj.inspection_report);
                retValue += "</table>";
                retValue += "</div>";

                retValue += "</div>"; 
            }
            else
            {
                //Table 3 
                retValue += "<div style='width: 50%; max-width: 1200px; margin: 0 auto; '>";
                retValue += String.Format("<table style='width: 100%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
                retValue += String.Format(template, "ID", "GO" + (m_GOObj.Id));
                retValue += String.Format(template, "Vendor Name", m_GOObj.vendor_name ?? "N/A");
                retValue += String.Format(template, "PO No.", m_GOObj.po_no);
                retValue += String.Format(template, "Amount", $"{m_GOObj.amount} [{m_GOObj.currency}]");
                retValue += String.Format(template, "PO Date", m_GOObj.po_date);
                retValue += "</table>";
                retValue += "</div>";
            }

            // Table 4
            retValue += "<div style='width: 50%; max-width: 1200px; margin: 0 auto; margin-top: 1cm;'>";
            retValue += String.Format("<table style='width: 100%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
            if (!string.IsNullOrEmpty(m_GOObj.submitted_by_name))
            {
                retValue += String.Format(template, "Submitted By", m_GOObj.submitted_by_name);
                retValue += String.Format(template, "Submitted At", m_GOObj.submitted_at);
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
            if (!string.IsNullOrEmpty(m_GOObj.go_updated_by_name))
            {
                retValue += String.Format(template, "Updated By", m_GOObj.go_updated_by_name);
                retValue += String.Format(template, "Updated At", m_GOObj.go_updatedOn);
            }
            retValue += "</table>";
            retValue += "</div>";

            retValue += "<br><br>";


            if (check1 || check2 || check3 || check4 == true)
            {
                // GO Items Table
                retValue += "<h4>Selected Material</h4>";
                retValue += "<table style='width: 100%; margin:0 auto; border-collapse: collapse; border-spacing: 10px; table-layout: auto;' border='1'>";
                retValue += "<tr>";
                retValue += "<th>Material</th>";
                retValue += "<th>Requested Quantity</th>";
                retValue += "<th>Unit Cost</th>";
                retValue += "<th>Dispatch Quantity</th>";
                retValue += "<th>Paid By</th>";
                retValue += "<th>Receive Quantity</th>";

                
                if (m_GOObj.GODetails.Any(item => item.asset_type == "Spare"))
                {
                    retValue += "<th>Spare Sr. No.</th>";
                }

                retValue += "<th>Accepted Quantity</th>";
                retValue += "<th>Damamaged Items</th>";
                //retValue += "<th>Rack No.</th>";
                //retValue += "<th>Row No.</th>";
                //retValue += "<th>Column No.</th>";
                retValue += "</tr>";

                foreach (var item in m_GOObj.GODetails)
                {
                    retValue += "<tr>";
                    retValue += String.Format("<td>{0}</td>", item.assetItem_Name);
                    retValue += String.Format("<td>{0}</td>", item.requested_qty);
                    retValue += String.Format("<td>{0}</td>", item.cost);
                    retValue += String.Format("<td>{0}</td>", item.ordered_qty);
                    retValue += String.Format("<td>{0}</td>", item.paid_by_name);
                    retValue += String.Format("<td>{0}</td>", item.received_qty);

                    if (item.asset_type == "Spare")
                    {
                        retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.sr_no);
                    }

                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.accepted_qty);
                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.damaged_qty);
                    //retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.storage_rack_no);
                    //retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.storage_row_no);
                    //retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.storage_column_no);
                    retValue += "</tr>";
                }
                retValue += "</table><br><br>";
            }
            else
            {
                retValue += "<h4>Selected Material</h4>";
                retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
                retValue += "<tr>";
                retValue += "<th>Material</th>";
                retValue += "<th>Requested Quantity</th>";
                retValue += "<th>Unit Cost</th>";
                retValue += "<th>Dispatch Quantity</th>";
                retValue += "<th>Paid By</th>";

                foreach (var item in m_GOObj.GODetails)
                {
                    retValue += "<tr>";
                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.assetItem_Name);
                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.requested_qty);
                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.cost);
                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.ordered_qty);
                    retValue += String.Format("<td style='word-wrap: break-word;'>{0}</td>", item.paid_by_name);
                }
                retValue += "</table><br><br>";

            }


            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue += String.Format(templateEnd, "Updated By", m_GOObj.go_updated_by_name);
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
