using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class IncidentReportNotification : CMMSNotification
    {
        int m_incidentReportId;
        CMViewIncidentReport m_IncidentReportObj;
        public IncidentReportNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMViewIncidentReport incidentReportObj) : base(moduleID, notificationID)
        {
            m_IncidentReportObj = incidentReportObj;
            //m_incidentReportId = m_IncidentReportObj.id;
        }

        override protected string getSubject(params object[] args)
        {
            string retValue = "My Incident Report subject";
            m_incidentReportId = m_IncidentReportObj.id;
            string desc = m_IncidentReportObj.description;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.IR_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("Incident Report <{0}> created , Incident Report Created By <{1}>, Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.created_by_name, desc);
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:
                    retValue = String.Format("Incident Report <{0}> Updated , Incident Report Updated By<{1}> Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.updated_by_name, desc);
                    break;
                case CMMS.CMMS_Status.IR_APPROVED:
                    retValue = String.Format("Incident Report <{0}> Approved,  Incident Report approved by name <{1}> Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.approved_by_name, desc);
                    break;
                case CMMS.CMMS_Status.IR_REJECTED://namr
                    retValue = String.Format("Incident Report <{0}> Rejected, Incident Report Rejected By Name <{1}> Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.approved_by_name, desc);
                    break;
                default:
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            int IncidentReportNo = m_IncidentReportObj.id;
            var IncidentReportDesc = m_IncidentReportObj.description;
            string IncidentReport_created_name = (string)m_IncidentReportObj.created_by_name;
            string IncidentReport_updated_name = (string)m_IncidentReportObj.updated_by_name;
            string IncidentReport_approver_name = (string)m_IncidentReportObj.approved_by_name;
            string IncidentReport_rejected_name = (string)m_IncidentReportObj.inverstigated_by_name;

            var template = getHTMLBodyTemplate(args);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.IR_CREATED:     
                    retValue = String.Format(template, IncidentReportNo, IncidentReportDesc, IncidentReport_created_name);
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:     
                    retValue = String.Format(template, IncidentReportNo, IncidentReport_updated_name, IncidentReportDesc);
                    break;
                case CMMS.CMMS_Status.IR_APPROVED:     
                    retValue = String.Format(template, IncidentReportNo, IncidentReport_approver_name, IncidentReportDesc);
                    break;
                case CMMS.CMMS_Status.IR_REJECTED:    
                    retValue = String.Format(template, IncidentReportNo, IncidentReport_rejected_name, IncidentReportDesc);
                    break;
                default:
                    break;
            }

            return retValue;
        }

        internal string getHTMLBodyTemplate(params object[] args)
        {
            string template = String.Format("<h1>This is Incident Report Title {0}</h1>", m_IncidentReportObj.description);
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.IR_CREATED:
                    template += String.Format("<p><b>Incident Report status is :</b> Created</p> Incident Report No {0} Incident Report Created by Name<{1}>", m_IncidentReportObj.id, m_IncidentReportObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:
                    template += String.Format("<p><b>Incident Report status is : Updated</p>");
                    template += String.Format("<p>Incident Report {0} is Updated </p>", m_IncidentReportObj.id);
                    template += String.Format("<p><b>Incident Report Updated By:</b> {0}</p>", m_IncidentReportObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.IR_APPROVED:
                    template += String.Format("<p><b>Incident Report status is : Approved Incident Report</p>");
                    template += String.Format("<p>Incident Report {0} Approved </p>", m_IncidentReportObj.id);
                    template += String.Format("<p><b>Incident Report approved By :</b> {0}</p>",m_IncidentReportObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.IR_REJECTED:
                    template += String.Format("<p><b>Incident Report status is : Incident Report rejected </p>");
                    template += String.Format("<p>Incident Report {0} is rejected </p>", m_IncidentReportObj.inverstigated_by_name);
                    template += String.Format("<p>Incident Report rejected By:</b> {0}</p>", m_IncidentReportObj.inverstigated_by_name);
                    break;
                default:
                    break;
            }
            template += String.Format("<p><B>Incident Report description: </b>{0}</p>", m_IncidentReportObj.description);
            return template;
        }
    }
}

