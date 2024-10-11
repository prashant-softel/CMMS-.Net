using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.PM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class AuditTest
    {

        string EP_GetAuditPlanList = "/api/AuditPlan/GetAuditPlanList";
        string EP_GetAuditPlanByID = "/api/AuditPlan/GetAuditPlanByID";
        string EP_CreateAuditPlan = "/api/AuditPlan/CreateAuditPlan";
        string EP_UpdateAuditPlan = "/api/AuditPlan/UpdateAuditPlan";
        string EP_ApproveAuditPlan = "/api/AuditPlan/ApproveAuditPlan";
        string EP_RejectAuditPlan = "/api/AuditPlan/RejectAuditPlan";

        string EP_GetTaskDetail = "/api/AuditPlan/GetTaskDetail";
        string EP_CreateAuditSkip = "/api/AuditPlan/CreateAuditSkip";
        string EP_ApproveAuditSkip = "/api/AuditPlan/ApproveAuditSkip";
        string EP_RejectAuditSkip = "/api/AuditPlan/RejectAuditSkip";
        string EP_StartAuditTask = "/api/AuditPlan/StartAuditTask";
        string EP_CloseAuditPlan = "/api/AuditPlan/CloseAuditPlan";
        string EP_ApproveClosedAuditPlan = "/api/AuditPlan/ApproveClosedAuditPlan";
        string EP_RejectCloseAuditPlan = "/api/AuditPlan/RejectCloseAuditPlan";
        string EP_AssignAuditTask = "/api/AuditPlan/AssignAuditTask";
        string EP_ExecuteAuditSchedule = "/api/AuditPlan/ExecuteAuditSchedule";








        [TestMethod]
        public void VerifyGetAuditPlanList()
        {
            int facility_id = 1;
            string fromDate = "2024-08-01";
            string toDate = "2024-08-10";
            int module_type_id = 3;

            var AuditPlanService = new CMMS_Services.APIService<List<CMMSAPIs.Models.PM.CMPMPlanList>>();
            var response = AuditPlanService.GetItem(EP_GetAuditPlanList + "?facility_id=" + facility_id + "&fromDate=" + fromDate + "&toDate=" + toDate + "&module_type_id=" + module_type_id);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.IsTrue(response.Count > 0, "The Audit Plan list should not be empty.");
            //Assert.AreEqual(facility_id, response[0].facility_id, "The facility ID should match.");
        }

        [TestMethod]
        public void VerifyGetAuditPlanByID()
        {
            int id = 127;
            int facility_id = 1;

            var AuditPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var response = AuditPlanService.GetItem(EP_GetAuditPlanByID + "?id=" + id + "&facility_id=" + facility_id);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.AreEqual(id, response.id, "The audit plan ID should match.");
        }

        [TestMethod]
        public void VerifyCreateAuditPlan()
        {
            int facility_id = 1;
            var payload = new CMMSAPIs.Models.Audit.CMCreateAuditPlan
            {
                plan_number = "adi123",
                assignedTo = "Admin HFE",
                Module_Type_id = 3,
                id = 0,
                is_PTW = false,
                max_score = 0,
                auditor_id = 1,
                auditee_id = 1,
                Facility_id = 1,
                ApplyFrequency = 1,
                Checklist_id = 136,
                Description = "no desc",
                Schedule_Date = DateTime.Parse("2024-10-08"),
                map_checklist = new List<CMEvaluationAudit>
                {
                    new CMEvaluationAudit()
                },

                Employees = new List<string> 
                {
                    "Pradeep Tholety",
                    "Majid Shaikh"
                },
            };

            string createjsonPayload = JsonConvert.SerializeObject(payload);

            var AuditService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = AuditService.CreateItem(EP_CreateAuditPlan, createjsonPayload);

            int Aud_id = Response.id[0];

            //Assert.AreEqual(Response.message, $("Plan with plan number : " + getItemresponse.title + " created successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var getItemresponse = getItem.GetItem(EP_GetAuditPlanByID + "?id=" + Aud_id + "&facility_id=" + facility_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SCHEDULE, getItemresponse.status);

            //Assert.AreEqual(payload.plan_number, getItemresponse., "plan_number should be match");
            Assert.AreEqual(payload.assignedTo, getItemresponse.assignedTo, "assignedTo should be match");
            Assert.AreEqual(payload.Module_Type_id, getItemresponse.Module_Type_id, "Module_Type_id should be match");
            Assert.AreEqual(payload.is_PTW.ToString(), getItemresponse.is_PTW, "is_PTW should be match");
            Assert.AreEqual(payload.max_score, getItemresponse.max_score, "max_score should be match");
            Assert.AreEqual("Bellary", getItemresponse.facility_name, "facility_name should be match");
            //Assert.AreEqual(payload.ApplyFrequency.ToString(), getItemresponse.FrequencyApplicable, "FrequencyApplicable should be match");
            Assert.AreEqual(payload.Checklist_id, getItemresponse.Checklist_id, "Checklist_id should be match");
            Assert.AreEqual(payload.Description, getItemresponse.Description, "Description should be match");
            //Assert.AreEqual(payload.Schedule_Date, getItemresponse.Schedule_Date, "Schedule_Date should be match");

            var actualEmployees = getItemresponse.Employees.Split(", ").ToList();
            Assert.AreEqual(payload.Employees[0], actualEmployees[0]);

        }




        [TestMethod]
        public void VerifyUpdateAuditPlan()
        {
            int facility_id = 1;
            var payload = new CMMSAPIs.Models.Audit.CMCreateAuditPlan
            {
                plan_number = "adi123",
                assignedTo = "Admin HFE",
                Module_Type_id = 3,
                id = 132,
                is_PTW = false,
                max_score = 0,
                auditor_id = 1,
                auditee_id = 1,
                Facility_id = 1,
                ApplyFrequency = 1,
                Checklist_id = 136,
                Description = "no desc",
                Schedule_Date = DateTime.Parse("2024-10-08"),
                map_checklist = new List<CMEvaluationAudit>
                {
                    new CMEvaluationAudit()
                },

                Employees = new List<string>
                {
                    "Pradeep Tholety",
                    "Majid Shaikh"
                },
            };

            string createjsonPayload = JsonConvert.SerializeObject(payload);

            var AuditService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = AuditService.CreateItem(EP_UpdateAuditPlan, createjsonPayload);

            int Aud_id = Response.id[0];

            Assert.AreEqual(Response.message, ("Audit plan with plan number :  updated successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var getItemresponse = getItem.GetItem(EP_GetAuditPlanByID + "?id=" + Aud_id + "&facility_id=" + facility_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SCHEDULE, getItemresponse.status);

            //Assert.AreEqual(payload.plan_number, getItemresponse., "plan_number should be match");
            Assert.AreEqual(payload.assignedTo, getItemresponse.assignedTo, "assignedTo should be match");
            Assert.AreEqual(payload.Module_Type_id, getItemresponse.Module_Type_id, "Module_Type_id should be match");
            Assert.AreEqual(payload.is_PTW.ToString(), getItemresponse.is_PTW, "is_PTW should be match");
            Assert.AreEqual(payload.max_score, getItemresponse.max_score, "max_score should be match");
            Assert.AreEqual("Bellary", getItemresponse.facility_name, "facility_name should be match");
            //Assert.AreEqual(payload.ApplyFrequency.ToString(), getItemresponse.FrequencyApplicable, "FrequencyApplicable should be match");
            Assert.AreEqual(payload.Checklist_id, getItemresponse.Checklist_id, "Checklist_id should be match");
            Assert.AreEqual(payload.Description, getItemresponse.Description, "Description should be match");
            //Assert.AreEqual(payload.Schedule_Date, getItemresponse.Schedule_Date, "Schedule_Date should be match");

            /*var actualEmployees = getItemresponse.Employees.Split(", ").ToList();
            Assert.AreEqual(payload.Employees[0], actualEmployees[0]);*/

        }


        [TestMethod]
        public void VerifyApproveAuditPlan()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 132,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_ApproveAuditPlan , jsonPayload);

            int Aud_id = Response.id[0];

            Assert.AreEqual(Response.message, ("Audit plan with plan number :  approved successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var getItemresponse = getItem.GetItem(EP_GetAuditPlanByID + "?id=" + Aud_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_APPROVED, getItemresponse.status);
            
        }



        [TestMethod]
        public void VerifyRejectAuditPlan()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 131,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_RejectAuditPlan, jsonPayload);

            int Aud_id = Response.id[0];

            Assert.AreEqual(Response.message, ("Audit plan with plan number :  rejected successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var getItemresponse = getItem.GetItem(EP_GetAuditPlanByID + "?id=" + Aud_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_REJECTED, getItemresponse.status);
            
        }









        [TestMethod]
        public void VerifyFunctionalTestAuditPlan()
        {
            int facility_id = 1;


            // Step 1: Create an Audit Plan
            var createPayload = new CMMSAPIs.Models.Audit.CMCreateAuditPlan
            {
                plan_number = "assedrftg",
                assignedTo = "Admin HFE",
                Module_Type_id = 3,
                id = 0,
                is_PTW = false,
                max_score = 0,
                auditor_id = 1,
                auditee_id = 1,
                Facility_id = 1,
                ApplyFrequency = 1,
                Checklist_id = 136,
                Description = "no desc",
                Schedule_Date = DateTime.Parse("2024-10-08"),
                map_checklist = new List<CMEvaluationAudit>
                {
                    new CMEvaluationAudit()
                },
                Employees = new List<string>
            { 
                "Pradeep Tholety",
                "Majid Shaikh"
            },
            };

            string createJsonPayload = JsonConvert.SerializeObject(createPayload);

            var auditService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var createResponse = auditService.CreateItem(EP_CreateAuditPlan, createJsonPayload);

            int auditPlanId = createResponse.id[0];

            Assert.AreEqual(createResponse.message, "Audit plan with plan number :  created successfully.", "message should be match");

            var CreatedgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var CreatedgetItemResponse = CreatedgetItem.GetItem(EP_GetAuditPlanByID + "?id=" + auditPlanId + "&facility_id=" + facility_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SCHEDULE, CreatedgetItemResponse.status);




            // Step 2: Reject the Audit Plan
            var rejectPayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = auditPlanId,
                comment = "test rejection"
            };

            string rejectJsonPayload = JsonConvert.SerializeObject(rejectPayload);
            var rejectResponse = auditService.CreateItem(EP_RejectAuditPlan, rejectJsonPayload);

            Assert.AreEqual(rejectResponse.message, "Audit plan with plan number :  rejected successfully.");

            var RejectedgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var RejectedgetItemresponse = RejectedgetItem.GetItem(EP_GetAuditPlanByID + "?id=" + auditPlanId + "&facility_id=" + facility_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_REJECTED, RejectedgetItemresponse.status);





            // Step 3: Update the Audit Plan
            var updatePayload = new CMMSAPIs.Models.Audit.CMCreateAuditPlan
            {
                plan_number = "assedrftg",
                assignedTo = "Admin HFE",
                Module_Type_id = 3,
                id = auditPlanId,
                is_PTW = false,
                max_score = 0,
                auditor_id = 1,
                auditee_id = 1,
                Facility_id = 1,
                ApplyFrequency = 1,
                Checklist_id = 136,
                Description = "updated Descrption",
                Schedule_Date = DateTime.Parse("2024-10-10"),
                map_checklist = new List<CMEvaluationAudit>
                {
                    new CMEvaluationAudit()
                },
                Employees = new List<string>
            {
                "Pradeep Tholety",
                "Majid Shaikh"
            },
            };

            string updateJsonPayload = JsonConvert.SerializeObject(updatePayload);
            var updateResponse = auditService.CreateItem(EP_UpdateAuditPlan, updateJsonPayload);

            Assert.AreEqual(updateResponse.message, "Audit plan with plan number :  updated successfully.");

            var UpdatedgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var UpdatedgetItemresponse = UpdatedgetItem.GetItem(EP_GetAuditPlanByID + "?id=" + auditPlanId + "&facility_id=" + facility_id);

            //Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SCHEDULE, UpdatedgetItemresponse.status);




            // Step 4: Approve the Audit Plan
            var approvePayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = auditPlanId,
                comment = "test approval"
            };

            string approveJsonPayload = JsonConvert.SerializeObject(approvePayload);
            var approveResponse = auditService.CreateItem(EP_ApproveAuditPlan, approveJsonPayload);

            Assert.AreEqual(approveResponse.message, "Audit plan with plan number :  approved successfully.");

            var ApprovedgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Audit.CMAuditPlanList>();
            var ApprovedgetItemResponse = ApprovedgetItem.GetItem(EP_GetAuditPlanByID + "?id=" + auditPlanId + "&facility_id=" + facility_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_APPROVED, ApprovedgetItemResponse.status);
           
        }










        [TestMethod]
        public void VerifyCreateAuditSkip()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2778,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_CreateAuditSkip, jsonPayload);

            int task_id = Response.id[0];

            Assert.AreEqual(Response.message, ($"Audit plan with plan task id : {task_id} skipped successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SKIP, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.skip_by_name);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.skip_date;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The skip timestamp should match.");

        }



        [TestMethod]
        public void VerifyApproveAuditSkip()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2778,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_ApproveAuditSkip, jsonPayload);

            int task_id = Response.id[0];

            Assert.AreEqual(Response.message, ($"Audit plan with plan task :  skip approved successfully."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SKIP_APPROVED, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.skip_approved_by_name);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.skip_approved_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The skip approved timestamp should match.");

        }


        [TestMethod]
        public void VerifyRejectAuditSkip()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2778,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_RejectAuditSkip, jsonPayload);

            int task_id = Response.id[0];

            Assert.AreEqual(Response.message, ($"Audit plan with task id : {task_id} skip rejected."));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SKIP_REJECT, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.skip_rejected_by_name);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.skip_approved_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour,0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour,0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The skip reject timestamp should match.");

        }





        [TestMethod]
        public void VerifyStartAuditTask()
        {
            int task_id = 2778;
            var payload = "null";

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_StartAuditTask + "?task_id=" + task_id, payload);

            //int task_id1 = Response.id[0];

            Assert.AreEqual(Response.message, ($"Execution AuditTask{task_id} Started Successfully"));

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_START, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.started_by_name);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.started_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day,0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The task start timestamp should match.");

        }



        [TestMethod]
        public void VerifyCloseAuditPlan()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2777,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_CloseAuditPlan, jsonPayload);

            int task_id = Response.id[0];

            //Assert.AreEqual(Response.message, $"Audit task with id : {task_id} closed successfully.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_START, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.started_by_name);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.started_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The task close timestamp should match.");

        }


        [TestMethod]
        public void VerifyApproveClosedAuditPlan()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2775,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(EP_ApproveClosedAuditPlan, jsonPayload);

            int task_id = Response.id[0];

            //Assert.AreEqual(Response.message, $"Audit plan with task id : {task_id} close approved successfully.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_CLOSED_APPROVED, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.closedApprovedByName);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.approved_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The task close approve timestamp should match.");

        }



        [TestMethod]
        public void VerifyRejectCloseAuditPlan()
        {
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2775,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var AuditService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = AuditService.CreateItem(EP_RejectCloseAuditPlan, jsonPayload);

            int task_id = Response.id[0];

            Assert.AreEqual(Response.message, $"Audit plan with plan number :  close rejected.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_CLOSED_REJECT, getItemresponse.status);

            Assert.AreEqual("Admin HFE", getItemresponse.closeRejectedbyName);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.rejected_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The task close approve timestamp should match.");

        }


        [TestMethod]
        public void VerifyExecuteAuditSchedule()
        {
            var payload = new CMPMExecutionDetail
            {
                task_id = 2803,
                comment = "",
                schedules = new List<CMPMScheduleObservation>
            {
                new CMPMScheduleObservation
                {
                    schedule_id = 995,
                    add_observations = new List<AddObservation>
                    {
                        new AddObservation
                        {
                            execution_id = 1999,
                            observation = "",
                            job_create = 0,
                            is_job_deleted = 0,
                            cp_ok = 0,
                            text = "null",
                            pm_files = new List<PMFileUpload>()
                        },
                        new AddObservation
                        {
                            execution_id = 2000,
                            observation = "",
                            job_create = 0,
                            is_job_deleted = 0,
                            cp_ok = 0,
                            text = "null",
                            pm_files = new List<PMFileUpload>()
                        },
                        new AddObservation
                        {
                            execution_id = 2001,
                            observation = "",
                            job_create = 0,
                            is_job_deleted = 0,
                            cp_ok = 0,
                            text = "null",
                            pm_files = new List<PMFileUpload>()
                        },
                        new AddObservation
                        {
                            execution_id = 2002,
                            observation = "",
                            job_create = 0,
                            is_job_deleted = 0,
                            cp_ok = 0,
                            text = "null",
                            pm_files = new List<PMFileUpload>()
                        },
                        new AddObservation
                        {
                            execution_id = 2003,
                            observation = "",
                            job_create = 0,
                            is_job_deleted = 0,
                            cp_ok = 0,
                            text = "null",
                            pm_files = new List<PMFileUpload>()
                        },
                        new AddObservation
                        {
                            execution_id = 2004,
                            observation = "",
                            job_create = 0,
                            is_job_deleted = 0,
                            cp_ok = 0,
                            text = "null",
                            pm_files = new List<PMFileUpload>()
                        }
                    }
                }
            }
            };


            string jsonPayload = JsonConvert.SerializeObject(payload);

            var ExecuteAuditScheduleService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = ExecuteAuditScheduleService.CreateItem(EP_ExecuteAuditSchedule, jsonPayload);

            int task_id = Response.task_id;

            //Assert.AreEqual(Response.message, $"Audit plan with plan number :  close rejected.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_EXECUTED, getItemresponse.status);

        }



        [TestMethod]
        public void VerifyAssignAuditTask()
        {

            int task_id = 2796;
            int assign_to = 37;
            var payload = "null";
            var AssignAuditTaskService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = AssignAuditTaskService.PutService(EP_AssignAuditTask + "?task_id=" + task_id + "&assign_to=" + assign_to , payload);

            //int task_id = Response.id[0];

            Assert.AreEqual(Response[0].message, $"Audit Task Assigned To user Id {assign_to}");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var getItemresponse = getItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_APPROVED, getItemresponse.status);

            /*Assert.AreEqual("Admin HFE", getItemresponse.status_updated_by_name);

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)getItemresponse.rejected_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The task close approve timestamp should match.");*/

        }


        [TestMethod]
        public void VerifyFunctionalTestAuditTask()
        {


            //STEP 1: Audit Skip
            var AuditSkippayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 2796,
                comment = "test"
            };

            string AuditSkipjsonPayload = JsonConvert.SerializeObject(AuditSkippayload);

            var AuditSkipService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var AuditSkipResponse = AuditSkipService.CreateItem(EP_CreateAuditSkip, AuditSkipjsonPayload);

            int task_id = AuditSkipResponse.id[0];

            Assert.AreEqual(AuditSkipResponse.message, ($"Audit plan with plan task id : {task_id} skipped successfully."));

            var AuditSkipgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var AuditSkipgetItemresponse = AuditSkipgetItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SKIP, AuditSkipgetItemresponse.status);






            //STEP 2: Reject Audit Skip
            var Rejectpayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = task_id,
                comment = "test"
            };

            string RejectjsonPayload = JsonConvert.SerializeObject(Rejectpayload);

            var RejectService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var RejectResponse = RejectService.CreateItem(EP_RejectAuditSkip, RejectjsonPayload);

            Assert.AreEqual(RejectResponse.message, ($"Audit plan with task id : {task_id} skip rejected."));

            var RejectgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var RejectgetItemresponse = RejectgetItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_SKIP_REJECT, RejectgetItemresponse.status);






            //STEP 3: Start Audit Task
            var Startpayload = "null";

            var StartService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var StartResponse = StartService.CreateItem(EP_StartAuditTask + "?task_id=" + task_id, Startpayload);

            Assert.AreEqual(StartResponse.message, ($"Execution AuditTask{task_id} Started Successfully"));

            var StartgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var StartgetItemresponse = StartgetItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_START, StartgetItemresponse.status);






            //STEP 4: Close Audit Task
            var Closepayload = new CMMSAPIs.Models.Utils.CMApproval        //issue in this 
            {
                id = task_id,
                comment = "test"
            };

            string ClosejsonPayload = JsonConvert.SerializeObject(Closepayload);

            var CloseService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var CloseResponse = CloseService.CreateItem(EP_CloseAuditPlan, ClosejsonPayload);   

            //Assert.AreEqual(Response.message, $"Audit task with id : {task_id} closed successfully.");

            var ClosegetItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var ClosegetItemresponse = ClosegetItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_CLOSED, ClosegetItemresponse.status);




            //STEP 5: Close Reject Task
            var CloseRejectpayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = task_id,
                comment = "test"
            };

            string CloseRejectjsonPayload = JsonConvert.SerializeObject(CloseRejectpayload);

            var CloseRejectService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var CloseRejectResponse = CloseRejectService.CreateItem(EP_RejectCloseAuditPlan, CloseRejectjsonPayload);

            Assert.AreEqual(CloseRejectResponse.message, $"Audit plan with plan number :  close rejected.");

            var CloseRejectgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>();
            var CloseRejectgetItemresponse = CloseRejectgetItem.GetItem(EP_GetTaskDetail + "?task_id=" + task_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.AUDIT_CLOSED_REJECT, CloseRejectgetItemresponse.status);
        }

    }
}
