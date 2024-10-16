using System;

namespace CMMSAPIs.Models.Grievance
{
    public class CMGrievance
    {
        public int id { get; set; }
        public string month_name { get; set; }
        //public DateTime? added_at { get; set; }
        //public string added_by { get; set; }
        public int facilityId { get; set; }
        public int grievanceTypeId { get; set; }
        public string grievanceType { get; set; }
        public string concern { get; set; }
        public string description { get; set; }
        public string type_description { get; set; }
        public string actionTaken { get; set; }
        public int resolutionLevel { get; set; }
        public string closedBy { get; set; }
        public string closedByName { get; set; }
        public int createdBy { get; set; }
        public string createdByName { get; set; }
        public dynamic createdAt { get; set; }
        public int updatedBy { get; set; }
        public string updatedByName{ get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime deletedAt { get; set; }
        public string deletedBy { get; set; }
        public string deletedByName { get; set; }
        public DateTime closedAt { get; set; }
        //        public int addedBy { get; set; }
        //      public DateTime? updated_at { get; set; }
        //       public string updated_by { get; set; }
        //       public DateTime? deleted_at { get; set; }
        //       public string deleted_by { get; set; }
        //      public string status_long { get; set; }
        public int statusId { get; set; }
        public string statusShort { get; set; }
        public string statusLong { get; set; }
        public int status { get; set; }
    }

    public class CMUpdateGrievance
    {
        public int id { get; set; }
        public int facilityId { get; set; }
        public int grievanceType { get; set; }
        //public string grievance { get; set; }
        public string concern { get; set; }
        public string description { get; set; }
        public string actionTaken { get; set; }
        public int resolutionLevel { get; set; }
        public int updatedBy { get; set; }
//        public DateTime? closedDate { get; set; }
 //       public DateTime? createdAt { get; set;}
 //       public int updatedBy { get; set; }

    }

    public class GrievanceSummaryReport
    {
        public int months { get; set; }
        public string month_name { get; set; }
        public int year {  get; set; }

      

        public GrievanceSummaryReport(int months, string month_name, int year)
        {
            this.months = months;
            this.month_name = month_name;
            this.year = year;
        }
        public int number_of_work_force_grievances {  get; set; }
        public int number_of_local_community_grievances { get; set; }
        public int workforce_case_resolved {  get; set; }
        public int worforce_case_pending {  get; set; }
        public int worforce_inspection_ongoing { get; set; }
        public int local_cases_resolved { get; set; }
        public int local_cases_pending { get; set; }
        public int local_cases_inspection_ongoing { get; set; }
        public int total_numer_of_grievances_raised { get; set; }
        public int total_number_of_grievances_resolved {  get; set; }
        public int total_number_of_grievances_pending {  get; set; }
        public int resolved_at_l1 {  get; set; }
        public int resolved_at_l2 {  get; set; }
        public int resolved_at_l3 {  get; set; }

    }
}

