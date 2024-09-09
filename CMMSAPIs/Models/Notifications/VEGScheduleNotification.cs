using System;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;

namespace CMMSAPIs.Models.Notifications
{
    internal class VEGScheduleNotification : CMMSNotification
    {
        int VegId;
        //CMMCPlan planObj;
        CMMCExecutionSchedule scheduleObj;

        public VEGScheduleNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCExecutionSchedule mcScheduleObj) : base(moduleID, notificationID)
        {
            scheduleObj = mcScheduleObj;

        }

    
         
        override protected string getEMSubject(params object[] args)
        {

            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue += String.Format("{0} SCH{1} Started by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.startedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("{0} SCH{1} Closed by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("{0} SCH{1} Abandoned by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} SCH{1} Approved by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} SCH{1} Rejected by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("{0} PTW{1} Linked with SCH{2} of MCT{3}", scheduleObj.facilityidName, scheduleObj.permit_id, scheduleObj.scheduleId, scheduleObj.executionId);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} SCH{1} Updated by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.updatedbyName);
                    break;

                default:
                   
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
                    retValue += String.Format("{0} SCH{1} Started by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.startedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue += String.Format("{0} SCH{1} Closed by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.endedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue += String.Format("{0} SCH{1} Abandoned by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.abandonedbyName);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_APPROVED:
                    retValue += String.Format("{0} SCH{1} Close Approved by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_END_REJECTED:
                    retValue += String.Format("{0} SCH{1} Close Rejected by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue += String.Format("{0} PTW{1} Linked with SCH{2} of MCT{3}", scheduleObj.facilityidName, scheduleObj.permit_id, scheduleObj.scheduleId, scheduleObj.executionId);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                    retValue += String.Format("{0} SCH{1} Updated by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, scheduleObj.updatedbyName);
                    break;
                default:
                    retValue += String.Format("{0} SCH{1} unsupported status by {2}", scheduleObj.facilityidName, scheduleObj.scheduleId, m_notificationID);
                    break;
            }
            return retValue;

        }



        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";
            if (scheduleObj != null && scheduleObj.scheduleId != 0)
            {
                retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", scheduleObj.status_long_schedule + " at " + scheduleObj.facilityidName);

                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "Schedule ID", "SCH" + scheduleObj.scheduleId );
                retValue += String.Format(template, "Task ID", "VE" + scheduleObj.executionId);
                retValue += String.Format(template, "Status", scheduleObj.status_short);
                // retValue += String.Format(template, "cleaned Status", scheduleObj.cleaned);
                retValue += String.Format(template, "Title", scheduleObj.title);
                if (!string.IsNullOrEmpty(scheduleObj.description))
                {
                    retValue += String.Format(template, "Description", scheduleObj.description);
                }

                retValue += String.Format(template, "Cleaning Day", scheduleObj.cleaningDay);
                retValue += String.Format(template, "Cleaning Type Name", scheduleObj.cleaningTypeName);
                //retValue += String.Format(template, "waterUsed", scheduleObj.waterUsed);

                if (scheduleObj.startedById > 0)
                {
                    retValue += String.Format(template, "Started By", scheduleObj.startedbyName + " at " + scheduleObj.start_date);
                }
                if (scheduleObj.rejectedById > 0)
                {
                    retValue += String.Format(template, "Rejected By", scheduleObj.rejectedBy + " at " + scheduleObj.rejectedAt);
                }
                if (scheduleObj.approvedById > 0)
                {
                    retValue += String.Format(template, "Approved By", scheduleObj.approvedBy + " at " + scheduleObj.approvedAt);
                }
                if (scheduleObj.abandonedById > 0)
                {
                    retValue += String.Format(template, "Abandoned By", scheduleObj.abandonedbyName + " at " + scheduleObj.abandonedAt);
                }

                if (scheduleObj.endedById > 0)
                {
                    retValue += String.Format(template, "Ended By", scheduleObj.endedbyName + " at " + scheduleObj.end_date);
                }

                if (scheduleObj.permit_id > 0)
                {
                    retValue += String.Format(template, "Permit Code", scheduleObj.permit_code);
                    retValue += String.Format(template, "PTW status", scheduleObj.status_short_ptw);
                }
                switch (m_notificationID)
                {
                    case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                        retValue += String.Format(template, "Permit ID Linked by ", scheduleObj.updatedbyName);
                        break;
                    case CMMS.CMMS_Status.VEG_TASK_UPDATED:
                        retValue += String.Format(template, "Updated by", scheduleObj.updatedbyName + " at " + scheduleObj.updatedAt);
                        break;

                    /* 
                     case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                         retValue += "</table>"; break;


                     case CMMS.CMMS_Status.MC_TASK_STARTED:
                         retValue += String.Format(templateEnd, "MC Task Started By ", scheduleObj.startedbyName);
                         break;


                     case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                         retValue += String.Format(templateEnd, "MC Execution Abandoned By ", scheduleObj.abandonedbyName);
                         break;
                     case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                         retValue += String.Format(templateEnd, "MC Execution Rejected By ", scheduleObj.rejectedBy);
                         break;
                    */

                    default:
                        break;

                }
                retValue += "</table>";
            }
            return retValue;
        }


    }
}
