namespace CMMSAPIs.Models.Masters
{
    public class CMModule
    {
        public int id { get; set; }
        public int software_id { get; set; }
        public string moduleName { get; set; }
        public string featureName { get; set; }
        public string menuImage { get; set; }
        public int? add { get; set; }
        public int? edit { get; set; }
        public int? delete { get; set; }
        public int? view { get; set; }
        public int? approve { get; set; }
        public int? issue { get; set; }
        public int? selfView { get; set; }
    }
    public class CMStatus
    {
        public int module_id { get; set; }
        public int module_software_id { get; set; }
        public string module_name { get; set; }
        public int status_id { get; set; } // only software id will be shown
        public string status_name { get; set; }
    }
}
