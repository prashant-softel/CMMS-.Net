using System;

namespace CMMSAPIs.Models.WC
{
    public class CMWCList
    {
        public int wc_id { get; set; }
        //        public int wc_number { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int site_wc_number { get; set; }
        public string warranty_claim_title { get; set; }
        public DateTime date_of_claim { get; set; }
        public string equipment_category { get; set; }
        public string equipment_name { get; set; }
        public string equipment_sr_no { get; set; }
        public string supplier_name { get; set; }
        public string good_order_id { get; set; }
        public string order_reference_number { get; set; }
        //public string warranty_start_date { get; set; }
        //public string warranty_end_date { get; set; }
        public string warranty_description { get; set; }
        public string affected_part { get; set; }
        public string affected_sr_no { get; set; }
        public DateTime failure_time { get; set; }
        public string estimated_cost { get; set; }
        public int quantity { get; set; }
        public string cost_of_replacement { get; set; }
        public string currency { get; set; }
        public string corrective_action_by_buyer { get; set; }
        public string request_to_supplier { get; set; }
        public int created_by { get; set; }
        public int approved_by { get; set; }
        public string approver_name { get; set; }
        public dynamic last_updated_at { get; set; }
        public dynamic closed_at { get; set; }
        public string status { get; set; }
        public int status_code { get; set; }
        public string material_replinishment_status { get; set; }
        public int claim_status { get; set; }

        public dynamic long_claim_status { get; set; }
    }
}
