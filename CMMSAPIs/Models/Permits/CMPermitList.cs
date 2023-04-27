using System;
using System.Collections.Generic;
using CMMSAPIs.Models.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Permits
{
    public class CMPermitList
    {
        public int permitId { get; set; }
        public int ptwStatus { get; set; }
        public int permit_site_no { get; set; }
        public int permit_type { get; set; }
        public string PermitTypeName { get; set; }
        public string equipment_categories { get; set; }
        public int workingAreaId { get; set; }
        public string workingAreaName { get; set; }
        public string description { get; set; }
        public string request_by_name { get; set; }
        public dynamic request_datetime { get; set; }
        public string issued_by_name { get; set; }
        public dynamic issue_datetime { get; set; }
        public string approved_by_name { get; set; }
        public dynamic approved_datetime { get; set; }
        public string current_status { get; set; }
    }
    public class CMSafetyMeasurementQuestionList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int inputID { get; set; }
        public string inputName { get; set; }
        public string permitType { get; set; }
    }
    public class CMSOPList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string jobTypeName { get; set; }
    }
}
