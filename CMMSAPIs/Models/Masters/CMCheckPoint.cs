namespace CMMSAPIs.Models.Masters
{
    public class CMCheckPointList : CMCreateCheckPoint
    {
        public string checklist_name { get; set; }        
    }

    public class CMCreateCheckPoint
    {
        public int id { get; set; }
        public int checklist_id { get; set; }
        public string name { get; set; }
        public string requirement { get; set; }
        public string check_point { get; set; }
        public int is_document_required { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public int status { get; set; }
    }
}
