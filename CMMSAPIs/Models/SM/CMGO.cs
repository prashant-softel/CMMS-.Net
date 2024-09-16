using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.GO
{
    public class CMGO
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string title { get; set; }
        public int order_by_type { get; set; }
        public string remarks { get; set; }
        public string rejectedRemark { get; set; }
        public int plantID { get; set; }
        public DateTime? purchaseDate { get; set; }
        public int vendorID { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
        public string generatedBy { get; set; }
        public int receiverID { get; set; }

        public string vendor_name { get; set; }
        public int generate_flag { get; set; }
        public int location_ID { get; set; }
        public DateTime received_on { get; set; }
        public string receivedBy { get; set; }
        public string approvedBy { get; set; }
        public DateTime? approvedOn { get; set; }


        public List<go_items> go_items { get;set; }
    }
    public class go_items
    {
        public int assetItemID { get; set; }
        public double cost { get; set; }
        public double ordered_qty { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public string serial_number { get; set; }
        public string asset_code { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public double received_qty { get; set; }
        public double damaged_qty { get; set; }
        public double accepted_qty { get; set; }



    }
}
