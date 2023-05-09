using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.Jobs
{
    public class CMCreateJob
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int assigned_id { get; set; }
        public int? permit_id { get; set; }
        public CMMS.CMMS_JobType? jobType { get; set; }
        public DateTime breakdown_time { get; set; }
//        public DateTime job_create_Date { get; set; }
//        public DateTime createdAt { get; set; }
//        public string cancellation_remark { get; set; }
        public List<int> WorkType_Ids { get; set; }
        public List<int> AssetsIds { get; set; }
    }
}

