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
        CMJCDetail m_JCObj;
        public JCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJCDetail jcObj) : base(moduleID, notificationID)
        {
            m_JCObj = jcObj;
            m_module_ref_id = m_JCObj.id;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATE : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    int jcId = m_JCObj.id;
                    retValue += String.Format("{0} BM JC{1} for JOB{2} Created By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.created_by);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:    //updated name 
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Started By {3} but not closed", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Carry forwarded approval requested By {3}  and waiting for approval", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:  
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Carryforward request Approved By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Carryforward request Rejected By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Close approval requested By {3} and waiting for approval", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:   
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Close request Approved By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue += String.Format("{0} BM JC{1} of JOB{2} Close request rejected By {3} and waiting for resubmit", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                default:
                    retValue += String.Format("{0} BM JC{1} of JOB{2} unknown status {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_notificationID);
                    break;
            }

            retValue += $" for {m_delayDays} days";

            return retValue;

        }


        override protected string getSubject(params object[] args)
        {
            string retValue = "";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    int jcId = m_JCObj.id;
                    retValue = String.Format("{0} BM JC{1} for JOB{2} Created By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.created_by);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:    //updated name 
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Started By {3} ", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Carry forwarded By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Carryforward request Approved By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Carryforward request Rejected By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Close approval requested By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:   //approved name   permit issuer = jc  approver
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Close request Approved By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = String.Format("{0} BM JC{1} of JOB{2} Close request rejected By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_UPDATED:
                    retValue = String.Format("{0} BM JC{1} of JOB{2} updated By {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_JCObj.JC_UpdatedByName);
                    break;
                default:
                    retValue = String.Format("{0} BM JC{1} of JOB{2} unknown status {3}", m_JCObj.plant_name, m_JCObj.id, m_JCObj.jobid, m_notificationID);
                    break;
            }
            return retValue;

        }


        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_JCObj.status_long + " at " + m_JCObj.plant_name);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "JC ID", "JC" + m_JCObj.id);
            retValue += String.Format(template, "Status", m_JCObj.status_short);
            retValue += String.Format(template, "Job ID", "JOB" + m_JCObj.jobid);
            retValue += String.Format(template, "PTW ID", "PTW" + m_JCObj.ptwId);
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
        
            if (m_JCObj.asset_category_name != null && m_JCObj.asset_category_name.Count > 0)
            {
                int i = 0;
                string categoryNames = "";
                foreach (var item in m_JCObj.asset_category_name)
                {
                    i++;
                    categoryNames += item.Equipment_name + "(" + item.Equipment_category + ")";
                    if (m_JCObj.asset_category_name.Count > 1 && i<m_JCObj.asset_category_name.Count)
                    {
                        categoryNames += ", ";
                    }
}

                if (i > 0)
                {
                    retValue += String.Format(template, "Equipments", categoryNames);
                }
            }
            if (m_JCObj.LstCMJCLotoDetailList != null && m_JCObj.LstCMJCLotoDetailList.Count > 0)
            {
                int i = 0;
                string categoryNames = "";
                foreach (var item in m_JCObj.LstCMJCLotoDetailList)
                {
                    i++;
                    categoryNames += item.isolated_assest_loto;
                    if (m_JCObj.LstCMJCLotoDetailList.Count > 1 && i < m_JCObj.LstCMJCLotoDetailList.Count)
                    {
                        categoryNames += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Loto Details", categoryNames);
                }
            }

            if (m_JCObj.Material_consumption != null && m_JCObj.Material_consumption.Count > 0)
            {
                int i = 0;
                string categoryNames = "";
                foreach (var item in m_JCObj.Material_consumption)
                {
                    i++;
                    categoryNames += item.Material_name + "(" + item.Material_type + ")";
                    if (m_JCObj.Material_consumption.Count > 1 && i<m_JCObj.Material_consumption.Count)
                    {
                        categoryNames += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Material consumption", categoryNames);
                }
            }

            retValue += "</table>";
            return retValue;
        }
    }
}
