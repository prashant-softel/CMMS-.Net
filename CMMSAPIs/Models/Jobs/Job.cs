using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class Job
    {
        public int id { get; set; }
        public int userId { get; set; }

        public string plantName { get; set; }
        public DateTime jobDate { get; set; }
        public string equipmentCat { get; set; }
        public string workingArea { get; set; }
        public string description { get; set; }
        public string jobDetails { get; set; }
        public string workType { get; set; }
        public string raisedBy { get; set; }

        public DateTime breaKdownTime { get; set; }
        public string breakdownType { get; set; }

        public string permitId { get; set; }
        public string assignedTo { get; set; }

        public int facility_id { get; set; }
/*        public string facility_name { get; set; }

        public int block_id { get; set; }
        public string block_name { get; set; }
        public int assigned_id { get; set; }
        public string assigned_name { get; set; }
        public int permit_id { get; set; }
        public string permit_code { get; set; }
        public DateTime breakdown_time { get; set; }
        public int job_type { get; set; }
        public string job_type_name { get; set; }
        public string cancellation_remark { get; set; }
*/    }
}
