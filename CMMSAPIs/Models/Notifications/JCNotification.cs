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

        override protected string getSubject(params object[] args)
        {
            string retValue = "My Job Card subject";
            m_jcId = m_JCObj.id;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    int jcId = m_JCObj.id;
                    retValue = String.Format("Job Card Created for job <{0}>", m_JCObj.jobid);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:    //updated name 
                    retValue = String.Format("Job Card <{0}> started Job Card Started By ", m_JCObj.id, m_JCObj.UpdatedByName);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = String.Format("Job Card <{0}> Closed of Job JC<{1}> Job Card Closed By<{2}>", m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_by_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRRY_FORWARDED:
                    retValue = String.Format("Job Card <{0}> Carry forward", m_JCObj.id);
                    break;
                case CMMS.CMMS_Status.JC_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("Job Card <{0}> Approved , Job Card Approved By Name <{1}>", m_JCObj.id, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_REJECTED:
                    retValue = String.Format("Job Card <{0}> Rejected , Job Card Rejected By Name <{1}>", m_JCObj.id, m_JCObj.JC_Rejected_By_Name);
                    break;
                //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                //    retValue = String.Format("Job card <{0}> and Permit <{1}>Time out ", m_JCObj.id, m_JCObj.ptwId);
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
            retValue += String.Format(template, "Created At ", m_JCObj.created_at);

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    retValue += "</table>";
                    break;
                case CMMS.CMMS_Status.JC_STARTED:
                    retValue += String.Format(templateEnd, "Started By", m_JCObj.UpdatedByName);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue += String.Format(templateEnd, "Closed By", m_JCObj.JC_Closed_by_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRRY_FORWARDED:
                    retValue += "</table>";
                    break;
                case CMMS.CMMS_Status.JC_APPROVED:
                    retValue += String.Format(templateEnd, "Approved By", m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_REJECTED:
                    retValue += String.Format(templateEnd, "Rejected By", m_JCObj.JC_Rejected_By_Name);
                    break;
                //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                //    break;
                default:
                    break;
            }



            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = String.Format("<h1>This is Job Card Title {0}</h1>", m_JCObj.description);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    template += String.Format("<p><b>Job Card status is :</b> Created</p> Job Card No {0}", m_JCObj.id);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:
                    template += String.Format("<p><b>Job Card status is : Started</p>");
                    template += String.Format("<p><b>Job Card No:</b> {0}</p><p> Job Card Started By {1}</p>", m_JCObj.id, m_JCObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    template += String.Format("<p><b>Job Card status is : Job Card Closed</p>");
                    template += String.Format("<p><b>Job Card No :</b> {0}</p><p>Job Card Closed By{1}</p>", m_JCObj.id, m_JCObj.JC_Closed_by_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRRY_FORWARDED:
                    template += String.Format("<p><b>Job Card  status is : Job Card Carry Forwarded </p>");
                    template += String.Format("<p>Job Card No:</b> {0}</p>", m_JCObj.id);
                    break;
                case CMMS.CMMS_Status.JC_APPROVED:
                    template += String.Format("<p><b>Job Card status is : Job Card approved </p>");
                    template += String.Format("<p>Job Card No {0}</p><p>Job Card Approved </b> {1}</p>", m_JCObj.id, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_REJECTED:
                    template += String.Format("<p><b>Job Card status is : Job Card Rejected </p>");
                    template += String.Format("<p>Job Card No {0} Job Card Rejected By :</b> {1}</p>", m_JCObj.id, m_JCObj.JC_Rejected_By_Name);
                    break;
                //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                //    template += String.Format("<p><b>Job Card status is : Job Card and Permit Time Out </p>");
                //    template += String.Format("<p>Job Card No:</b> {0}</p> <p>Permit No{1}</p>", m_JCObj.id, m_JCObj.ptwId);
                //    break;
                default:
                    break;
            }
            template += String.Format("<p><B>Permit description: </b>{0}</p>", m_JCObj.description);
            return template;
        }
    }
}
