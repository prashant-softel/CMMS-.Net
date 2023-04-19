using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models
{
    public class CMGO
    {
        public int id { get; set; }
        public string title { get; set; }
        public int podID { get; set; }
        public string order_type { get; set; }
        public string spare_status { get; set; }
        public string remarks { get; set; }
        public int orderflag { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public int purchaseID { get; set; }
        public int assetItemID { get; set; }
        public string serial_number { get; set; }
        public int location_ID { get; set; }
        public double cost { get; set; }
        public double ordered_qty { get; set; }
        public string rejectedRemark { get; set; }
        public int plantID { get; set; }
        public DateTime? purchaseDate { get; set; }
        public int vendorID { get; set; }
        public int flag { get; set; }
        public string asset_code { get; set; }
        public string asset_type { get; set; }
        public string cat_name { get; set; }
        public double received_qty { get; set; }
        public double damaged_qty { get; set; }
        public double accepted_qty { get; set; }
        public DateTime received_on { get; set; }
        public DateTime approvedOn { get; set; }
        public int generatedBy { get; set; }
        public int receivedBy { get; set; }
        public int approvedBy { get; set; }
        public string vendor_name { get; set; }
        public int generate_flag { get; set; }


    }
}
