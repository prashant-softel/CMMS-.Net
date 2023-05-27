using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models
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
        public DateTime? received_on { get; set; }
        public string receivedBy { get; set; }
        public string approvedBy { get; set; }
        public DateTime? approvedOn { get; set; }

        public int receive_later { get; set; }
        public int added_to_store { get; set; }
        public int assetItemID { get; set; }
        public decimal accepted_qty { get; set; }
        public int spare_status { get; set; }

        public List<go_items> go_items { get;set; }
    }
    public class go_items
    {
        public int poID { get; set; }
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

    public class PurchaseData
    {
        public string facilityName { get; set; }
        public int orderID { get; set; }
        public DateTime? purchaseDate { get; set; }
        public int generate_flag { get; set; }
        public DateTime? received_on { get; set; }
        public int status { get; set; }
        public string vendor_name { get; set; }
        public int? vendorID { get; set; }
        public int generatedByID { get; set; }
        public string remarks { get; set; }
        public string generatedBy { get; set; }
        public string receivedOn { get; set; }
        public string receivedDate { get; set; }
        public string approvedBy { get; set; }
        public DateTime? approvedOn { get; set; }
        public int statusFlag { get; set; }

    }

    public class SubmitPurchaseData
    {
        public int purchaseID { get; set; }
        public int facilityId { get; set; }
        public int vendor { get; set; }
        public int empId { get; set; }
        public DateTime purchaseDate { get; set; }
        public int generateFlag { get; set; }
        public List<submitItems> submitItems { get; set; }
    }
    public class submitItems
    {
        public string assetCode { get; set; }
        public int assetItemID { get; set; }
        public decimal orderedQty { get; set; }
        public int type { get; set; }
        public decimal cost { get; set; }
    }
}
