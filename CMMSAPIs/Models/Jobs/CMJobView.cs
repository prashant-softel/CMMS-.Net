using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMJobView
    {
        public string facility_name { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int JobStatus { get; set; }
        public int equipmentCat_id { get; set; }
        public string equipmentCat_name { get; set; }
        public int workingArea_id { get; set; }
        public string workingArea_name { get; set; }
        public int status { get; set; }
        public int assigned_id { get; set; }
        public string assigned_name { get; set; }
        public string workType { get; set; }
        public string job_title { get; set; }
        public string job_description { get; set; }
        public dynamic breaKdownTime { get; set; }

        public List<CMequipmentCatList> LstCMequipmentCatList { get; set; }
        public List<CMworkingAreaNameList> LstCMworkingAreaNameList { get; set; }

        public List<CMAssociatedPermitList> LstAssociatedPermit { get; set; }
    }
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
    public int JobId { get; set; }
    public int sitePermitNo { get; set; }
    public string PermitTypeName { get; set; }
    public string issuedByName { get; set; }
    public DateTime issue_at { get; set; }
    public int ptwStatus { get; set; }
}
