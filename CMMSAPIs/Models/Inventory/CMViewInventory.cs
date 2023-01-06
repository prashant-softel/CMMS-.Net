using CMMSAPIs.Models.Incident_Reports;
using System;

namespace CMMSAPIs.Models.Inventory
{
    public class CMViewInventory : CMWarrantyDetail
    {
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string facility_name { get; set; }
        public string block_name { get; set; }
        public string parent_name { get; set; }
        public string serial_number { get; set; }
        public int calibration_frequency { get; set; }
        public string frequency_type { get; set; }
        public int calibration_reminder { get; set; }
        public DateTime last_calibration_date { get; set; }
        public string manufacturer_name { get; set; }
        public string supplier_name { get; set; }
        public string model { get; set; }
        public int last_price { get; set; }
        public string currency { get; set; }
        public string warranty_type_name { get; set; }
        public string warranty_provider_name { get; set; }
        public string warrranty_term_type_name { get; set; }
        public AttachmentByReporter attachments { get; set; }

    }
}
