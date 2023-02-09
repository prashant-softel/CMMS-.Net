using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMUpdateJobWorkType
    {
        public int id { get; set; }
        public int categoryid { get; set; }
        public string categoryName { get; set; }
        public string workType { get; set; }
        public DateTime updatedAt { get; set; }
        public int updateBy { get; set; }
        public int status { get; set; }
    }
}
