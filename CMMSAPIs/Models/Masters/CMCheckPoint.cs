namespace CMMSAPIs.Models.Masters
{
    public class CMCreateCheckPoint
    {
        public int id { get; set; }
        public string check_point { get; set; }
        public int checklist_id { get; set; }
        public string requirement { get; set; }
        public int? is_document_required { get; set; }
        public string action_to_be_done { get; set; }
        public int? status { get; set; }
    }

    public class CMCheckPointList
    {
        public int id { get; set; }
        public string check_point { get; set; }
        public int checklist_id { get; set; }
        public string checklist_name { get; set; }
        public string requirement { get; set; }
        public int? is_document_required { get; set; }
        public string action_to_be_done { get; set; }
        public int created_by_id { get; set; }
        public string created_by_name { get; set; }
        public dynamic created_at { get; set; }
        public int updated_by_id { get; set; }
        public string updated_by_name { get; set; }
        public dynamic updated_at { get; set; }
        public int? status { get; set; }
    }
}
