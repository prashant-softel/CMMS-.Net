using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Collections.Generic;

namespace CMMSAPIs.Models.JC
{
    public class JCDetailModel
    {
        public int id { get; set; }
        public string current_status { get; set; }
        public string plant_name { get; set; }
        public string block { get; set; }
        public string asset_category_name { get; set; }
        public int job_id { get; set; }
        public string job_title { get; set; }
        public string job_assigned_employee_name { get; set; }
        public string job_description { get; set; }
        public string work_type { get; set; }
        public string linked_tool_to_work_type { get; set; }
        public string standard_action { get; set; }
        public int permit_id { get; set; }
        public string site_permit_no { get; set; }
        public string permit_type { get; set; }
        public string permit_description { get; set; }
        public string job_created_by_name { get; set; }
        public string permit_issued_by_name { get; set; }
        public string permit_approved_by_name { get; set; }        
        public string isolated_assest { get; set; }
        public string isolated_assest_loto { get; set; }
        public string employees { get; set; }
        public string files { get; set; }
        public string history { get; set; }

    }
}
