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

        public string Mode { get; set; }
        public int CreatedBy { get; set; }
        public List<InternalEmployee> internalEmployees { get; set; }
        public List<ExternalEmployee> externalEmployees { get; set; }
        public List<int> UploadfileIds { get; set; }

    }

    public class InternalEmployee
    {
        public int EmpId { get; set; }
        public string empName { get; set; }
        public string empEmail { get; set; }
        public string empNumber { get; set; }
        public string Attendence { get; set; }
        public int empDesignation { get; set; }
    }

    public class ExternalEmployee
    {
        public int id { get; set; }
        public string employeeName { get; set; }
        public string employeeEmail { get; set; }
        public string employeeNumber { get; set; }
        public string designation { get; set; }
        public string Address { get; set; }
        public string companyName { get; set; }
    }

}
