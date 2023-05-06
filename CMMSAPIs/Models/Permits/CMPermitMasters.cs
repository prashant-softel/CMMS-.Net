using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;

namespace CMMSAPIs.Models.Permits
{
    public class CMCreatePermitType
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int facilityId { get; set; }
        public int? status { get; set; }
    }
    public class CMCreateSafetyMeasures
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int permitType { get; set; }
        public CMMS.CMMS_Input input { get; set; }
        public int? required { get; set; }
    }
    public class CMCreateJobType
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? status { get; set; }
        public int facilityId { get; set; }
        public int requires_SOP_JSA { get; set; }
    }
    public class CMCreateSOP
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int jobType { get; set; }
        public int fileId { get; set; }
        public int? status { get; set; }
        public string tbt_remarks { get; set; }
        public int jsa_fileId { get; set; }
    }
}
