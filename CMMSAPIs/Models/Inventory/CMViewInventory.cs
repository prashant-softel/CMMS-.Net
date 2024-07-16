using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Inventory
{
    public class CMViewInventory
    {
        public int id { get; set; }
        public string name { get; set; }
        public int facilityId { get; set; }
        public string facilityName { get; set; }

        public int stockCount { get; set; }
        public int blockId { get; set; }
        public string blockName { get; set; }
        public string asset_description { get; set; }
        public int typeId { get; set; }
        public int vendorid { get; set; }
        public string meter_limit { get; set; }
        public string meter_unit { get; set; }
        public string type { get; set; }
        public dynamic parent_equipment_no { get; set; }
        public int retirementStatus { get; set; }
        public int categoryId { get; set; }
        public dynamic WarrantyStatus { get; set; }
        public string categoryName { get; set; }
        public int parentId { get; set; }
        public string parentName { get; set; }
        public string parentSerial { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public int ownerId { get; set; }
        public string ownerName { get; set; }
        public int operatorId { get; set; }
        public string operatorName { get; set; }
        public int manufacturerId { get; set; }
        public string manufacturerName { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public int acCapacity { get; set; }
        public int dcCapacity { get; set; }
        public string model { get; set; }
        public string serialNumber { get; set; }
        public double? cost { get; set; }
        public string purchaseCode { get; set; }
        public string unspCode { get; set; }
        public string barcode { get; set; }
        public string descMaintenace { get; set; }
        public string dcRating { get; set; }
        public string acRating { get; set; }
        public string currency { get; set; }
        public int moduleQuantity { get; set; }
        public int photoId { get; set; }
        public int calibrationFrequency { get; set; }
        public string calibrationFreqType { get; set; }
        public int? calibrationReminderDays { get; set; }
        public DateTime calibrationLastDate { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public DateTime calibrationDueDate { get; set; }
        public int specialTool { get; set; }
        public int specialToolEmp { get; set; }
        public int warrantyId { get; set; }
        public string warranty_description { get; set; }
        public string certificate_number { get; set; }
        public string warranty_certificate_path { get; set; }
        public int warrantyTypeId { get; set; }
        public string warrantyType { get; set; }

        public int warrantyProviderId { get; set; }
        public string warrantyProviderName { get; set; }
        public int warrantyTermTypeId { get; set; }
        public string warranty_term_type { get; set; }
        // public AttachmentByReporter attachments { get; set; }
        public int mutliplier { get; set; }
        public int statusId { get; set; }
        public string status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public DateTime? Imported_at { get; set; }
        public string Imported_by { get; set; }
        public DateTime? added_at { get; set; }
        public string added_by { get; set; }
        public DateTime? updated_at { get; set; }
        public string updated_by { get; set; }
        public DateTime? deleted_at { get; set; }
        public string deleted_by { get; set; }
        public List<CMFileDetailJc> inventory_image { get; set; }

    }
    public class CMItem
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
