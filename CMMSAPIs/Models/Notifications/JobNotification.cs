﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
                    retValue = String.Format("Job <{0}> linked to PTW <{1}>", m_jobObj.job_title,m_jobObj.current_ptw_id);
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
            string encodedStatusLong = HttpUtility.HtmlEncode(m_jobObj.status_long);

            retValue = String.Format("<h3><b style='color:#31576D'>Status: </b>{0}</h3><br>", encodedStatusLong);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "Job ID", m_jobObj.id);
            retValue += String.Format(template, "Job Status", m_jobObj.status_short);
            retValue += String.Format(template, "Job Title", m_jobObj.job_title);
            retValue += String.Format(template, "Job Description", m_jobObj.job_description);
            retValue += String.Format(template, "PTW Id",m_jobObj.current_ptw_id);
            retValue += String.Format(template, "PTW Title", m_jobObj.current_ptw_title);
            retValue += String.Format(template, "Created by", m_jobObj.created_by_name);
            retValue += String.Format(template, "Created At", m_jobObj.created_at);
            retValue += String.Format(template, "Assigned To", m_jobObj.assigned_name);
            //retValue += String.Format(template, "Assigned At", m_jobObj.created_by_name);
            //retValue += String.Format(template, "Closed By", m_jobObj.assigned_name);
            retValue += String.Format(templateEnd, "Closed At", m_jobObj.closed_at);



            //switch (m_notificationID)
            //{
            //    case CMMS.CMMS_Status.JOB_CREATED:
            //        retValue += "</table>";            //Created
            //        break;
            //    case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
            //        retValue += String.Format(templateEnd, "Assigned To", m_jobObj.assigned_name);
            //        break;
            //    case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
            //        retValue += String.Format(templateEnd, "Closed At", m_jobObj.closed_at);

            //        break;
            //    case CMMS.CMMS_Status.JOB_LINKED:     //Linked to PTW
            //        retValue += String.Format(template, "PTW Id", m_jobObj.current_ptw_id);
            //        retValue += String.Format(templateEnd, "PTW Title", m_jobObj.current_ptw_title);
            //        break;
            //    default:
            //        break;
            //}

            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = String.Format("<h1>This is Job Title {0}</h1>",m_jobObj.job_title);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    template += String.Format("<p><b>{0}</b> Created{0}</p>",m_jobObj.job_title);
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    template += String.Format("<p><b>Job status is : Assiged</p>");
                    template += String.Format("<p><b>Job assigned to:</b> {0} </p>",m_jobObj.assigned_name);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    template += String.Format("<p><b>Job status is : Closed</p>");
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked to PTW
                    template += String.Format("<p><b>Job status is : Assigned to {0} Job Id {1} Linked to PTW ID {2}<p>",m_jobObj.assigned_name,m_jobObj.id,m_jobObj.current_ptw_id);
                    break;
                default:
                    break;
            }
            template += "<p><B>Job description: </b>{1}</p>";
            return template;
        }
    }
}
