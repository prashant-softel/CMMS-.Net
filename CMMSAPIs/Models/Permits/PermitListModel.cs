using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Permits
{
    public class PermitListModel
    {
        public int permitId { get; set; }
        public int permit_site_no { get; set; }
        public int permit_type { get; set; }
        public string PermitTypeName { get; set; }

        public int equipment_category { get; set; }
        public string equipment { get; set; }
        public int workingAreaId { get; set; }
        public string workingAreaName { get; set; }
        public string description { get; set; }
        public string request_by_name { get; set; }
        public dynamic request_datetime { get; set;}
        public string approved_by_name { get; set; }
        public dynamic approved_datetime { get; set; }
        public string current_status { get; set; }

    }
}
