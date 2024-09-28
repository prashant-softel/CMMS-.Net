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
        public int risk_type_id { get; set; }
        public string risktype { get; set; }
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
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public List<Checklist1> checklist { get; set; }
    }
    public class ChecklistDetails
    {
        public string checklist_name { get; set; }
        public string SOP_number { get; set; }
        public string frequency { get; set; }
        public string inspection_status { get; set; }
        public DateTime? date_of_inspection { get; set; }
        public string checklist_attachment { get; set; }
        public int no_of_unsafe_observation { get; set; }
    }

    public class Checklist1 : ChecklistDetails
    {
        public dynamic month { get; set; }
        public dynamic month_id { get; set; }
        public dynamic year_id { get; set; }
        public List<ChecklistDetails> Details { get; set; }

    }
    public class CMObservationReport
    {
        public int id { get; set; }
        public string month_of_observation { get; set; }
        public DateTime? date_of_observation { get; set; }
        public string contractor_name { get; set; }
        public string location_of_observation { get; set; }
        public int type_of_observation { get; set; }
        public int source_of_observation { get; set; }
        public string month_name { get; set; }
        public string risk_type { get; set; }
        public string observation_description { get; set; }
        public string corrective_action { get; set; }
        public int responsible_person { get; set; }


        public int assign_to { get; set; }
        public string assigned_to_name { get; set; }
        public DateTime? target_date { get; set; }
        public string action_taken { get; set; }
        public DateTime? closer_date { get; set; }
        public int cost_type { get; set; }
        public string status { get; set; }
        public int days_remaining { get; set; }
        public string timeline { get; set; }
        public int risk_type_id { get; set; }

        public int status_code { get; set; }
        public int short_Status { get; set; }

    }

    public class CMObservationSummary
    {
        public CMObservationSummary(int month, int year, string month_name)
        {
            this.month = month;
            this.year = year;
            this.month_name = month_name;
        }
        public string month_name { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        //public int id { get; set; }
        public int created { get; set; }
        public int open { get; set; }
        public int closed { get; set; }
        public int unsafe_act { get; set; }
        public int unsafe_condition { get; set; }
        public int statutory_non_compliance { get; set; }
        public int createdCount_Critical { get; set; }
        public int openCount_Critical { get; set; }
        public int closeCount_Critical { get; set; }
        public int createdCount_Significant { get; set; }
        public int openCount_Significant { get; set; }
        public int closeCount_Significant { get; set; }
        public int createdCount_Moderate { get; set; }
        public int openCount_Moderate { get; set; }
        public int closeCount_Moderate { get; set; }
        public int status_code { get; set; }
        public int target_count { get; set; }


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
        public int renewFlag { get; set; }
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


    public class CMObservation
    {
        public int id { get; set; }
        public int check_point_type_id { get; set; }  // added type of enum 
        public int facility_id { get; set; }
        public string contractor_name { get; set; }
        public int risk_type_id { get; set; }
        public string risk_type { get; set; }
        public string preventive_action { get; set; }
        public int assigned_to_id { get; set; }
        public string assigned_to_name { get; set; }
        public string contact_number { get; set; }
        public int cost_type { get; set; }
        public string Cost_name { get; set; }
        public DateTime closed_date { get; set; }
        public string observation_status { get; set; }
        public DateTime? date_of_observation { get; set; }
        public int type_of_observation { get; set; }
        public DateTime closer_date { get; set; }
        public string location_of_observation { get; set; }
        public dynamic remaining_days { get; set; }
        public int source_of_observation { get; set; }
        public DateTime? target_date { get; set; }
        public string corrective_action { get; set; }
        public string observation_description { get; set; }
        public string type_of_observation_name { get; set; }
        public string source_of_observation_name { get; set; }
        public DateTime? created_at { get; set; }
        public string created_by { get; set; }
        public DateTime? updated_at { get; set; }
        public string updated_by { get; set; }
        public string action_taken { get; set; }
        public dynamic month_of_observation { get; set; }
        public int status_code { get; set; }
        public int createdid { get; set; }
        public int updateid { get; set; }
        public string short_status { get; set; }
        // public List<int> file_ids { get; set; }
        public List<int> uploadfileIds { get; set; }
        public string comment { get; set; }

    }
    public class CMObservationDetails
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string operator_name { get; set; }
        public int risk_type_id { get; set; }
        public string risk_type { get; set; }
        public string preventive_action { get; set; }
        public int assigned_to_id { get; set; }
        public string assigned_to_name { get; set; }
        public int status_code { get; set; }
        public string short_status { get; set; }
        public string contact_number { get; set; }
        public int cost_type { get; set; }
        public string Cost_name { get; set; }
        public DateTime? date_of_observation { get; set; }
        public int type_of_observation { get; set; }
        public string type_of_observation_name { get; set; }
        public string location_of_observation { get; set; }
        public int source_of_observation { get; set; }
        public string source_of_observation_name { get; set; }

        public DateTime? target_date { get; set; }
        public string observation_description { get; set; }
        public DateTime? created_at { get; set; }
        public string created_by { get; set; }
        public int createdid { get; set; }
        public DateTime? updated_at { get; set; }
        public string updated_by { get; set; }
        public int updateid { get; set; }
        public dynamic month_of_observation { get; set; }
        public string action_taken { get; set; }
        public dynamic observation_status { get; set; }
        public List<CMFileDetailObservation> FileDetails { get; set; }
    }
    public class CMFileDetailObservation
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public string fileCategory { get; set; }
        public double fileSize { get; set; }
        public int status { get; set; }
        public string description { get; set; }
    }
    // Added for new api sned data hardcoded data 

    public class GetChecklistInspection
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public List<InspectionData> InspectionData { get; set; }
    }

    public class InspectionData
    {
        public string MonthName { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public List<CheckList> CheckList { get; set; }
    }

    public class CheckList
    {
        public string ChecklistName { get; set; }
        public string SopNumber { get; set; }
        public string Frequency { get; set; }
        public string InspectionStatus { get; set; }
        public string DateOfInspection { get; set; }
        public string ChecklistAttachment { get; set; }
        public double NoOfUnsafeObservation { get; set; }
    }

    public class CMDocumentVersion
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public int doc_master_id { get; set; }
        public int file_id { get; set; }
        public string sub_doc_name { get; set; }
        public DateTime? renew_date { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; }
        public string Remarks { get; set; }
        public int is_renew { get; set; }
        public int docuemnt_id { get; set; }
    }
    public class CMDocumentVersionList
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int doc_master_id { get; set; }
        public string doc_master_name { get; set; }
        public dynamic Activation_status { get; set; }
        public int file_id { get; set; }
        public string file_path { get; set; }
        public string sub_doc_name { get; set; }
        public DateTime? renew_date { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; }
        public string Remarks { get; set; }
        public string description { get; set; }
    }

    public class FuelData
    {
        public int id { get; set; }
        public string month_name { get; set; }
        public int month_id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int year { get; set; }
        public int DieselConsumedForVehicles { get; set; }
        public int PetrolConsumedForVehicles { get; set; }
        public int PetrolConsumedForGrassCuttingAndMovers { get; set; }
        public int DieselConsumedAtSite { get; set; }
        public int PetrolConsumedAtSite { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public string Submited_by { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string Updated_by_name { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class KaizensData
    {
        public int id { get; set; }
        public string month_name { get; set; }
        public int month_id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string Submited_by { get; set; }
        public int year { get; set; }
        public int KaizensImplemented { get; set; }
        public int CostForImplementation { get; set; }
        public int CostSavedFromImplementation { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string Updated_by_name { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class PlantationData
    {
        public int id { get; set; }
        public string month_name { get; set; }
        public int month_id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int year { get; set; }
        public int SaplingsPlanted { get; set; }
        public int SaplingsSurvived { get; set; }
        public int SaplingsDied { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Submited_by { get; set; }
        public int UpdatedBy { get; set; }
        public string Updated_by_name { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class VisitsAndNotices
    {
        public int id { get; set; }
        public string month_name { get; set; }
        public int month_id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int year { get; set; }
        public int GovtAuthVisits { get; set; }
        public int NoOfFineByThirdParty { get; set; }
        public int NoOfShowCauseNoticesByThirdParty { get; set; }
        public int NoticesToContractor { get; set; }
        public int AmountOfPenaltiesToContractors { get; set; }
        public int AnyOther { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public string Submited_by { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string Updated_by_name { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OccupationalHealthData
    {
        public int id { get; set; }

        public int month_id { get; set; }
        public string month_name { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int year { get; set; }
        public int NoOfHealthExamsOfNewJoiner { get; set; }
        public int PeriodicTests { get; set; }
        public int OccupationalIllnesses { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public string Submited_by { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string Updated_by_name { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class CumalativeReport
    {
        public string Site_name { get; set; }
        public dynamic Created { get; set; }
        public dynamic Closed { get; set; }
        public dynamic Cancelled { get; set; }
        public dynamic NotStarted { get; set; }
        public dynamic Ongoing { get; set; }
        public dynamic ClosedOnTime { get; set; }
        public dynamic ClosedWithExtension { get; set; }
        public dynamic ClosedOnTimeCreate { get; set; }
        public dynamic CleaningType { get; set; }
        public dynamic WaterUsed { get; set; }
        public dynamic ScheduledQuantity { get; set; }
        public dynamic ActualQuantity { get; set; }
        public dynamic Abandoned { get; set; }
        public dynamic Remark { get; set; }
        public dynamic Deviation { get; set; }
        public dynamic TimeTaken { get; set; }
    }

    public class AssignToObservation
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int check_point_type_id { get; set; }
        public DateTime target_date { get; set; }
        public string comment { get; set; }
        public int assigned_to_id { get; set; }
        public int cost_type { get; set; }
        public string preventive_action { get; set; }
        public string observation_description { get; set; }
        public string contractor_name { get; set; }  // contractor_name
        public int risk_type_id { get; set; }  // risk_type_id
        public int responsible_person { get; set; }  // responsible_person
        public string contact_number { get; set; }  // contact_number   
        public DateTime date_of_observation { get; set; }  // date_of_observation    
        public string location_of_observation { get; set; }  // location_of_observation
        public string action_taken { get; set; }  // action_taken
        public int source_of_observation { get; set; }  // source_of_observation
        public int status_code { get; set; }  // status_code
        public DateTime updated_at { get; set; }  // updated_at
        public int updated_by { get; set; }  // updated_by
    }


    public class CMEvaluationUpdate : CMEvaluationCreate
    {
        public int id { get; set; }
    }

    public class CMEvaluationCreate
    {
        public string plan_name { get; set; }
        public int facility_id { get; set; }
        public int frequency_id { get; set; }
        public DateTime plan_date { get; set; }
        public int assigned_to { get; set; }
        public string remarks { get; set; }
        //List of Auditids
        public List<CMEvaluationAudit> audit_list { get; set; }
    }

    public class CMEvaluationAudit
    {
        public int id { get; set; }
        public int evalution_id { get; set; }
        public int audit_id { get; set; }
        public decimal weightage { get; set; }
        public string comment { get; set; }

        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class CMEvaluation
    {
        public int id { get; set; }
        public string plan_name { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
        public DateTime plan_date { get; set; }
        public int assigned_to { get; set; }
        public string assigned_to_name { get; set; }
        public int createdById { get; set; }
        public int updatedById { get; set; }
        public int approvedById { get; set; }
        public int rejectedById { get; set; }
        public int deletedById { get; set; }
        public string createdByName { get; set; }
        public string updatedByName { get; set; }
        public string approvedByName { get; set; }
        public string rejectedByName { get; set; }
        public string deletedByName { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? approved_at { get; set; }
        public DateTime? rejected_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public string audit_details { get; set; }

        public int status { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public List<CMEvaluationAudit> audit_list { get; set; }


    }

    public class ProjectDetails
    {
        public string SpvName { get; set; }
        public string sitename { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string TalukMandal { get; set; }
        public string ContractorName { get; set; }
        public string ContractorSiteInChargeName { get; set; }
        public string HfeSiteInChargeName { get; set; }
        public string CapacityAC { get; set; }
        public string TotalLandArea { get; set; }
        public string LengthOfInternalTransmissionLineKm { get; set; }
        public string NoOfWTGs { get; set; }
        public string NoOfInternalPoles { get; set; }
        public string LengthOfExternalTransmissionLineKm { get; set; }
        public string RatingOfPSS { get; set; }
        public string NoOfPowerTransformerInPSS { get; set; }
        public string TransformerCapacity { get; set; }
        public List<ManPowerData> MonthlyData { get; set; }
        public List<OccupationalHealthData> healthDatas { get; set; }
        public List<VisitsAndNotices> visitsAndNotices { get; set; }


    }


    /*
    public class MapAuditlist
        {
            public int id { get; set; }
            public int evalution_id { get; set; }
            public int audit_id { get; set; }
            public decimal weightage { get; set; }
            public string comments { get; set; }
            public int created_by { get; set; }
            public DateTime created_at { get; set; }
            public int updated_by { get; set; }
            public DateTime updated_at { get; set; }
        }
    */
public class ManPowerData
    {
        public dynamic AvgHFEEmployee { get; set; }
        public dynamic ManDaysHFEEmployee { get; set; }
        public dynamic ManHoursWorkedHFEEmployee { get; set; }
        public dynamic AvgContractorWorkers { get; set; }
        public dynamic ManHoursWorked { get; set; }
        public dynamic TotalManHours { get; set; }
    }

    public class IncidentAccidentData
    {
        public int FatalIncidents { get; set; }
        public int LostTimeInjuries { get; set; }
        public int MedicalTreatmentInjuries { get; set; }
        public int FirstAidIncidents { get; set; }
        public int FireIncidents { get; set; }
        public int NearMisses { get; set; }
        public int ManDaysLost { get; set; }
        public decimal CostOfAccidents { get; set; }
    }

    public class HseTrainingData
    {
        public int TotalTrainings { get; set; }
        public int TrainingManHours { get; set; }
        public int MockDrillsConducted { get; set; }
        public int SpecialTrainingsConducted { get; set; }
    }

    public class HseInspectionAuditData
    {
        public int ObservationsRaised { get; set; }
        public int ObservationsClosed { get; set; }
        public int MajorObservationsRaised { get; set; }
        public int MajorObservationsClosed { get; set; }
        public int UnsafeActsRecorded { get; set; }
        public int UnsafeConditionsRecorded { get; set; }
    }

    public class ReportChecklistData
    {
        public int ReportsToBeInspected { get; set; }
        public int ReportsInspectedInMonth { get; set; }
        public int ReportsNotInspected { get; set; }
    }

    public class GrievanceData
    {
        public int TotalGrievancesRaised { get; set; }
        public int GrievancesResolved { get; set; }
        public int WorkforceGrievancesPending { get; set; }
        public int LocalCommunityGrievancesResolved { get; set; }
    }

    public class EnviromentalSummary
    {
        public int facility_id { get; set; }
        public string facilty_name { get; set; }
        public List<OccupationalHealthData> healthDatas { get; set; }
        public List<VisitsAndNotices> visitsAndNotices { get; set; }
    }

}

