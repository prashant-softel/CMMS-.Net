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
                    retValue += String.Format("{0} RO{1} Drafted by {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue += String.Format("{0} RO{1} Submitted By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_UPDATED:
                    retValue += String.Format("{0} RO{1} Updated By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.updated_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue += String.Format("{0} RO{1} Rejected By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue += String.Format("{0} RO{1} Approved By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue += String.Format("{0} RO{1} Closed By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.closed_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue += String.Format("{0} RO{1} Closed By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj);
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
                    retValue = String.Format("{0} RO{1} Drafted by {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue = String.Format("{0} RO{1} Submitted By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_UPDATED:
                    retValue = String.Format("{0} RO{1} Updated By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.updated_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue = String.Format("{0} RO{1} Rejected By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue = String.Format("{0} RO{1} Approved By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = String.Format("{0} RO{1} Closed By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj.closed_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = String.Format("{0} RO{1} Closed By {2}", m_SMROObj.facilityName, m_SMROObj.request_order_id, m_SMROObj);
                    break;
                default:
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

                retValue = String.Format("<table style='width: 50%; margin: 0 auto; border-collapse: collapse; border-spacing: 10px'><tr><td style='white-space: nowrap;'><h3><b style='color:#31576D'>Status : </b>{0}</h3></td></tr></table>", m_SMROObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "ID","RO"+m_SMROObj.request_order_id);    
                retValue += String.Format(template, "Facility Name", m_SMROObj.facilityName);
                retValue += String.Format(template, "Comment", m_SMROObj.comment);
                retValue += String.Format(template, "Status", m_SMROObj.status_short);


            if (!string.IsNullOrEmpty(m_SMROObj.generatedBy))
            {
                retValue += String.Format(template, "Submitted By", m_SMROObj.generatedBy + " at " + m_SMROObj.generatedAt);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.approvedBy))
            {
                retValue += String.Format(template, "Approved By", m_SMROObj.approvedBy + " at " + m_SMROObj.approvedAt);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.rejectedBy))
            {
                retValue += String.Format(template, "Rejected By", m_SMROObj.rejectedBy + " at " + m_SMROObj.rejectedAt);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.closed_by))
            {
                retValue += String.Format(template, "Closed By", m_SMROObj.closed_by + " at " + m_SMROObj.closed_at);
            }
            if (!string.IsNullOrEmpty(m_SMROObj.updated_by))
            {
                retValue += String.Format(template, "Updated By", m_SMROObj.updated_by + " at " + m_SMROObj.updated_by);
            }

            retValue += "</table><br><br>";

            // Request Order Item Table
            retValue += "<h4 style='text-align:center;'>Selected Materials</h4>";
            retValue += "<table style='width: 60%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
            retValue += "<tr>";
            retValue += "<th>Item Name</th>";
            retValue += "<th>Currency</th>";
            retValue += "<th>Unit Cost</th>";
            retValue += "<th>Requested Quantity</th>";
            retValue += "<th>Comment</th>";
            retValue += "</tr>";

            foreach (var item in m_SMROObj.request_order_items)
            {
                retValue += "<tr>";
                retValue += String.Format("<td>{0}</td>", item.asset_name);
                retValue += String.Format("<td>{0}</td>", item.currency);
                retValue += String.Format("<td>{0}</td>", item.cost);
                retValue += String.Format("<td>{0}</td>", item.ordered_qty);
                retValue += String.Format("<td>{0}</td>", item.comment);
                retValue += "</tr>";
            }

            retValue += "</table><br><br>";

            retValue += "<div style='text-align:center;'>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue += "</table>";
                    retValue += String.Format(templateEnd, "Submitted By", m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue += String.Format(templateEnd, "Approved By", m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_UPDATED:
                    retValue += String.Format(templateEnd, "Updated By", m_SMROObj.updated_by);
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

