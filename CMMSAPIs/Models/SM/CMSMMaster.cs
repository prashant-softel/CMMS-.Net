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
        public int asset_type_ID { get; set; }
        public string asset_code { get; set; }
        public string asset_name { get; set; }
        public string description { get; set; }
        public string approval_required { get; set; }
        public string cat_name { get; set; }
        public string measurement { get; set; }
        public double decimal_status { get; set; }
        public string asset_description { get; set; }
        public int item_category_ID { get; set; }
        public int unit_measurement_ID { get; set; }
        public int approval_required_ID { get; set; }
        
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

    public class AssetMasterFiles
    {
        public int id { get; set; }
        public int Asset_master_id { get; set; }
        public string File_path { get; set; }
        public string File_name { get; set; }
        public string File_type { get; set; }
        public string File_size { get; set; }
        public DateTime? Uploaded_date { get; set; }
    }
}
