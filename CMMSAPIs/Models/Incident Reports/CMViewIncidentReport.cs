using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMViewIncidentReport
    {
        public string facility_name { get; set; }
        public string block_name { get; set; }
        public string code { get; set; }
        public string reporting_type { get; set; }
        public string incident_type { get; set; }
        public string equipment_name { get; set; }
        public string risk_rating { get; set; }
        public string observation { get; set; }
        public DateTime incident_datetime { get; set; }
        public string reported_by { get; set; }
        public List<AttachmentByReporter> attachment_by_reporter { get; set; }
        public DateTime feedback_at { get; set; }
        public string feedback_by { get; set; }
        public string description_of_action_taken { get; set; }
        public List<AttachmentByReporter> attachment_by_approver { get; set; }
        public string approved_by { get; set; }
        public string status { get; set; }
        public DateTime approved_at { get; set; }
    }

    public class AttachmentByReporter
    {
        public string file_name { get; set; }
        public string category_name { get; set; }
        public float file_size { get; set; }
    }
}
