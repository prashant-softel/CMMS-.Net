using CMMSAPIs.Models.Masters;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.SM
{
    public class CMReports
    {

    }

    public class CMPlantStockOpening
    {
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string serial_number { get; set; }
        public decimal Opening { get; set; }
        public decimal inward { get; set; }
        public decimal outward { get; set; }
        public decimal balance { get; set; }

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
        public decimal balance { get; set; }

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
        public int facility_id { get; set; }
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
        public int facility_id { get; set; }
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
    public class CMEmployeeStockList
    {
        public int emp_ID { get; set; }
        public string emp_name { get; set; }
        public List<CMEmpStockItems> CMMRSItems { get; set; }

    }

    public class CMEmpStockItems
    {
        public int asset_item_ID { get; set; }
        public string item_name { get; set; }
        public decimal quantity { get; set; }

    }
    public class CMTaskStockItems
    {

        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public int facilityID { get; set; }
        public decimal available_qty { get; set; }

    }

    public class CMPlantStockOpeningResponse
    {
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public List<CMPlantStockOpeningItemWiseResponse> stockDetails { get; set; }
    }

    public class CMPlantStockOpeningItemWiseResponse
    {
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public decimal Opening { get; set; }
        public decimal inward { get; set; }
        public decimal outward { get; set; }
        public decimal balance { get; set; }
    }

    public class CMEmployeeStockTransactionReport
    {
        public int facilityID { get; set; }
        public int fromActorID { get; set; }
        public string fromActorType { get; set; }
        public string FromActorName { get; set; }
        public int toActorID { get; set; }
        public string toActorType { get; set; }
        public string toActorName { get; set; }
        public int assetItemID { get; set; }
        public string assetItemName { get; set; }
        public decimal qty { get; set; }
        public decimal Opening { get; set; }
        public string facilityName { get; set; }
        public string remarks { get; set; }
        public string asset_type { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? createdAt { get; set; }

    }

    public class CMItemWiseTransaction
    {
        public string facilityName { get; set; }
        public int facilityID { get; set; }
        public decimal opening { get; set; }
        public List<CMItemWiseTransactionDetails> details { get; set; }

    }

    public class CMItemWiseTransactionDetails
    {
        public int fromActorID { get; set; }
        public string fromActorType { get; set; }
        public string FromActorName { get; set; }
        public int toActorID { get; set; }
        public string toActorType { get; set; }
        public string toActorName { get; set; }
        public int assetItemID { get; set; }
        public string assetItemName { get; set; }
        public string asset_type { get; set; }
        public decimal qty { get; set; }
        public string remarks { get; set; }
   
        public DateTime? LastUpdated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? createdAt { get; set; }
    }

    public class CMAssetMasterStockItems
    {
        public string asset_code { get; set; }
        public string serial_number { get; set; }
        public int asset_type_id { get; set; }
        public string asset_type { get; set; }
        public int item_category_ID { get; set; }
        public string item_category { get; set; }
        public int unit_of_measurement_ID { get; set; }
        public string unit_of_measurement { get; set; }

    }

    public class CMPlantStockOpeningResponse_MRSRetrun
    {
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public List<CMPlantStockOpeningItemWiseResponse_MRSReturn> stockDetails { get; set; }
    }

    public class CMPlantStockOpeningItemWiseResponse_MRSReturn
    {

        public int mrs_item_id { get; set; }
        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string serial_number { get; set; }
        public decimal available_qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal issued_qty { get; set; }
        public decimal approved_qty { get; set; }
    }
    public class CMPlantStockOpening_MRSReturn
    {
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public int Facility_Is_Block { get; set; }
        public string Facility_Is_Block_of_name { get; set; }
        public int mrs_item_id { get; set; }
        public int assetItemID { get; set; }
        public string asset_name { get; set; }
        public string asset_code { get; set; }
        public int asset_type_ID { get; set; }
        public string asset_type { get; set; }
        public string serial_number { get; set; }
        public decimal available_qty { get; set; }
        public decimal requested_qty { get; set; }
        public decimal used_qty { get; set; }
        public decimal issued_qty { get; set; }
        public decimal approved_qty { get; set; }


    }
}
