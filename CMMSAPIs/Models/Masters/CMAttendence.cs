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
    public class CMGetCotractor
    {
        public int Id { get; set; }
        public int contractor_id { get; set; }
        public int lessThan35 { get; set; }
        public int between35to50 { get; set; }
        public int greaterThan50 { get; set; }
        public string purpose { get; set; }
    }
    public class CMGetAttendenceList
    {
        public int facility_id { get; set; }
        public string facilityname { get; set; }
        public int? month_id { get; set; }
        public String? month_name { get; set; }
        public int? year { get; set; }
        public List<MonthData>? month_data { get; set; }
    }
    public class MonthData
    {
        public string date { get; set; }
        public int hfe_employees { get; set; }
        public int less_than_35 { get; set; }
        public int between_30_to_50 { get; set; }
        public int greater_than_50 { get; set; }
    }
}

