using System;

namespace CMMSAPIs.Models.Inventory
{
    public class CMInventoryList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int facilityId { get; set; }
        public string facilityName { get; set; }
        public int blockId { get; set; }
        public string blockName { get; set; }
        public int linkedToBlockId { get; set; }
        public string linkedToBlockName { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public int parentId { get; set; }
        public string parentName { get; set; }
        public string customerName { get; set; }
        public string ownerName { get; set; }
        public string operatorName { get; set; }
        public string serialNumber { get; set; }
        public int specialTool { get; set; }
        public int warrantyId { get; set; }
        public dynamic calibrationDueDate { get; set; }
        public string status { get; set; }
        public int moduleQuantity { get; set; }
        public int dccapacity { get; set; }
        public string acrating { get; set; }
        public string dcRating { get; set; }
        public int acCapacity { get; set; }
        public string descMaintenace { get; set; }
        public string warrantyType { get; set; }
        public string warrantyProviderName { get; set; }
        public DateTime? start_date { get; set; }
        public int warrantyTenture { get; set; }
        public string certificate_number { get; set; }
        public double cost { get; set; }
        public string currency { get; set; }
        public string barcode { get; set; }
        public string unspCode { get; set; }
        public string purchaseCode { get; set; }
        public int calibrationReminderDays { get; set; }
        public int calibrationFrequency { get; set; }
        public DateTime? calibrationLastDate { get; set; }
        public DateTime? calibration_testing_date { get; set; }
        public string model { get; set; }
        public string manufacturername { get; set; }

    }

    public class CMInventoryTypeList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CMInventoryStatusList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CMInventoryCategoryList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? calibration_required { get; set; }
    }

    public class CMWarrantyCertificate
    {

        public int asset_id { get; set; }
        public int categoryId { get; set; }
        public string asset_name { get; set; }
        public int warrantyTypeId { get; set; }
        public int warranty_provider { get; set; }
        public string categoryName { get; set; }
        public string warranty_description { get; set; }
        public string warrantyTypeName { get; set; }
        public int warrantyTermId { get; set; }
        public string warrantyTermName { get; set; }
        public string certificate_number { get; set; }
        public string warranty_certificate_file_path { get; set; }
        public DateTime warrantyStartDate { get; set; }
        public DateTime warrantyExpiryDate { get; set; }
        public int warrantyProviderId { get; set; }
        public string warrantyProviderName { get; set; }
        public string warranty_term_type { get; set; }
    }
    public class CMCalibrationAssets
    {
        public int id { get; set; }
        public string name { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public int vendorId { get; set; }
        public string vendorName { get; set; }
        public string calibrationFreqType { get; set; }
        public int frequencyId { get; set; }
        public string frequencyName { get; set; }
        public string description { get; set; }
        public int calibrationFrequency { get; set; }
        public int calibrationReminderDays { get; set; }
        public DateTime calibrationLastDate { get; set; }
        public DateTime calibrationDueDate { get; set; }
        public string calibrationStatus { get; set; }
    }

}
