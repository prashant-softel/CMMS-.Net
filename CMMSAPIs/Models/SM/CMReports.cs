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

}
