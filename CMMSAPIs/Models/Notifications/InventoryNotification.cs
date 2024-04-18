using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Models.Notifications
{
    internal class InventoryNotification : CMMSNotification
    {
        int m_InvObjID;
        CMViewInventory m_InvObj;

        public InventoryNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMViewInventory InvObj) : base(moduleID, notificationID)
        {
            m_InvObj = InvObj;
            m_InvObjID = InvObj.id;
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "My job subject";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue += String.Format("Assets Imorted by {1} at {2}</p>", m_InvObj.name, m_InvObj.Imported_by, m_InvObj.Imported_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", m_InvObj.name, m_InvObj.added_by, m_InvObj.added_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue += String.Format("Asset {0} Updated by {1} at {2}</p>", m_InvObj.name, m_InvObj.updated_by, m_InvObj.updated_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue += String.Format("Asset {0} Deleted by {1} at {2}</p>", m_InvObj.name, m_InvObj.deleted_by, m_InvObj.deleted_at);
                    break;
                default:
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<h3><b style='color:#31576D'>Status:</b>{0}</h3><br>", m_InvObj.status_long);

            if (m_notificationID != CMMS.CMMS_Status.INVENTORY_IMPORTED)
            {
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");
                retValue += String.Format(template, "ID", m_InvObj.id);
                retValue += String.Format(template, "Name", m_InvObj.name);
                retValue += String.Format(template, "Facility Name", m_InvObj.facilityName);
                retValue += String.Format(template, "Block Name", m_InvObj.blockName);
                retValue += String.Format(template, "Type", m_InvObj.type);
                retValue += String.Format(template, "Category", m_InvObj.categoryName);
                retValue += String.Format(template, "Description", m_InvObj.asset_description);
            }
            switch (m_notificationID)
            {

                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    //   retValue += String.Format(template, "Added By", m_InvObj.added_by);
                    retValue += String.Format(templateEnd, "Added By", m_InvObj.added_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue += String.Format(template, "Added By", m_InvObj.added_by);
                    //   retValue += String.Format(template, "Added At", m_InvObj.added_by);
                    retValue += String.Format(templateEnd, "Updated By", m_InvObj.updated_by);
                    //   retValue += String.Format(templateEnd, "Updated At", m_InvObj.updated_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue += String.Format(template, "Added By", m_InvObj.added_by);
                    //   retValue += String.Format(template, "Added At", m_InvObj.added_by);
                    retValue += String.Format(templateEnd, "Deleted By", m_InvObj.deleted_by);
                    //   retValue += String.Format(templateEnd, "Deleted At", m_InvObj.updated_at);
                    break;
                default:
                    break;
            }

            return retValue;
        }


    }
}
