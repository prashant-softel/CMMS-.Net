namespace CMMSAPIs.Models.Utils
{
    public class TZone
    {
        public string standard_name { get; set; }
        public string display_name { get; set; }
        public string offset { get; set; }
    }
    public class CMTimeZone
    {
        public int facility_id { get; set; }
        public string timezone { get; set; }     
    }

    public class CMFacilityInfo
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string timezone { get; set; }
    }

}
