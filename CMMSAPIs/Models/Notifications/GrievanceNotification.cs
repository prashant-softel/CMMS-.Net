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
        int m_InvObjID;
        CMGrievance m_InvObj;

        public GrievanceNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMGrievance InvObj) : base(moduleID, notificationID)
        {
            m_InvObj = InvObj;
            m_InvObjID = InvObj.id;
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                
                case CMMS.CMMS_Status.Grievance_ADDED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", m_InvObj.concern, m_InvObj.createdBy, m_InvObj.createdAt);
                    break;
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue += String.Format("Asset {0} Updated by {1} at {2}</p>", m_InvObj.concern, m_InvObj.updatedBy, m_InvObj.updatedAt);
                     break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue += String.Format("Asset {0} Deleted by {1} at {2}</p>", m_InvObj.concern, m_InvObj.deletedBy, m_InvObj.deletedAt);
                    break;
                case CMMS.CMMS_Status.GRIEVANCE_CLOSED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", m_InvObj.concern, m_InvObj.createdBy, m_InvObj.createdAt);
                    break;
                default:
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_InvObj.statusLong);

            
            switch (m_notificationID)
            {

               
                case CMMS.CMMS_Status.Grievance_ADDED:
                    //   retValue += String.Format(template, "Added By", m_InvObj.added_by);
                    retValue += String.Format(templateEnd, "Added By", m_InvObj.createdBy);
                    break;
                case CMMS.CMMS_Status.Grievance_UPDATED:
                    retValue += String.Format(template, "Added By", m_InvObj.createdBy);
                    //   retValue += String.Format(template, "Added At", m_InvObj.added_by);
                    retValue += String.Format(templateEnd, "Updated By", m_InvObj.updatedBy);
                    //   retValue += String.Format(templateEnd, "Updated At", m_InvObj.updated_at);
                    break;
                case CMMS.CMMS_Status.Grievance_DELETED:
                    retValue += String.Format(template, "Added By", m_InvObj.createdBy);
                    //   retValue += String.Format(template, "Added At", m_InvObj.added_by);
                    retValue += String.Format(templateEnd, "Deleted By", m_InvObj.deletedBy);
                    //   retValue += String.Format(templateEnd, "Deleted At", m_InvObj.updated_at);
                    break;
                default:
                    break;
            }

            return retValue;
        }


    }
}
