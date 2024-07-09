using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Inventory
{
    public class CMAddInventory //: CMWarrantyDetail  //where is this clas defined??
                                //I tried doing a func f12, but it didnt lead to anything but a revert which i forgot what it said
                                //tmrw ask amit about it. He might have forgot to put in git
                                //test add inventory
                                //what are other api still need to implement??
                                //I think that should be the UpdateInventory
                                //
    {
        public int id { get; set; }
        public string name { get; set; }
        public string assetdescription { get; set; }
        public int typeId { get; set; }
        public int facilityId { get; set; }
        public int blockId { get; set; }
        public int categoryId { get; set; }
        public int statusId { get; set; }
        public int parentId { get; set; }
        public int customerId { get; set; }
        public int ownerId { get; set; }
        public int operatorId { get; set; }
        public int supplierId { get; set; }
        public int manufacturerId { get; set; }
        public int acCapacity { get; set; }
        public int dcCapacity { get; set; }
        public string model { get; set; }
        public string serialNumber { get; set; }
        public int cost { get; set; }
        public string parent_equipment_no { get; set; }

        public double area { get; set; }
        public string currency { get; set; }
        public int currencyId { get; set; }
        public int stockCount { get; set; }
        public int moduleQuantity { get; set; }
        public int photoId { get; set; }
        public int vendorId { get; set; }
        public DateTime? calibrationFirstDueDate { get; set; }
        public DateTime? calibrationNextDueDate { get; set; }
        public DateTime? calibrationLastDate { get; set; } //add this col to database if its not there
        public int calibrationFrequency { get; set; }
        public string calibrationFrequencyType { get; set; }
        public int calibrationReminderDays { get; set; }
        public int retirementStatus { get; set; }
        public int specialToolId { get; set; }
        public int specialToolEmpId { get; set; }
        public int warranty_type { get; set; }
        public string warranty_description { get; set; }
        public int warranty_term_type { get; set; }
        public int warranty_certificate_file_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public int meter_limit { get; set; }
        public int meter_unit { get; set; }
        public int warranty_provider_id { get; set; }
        public string certificate_number { get; set; }
        public int warranty_status { get; set; }

        public int multiplier { get; set; }
        public string acRating { get; set; }
        public string dcRating { get; set; }
        public string descMaintenace { get; set; }
        public int warrantyTenture { get; set; }
        public string barcode { get; set; }
        public string unspCode { get; set; }
        public string purchaseCode { get; set; }
        public List<int> uplaodfile_ids { get; set; }
        public int num_of_module { get; set; }
    }

    public class CMSetParentAsset
    {
        public int parentId { get; set; }
        public List<int> childAssets { get; set; }
    }
}
