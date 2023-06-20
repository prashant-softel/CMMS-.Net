using System.Collections.Generic;
using System;

namespace CMMSAPIs.Models.SM
{
    
    public class CMRequestOrder
    {
        public int id { get; set; }
        public int requestID { get; set; }
        public int facility_id { get; set; }
        public string title { get; set; }
        public int order_by_type { get; set; }
        public string remarks { get; set; }
        public string rejectedRemark { get; set; }
        public int plantID { get; set; }
        public DateTime? request_date { get; set; }
        public int vendorID { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string generatedBy { get; set; }
        public int receiverID { get; set; }

        public string vendor_name { get; set; }
        public int generate_flag { get; set; }
        public int location_ID { get; set; }
        public DateTime? received_on { get; set; }
        public string receivedBy { get; set; }
        public string approvedBy { get; set; }
        public DateTime? approvedOn { get; set; }

        public int receive_later { get; set; }
        public int added_to_store { get; set; }
        public int assetItemID { get; set; }
        public decimal accepted_qty { get; set; }
        public int spare_status { get; set; }
        public DateTime? challan_date { get; set; }
        public DateTime? requestdate { get; set; }
        public string challan_no { get; set; }
        public string freight { get; set; }
        public string no_pkg_received { get; set; }
        public string lr_no { get; set; }
        public string condition_pkg_received { get; set; }
        public string vehicle_no { get; set; }
        public string gir_no { get; set; }

        public string job_ref { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public decimal cost { get; set; }


        public List<CMRequestOrder_ITEMS> go_items { get; set; }
    }
    public class CMRequestOrder_ITEMS
    {
        public int requestID { get; set; }
        public int assetItemID { get; set; }
        public double cost { get; set; }
        public double ordered_qty { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public string serial_number { get; set; }
        public string asset_code { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public double received_qty { get; set; }
        public double damaged_qty { get; set; }
        public double accepted_qty { get; set; }



    }

}
