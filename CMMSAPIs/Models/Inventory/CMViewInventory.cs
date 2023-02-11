using CMMSAPIs.Models.Incident_Reports;
using System;
using static CMMSAPIs.Models.Inventory.CMAddInventory;

namespace CMMSAPIs.Models.Inventory
{
    public class CMViewInventory
    {
        public int id { get; set; }
        public string name { get; set; }
        public string facilityName { get; set; }
        public string blockName { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string categoryName { get; set; }
        public string parentName { get; set; }
        public string customerName { get; set; }
        public string ownerName { get; set; }
        public string operatorName { get; set; }
        public string manufacturerName { get; set; }
        public string supplierName { get; set; }
        public int acCapacity { get; set; }
        public int dcCapacity { get; set; }
        public string model { get; set; }
        public string serialNumber { get; set; }
        public double cost { get; set; }
        public string currency { get; set; }
        public int moduleQuantity { get; set; }
        public string calibrationFrequency { get; set; }
        public string calibrationFreqType { get; set; }
        public int calibrationReminderDays { get; set; }
        public DateTime calibrationLastDate { get; set; }
        public DateTime calibrationDueDate { get; set; }
        public int specialTool { get; set; }
        public string specialToolEmp { get; set; }
        public int warrantyId { get; set; }
        public string warrantyTypeName { get; set; }
        public string warrantyProviderName { get; set; }
        public string warrrantyTermTypeName { get; set; }
        public AttachmentByReporter attachments { get; set; }
        public int mutliplier { get; set; }
        public string status { get; set; }

    }
}
