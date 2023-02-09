using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMADDJobWorkType
    {
        public int id { get; set; }
        public int categoryid { get; set; }
        public string categoryName { get; set; }
        public string workType { get; set; }
        public DateTime createdAt { get; set; }
        public int createdBy { get; set; }
        public int status { get; set; }
    }
}
