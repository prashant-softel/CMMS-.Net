using System;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMIncidentList
    {
        public int id { get; set; }
        public string title { get; set; }
        public string location { get; set; }
        public string risk { get; set; }
        public DateTime created_at { get; set; }
        public string reported_by_name { get; set; }
        public string status { get; set; }
    }
}
