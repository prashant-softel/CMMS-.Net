using System;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;

namespace CMMSAPIs.Models.Notifications
{
    internal class VEGNotificationTask : CMMSNotification
    {

        CMMCExecution taskObj;



        public VEGNotificationTask(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecution vegTaskObj) : base(moduleID, notificationID)
        {
            taskObj = vegTaskObj;

        }



        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("{0} VE{1} Started by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("{0} VE{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("{0} VE{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("{0} VE{1} Abandoned by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("{0} VE{1} Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("{0} VE{1} Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} VE{1} Updated by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} VE{1} Abandoned Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonRejectedByName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} VE{1} Abandoned Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonApprovedByName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} VE{1} End Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} VE{1} End Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue = String.Format("{0} VE{1} Re Assigned by {2} to {3}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy, taskObj.assignedTo);
                    break;
                case CMMS.CMMS_Status.MC_TASK_REASSIGNED:
                    retValue = String.Format("{0} VEG(1} Re Assigned by {2} to {3}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy, taskObj.assignedTo);
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
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("{0} VE{1} Started by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ENDED:
                    retValue += String.Format("{0} VE{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("{0} VE{1} Closed by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("{0} VE{1} Abandoned by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue += String.Format("{0} VE{1} Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.approvedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue += String.Format("{0} VE{1} Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.rejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} VE{1} Updated by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED:
                    retValue += String.Format("{0} VE{1} Abandoned Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonRejectedByName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED:
                    retValue += String.Format("{0} VE{1} Abandoned Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.abandonApprovedByName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} VE{1} End Approved by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endapprovedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} VE{1} End Rejected by {2}", taskObj.facilityidName, taskObj.executionId, taskObj.endrejectedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                    retValue = String.Format("{0} VE{1} Re Assigned by {2} to {3}", taskObj.facilityidName, taskObj.executionId, taskObj.updatedBy, taskObj.assignedTo);
                    break;
                default:
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
                retValue += String.Format(template, "Execution ID", "VE" + taskObj.executionId);
                retValue += String.Format(template, "Status", taskObj.status_short);
                retValue += String.Format(template, "Title", taskObj.title);
                retValue += String.Format(template, "Frequency", taskObj.frequency);

                if (taskObj.assignedToId > 0)
                {
                    retValue += String.Format(template, "VEG Task Assigned to", taskObj.assignedTo);
                }

                if (taskObj.rejectedbyId > 0)
                {
                    retValue += String.Format(template, "VEG Task Rejected By", taskObj.rejectedbyName);
                }

                if (taskObj.approvedbyId > 0)
                {
                    retValue += String.Format(template, "VEG Task Approved By", taskObj.approvedbyName);
                }

                if (taskObj.startedById > 0)
                {
                    retValue += String.Format(template, "Started By", taskObj.startedBy + " at " + taskObj.startDate);
                }

                if (taskObj.endedById > 0)
                {
                    retValue += String.Format(template, "VEG Task Closed By", taskObj.endedBy);

                    if (taskObj.endrejectedbyId > 0)
                    {
                        retValue += String.Format(template, "VEG Task End Rejected By", taskObj.endrejectedbyName + " At " + taskObj.end_rejected_at);
                    }

                    if (taskObj.endapprovedbyId > 0)
                    {
                        retValue += String.Format(template, "VEG Task End Approved By", taskObj.endapprovedbyName + " At " + taskObj.end_approved_at);
                    }
                }

                if (taskObj.abandonedById > 0)
                {
                    retValue += String.Format(template, "VEG Task Abandoned By", taskObj.abandonedBy);

                    if (taskObj.abandonRejectedById > 0)
                    {
                        retValue += String.Format(template, "VEG Task Abandoned Rejected By", taskObj.abandonRejectedByName);
                    }

                    if (taskObj.abandonApprovedById > 0)
                    {
                        retValue += String.Format(template, "VEG Task Abandoned Approved By", taskObj.abandonApprovedByName);
                    }
                }

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                        retValue += String.Format(template, "VEG Task Updated By", taskObj.updatedBy);
                        break;

                    case CMMS.CMMS_Status.VEG_TASK_ASSIGNED:
                        retValue += String.Format(template, "VEG Task Reassigned By", taskObj.updatedBy);
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