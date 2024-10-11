using System;
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
        public string name { get; set; }
        public string employeeEmail { get; set; }
        public string email { get; set; }
        public DateTime rsvps { get; set; }
        public int employee_id { get; set; }
        public dynamic employeeNumber { get; set; }
        public dynamic mobile { get; set; }
        public string designation { get; set; }
        public DateTime Rsvp { get; set; }
        public int attendend { get; set; }
        public string notes { get; set; }
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
        public int status { get; set; }
        public int Traning_category_id { get; set; }
        public int No_Of_Days { get; set; }
        public int Targeted_Group_Id { get; set; }
        public int Duration_in_Minutes { get; set; }
        public string Course_Category { get; set; }
        public string Targeted_Group { get; set; }
        public string short_status { get; set; }
        public string month_name { get; set; }
        public int status_code { get; set; }



    }
    public class GETSCHEDULEDETAIL
    {

        public int ScheduleID { get; set; }
        public int facility_id { get; set; }
        public string Date_of_Trainig { get; set; }
        public string Training_course { get; set; }
        public string Trainer { get; set; }
        public string Training_company { get; set; }
        public int hfeEmployeeId { get; set; }
        public string course_name { get; set; }
        public int status { get; set; }
        public string short_status { get; set; }
        public string Mode { get; set; }
        public string Venue { get; set; }
        public string Training_Agency { get; set; }
        public string HFE_Epmloyee { get; set; }
        public int status_code { get; set; }
        public List<INTERNALEMPLOYEES> internal_employee { get; set; }
        public List<ExternalEmployee> external_employee { get; set; }
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
    public class INTERNALEMPLOYEES
    {
        public int id { get; set; }

        public int Schid { get; set; }
        public int employee_id { get; set; }
        public int Visitor_id { get; set; }
        public DateTime rsvp { get; set; }
        public string notes { get; set; }
        public string designation { get; set; }
        public dynamic attendend { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public dynamic mobile { get; set; }
        public dynamic attended { get; set; }

    }


    public class CMTrainingSummary
    {

        public CMTrainingSummary(int month, string month_name, int year)
        {
            this.month = month;
            this.month_name = month_name;
            this.year = year;
        }
        public int created { get; set; }
        public int month { get; set; }
        public string month_name { get; set; }
        public int year { get; set; }
        public int closed { get; set; }
        public decimal manHours { get; set; }
        public int special_mockDrill { get; set; }
        public int hfe_mockDrill { get; set; }
        public int induction { get; set; }
        public int hfe_training { get; set; }
        public int special_training { get; set; }
        public int number_of_people_inducted { get; set; }
        public decimal total_man_hours_excluding_mock_and_induction { get; set; }
        public decimal total_training_hours_excluding_mock_and_induction { get; set; }
        public int scheduled { get; set; }
        public int ended { get; set; }
    }

}
