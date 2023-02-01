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

        override protected string getSubject(params object[] args)
        {
            string retValue = "My permit subject";
            m_permitId = m_permitObj.insertedId;
            string desc = m_permitObj.description;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("Permit <{0}><{1}> created", m_permitId, desc);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:     //ptw issued
                    retValue = String.Format("Permit <{0}> Issued to <{1}><{2}>", m_permitObj.permitNo, m_permitObj.issuedByName, desc);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:   // ptw reject by issuer  
                    retValue = String.Format("Permit <{0}> Rejected By Issuer <{1}><{2}>", m_permitObj.permitNo, m_permitObj.cancelRequestByName, desc);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //permit Approve
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:     //ptw reject by approve
                    retValue = String.Format("Permit <{0}> Rejected By Approver <{1}><{2}>", m_permitObj.permitNo, m_permitObj.approvedByName,desc);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED: //permit closed ask by name
                    retValue = String.Format("Permit <{0}> Closed<{1}>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER: // permit cancel by issuer
                    retValue = String.Format("Permit <{0}> Cancelled By Issuer <{1}><{2}>", m_permitObj.permitNo, m_permitObj.cancelRequestByName, desc);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLLED_BY_HSE:
                    retValue = String.Format("Pemit <{0}> cancelled by HSE<{1}>>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("Permit <{0}> cancelled by approver <{1}> <{2}>", m_permitObj.permitNo, m_permitObj.approvedByName,desc);
                    break;
                case CMMS.CMMS_Status.PTW_EDIT: //update ptw edited by name
                    retValue = String.Format("Permit <{0}> Updated <{1}><{2}>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("Permit <{0}> Extend Requested By<{1}><{2}>", m_permitObj.permitNo, m_permitObj.issuedByName,desc);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("Permit <{0}> Extend Request Approve <{1}><{2}>", m_permitObj.permitNo, m_permitObj.approvedByName,desc);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("Permit <{0}> Extend Request Rejected <{1}><{2}>", m_permitObj.permitNo, m_permitObj.cancelRequestByName,desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:  
                    retValue = String.Format("Permit <{0}> Linked to Job <{1}>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("Permit <{0}> Linked to PM <{1}>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("Permit <{0}> Linked to Audit<{1}>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("Permit <{0}> Linked to Hoto <{1}>", m_permitObj.permitNo,desc);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("Permit <{0}> Expired <{1}>", m_permitObj.permitNo,desc);
                    break;
                default:
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

            var template = getHTMLBodyTemplate(args);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:     //Created
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:     //Assigned
                    retValue = String.Format(template, permitName, ptw_issued, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:     //Closed
                    retValue = String.Format(template, permitName, ptw_cancelled_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_approve_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_approve_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_issued, ptw_cancelled_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLLED_BY_HSE:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_approve_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_EDIT:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_issued, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_approve_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:     //Linked to PTW
                    retValue = String.Format(template, permitName, ptw_cancelled_name, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:     //Linked to PTW
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                default:
                    break;
            }

            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = String.Format("<h1>This is Permit Title {0}</h1>",m_permitObj.description);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:     //Created
                    template += String.Format("<p><b>Permit status is :</b> Created</p> Permit No {0}",m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:     //Assigned
                    template += String.Format("<p><b>Pemit status is : Issued</p>");
                    template += String.Format("<p><b>Permit Issued to:</b> {0}</p>",m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:     //Closed
                    template += String.Format("<p><b>Permit status is : Permit Rejected by issuer</p>");
                    template += String.Format("<p><b>Permit Rejected by issuer :</b> {0}</p>",m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    template += String.Format("<p><b>Permit status is : Permit approve </p>");
                    template += String.Format("<p>Permit Approve By:</b> {0}</p>",m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:     //Linked to PTW
                    template += String.Format("<p><b>Permit status is : Permit rejected by approve </p>");
                    template += String.Format("<p>Permit rejected Approve By:</b> {0}</p>", m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:     //Linked to PTW
                    template += String.Format("<p><b>Permit status is : Permit closed </p>");
                    template += String.Format("<p>Permit {0} is closed :</b> {0}</p>", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:     
                    template += String.Format("<p><b>Permit status is : Permit cancelled by issuer </p>");
                    template += String.Format("<p>Permit cancelled issuer By:</b> {0}</p>", m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLLED_BY_HSE:
                    template += String.Format("<p><b>Permit status is : Permit cancelled by HSE </p>");
                    template += String.Format("<p>Permit cancelled HSE By:</b> {0}</p>");
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    template += String.Format("<p><b>Permit status is : Permit cancelled by Approver </p>");
                    template += String.Format("<p>Permit cancelled By approver :</b> {0}</p>", m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EDIT:
                    template += String.Format("<p><b>Permit status is : Permit Updated </p>");
                    template += String.Format("<p>Permit No <b> {0}</b> is Updated</p>", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    template += String.Format("<p><b>Permit status is : Permit Extend Requested </p>");
                    template += String.Format("<p>Permit :<b> {0}</b> Extend Request</p>", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    template += String.Format("<p><b>Permit status is : Permit Extend Request Approved </p>");
                    template += String.Format("<p>Permit :<b> {0}</b> Extend Request approved by {1} </p>", m_permitObj.permitNo,m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    template += String.Format("<p><b>Permit status is : Permit Extend Request Rejected </p>");
                    template += String.Format("<p>Permit :<b> {0}</b> Extend Request rejected by {1} </p>", m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    template += String.Format("<p><b>Permit status is : Permit linked to job </p>");
                    template += String.Format("<p>Permit :<b> {0}</b> Linked {1} by job </p>", m_permitObj.permitNo,m_permitObj.insertedId);
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
                    template += String.Format("<p>Permit :<b> {0}</b> Expired>", m_permitObj.permitNo);
                    break;
                default:
                    break;
            }
            template += String.Format("<p><B>Permit description: </b>{0}</p>",m_permitObj.description);
            return template;
        }
    }
}
