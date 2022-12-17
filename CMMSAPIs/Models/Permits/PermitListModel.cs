using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Permits
{
    public class PermitListModel
    {
        public int id { get; set; }
        public string permit_site_no { get; set; }
        public string permit_type { get; set; }
        public int equipment_category { get; set; }
        public string equipment { get; set; }
        public int description { get; set; }
        public string request_by_name { get; set; }
        public DateTime request_datetime { get; set;}
        public string approved_by_name { get; set; }
        public DateTime approved_datetime { get; set; }
        public string current_status { get; set; }

    }
}
