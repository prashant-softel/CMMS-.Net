﻿using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.WC
{
    public class CMWCDetail
    {
        public int wc_id { get; set; }
        //        public int wc_number { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int site_wc_number { get; set; }
        public string warranty_claim_title { get; set; }
        public dynamic date_of_claim { get; set; }
        public int equipment_category_id { get; set; }
        public string equipment_category { get; set; }
        public int equipment_id { get; set; }
        public string equipment_name { get; set; }
        public int is_required { get; set; }
        public string equipment_sr_no { get; set; }
        public int supplier_id { get; set; }
        public string severity { get; set; }
        public string supplier_name { get; set; }
        public string manufacture_name { get; set; }
        public string good_order_id { get; set; }
        public string order_reference_number { get; set; }
        //public string warranty_start_date { get; set; }
        //public string warranty_end_date { get; set; }
        public string warranty_description { get; set; }
        public string affected_part { get; set; }
        public string affected_sr_no { get; set; }
        public DateTime failure_time { get; set; }
        public int estimated_cost { get; set; }
        public int quantity { get; set; }
        public int cost_of_replacement { get; set; }
        public int approxdailyloss { get; set; }
        public int currencyId { get; set; }
        public string currency { get; set; }
        public string corrective_action_by_buyer { get; set; }
        public string request_to_supplier { get; set; }
        public int created_by { get; set; }
        public int created_by_name { get; set; }
        public dynamic created_at { get; set; }
        public int approved_by { get; set; }
        public string approver_name { get; set; }
        public dynamic approved_at { get; set; }
        public int rejected_by { get; set; }
        public string rejected_by_name { get; set; }
        public dynamic rejected_at { get; set; }
        public dynamic last_updated_at { get; set; }
        public dynamic closed_at { get; set; }
        public int closed_by { get; set; }
        public string closed_by_name { get; set; }
        public int closed_approved_by { get; set; }
        public string closed_approver_name { get; set; }
        public dynamic closed_approved_at { get; set; }
        public int closed_rejected_by { get; set; }
        public string closed_rejected_by_name { get; set; }
        public dynamic closed_rejected_at { get; set; }
        public dynamic cancelled_at { get; set; }
        public int cancelled_by { get; set; }
        public string cancelled_by_name { get; set; }
        public List<CMLog> log { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        //public string srNumber { get; set; }
        public string status_long { get; set; }
        public DateTime warrantyStartDate { get; set; }
        public DateTime warrantyEndDate { get; set; }
        public int claim_status { get; set; }
        public dynamic long_claim_status { get; set; }
        public string updatedbyIdName { get; set; }
        public List<CMWCinternalEmail> additionalEmailEmployees { get; set; }
        public List<CMWCExternalEmail> externalEmails { get; set; }
        public List<CMWCSupplierActions> supplierActions { get; set; }
        public List<affectedParts> affectedParts { get; set; }
        public List<WCFileDetail> affectedPartsImages { get; set; }
        public List<WCFileDetail> Images { get; set; }
    }
    public class WCFileDetail
    {
        public int file_id { get; set; }
        public string description { get; set; }

        public string fileName { get; set; }

    }
}