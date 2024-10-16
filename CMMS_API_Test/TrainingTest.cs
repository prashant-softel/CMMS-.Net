using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class TrainingTest
    {
        //Traning 
        string EP_CreateTrainingCourse = "/api/Training/CreateTrainingCourse";
        string EP_GetCourseList = "/api/Training/GetCourseList";
        string EP_GetCourseDetailById = "/api/Training/GetCourseDetailById";
        string EP_UpdateCourse = "/api/Training/UpdateCourse";
        string EP_DeleteCourse = "/api/Training/DeleteCourse";

        //Schedule
        string EP_GetScheduleCourseList = "/api/Training/GetScheduleCourseList";
        string EP_GetScheduleCourseDetail = "/api/Training/GetScheduleCourseDetail";
        string EP_CreateScheduleCourse = "/api/Training/ScheduleCourse";
        string EP_ExecuteScheduleCourse = "/api/Training/ExecuteScheduleCourse";




        [TestMethod]
        public void VerifyGetCourseList()
        {
            int facility_id = 17;
            string start_date = "2024-10-01";
            string end_date = "2024-10-10";

            var TrainingService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMTrainingCourse>>();
            var response = TrainingService.GetItem(EP_GetCourseList + "?facility_id=" + facility_id + "&start_date=" + start_date + "&end_date=" + end_date);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.IsTrue(response.Count > 0, "The Training list should not be empty.");

        }


        [TestMethod]
        public void VerifyGetCourseDetailById()
        {
            int id = 3;

            var TrainingService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMTrainingCourse>>();
            var response = TrainingService.GetItem(EP_GetCourseDetailById + "?id=" + id);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.AreEqual(id, response[0].Id, "The training ID should match.");
        }




        [TestMethod]
        public void VerifyCreateTrainingCourse()
        {
            var payload = new CMMSAPIs.Models.Masters.CMTrainingCourse
            {
                Id = 0,
                name = "FSD",
                category_id = 2,
                group_id = 1,
                number_of_days = 365,
                duration = 21900,
                uploadfile_ids = new List<int>(),
                facility_id = 17
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var TrainingService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = TrainingService.CreateItem(EP_CreateTrainingCourse, jsonPayload);

            int id = Response.id[0];

            Assert.AreEqual(Response.message, "Course Added Successfully.");

            var getItem = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMTrainingCourse>>();
            var getItemResponse = getItem.GetItem(EP_GetCourseDetailById + "?id=" + id);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_CREATED, getItemResponse[0].status);

            Assert.AreEqual(payload.name, getItemResponse[0].name, "name should be match");
            Assert.AreEqual(payload.category_id, getItemResponse[0].category_id, "category_id should be match");
            Assert.AreEqual(payload.group_id, getItemResponse[0].group_id, "group_id should be match");
            Assert.AreEqual(payload.number_of_days, getItemResponse[0].number_of_days, "number_of_days should be match");
            Assert.AreEqual(payload.duration, getItemResponse[0].duration, "duration should be match");
            Assert.AreEqual(payload.facility_id, getItemResponse[0].facility_id, "facility_id should be match"); 
        }



        [TestMethod]
        public void VerifyUpdateCourse()
        {
            var payload = new CMMSAPIs.Models.Masters.CMTrainingCourse
            {
                Id = 2,
                name = "FSD",
                category_id = 2,
                group_id = 1,
                number_of_days = 365,
                duration = 21900,
                uploadfile_ids = new List<int>(),
                facility_id = 17
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var TrainingService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = TrainingService.PatchService(EP_UpdateCourse, jsonPayload);

            int id = Response[0].id[0];

            Assert.AreEqual(Response[0].message, "Course Updated Successfully.");

            var getItem = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMTrainingCourse>>();
            var getItemResponse = getItem.GetItem(EP_GetCourseDetailById + "?id=" + id);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_UPDATED, getItemResponse[0].status);

            Assert.AreEqual(payload.name, getItemResponse[0].name, "name should be match");
            Assert.AreEqual(payload.category_id, getItemResponse[0].category_id, "category_id should be match");
            Assert.AreEqual(payload.group_id, getItemResponse[0].group_id, "group_id should be match");
            Assert.AreEqual(payload.number_of_days, getItemResponse[0].number_of_days, "number_of_days should be match");
            Assert.AreEqual(payload.duration, getItemResponse[0].duration, "duration should be match");
            Assert.AreEqual(payload.facility_id, getItemResponse[0].facility_id, "facility_id should be match");

        }



        /*[TestMethod]
        public void VerifyDeleteCourse()
        {
            int id = 2;
            //var payload = id;

            var TrainingService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = TrainingService.DeleteService(EP_DeleteCourse + "id=" + id, payload);

            int Aud_id = Response[0].id[0];

            Assert.AreEqual(Response[0].message, ("Course Deleted Successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMTrainingCourse>();
            var getItemresponse = getItem.GetItem(EP_GetCourseDetailById + "?id=" + Aud_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_DELETED, getItemresponse.status);

        }*/




        [TestMethod]
        public void VerifyGetScheduleCourseList()
        {
            int facility_id = 17;
            string start_date = "2024-10-01";
            string end_date = "2024-10-10";

            var ScheduleService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.GETSCHEDULE>>();
            var response = ScheduleService.GetItem(EP_GetScheduleCourseList + "?facility_id=" + facility_id + "&start_date=" + start_date + "&end_date=" + end_date);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.IsTrue(response.Count > 0, "The Training list should not be empty.");

        }


        [TestMethod]
        public void VerifyGetScheduleCourseDetail()
        {
            int schedule_id = 3;

            var ScheduleService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL>>();
            var response = ScheduleService.GetItem(EP_GetScheduleCourseDetail + "?schedule_id=" + schedule_id);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.AreEqual(schedule_id, response[0].ScheduleID, "The training ID should match.");
        }




        [TestMethod]
        public void VerifyCreateScheduleCourse()
        {
            var payload = new CMMSAPIs.Models.Masters.TrainingSchedule
            {
                CourseId = 2,
                CourseName = "MCA",
                Comment = "TEST comment",
                Date_of_training = "2024-10-15",
                TrainerName = "Adesh",
                TrainingAgencyId = 1,
                HfeEmployeeId = 215,
                Venue = "no",
                Mode = "Online + Offline",
                facility_id = 17,
                externalEmployees = new List<ExternalEmployee>
                {
                    new ExternalEmployee
                    {
                        employeeName = "aditya",
                        employeeEmail = "aditya@gmail.com",
                        employeeNumber = "7039450903",
                        designation = "SDE",
                        companyName = "HFE"
                    }
                },
                internalEmployees = new List<CMMSAPIs.Models.Masters.InternalEmployee>
                {
                    new InternalEmployee
                    {
                        EmpId = 216,
                        empName = "Kundan Kumar",
                        empEmail = "kundan.kumar2@herofutureenergies.com",
                        empNumber = "9654670663",
                        empDesignation = "Senior Manager"
                    },

                    new InternalEmployee
                    {
                        EmpId = 215,
                        empName = "Kumar Prabhanshu",
                        empEmail = "kumar.prabhanshu@herofutureenergies.com",
                        empNumber = "8590686751",
                        empDesignation = "Solar Site Manager - HFE"
                    }
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var TrainingService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = TrainingService.CreateItem(EP_CreateScheduleCourse, jsonPayload);

            int schedule_id = Response.id[0];

            Assert.AreEqual(Response.message, "Course Schedule Successfully Created");

            var ScheduleService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL>>();
            var response = ScheduleService.GetItem(EP_GetScheduleCourseDetail + "?schedule_id=" + schedule_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_SCHEDULE, response[0].status_code);

            Assert.AreEqual(Response.id[0], response[0].ScheduleID, "CourseId should be match");
            Assert.AreEqual(payload.CourseName, response[0].Training_course, "CourseName should be match");
            Assert.AreEqual(payload.Date_of_training, response[0].Date_of_Trainig, "Date_of_training should be match");
            Assert.AreEqual(payload.TrainerName, response[0].Trainer, "TrainerName should be match");
            //Assert.AreEqual(payload.TrainingAgencyId, response[0].Training_Agency, "TrainingAgencyId should be match");
            Assert.AreEqual(payload.HfeEmployeeId, response[0].hfeEmployeeId, "HfeEmployeeId should be match");
            Assert.AreEqual(payload.Venue, response[0].Venue, "Venue should be match");
            Assert.AreEqual(payload.Mode, response[0].Mode, "Mode should be match");

            for(int i = 0; i < payload.internalEmployees.Count; i++)
            {
                var InternamExpected = payload.internalEmployees[i];
                var InternalActual = response[0].internal_employee[i];

                Assert.AreEqual(InternamExpected.EmpId, InternalActual.employee_id, "emp id should be match");
                Assert.AreEqual(InternamExpected.empEmail, InternalActual.email, "empEmail should be match");
                Assert.AreEqual(InternamExpected.empDesignation, InternalActual.designation, "empDesignation should be match");
                Assert.AreEqual(InternamExpected.empNumber, InternalActual.mobile, "empNumber should be match");
                Assert.AreEqual(InternamExpected.empName, InternalActual.name, "empName should be match");
            }


            for (int i = 0; i < payload.externalEmployees.Count; i++)
            {
                var ExternalExpected = payload.externalEmployees[i];
                var ExternalActual = response[0].external_employee[i];

                Assert.AreEqual(ExternalExpected.employeeName, ExternalActual.employeeName, "employeeName should be match");
                Assert.AreEqual(ExternalExpected.employeeEmail, ExternalActual.employeeEmail, "employeeEmail should be match");
                Assert.AreEqual(ExternalExpected.employeeNumber, ExternalActual.employeeNumber, "employeeNumber should be match");
                //Assert.AreEqual(ExternalExpected.designation, ExternalActual.designation, "designation should be match");
                Assert.AreEqual(ExternalExpected.companyName, ExternalActual.companyName, "companyName should be match");
            }

        }







        [TestMethod]
        public void VerifyExecuteScheduleCourse()
        {
            var payload = new CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL
            {
                ScheduleID = 20,
                Date_of_Trainig = "2024-10-11",
                Trainer = "Adesh",
                hfeEmployeeId = 215,
                Venue = "no",
                Mode = "Online - Offline",
                Training_Agency = "Hero Future Energies",
                HFE_Epmloyee = "KumarPrabhanshu",
                facility_id = 17,
                external_employee = new List<ExternalEmployee>
                {
                    new ExternalEmployee
                    {
                        name = "aditya",
                        email = "aditya@gmail.com",
                        mobile = "7039450903",
                        attendend = 1,
                        designation = "SDE",
                        companyName = "HFE",
                        Rsvp = DateTime.Parse("2024-10-15"),
                        notes = "no_notes"
                    }
                },
                internal_employee = new List<CMMSAPIs.Models.Masters.INTERNALEMPLOYEES>
                {
                    new INTERNALEMPLOYEES
                    {
                        employee_id = 216,
                        name = "Kundan Kumar",
                        attendend = 1,
                        email = "kundan.kumar2@herofutureenergies.com",
                        mobile = "9654670663",
                        designation = "Senior Manager",
                        rsvp = DateTime.Parse("2024-10-15"),
                        notes = "no_notes"
                    },

                    new INTERNALEMPLOYEES
                    {
                        employee_id = 215,
                        name = "Kumar Prabhanshu",
                        attendend = 1,
                        email = "kumar.prabhanshu@herofutureenergies.com",
                        mobile = "8590686751",
                        designation = "Solar Site Manager - HFE",
                        rsvp = DateTime.Parse("2024-10-15"),
                        notes = "no_notes"
                    }
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var TrainingService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = TrainingService.CreateItem(EP_ExecuteScheduleCourse, jsonPayload);

            int schedule_id = Response.id[0];

            Assert.AreEqual(Response.message, "Schedule Coures Executed");

            var ScheduleService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL>>();
            var response = ScheduleService.GetItem(EP_GetScheduleCourseDetail + "?schedule_id=" + schedule_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_ENDED, response[0].status_code);

            Assert.AreEqual(schedule_id, response[0].ScheduleID, "CourseId should be match");
            //Assert.AreEqual(payload.course_name, response[0].Training_course, "CourseName should be match");
            Assert.AreEqual(payload.Date_of_Trainig, response[0].Date_of_Trainig, "Date_of_training should be match");
            Assert.AreEqual(payload.Trainer, response[0].Trainer, "TrainerName should be match");
            Assert.AreEqual(payload.Training_Agency, response[0].Training_Agency, "TrainingAgencyId should be match");
            Assert.AreEqual(payload.hfeEmployeeId, response[0].hfeEmployeeId, "HfeEmployeeId should be match");
            Assert.AreEqual(payload.Venue, response[0].Venue, "Venue should be match");
            Assert.AreEqual(payload.Mode, response[0].Mode, "Mode should be match");
            Assert.AreEqual(payload.HFE_Epmloyee, response[0].HFE_Epmloyee, "HFE_Epmloyee should be match");

            for (int i = 0; i < payload.internal_employee.Count; i++)
            {
                var InternamExpected = payload.internal_employee[i];
                var InternalActual = response[0].internal_employee[i];

                Assert.AreEqual(InternamExpected.employee_id, InternalActual.employee_id, "emp id should be match");
                Assert.AreEqual(InternamExpected.email, InternalActual.email, "empEmail should be match");
                Assert.AreEqual(InternamExpected.designation, InternalActual.designation, "empDesignation should be match");
                Assert.AreEqual(InternamExpected.mobile, InternalActual.mobile, "empNumber should be match");
                Assert.AreEqual(InternamExpected.name, InternalActual.name, "empName should be match");
                //Assert.AreEqual(InternamExpected.attendend, InternalActual.attendend, "attendend should be match");
                //Assert.AreEqual(InternamExpected.rsvp, InternalActual.rsvp, "rsvp should be match");
            }
        }



        [TestMethod]
        public void VerifyFunctionalTestTraining()
        {



            // Step 1: Create Training Course
            var createPayload = new CMMSAPIs.Models.Masters.CMTrainingCourse
            {
                Id = 0,
                name = "FSD",
                category_id = 2,
                group_id = 1,
                number_of_days = 365,
                duration = 21900,
                uploadfile_ids = new List<int>(),
                facility_id = 17
            };

            string createJsonPayload = JsonConvert.SerializeObject(createPayload);
            var TrainingService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var createResponse = TrainingService.CreateItem(EP_CreateTrainingCourse, createJsonPayload);
            int courseId = createResponse.id[0];

            Assert.AreEqual(createResponse.message, "Course Added Successfully.");

            var getItem = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMTrainingCourse>>();
            var getItemResponse = getItem.GetItem(EP_GetCourseDetailById + "?id=" + courseId);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_CREATED, getItemResponse[0].status);



            // Step 2: Update Training Course
            var updatePayload = new CMMSAPIs.Models.Masters.CMTrainingCourse
            {
                Id = courseId,  // Use the created course ID
                name = "Updated FSD",
                category_id = 3,
                group_id = 2,
                number_of_days = 180,
                duration = 14400,
                uploadfile_ids = new List<int>(),
                facility_id = 17
            };

            string updateJsonPayload = JsonConvert.SerializeObject(updatePayload);
            var updateResponse = TrainingService.PatchService(EP_UpdateCourse, updateJsonPayload);
            Assert.AreEqual(updateResponse[0].message, "Course Updated Successfully.");

            var getItem1 = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMTrainingCourse>>();
            var getItemResponse1 = getItem1.GetItem(EP_GetCourseDetailById + "?id=" + courseId);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_UPDATED, getItemResponse1[0].status);




            // Step 3: Create Training Schedule
            var schedulePayload = new CMMSAPIs.Models.Masters.TrainingSchedule
            {
                CourseId = courseId, // Use the updated course ID
                CourseName = "Updated FSD",
                Comment = "Scheduled after update",
                Date_of_training = "2024-10-15",
                TrainerName = "Adesh",
                TrainingAgencyId = 1,
                HfeEmployeeId = 215,
                Venue = "Virtual",
                Mode = "Online",
                facility_id = 17,
                externalEmployees = new List<ExternalEmployee>
        {
            new ExternalEmployee
            {
                employeeName = "John Doe",
                employeeEmail = "john.doe@gmail.com",
                employeeNumber = "9876543210",
                designation = "Engineer",
                companyName = "TechCorp"
            }
        },
                internalEmployees = new List<InternalEmployee>
        {
            new InternalEmployee
            {
                EmpId = 215,
                empName = "Kumar Prabhanshu",
                empEmail = "kumar.prabhanshu@herofutureenergies.com",
                empNumber = "8590686751",
                empDesignation = "Solar Site Manager"
            }
        }
            };

            string scheduleJsonPayload = JsonConvert.SerializeObject(schedulePayload);
            var scheduleResponse = TrainingService.CreateItem(EP_CreateScheduleCourse, scheduleJsonPayload);
            int scheduleId = scheduleResponse.id[0];

            Assert.AreEqual(scheduleResponse.message, "Course Schedule Successfully Created");

            var ScheduleService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL>>();
            var response = ScheduleService.GetItem(EP_GetScheduleCourseDetail + "?schedule_id=" + scheduleId);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_SCHEDULE, response[0].status_code);




            // Step 4: Execute Training Schedule
            var executePayload = new CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL
            {
                ScheduleID = scheduleId,
                Date_of_Trainig = "2024-10-15",
                Trainer = "Adesh",
                hfeEmployeeId = 215,
                Venue = "Virtual",
                Mode = "Online",
                Training_Agency = "Hero Future Energies",
                HFE_Epmloyee = "KumarPrabhanshu",
                facility_id = 17,
                external_employee = new List<ExternalEmployee>
        {
            new ExternalEmployee
            {
                name = "John Doe",
                email = "john.doe@gmail.com",
                mobile = "9876543210",
                attendend = 1,
                designation = "Engineer",
                companyName = "TechCorp",
                Rsvp = DateTime.Parse("2024-10-15"),
                notes = "No issues"
            }
        },
                internal_employee = new List<CMMSAPIs.Models.Masters.INTERNALEMPLOYEES>
        {
            new INTERNALEMPLOYEES
            {
                employee_id = 215,
                name = "Kumar Prabhanshu",
                attendend = 1,
                email = "kumar.prabhanshu@herofutureenergies.com",
                mobile = "8590686751",
                designation = "Solar Site Manager",
                rsvp = DateTime.Parse("2024-10-15"),
                notes = "Completed successfully"
            }
        }
            };

            string executeJsonPayload = JsonConvert.SerializeObject(executePayload);
            var executeResponse = TrainingService.CreateItem(EP_ExecuteScheduleCourse, executeJsonPayload);
            Assert.AreEqual(executeResponse.message, "Schedule Coures Executed");

            var ScheduleService1 = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.GETSCHEDULEDETAIL>>();
            var executionResponse = ScheduleService1.GetItem(EP_GetScheduleCourseDetail + "?schedule_id=" + scheduleId);

            Assert.AreEqual((int)CMMS.CMMS_Status.COURSE_ENDED, executionResponse[0].status_code);

            Assert.AreEqual(scheduleId, executionResponse[0].ScheduleID, "ScheduleID should match");
            Assert.AreEqual(executePayload.Trainer, executionResponse[0].Trainer, "Trainer should match");
            Assert.AreEqual(executePayload.Mode, executionResponse[0].Mode, "Mode should match");
            Assert.AreEqual(executePayload.Venue, executionResponse[0].Venue, "Venue should match");
            Assert.AreEqual(executePayload.HFE_Epmloyee, executionResponse[0].HFE_Epmloyee, "HFE Employee should match");
        }


    }
}
