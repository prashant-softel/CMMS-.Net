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
        public string approval_required { get; set; }
        public string cat_name { get; set; }
        public string measurement { get; set; }
        public double decimal_status { get; set; }
        public string asset_description { get; set; }
        public int item_category_ID { get; set; }
        public int unit_measurement_ID { get; set; }
        public int approval_required_ID { get; set; }
        public AssetMasterFiles fileData { get; set; }


    }

    public class UnitMeasurement
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int flag { get; set; }
        public int decimal_status { get; set; }
        public int spare_multi_selection { get; set; }
    }

    public class ItemCategory
    {
        public int ID { get; set; }
        public string cat_name { get; set; }
        public int flag { get; set; }
    }

    public class AssetMasterFiles
    {
        public int id { get; set; }
        public int Asset_master_id { get; set; }
        public string File_path { get; set; }
        public string File_name { get; set; }
        public string File_type { get; set; }
        public string File_size { get; set; }
        public DateTime? Uploaded_date { get; set; }
    }

    public class MRS
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

        public int plant_ID { get; set; }
        public int approved_by_emp_ID { get; set; }
        public string approved_date { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string referenceID { get; set; }
        public string lastmodifieddate { get; set; }
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
        public string issued_date { get; set; }
        public decimal requested_qty { get; set; }
        public string approval_required { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string file_path { get; set; }
        public int Asset_master_id { get; set; }
        public int isEditMode { get; set; }
        public int item_condition { get; set; }

        public List<equipments> equipments { get; set;}

    }
    public class equipments
    {
        public int id { get; set; }
        public int equipmentID { get; set; }
        public decimal qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal issued_qty { get; set; }
        public string return_remarks { get; set; }
    }
}
