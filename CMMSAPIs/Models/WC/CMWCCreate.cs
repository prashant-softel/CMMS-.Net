using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.WC
{
    public class CMWCCreate
    {
        public int facility_id { get; set; }
        public int equipment_category { get; set; }
        public int equipment_id { get; set; }
        public int supplier_id { get; set; }        
        public List<int> additional_email_employees { get; set; }
        public List<CMWCExternalEmail> external_emails { get; set; }
        public string contact_reference_number { get; set; }
        public DateTime failure_at { get; set; }
        public int cost_of_replacement { get; set; }
        public string currency { get; set; }
        public DateTime warranty_start_at { get; set; }
        public DateTime warranty_end_at { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<IFormFile> equipment_images { get; set; }
        public string action_by_buyer { get; set; }
        public string request_to_supplier { get; set; }
        public List<CMWCSupplierActions> supplier_actions { get; set; }
        public List<IFormFile> attachments { get; set; }
        public int approver_id { get; set; }

    }

    public class CMWCExternalEmail 
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class CMWCSupplierActions 
    {
        public string name { get; set; }
        public bool is_required { get; set; }
        public DateTime required_by_date { get; set; }

    }
}
