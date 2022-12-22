namespace CMMSAPIs.Models.Utils
{
    public class LogModel
    {
        public int id { get; set; }
        public int module_type { get; set; }
        public int module_ref_id { get; set; }
        public int secondary_module_type { get; set; }
        public int secondary_module_ref_id { get; set; }
        public string comment { get; set; }
        public int status { get; set; }
        public int created_by { get; set; }
        public int created_at { get; set; }
        public string current_latitude { get; set; }
        public string current_longitude { get; set; }
    }
}
