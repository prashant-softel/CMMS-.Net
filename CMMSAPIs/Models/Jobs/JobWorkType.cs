using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class JobWorkType
    {
        public int id { get; set; }
        public int categoryid { get; set; }
        public string categoryName { get; set; }
        public string workType { get; set; }
    }
}
