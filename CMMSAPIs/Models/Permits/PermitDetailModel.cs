using System;
using System.Collections.Generic;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.Permits
{
    public class PermitDetailModel
    {
        public string title { get; set; }
        public string current_status { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string plant_name { get; set; }
        public string permit_number { get; set; }
        public string site_permit_number { get; set; }
        public string permit_type { get; set; }
        public string equipment_categories { get; set; }
        public string block_names { get; set; }
        public List<KeyValuePairs> isolated_equipment_name { get; set; }
        public List<string> safety_question_list { get; set; }
        public string job_type_name { get; set; }
        public string sop_type_name { get; set; }
        public List<FileDetailModel> file_list { get; set; }
        public string created_by_name { get; set; }
        public DateTime created_at { get; set; }
        public string issued_by_name { get; set; }
        public DateTime issue_at { get; set; }
        public string approved_by_name { get; set; }
        public DateTime approve_at { get; set; }
        public string closed_by_name { get; set; }
        public DateTime close_at { get; set; }
        public string cancelled_by_name { get; set; }
        public DateTime cancel_at { get; set; }
    }
}
