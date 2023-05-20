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

}
