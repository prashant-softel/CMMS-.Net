using System;
using System.Collections.Generic;
namespace CMMSAPIs.Models.Masters
{
    public class TrainingSchedule
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Comment { get; set; }
        public DateTime DateOfTraining { get; set; }
        public string TrainerName { get; set; }
        public int TrainingAgencyId { get; set; }
        public int HfeEmployeeId { get; set; }
        public string Venue { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }
        public List<InternalEmployee> Scheduleinvitataion { get; set; }
        public List<ExternalEmployee> VsitorDetail { get; set; }
        public List<int> UploadfileIds { get; set; }
        public object online { get; internal set; }
    }

    public class InternalEmployee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpEmail { get; set; }
        public string EmpNumber { get; set; }
        public string EmpDesignation { get; set; }
    }

    public class ExternalEmployee
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string mobileNumber { get; set; }
        public string Designation { get; set; }
        public string CompanyName { get; set; }
    }

}
