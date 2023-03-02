namespace CMMSAPIs.Models.Masters
{
    public class CMModule
    {
        public int id { get; set; }
        public string module_name { get; set; }
        public string feature_name { get; set; }
        public int add { get; set; }
        public int update { get; set; }
        public int delete { get; set; }
        public int view { get; set; }
        public int approve { get; set; }
        public int issue { get; set; }
        public int self_view { get; set; }
    }
}
