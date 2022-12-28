using System;

namespace CMMSAPIs.Models.WC
{
    public class CMWCList
    {
        public int wc_id { get; set; }
        public int wc_number { get; set; }
        public int site_wc_number { get; set; }
        public DateTime date_of_claim { get; set; }
        public string country_name { get; set; }
        public string plant_name { get; set; }
        public string title { get; set; }
        public string equipment_category { get; set;}
        public int quantity { get; set; }
        public string supplier { get; set; }
        public string status { get; set; }
        public DateTime last_updated_at { get; set; }
        public DateTime closed_at { get; set; }
        public string material_replinishment_status { get; set; }
        public int estimated_cost { get; set; }

    }
}
