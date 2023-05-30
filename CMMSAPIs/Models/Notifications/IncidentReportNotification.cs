//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CMMSAPIs.Helper;
//using CMMSAPIs.Models.Incident_Reports;
//using CMMSAPIs.Models.Utils;
//using CMMSAPIs.Repositories.Utils;

//namespace CMMSAPIs.Models.Notifications
//{
//    internal class IncidentReportNotification : CMMSNotification
//    {
//        int m_incidentReportId;
//        CMViewIncidentReport m_IncidentReportObj;
//        public IncidentReportNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMViewIncidentReport incidentReportObj) : base(moduleID, notificationID)
//        {
//            m_IncidentReportObj = incidentReportObj;
//            //m_incidentReportId = m_IncidentReportObj.id;
//        }

//        override protected string getSubject(params object[] args)
//        {
//            string retValue = "My Incident Report subject";
//            m_incidentReportId = m_IncidentReportObj.id;
//            string desc = m_IncidentReportObj.description;

//            switch (m_notificationID)
//            {
//                case CMMS.CMMS_Status.IR_CREATED:
//                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
//                    retValue = String.Format("Incident Report <{0}> created , Incident Report Created By <{1}>, Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.created_by_name, desc);
//                    break;
//                case CMMS.CMMS_Status.IR_UPDATED:
//                    retValue = String.Format("Incident Report <{0}> Updated , Incident Report Updated By<{1}> Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.updated_by_name, desc);
//                    break;
//                case CMMS.CMMS_Status.IR_APPROVED:
//                    retValue = String.Format("Incident Report <{0}> Approved,  Incident Report approved by name <{1}> Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.approved_by_name, desc);
//                    break;
//                case CMMS.CMMS_Status.IR_REJECTED://namr
//                    retValue = String.Format("Incident Report <{0}> Rejected, Incident Report Rejected By Name <{1}> Incident Report Description <{2}>", m_incidentReportId, m_IncidentReportObj.approved_by_name, desc);
//                    break;
//                default:
//                    break;
//            }
//            return retValue;
//        }

//        override protected string getHTMLBody(params object[] args)
//        {
//            string retValue = "";

//            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_IncidentReportObj.status_long);

//            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
//            retValue += String.Format(template, "ID", m_IncidentReportObj.id);
//            retValue += String.Format(template, "Title", m_IncidentReportObj.title);
//            retValue += String.Format(template, "Description", m_IncidentReportObj.description);
//            retValue += String.Format(template, "Status ", m_IncidentReportObj.status_short);
//            retValue += String.Format(template, "Incident Date & Time", m_IncidentReportObj.incident_datetime);
//            retValue += String.Format(template, "Reporting Date & Time", m_IncidentReportObj.incident_datetime);
//            retValue += String.Format(template, "Severity ", m_IncidentReportObj.severity);
//            retValue += String.Format(template, "Action Taken By", m_IncidentReportObj.action_taken_by_name);
//            retValue += String.Format(template, "Action Taken Date & Time ", m_IncidentReportObj.action_taken_datetime);
//            retValue += String.Format(template, "Investigated By", m_IncidentReportObj.inverstigated_by_name);
//            retValue += String.Format(template, "Verified By", m_IncidentReportObj.verified_by_name);
//            retValue += String.Format(template, "Risk Type", m_IncidentReportObj.risk_type);
//            retValue += String.Format(template, "Risk Level", m_IncidentReportObj.risk_level_name);
//            retValue += String.Format(template, "Damaged asset cost approx.", m_IncidentReportObj.damaged_cost);
//            retValue += String.Format(template, "Gen loss due to asset damage", m_IncidentReportObj.generation_loss);
//            retValue += String.Format(template, "Insurance", m_IncidentReportObj.is_insurance_applicable_name);
//            retValue += String.Format(template, "Insurance Status", m_IncidentReportObj.insurance_status_name);
//            retValue += String.Format(template, "Insurance Remark", m_IncidentReportObj.insurance_remark);

//            switch (m_notificationID)
//            {
//                case CMMS.CMMS_Status.IR_CREATED:
//                    retValue += "</table>"; break;
//                case CMMS.CMMS_Status.IR_UPDATED:
//                    retValue += String.Format(templateEnd, "Updated By", m_IncidentReportObj.updated_by_name);
//                    break;
//                case CMMS.CMMS_Status.IR_APPROVED:                   
//                    retValue += String.Format(templateEnd, "Approved By", m_IncidentReportObj.approved_by_name);
//                    break;
//                case CMMS.CMMS_Status.IR_REJECTED:
//                    retValue += String.Format(templateEnd, "Rejected By", m_IncidentReportObj.inverstigated_by_name);
//                    break;
//                default:
//                    break;
//            }

//            return retValue;
//        }

//        internal string getHTMLBodyTemplate(params object[] args)
//        {
//            string template = String.Format("<h1>This is Incident Report Title {0}</h1>", m_IncidentReportObj.description);
//            switch (m_notificationID)
//            {
//                case CMMS.CMMS_Status.IR_CREATED:
//                    template += String.Format("<p><b>Incident Report status is :</b> Created</p> Incident Report No {0} Incident Report Created by Name<{1}>", m_IncidentReportObj.id, m_IncidentReportObj.created_by_name);
//                    break;
//                case CMMS.CMMS_Status.IR_UPDATED:
//                    template += String.Format("<p><b>Incident Report status is : Updated</p>");
//                    template += String.Format("<p>Incident Report {0} is Updated </p>", m_IncidentReportObj.id);
//                    template += String.Format("<p><b>Incident Report Updated By:</b> {0}</p>", m_IncidentReportObj.updated_by_name);
//                    break;
//                case CMMS.CMMS_Status.IR_APPROVED:
//                    template += String.Format("<p><b>Incident Report status is : Approved Incident Report</p>");
//                    template += String.Format("<p>Incident Report {0} Approved </p>", m_IncidentReportObj.id);
//                    template += String.Format("<p><b>Incident Report approved By :</b> {0}</p>",m_IncidentReportObj.approved_by_name);
//                    break;
//                case CMMS.CMMS_Status.IR_REJECTED:
//                    template += String.Format("<p><b>Incident Report status is : Incident Report rejected </p>");
//                    template += String.Format("<p>Incident Report {0} is rejected </p>", m_IncidentReportObj.inverstigated_by_name);
//                    template += String.Format("<p>Incident Report rejected By:</b> {0}</p>", m_IncidentReportObj.inverstigated_by_name);
//                    break;
//                default:
//                    break;
//            }
//            template += String.Format("<p><B>Incident Report description: </b>{0}</p>", m_IncidentReportObj.description);
//            return template;
//        }
//    }
//}

