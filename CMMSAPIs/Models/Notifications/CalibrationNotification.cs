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
        }
        override protected string getEMSubject(params object[] args)
        {
            m_calibrationId = calibObj.calibration_id;
            string retValue = "ESCLATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_SCHEDULED:
                    retValue += String.Format("CAL{0} Scheduled ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:
                    retValue += String.Format("CAL{0} Requested ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:
                    retValue += String.Format("CAL{0} Request in Rejected ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED:
                    retValue += String.Format("CAL{0} Request in Approved ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:
                    retValue += String.Format("CAL{0} Request Started ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:
                    retValue += String.Format("CAL{0} Completed ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                    retValue += String.Format("CAL{0} Completed ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_REJECTED:
                    retValue += String.Format("CAL{0} Close Request Rejected ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_APPROVED:
                    retValue += String.Format("CAL{0} Close Request Approved ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                    retValue += String.Format("CAL{0} Close Request Rejected ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue += String.Format("CAL{0} Close Request Approved ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED:
                    retValue += String.Format("CAL{0} Skipped ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_REJCTED:
                    retValue += String.Format("CAL{0} Skip Request Rejected ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_APPROVED:
                    retValue = String.Format("CAL{0} Skipped Approved", calibObj.calibration_id);
                    break;
                default:
                    retValue += String.Format("CAL{0} Undefined status {1}", calibObj.calibration_id, m_notificationID);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_SCHEDULED:
                    retValue = String.Format("CAL{0}  Scheduled", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:
                    retValue = String.Format("CAL{0}  Requested", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:
                    retValue = String.Format("CAL{0}  Request Rejected", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:
                    retValue = String.Format("CAL{0} Started", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:
                    retValue = String.Format("CAL{0} Complete Requested", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                    retValue = String.Format("CAL{0} Complete Requested", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED_REJECTED:
                    retValue = String.Format("CAL{0} Complete Requested", calibObj.calibration_id);
                    break;
               /* case CMMS.CMMS_Status.CALIBRATION_CLOSED_APPROVED:
                    retValue = String.Format("CAL{0} Complete Requested", calibObj.calibration_id);
                    break;*/
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                    retValue = String.Format("CAL{0} Completed Rejectde", calibObj.calibration_id);
                    break;
                /*case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue = String.Format("CAL{0} Completed Approved", calibObj.calibration_id);
                    break;*/
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED:
                    retValue = String.Format("CAL{0} Skipped Requested", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED_REJCTED:
                    retValue = String.Format("CAL{0} Skipped Rejectde", calibObj.calibration_id);
                    break;
                /*case CMMS.CMMS_Status.CALIBRATION_SKIPPED_APPROVED:
                    retValue = String.Format("CAL{0} Skipped Approved", calibObj.calibration_id);
                    break;*/
                default:
                    retValue += String.Format("CAL{0} Undefined status {1}", calibObj.calibration_id, m_notificationID);
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", calibObj.status_long);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID", calibObj.calibration_id);
            retValue += String.Format(template, "Status", calibObj.status_short);
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
