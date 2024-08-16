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
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_DRAFT:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("Warranty Claim <{0}><{1}> Draft", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMITTED:     //Assigned
                    retValue = String.Format("Warranty Claim <{0}><{1}> Submitted", WCObj.wc_id, WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:     //Linked
                    retValue = String.Format("Warranty Claim <{0}> Submit Rejected  ", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:     //Closed
                    retValue = String.Format("Warranty Claim <{0}> Submit Approved", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_DISPATCHED:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Dispached", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Rejected by Manufacturer", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Approved by Manufacturer", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Item Replenished", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Close Rejected", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Close Approved", WCObj.warranty_claim_title);
                    break;
                case CMMS.CMMS_Status.WC_CANCELLED:     //Cancelled
                    retValue = String.Format("Warranty Claim <{0}> Cancelled", WCObj.warranty_claim_title);
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
            retValue += String.Format(template, "ID", WCObj.wc_id);
            retValue += String.Format(template, "Status", WCObj.status_short);
            retValue += String.Format(template, "Warranty Claim Title", WCObj.warranty_claim_title);
            retValue += String.Format(template, "Warranty Claim  Description", WCObj.warranty_description);
            retValue += String.Format(template, "Created By", WCObj.created_by);

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_DRAFT:
                    retValue += "</table>"; break;
                case CMMS.CMMS_Status.WC_SUBMITTED:
                    retValue += String.Format(templateEnd, "Submitted By", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:
                    retValue += String.Format(templateEnd, "Rejected By", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:
                    retValue += String.Format(templateEnd, "Approved By", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_DISPATCHED:
                    retValue += String.Format(template, "Dispached By", WCObj.created_by);
                    retValue += String.Format(templateEnd, "Dispached At", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:
                    retValue += String.Format(templateEnd, "Rejected By", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:
                    retValue += String.Format(templateEnd, "Approved By", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:
                    retValue += String.Format(templateEnd, "Item Replenished At", WCObj.closed_at);
                    break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:
                    retValue += String.Format(templateEnd, "Close Rejected by", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:
                    retValue += String.Format(templateEnd, "Close Approved by", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CANCELLED:
                    retValue += String.Format(template, "Cancelled By", WCObj.created_by);
                    retValue += String.Format(templateEnd, "Cancelled At", WCObj.closed_at);
                    break;
                default:
                    break;
            }

            return retValue;
        }


    }
}
