using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class JCNotification : CMMSNotification
    {
        int m_jcId;
        CMJCDetail m_JCObj;
        public JCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJCDetail jcObj) : base(moduleID, notificationID)
        {
            m_JCObj = jcObj;
            m_jcId = m_JCObj.id;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATE : ";
            m_jcId = m_JCObj.id;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    retValue += String.Format("JC{0} for JOB{1} Created by <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.created_by);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:    //updated name 
                    retValue += String.Format("JC{0} of JOB{1} Started By <{1}> but not closed", m_JCObj.id, m_JCObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue += String.Format("JC{0} of JOB{1} Closed requested By <{2}> and waiting for approval", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue += String.Format("JC{0} of JOB{1} Carryforwarded By <{2}> and waiting for approval", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue += String.Format("JC{0} of JOB{1} Close Approved By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue += String.Format("JC{0} of JOB{1} Close request rejected By <{2}> and waiting for resubmit", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                    //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                    //    retValue = String.Format("JC{0} and Permit <{1}>Time out ", m_JCObj.id, m_JCObj.ptwId);
                    break;
            }
            retValue += $" for {m_delayDays} days";

            return retValue;

        }


        override protected string getSubject(params object[] args)
        {
            string retValue = "My Job Card subject";
            m_jcId = m_JCObj.id;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    int jcId = m_JCObj.id;
                    retValue = String.Format("JC{0} for JOB{1} Created by <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.created_by);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:    //updated name 
                    retValue = String.Format("JC{0} of JOB{1} Started By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue = String.Format("JC{0} of JOB{1} Carry forwarded By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("JC{0} of JOB{1} Carryforward request Approved By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = String.Format("JC{0} of JOB{1} Carryforward request  rejected By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = String.Format("JC{0} of JOB{1} Closed By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("JC{0} of JOB{1} Close request Approved By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = String.Format("JC{0} of JOB{1} Close request rejected By <{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                    //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                    //    retValue = String.Format("JC{0} and Permit <{1}>Time out ", m_JCObj.id, m_JCObj.ptwId);
                    break;
            }
            return retValue;

        }


        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_JCObj.status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", m_JCObj.id);
            retValue += String.Format(template, "Status", m_JCObj.status_short);
            retValue += String.Format(template, "Job ID", m_JCObj.jobid);
            retValue += String.Format(template, "PTW ID", m_JCObj.ptwId);
            retValue += String.Format(template, "Job Card Description", m_JCObj.description);
            retValue += String.Format(template, "Created By", m_JCObj.created_by);
            retValue += String.Format(template, "Created At", m_JCObj.created_at);

            if (m_JCObj.JC_Update_by > 0)
            {
                retValue += String.Format(template, "Updated By", m_JCObj.JC_UpdatedByName);
            }
            if (m_JCObj.JC_Start_By_id > 0)
            {
                retValue += String.Format(template, "Started By", m_JCObj.JC_Start_By_Name);
            }
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue += String.Format(template, "Carryforwarded By", m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:
                    retValue += String.Format(template, "Carryforwarded By", m_JCObj.JC_Closed_By_Name);
                    retValue += String.Format(template, "CF Approved By", m_JCObj.JC_Approved_By_Name);
                    retValue += String.Format(template, "CF Approval Reason", m_JCObj.JC_Approve_Reason);
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue += String.Format(template, "Carryforwarded By", m_JCObj.JC_Closed_By_Name);
                    retValue += String.Format(template, "CF Rejected By", m_JCObj.JC_Rejected_By_Name);
                    retValue += String.Format(template, "CF Rejection Reason", m_JCObj.JC_Rejected_Reason);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue += String.Format(template, "Closed By", m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:
                    retValue += String.Format(template, "Closed By", m_JCObj.JC_Closed_By_Name);
                    retValue += String.Format(template, "Close Approved By", m_JCObj.JC_Approved_By_Name);
                    retValue += String.Format(template, "Close Approval Reason", m_JCObj.JC_Approve_Reason);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue += String.Format(template, "Closed By", m_JCObj.JC_Closed_By_Name);
                    retValue += String.Format(template, "Close Rejected By", m_JCObj.JC_Rejected_By_Name);
                    retValue += String.Format(template, "Close Rejection Reason", m_JCObj.JC_Rejected_Reason);
                    break;
                default:
                    break;
            }
            retValue += "</table>";
            return retValue;
        }
    }
}
