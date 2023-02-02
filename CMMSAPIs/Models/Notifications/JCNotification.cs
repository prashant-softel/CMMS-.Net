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
                case CMMS.CMMS_Status.JC_OPENED:    
                    int jcId = m_JCObj.id;
                    retValue = String.Format("Job Card Opened for job <{0}>" , m_JCObj.jobid);
                    break;
                case CMMS.CMMS_Status.JC_UPDADATED:    //updated name 
                    retValue = String.Format("Job Card <{0}> updated Job Card Updated By ", m_JCObj.id, m_JCObj.UpdatedByName);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:    
                    retValue = String.Format("Job Card <{0}> Closed of Job JC<{1}> Job Card Closed By<{2}>", m_JCObj.id, m_JCObj.jobid,m_JCObj.JC_Closed_by_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRRY_FORWARDED:     
                    retValue = String.Format("Job Card <{0}> Carry forward", m_JCObj.id);
                    break;
                case CMMS.CMMS_Status.JC_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("Job Card <{0}> Approved , Job Card Approved By Name <{1}>", m_JCObj.id,m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_REJECTED5: 
                    retValue = String.Format("Job Card <{0}> Rejected , Job Card Rejected By Name <{1}>", m_JCObj.id, m_JCObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                    retValue = String.Format("Job card <{0}> and Permit <{1}>Time out ", m_JCObj.id, m_JCObj.ptwId);
                    break;
            }
            return retValue;

        }


        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int jcId = m_JCObj.id;
            int permitId = m_JCObj.ptwId;
            int jobid = m_JCObj.jobid;
            var JobDesc = m_JCObj.description;
            string JC_Closed_by_Name = (string)m_JCObj.JC_Closed_by_Name;
            string JC_Rejected_By_Name = (string)m_JCObj.JC_Rejected_By_Name;
            string JC_Updated_By_Name = (string)m_JCObj.UpdatedByName;
            string JC_Approved_By_Name = (string)m_JCObj.JC_Approved_By_Name;

            var template = getHTMLBodyTemplate(args);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_OPENED:     
                    retValue = String.Format(template, jcId, JobDesc);
                    break;
                case CMMS.CMMS_Status.JC_UPDADATED:     
                    retValue = String.Format(template, jcId, JC_Updated_By_Name, JobDesc);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:     
                    retValue = String.Format(template, jcId, JC_Closed_by_Name, JobDesc);
                    break;
                case CMMS.CMMS_Status.JC_CARRRY_FORWARDED:     
                    retValue = String.Format(template, jcId, JobDesc);
                    break;
                case CMMS.CMMS_Status.JC_APPROVED:     
                    retValue = String.Format(template, jcId, JC_Approved_By_Name, JobDesc);
                    break;
                case CMMS.CMMS_Status.JC_REJECTED5:     
                    retValue = String.Format(template, jcId, JC_Rejected_By_Name, JobDesc);
                    break;
                case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:    
                    retValue = String.Format(template, jcId, JobDesc);
                    break;               
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
                case CMMS.CMMS_Status.JC_OPENED:     
                    template += String.Format("<p><b>Job Card status is :</b> Opened</p> Job Card No {0}", m_JCObj.id);
                    break;
                case CMMS.CMMS_Status.JC_UPDADATED:     
                    template += String.Format("<p><b>Job Card status is : Updated</p>");
                    template += String.Format("<p><b>Job Card No:</b> {0}</p><p> Job Card Updated By {1}</p>", m_JCObj.id,m_JCObj.UpdatedByName);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:     
                    template += String.Format("<p><b>Job Card status is : Job Card Closed</p>");
                    template += String.Format("<p><b>Job Card No :</b> {0}</p><p>Job Card Closed By{1}</p>", m_JCObj.id,m_JCObj.JC_Closed_by_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRRY_FORWARDED:    
                    template += String.Format("<p><b>Job Card  status is : Job Card Carry Forwarded </p>");
                    template += String.Format("<p>Job Card No:</b> {0}</p>", m_JCObj.id);
                    break;
                case CMMS.CMMS_Status.JC_APPROVED:     
                    template += String.Format("<p><b>Job Card status is : Job Card approved </p>");
                    template += String.Format("<p>Job Card No {0}</p><p>Job Card Approved </b> {1}</p>", m_JCObj.id,m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_REJECTED5:     
                    template += String.Format("<p><b>Job Card status is : Job Card Rejected </p>");
                    template += String.Format("<p>Job Card No {0} Job Card Rejected By :</b> {1}</p>", m_JCObj.id,m_JCObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                    template += String.Format("<p><b>Job Card status is : Job Card and Permit Time Out </p>");
                    template += String.Format("<p>Job Card No:</b> {0}</p> <p>Permit No{1}</p>", m_JCObj.id,m_JCObj.ptwId);
                    break;
                default:
                    break;
            }
            template += String.Format("<p><B>Permit description: </b>{0}</p>", m_JCObj.description);
            return template;
        }
    }
}
