using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Permits
{
    public class Permit
    {
        public int permitId { get; set; }
        public string sitePermitNo { get; set; }
        public int permitType { get; set; }
        public string PermitTypeName { get; set; }
        public string equipmentCat { get; set; }
        public int workingAreaId { get; set; }
        public string workingAreaName { get; set; }
        public string description { get; set; }
        public int ptwRequestedBy { get; set; }
        public dynamic ptwRequestDate { get; set; }
        public int approvedBy { get; set; }
        public dynamic approvedDate { get; set; }
        public int currentStatus { get; set; }

/*
        public string title { get; set; }

        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int block_id { get; set; }

        public string block_name { get; set; }
        public int assigned_id { get; set; }
        public string assigned_name { get; set; }
      
        public string cancellation_remark { get; set; }*/

    }
}
