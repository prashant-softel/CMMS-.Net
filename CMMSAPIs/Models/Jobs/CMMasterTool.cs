namespace CMMSAPIs.Models.Jobs
{
    public class CMMasterTool
    {
        public int id { get; set; }
        public string linkedToolName { get; set; }
        public string workTypeName { get; set; }
        public string Equipment_name { get; set; }
        public int  equipmentCategoryId { get; set; }
        public int WorkTypeId { get; set; }
    }
}
