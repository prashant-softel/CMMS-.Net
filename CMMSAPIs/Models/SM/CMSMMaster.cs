using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.SM
{
    public class CMSMMaster
    {
        public int ID { get; set; }
        public string asset_type { get; set; }
        public int flag { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_code { get; set; }
        public string asset_name { get; set; }
        public string description { get; set; }
        public int approval_required { get; set; }
        public string cat_name { get; set; }
        public string measurement { get; set; }
        public double decimal_status { get; set; }
        public string asset_description { get; set; }
        public int item_category_ID { get; set; }
        public int unit_measurement_ID { get; set; }
        public int approval_required_ID { get; set; }
        public int min_req_qty { get; set; }
        public int reorder_qty { get; set; }
        public string section { get; set; }
        public CMAssetMasterFiles fileData { get; set; }


    }

    public class CMAssetTypes
    {
        public int ID { get; set; }
        public string asset_type { get; set; }
        public int status { get; set; }
    }
    public class CMItemCategory
    {
        public int ID { get; set; }
        public string cat_name { get; set; }
        public int status { get; set; }
    }


    public class CMUnitMeasurement
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int flag { get; set; }
        public int decimal_status { get; set; }
        public int spare_multi_selection { get; set; }
    }

    //public class ItemCategory
    //{
    //    public int ID { get; set; }
    //    public string cat_name { get; set; }
    //    public int flag { get; set; }
    //}

    public class CMAssetMasterFiles
    {
        public int id { get; set; }
        public int Asset_master_id { get; set; }
        public string File_path { get; set; }
        public string File_name { get; set; }
        public string File_type { get; set; }
        public string File_size { get; set; }
        public DateTime? Uploaded_date { get; set; }
    }

    public class CMMRS
    {
        public int ID { get; set; }
        public int mrsreturnID { get; set; }
        public int requested_by_emp_ID { get; set; }
        public string approver_name { get; set; }
        public DateTime? requestd_date { get; set; }
        public DateTime? returnDate { get; set; }
        public DateTime? approval_date { get; set; }
        public int approval_status { get; set; }
        public string approval_comment { get; set; }
        public string emp_name { get; set; }
        public int flag { get; set; }

        public int facility_ID { get; set; }
        public int approved_by_emp_ID { get; set; }
        public DateTime approved_date { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string reference { get; set; }
        public int referenceID { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public string setAsTemplate { get; set; }
        public string templateName { get; set; }
        public int approval_manager_ID { get; set; }
        public DateTime? approval_manager_date { get; set; }
        public int setTemplateflag { get; set; }

        public string return_remarks { get; set; }
        public int mrs_return_ID { get; set; }
        public string finalRemark { get; set; }
        public int asset_item_ID { get; set; }
        public string asset_MDM_code { get; set; }
        public string serial_number { get; set; }
        public string returned_qty { get; set; }
        public decimal available_qty { get; set; }
        public decimal used_qty { get; set; }

        public decimal issued_qty { get; set; }
        public DateTime? issued_date { get; set; }
        public decimal requested_qty { get; set; }
        public string approval_required { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string file_path { get; set; }
        public int Asset_master_id { get; set; }
        public int isEditMode { get; set; }
        public int item_condition { get; set; }
        public string activity { get; set; }
        public int whereUsedType { get; set; }
        public string whereUsedTypeName { get; set; }
        public int whereUsedRefID { get; set; }
        public string? remarks { get; set; }
        public int from_actor_type_id { get; set; }
        public int from_actor_id { get; set; }
        public int to_actor_type_id { get; set; }
        public int to_actor_id { get; set; }
        public int is_submit { get; set; }
        public string issue_comment { get; set; }

        public List<CMEquipments> cmmrsItems { get; set; }
        public List<CMFaultyItems> faultyItems { get; set; }

    }

    public class CMFaultyItems
    {
        public int mrs_item_ID { get; set; }
        public int assetMasterItemID { get; set; }
        public int faulty_item_asset_id { get; set; }
        public int returned_qty { get; set; }
        public string return_remarks { get; set; }
        public string serial_number { get; set; }
        public string sr_no { get; set; }
    }

    public class CMEquipments
    {
        public int id { get; set; }
        public int mrs_item_id { get; set; }
        public int material_id { get; set; }
        public int asset_item_ID { get; set; }
        public decimal qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal issued_qty { get; set; }
        public string return_remarks { get; set; }
        public decimal returned_qty { get; set; }
        public decimal received_qty { get; set; }
        public decimal used_qty { get; set; }
        public decimal available_qty { get; set; }
        public int is_faulty { get; set; }
        public string issue_remarks { get; set; }
        public string serial_number { get; set; }
    }

    public class CMVendorList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CMMRSItems
    {
        public int ID { get; set; }
        public int mrs_item_id { get; set; }
        public string return_remarks { get; set; }
        public int original_mrs_ID { get; set; }
        public int mrs_return_ID { get; set; }
        public int fromActorID { get; set; }
        public string fromActorType { get; set; }
        public string fromActorName { get; set; }
        public string finalRemark { get; set; }
        public int asset_item_ID { get; set; }
        public string asset_code { get; set; }
        //public string serial_number { get; set; }
        public decimal requested_qty { get; set; }
        public decimal returned_qty { get; set; }
        public decimal available_qty { get; set; }
        public decimal used_qty { get; set; }
        public decimal issued_qty { get; set; }
        //public int flag { get; set; }
        //public string returnDate { get; set; }
        //public int approval_status { get; set; }
        public string approved_date { get; set; }
        public string issued_date { get; set; }

        public string approval_required { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        //public string file_path { get; set; }
        //public int Asset_master_id { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public int is_splited { get; set; }
        public int materialID { get; set; }
        public int is_faulty { get; set; }
        public int assetMasterID { get; set; }
        public string serial_number { get; set; }
        public int transaction_id { get; set; }
        public int faulty_item_asset_id { get; set; }

    }
    public class Cmmrs1
    {
        public int actor_id { get; set; }
        public dynamic actor_type { get; set; }
        public string actorname { get; set; }

    }

    public class CMAssetBySerialNo
    {
        public int ID { get; set; }
        public int plant_ID { get; set; }
        public string asset_code { get; set; }
        public string serial_number { get; set; }
        public int replaced_asset_id { get; set; }
        public int replaced_serial_number { get; set; }
        public int location_ID { get; set; }
        public int item_condition { get; set; }
        public int status { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int orderflag { get; set; }

    }

    public class CMAssetItem
    {
        public int asset_type_ID { get; set; }
        public int asset_ID { get; set; }
        public int materialID { get; set; }
        public string asset_code { get; set; }
        public string cat_name { get; set; }
        public string serial_number { get; set; }
        public int ID { get; set; }
        public string asset_name { get; set; }
        public string asset_type { get; set; }
        public string approval_required { get; set; }
        public string file_path { get; set; }
        public int Asset_master_id { get; set; }
        public int spare_multi_selection { get; set; }
        public decimal available_qty { get; set; }
        public long is_approval_required { get; set; }


    }

    public class CMMRSItemsBeforeIssue
    {
        public int ID { get; set; }
        public string return_remarks { get; set; }
        public int mrs_return_ID { get; set; }
        public string finalRemark { get; set; }
        public int asset_item_ID { get; set; }
        public string asset_MDM_code { get; set; }
        public decimal returned_qty { get; set; }
        public decimal available_qty { get; set; }
        public decimal used_qty { get; set; }
        public decimal issued_qty { get; set; }
        public int flag { get; set; }
        public string returnDate { get; set; }
        public int approval_status { get; set; }
        public string approved_date { get; set; }
        public string issued_date { get; set; }

        public decimal requested_qty { get; set; }
        public string approval_required { get; set; }
        public string asset_name { get; set; }
        public string asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string file_path { get; set; }
        public string Asset_master_id { get; set; }
        public string spare_multi_selection { get; set; }

    }

    public class CMMRSList
    {
        public int ID { get; set; }
        public int facilityId { get; set; }
        public string facilityName { get; set; }
        public int requested_by_emp_ID { get; set; }
        public int approved_by_emp_ID { get; set; }
        public string approver_name { get; set; }
        public string? requestd_date { get; set; }
        public string returnDate { get; set; }
        public string? issued_date { get; set; }
        public dynamic issuedAt { get; set; }
        public string approval_date { get; set; }
        public int approval_status { get; set; }
        public string approval_comment { get; set; }
        public string requested_by_name { get; set; }
        public string issued_name { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public string activity { get; set; }
        //public int whereUsedType { get; set; }
        public string whereUsedTypeName { get; set; }
        public int whereUsedRefID { get; set; }
        public string remarks { get; set; }
        public int is_splited { get; set; }
        public int is_mrs_return { get; set; }
        public string request_updated_by_name { get; set; }
        public int updated_by_emp_ID { get; set; }
        public string request_rejected_by_name { get; set; }
        public dynamic rejected_by_emp_ID { get; set; }
        public dynamic request_rejected_at { get; set; }
        public string issue_appoved_by_name { get; set; }
        public int issue_approved_by_emp_ID { get; set; }
        public string issue_rejected_by_name { get; set; }
        public int issue_rejected_by_emp_ID { get; set; }
        public string updated_by_emp { get; set; }
        public string rejected_by_emp { get; set; }
        public string issue_approved_by_emp { get; set; }
        public string issue_rejected_by_emp { get; set; }
        public int UpdatedByEmpID { get; set; }
        public int RejectedByEmpID { get; set; }
        public int IssueApprovedByEmpID { get; set; }
        public int IssueRejectedByEmpID { get; set; }
        public DateTime? issue_approved_date { get; set; }
        public DateTime? issue_rejected_date { get; set; }
        public List<CMMRSItems> CMMRSItems { get; set; }
    }

    public class CMMRSReturnList
    {
        public int ID { get; set; }
        public int mrs_id { get; set; }
        public int facilityId { get; set; }
        public string facilityName { get; set; }
        public int requested_by_emp_ID { get; set; }
        public string approver_name { get; set; }
        public DateTime? requested_date { get; set; }
        public string returnDate { get; set; }
        public string? issued_date { get; set; }
        public DateTime? approved_date { get; set; }
        public int approval_status { get; set; }
        public string approval_comment { get; set; }
        public string requested_by_name { get; set; }
        public string issued_name { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public string activity { get; set; }
        //public int whereUsedType { get; set; }
        public string whereUsedTypeName { get; set; }
        public int whereUsedRefID { get; set; }
        public string remarks { get; set; }
        public int is_splited { get; set; }
        public int is_mrs_return { get; set; }
        public int return_mrs { get; set; }
        public string request_rejected_by_name { get; set; }
        public dynamic rejected_date { get; set; }
        public List<CMMRSItems> CMMRSItems { get; set; }
        public List<CMMRSItems> CMMRSFaultyItems { get; set; }
    }

    public class CMMRSListByModule
    {
        public int mrsId { get; set; }
        public int is_mrs_return { get; set; }
        public int mrs_return_ID { get; set; }
        public int jobId { get; set; }
        public int jobCardId { get; set; }
        public int pmId { get; set; }
        public string mrsItems { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public List<CMMRSItems> CMMRSItems { get; set; }
        public List<IDASETS> material_used_by_assets { get; set; }

    }
    public class ASSETSITEM
    {
        public dynamic sm_asset_id { get; set; }
        public string sm_asset_name { get; set; }
        public dynamic mrs_Item_Id { get; set; }

        public dynamic used_qty { get; set; }
    }
    public class IDASETS
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public List<ASSETSITEM> Items { get; set; }
    }
    public class CMRETURNMRSDATA
    {
        public int ID { get; set; }
        public int mrs_ID { get; set; }
        public int mrs_return_ID { get; set; }
        public int asset_item_ID { get; set; }
        public string asset_MDM_code { get; set; }
        public decimal requested_qty { get; set; }
        public decimal issued_qty { get; set; }
        public decimal returned_qty { get; set; }
        public decimal used_qty { get; set; }
        public string return_remarks { get; set; }
        public int approval_required { get; set; }
        public int status { get; set; }
        public int flag { get; set; }
        public DateTime lastmodifieddate { get; set; }
        public decimal available_qty { get; set; }
        public string finalRemark { get; set; }

    }

    public class CMASSETMASTERLIST
    {
        public int ID { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string asset_code { get; set; }
        public string cat_name { get; set; }
        public string asset_name { get; set; }
        public string description { get; set; }
        public string approval_required { get; set; }
        public string section { get; set; }
        public int reorder_qty { get; set; }
        public int min_qty { get; set; }
        public string measurement { get; set; }
        public int decimal_status { get; set; }

    }

    public class CMMRSAssetTypeList
    {
        public string asset_type { get; set; }
        public string asset_code { get; set; }
        public string asset_name { get; set; }
        public int ID { get; set; }
        public int item_ID { get; set; }

        public string serial_number { get; set; }
        public int asset_type_ID { get; set; }
        public int decimal_status { get; set; }
        public string file_path { get; set; }
        public int Asset_master_id { get; set; }
        public int spare_multi_selection { get; set; }
        public decimal available_qty { get; set; }
        public int facility_ID { get; set; }

    }

    public class CMGETASSETDATALIST
    {
        public int asset_ID { get; set; }
        public string asset_code { get; set; }
        public string CategoryName { get; set; }
        public string serial_number { get; set; }
        public string asset_name { get; set; }
        public string asset_type { get; set; }
        public string approval_required { get; set; }

    }

    public class CMPaidBy
    {
        public int ID { get; set; }
        public string paid_by { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public int created_by { get; set; }
        public DateTime updated_at { get; set; }
        public int updated_by { get; set; }
    }
    public class CMEmployeeStock
    {
        public int ID { get; set; }
        public int emp_ID { get; set; }
        public string emp_name { get; set; }

        public List<CMStockItems> CMMRSItems { get; set; }
    }

    public class CMStockItems
    {
        public int ID { get; set; }
        public int item_name { get; set; }
        public decimal quantity { get; set; }

    }

    public class CMTransferItems
    {
        public int facilityID { get; set; }
        public int fromActorID { get; set; }
        public int fromActorType { get; set; }
        public int toActorID { get; set; }
        public int toActorType { get; set; }
        public int assetItemID { get; set; }
        public int qty { get; set; }
        public int refType { get; set; }
        public int refID { get; set; }
        public string remarks { get; set; }
        public int mrsID { get; set; }
        public int mrsItemID { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }

        public string address { get; set; }
    }

    public class CMIssuedItems
    {
        public int facility_ID { get; set; }
        public int assetMasterID { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string asset_name { get; set; }
        public string serial_number { get; set; }
        public int materialID { get; set; }
    }

    public class CMIssuedAssetItems
    {
        public int facility_ID { get; set; }
        public int assetMasterID { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string asset_name { get; set; }
        public List<CMIssuedAssetItemsWithSerialNo> CMIssuedAssetItemsWithSerialNo { get; set; }
    }

    public class CMIssuedAssetItemsWithSerialNo
    {
        public string serial_number { get; set; }
        public int materialID { get; set; }
    }

    public class CMMrsApproval
    {
        public int id { get; set; }
        public int facility_ID { get; set; }
        public string comment { get; set; }
        public List<CMMrsApprovalItems> cmmrsItems { get; set; }
    }
    public class CMMrsApprovalItems
    {
        public int mrs_item_id { get; set; }
        public decimal approval_qty { get; set; }
    }

}
