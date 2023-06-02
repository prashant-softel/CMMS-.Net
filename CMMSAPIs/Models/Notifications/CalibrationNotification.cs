using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;


namespace CMMSAPIs.Models.Notifications
{
    internal class CalibrationNotification : CMMSNotification
    {
        int m_jobId;
        CMCalibrationDetails calibObj;

        public CalibrationNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMCalibrationDetails CaliObj) : base(moduleID, notificationID)
        {
            calibObj = CaliObj;
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:                   
                    retValue = String.Format("Calibration Requested <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED:   
                    retValue = String.Format("Calibration Request Approved <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:    
                    retValue = String.Format("Calibration Request Rejected <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:   
                    retValue = String.Format("Calibration Started <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:   
                    retValue = String.Format("Calibration Completed <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:     
                    retValue = String.Format("Calibration Closed <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:    
                    retValue = String.Format("Calibration Approved <{0}> ", calibObj.calibration_id);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:    
                    retValue = String.Format("Calibration Rejected <{0}> ", calibObj.calibration_id);
                    break;
                default:
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
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Completed By", calibObj.completed_by);
                    retValue += String.Format(templateEnd, "Completed At", calibObj.completed_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Closed By", calibObj.Closed_by);
                    retValue += String.Format(templateEnd, "Closed At", calibObj.Closed_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Completed By", calibObj.completed_by);
                    retValue += String.Format(template, "Completed At", calibObj.completed_at);
                    retValue += String.Format(templateEnd, "Approved By", calibObj.approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                    retValue += String.Format(template, "Request Approved by", calibObj.request_approved_by);
                    retValue += String.Format(template, "Started At", calibObj.started_at);
                    retValue += String.Format(template, "Completed By", calibObj.completed_by);
                    retValue += String.Format(template, "Completed At", calibObj.completed_at);
                    retValue += String.Format(templateEnd, "Rejected By", calibObj.rejected_by);
                    break;

                default:
                    break;
            }

            return retValue;
        }

      
    }
}
