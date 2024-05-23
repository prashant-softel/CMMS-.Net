using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMCreateAttendence
    {

        public DateTime Date { get; set; }
        public List<CMGetAttendence> hfeAttendance { get; set; }
        public CMGetCotractor contractAttendance { get; set; }


    }
    public class CMGetAttendence
    {
        public int Id { get; set; }
        public int Attendance_Id { get; set; }

        public int Facility_Id { get; set; }
        public int Employee_Id { get; set; }
        public dynamic Present { get; set; }
        public string In_Time { get; set; }
        public string Out_Time { get; set; }
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
        public int Contractor_Id { get; set; }
        public int Age_Less_Than35 { get; set; }
        public int Age_Between_35And50 { get; set; }
        public int Age_Greater50 { get; set; }
        public string Purpose { get; set; }
    }
}
