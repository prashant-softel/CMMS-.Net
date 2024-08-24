using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class PTWNotification : CMMSNotification
    {
        int m_permitId;
        CMPermitDetail m_permitObj;
        public PTWNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPermitDetail ptwObj) : base(moduleID, notificationID)
        {
            m_permitObj = ptwObj;
            m_permitId = m_permitObj.insertedId;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATE : ";
            m_permitId = m_permitObj.insertedId;
            string desc = m_permitObj.description;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue += String.Format("PTW{0} <{1}> requested by  <{2}>", m_permitId, desc, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = String.Format("PTW{0} <{1}> issued by <{2}>", m_permitObj.permitNo, desc, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = String.Format("PTW{0} <{1}> Rejected By <{2}>", m_permitObj.permitNo, desc, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    retValue = String.Format("PTW{0} <{1}> Approved By <{2}>", m_permitObj.permitNo, desc, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    retValue = String.Format("PTW{0} <{1}> Rejected By <{2}>", m_permitObj.permitNo, desc, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    retValue = String.Format("PTW{0} <{1}> Closed By <{2}>", m_permitObj.permitNo, desc, m_permitObj.closedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = String.Format("PTW{0} <{1}> Cancelled by Issuer <{2}> ", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = String.Format("PTW{0} <{1}> Cancelled by HSE <{2}> ", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("PTW{0} <{1}> Cancelled by approver <{2}> ", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested by <{2}>", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Request Approve by <{2}>", m_permitObj.permitNo, desc, m_permitObj.cancelRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Rejected by <{2}>", m_permitObj.permitNo, desc, m_permitObj.cancelRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("PTW{0} <{1}> Extend Requested By <{2}>", m_permitObj.permitNo, desc, m_permitObj.extendRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Approve by <{2}>", m_permitObj.permitNo, desc, m_permitObj.extendRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Rejected by <{2}>", m_permitObj.permitNo, desc, m_permitObj.extendRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("PTW{0} <{1}> Linked to Job", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("PTW{0} <{1}> Linked to PM Permit", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("PTW{0} <{1}> Linked to Audit", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("PTW{0} <{1}> Linked to Hoto", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("PTW{0} <{1}> Expired", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    retValue = String.Format("PTW{0} <{1}> Updated", m_permitObj.permitNo, desc);
                    break;
                default:
                    retValue = String.Format("PTW{0} <{1}> Unknow status <{3}>", m_permitObj.permitNo, desc, m_notificationID);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;
        }


        override protected string getSubject(params object[] args)
        {
            string retValue = "My permit subject";
            m_permitId = m_permitObj.insertedId;
            string desc = m_permitObj.title;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("PTW{0} <{1}> requested by ", m_permitId, desc, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = String.Format("PTW{0} <{1}> issued by <{2}>", m_permitObj.permitNo, desc, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = String.Format("PTW{0} <{1}> Rejected By <{2}>", m_permitObj.permitNo, desc, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    retValue = String.Format("PTW{0} <{1}> Approved By <{2}>", m_permitObj.permitNo, desc, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    retValue = String.Format("PTW{0} <{1}> Rejected By <{2}>", m_permitObj.permitNo, desc, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    retValue = String.Format("PTW{0} <{1}> Closed By <{2}>", m_permitObj.permitNo, desc, m_permitObj.closedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = String.Format("PTW{0} <{1}> cancelled by Issuer <{2}> ", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = String.Format("PTW{0} <{1}> cancelled by HSE <{2}> ", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("PTW{0} <{1}> cancelled by approver <{2}> ", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested by <{2}>", m_permitObj.permitNo, desc, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Approve by <{2}>", m_permitObj.permitNo, desc, m_permitObj.cancelRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Rejected by <{2}>", m_permitObj.permitNo, desc, m_permitObj.cancelRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("PTW{0} <{1}> Extend Requested By <{2}>", m_permitObj.permitNo, desc, m_permitObj.extendRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Approve by <{2}>", m_permitObj.permitNo, desc, m_permitObj.extendRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("PTW{0} <{1}> Cancel Requested Rejected by <{2}>", m_permitObj.permitNo, desc, m_permitObj.extendRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("PTW{0} <{1}> Linked to Job", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("PTW{0} <{1}> Linked to PM Permit", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("PTW{0} <{1}> Linked to Audit", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("PTW{0} <{1}> Linked to Hoto", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("PTW{0} <{1}> Expired", m_permitObj.permitNo, desc);
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    retValue = String.Format("PTW{0} <{1}> Updated", m_permitObj.permitNo, desc);
                    break;
                default:
                    retValue = String.Format("PTW{0} <{1}> Unknow status <{3}>", m_permitObj.permitNo, desc, m_notificationID);
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int permitNo = m_permitObj.permitNo; //jobid
            var permitName = m_permitObj.PermitTypeName;//job title
            var permitDesc = m_permitObj.description;//job desc
            string ptw_cancelled_name = (string)m_permitObj.cancelRequestByName;
            string ptw_issued = (string)m_permitObj.issuedByName;
            string ptw_approve_name = (string)m_permitObj.approvedByName;
            string ptw_closed_name = (string)m_permitObj.closedByName;

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_permitObj.current_status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", m_permitObj.permitNo);
            retValue += String.Format(template, "Status", m_permitObj.current_status_short);
            retValue += String.Format(template, "PTW Title", m_permitObj.PermitTypeName);
            retValue += String.Format(template, "PTW Description", m_permitObj.description);
            retValue += String.Format(template, "Created By", m_permitObj.requestedByName);
            if (m_permitObj.rejecter_id > 0)
            {
                retValue += String.Format(templateEnd, "Rejected By", m_permitObj.rejectedByName);
            }
            if (m_permitObj.approver_id > 0)
            {
                retValue += String.Format(templateEnd, "Approved By", m_permitObj.approvedByName);
            }

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_ISSUED:     //Assigned
                    retValue += String.Format(templateEnd, "Issued By", m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:     //Linked to PTW
                    retValue += String.Format(template, "Linked Job", m_permitObj.job_type_id);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:     //Linked to PTW                   
                    retValue += String.Format(templateEnd, "Linked PM", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:     //Linked to PTW
                    retValue += String.Format(templateEnd, "Linked Audit", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:     //Linked to PTW
                    retValue += String.Format(templateEnd, "Linked HOTO", m_permitObj.permitNo);
                    break;
                default:
                    break;
            }

            if (m_permitObj.cancelRequestby_id > 0)
            {
                retValue += String.Format(templateEnd, "Extend Requested By", m_permitObj.requestedByName);
                retValue += String.Format(templateEnd, "Extend Request Approved By", m_permitObj.approvedByName);
                retValue += String.Format(templateEnd, "Extend Request Rejected by", m_permitObj.approvedByName);
            }
            if (m_permitObj.cancelRequestby_id > 0)
            {
                retValue += String.Format(templateEnd, "Cancel Requested By", m_permitObj.cancelRequestByName);
                retValue += String.Format(templateEnd, "Cancel Request Approved By", m_permitObj.cancelRequestApprovedByName);
                retValue += String.Format(templateEnd, "Cancel Request Rejected by", m_permitObj.cancelRequestRejectedByName);
            }

            if (m_permitObj.closedby_id > 0)
            {
                retValue += String.Format(templateEnd, "Closed By", m_permitObj.closedByName);
            }

            retValue += "</table>";

            return retValue;
        }
    }
}
