﻿using System;
using System.Collections.Generic;
namespace CMMSAPIs.Models.Masters
{
    public class TrainingSchedule
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Comment { get; set; }
        public string Date_of_training { get; set; }
        public string TrainerName { get; set; }
        public int TrainingAgencyId { get; set; }
        public int HfeEmployeeId { get; set; }
        public string Venue { get; set; }
        public DateTime CreatedAt { get; set; }
        public int facility_id { get; set; }
        public string Mode { get; set; }
        public int CreatedBy { get; set; }
        public int status_code { get; set; }
        public string short_status { get; set; }
        public List<InternalEmployee> internalEmployees { get; set; }
        public List<ExternalEmployee> externalEmployees { get; set; }
        public List<int> uploadfile_ids { get; set; }

    }

    public class InternalEmployee
    {
        public int EmpId { get; set; }
        public string empName { get; set; }
        public string empEmail { get; set; }
        public string empNumber { get; set; }
        public string Attendence { get; set; }
        public string empDesignation { get; set; }
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
    public class GETSCHEDULE
    {
        public int ScheduleID { get; set; }
        public int CourseID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string TrainingCompany { get; set; }
        public string Trainer { get; set; }
        public string Mode { get; set; }
        public string Venue { get; set; }
        public int Traning_category_id { get; set; }
        public int No_Of_Days { get; set; }
        public int Targeted_Group_Id { get; set; }
        public int Duration_in_Minutes { get; set; }
        public string Course_Category { get; set; }
        public string Targeted_Group { get; set; }



    }
    public class GETSCHEDULEDETAIL
    {

        public int ScheduleID { get; set; }
        public int facility_id { get; set; }
        public string Date_of_Trainig { get; set; }
        public string Training_course { get; set; }
        public string Trainer { get; set; }
        public string Mode { get; set; }
        public string Venue { get; set; }
        public string Training_Agency { get; set; }
        public string HFE_Epmloyee { get; set; }
        public List<INTERNALEMPLOYEE> internal_employee { get; set; }
        public List<INTERNALEMPLOYEE> external_employee { get; set; }
        public List<CMTRAININGFILE> uploadfile_ids { get; set; }
    }
    public class INTERNALEMPLOYEE
    {
        public int id { get; set; }
        // public int ScheduleID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public dynamic Mobile { get; set; }
        public int Attendend { get; set; }
        public dynamic Rsvp { get; set; }
        public string notes { get; set; }

    }

}
