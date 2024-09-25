using CMMSAPIs.Helper;
using CMMSAPIs.Models.WC;
using System;

namespace CMMSAPIs.Models.Notifications
{
    internal class WCNotification : CMMSNotification
    {
        CMWCDetail WCObj;

        public WCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMWCDetail wcObj) : base(moduleID, notificationID)
        {
            WCObj = wcObj;
            m_module_ref_id = WCObj.wc_id;
        }
        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATE : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_DRAFT:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue += String.Format("{0} WC{1} is in Draft", WCObj.facility_name, WCObj.wc_id);
                    break;
                case CMMS.CMMS_Status.WC_SUBMITTED:     //Assigned
                    retValue += String.Format("{0} WC{1} is in Submitted", WCObj.facility_name, WCObj.wc_id);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:     //Linked
                    retValue += String.Format("{0} WC{1} is in Submit Rejected", WCObj.facility_name, WCObj.wc_id);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:     //Closed
                    retValue += String.Format("{0} WC{1} is in Submit Approved", WCObj.facility_name, WCObj.wc_id);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED:            //Closed
                    retValue += String.Format("{0} WC{1} is in submitted to close", WCObj.facility_name, WCObj.wc_id);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:     //Closed-Rejected
                    retValue += String.Format("{0} WC{1} is in Close Rejected", WCObj.facility_name, WCObj.wc_id);
                    break;
                default:
                    retValue += String.Format("{0} WC{1} Undefined status {1} ", WCObj.facility_name, WCObj.wc_id);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }

        protected override string getURL(params object[] args)
        {
            return $"{m_baseURL}/purchaseGoodsorder-detail/{m_module_ref_id}";
        }

        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_DRAFT:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("{0} WC{1} Draft created by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.WC_SUBMITTED:     //Assigned
                    retValue = String.Format("{0} WC{1} Submitted by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:     //Linked
                    retValue = String.Format("{0} WC{1}  claim Submit Rejected by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:     //Closed
                    retValue = String.Format("{0} WC{1}  claim Submit Approved by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.approver_name);
                    break;
/*                case CMMS.CMMS_Status.WC_DISPATCHED:     //Cancelled
                    retValue = String.Format("{0} WC{1}  Dispached", WCObj.wc_id, WCObj.facility_name, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:     //Cancelled
                    retValue = String.Format("{0} WC{1}  Rejected by Manufacturer", WCObj.facility_name, WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:     //Cancelled
                    retValue = String.Format("{0} WC{1}  Approved by Manufacturer", WCObj.facility_name, WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:     //Cancelled
                    retValue = String.Format("{0} WC{1}  Item Replenished", WCObj.facility_name, WCObj.wc_id, WCObj.warranty_claim_title);
                    break;*/
                case CMMS.CMMS_Status.WC_CLOSED:     //Closed
                    retValue = String.Format("{0} WC{1} claim Close request submitted by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:     //Cancelled
                    retValue = String.Format("{0} WC{1} claim Close request Rejected by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.closed_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:     //Cancelled
                    retValue = String.Format("{0} WC{1} claim Close request Approved by {2}", WCObj.facility_name, WCObj.wc_id, WCObj.closed_approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CANCELLED:     //Cancelled
                    retValue = String.Format("{0} WC{1} claim Cancelled", WCObj.facility_name, WCObj.wc_id);
                    break;
                case CMMS.CMMS_Status.WC_UPDATED:
                    retValue += String.Format("{0} WC{1} claim Updated By {2}", WCObj.facility_name, WCObj.wc_id, WCObj.updatedbyIdName);
                    break;
                default:
                    retValue = String.Format("{0} WC{1} claim submitted unsupported status {2}", WCObj.facility_name, WCObj.wc_id, m_notificationID);
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", WCObj.status_long + " at " + WCObj.facility_name);
            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", "WC" + WCObj.wc_id);
            retValue += String.Format(template, "Status", WCObj.status_short);
            retValue += String.Format(template, "Warranty Claim Title", WCObj.warranty_claim_title);
            retValue += String.Format(template, "Warranty Claim Description", WCObj.warranty_description);
            retValue += String.Format(template, "Equipment name", WCObj.equipment_name + "(" + WCObj.equipment_category + ")");
            retValue += String.Format(template, "Cost of replacement", WCObj.cost_of_replacement);
            retValue += String.Format(template, "Estimated loss", WCObj.estimated_cost);
            retValue += String.Format(template, "Manufacture name", WCObj.manufacture_name);
            retValue += String.Format(template, "Supplier name", WCObj.supplier_name);
            retValue += String.Format(template, "Warranty Start Date", WCObj.warrantyStartDate);
            retValue += String.Format(template, "Warranty End Date", WCObj.warrantyEndDate);

            retValue += String.Format(template, "Created By", WCObj.created_by_name + " at " + WCObj.created_at);

            if (WCObj.rejected_by > 0)
                retValue += String.Format(template, "Warranty Claim Rejected By ", WCObj.rejected_by_name + " at " + WCObj.rejected_at);
            if (WCObj.approved_by > 0)
                retValue += String.Format(template, "Warranty Claim Approved By ", WCObj.approver_name + " at " + WCObj.approved_at);
            if (WCObj.cancelled_by > 0)
                retValue += String.Format(template, "Warranty Claim Cancelled By ", WCObj.cancelled_by_name + " at " + WCObj.created_at);
            if(WCObj.closed_by > 0)
                retValue += String.Format(template, "Warranty Claim Closed By ", WCObj.closed_by_name + " at " + WCObj.closed_at);
            if (WCObj.closed_rejected_by > 0)
                retValue += String.Format(template, "Warranty Claim Closed Rejected By ", WCObj.closed_rejected_by_name + " at " + WCObj.closed_rejected_at);
            if (WCObj.closed_approved_by > 0)
                retValue += String.Format(template, "Warranty Claim Closed Approved By ", WCObj.closed_approver_name + " at " + WCObj.closed_approved_at);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_UPDATED:
                    retValue += String.Format(template, "Warranty Claim Updated By ", WCObj.updatedbyIdName + " at " + WCObj.last_updated_at);
                    break;
                default:
                    retValue += String.Format(template, "Warranty Claim Undefined Status for ", m_notificationID);
                    break;
            }

            retValue += "</table>";
            return retValue;
        }
    }
}
