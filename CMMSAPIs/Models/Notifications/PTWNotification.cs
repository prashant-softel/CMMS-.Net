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
            m_permitId = m_permitObj.id;
        }

        override protected string getSubject(params object[] args)
        {
            string retValue = "My permit subject";
            m_permitId = m_permitObj.id;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    string desc = m_permitObj.description;
                    retValue = String.Format("Permit <{0}><{1}> created", m_permitId, desc);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:     //ptw issued
                    retValue = String.Format("Permit <{0}> Issued to <{1}>", m_permitObj.permitNo, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:   // ptw reject by issuer  
                    retValue = String.Format("Permit <{0}> Rejected By Issuer <{1}>", m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //permit Approve
                    retValue = String.Format("Permit <{0}> Approved By <{1}>", m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:     //ptw reject by approve
                    retValue = String.Format("Permit <{0}> Rejected By Approver <{1}>", m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED: //permit closed
                    retValue = String.Format("Permit <{0}> Closed", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER: // permit cancel by issuer
                    retValue = String.Format("Permit <{0}> Cancelled By Issuer <{1}>", m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLLED_BY_HSE:
                    retValue = String.Format("Pemit <{0}> cancelled by HSE>", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("Permit <{0}> cancelled by approver <{1}>", m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EDIT: //update ptw
                    retValue = String.Format("Permit <{0}> Updated", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("Permit <{0}> Extend Requested By<{1}>", m_permitObj.permitNo, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("Permit <{0}> Extend Request Approve <{1}>", m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("Permit <{0}> Extend Request Rejected <{1}>", m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("Permit <{0}> Linked to Job ", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("Permit <{0}> Linked to PM", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("Permit <{0}> Linked to Audit", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("Permit <{0}> Linked to Hoto", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("Permit <{0}> Expired", m_permitObj.permitNo);
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
            var template = getHTMLBodyTemplate(args);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:     //Created
                    retValue = String.Format(template, permitName, permitDesc);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:     //Assigned
                    string ptw_issued = (string)m_permitObj.issuedByName;
                    retValue = String.Format(template, permitName, ptw_issued);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:     //Closed
                    string ptw_rejeceted_name = (string)m_permitObj.cancelRequestByName;
                    retValue = String.Format(template, permitName, ptw_rejeceted_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:     //Linked to PTW
                    string ptw_rejected_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_rejected_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:     //Linked to PTW
                    string ptw_approve_name = (string)m_permitObj.approvedByName;
                    retValue = String.Format(template, permitName, ptw_approve_name);
                    break;
                default:
                    break;
            }

            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = "<h1>This is Job Title {0}</h1>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    template += "<p><b>Job status is :</b> Created</p>";
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    template += "<p><b>Job status is : Assiged</p>";
                    template += "<p><b>Job assigned to:</b> {1}</p>";
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    template += "<p><b>Job status is : Assiged</p>";
                    template += "<p><b>Job assigned to:</b> {1}</p>";
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked to PTW
                    template += "<p><b>Job status is : Assigned to PTW ID {3} PTW Desc {4]}</p>";
                    template += "<p>Job assigned to:</b> {1}</p>";
                    break;
                default:
                    break;
            }
            template += "<p><B>Job description: </b>{1}</p>";
            return template;
        }
    }
}
