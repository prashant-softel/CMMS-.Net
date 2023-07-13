using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMCreateIncidentReport
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int equipment_id { get; set; }
        public int risk_level { get; set; }
        public DateTime incident_datetime { get; set; }
        public int victim_id { get; set; }
        public int action_taken_by { get; set; }
        public DateTime action_taken_datetime { get; set; }
        public int inverstigated_by { get; set; }
        public int verified_by { get; set; }
        public int risk_type { get; set; }
        public bool esi_applicability { get; set; }
        public bool legal_applicability { get; set; }
        public bool rca_required { get; set; }
        public double damaged_cost { get; set; }
        public int damaged_cost_curr_id { get; set; }
        public int generation_loss { get; set; }
        public int generation_loss_curr_id { get; set; }

        public int job_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool is_insurance_applicable { get; set; }
        public string insurance { get; set; }
        public int insurance_id { get; set; }
        public int insurance_status { get; set; }
        public string insurance_remark { get; set; }
        public int approved_by { get; set; }
        
        
    }
}
