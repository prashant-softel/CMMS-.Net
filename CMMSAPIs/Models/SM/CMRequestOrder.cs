using System.Collections.Generic;
using System;

namespace CMMSAPIs.Models.SM
{
    
    public class CMRequestOrder
    {
        public int id { get; set; }
        public int requestID { get; set; }
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        //public int order_by_type { get; set; }
        public string remarks { get; set; }
        public string rejectedRemark { get; set; }
     
        public DateTime? request_date { get; set; }
        public int vendorID { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string generatedBy { get; set; }
        public int receiverID { get; set; }

        public string vendor_name { get; set; }
        public int generate_flag { get; set; }
        public int location_ID { get; set; }
        public DateTime? receivedAt { get; set; }
        public string receivedBy { get; set; }
        public string approvedBy { get; set; }
        public DateTime? approvedAt { get; set; }

        public int receive_later { get; set; }
        public int added_to_store { get; set; }
        public int assetItemID { get; set; }
        public decimal accepted_qty { get; set; }
        public int spare_status { get; set; }
        public DateTime? challan_date { get; set; }
        //public DateTime? requestdate { get; set; }
        public string challan_no { get; set; }
        public string freight { get; set; }
        public string no_pkg_received { get; set; }
        public string lr_no { get; set; }
        public string condition_pkg_received { get; set; }
        public string vehicle_no { get; set; }
        public string gir_no { get; set; }

        public string job_ref { get; set; }
        public decimal amount { get; set; }
        public int currencyID { get; set; }
        public string currency { get; set; }
        public decimal cost { get; set; }
        public string status_long { get; set; }


        public List<CMRequestOrder_ITEMS> go_items { get; set; }
    }
    public class CMRequestOrder_ITEMS
    {
        public int itemID { get; set; }
        public int requestID { get; set; }
        public int assetItemID { get; set; }
        public decimal cost { get; set; }
        public decimal ordered_qty { get; set; }
        public string asset_name { get; set; }
        //public int asset_type_ID { get; set; }
        //public string serial_number { get; set; }
        //public string asset_code { get; set; }
        //public string asset_type { get; set; }
        //public string cat_name { get; set; }
        //public decimal received_qty { get; set; }
        //public decimal damaged_qty { get; set; }
        public decimal accepted_qty { get; set; }

        public string comment { get; set; }


    }

    public class CMRequestOrderList
    {
        public string facilityName { get; set; }
        public int requestDetailsID { get; set; }
        public int facility_id { get; set; }
        public int spare_status { get; set; }
        public string remarks { get; set; }
        public int orderflag { get; set; }
        public int asset_type_ID { get; set; }
        public int requestID { get; set; }
        public int assetItemID { get; set; }
        public string serial_number { get; set; }
        public int location_ID { get; set; }
        public decimal cost { get; set; }
        public decimal ordered_qty { get; set; }
        public string vendor_name { get; set; }
        public DateTime request_date { get; set; }
        public string asset_name { get; set; }
        public int receiverID { get; set; }
        public int vendorID { get; set; }
        public int status { get; set; }
        public string asset_code { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public decimal received_qty { get; set; }
        public decimal damaged_qty { get; set; }
        public decimal accepted_qty { get; set; }
        public string file_path { get; set; }
        public int Asset_master_id { get; set; }
        public int decimal_status { get; set; }
        public int spare_multi_selection { get; set; }
        public int generated_by { get; set; }
        public int asset_type_ID_OrderDetails { get; set; }
        public int receive_later { get; set; }
        public int added_to_store { get; set; }
        public string challan_no { get; set; }
        public string freight { get; set; }
        public string transport { get; set; }
        public string no_pkg_received { get; set; }
        public string lr_no { get; set; }
        public string condition_pkg_received { get; set; }
        public string vehicle_no { get; set; }
        public string gir_no { get; set; }
        public DateTime challan_date { get; set; }
        public string job_ref { get; set; }
        public decimal amount { get; set; }
        public int currencyID { get; set; }
        public string currency { get; set; }
        public string asset_type_Name { get; set; }
        public string rejectedRemark { get; set; }
        public string generatedBy { get; set; }
        public string receivedBy { get; set; }
        public string approvedBy { get; set; }
        public DateTime? approvedAt { get; set; }
        public DateTime? receivedAt { get; set; }
        public string itemcomment { get; set; }
        public string rejectedBy { get; set; }
        public DateTime? rejectedAt { get; set; }
    }

    public class CMCreateRequestOrder
    {
        public int request_order_id { get; set; }
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public decimal cost { get; set; }
        public string comment { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public string rejectedRemark { get; set; }
        public string generatedBy { get; set; }

        public string rejectedBy { get; set; }
        public DateTime? rejectedAt { get; set; }
        public string approvedBy { get; set; }      
        public DateTime? approvedAt { get; set; }
        public DateTime? generatedAt { get; set; }
        public List<CMRequestOrder_ITEMS> request_order_items { get; set; }
    }

}
