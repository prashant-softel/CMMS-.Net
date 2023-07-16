using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string referenceID { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int setAsTemplate { get; set; }
        public string templateName { get; set; }
        public int approval_manager_ID { get; set; }
        public DateTime? approval_manager_date { get; set; }
        public int setTemplateflag { get; set; }

        public string return_remarks { get; set; }
        public int mrs_return_ID { get; set; }
        public string finalRemark { get; set; }
        public int asset_item_ID { get; set; }
        public string asset_MDM_code { get; set; }
        public long serial_number { get; set; }
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
        public int whereUsedTypeId { get; set; }

        public List<CMEquipments> equipments { get; set;}

    }
    public class CMEquipments
    {
        public int id { get; set; }
        public int equipmentID { get; set; }
        public decimal qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal issued_qty { get; set; }
        public string return_remarks { get; set; }
        public decimal returned_qty { get; set; }
        public decimal received_qty { get; set; }
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
        public string return_remarks { get; set; }
        public int mrs_return_ID { get; set; }
        public string finalRemark { get; set; }
        public int asset_item_ID { get; set; }
        public string asset_MDM_code { get; set; }
        public string serial_number { get; set; }
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
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string file_path { get; set; }
        public int Asset_master_id { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }

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
        public int requested_by_emp_ID { get; set; }
        public string approver_name { get; set; }
        public string? requestd_date { get; set; }
        public string returnDate { get; set; }
        public string approval_date { get; set; }
        public int approval_status { get; set; }
        public string approval_comment { get; set; }
        public string requested_by_name { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string activity { get; set; }
        public int whereUsedType { get; set; }
        public int whereUsedTypeId { get; set; }
        public List<CMMRSItems> CMMRSItems { get; set; }
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
        public decimal   available_qty { get; set; }
        public string finalRemark { get; set; }

    }

    public class CMASSETMASTERLIST
    {
        public int ID { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_code { get; set; }
        public string asset_name { get; set; }
        public string description { get; set; }
        public string approval_required { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
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
}
