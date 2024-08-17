using System;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;

namespace CMMSAPIs.Models.Notifications
{
    internal class MCNotification : CMMSNotification
    {
        int VegId;
        CMMCPlan planObj;
        CMMCExecution executionObj;


        public MCNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan mcPlanObj, CMMCExecution mcTaskObj) : base(moduleID, notificationID)
        {
            planObj = mcPlanObj;
            executionObj = mcTaskObj;
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    retValue = String.Format("Vegetation Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                    retValue = String.Format("Vegetation Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue = String.Format("Vegetation Plan <{0}> Rejected by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue = String.Format("Vegetation Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue = String.Format("Vegetation Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("Vegetation Task <{0}> Execution Completed by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("Vegetation Task <{0}>  Execution Abandoned by {1} ", executionObj.id, executionObj.abandonedBy);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            if (planObj.planId != 0)
            {

                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", planObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "ID", planObj.planId);
                retValue += String.Format(template, "Status", planObj.status_short);
                retValue += String.Format(template, "Vegetation Plan Title", planObj.title);
                retValue += String.Format(template, "Frequency", planObj.frequency);
                retValue += String.Format(template, "Created By", planObj.createdBy);


                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                        retValue += "</table>"; break;
                        break;
                    case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                        retValue += String.Format(templateEnd, "Rejected By", planObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                        retValue += String.Format(templateEnd, "Approved By", planObj.approvedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                        retValue += String.Format(templateEnd, "Deleted By", planObj.deletedBy);
                        break;
                    default:
                        break;
                }
            }
            if (executionObj.executionId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", executionObj.status_long);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Execution ID", executionObj.executionId);
                retValue += String.Format(template, "Status", executionObj.status_short);
                retValue += String.Format(template, "Vegetation Plan Title", executionObj.title);
                retValue += String.Format(template, "Frequency", executionObj.frequency);
                retValue += String.Format(template, "started By", executionObj.startedBy);

                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                        retValue += "</table>"; break;
                    case CMMS.CMMS_Status.VEG_TASK_STARTED:
                        retValue += String.Format(templateEnd, "schedule started By", executionObj.startedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                        retValue += String.Format(templateEnd, "schedule Completed By", executionObj.startedBy);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                        retValue += String.Format(template, "Abandoned By", executionObj.abandonedBy);
                        retValue += String.Format(templateEnd, "Reason For Abandon", executionObj.status_short);
                        break;
                }
            }
            return retValue;
        }


    }
}
