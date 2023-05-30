//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CMMSAPIs.Helper;
//using CMMSAPIs.Models.PM;
//using CMMSAPIs.Models.Utils;
//using CMMSAPIs.Repositories.Utils;

//namespace CMMSAPIs.Models.Notifications
//{
//    internal class PMNotification : CMMSNotification
//    {
//        int m_PMId;
//        CMPMScheduleViewDetail m_pmObj;

//        public PMNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMScheduleViewDetail pmObj) : base(moduleID, notificationID)
//        {
//            m_pmObj = pmObj;
//            m_PMId = m_pmObj.id;
//        }
//        override protected string getSubject(params object[] args)
//        {

//            string retValue = "Subject";
//            m_PMId = m_pmObj.id;

//            switch (m_notificationID)
//            {
//                case CMMS.CMMS_Status.PM_START:     //Created                  
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Started", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_UPDATE:     //Assigned
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Updated", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_SUBMIT:     //Linked
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Submitted", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_LINK_PTW:     //Closed
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Linked to PTW <{2}>", m_pmObj.maintenance_order_number, m_pmObj.equipment_name, m_pmObj.permit_id);
//                    break;
//                case CMMS.CMMS_Status.PM_COMPLETED:     //Cancelled
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Completed", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_APPROVE:     //Closed
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Approved", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_REJECT:     //Cancelled
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Rejected", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_CANCELLED:     //Closed
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Cancelled", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                case CMMS.CMMS_Status.PM_PTW_TIMEOUT:     //Cancelled
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> PTW <{2}> Timed Out", m_pmObj.maintenance_order_number, m_pmObj.equipment_name, m_pmObj.permit_id);
//                    break;
//                case CMMS.CMMS_Status.PM_DELETED:     //Cancelled
//                    retValue = String.Format("Maintenance Order <{0}> Equipment <{1}> Deleted", m_pmObj.maintenance_order_number, m_pmObj.equipment_name);
//                    break;
//                //case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
//                //    retValue = String.Format("Maintenance Order <{0}> Cancelled", m_pmObj.maintenance_order_number);
//                //    break;
//                default:
//                    break;
//            }
//            return retValue;

//        }

//        override protected string getHTMLBody(params object[] args)
//        {
//            string retValue = "";

//            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_pmObj.status_long);
           
//            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
//            retValue += String.Format(template, "ID", m_pmObj.id);
//            retValue += String.Format(template, "Status", m_pmObj.status_short);
//            retValue += String.Format(template, "Equipment Name", m_pmObj.equipment_name);
//            retValue += String.Format(template, "Responsible Person", m_pmObj.assigned_to_name);
//            retValue += String.Format(template, "Check List", m_pmObj.schedule_check_list);
//            retValue += String.Format(template, "Frequency", m_pmObj.frequency_name);
//            retValue += String.Format(template, "Last Done Date", m_pmObj.last_done_date);
//            retValue += String.Format(template, "Scheduled Date", m_pmObj.schedule_date);
            

//            if (m_pmObj.schedule_link_job.Count != 0)
//            {
//                retValue += String.Format(template, "Associated Jobs", m_pmObj.schedule_link_job);
//            }
//            if (string.IsNullOrEmpty(m_pmObj.permit_code))
//            {
//                retValue += String.Format(template, "Permit ID", m_pmObj.permit_id);
//            }

//            switch (m_notificationID)
//            {
//                case CMMS.CMMS_Status.PM_START:
//                    retValue += String.Format(template, "Started By", m_pmObj.started_by);
//                    retValue += String.Format(templateEnd, "Started At", m_pmObj.started_at);
//                    break;
//                case CMMS.CMMS_Status.PM_UPDATE:
//                    retValue += String.Format(template, "Updated By", m_pmObj.updated_by);
//                    retValue += String.Format(templateEnd, "Updated At", m_pmObj.updated_at);
//                    break;
//                case CMMS.CMMS_Status.PM_SUBMIT:
//                    retValue += String.Format(template,"Submitted By", m_pmObj.submitted_by);
//                    retValue += String.Format(templateEnd, "Submitted At", m_pmObj.submitted_at);
//                    break;
//                case CMMS.CMMS_Status.PM_LINK_PTW:
//                    retValue += "</table>"; break;
//                case CMMS.CMMS_Status.PM_COMPLETED:
//                    retValue += String.Format(templateEnd, "Completed At", m_pmObj.completed_at);
//                    break;
//                case CMMS.CMMS_Status.PM_APPROVE:
//                    retValue += String.Format(template, "Approved By", m_pmObj.approved_by);
//                    retValue += String.Format(templateEnd, "Approved At", m_pmObj.approved_at);
//                    break;
//                case CMMS.CMMS_Status.PM_REJECT:
//                    retValue += String.Format(template, "Rejected By", m_pmObj.rejected_by);
//                    retValue += String.Format(templateEnd, "Rejected At", m_pmObj.rejected_at);
//                    break;
//                case CMMS.CMMS_Status.PM_CANCELLED:
//                    retValue += String.Format(template, "Cancelled By", m_pmObj.cancelled_by);
//                    retValue += String.Format(templateEnd, "Cancelled At", m_pmObj.cancelled_at);
//                    break;
//                case CMMS.CMMS_Status.PM_PTW_TIMEOUT:
//                    retValue += "</table>"; break;
//                case CMMS.CMMS_Status.PM_DELETED:
//                    retValue += String.Format(template, "Deleted By", m_pmObj.deleted_by);
//                    retValue += String.Format(templateEnd, "Deleted At", m_pmObj.deleted_at); break;
//                //case CMMS.CMMS_Status.PM_SCHEDULED:     //Cancelled
//                //    retValue = String.Format("Maintenance Order <{0}> Cancelled", m_pmObj.maintenance_order_number);
//                //    break;
//                default:
//                    break;
//            }

//            return retValue;
//        }


//    }
//}
