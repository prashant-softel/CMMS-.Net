using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
using System;

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
            string facilityName = m_permitObj.siteName;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("{0} PTW{1} Requested by <{2}>", facilityName, m_permitId, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = String.Format("{0} PTW{1} Issued by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Rejected By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    retValue = String.Format("{0} PTW{1} Approved By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} Rejected By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    retValue = String.Format("{0} PTW{1} Closed By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.closedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Cancelled by Issuer <{2}> ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = String.Format("{0} PTW{1} Cancelled by HSE <{2}> ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} cancelled by approver <{2}> ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Extend Requested By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("{0} PTW{1} Linked to Job", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("{0} PTW{1} Linked to PM Permit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("{0} PTW{1} Linked to Audit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("{0} PTW{1} Linked to Hoto", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("{0} PTW{1} Expired", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    retValue = String.Format("{0} PTW{1} Updated", facilityName, m_permitObj.permitNo);
                    break;
                default:
                    retValue = String.Format("{0} PTW{1} Unknow status <{2}>", facilityName, m_permitObj.permitNo, m_notificationID);

                    break;
            }

            retValue += $" for {m_delayDays} days";
            return retValue;
        }


        override protected string getSubject(params object[] args)
        {
            string retValue = "My permit subject";
            m_permitId = m_permitObj.insertedId;
            string facilityName = m_permitObj.siteName;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("{0} PTW{1} Requested by <{2}>", facilityName, m_permitId, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = String.Format("{0} PTW{1} Issued by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Rejected By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    retValue = String.Format("{0} PTW{1} Approved By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} Rejected By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    retValue = String.Format("{0} PTW{1} Closed By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.closedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Cancelled by Issuer <{2}> ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = String.Format("{0} PTW{1} Cancelled by HSE <{2}> ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} cancelled by approver <{2}> ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Extend Requested By <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by <{2}>", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("{0} PTW{1} Linked to Job", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("{0} PTW{1} Linked to PM Permit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("{0} PTW{1} Linked to Audit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("{0} PTW{1} Linked to Hoto", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("{0} PTW{1} Expired", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    retValue = String.Format("{0} PTW{1} Updated", facilityName, m_permitObj.permitNo);
                    break;
                default:
                    retValue = String.Format("{0} PTW{1} Unknow status <{2}>", facilityName, m_permitObj.permitNo, m_notificationID);
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
            retValue += String.Format(template, "ID", "PTW" + m_permitObj.permitNo);
            retValue += String.Format(template, "Status", m_permitObj.current_status_long + " at " + m_permitObj.siteName);
            retValue += String.Format(template, "PTW Title", m_permitObj.PermitTypeName);
            retValue += String.Format(template, "PTW Description", m_permitObj.description);
            retValue += String.Format(template, "Created By", m_permitObj.requestedByName);
            if (m_permitObj.TBT_Done_By_id > 0)
            {
                retValue += String.Format(template, "TBT Done By", m_permitObj.TBT_Done_By + " at " + m_permitObj.TBT_Done_At);
            }
            if (m_permitObj.sop_type_id > 0)
            {
                retValue += String.Format(template, "SOP Name", m_permitObj.sop_type_name);
            }
            if (m_permitObj.rejecter_id > 0)
            {
                retValue += String.Format(template, "Rejected By", m_permitObj.rejectedByName);
            }
            if (m_permitObj.approver_id > 0)
            {
                retValue += String.Format(template, "Approved By", m_permitObj.approvedByName);
            }

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_ISSUED:     //Assigned
                    retValue += String.Format(template, "Issued By", m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:     //Linked to PTW
                    retValue += String.Format(template, "Linked Job", m_permitObj.job_type_id);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:     //Linked to PTW                   
                    retValue += String.Format(template, "Linked PM", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:     //Linked to PTW
                    retValue += String.Format(template, "Linked Audit", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:     //Linked to PTW
                    retValue += String.Format(template, "Linked HOTO", m_permitObj.permitNo);
                    break;
                default:
                    break;
            }

            if (m_permitObj.extendRequestby_id > 0)
            {
                retValue += String.Format(template, "Extend Requested By", m_permitObj.extendRequestByName);

                if (m_permitObj.extendRequestRejectedby_id > 0)
                {
                    retValue += String.Format(template, "Extend  Rejected by", m_permitObj.extendRequestRejectedByName);
                }
                if (m_permitObj.extendRequestApprovedby_id > 0)
                {
                    retValue += String.Format(template, "Extend Approved By", m_permitObj.extendRequestApprovedByName);
                }
            }

            if (m_permitObj.cancelRequestby_id > 0)
            {
                retValue += String.Format(template, "Cancel Requested By", m_permitObj.cancelRequestByName);
                if (m_permitObj.cancelRequestRejectedby_id > 0)
                {
                    retValue += String.Format(template, "Cancel Rejected by", m_permitObj.cancelRequestRejectedByName);
                }
                if (m_permitObj.cancelRequestApprovedby_id > 0)
                {
                    retValue += String.Format(template, "Cancel Approved By", m_permitObj.cancelRequestApprovedByName);
                }
            }
            if (m_permitObj.closedby_id > 0)
            {
                retValue += String.Format(template, "Closed By", m_permitObj.closedByName);
            }

            retValue += "</table>";

            return retValue;
        }
            internal string getHTMLBodyTemplate(params object[] args)
            {
                string template = String.Format("<h1>This is Permit Title {0}</h1>", m_permitObj.description);
                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.PTW_CREATED:
                        template += String.Format("<p><b>Permit status is :</b> Created</p> Permit No {0}", m_permitObj.permitNo);
                        break;
                    case CMMS.CMMS_Status.PTW_ISSUED:
                        template += String.Format("<p><b>Pemit status is : Issued</p>");
                        template += String.Format("<p>Permit {0} is Issued </p>", m_permitObj.permitNo);
                        template += String.Format("<p><b>Permit Issued to:</b> {0}</p>", m_permitObj.permitNo, m_permitObj.issuedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                        template += String.Format("<p><b>Permit status is : Permit Rejected by issuer</p>");
                        template += String.Format("<p>Permit {0} Rejected by issuer </p>", m_permitObj.permitNo);
                        template += String.Format("<p><b>Permit Rejected by issuer :</b> {0}</p>", m_permitObj.cancelRequestByName);
                        break;
                    case CMMS.CMMS_Status.PTW_APPROVED:
                        template += String.Format("<p><b>Permit status is : Permit approve </p>");
                        template += String.Format("<p>Permit {0} is closed </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Approve By:</b> {0}</p>", m_permitObj.approvedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                        template += String.Format("<p><b>Permit status is : Permit rejected by approver </p>");
                        template += String.Format("<p>Permit {0} rejected by approver </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit rejected By Approver Name :</b> {0}</p>", m_permitObj.approvedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_CLOSED:
                        template += String.Format("<p><b>Permit status is : Permit closed </p>");
                        template += String.Format("<p>Permit {0} is closed </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit closed by name:</b> {0}</p>", m_permitObj.closedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                        template += String.Format("<p><b>Permit status is : Permit cancelled by issuer </p>");
                        template += String.Format("<p>Permit {0} cancelled by issuer</p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit cancelled issuer By:</b> {0}</p>", m_permitObj.cancelRequestByName);
                        break;
                    case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                        template += String.Format("<p><b>Permit status is : Permit cancelled by HSE </p>");
                        template += String.Format("<p>Permit {0} is cancelled by HSE </p>", m_permitObj.permitNo);
                        break;
                    case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                        template += String.Format("<p><b>Permit status is : Permit cancelled by Approver </p>");
                        template += String.Format("<p>Permit {0} is cancelled by Approver</p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit cancelled By approver Name :</b> {0}</p>", m_permitObj.approvedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                        template += String.Format("<p><b>Permit status is : Permit Cancel Requested </p>");
                        template += String.Format("<p>Permit {0} is Cancel Requested </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Cancel Request by name <{0}></p>", m_permitObj.issuedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                        template += String.Format("<p><b>Permit status is : Permit Cancel Request Approved </p>");
                        template += String.Format("<p>Permit {0} is Cancel Request Approved </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Cancel Request approved by {0} </p>", m_permitObj.approvedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                        template += String.Format("<p><b>Permit status is : Permit Extend Request Rejected </p>");
                        template += String.Format("<p>Permit {0} is Extend Request Rejected</p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Extend Request rejected by Name {0}</p>", m_permitObj.cancelRequestByName);
                        break;
                    case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                        template += String.Format("<p><b>Permit status is : Permit Extend Requested </p>");
                        template += String.Format("<p>Permit {0} is Extend Requested </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Extend Request by name <{0}></p>", m_permitObj.issuedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                        template += String.Format("<p><b>Permit status is : Permit Extend Request Approved </p>");
                        template += String.Format("<p>Permit {0} is Extend Request Approved </p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Extend Request approved by {0} </p>", m_permitObj.approvedByName);
                        break;
                    case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                        template += String.Format("<p><b>Permit status is : Permit Extend Request Rejected </p>");
                        template += String.Format("<p>Permit {0} is Extend Request Rejected</p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit Extend Request rejected by Name {0}</p>", m_permitObj.cancelRequestByName);
                        break;
                    case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                        template += String.Format("<p><b>Permit status is : Permit linked to job </p>");
                        template += String.Format("<p>Permit {0} is linked to job</p>", m_permitObj.permitNo);
                        template += String.Format("<p>Permit :<b> {0}</b> Linked {1} by job </p>", m_permitObj.permitNo, m_permitObj.insertedId);
                        break;
                    case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                        template += String.Format("<p><b>Permit status is : Permit linked to PM </p>");
                        template += String.Format("<p>Permit :<b> {0}</b> Linked to PM </p>", m_permitObj.permitNo);
                        break;
                    case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                        template += String.Format("<p><b>Permit status is : Permit linked to Audit </p>");
                        template += String.Format("<p>Permit :<b> {0}</b> Linked to Audit </p>", m_permitObj.permitNo);
                        break;
                    case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                        template += String.Format("<p><b>Permit status is : Permit linked to HOTO </p>");
                        template += String.Format("<p>Permit :<b> {0}</b> Linked to HOTO </p>", m_permitObj.permitNo);
                        break;
                    case CMMS.CMMS_Status.PTW_EXPIRED:
                        template += String.Format("<p><b>Permit status is : Permit Expired </p>");
                        template += String.Format("<p>Permit :<b> {0}</b> Expired</p>", m_permitObj.permitNo);
                        break;
                    case CMMS.CMMS_Status.PTW_UPDATED:
                        template += String.Format("<p><b>Permit status is : Permit Updated </p>");
                        template += String.Format("<p>Permit :<b> {0}</b> Updated</p>", m_permitObj.permitNo);
                        break;
                    default:
                        break;
                }
                template += String.Format("<p><B>Permit description: </b>{0}</p>", m_permitObj.description);
                return template;

            }
        }
    }
