using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.WC;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.JC
{
    public class CMJCDetail
    {

        public int id { get; set; }
        public int facility_id { get; set; }
        public string plant_name { get; set; }
        public string block_name { get; set; }
        //  public string asset_category_name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int currentEmpID { get; set; }
        public int jobid { get; set; }
        public int ptwId { get; set; }
        public dynamic created_at { get; set; }
        public string created_by { get; set; }
        public int created_by_id { get; set; }
        public int JC_Start_By_id { get; set; }
        public dynamic JC_Start_At { get; set; }
        public string JC_Start_By_Name { get; set; }
        public int JC_Close_By_id { get; set; }
        public dynamic JC_Closed_At { get; set; }
        public string JC_Closed_By_Name { get; set; }
        public string JC_Approved_By_Name { get; set; }
        public string JC_Approve_Reason { get; set; }
        public string JC_Rejected_By_Name { get; set; }
        public string JC_Rejected_Reason { get; set; }
        public int JC_Update_by { get; set; }

        public string JC_UpdatedByName { get; set; }
        public int JC_Approved { get; set; }
        public int status { get; set; }
        public string Remark_new { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public List<CMJCJobDetail> LstCMJCJobDetailList { get; set; }
        public List<CMJCPermitDetail> LstPermitDetailList { get; set; }
        public List<CMJCAssetName> asset_category_name { get; set; }
        public List<CMJCIsolatedDetail> LstCMJCIsolatedDetailList { get; set; }
        public List<CMJCLotoDetail> LstCMJCLotoDetailList { get; set; }
        public List<CMJCEmpDetail> LstCMJCEmpList { get; set; }
        public List<CMFileDetail> file_list { get; set; }
        public List<CMFileDetailJc> file_listJc { get; set; }
        public List<CMWorkTypeTools> tool_List { get; set; }
        public List<int> uploadfile_ids { get; set; }
        public List<Materialconsumption> Material_consumption { get; set; }
    }

    public class CMJCRequest
    {
        public int jc_id { get; set; }
        public CMJCDetail request { get; set; }
    }
}



public class CMFileDetail
{
    public int id { get; set; }
    public string fileName { get; set; }
    public string fileCategory { get; set; }
    public double fileSize { get; set; }
    public DateTime created_at { get; set; }
    public string created_by { get; set; }
    public int status { get; set; }
    public string PTWFiles { get; set; }
    public string description { get; set; }
}

public class CMFileDetailJc
{
    public int id { get; set; }
    public string fileName { get; set; }
    public string fileCategory { get; set; }
    public double fileSize { get; set; }
    public int status { get; set; }
    public string PTWFiles { get; set; }
    public DateTime created_at { get; set; }
    public string created_by { get; set; }

    public string description { get; set; }
}

public class CMJCJobDetail
{
    public int job_id { get; set; }
    public string job_title { get; set; }
    public string job_assigned_employee_name { get; set; }
    public string job_description { get; set; }
    public string work_type { get; set; }
    public int facility_id { get; set; }
    public int status { get; set; }
    public string status_short { get; set; }
    public string perform_by { get; set; }
    public string Employee_name { get; set; }
    public string Company { get; set; }
    public int Employee_ID { get; set; }
    public DateTime Breakdown_end_time { get; set; }
    public DateTime Breakdown_start_time { get; set; }
    public DateTime Job_closed_on { get; set; }
    public DateTime Job_raised_on { get; set; }
    public string Type_of_Job { get; set; }
    public dynamic turnaround_time_minutes { get; set; }
    public string JobAssignedEmployeeName { get; set; }
    public string JobDescription { get; set; }


}
public class CMJCPermitDetail
{
    public int permit_id { get; set; }
    public int site_permit_no { get; set; }
    public string permit_type { get; set; }
    public string permit_description { get; set; }
    public string job_created_by_name { get; set; }
    public string permit_issued_by_name { get; set; }
    public string permit_approved_by_name { get; set; }
    public int status { get; set; }
    public string status_short { get; set; }
    public string Isolation_taken { get; set; }
    public string Isolated_equipment { get; set; }
    public string TBT_conducted_by_name { get; set; }
    public DateTime TBT_done_time { get; set; }
    public DateTime Start_time { get; set; }
    public int Status_PTW { get; set; }
    public dynamic tbt_start { get; set; }
    public int TBT_Done_Check { get; set; }
    public DateTime? endDate { get; set; }
}
public class CMJCIsolatedDetail
{
    public string isolated_assestName { get; set; }
}

public class CMJCLotoDetail
{
    public string isolated_assest_loto { get; set; }
}

public class CMJCEmpDetail
{
    public int id { get; set; }

    public string name { get; set; }
    public string responsibility { get; set; }
    public string designation { get; set; }
}
public class Files
{
    public List<int> uploadfile_ids { get; set; }
}
public class filesforwc
{
    public int id { get; set; }
    public int facilityId { get; set; }
    public string comment { get; set; }
    public List<int> uploadfile_ids { get; set; }
    public List<CMWCSupplierActions> supplierActions { get; set; }

}
public class CMWorkTypeTools
{
    public int toolId { get; set; }
    public string toolName { get; set; }
    public dynamic No_of_tools { get; set; }
}
public class CMJCAssetName
{
    public string Equipment_name { get; set; }
    public string Equipment_category { get; set; }
}


/*public string linked_tool_to_work_type { get; set; }
public string standard_action { get; set; }
public string history { get; set; }*/
