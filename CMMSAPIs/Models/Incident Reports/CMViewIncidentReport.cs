using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMViewIncidentReport
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int equipment_id { get; set; }
        public string equipment_name { get; set; }
        public int risk_level { get; set; }
        public string risk_level_name { get; set; }
        public string updated_by_name { get; set; }
        public string created_by_name { get; set; }
        public int created_by_id { get; set; }
        public DateTime incident_datetime { get; set; }
        public int victim_id { get; set; }
        public string victim_name { get; set; }
        public int action_taken_by { get; set; }
        public string action_taken_by_name { get; set; }
        public DateTime action_taken_datetime { get; set; }
        public int inverstigated_by { get; set; }
        public string inverstigated_by_name { get; set; }
        public int verified_by { get; set; }
        public string verified_by_name { get; set; }
        public int risk_type { get; set; }
        public string risk_type_name { get; set; }
        public int esi_applicability { get; set; }
        public string esi_applicability_name { get; set; }
        public int legal_applicability { get; set; }
        public string legal_applicability_name { get; set; }
        public int rca_required { get; set; }
        public string rca_required_name { get; set; }
        public double damaged_cost { get; set; }
        public int generation_loss { get; set; }
        public int job_id { get; set; }
        public string job_name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int is_insurance_applicable { get; set; }
        public string is_insurance_applicable_name { get; set; }
        public string insurance_name { get; set; }
        public int insurance_status { get; set; }
        public string insurance_status_name { get; set; }
        public string insurance_remark { get; set; }
        public int approved_by { get; set; }
        public string approved_by_name { get; set; }
        public int status { get; set; }
        public int historyId { get; set; }
        public string status_name { get; set; }
        public DateTime approved_at { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public string severity { get; set; }
        public string reject_comment { get; set; }


        public List<CMHistoryLIST> LstHistory { get; set; }

        public AttachmentByReporter attachments { get; set; }
    }
    public class CMHistoryLIST
    {
        public int moduleRefId { get; set; }
        public int moduleType { get; set; }
        public string comment { get; set; }
    }
    public class AttachmentByReporter
    {
        public string file_name { get; set; }
        public string category_name { get; set; }
        public float file_size { get; set; }

        public string file_path { get; set; }
    }
}
