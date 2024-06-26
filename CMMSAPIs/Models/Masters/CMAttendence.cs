using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMCreateAttendence
    {
        public DateTime date { get; set; }
        public int facility_id { get; set; }
        public List<CMGetAttendence> hfeAttendance { get; set; }
        public CMGetCotractor contractAttendance { get; set; }


    }
    public class CMGETAttendenceDETAIL
    {
        public dynamic date { get; set; }
        public int facility_id { get; set; }
        public List<CMGetAttendence1> hfeAttendance { get; set; }
        // public CMGetCotractor contractAttendance { get; set; }


    }
    public class CMGetAttendence
    {
        public int id { get; set; }
        public string name { get; set; }
        public dynamic present { get; set; }
        public string? InTime { get; set; }
        public string? OutTime { get; set; }
        public int Attendance_Id { get; set; }
        public int facility_id { get; set; }

        public int employee_id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public dynamic Dates { get; set; }
        public CMGetCotractor Contractors { get; set; }
    }
    public class CMGetAttendence1
    {
        //  public int id { get; set; }
        public int employee_id { get; set; }
        public string name { get; set; }
        public dynamic present { get; set; }
        public string? In_Time { get; set; }

        public string? Out_Time { get; set; }


    }

    public class CMGetCotractor
    {
        // public int Id { get; set; }
        public int contractor_id { get; set; }
        public int lessThan35 { get; set; }
        public int between35to50 { get; set; }
        public int greaterThan50 { get; set; }
        public string purpose { get; set; }


    }
    public class CMGetCotractor1
    {

        //  public int id { get; set; }
        public int contractor_id { get; set; }
        public int Age_Less_Than35 { get; set; }
        public int Age_Between_35And50 { get; set; }
        public int Age_Greater50 { get; set; }
        public string Purpose { get; set; }

    }
    public class CMGetAttendenceList
    {
        public int facility_id { get; set; }
        //s   public DateTime dates { get; set; }
        public int? month_id { get; set; }
        public String? month_name { get; set; }
        public int? years { get; set; }
        public List<MonthData>? month_data { get; set; }
    }
    public class MonthData
    {
        public DateTime date { get; set; }
        public Int64 hfe_employees { get; set; }

        public decimal age_lessthan_35 { get; set; }
        public decimal age_Between_35_50 { get; set; }
        public decimal age_Greater_50 { get; set; }
    }
    public class CMGETAttendenceMONTH
    {
        public int? facility_id { get; set; }
        public String? facility_name { get; set; }
        public List<Employee>? attendance { get; set; }
    }
    public class EmployeeMonth
    {
        public int? employeeId { get; set; }
        public String? employeeName { get; set; }
        public DateTime dateOfJoining { get; set; }
        public DateTime DateofExit { get; set; }
        public String? workingStatus { get; set; }
        public List<DetailsOFMonth>? details { get; set; }
    }
    public class DetailsOFMonth
    {
        public int emp_id { get; set; }
        public DateTime date { get; set; }
        public dynamic status { get; set; }
        public string? inTime { get; set; }
        public string? outTime { get; set; }
    }
}

