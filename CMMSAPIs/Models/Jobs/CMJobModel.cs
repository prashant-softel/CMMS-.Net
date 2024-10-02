using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMJobModel
    {
        public int id { get; set; }
        public int facilityId { get; set; }
        public string facilityName { get; set; }
        public dynamic jobDate { get; set; }
        public int categoryId { get; set; }
        public string equipmentCat { get; set; }
        public string workingArea { get; set; }
        public string description { get; set; }
        public string jobDetails { get; set; }
        public string workType { get; set; }
        public dynamic breakdownTime { get; set; }
        public string breakdownType { get; set; }
        public int ptw_id { get; set; }
        public string permitType { get; set; }
        public string permitId { get; set; }
        public int Isolation { get; set; }
        public int raisedBy { get; set; }
        public string raisedByName { get; set; }
        public string assignedToName { get; set; }
        public int assignedToId { get; set; }
        public int status { get; set; }
        public int latestJCid { get; set; }
        public int latestJCStatus { get; set; }
        public int latestJCPTWStatus { get; set; }
        public int latestJCApproval { get; set; }
        public string latestJCStatusShort { get; set; }
    }

    public class CMJobList
    {
        public int jobId { get; set; }
        public int permitId { get; set; }
        public string title { get; set; }
        public string equipmentCat { get; set; }
        public string equipment { get; set; }
        public dynamic breakdownTime { get; set; }
        public string assignedTo { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }

    }
}
