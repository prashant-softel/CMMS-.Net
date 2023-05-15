using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Collections.Generic;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.JC
{
    public class CMJCDetail
    {
        public string plant_name { get; set; }
        public string asset_category_name { get; set; }
        public string JC_Approved_By_Name { get; set; }
        public string UpdatedByName { get; set; }
        public int id { get; set; }
        public int currentEmpID { get; set; }
        public string current_status { get; set; }
        public string JC_Closed_by_Name { get; set; }
        public string JC_Rejected_By_Name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int jobid { get; set; }
        public int ptwId { get; set; }          

        public List<CMJCJobDetail> LstCMJCJobDetailList { get; set; }
        public List<CMJCPermitDetail> LstPermitDetailList { get; set; }
        public List<CMJCIsolatedDetail> LstCMJCIsolatedDetailList { get; set; }
        public List<CMJCLotoDetail> LstCMJCLotoDetailList { get; set; }        
        public List<CMJCEmpDetail> LstCMJCEmpList { get; set; }
        public List<CMFileDetail> file_list { get; set; }
    }
}

public class CMFileDetail
{
    public int id { get; set; }
    public string fileName { get; set; }
    public string fileCategory { get; set; }
    public double fileSize { get; set; }
    public int status { get; set; }
    public string PTWFiles { get; set; }
}
public class CMJCJobDetail
{  
    public int job_id { get; set; }
    public string job_title { get; set; }
    public string job_assigned_employee_name { get; set; }
    public string job_description { get; set; }
    public string work_type { get; set; }
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
    public string empName { get; set; }
    public string resp { get; set; }
}


/*public string linked_tool_to_work_type { get; set; }
public string standard_action { get; set; }
public string history { get; set; }*/