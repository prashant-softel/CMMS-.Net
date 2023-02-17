using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMJobModel
    {
        public int id { get; set; }
        public int createdBy { get; set; }
        public string plantName { get; set; }
        public dynamic jobDate { get; set; }
        public string equipmentCat { get; set; }
        public string workingArea { get; set; }
        public string description { get; set; }
        public string jobDetails { get; set; }
        public string workType { get; set; }
        public dynamic breakdownTime { get; set; }
        public string breakdownType { get; set; }
        public string permitId { get; set; }
        public int raisedBy { get; set; }
        public string raisedByName { get; set; }
        public string assignedToName { get; set; }
        public int assignedToId { get; set; }
        public int status { get; set; }
        public int facilityId { get; set; }
    }

}
