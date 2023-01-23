using System;

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
        public string description { get; set; }
        public int type { get; set; }
        public int status { get; set; }
      
        public int parent_id { get; set; }
        public string serial_number { get; set; }
        public int mutliplier { get; set; }
        public int calibration_frequency { get; set; }
        public int frequency_type { get; set; }
        public int calibration_reminder { get; set; }
        public DateTime last_calibration_date { get; set; }
        
        public string model { get; set; }
        public int last_price { get; set; }
        public string currency { get; set; }
        public int moduleQuantity { get; set; }
        public int acCapacity { get; set; }
        public int dcCapacity { get; set; }
        public int category_Id { get; set; }
        public int type_Id { get; set; }
        public int status_Id { get; set; }
        public int facility_Id { get; set; }
        public int block_Id { get; set; }
        public int customer_Id { get; set; }
        public int owner_Id { get; set; }
        public int operator_Id { get; set; }
        public int manufacturer_Id { get; set; }
        public int supplier_Id { get; set; }
        public int serialNumber { get; set; }
        public int warranty_Id { get; set; }
        public int createdAt { get; set; }
        public int createdBy { get; set; }
        public int updatedAt { get; set; }
        public int updatedBy { get; set; }
        public int photoId { get; set; }
        public int cost { get; set; }
        public int stockCount { get; set; }
        public int specialTool { get; set; }
        public int specialToolEmpId { get; set; }
        public DateTime firstDueDate { get; set; }
        public int frequency { get; set; }
        public int descriptionMaintainence { get; set; }
        public int calibrationFrequency { get; set; }
        public int calibrationReminder { get; set; }
        public int retirementStatus { get; set; }
        public int multiplier { get; set; }





        public class CMWarrantyDetail
        {
            public int warranty_type { get; set; }
            public int warranty_provider { get; set; }
            public int warrranty_term_type { get; set; }
            public int meter_limit { get; set; }
            public int meter_unit { get; set; }
            public DateTime expiry_date { get; set; }
            public string certificate_number { get; set; }
            public string warranty_description { get; set; }
        }
    }
}
