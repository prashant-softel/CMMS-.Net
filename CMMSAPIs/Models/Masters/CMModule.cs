namespace CMMSAPIs.Models.Masters
{
    public class CMModule
    {
        public int id { get; set; }
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
}
