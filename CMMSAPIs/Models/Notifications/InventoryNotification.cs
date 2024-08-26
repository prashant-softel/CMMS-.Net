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

        override protected string getEMSubject(params object[] args)
        {

            string retValue = "ESCALALTION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue += String.Format("Asset{0} Imorted by {1}", m_InvObj.id, m_InvObj.Imported_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue += String.Format("Asset{0} Added by {1}", m_InvObj.id, m_InvObj.added_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue += String.Format("Asset{0} Updated by {1}", m_InvObj.id, m_InvObj.updated_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue += String.Format("Asset{0} Deleted by {1}", m_InvObj.id, m_InvObj.deleted_by);
                    break;
                default:
                    retValue += String.Format("Asset <{0}> Undefined status {1} ", m_InvObj.id, m_notificationID);
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
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue = String.Format("Asset{0} Imorted by {1}", m_InvObj.id, m_InvObj.Imported_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue = String.Format("Asset{0} Added by {1}", m_InvObj.id, m_InvObj.added_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue = String.Format("Asset{0} Updated by {1}", m_InvObj.id, m_InvObj.updated_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue = String.Format("Asset{0} Deleted by {1}", m_InvObj.id, m_InvObj.deleted_by);
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
                retValue += String.Format(template, "Asset ID", m_InvObj.id);
                retValue += String.Format(template, "Asset Name", m_InvObj.name);
                retValue += String.Format(template, "Facility Name", m_InvObj.facilityName);
                retValue += String.Format(template, "Block Name", m_InvObj.blockName);
                retValue += String.Format(template, "Type", m_InvObj.type);
                retValue += String.Format(template, "Category", m_InvObj.categoryName);
                retValue += String.Format(template, "Description", m_InvObj.asset_description);
            }
            if (!string.IsNullOrEmpty(m_InvObj.Imported_by))
            {
                retValue += String.Format(template, "Imported By", m_InvObj.Imported_by);
                retValue += String.Format(template, "Imported At", m_InvObj.Imported_at);
            }
            if (!string.IsNullOrEmpty(m_InvObj.added_by))
            {
                retValue += String.Format(template, "Added By", m_InvObj.added_by);
                retValue += String.Format(template, "Added At", m_InvObj.added_at);
            }
            if (!string.IsNullOrEmpty(m_InvObj.updated_by))
            {
                retValue += String.Format(template, "Updated By", m_InvObj.updated_by);
                retValue += String.Format(template, "Updated At", m_InvObj.updated_at);
            }
            if (!string.IsNullOrEmpty(m_InvObj.deleted_by))
            {
                retValue += String.Format(template, "Deleted By", m_InvObj.deleted_by);
                retValue += String.Format(template, "Deleted At", m_InvObj.deleted_at);
            }

            return retValue;
        }


    }
}
