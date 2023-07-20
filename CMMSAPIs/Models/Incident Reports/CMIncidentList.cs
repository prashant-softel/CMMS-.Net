using System;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMIncidentList
    {
        public int id { get; set; }
        public string description { get; set; }
        public string equipment_name { get; set; }
        public string facility_name { get; set; }
        public string block_name { get; set; }
        public string severity { get; set; }
        public string approved_by { get; set; }
        public DateTime approved_at { get; set; }
        public string reported_by_name { get; set; }
        public DateTime reported_at { get; set; }
        public string status { get; set; }
    }
}
