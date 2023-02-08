namespace CMMSAPIs.Models.Inventory
{
    public class CMInventoryList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string category_name { get; set; }
        public string block_name { get; set; }
        public string parent_name { get; set; }
        public string serial_number { get; set; }
        public string customer_name { get; set; }
        public string owner_name { get; set; }
        public string operator_name { get; set; }
        public string status { get; set; }

    }

    public class CMInventoryTypeList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status{ get; set; }
    }

    public class CMInventoryStatusList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class CMInventoryCategoryList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

}
