using CMMSAPIs.Helper;
using CMMSAPIs.Models.WC;
using System;

namespace CMMSAPIs.Models.Notifications
{
    internal class WCNotification : CMMSNotification
    {
        int WCId;
        CMWCDetail WCObj;

        public WCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMWCDetail wcObj) : base(moduleID, notificationID)
        {
            WCObj = wcObj;
            WCId = WCObj.wc_id;
        }
        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATE : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_DRAFT:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue += String.Format("WC{0} <{1}> is in Draft", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMITTED:     //Assigned
                    retValue += String.Format("WC{0} <{1}> is in Submitted", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:     //Linked
                    retValue += String.Format("WC{0} <{1}> is in Submit Rejected  ", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:     //Closed
                    retValue += String.Format("WC{0} <{1}> is in Submit Approved", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED:            //Closed
                    retValue += String.Format("WC{0} <{1}> is in Close", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:     //Closed-Rejected
                    retValue += String.Format("WC{0}  <{1}> is in Close Rejected", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                default:
                    retValue += String.Format("WC{0} <{1}> Undefined status {1} ", WCObj.wc_id, WCObj.warranty_claim_title);
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
                case CMMS.CMMS_Status.WC_DRAFT:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("WC{0} <{1}> Draft", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMITTED:     //Assigned
                    retValue = String.Format("WC{0} <{1}> Submitted", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:     //Linked
                    retValue = String.Format("WC{0} <{1}>  claim Submit Rejected  ", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:     //Closed
                    retValue = String.Format("WC{0} <{1}>  claim Submit Approved", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_DISPATCHED:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  Dispached", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  Rejected by Manufacturer", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  Approved by Manufacturer", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  Item Replenished", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED:     //Closed
                    retValue = String.Format("WC{0} <{1}>  claim Close waiting for approval", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  claim Close Rejected", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  claim Close Approved", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CANCELLED:     //Cancelled
                    retValue = String.Format("WC{0} <{1}>  claim Cancelled", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";


            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", WCObj.status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", "WC" + WCObj.wc_id);
            retValue += String.Format(template, "Status", WCObj.status_short);
            retValue += String.Format(template, "Warranty Claim Title", WCObj.warranty_claim_title);
            retValue += String.Format(template, "Warranty Claim  Description", WCObj.warranty_description);
            retValue += String.Format(template, "Created By", WCObj.created_by_name);

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_SUBMITTED:
                    retValue += "</table>"; break;
                /*case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                    retValue += String.Format(templateEnd, "PM Task Linked By", m_pmExecutionObj.updated_by_name);
                    break;*/
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:
                    retValue += String.Format(templateEnd, "Warranty Claim Rejected By ", WCObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:
                    retValue += String.Format(templateEnd, "Warranty Claim Approved By ", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED:
                    retValue += String.Format(templateEnd, "Warranty Claim Closed By ", WCObj.closed_approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:
                    retValue += String.Format(templateEnd, "Warranty Claim Closed Rejected By ", WCObj.closed_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:
                    retValue += String.Format(templateEnd, "Warranty Claim Closed Approved By ", WCObj.closed_approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CANCELLED:
                    retValue += String.Format(templateEnd, "Warranty Claim Cancelled By ", WCObj.cancelled_by_name);
                    break;
                case CMMS.CMMS_Status.WC_UPDATED:
                    retValue += String.Format(templateEnd, "Warranty Claim Updated By ", WCObj.updatedbyIdName);
                    break;
                default:
                    retValue += String.Format(templateEnd, "Warranty Claim Undefined Status for ", m_notificationID);
                    break;

            }

            return retValue;
        }


    }
}
