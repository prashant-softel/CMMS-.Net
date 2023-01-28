namespace CMMSAPIs.Models.Masters
{
    public class CMCheckList : CMCreateCheckList
    {
        public string category_name { get; set; }        
        public string frequency_name { get; set; }
    }

    public class CMCreateCheckList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
    }
}
