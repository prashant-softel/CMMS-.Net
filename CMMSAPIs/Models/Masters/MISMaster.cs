using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class MISSourceOfObservation
    {
        public int id { get; set; } 
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISTypeObservation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISRiskType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISCostType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISGrievanceType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class MISResolutionLevel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }
    public class BODYPARTS
    {
        public int sequence_no { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int Status { get; set; }
    }
    public class Responsibility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class CMIncidentType
    {
        public int id { get; set; }
        public string incidenttype { get; set; }
        public int status { get; set; }
       // public DateTime CreatedAt { get; set; }
       // public string CreatedBy { get; set; }
       // public DateTime UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        //changes
        public int addedBy { get; set; }
        public DateTime  addedAt { get; set; }
        public  int updatedBy { get; set; }
        public  DateTime updatedAt { get; set; }
    }

    public class CMMisWaterData
    {
        public int Id { get; set; } 
        public int facilityID { get; set; } 
        public DateTime Date { get; set; } 
        public int WaterTypeId { get; set; }
        public int consumeType { get; set; }
        public string Description { get; set; }
        public decimal DebitQty { get; set; } 
        public decimal CreditQty { get; set; } 
        public int AddedBy { get; set; } 
        public DateTime AddedAt { get; set; }
        public int UpdatedBy { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }

    public class CMGetMisWaterData
    {
        public int Id { get; set; }
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public DateTime Date { get; set; }
        public int WaterTypeId { get; set; }
        public string Description { get; set; }
        public decimal DebitQty { get; set; }
        public decimal CreditQty { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CMWaterDataReport
    {
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public int WaterTypeId { get; set; }
        public string description { get; set; }
        public decimal opening { get; set; }
        public decimal inward { get; set; }
        public decimal outward { get; set; }
        public decimal balance { get; set; }
    }
    public class WaterDataType
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public int createdBy { get; set; }
        public DateTime? createdAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class WasteDataType
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string name { get; set; }
        public int Type { get; set; }
        public string description { get; set; }
       // public int status { get; set; }
        public int createdBy { get; set; }
        public DateTime? createdAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
        public class CMWasteData
    {
        public int Id { get; set; }
        public decimal? Solid_Waste { get; set; }
        public decimal? E_Waste { get; set; }
        public int? Battery_Waste { get; set; }
        public int? Solar_Module_Waste { get; set; }
        public decimal? Haz_Waste_Oil { get; set; }
        public decimal? Haz_Waste_Grease { get; set; }
        public decimal? Haz_Solid_Waste { get; set; }
        public int? Haz_Waste_Oil_Barrel_Generated { get; set; }
        public decimal? Solid_Waste_Disposed { get; set; }
        public decimal? E_Waste_Disposed { get; set; }
        public int? Battery_Waste_Disposed { get; set; }
        public int? Solar_Module_Waste_Disposed { get; set; }
        public decimal? Haz_Waste_Oil_Disposed { get; set; }
        public decimal? Haz_Waste_Grease_Disposed { get; set; }
        public decimal? Haz_Solid_Waste_Disposed { get; set; }
        public int? Haz_Waste_Oil_Barrel_Disposed { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Modified_By { get; set; }
        public string Modified_At { get; set; }
                    public int facilityID { get; set; }
        public DateTime Date { get; set; }
        public int wasteTypeId { get; set; }
        public int consumeType { get; set; }
        public string Description { get; set; }
        public decimal DebitQty { get; set; }
        public decimal CreditQty { get; set; }
    }
         public class CMWaterDataMonthWise
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string month_name { get; set; }
        public int year { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }
        public string water_type { get; set; }
        public int show_opening { get; set; }
    }
    public class WaterDataResult
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public List<FacilityPeriodData> period { get; set; }
    }

    public class FacilityPeriodData
    {
        public string month_name { get; set; }
        public int year { get; set; }
        public List<CMWaterDataMonthWiseDetails> details { get; set; }
    }
    public class CMWaterDataMonthWiseDetails
    {
        public string water_type { get; set; }
        public decimal opening_qty { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }
        public int show_opening { get; set; }

    }

    public class CMWaterDataMonthDetail
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string date { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }
        public string water_type { get; set; }
        public int waterTypeId { get; set; }
        public int consumeTypeId { get; set; }
        public string month { get; set; }
        public int year { get; set; }

    }

    public class WaterDataResult_Month
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public List<FacilityPeriodData_Month> item_data { get; set; }
    }
    public class FacilityPeriodData_Month
    {
        public string water_type { get; set; }
        public int waterTypeId { get; set; }
        public decimal opening { get; set; }
        public List<CMWaterDataMonthWiseDetails_Month> details { get; set; }
    }
    public class CMWaterDataMonthWiseDetails_Month
    {

        public string date { get; set; }
        public string Description { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public string TransactionType { get; set; }

    }

    public class CMWasteDataResult
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int hazardous { get; set; }
        public List<CMFacilityPeriodData_Waste> period { get; set; }
    }

    public class CMFacilityPeriodData_Waste
    {
        public string month_name { get; set; }
        public int year { get; set; }
        public List<CMWasteDataMonthWiseDetails> details { get; set; }
    }
    public class CMWasteDataMonthWiseDetails
    {
        public string waste_type { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }

    }
}
