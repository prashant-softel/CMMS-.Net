using System;

namespace CMMSAPIs.Models.Permits
{
    public class CMPermitList
    {
        public int isExpired { get; set; }
        public int permitId { get; set; }
        public int ptwStatus { get; set; }
        public int permit_site_no { get; set; }
        public int permit_type { get; set; }
        public string PermitTypeName { get; set; }
        public int exentedbyid { get; set; }
        public string equipment_categories { get; set; }
        public int workingAreaId { get; set; }
        public string workingAreaName { get; set; }
        public string description { get; set; }
        public int jc_id { get; set; }
        public DateTime endDatetime { get; set; }
        public int jc_status { get; set; }
        public int request_by_id { get; set; }
        public string request_by_name { get; set; }
        public dynamic request_datetime { get; set; }
        public int issued_by_id { get; set; }
        public string issued_by_name { get; set; }
        public dynamic issued_datetime { get; set; }
        public int approved_by_id { get; set; }
        public string approved_by_name { get; set; }
        public DateTime? startDate { get; set; }
        public dynamic endDate { get; set; }
        public dynamic approved_datetime { get; set; }
        public string current_status_short { get; set; }
        public string current_status_long { get; set; }
        public int TBT_Done_By_id { get; set; }
        public int extend_request_status_id { get; set; }
        public int TBT_Done_Check { get; set; }
        public dynamic tbt_start { get; set; }
    }
    public class CMSafetyMeasurementQuestionList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int inputID { get; set; }
        public string inputName { get; set; }
        public string permitType { get; set; }
        public string discription { get; set; }
        public int isRequired { get; set; }
    }

    public class CMSOPList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string jobTypeName { get; set; }
        public int sop_file_id { get; set; }
        public string sop_file_name { get; set; }
        public string sop_file_path { get; set; }
        public int sop_file_cat_id { get; set; }
        public string sop_file_cat_name { get; set; }
        public int jsa_file_id { get; set; }
        public string jsa_file_name { get; set; }
        public string jsa_file_path { get; set; }
        public int jsa_file_cat_id { get; set; }
        public string jsa_file_cat_name { get; set; }
        public string tbt_remarks { get; set; }
    }
}
