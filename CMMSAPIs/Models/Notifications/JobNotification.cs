using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class JobNotification : CMMSNotification
    {
        int m_jobId;
        CMJobView m_jobObj;
       
        public JobNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJobView jobObj) : base(moduleID, notificationID)
        {
            m_jobObj = jobObj;
            m_jobId = m_jobObj.id;
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";
            m_jobId = m_jobObj.id;
          
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    string desc = m_jobObj.job_description;
                    retValue = String.Format("Job <{0}><{1}> created", m_jobId, desc);
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = String.Format("Job <{0}> assigned to <{1}>", m_jobObj.job_title, m_jobObj.assigned_name);
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = String.Format("Job <{0}> linked to PTW <{1}>", m_jobObj.job_title,m_jobObj.current_ptwId);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = String.Format("Job <{0}> closed", m_jobObj.job_title);
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = String.Format("Job <{0}> Cancelled", m_jobObj.job_title);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int jobID = m_jobObj.id; //jobid
            var jobTitle = m_jobObj.job_title;//job title
            var jobDesc = m_jobObj.job_description;//job desc
            var template = getHTMLBodyTemplate(args);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = String.Format(template, jobTitle, jobDesc);
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = String.Format(template, jobTitle, jobDesc);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = String.Format(template, jobTitle, jobDesc);
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked to PTW
                    int ptwId = m_jobObj.current_ptwId;
                    string ptwDesc = (string)m_jobObj.job_description;
                    retValue = String.Format(template, jobTitle, jobDesc, ptwId, ptwDesc);
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
                    template += "<p><b>Job status is : Assigned to PTW ID  PTW Desc</p>";
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
