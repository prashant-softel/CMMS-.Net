using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
using System;

namespace CMMSAPIs.Models.Notifications
{
    internal class PTWNotification : CMMSNotification
    {
        CMPermitDetail m_permitObj;
        public PTWNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPermitDetail ptwObj) : base(moduleID, notificationID)
        {
            m_permitObj = ptwObj;
            m_module_ref_id = m_permitObj.insertedId;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATE : ";
            m_module_ref_id = m_permitObj.insertedId;
            string facilityName = m_permitObj.siteName;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("{0} PTW{1} Requested by {2}", facilityName, m_module_ref_id, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = String.Format("{0} PTW{1} Issued by {2}", facilityName, m_permitObj.permitNo, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Rejected By {2}", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    retValue = String.Format("{0} PTW{1} Approved By {2}", facilityName, m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} Rejected By {2}", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    retValue = String.Format("{0} PTW{1} Closed By {2}", facilityName, m_permitObj.permitNo, m_permitObj.closedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Cancelled by Issuer {2} ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = String.Format("{0} PTW{1} Cancelled by HSE {2} ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} cancelled by approver {2} ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested by {2}", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by {2}", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by {2}", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Extend Requested By {2}", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by {2}", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by {2}", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("{0} PTW{1} Linked to Job", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("{0} PTW{1} Linked to PM Permit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("{0} PTW{1} Linked to Audit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("{0} PTW{1} Linked to Hoto", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("{0} PTW{1} Expired", facilityName, m_permitObj.permitNo);
                    break;                
                default:
                    retValue = String.Format("{0} PTW{1} Unknow status {2}", facilityName, m_permitObj.permitNo, m_notificationID);
                    break;
            }

            retValue += $" for {m_delayDays} days";
            return retValue;
        }


        protected override string getURL(params object[] args)
        {
            return $"{m_baseURL}/permit-details/{m_module_ref_id}/0";
        }


        override protected string getSubject(params object[] args)
        {
            string retValue = "";
            m_module_ref_id = m_permitObj.insertedId;
            string facilityName = m_permitObj.siteName;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                    retValue = String.Format("{0} PTW{1} Requested by {2}", facilityName, m_module_ref_id, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = String.Format("{0} PTW{1} Issued by {2}", facilityName, m_permitObj.permitNo, m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Rejected By {2}", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    retValue = String.Format("{0} PTW{1} Approved By {2}", facilityName, m_permitObj.permitNo, m_permitObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} Rejected By {2}", facilityName, m_permitObj.permitNo, m_permitObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    retValue = String.Format("{0} PTW{1} Closed By {2}", facilityName, m_permitObj.permitNo, m_permitObj.closedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = String.Format("{0} PTW{1} Cancelled by Issuer {2} ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = String.Format("{0} PTW{1} Cancelled by HSE {2} ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = String.Format("{0} PTW{1} cancelled by approver {2} ", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested by {2}", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by {2}", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by {2}", facilityName, m_permitObj.permitNo, m_permitObj.cancelRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = String.Format("{0} PTW{1} Extend Requested By {2}", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Approve by {2}", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestApprovedByName);
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = String.Format("{0} PTW{1} Cancel Requested Rejected by {2}", facilityName, m_permitObj.permitNo, m_permitObj.extendRequestRejectedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = String.Format("{0} PTW{1} Linked to Job", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = String.Format("{0} PTW{1} Linked to PM Permit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = String.Format("{0} PTW{1} Linked to Audit", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = String.Format("{0} PTW{1} Linked to Hoto", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = String.Format("{0} PTW{1} Expired", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    retValue = String.Format("{0} PTW{1} Updated", facilityName, m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED_WITH_TBT:
                    retValue = String.Format("{0} PTW{1} TBT update by {2}", facilityName, m_permitObj.permitNo, m_permitObj.TBT_Done_By);
                    break;
                case CMMS.CMMS_Status.PTW_RESUBMIT:
                    retValue = String.Format("{0} PTW{1} resubmitted", facilityName, m_permitObj.permitNo);
                    break;
                default:
                    retValue = String.Format("{0} PTW{1} Unknow status {2}", facilityName, m_permitObj.permitNo, m_notificationID);
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_permitObj.current_status_long + " at " + m_permitObj.siteName);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", "PTW" + m_permitObj.permitNo);
            retValue += String.Format(template, "Facility Name", m_permitObj.siteName);
            retValue += String.Format(template, "Status", m_permitObj.current_status_short);
            retValue += String.Format(template, "Start time", m_permitObj.startDate);
            retValue += String.Format(template, "End time", m_permitObj.end_datetime);
            //retValue += String.Format(template, "PTW Title", m_permitObj.PermitTypeName);
            retValue += String.Format(template, "PTW Description", m_permitObj.description);
            retValue += String.Format(template, "Permit Type", m_permitObj.PermitTypeName);
            if (m_permitObj.sop_type_id > 0)
            {
                retValue += String.Format(template, "SOP Name", m_permitObj.sop_type_name);
            }
            retValue += String.Format(template, "Created By", m_permitObj.requestedByName + " at " + m_permitObj.request_datetime);
            if (m_permitObj.rejecter_id > 0)
            {
                retValue += String.Format(template, "Rejected By", m_permitObj.rejectedByName + " at " + m_permitObj.rejected_at);
            }
            if (m_permitObj.approver_id > 0)
            {
                retValue += String.Format(template, "Approved By", m_permitObj.approvedByName + " at " + m_permitObj.approve_at);
            }
            if (m_permitObj.TBT_Done_By_id > 0)
            {
                retValue += String.Format(template, "TBT Done By", m_permitObj.TBT_Done_By + " at " + m_permitObj.TBT_Done_At);
            }

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PTW_ISSUED:     //Assigned
                    retValue += String.Format(template, "Issued By", m_permitObj.issuedByName);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:     //Linked to PTW
                    retValue += String.Format(template, "Linked Job", m_permitObj.job_type_id);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:     //Linked to PTW                   
                    retValue += String.Format(template, "Linked PM", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:     //Linked to PTW
                    retValue += String.Format(template, "Linked Audit", m_permitObj.permitNo);
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:     //Linked to PTW
                    retValue += String.Format(template, "Linked HOTO", m_permitObj.permitNo);
                    break;
                default:
                    break;
            }

            if (m_permitObj.extendRequestby_id > 0)
            {
                retValue += String.Format(template, "Extend Requested By", m_permitObj.extendRequestByName + " at " + m_permitObj.extend_at);

                if (m_permitObj.extendRequestRejectedby_id > 0)
                {
                    retValue += String.Format(template, "Extend  Rejected by", m_permitObj.extendRequestRejectedByName);
                }
                if (m_permitObj.extendRequestApprovedby_id > 0)
                {
                    retValue += String.Format(template, "Extend Approved By", m_permitObj.extendRequestApprovedByName);
                }
            }

            if (m_permitObj.cancelRequestby_id > 0)
            {
                retValue += String.Format(template, "Cancel Requested By", m_permitObj.cancelRequestByName + " at " + m_permitObj.cancel_at);
                if (m_permitObj.cancelRequestRejectedby_id > 0)
                {
                    retValue += String.Format(template, "Cancel Rejected by", m_permitObj.cancelRequestRejectedByName);
                }
                if (m_permitObj.cancelRequestApprovedby_id > 0)
                {
                    retValue += String.Format(template, "Cancel Approved By", m_permitObj.cancelRequestApprovedByName);
                }
            }
            if (m_permitObj.closedby_id > 0)
            {
                retValue += String.Format(template, "Closed By", m_permitObj.closedByName + " at " + m_permitObj.close_at);
            }

            if (m_permitObj.LstCategory != null && m_permitObj.LstCategory.Count > 0)
            {
                int i = 0;
                string categoryNames = "";
                foreach (var item in m_permitObj.LstCategory)
                {
                    i++;
                    categoryNames += item.equipmentCat;
                    if (m_permitObj.LstCategory.Count > 1 && i < m_permitObj.LstCategory.Count)
                    {
                        categoryNames += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Equipment categories", categoryNames);
                }
            }

            if (m_permitObj.LstIsolationCategory != null && m_permitObj.LstIsolationCategory.Count > 0)
            {
                int i = 0;
                string categoryNames = "";
                foreach (var item in m_permitObj.LstIsolationCategory)
                {
                    i++;
                    categoryNames += item.name;
                    if (m_permitObj.LstIsolationCategory.Count > 1 && i < m_permitObj.LstIsolationCategory.Count)
                    {
                        categoryNames += ", ";
                    }
                }

                if (i > 0)
                {
                    retValue += String.Format(template, "Isolation categories", categoryNames);
                }
            }
            
            if(m_permitObj.is_physical_iso_required > 0)
            {
                if (m_permitObj.physical_iso_equips.Count > 0)
                {
                    int i = 0;
                    string categoryNames = "";
                    foreach (var item in m_permitObj.physical_iso_equips)
                    {
                        i++;
                        categoryNames += item.name;
                        if (m_permitObj.physical_iso_equips.Count > 1 && i < m_permitObj.physical_iso_equips.Count)
                        {
                            categoryNames += ", ";
                        }
                    }

                    if (i > 0)
                    {
                        retValue += String.Format(template, "Equipment categories", categoryNames);
                        retValue += String.Format(template, "physical isolation remark", m_permitObj.physical_iso_remark);
                    }
                }

            }
            if (m_permitObj.is_loto_required > 0)
            {
                if (m_permitObj.Loto_list.Count > 0)
                {
                    int i = 0;
                    string categoryNames = "";
                    foreach (var item in m_permitObj.Loto_list)
                    {
                        i++;
                        categoryNames += item.equipment_name;
                        if (m_permitObj.Loto_list.Count > 1 && i < m_permitObj.Loto_list.Count)
                        {
                            categoryNames += ", ";
                        }
                    }

                    if (i > 0)
                    {
                        retValue += String.Format(template, "Loto equipments", categoryNames);
                        retValue += String.Format(template, "LOTO remark", m_permitObj.loto_remark);
                    }
                }

            }

            retValue += "</table>";

            return retValue;
        }
    }
}
