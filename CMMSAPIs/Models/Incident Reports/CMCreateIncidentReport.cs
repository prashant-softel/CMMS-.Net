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
        public string severity { get; set; }
        public DateTime incident_datetime { get; set; }
        public int victim_id { get; set; }
        public int action_taken_by { get; set; }
        public DateTime action_taken_datetime { get; set; }
        public int inverstigated_by { get; set; }
        public int verified_by { get; set; }
        public int risk_type { get; set; }
        public bool esi_applicability { get; set; }
        public string esi_applicability_remark { get; set; }
        public bool legal_applicability { get; set; }
        public string legal_applicability_remark { get; set; }
        public bool rca_required { get; set; }
        public string rca_required_remark { get; set; }
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


        // New modifications added 25-11-2023

        public int incidet_type_id { get; set; }
        public string location_of_incident { get; set; }
        public string type_of_job { get; set; }
        public string is_activities_trained { get; set; }
        public string is_person_authorized { get; set; }
        public string instructions_given { get; set; }
        public string safety_equipments { get; set; }
        public string safe_procedure_observed { get; set; }
        public string unsafe_condition_contributed { get; set; }
        public string unsafe_act_cause { get; set; }
        public string is_person_involved { get; set; }
        public List<CMInjured_person> injured_person { get; set; }
        public List<CMWhy_why_analysis> why_why_analysis { get; set; }
        public List<CMRoot_cause> root_cause { get; set; }
        public List<CMImmediate_correction> immediate_correction { get; set; }
        public List<CMProposed_action_plan> proposed_action_plan { get; set; }
        public List<CMInvestigation_team> investigation_team { get; set; }
        public List<int> uploadfile_ids { get; set; }

    }

    public class CMInvestigation_team
    {
        public int investigation_item_id { get; set; }
        public int incidents_id { get; set; }
        public string person_id { get; set; }
        public string person_name { get; set; }
        public int person_type { get; set; }
        public string person_type_name { get; set; }
        public string designation { get; set; }
        public DateTime? investigation_date { get; set; }
    }
    public class CMProposed_action_plan
    {
        public int proposed_item_id { get; set; }
        public int incidents_id { get; set; }
        public string actions_as_per_plan { get; set; }
        public string responsibility { get; set; }
        public DateTime? target_date { get; set; }
        public string remarks { get; set; }
        public string hse_remark { get; set; }
        public int id_Status { get; set; }
    }

    public class CMImmediate_correction
    {
        public int ic_item_id { get; set; }
        public int incidents_id { get; set; }
        public string details { get; set; }
    }

    public class CMRoot_cause
    {
        public int root_item_id { get; set; }
        public int incidents_id { get; set; }
        public string cause { get; set; }
    }
    public class CMWhy_why_analysis
    {
        public int why_item_id { get; set; }
        public int incidents_id { get; set; }
        public string why { get; set; }
        public string cause { get; set; }
    }


    public class CMInjured_person
    {
        public int? injured_item_id { get; set; }
        public int? incidents_id { get; set; }
        public string? name { get; set; }
        public int? person_type { get; set; }
        public string? other_victim { get; set; }
        public int? age { get; set; }
        public int? sex { get; set; }
        public string? designation { get; set; }
        public string? address { get; set; }
        public string? name_contractor { get; set; }
        public string? body_part_and_nature_of_injury { get; set; }
        public int? work_experience_years { get; set; }
        public string? plant_equipment_involved { get; set; }
        public string? location_of_incident { get; set; }
    }

}
