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
        CMViewInventory m_InvObj;

        public InventoryNotification(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMViewInventory InvObj) : base(moduleID, notificationID)
        {
            m_InvObj = InvObj;
            m_module_ref_id = InvObj.id;
        }

        
        override protected string getEMSubject(params object[] args)
        {
            string retValue = "ESCALATION : ";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue += String.Format("{0} Asset{1} <{2}> Imorted by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.Imported_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue += String.Format("{0} Asset{1} <{2}> Added by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.added_by_name);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue += String.Format("{0} Asset{1} <{1}> Updated by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue += String.Format("{0} Asset{1} <{2}> Deleted by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.deleted_by);
                    break;
                default:
                    break;
            }
            retValue += $" for {m_delayDays} days";
            return retValue;

        }

        protected override string getURL(params object[] args)
        {
            return $"{m_baseURL}/view-add-inventory-screen/{m_module_ref_id}";
        }
        override protected string getSubject(params object[] args)
        {

            string retValue = "";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue = String.Format("{0} Asset{1} <{2}> Imorted by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.Imported_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue = String.Format("{0} Asset{1} <{2}> Added by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.added_by_name);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue = String.Format("{0} Asset{1} <{1}> Updated by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue = String.Format("{0} Asset{1} <{2}> Deleted by {3}", m_InvObj.facilityName, m_InvObj.id, m_InvObj.name, m_InvObj.deleted_by);
                    break;
                default:
                    break;
            }
            return retValue;
        }

        override protected string getHTMLBody(params object[] args)
        {
            string retValue = "";

            retValue = String.Format("<table style='width: 50%; margin: 0 auto; border-collapse: collapse; border-spacing: 10px'><tr><td style='white-space: nowrap;'><h3><b style='color:#31576D'>Status : </b>{0}</h3></td></tr></table>", m_InvObj.status_long + " at " + m_InvObj.facilityName);

            if (m_notificationID != CMMS.CMMS_Status.INVENTORY_IMPORTED)
            {
                retValue += String.Format("<table style='width: 50%; margin:0 auto; border-collapse: collapse ; border-spacing: 10px; ' border='1'>");

                retValue += String.Format(template, "ID","Asset"+m_InvObj.id);
                retValue += String.Format(template, "Name", m_InvObj.name);
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
            if (!string.IsNullOrEmpty(m_InvObj.added_by_name))
            {
                retValue += String.Format(template, "Added By", m_InvObj.added_by_name);
                retValue += String.Format(template, "Added At", m_InvObj.createdAt);
            }
            if (!string.IsNullOrEmpty(m_InvObj.updated_by_name))
            {
                retValue += String.Format(template, "Updated By", m_InvObj.updated_by_name);
                retValue += String.Format(template, "Updated At", m_InvObj.updatedAt);
            }
            if (!string.IsNullOrEmpty(m_InvObj.deleted_by))
            {
                retValue += String.Format(template, "Deleted By", m_InvObj.deleted_by);
                retValue += String.Format(template, "Deleted At", m_InvObj.deleted_at);
            }
            if (m_InvObj.warrantyId > 0)
            {
                retValue += String.Format(template, "Warranty Provider Name", m_InvObj.warrantyProviderName);
                retValue += String.Format(template, "Warranty StartDate", m_InvObj.warranty_start_date);
                retValue += String.Format(template, "Warranty End Date", m_InvObj.warranty_expiry_date);
                retValue += String.Format(template, "Warranty Term Type", m_InvObj.warranty_term_type);
                retValue += String.Format(template, "Warranty Type", m_InvObj.warrantyType);
            }

            retValue += "</table><br><br>";

            retValue += "<div style='text-align:center;'>";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue += "</table>";
                    retValue += String.Format(templateEnd, "Imported By", m_InvObj.Imported_by);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue += String.Format(templateEnd, "Added By", m_InvObj.added_by_name);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue += String.Format(templateEnd, "Updated By", m_InvObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue += String.Format(templateEnd, "Deleted By", m_InvObj.deleted_by);
                    break;
                default:
                    break;
            }

            return retValue;
        }
    }
}
