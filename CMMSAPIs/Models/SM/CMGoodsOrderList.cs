using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models
{
    public class CMGoodsOrderList
    {
        public int id { get; set; }
        public int purchaseID { get; set; }
        public int facility_id { get; set; }
        public string title { get; set; }
        public int order_by_type { get; set; }
        public string remarks { get; set; }
        public string rejectedRemark { get; set; }
        public int plantID { get; set; }
        public DateTime? purchaseDate { get; set; }
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
        public int asset_type_ID { get; set; }
        public int asset_type_ID_OrderDetails { get; set; }
        public decimal accepted_qty { get; set; }
        public decimal ordered_qty { get; set; }
        public int spare_status { get; set; }
        public DateTime? challan_date { get; set; }
        public DateTime? po_date { get; set; }
        public string po_no { get; set; }
        public string challan_no { get; set; }
        public string freight { get; set; }
        public string no_pkg_received { get; set; }
        public string lr_no { get; set; }
        public string condition_pkg_received { get; set; }
        public string vehicle_no { get; set; }
        public string gir_no { get; set; }

        public string job_ref { get; set; }
        public int amount { get; set; }
        public int currencyID { get; set; }
        public string currency { get; set; }
        public decimal cost { get; set; }
        public int podID { get; set; }
        public string asset_type_Name { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public string facilityName { get; set; }
        public decimal received_qty { get; set; }
        public decimal lost_qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal damaged_qty { get; set; }
        public string paid_by_name { get; set; }
        public int paid_by_ID { get; set; }
        public int is_submit { get; set; }
        public string cat_name { get; set; }
        public string asset_type { get; set; }
        public int is_splited { get; set; }
        public int requestOrderId { get; set; }
        public int requestOrderItemID { get; set; }
        public string sr_no { get; set; }
        public string freight_value { get; set; }
        public string inspection_report { get; set; }
        public string storage_rack_no { get; set; }
        public string storage_row_no { get; set; }
        public string storage_column_no { get; set; }
        public string closed_by_name { get; set; }
        public string deleted_by { get; set; }
        public string drafted_by_name { get; set; }
        public int drafted_by_id { get; set; }

        public string rejected_by_name { get; set; }
        public string receive_rejected_by_name { get; set; }
        public int receive_approved_by_id { get; set; }
        public DateTime? receive_approved_at { get; set; }
        public string receive_approved_by_name { get; set; }
        public string receive_submit_by_name { get; set; }
        public DateTime? rejectedOn { get; set; }
        public DateTime? rejected_at { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? submitted_at { get; set; }
        public DateTime? withdrawOn { get; set; }
        public DateTime? closed_at { get; set; }
        public DateTime? approved_at { get; set; }
        public DateTime? approvedOn { get; set; }
        public DateTime? updatedOn { get; set; }
        public DateTime? receive_submitted_at { get; set; }
        public DateTime? receive_rejected_at { get; set; }

        public string submitted_by_name { get; set; }




        public List<CMGO_ITEMS> go_items { get; set; }
    }
    public class CMGO_ITEMS
    {
        public int poID { get; set; }
        public int goItemID { get; set; }
        public int assetMasterItemID { get; set; }
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
        public double lost_qty { get; set; }
        public double requested_qty { get; set; }
        public int paid_by_ID { get; set; }
        public string sr_no { get; set; }
        public int requestOrderId { get; set; }
        public int requestOrderItemID { get; set; }
        public string storage_rack_no { get; set; }
        public string storage_row_no { get; set; }
        public string storage_column_no { get; set; }
        public string remarks { get; set; }


    }

    public class CMPURCHASEDATA
    {
        public string facilityName { get; set; }
        public int orderID { get; set; }
        public DateTime? purchaseDate { get; set; }
        public int generate_flag { get; set; }
        public DateTime? received_on { get; set; }
        public int status { get; set; }
        public string vendor_name { get; set; }
        public int? vendorID { get; set; }
        public int generatedByID { get; set; }
        public string remarks { get; set; }
        public string generatedBy { get; set; }
        public string receivedOn { get; set; }
        public string receivedDate { get; set; }
        public string approvedBy { get; set; }
        public string approvedOn { get; set; }
        public int statusFlag { get; set; }

    }

    public class CMSUBMITPURCHASEDATA
    {
        public int id { get; set; }
        public int purchaseID { get; set; }
        public int facilityId { get; set; }

        public string remarks { get; set; }
        public List<CMSUBMITITEMS> submitItems { get; set; }
        public List<IFormFile> attachments { get; set; }
    }
    public class CMSUBMITITEMS
    {
        public string assetCode { get; set; }
        public int assetItemID { get; set; }
        public decimal orderedQty { get; set; }
        public int type { get; set; }
        public decimal cost { get; set; }
    }

    public class CMGOODSORDERLIST
    {
        public string facilityName { get; set; }
        public int orderID { get; set; }
        public DateTime purchaseDate { get; set; }
        public int generate_flag { get; set; }
        public DateTime received_on { get; set; }
        public int status { get; set; }
        public string vendor_name { get; set; }
        public int vendorID { get; set; }
        public int generatedByID { get; set; }
        public string remarks { get; set; }
        public string generatedBy { get; set; }
        public string receivedOn { get; set; }
        public DateTime receivedDate { get; set; }
        public string approvedBy { get; set; }
        public DateTime approvedOn { get; set; }
        public int statusFlag { get; set; }

    }
    public class CMGOList
    {
        public string facilityName { get; set; }
        public int podID { get; set; }
        public string spare_status { get; set; }
        public string remarks { get; set; }
        public string orderflag { get; set; }
        public int asset_type_ID { get; set; }
        public int purchaseID { get; set; }
        public int assetItemID { get; set; }
        public string serial_number { get; set; }
        public int location_ID { get; set; }
        public decimal cost { get; set; }
        public decimal ordered_qty { get; set; }
        public string vendor_name { get; set; }
        public DateTime purchaseDate { get; set; }
        public string asset_name { get; set; }
        public string receiverID { get; set; }
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
        public DateTime po_date { get; set; }
        public string job_ref { get; set; }
        public decimal amount { get; set; }


    }

    public class CMGOMaster
    {
        public int Id { get; set; }
        public int facility_id { get; set; }
        public int vendorID { get; set; }
        public string vendor_name { get; set; }
        //public int asset_type_ID { get; set; }   
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public decimal accepted_qty { get; set; }

        public int currencyID { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string job_ref { get; set; }
        public string gir_no { get; set; }
        public string vehicle_no { get; set; }
        public string condition_pkg_received { get; set; }
        public string lr_no { get; set; }
        public string no_pkg_received { get; set; }
        public DateTime? receivedAt { get; set; }
        public string freight { get; set; }
        public DateTime? po_date { get; set; }
        public string po_no { get; set; }
        public DateTime? challan_date { get; set; }
        public string challan_no { get; set; }
        public DateTime? purchaseDate { get; set; }
        public int location_ID { get; set; }
        public string facilityName { get; set; }
        public string freight_value { get; set; }
        public string inspection_report { get; set; }
        public int purchaseID { get; set; }
        public string title { get; set; }
        public int order_by_type { get; set; }
        public string remarks { get; set; }
        public int drafted_by_id { get; set; }
        public string drafted_by_name { get; set; }
        public DateTime? drafted_at { get; set; }
        public int submitted_by_id { get; set; }
        public string submitted_by_name { get; set; }
        public DateTime? submitted_at { get; set; }
        public int closed_by_id { get; set; }
        public string closed_by_name { get; set; }
        public DateTime? closed_at { get; set; }
        public int deleted_by_id { get; set; }
        public string deleted_by_name { get; set; }
        public DateTime? deleted_at { get; set; }
        public int rejected_by_id { get; set; }
        public string rejected_by_name { get; set; }
        public DateTime? rejected_at { get; set; }
        public int approved_by_id { get; set; }
        public string approved_by_name { get; set; }
        public DateTime? approved_at { get; set; }
        public int receive_draft_by_id { get; set; }
        public string receive_draft_by_name { get; set; }
        public DateTime? receive_draft_at { get; set; }
        public int receive_submitted_by_id { get; set; }
        public string receive_submitted_by_name { get; set; }
        public DateTime? receive_submitted_at { get; set; }
        public int receive_rejected_by_id { get; set; }
        public string receive_rejected_by_name { get; set; }
        public DateTime? receive_rejected_at { get; set; }
        public int receive_approved_by_id { get; set; }
        public string receive_approved_by_name { get; set; }
        public DateTime? receive_approved_at { get; set; }
        public List<CMGODetails> GODetails { get; set; }
    }


    public class CMGODetails
    {
        public int id { get; set; }
        public int assetMasterItemID { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_code { get; set; }
        public string assetItem_Name { get; set; }
        public int location_ID { get; set; }
        public decimal cost { get; set; }
        public decimal ordered_qty { get; set; }
        public decimal received_qty { get; set; }
        public decimal lost_qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal damaged_qty { get; set; }
        public decimal accepted_qty { get; set; }
        //public decimal manager_approve_qty { get; set; }
        //public DateTime lastModifiedDate { get; set; }
        public int spare_status { get; set; }
        public string remarks { get; set; }
        //public int order_type { get; set; }
        public int receive_later { get; set; }

        public string paid_by_name { get; set; }
        public int paid_by_ID { get; set; }
        public string cat_name { get; set; }
        public string asset_type { get; set; }
        public int is_splited { get; set; }
        public int requestOrderId { get; set; }
        public int requestOrderItemID { get; set; }
        public string storage_rack_no { get; set; }
        public string storage_row_no { get; set; }
        public string storage_column_no { get; set; }
        public string sr_no { get; set; }
    }

    public class CMGoodsOrderDetailList
    {
        public int ID { get; set; }
        public int spare_status { get; set; }
        public string remarks { get; set; }
        public int orderflag { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public int purchaseID { get; set; }
        public int assetItemID { get; set; }
        public string serial_number { get; set; }
        public int location_ID { get; set; }
        public decimal cost { get; set; }
        public decimal ordered_qty { get; set; }
        public string rejectedRemark { get; set; }
        public string facility_name { get; set; }
        public int facility_id { get; set; }
        public DateTime? purchaseDate { get; set; }

        public int vendorID { get; set; }
        public int status { get; set; }
        public string asset_code { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public decimal received_qty { get; set; }
        public decimal damaged_qty { get; set; }
        public decimal accepted_qty { get; set; }
        public DateTime? receivedAt { get; set; }
        public DateTime? approvedAt { get; set; }
        public string generatedBy { get; set; }
        public string receivedBy { get; set; }
        public string approvedBy { get; set; }
        public string vendor_name { get; set; }
        public string status_short { get; set; }

    }

    public class CMGOListByFilter
    {
        public int Id { get; set; }
        public int facility_id { get; set; }
        public int vendorID { get; set; }

        public int asset_type_ID { get; set; }
        public string vendor_name { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public decimal accepted_qty { get; set; }

        public int currencyID { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string job_ref { get; set; }
        public string gir_no { get; set; }
        public string vehicle_no { get; set; }
        public string condition_pkg_received { get; set; }
        public string lr_no { get; set; }
        public string no_pkg_received { get; set; }
        public DateTime? receivedAt { get; set; }
        public string freight { get; set; }
        public DateTime? po_date { get; set; }
        public string po_no { get; set; }
        public DateTime? challan_date { get; set; }
        public string challan_no { get; set; }
        public DateTime? purchaseDate { get; set; }
        public int location_ID { get; set; }
        public string facilityName { get; set; }
        public decimal cost { get; set; }
        public string generatedBy { get; set; }

    }

    public class CMGODashboardList
    {
        public int go_id { get; set; }
        public int facilityId { get; set; }
        public string facilityName { get; set; }
        public string GRNo { get; set; }
        public string GONo { get; set; }
        public int status { get; set; }
        public string product_name { get; set; }
        public decimal requested_qty { get; set; }
        public DateTime? gr_date { get; set; }
        public decimal ordered_qty { get; set; }
        public DateTime? go_date { get; set; }
        public decimal unit_amount { get; set; }
        public int total_amount { get; set; }
        public DateTime? grn_date { get; set; }
        public decimal grn_qty { get; set; }
    }
}
