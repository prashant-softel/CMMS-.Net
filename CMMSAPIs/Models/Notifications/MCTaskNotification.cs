using System;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;

namespace CMMSAPIs.Models.Notifications
{
    internal class MCNotificationTask : CMMSNotification
    {

        CMMCExecution taskObj;



        public MCNotificationTask(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecution mcTaskObj) : base(moduleID, notificationID)
        {
            taskObj = mcTaskObj;

        }



        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("{0} MC{1} Started by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("{0} MC{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("{0} MC{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("{0} MC{1} Abandoned by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("{0} MC{1} Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("{0} MC{1} Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("{0} MC{1} Updated by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} MC{1} Abandoned Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} MC{1} Abandoned Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("{0} MC{1} End Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("{0} MC{1} End Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_ASSIGNED:
                    retValue = String.Format("{0} MC{1} Assigned to {2}", taskObj.facilityidName, taskObj.executionId, taskObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REASSIGNED:
                    retValue = String.Format("{0} MC{1} Re Assigned by {2} to {3}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy, taskObj.assignedTo);
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
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue += String.Format("{0} MC{1} Started by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ENDED:
                    retValue += String.Format("{0} MC{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue += String.Format("{0} MC{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue += String.Format("{0} MC{1} Abandoned by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue += String.Format("{0} MC{1} Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue += String.Format("{0} MC{1} Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_UPDATED:
                    retValue += String.Format("{0} MC{1} Updated by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} MC{1} Abandoned Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} MC{1} Abandoned Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue += String.Format("{0} MC{1} End Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue += String.Format("{0} MC{1} End Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.MC_ASSIGNED:
                    retValue = String.Format("{0} MC{1} Assigned to {2}", taskObj.facilityidName, taskObj.executionId, taskObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REASSIGNED:
                    retValue = String.Format("{0} MC{1} Re Assigned by {2} to {3}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy, taskObj.assignedTo);
                    break;

            }
            return retValue;

        }



        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            if (taskObj != null && taskObj.executionId != 0)
            {
                // Wrap the status and table in a div to control layout
                retValue = "<div style='width: 100%; text-align: center; margin-bottom: 0px;'>";

                // Status on top
                retValue += String.Format("<h3><b style='color:#31576D'>Status:</b> {0}</h3>", taskObj.status_long + " at " + taskObj.facilityidName);

                // Close div to ensure the status is on top
                retValue += "</div>";

                // Table for task details
                retValue += "<div style='width: 100%;'>";
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse; border-spacing: 10px;' border='1'>");
                retValue += String.Format(template, "Execution ID", "MCT" + taskObj.executionId);
                retValue += String.Format(template, "Status", taskObj.status_short);
                retValue += String.Format(template, "MC Execution Title", taskObj.title);
                retValue += String.Format(template, "Frequency", taskObj.frequency);

                if (taskObj.assignedToId > 0)
                {
                    retValue += String.Format(template, "MC Task Schedule Assigned to", taskObj.assignedTo);
                }

                if (taskObj.rejectedbyId > 0)
                {
                    retValue += String.Format(template, "MC Task Rejected By", taskObj.rejectedbyName);
                }

                if (taskObj.approvedbyId > 0)
                {
                    retValue += String.Format(template, "MC Task Approved By", taskObj.approvedbyName);
                }

                if (taskObj.startedById > 0)
                {
                    retValue += String.Format(template, "Started By", taskObj.startedBy + " at " + taskObj.startDate);
                }

                if (taskObj.endedById > 0)
                {
                    retValue += String.Format(template, "MC Task Scheduled Closed By", taskObj.endedBy);

                    if (taskObj.endrejectedbyId > 0)
                    {
                        retValue += String.Format(template, "MC Task End Rejected By", taskObj.endrejectedbyName + " At " + taskObj.end_rejected_at);
                    }

                    if (taskObj.endapprovedbyId > 0)
                    {
                        retValue += String.Format(template, "MC Task End Approved By", taskObj.endapprovedbyName + " At " + taskObj.end_approved_at);
                    }
                }

                if (taskObj.abandonedById > 0)
                {
                    retValue += String.Format(template, "MC Task Abandoned By", taskObj.abandonedBy);

                    if (taskObj.abandonRejectedById > 0)
                    {
                        retValue += String.Format(template, "MC Task Abandoned Rejected By", taskObj.abandonRejectedByName);
                    }

                    if (taskObj.abandonApprovedById > 0)
                    {
                        retValue += String.Format(template, "MC Task Abandoned Approved By", taskObj.abandonApprovedByName);
                    }
                }

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.MC_TASK_UPDATED:
                        retValue += String.Format(template, "MC Task Updated By", taskObj.updatedBy);
                        break;

                    case CMMS.CMMS_Status.MC_TASK_REASSIGNED:
                        retValue += String.Format(template, "MC Task Reassigned By", taskObj.updatedBy);
                        break;

                    default:
                        break;
                }

                // Close the table
                retValue += "</table>";

                // Close the wrapping div
                retValue += "</div>";
            }
            return retValue;


        }
    }
}