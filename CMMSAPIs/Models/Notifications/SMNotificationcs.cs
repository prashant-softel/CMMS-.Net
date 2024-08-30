using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Notifications
{
    internal class SMNotification : CMMSNotification
    {
        int m_SMROObjID;
        CMCreateRequestOrderGET m_SMROObj;

        public SMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMCreateRequestOrderGET SMROObj) : base(moduleID, notificationID)
        {
            m_SMROObj = SMROObj;
            m_SMROObjID = SMROObj.request_order_id;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";
            m_SMROObjID = m_SMROObj.request_order_id;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_DRAFT:
                    retValue += String.Format("Request order{0} drafted by {1}", m_SMROObj.request_order_id, m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue += String.Format("Request order{0} submitted and waiting for approval by {1}", m_SMROObj.request_order_id, m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue += String.Format("Request order{0} Submitted but rejected by {1}", m_SMROObj.request_order_id, m_SMROObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue += String.Format("Request order{0} approved by {1}", m_SMROObj.request_order_id, m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue += String.Format("Request order{0} deleted by {1}", m_SMROObj.request_order_id ,m_SMROObj.deleted_by); 
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue += String.Format("Request order{0} closed by {1}", m_SMROObj.request_order_id, m_SMROObj.closed_by);
                    break;
                default:
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
                case CMMS.CMMS_Status.SM_RO_DRAFT:
                    retValue = String.Format("RO{0} Drafted by {1}", m_SMROObj.request_order_id, m_SMROObj);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue = String.Format("RO{0} Submitted by {1}", m_SMROObj.request_order_id, m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue = String.Format("RO{0} Rejected By {1}", m_SMROObj.request_order_id, m_SMROObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue = String.Format("RO{0} Approved By {1}", m_SMROObj.request_order_id, m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = String.Format("RO{0} Deleted By {1}", m_SMROObj.request_order_id, m_SMROObj.deleted_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = String.Format("RO{0} Closed By {1}", m_SMROObj.request_order_id, m_SMROObj.closed_by);
                    break;
                default:
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_SMROObj.status_long);

            
            
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "ID","RO"+m_SMROObj.request_order_id);    
                retValue += String.Format(template, "Facility Name", m_SMROObj.facilityName);
                retValue += String.Format(template, "Cost", m_SMROObj.cost);
                retValue += String.Format(template, "Comment", m_SMROObj.comment);
                retValue += String.Format(template, "Status", m_SMROObj.status_short);


            if (!string.IsNullOrEmpty(m_SMROObj.generatedBy))
            {
                retValue += String.Format(template, "Submitted By", m_SMROObj.generatedBy);
                retValue += String.Format(template, "Submitted At", m_SMROObj.generatedAt);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.approvedBy))
            {
                retValue += String.Format(template, "Approved By", m_SMROObj.approvedBy);
                retValue += String.Format(template, "Approved At", m_SMROObj.approvedAt);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.rejectedBy))
            {
                retValue += String.Format(template, "Rejected By", m_SMROObj.rejectedBy);
                retValue += String.Format(template, "Rejected At", m_SMROObj.rejectedAt);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.closed_by))
            {
                retValue += String.Format(template, "Closed By", m_SMROObj.closed_by);
                retValue += String.Format(template, "Closed At", m_SMROObj.closed_at);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.deleted_by))
            {
                retValue += String.Format(template, "Deleted By", m_SMROObj.deleted_by);
                retValue += String.Format(template, "Deleted At", m_SMROObj.deleted_at);
            }

            retValue += "</table><br><br>";

            // Request Order Item Table
            retValue += "<h4>RO Items</h4>";
            retValue += "<table style='width: 60%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
            retValue += "<tr>";
            retValue += "<th>Item Name</th>";
            retValue += "<th>Number Of Items</th>";
            retValue += "<th>Ordered Quantity</th>";
            retValue += "<th>Accepted Quantity</th>";
            retValue += "</tr>";

            foreach (var item in m_SMROObj.request_order_items)
            {
                retValue += "<tr>";
                retValue += String.Format("<td>{0}</td>", item.asset_name);
                retValue += String.Format("<td>{0}</td>", m_SMROObj.number_of_item_count);
                retValue += String.Format("<td>{0}</td>", item.ordered_qty);
                retValue += String.Format("<td>{0}</td>", item.accepted_qty);              
                retValue += "</tr>";
            }

            retValue += "</table>";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue += "</table>";
                    retValue += String.Format(templateEnd, "Submitted By", m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue += String.Format(templateEnd, "Approved By", m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue += String.Format(templateEnd, "Rejected By", m_SMROObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue += String.Format(templateEnd, "Closed By", m_SMROObj.closed_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue += String.Format(templateEnd, "Deleted By", m_SMROObj.deleted_by);
                    break;
                    default:
                    break;
            }
            return retValue;
        }
    }
}

