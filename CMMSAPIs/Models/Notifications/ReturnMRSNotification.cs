using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Notifications
{
    public class ReturnMRSNotification : CMMSNotification
    {
        CMMRSReturnList m_RMRSObj;

        public ReturnMRSNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMRSReturnList RMRSObj) : base(moduleID, notificationID)
        {
            m_RMRSObj = RMRSObj;
            m_module_ref_id = m_RMRSObj.ID;
        }

        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue += String.Format("{0} RMRS{1} Requested By {2}.", m_RMRSObj.facilityName, m_RMRSObj.ID, m_RMRSObj.requested_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue += String.Format("MRS{0} Request Rejected By {1}", m_RMRSObj.ID, m_RMRSObj.request_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue += String.Format("MRS{0} Request Approved By {1}", m_RMRSObj.ID, m_RMRSObj.approver_name);
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }
        override protected string getSubject(params object[] args)
        {
            string retValue = "My Job Card subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue = String.Format("{0} RMRS{1} Requested By {2}.", m_RMRSObj.facilityName, m_RMRSObj.ID, m_RMRSObj.requested_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue = String.Format("{0} RMRS{1} Request Rejected By {2}", m_RMRSObj.facilityName, m_RMRSObj.ID, m_RMRSObj.request_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue = String.Format("{0} RMRS{1} Request Approved By {2}", m_RMRSObj.facilityName, m_RMRSObj.ID, m_RMRSObj.approver_name);
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args) 
        {
            string retValue = "";

            retValue = String.Format("<h3 style='text-align:center;'><b style='color:#31576D'>Status : </b>{0}</h3><br>", m_RMRSObj.status_long + " at " + m_RMRSObj.facilityName);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            
            retValue += String.Format(template, "ID", "RMRS" + m_RMRSObj.ID);
            retValue += String.Format(template, "Status", m_RMRSObj.status_short);
            retValue += String.Format(template, "Activity", m_RMRSObj.activity);
            retValue += String.Format(template, "Where Used", $"{m_RMRSObj.whereUsedTypeName} {m_RMRSObj.whereUsedRefID}");

            if (m_RMRSObj.requested_by_emp_ID > 0)
            {
                retValue += String.Format(template, "Requested By", m_RMRSObj.requested_by_name + " at " + m_RMRSObj.requested_date);
            }
            if (!string.IsNullOrEmpty(m_RMRSObj.request_rejected_by_name))
            {
                retValue += String.Format(template, "Request Rejected By", m_RMRSObj.request_rejected_by_name + " at " + m_RMRSObj.rejected_date);
            }
            if (!string.IsNullOrEmpty(m_RMRSObj.approver_name))
            {
                retValue += String.Format(template, "Request Approved By", m_RMRSObj.approver_name + " at " + m_RMRSObj.approved_date);
            }

            retValue += "</table><br><br>";

            //Return Materials Table
            retValue += "<h4 style='text-align:center;'>Issued Return Materials</h4>";
            retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
            retValue += "<tr>";
            retValue += "<th>Material Name</th>";
            retValue += "<th>Asset Type</th>";
            retValue += "<th>Issued Quantity</th>";
            retValue += "<th>Issued Date</th>";
            retValue += "<th>Return Quantity</th>";
            retValue += "<th>Remarks</th>";
            retValue += "</tr>";

            foreach (var item in m_RMRSObj.CMMRSItems)
            {
                retValue += "<tr>";
                retValue += String.Format("<td>{0}</td>", item.asset_name);
                retValue += String.Format("<td>{0}</td>", item.asset_type);
                retValue += String.Format("<td>{0}</td>", item.issued_qty);
                retValue += String.Format("<td>{0}</td>", item.issued_date);
                retValue += String.Format("<td>{0}</td>", item.returned_qty);
                retValue += String.Format("<td>{0}</td>", item.return_remarks);
                retValue += "</tr>";
            }

            retValue += "</table><br><br>";

                //Faulty Return Material Table
                retValue += "<h4 style='text-align:center;'>Faulty Return Materials</h4>";
                retValue += "<table style='width: 80%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>";
                retValue += "<tr>";
                retValue += "<th>Material Name</th>";
                retValue += "<th>Remove From</th>";
                retValue += "<th>material Code</th>";
                retValue += "<th>Serial No.</th>";
                retValue += "<th>Return Quantity</th>";
                retValue += "<th>Remarks</th>";
                retValue += "</tr>";

                foreach (var item in m_RMRSObj.CMMRSFaultyItems)
                {
                    retValue += "<tr>";
                    retValue += String.Format("<td>{0}</td>", item.asset_name);
                    retValue += String.Format("<td>{0}</td>", item.fromActorName);
                    retValue += String.Format("<td>{0}</td>", item.asset_code);
                    retValue += String.Format("<td>{0}</td>", item.serial_number);
                    retValue += String.Format("<td>{0}</td>", item.returned_qty);
                    retValue += String.Format("<td>{0}</td>", item.return_remarks);
                    retValue += "</tr>";
                }

                retValue += "</table><br><br>";

            retValue += "<div style='text-align:center;'>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue += String.Format(templateEnd, "Requested By", m_RMRSObj.requested_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue += String.Format(templateEnd, "Request Rejected By", m_RMRSObj.request_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue += String.Format(templateEnd, "Request Approved By", m_RMRSObj.approver_name);
                    break;
                default:
                    break;
            }

            return retValue;
        }
    }
}
