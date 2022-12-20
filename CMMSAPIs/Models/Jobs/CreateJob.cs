using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CreateJob
    {
        public int facilityId { get; set; }
        public int blockId { get; set; }
        public int assignedId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int linkedPermit { get; set; }
        public DateTime breakdownTime { get; set; }

        public int belongsTo { get; set; }
        public int status { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
        public string createdBy { get; set; }

        public string updatedBy { get; set; }
        public string cancellationRemarks { get; set; }

        public int cancelStatus { get; set; }

        
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
        */
    }
}
