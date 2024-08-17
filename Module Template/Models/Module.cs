using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models
{
    public class Module
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int assigned_id { get; set; }
        public string assigned_name { get; set; }
        public int permit_id { get; set; }
        public string permit_code { get; set; }
        public DateTime breakdown_time  { get; set; }
        public int Module_type { get; set; }
        public string Module_type_name { get; set; }
        public string cancellation_remark { get; set; }
    }
}
