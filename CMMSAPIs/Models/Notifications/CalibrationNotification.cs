using CMMSAPIs.Helper;
using CMMSAPIs.Models.Calibration;
using System;


namespace CMMSAPIs.Models.Notifications
{
    internal class CalibrationNotification : CMMSNotification
    {
        int m_calibrationId;
        CMCalibrationDetails calibObj;

        public CalibrationNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMCalibrationDetails CaliObj) : base(moduleID, notificationID)
        {
            calibObj = CaliObj;
            m_module_ref_id = CaliObj.calibration_id;

        }
        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCLATION : ";
            string facilityName = calibObj.facility_name;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_SCHEDULED:
                    retValue += String.Format("{0} CAL{1} Scheduled", facilityName, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:
                    retValue += String.Format("{0} CAL{1} Requested By <{2}>", facilityName, m_module_ref_id, calibObj.responsible_person);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:
                    retValue += String.Format("{0} CAL{1} Request Rejected By <{2}>", facilityName, m_module_ref_id, calibObj.request_rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:
                    retValue += String.Format("{0} CAL{1} Started By <{2}>", facilityName, m_module_ref_id, calibObj.started_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:
                    retValue += String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.completed_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                    retValue += String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.completed_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_REJECTED:
                    retValue += String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.request_rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_APPROVED:
                    retValue += String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.request_approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                    retValue += String.Format("{0} CAL{1} Completed Rejectde By <{2}>", facilityName, m_module_ref_id, calibObj.rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue += String.Format("{0} CAL{1} Completed ApprovedBy <{2}>", facilityName, m_module_ref_id, calibObj.approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED:
                    retValue += String.Format("{0} CAL{1} Skipped Requested By <{2}>", facilityName, m_module_ref_id, calibObj.skipped_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_REJCTED:
                    retValue += String.Format("{0} CAL{1} Skipped Rejectde By <{2}>", facilityName, m_module_ref_id, calibObj.rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_APPROVED:
                    retValue += String.Format("{0} CAL{1} Skipped Approved By <{2}>", facilityName, m_module_ref_id, calibObj.approved_by);
                    break;
                default:
                    retValue += String.Format("{0} CAL{1} Undefined status {2}", facilityName, m_module_ref_id, m_notificationID);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "";
            string facilityName = calibObj.facility_name;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_SCHEDULED:
                    retValue = String.Format("{0} CAL{1} Scheduled", facilityName, m_module_ref_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:
                    retValue = String.Format("{0} CAL{1} Requested By <{2}>", facilityName, m_module_ref_id, calibObj.responsible_person);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:
                    retValue = String.Format("{0} CAL{1} Request Rejected By <{2}>", facilityName, m_module_ref_id, calibObj.request_rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:
                    retValue = String.Format("{0} CAL{1} Started By <{2}>", facilityName, m_module_ref_id, calibObj.started_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:
                    retValue = String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.completed_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                    retValue = String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.completed_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_REJECTED:
                    retValue = String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.request_rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_APPROVED:
                     retValue = String.Format("{0} CAL{1} Complete Requested By <{2}>", facilityName, m_module_ref_id, calibObj.request_approved_by);
                     break;
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                    retValue = String.Format("{0} CAL{1} Completed Rejectde By <{2}>", facilityName, m_module_ref_id, calibObj.rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue = String.Format("{0} CAL{1} Completed ApprovedBy <{2}>", facilityName, m_module_ref_id, calibObj.approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED:
                    retValue = String.Format("{0} CAL{1} Skipped Requested By <{2}>", facilityName, m_module_ref_id, calibObj.skipped_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_REJCTED:
                    retValue = String.Format("{0} CAL{1} Skipped Rejectde By <{2}>", facilityName, m_module_ref_id, calibObj.rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_APPROVED:
                    retValue = String.Format("{0} CAL{1} Skipped Approved By <{2}>", facilityName, m_module_ref_id, calibObj.approved_by);
                    break;
                default:
                    retValue = String.Format("{0} CAL{1} Undefined status {2}", facilityName, m_module_ref_id, m_notificationID);
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", calibObj.status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", "CAL" + calibObj.calibration_id);
            retValue += String.Format(template, "Status", calibObj.status_long + " at " + calibObj.facility_name);
            retValue += String.Format(template, "Equipment Name", calibObj.asset_name);
            retValue += String.Format(template, "Equipment Categories", calibObj.category_name);
            retValue += String.Format(template, "Last Calibration Date", calibObj.last_calibration_date);
            retValue += String.Format(template, "Vendor Name", calibObj.vendor_name);
            retValue += String.Format(template, "Responsible Person Name", calibObj.responsible_person);
            retValue += String.Format(template, "Requested At", calibObj.requested_at);


            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:
                    retValue += "</table>";            //Created
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED:     //Assigned                 
                    retValue += String.Format(templateEnd, "Request Approved By", calibObj.request_approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:     //Closed
                    retValue += String.Format(template, "Request Rejected By", calibObj.request_rejected_by);
                    retValue += String.Format(templateEnd, "Request Rejected At", calibObj.request_rejected_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:
                    retValue += String.Format(templateEnd, "Started At", calibObj.started_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Completed By", calibObj.completed_by);
                    retValue += String.Format(templateEnd, "Completed At", calibObj.completed_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_REJECTED:
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_APPROVED:
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Closed By", calibObj.Closed_by);
                    retValue += String.Format(templateEnd, "Closed At", calibObj.Closed_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED:
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_REJCTED:
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_APPROVED:
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Closed By", calibObj.Closed_by);
                    retValue += String.Format(templateEnd, "Closed At", calibObj.Closed_at);
                    break;
                /* case CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED:
                     retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                     retValue += String.Format(template, "Started At", calibObj.started_at);
                     retValue += String.Format(template, "Completed By", calibObj.completed_by);
                     retValue += String.Format(template, "Completed At", calibObj.completed_at);
                     retValue += String.Format(templateEnd, "Approved By", calibObj.approved_by);
                     break;*/
                /* case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                     retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                     retValue += String.Format(template, "Started At", calibObj.started_at);
                     retValue += String.Format(template, "Completed By", calibObj.completed_by);
                     retValue += String.Format(template, "Completed At", calibObj.completed_at);
                     retValue += String.Format(templateEnd, "Rejected By", calibObj.rejected_by);
                     break;*/

                default:
                    retValue += String.Format(templateEnd, "Closed At", calibObj.Closed_at);
                    break;
            }

            return retValue;
        }


    }
}
