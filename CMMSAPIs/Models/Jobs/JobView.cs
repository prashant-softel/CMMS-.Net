using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class JobView
    {
        public string facility_name { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
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
        public DateTime breaKdownTime { get; set; }
/*
        public int permit_id { get; set; }
        public string permit_code { get; set; }
        public DateTime breakdown_time { get; set; }
        public int job_type { get; set; }
        public string job_type_name { get; set; }
        public string cancellation_remark { get; set; }
*/
    }
}
