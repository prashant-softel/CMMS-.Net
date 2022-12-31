using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class ReAssignJob
    {
        public int job_id { get; set; }
        public int ptw_id { get; set; }
        public string user_id { get; set; }
        public int changed_by { get; set; }
        public string Cancelremark { get; set; }
    }
}

