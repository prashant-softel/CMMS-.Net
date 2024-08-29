using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Grievance;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class GrievanceNotification : CMMSNotification
    {
        //int WCId;
        CMGrievance GrievanceObj;

        public GrievanceNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMGrievance GObj) : base(moduleID, notificationID)
        {
            GrievanceObj = GObj;
           
        }

        override protected string getEMSubject(params object[] args)
        {

            string retValue = "Escalaltion : ";

            switch (m_notificationID)
            {

                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue += String.Format("Grievance {0} Added by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.createdByName, GrievanceObj.createdAt);
                    break;
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue += String.Format("Grievance {0} Updated by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.updatedByName, GrievanceObj.updatedAt);
                    break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue += String.Format("Grievance {0} Deleted by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.deletedByName, GrievanceObj.deletedAt);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_CLOSED:
                    retValue += String.Format("Grievance {0} Added by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.closedByName, GrievanceObj.closedAt);
                    break;
                default:
                    retValue += String.Format("Grievance <{0}> Undefined status {1} ", GrievanceObj.grievanceType, m_notificationID);
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
                
                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue += String.Format("Grievance {0} Added by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.createdByName, GrievanceObj.createdAt);
                    break;
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue += String.Format("Grievance {0} Updated by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.updatedByName, GrievanceObj.updatedAt);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_ONGOING:
                    retValue += String.Format("Grievance {0} Ongoing, created by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.createdByName, GrievanceObj.createdAt);
                     break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue += String.Format("Grievance {0} Deleted by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.deletedByName, GrievanceObj.deletedAt);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_CLOSED:
                    retValue += String.Format("Grievance {0} Added by {1} at {2}</p>", GrievanceObj.grievanceType, GrievanceObj.closedByName, GrievanceObj.closedAt);
                    break;
                default:
                    retValue += String.Format("Grievance <{0}> Undefined status {1} ", GrievanceObj.grievanceType, m_notificationID);
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", GrievanceObj.statusLong);

            retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
            retValue += String.Format(template, "ID" ,GrievanceObj.id);
            retValue += String.Format(template, "Status", GrievanceObj.statusShort);
            retValue += String.Format(template, "Grievance Type", GrievanceObj.grievanceType);
            if(GrievanceObj.createdByName != null)
            {
                retValue += String.Format(template, "Created By", GrievanceObj.createdByName);
            }
            else if(GrievanceObj.updatedByName != null)
            {
                retValue += String.Format(template, "Updated By", GrievanceObj.updatedByName);
            }
            else if(GrievanceObj.deletedByName != null)
            {
                retValue += String.Format(template, "Deleted By", GrievanceObj.deletedByName);
            }
            else if(GrievanceObj.closedByName != null)
            {
                retValue += String.Format(template, "Closed By", GrievanceObj.closedByName);
            }
            
            

            switch (m_notificationID)
            {

               
                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue += String.Format(templateEnd, "Added By", GrievanceObj.createdByName);
                    break;
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue += String.Format(templateEnd, "Updated By", GrievanceObj.updatedByName);
                    break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue += String.Format(templateEnd, "Deleted By", GrievanceObj.deletedByName);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_ONGOING:
                    retValue += String.Format(templateEnd, "Deleted By", GrievanceObj.createdByName);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_CLOSED:
                    retValue += String.Format(templateEnd, "Closed By", GrievanceObj.closedByName);
                    break;
                default:
                    retValue += String.Format(templateEnd, "Grievance undefined for ", m_notificationID);
                    break;
            }

            return retValue;
        }


    }
}
