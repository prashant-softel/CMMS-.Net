using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMJobView
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int latestJCid { get; set; }
        public int latestJCStatus { get; set; }
        public int latestJCApproval { get; set; }
        public string latestJCStatusShort { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public int created_by_id { get; set; }
        public string created_by_name { get; set; }
        public DateTime? created_at { get; set; }
        public int assigned_id { get; set; }
        public string assigned_name { get; set; }
        public int job_type { get; set; }
        public string work_type { get; set; }
        public string standard_action{ get; set; }
        public string breakdown_type { get; set; }
        public string job_title { get; set; }
        public string job_description { get; set; }
        public DateTime? breakdown_time { get; set; }
        public int current_ptw_id { get; set; }
        public string current_ptw_title { get; set; }

        public dynamic closed_at { get; set; }


        
        public List<CMequipmentCatList> equipment_cat_list { get; set; }
        public List<CMworkingAreaNameList> working_area_name_list { get; set; }
        public List<CMAssociatedPermitList> associated_permit_list { get; set; }
        public List<CMWorkType> work_type_list { get; set; }
        public List<CMWorkTypeTool> tools_required_list { get; set; }

    }

    public class CMequipmentCatList
    {
        public int equipmentCat_id { get; set; }
        public string equipmentCat_name { get; set; }
    }

    public class CMworkingAreaNameList
    {
        public int workingArea_id { get; set; }
        public string workingArea_name { get; set; }
    }
    public class CMAssociatedPermitList
    {
        public int permitId { get; set; }
        public string sitePermitNo { get; set; }
        public string PermitTypeName { get; set; }
        public string title { get; set; }
        public string issuedByName { get; set; }
        public DateTime issue_at { get; set; }
        public int ptwStatus { get; set; }
        public string ptwStatus_short { get; set; }
        public dynamic startDate { get; set; }
        public dynamic endDate { get; set; }
    }

    public class CMWorkType
    {
        public int workTypeId { get; set; }
        public string workTypeName { get; set; }
    }
    public class CMWorkTypeTool
    {
        public int toolId { get; set; }
        public string toolName { get; set; }
    }
}
