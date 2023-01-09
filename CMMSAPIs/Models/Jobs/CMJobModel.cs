using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMJobModel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string plantName { get; set; }
        public dynamic jobDate { get; set; }
        public string equipmentCat { get; set; }
        public string workingArea { get; set; }
        public string description { get; set; }
        public string jobDetails { get; set; }
        public string workType { get; set; }
        public string raisedBy { get; set; }
        public dynamic breaKdownTime { get; set; }
        public string breakdownType { get; set; }
        public string permitId { get; set; }
        public string assignedTo { get; set; }
        public int facility_id { get; set; }
    }

}
