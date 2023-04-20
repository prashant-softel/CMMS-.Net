using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.SM
{
    public class CMSMMaster
    {
        public int ID { get; set; }
        public string asset_type { get; set; }
        public int flag { get; set; }
    }

    public class UnitMeasurement
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int flag { get; set; }
        public int decimal_status { get; set; }
        public int spare_multi_selection { get; set; }
    }

    public class ItemCategory
    {
        public int ID { get; set; }
        public string cat_name { get; set; }
        public int flag { get; set; }
    }
}
