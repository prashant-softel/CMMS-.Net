using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.SM
{
    public class ReOrder
    {
        public int ID { get; set; }
        public int asset_code_ID { get; set; }
        public string asset_code { get; set; }
        public int plant_ID { get; set; }
        public decimal max_qty { get; set; }
        public decimal min_qty { get; set; }
        public decimal ordered_qty { get; set; }
        public DateTime? lastModifiedTIme { get; set; }
        public DateTime? purchase_date { get; set; }
        public string asset_name { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public int emp_ID { get; set; }
        public List<ReOrderAsset> ReOrderAsset { get; set; }
    }

    public class ReOrderAsset
    {
        public int reorderID { get; set; }
        public string asset_code { get; set; }
        public decimal max_qty { get; set; }
        public decimal min_qty { get; set; }
        public decimal ordered_qty { get; set; }
    }

    public class ReOrderItems
    {
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public int? plant_ID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public decimal? availableQty { get; set; }
        public decimal? max_qty { get; set; }
        public decimal? min_qty { get; set; }
        public int? reorderID { get; set; }
        public decimal? ordered_qty { get; set; }

    }
}
