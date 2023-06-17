using System;

namespace CMMSAPIs.Models.SM
{
    public class CMReports
    {

    }

    public class CMPlantStockOpening
    {
        public string plant_name { get; set; }
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public int asset_type_ID { get; set; }
        public decimal Opening { get; set; }
        public decimal inward { get; set; }
        public decimal outward { get; set; }

    }

    public class CMEmployeeStockReport
    {
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public string requested_by_name { get; set; }
        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public int asset_type_ID { get; set; }
        public decimal Opening { get; set; }
        public decimal inward { get; set; }
        public decimal outward { get; set; }

    }

    public class CMFaultyMaterialReport
    {
        public int ID { get; set; }
        public int fromActorID { get; set; }
        public string fromActorType { get; set; }
        public int toActorID { get; set; }
        public string toActorType { get; set; }
        public int assetItemID { get; set; }
        public decimal qty { get; set; }
        public int plantID { get; set; }
        public string referedby { get; set; }
        public int reference_ID { get; set; }
        public string remarks { get; set; }
        public int Nature_Of_Transaction { get; set; }
        public int Asset_Item_Status { get; set; }
        public int flag { get; set; }
        public DateTime lastInsetedDateTime { get; set; }
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public string emp_name { get; set; }
        public string asset_code { get; set; }
        public int item_condition { get; set; }
        public string serial_number { get; set; }
        public string replaceSerialNo { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public int item_category_ID { get; set; }
        public int actorID { get; set; }

    }

    public class CMEmployeeTransactionReport
    {
        public int ID { get; set; }
        public int fromActorID { get; set; }
        public string fromActorType { get; set; }
        public int toActorID { get; set; }
        public string toActorType { get; set; }
        public int assetItemID { get; set; }
        public decimal qty { get; set; }
        public int plantID { get; set; }
        public string referedby { get; set; }
        public int reference_ID { get; set; }
        public string remarks { get; set; }
        public int Nature_Of_Transaction { get; set; }
        public int Asset_Item_Status { get; set; }
        public int flag { get; set; }
        public DateTime lastInsetedDateTime { get; set; }
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public string requested_by_name { get; set; }
        public string asset_code { get; set; }
        public int item_condition { get; set; }
        public string serial_number { get; set; }
        public string asset_name { get; set; }
        public int asset_type_ID { get; set; }
        public decimal available_qty { get; set; }
        public string return_remarks { get; set; }
        public int actorID { get; set; }
        public int mrs_return_ID { get; set; }
        public decimal InwardQty { get; set; }
        public decimal OutwardQty { get; set; }
        public string remarks_in_short { get; set; }

    }
}
