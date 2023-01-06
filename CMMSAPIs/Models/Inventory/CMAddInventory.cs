using System;

namespace CMMSAPIs.Models.Inventory
{
    public class CMAddInventory : CMWarrantyDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int parent_id { get; set; }
        public string serial_number { get; set; }
        public int calibration_frequency { get; set; }
        public int frequency_type { get; set; }
        public int calibration_reminder { get; set; }
        public DateTime last_calibration_date { get; set; }
        public int manufacturer_id { get; set; }
        public int supplier_id { get; set; }
        public string model { get; set; }
        public int last_price { get; set; }
        public string currency { get; set; }
    }

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
