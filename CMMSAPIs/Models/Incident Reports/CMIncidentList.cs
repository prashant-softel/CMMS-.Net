using System;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMIncidentList
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string equipment_name { get; set; }
        public string facility_name { get; set; }
        public string type_of_job { get; set; }
        public string location_of_incident { get; set; }
        public DateTime? incident_datetime { get; set; }
        public string block_name { get; set; }
        public string severity { get; set; }
        public string approved_by { get; set; }
        public DateTime approved_at { get; set; }
        public string reported_by_name { get; set; }
        public DateTime reported_at { get; set; }
        public string status_short { get; set; }
        public int status { get; set; }
        public int is_why_why_required { get; set; }
        public int is_investigation_required { get; set; }
     
    }
}
