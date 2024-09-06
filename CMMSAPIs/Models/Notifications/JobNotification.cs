using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class JobNotification : CMMSNotification
    {
        CMJobView m_jobObj;
        public JobNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJobView jobObj) : base(moduleID, notificationID)
        {
            m_jobObj = jobObj;
            m_module_ref_id = m_jobObj.id;
            //m_notificationType = notificationType;
        }
        override protected string getSubject(params object[] args)
        {
            string retValue = "";
            
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = String.Format("{0} BM JOB{1} Created", m_jobObj.facility_name, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = String.Format("{0} BM JOB{1} Assigned", m_jobObj.facility_name, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = String.Format("{0} BM JOB{1} linked to PTW{2}", m_jobObj.facility_name, m_module_ref_id, m_jobObj.current_ptw_id);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = String.Format("{0} BM JOB{1} Closed", m_jobObj.facility_name, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = String.Format("{0} BM JOB{1} Cancelled by", m_jobObj.facility_name, m_module_ref_id, m_jobObj.cancelled_by_name);
                    break;
                case CMMS.CMMS_Status.JOB_UPDATED:     //Updated
                    retValue = String.Format("{0} BM JOB{1} updated", m_jobObj.facility_name, m_module_ref_id);
                    break;
                default:
                    retValue = String.Format("{0} BM JOB{1} undefined status", m_jobObj.facility_name, m_module_ref_id);
                    break;
            }
            return retValue;

        }
        override protected string getEMSubject(params object[] args)
        {

            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    string desc = m_jobObj.job_title;
                    retValue += String.Format("{0} BM JOB{1} Created", m_jobObj.facility_name, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue += String.Format("{0} BM JOB{1} Assigned", m_jobObj.facility_name, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue += String.Format("{0} BM JOB{1} linked to PTW{2}", m_jobObj.facility_name, m_module_ref_id, m_jobObj.current_ptw_id);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue += String.Format("{0} BM JOB{1} Closed", m_jobObj.facility_name, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue += String.Format("{0} BM JOB{1} Cancelled", m_jobObj.facility_name, m_module_ref_id);
                    break;
                default:
                    retValue += String.Format("{0} BM JOB{1} undefined status", m_jobObj.facility_name, m_module_ref_id);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int jobID = m_jobObj.id; //jobid
            var jobTitle = m_jobObj.job_title;//job title
            var jobDesc = m_jobObj.job_description;//job desc
            string encodedStatusLong = HttpUtility.HtmlEncode(m_jobObj.status_long + " at " + m_jobObj.facility_name);

            retValue = String.Format("<h3><b style='color:#31576D'>Status: </b>{0}</h3><br>", encodedStatusLong);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "Job ID", "JOB" + m_jobObj.id);
            retValue += String.Format(template, "Job Status", m_jobObj.status_long);
            retValue += String.Format(template, "Job Title", m_jobObj.job_title);
            retValue += String.Format(template, "Job Description", m_jobObj.job_description);
            retValue += String.Format(template, "Facility name", m_jobObj.facility_name);
            retValue += String.Format(template, "Block name", m_jobObj.block_name);

            if (m_jobObj.equipment_cat_list.Count > 0)
            {
                int i = 0;
                string categoryNames = "";
                foreach (var item in m_jobObj.equipment_cat_list)
                {
                    i++;
                    categoryNames += item.name;
                    if (m_jobObj.equipment_cat_list.Count > 1 && i < m_jobObj.equipment_cat_list.Count)
                    {
                        categoryNames += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Equipment categories", categoryNames);
                }
            }

            if (m_jobObj.working_area_name_list.Count > 0)
            {
                int i = 0;
                string eqNames = "";
                foreach (var item in m_jobObj.working_area_name_list)
                {
                    i++;
                    eqNames += item.name;
                    if (m_jobObj.working_area_name_list.Count > 1 && i < m_jobObj.working_area_name_list.Count)
                    {
                        eqNames += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Equipment Names", eqNames);
                }
            }
            retValue += String.Format(template, "Breakdown time", m_jobObj.breakdown_time);
            retValue += String.Format(template, "Created by", m_jobObj.created_by_name);
            retValue += String.Format(template, "Created At", m_jobObj.created_at);
            if (m_jobObj.assigned_id > 0)
            {
                retValue += String.Format(template, "Assigned To", m_jobObj.assigned_name);
                //retValue += String.Format(template, "Assigned At", m_jobObj.created_by_name);
            }
            if (m_jobObj.status == (int)CMMS.CMMS_Status.JOB_CLOSED)
            {
                //retValue += String.Format(template, "Closed By", m_jobObj.assigned_name);
                retValue += String.Format(template, "Closed At", m_jobObj.Job_closed_on);
            }
            if (m_jobObj.cancelled_by_id > 0)
            {
                retValue += String.Format(template, "Cancelled by", m_jobObj.cancelled_by_name);
                retValue += String.Format(template, "Cancelled At", m_jobObj.cancelled_at);
            }

            if (m_jobObj.current_ptw_id > 0)
            {
                retValue += String.Format(template, "PTW Id", "PTW" + m_jobObj.current_ptw_id);
                retValue += String.Format(template, "PTW Title", m_jobObj.current_ptw_title);
                retValue += String.Format(template, "TBT Conducted by", m_jobObj.TBT_conducted_by_name);
            }
            if(m_jobObj.latestJCid > 0)
            {
                retValue += String.Format(template, "Latest Job card", "JC" + m_jobObj.latestJCid + " (" + m_jobObj.latestJCStatusShort + ")");

            }

            if (m_jobObj.work_type_list.Count > 0)
            {
                int i = 0;
                string displayList = "";
                foreach (var item in m_jobObj.work_type_list)
                {
                    i++;
                    displayList += item.workType;
                    if (m_jobObj.work_type_list.Count > 1 && i < m_jobObj.work_type_list.Count)
                    {
                        displayList += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Work type", displayList);
                }
            }
            if (m_jobObj.tools_required_list.Count > 0)
            {
                int i = 0;
                string displayList = "";
                foreach (var item in m_jobObj.tools_required_list)
                {
                    i++;
                    displayList += item.linkedToolName;
                    if (m_jobObj.tools_required_list.Count > 1 && i < m_jobObj.tools_required_list.Count)
                    {
                        displayList += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Tools required", displayList);
                }
            }

            retValue += "</table><br><br>";
            return retValue;
        }
    }
}
