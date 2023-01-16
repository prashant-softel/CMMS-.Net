using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.SM
{
    public class CMSMMaster
    {
        public int id { get; set; }
        public string title { get; set; }
    }

    public class CMSMUnitMaster : CMSMMaster
    {
        public bool is_multiple { get; set; }
    }

    public class CMSMAssetMaster
    {
        public string code { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string type_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int unit_id { get; set; }
        public string unit_name { get; set; }
        public bool is_approval_required { get; set; }
        public string description { get; set; }      
    }

    public class CMStock
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public int available_qty { get; set; }
        public int min_order_qty { get; set; }
        public int max_order_qty { get; set; }
    }
}
