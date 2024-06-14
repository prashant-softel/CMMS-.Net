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
        public DateTime addedAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
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
        public int show_opening { get; set; }
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
        public int show_opening { get; set; }
        public int isHazardous { get; set; }
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
        public int id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string month_name { get; set; }
        public Int64 month_id { get; set; }
        public Int64 year { get; set; }
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
        public List<CMWaterTypeMaster> master_list { get; set; }
        public List<FacilityPeriodData> period { get; set; }

    }
    public class CMWaterTypeMaster
    {
        public string water_type { get; set; }
        public int show_opening { get; set; }
    }
    public class FacilityPeriodData
    {
        public string month_name { get; set; }
        public int month_id { get; set; }
        public int year { get; set; }
        public List<CMWaterDataMonthWiseDetails> details { get; set; }

    }
    public class CMWaterDataMonthWiseDetails
    {
        public int water_id { get; set; }
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
        public dynamic date { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }
        public string water_type { get; set; }

        public int waterTypeId { get; set; }

        public int id { get; set; }
        public int consumeTypeId { get; set; }
        public dynamic months { get; set; }
        public dynamic year { get; set; }

    }

    public class WaterDataResult_Month
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public dynamic month { get; set; }
        public int year { get; set; }
        public string water_type { get; set; }
        public int waterTypeId { get; set; }

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

        public int id { get; set; }
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
        public List<CMWasteTypeMaster> master_list { get; set; }
        public List<CMFacilityPeriodData_Waste> period { get; set; }
    }
    public class CMWasteTypeMaster
    {
        public string waste_type { get; set; }
        public int show_opening { get; set; }
    }
    public class CMFacilityPeriodData_Waste
    {
        public string month_name { get; set; }
        public int month_id { get; set; }
        public int year { get; set; }
        public List<CMWasteDataMonthWiseDetails> details { get; set; }

    }
    public class CMWasteDataMonthWiseDetails
    {
        public int waste_id { get; set; }
        public string waste_type { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }
        public int show_opening { get; set; }

    }
    public class CMGetMisWasteData
    {
        public int Id { get; set; }
        public int facilityID { get; set; }
        public string facilityName { get; set; }
        public DateTime Date { get; set; }
        public int WasteTypeId { get; set; }
        public string Description { get; set; }
        public int isHazardous { get; set; }
        public decimal DebitQty { get; set; }
        public decimal CreditQty { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime? updatedAt { get; set; }
    }
    public class CMWasteDataMonthDetail
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public dynamic date { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }

        public string waste_type { get; set; }

        public int wasteTypeId { get; set; }
        public int id { get; set; }
        public int consumeTypeId { get; set; }
        public dynamic months { get; set; }
        public dynamic year { get; set; }
        public int show_opening { get; set; }

    }

    public class CMWasteDataResult_Month
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public dynamic month { get; set; }
        public int year { get; set; }
        public string waste_type { get; set; }
        public int wasteTypeId { get; set; }
        public string Description { get; set; }
        public List<CMWasteFacilityPeriodData_Month> item_data { get; set; }
    }
    public class CMWasteFacilityPeriodData_Month
    {
        public string waste_type { get; set; }
        public int wasteTypeId { get; set; }
        public decimal opening { get; set; }
        public List<CMWasteDataMonthWiseDetails_Month> details { get; set; }
    }
    public class CMWasteDataMonthWiseDetails_Month
    {
        public int id { get; set; }
        public dynamic date { get; set; }
        public string waste_type { get; set; }
        public decimal opening { get; set; }
        public decimal procured_qty { get; set; }
        public decimal consumed_qty { get; set; }
        public decimal closing_qty { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public int show_opening { get; set; }

    }

    public class CMChecklistInspectionReport
    {
        public int id { get; set; }
        public string checklist_name { get; set; }
        public string SOP_number { get; set; }
        public string frequency { get; set; }
        public string month { get; set; }
        public string inspection_status { get; set; }
        public DateTime? date_of_inspection { get; set; }
        public string checklist_attachment { get; set; }
        public int no_of_unsafe_observation { get; set; }
    }
    public class CMObservationReport
    {
        public int id { get; set; }
        public string month_of_observation { get; set; }
        public DateTime? date_of_observation { get; set; }
        public string contractor_name { get; set; }
        public string location_of_observation { get; set; }
        public string type_of_observation { get; set; }
        public string source_of_observation { get; set; }
        public string risk_type { get; set; }
        public string observation_description { get; set; }
        public string corrective_action { get; set; }
        public string responsible_person { get; set; }
        public DateTime? target_date { get; set; }
        public string action_taken { get; set; }
        public DateTime? closer_date { get; set; }
        public string cost_type { get; set; }
        public string status { get; set; }
        public int days_remaining { get; set; }
        public string timeline { get; set; }
    }

    public class CMStatutoryCompliance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class CMCreateStatutory
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public int? compliance_id { get; set; }
        public DateTime issue_date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime expires_on { get; set; }

        public int status { get; set; }
        public string status_short { get; set; }
        public int created_by { get; set; }
        public DateTime? created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime? updated_at { get; set; }
        public dynamic? renew_from { get; set; }
        public int? renew_from_id { get; set; }
        public int approved_by { get; set; }
        public DateTime? approved_at { get; set; }
        public int status_of_aplication_id { get; set; }
        public int renewflag { get; set; }
        public string Comment { get; set; }
    }
    public class CMStatutory
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public int? compliance_id { get; set; }
        public string compilanceName { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int status_id { get; set; }
        public string Current_status { get; set; }
        public string status_short { get; set; }
        public int created_by { get; set; }
        public DateTime? created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime? updated_at { get; set; }
        public dynamic renew_from { get; set; }
        public string description { get; set; }
        public string status_of_application { get; set; }
        public int? renew_by { get; set; }
        public int approved_by { get; set; }
        public DateTime? approved_at { get; set; }
        public string createdByName { get; set; }
        public string UpdatedByName { get; set; }
        public string ApprovedByName { get; set; }
        public dynamic validity_month { get; set; }
        public dynamic expiry_year { get; set; }
        public dynamic daysLeft { get; set; }
        public string Activation_status { get; set; }
    }
    public class CMStatutoryHistory
    {

        public int id { get; set; }
        public int facility_id { get; set; }
        public int? compliance_id { get; set; }
        public string compliance_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int status { get; set; }
        public string facility_name { get; set; }
        public string Current_status_short { get; set; }
        public string status_short { get; set; }
        public string created_by { get; set; }
        public DateTime? created_at { get; set; }
        public string updated_by { get; set; }
        public DateTime? updated_at { get; set; }
        public dynamic renew_from { get; set; }
        public int? renew_from_id { get; set; }
        public string approved_by { get; set; }
        public DateTime? approved_at { get; set; }
        public string createdByName { get; set; }
    }
    public class CMApprovals
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int facility_id { get; set; }
    }
    public class Details
    {
        public DateTime Date { get; set; }
        public String Status { get; set; }
        public String In_time { get; set; }
        public String Out_time { get; set; }
    }

    public class Employee
    {
        public int employeeId { get; set; }
        public String employeeName { get; set; }
        public DateTime dateOfJoining { get; set; }
        public DateTime dateOfExit { get; set; }
        public String workingStatus { get; set; }
        public List<Details> Details { get; set; }

    }

    public class AttendanceMonthModel
    {
        public int facility_id { get; set; }
        public String facility_name { get; set; }
        public List<Employee> Employee { get; set; }
    }

    public class AttendanceListModel
    {
        public int month_id { get; set; }
        public String month_name { get; set; }
        public int year { get; set; }
        public List<month_emp> Months_Data_employee { get; set; }
    }

    public class month_emp
    {
        public DateTime date { get; set; }
        public int hfe_employees { get; set; }
        public int less_than_35 { get; set; }
        public int between_30_to_50 { get; set; }
        public int greater_than_50 { get; set; }
    }
}
