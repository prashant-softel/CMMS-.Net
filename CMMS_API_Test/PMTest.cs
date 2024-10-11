using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Permits;

namespace CMMS_API_Test
{
    [TestClass]
    public class PMTest
    {
        string EP_CreatePMPlan = "/api/PM/CreatePMPlan";
        string EP_GetPMPlanList = "/api/PM/GetPMPlanList";
        string EP_GetPMPlanDetail = "/api/PM/GetPMPlanDetail";
        string EP_ApprovePMPlan = "/api/PM/ApprovePMPlan";
        string EP_RejectPMPlan = "/api/PM/RejectPMPlan";
        string EP_UpdatePMPlan = "/api/PM/UpdatePMPlan";
        //string EP_DeletePMPlan = "/api/PM/DeletePMPlan";
        string EP_GetPermitDetails = "/api/Permit/GetPermitDetails";


        [TestMethod]
        public void VerifyGetPMPlanList()
        {
            int facility_id = 17;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanList>(true);
            var response = pmService.GetItemList(EP_GetPMPlanList + "?facility_id=" + facility_id);
            Assert.IsNotNull(response);

        }


        [TestMethod]
        public void VerifygetPMPlanDetails()
        {
            var planId = 189;
            var facility_id = 1;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var response = mrsService.GetItem(EP_GetPMPlanDetail + "?planId=" + planId + "&facility_id=" + facility_id);
            Assert.IsNotNull(response);

        }

        [TestMethod]
        public void VerifyCreatePMPlan()
        {

            string payload = @"{
                                 ""plan_name"": ""plan flow test"",
                                 ""plan_date"": ""2024-10-02"",
                                 ""facility_id"": 17,
                                 ""category_id"": 8,
                                 ""plan_freq_id"": 4,
                                 ""plan_id"": 0,
                                 ""assigned_to_id"": 194,
                                 ""mapAssetChecklist"": [
                                     {
                                         ""id"": 2533,
                                         ""parent_name"": """",
                                         ""module_qty"": 0,
                                         ""checklist_id"": 39,
                                         ""checklist_name"": """",
                                         ""asset_name"": """",
                                         ""parent_id"": 0
                                     },
                                     {
                                         ""id"": 2532,
                                         ""parent_name"": """",
                                         ""module_qty"": 0,
                                         ""checklist_id"": 39,
                                         ""checklist_name"": """",
                                         ""asset_name"": """",
                                         ""parent_id"": 0
                                     }
                                 ]
                            }";


            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = pmPlanService.CreateItem(EP_CreatePMPlan, payload);

            string expectedMessage = "Plan added successfully";
            Assert.AreEqual(expectedMessage, response.message, "The PM Plan creation message is not as expected.");

            int planId = response.id[0];
            Assert.IsTrue(planId > 0, "The PM Plan ID should be greater than 0.");

            var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var planResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);

            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_CREATED, planResponse.status_id);

            var expectedCMMRS = JsonConvert.DeserializeObject<CMPMPlanDetail>(payload);

            // Verify the plan details
            Assert.AreEqual(expectedCMMRS.plan_name, planResponse.plan_name, "Plan name mismatch.");
            Assert.AreEqual(expectedCMMRS.plan_date, planResponse.plan_date, "Plan date mismatch."); 
            Assert.AreEqual(expectedCMMRS.facility_id, planResponse.facility_id, "Facility ID mismatch.");
            Assert.AreEqual(expectedCMMRS.category_id, planResponse.category_id, "Category ID mismatch.");
            Assert.AreEqual(expectedCMMRS.plan_freq_id, planResponse.plan_freq_id, "Plan frequency ID mismatch.");
            //Assert.AreEqual(194, planResponse.assigned_to_id, "Assigned to ID mismatch.");


            Assert.AreEqual(expectedCMMRS.mapAssetChecklist[0].id, planResponse.mapAssetChecklist[0].id, "First Asset ID mismatch.");
            //Assert.AreEqual(expectedCMMRS.mapAssetChecklist[0].module_qty, planResponse.mapAssetChecklist[0].module_qty, "First Asset Module Qty mismatch.");
            Assert.AreEqual(expectedCMMRS.mapAssetChecklist[0].checklist_id, planResponse.mapAssetChecklist[0].checklist_id, "First Asset Checklist ID mismatch.");

            Assert.AreEqual(expectedCMMRS.mapAssetChecklist[1].id, planResponse.mapAssetChecklist[1].id, "Second Asset ID mismatch.");
            //Assert.AreEqual(expectedCMMRS.mapAssetChecklist[1].module_qty, planResponse.mapAssetChecklist[1].module_qty, "Second Asset Module Qty mismatch.");
            Assert.AreEqual(expectedCMMRS.mapAssetChecklist[1].checklist_id, planResponse.mapAssetChecklist[1].checklist_id, "Second Asset Checklist ID mismatch.");
        }


        [TestMethod]
        public void VerifyUpdatePMPlan()
        {

            string payload = @"{
                                 ""plan_name"": ""plan flow test"",
                                 ""plan_date"": ""2024-10-02"",
                                 ""facility_id"": 17,
                                 ""category_id"": 8,
                                 ""plan_freq_id"": 4,
                                 ""plan_id"": 117,
                                 ""assigned_to_id"": 194,
                                 ""mapAssetChecklist"": [
                                     {
                                         ""id"": 2533,
                                         ""parent_name"": """",
                                         ""module_qty"": 0,
                                         ""checklist_id"": 39,
                                         ""checklist_name"": """",
                                         ""asset_name"": """",
                                         ""parent_id"": 0
                                     }
                                    
                                 ]
                            }";


            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = pmPlanService.CreateItem(EP_UpdatePMPlan, payload);

            string expectedMessage = "Plan Updated Successfully ";
            Assert.AreEqual(expectedMessage, response.message, "The PM Plan creation message is not as expected.");

            int planId = response.id[0];
            Assert.IsTrue(planId > 0, "The PM Plan ID should be greater than 0.");

            var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var planResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);

            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_CREATED, planResponse.status_id);

            Assert.AreEqual("plan flow test", planResponse.plan_name, "Plan name mismatch.");
            Assert.AreEqual("2024-10-02", planResponse.plan_date.ToString("yyyy-MM-dd"), "Plan date mismatch.");
            Assert.AreEqual(17, planResponse.facility_id, "Facility ID mismatch.");
            Assert.AreEqual(8, planResponse.category_id, "Category ID mismatch.");
            Assert.AreEqual(4, planResponse.plan_freq_id, "Plan frequency ID mismatch.");

            Assert.AreEqual(1, planResponse.mapAssetChecklist.Count, "Asset checklist count mismatch.");
        }



        [TestMethod]
        public void VerifyApprovePMPlan()
        {
            string payload = @"{
                                ""ID"":2525,
                                ""comment"": ""pm plan Approval""
                         
                               }";
            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = pmPlanService.PutService(EP_ApprovePMPlan, payload);
            string responseMessage = response[0].message;
            Assert.AreEqual("PM Plan Approved Successfully", responseMessage);

            int planId = response[0].id[0];
            Assert.IsTrue(planId > 0, "The PM Plan ID should be greater than 0.");

            var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var planResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);

            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_APPROVED, planResponse.status_id);

            Assert.AreEqual(planResponse.approved_by_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            /*DateTime actualGeneratedAt = (DateTime)planResponse.approved_at;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS approved timestamp should match.");*/

            Assert.AreEqual(planId, 2525);
        }


        [TestMethod]
        public void VerifyRejectPMPlan()
        {
            string payload = @"{
                                ""ID"":2525,
                                ""comment"": ""pm plan Approval""
                         
                               }";
            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = pmPlanService.PutService(EP_RejectPMPlan, payload);
            string responseMessage = response[0].message;
            Assert.AreEqual("PM Plan Rejected Successfully", responseMessage);

            int planId = response[0].id[0];
            Assert.IsTrue(planId > 0, "The PM Plan ID should be greater than 0.");

            var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var planResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);

            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_REJECTED, planResponse.status_id);

            /*Assert.AreEqual(mrsResponse.approver_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.approval_date;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS approved timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);*/
        }


        /*[TestMethod]
        public void VerifyDeletePMPlan()
        {
            string payload = @"{
                                ""ID"":25,
                                ""comment"": ""pm plan Approval""
                         
                               }";
            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = pmPlanService.PutService(EP_DeletePMPlan, payload);
            string responseMessage = response[0].message;
            Assert.AreEqual(" PM Plan Deleted", responseMessage);

            int planId = response[0].id[0];
            Assert.IsTrue(planId > 0, "The PM Plan ID should be greater than 0.");

            var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var planResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);

            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_DELETED, planResponse.status_id);

            *//*Assert.AreEqual(mrsResponse.approver_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.approval_date;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS approved timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);*//*
        }*/



        [TestMethod]
        public void VerifyScheduleData()
        {
            int facilityId = 1;
            int categoryId = 2;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMScheduleData>(true);
            var response = pmService.GetItemList($"/api/PM/GetScheduleData?facility_id={facilityId}&category_id={categoryId}");
            Assert.IsNotNull(response);
            Assert.AreEqual(16, response.Count);
            /*Assert.AreEqual(123111, response[0].asset_id);
            Assert.AreEqual("RISEN_WIND_BLOCK_1_INV_1", response[0].asset_name);
            Assert.IsNotNull(response[0].frequency_dates);
            Assert.AreNotEqual(0, response[0].frequency_dates.Count);*/
        }
        [TestMethod]
        public void VerifyPMTaskList()
        {
            int facilityId = 1736;
            List<int> categoryIds = new List<int>() { 2 };
            List<int> frequencyIds = new List<int>() { 4 };
            string startDate = "2023-07-01";
            string endDate = "2023-07-31";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleView>(true);
            var response = pmService.GetItemList($"/api/PMScheduleView/GetPMTaskList?facility_id={facilityId}&start_date={startDate}&end_date={endDate}&categoryIds={string.Join(',', categoryIds)}&frequencyIds={string.Join(',', frequencyIds)}");
            Assert.IsNotNull(response);

        }
        [TestMethod]
        public void VerifyGetPMTaskDetail()
        {

            int task_id = 143;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>(true);

            var response = pmService.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={task_id}");
            Assert.IsNotNull(response);
            Assert.AreEqual("PMTASK143", response.task_code);
            Assert.AreEqual("Monthly", response.frequency_name);
        }
        [TestMethod]
        public void SetScheduleData()
        {
            int facility_id = 1736;
            int task_id = 4;
            int schedule_id = 123123;
            //int frequency_id = 7;
            DateTime scheduleDate = new DateTime(2023, 7, 7);
            string payload = $"{{\n" +
                                $"\t\"facility_id\": {facility_id}\n," +
                                $"\t\"asset_schedules\": [\n" +
                                $"\t\t{{\n" +
                                //$"\t\t\t\"asset_id\": {asset_id},\n" +
                                $"\t\t\t\"frequency_dates\": [\n" +
                                $"\t\t\t\t{{\n" +
                                //$"\t\t\t\t\t\"frequency_id\": {frequency_id},\n" +
                                $"\t\t\t\t\t\"schedule_date\": \"{scheduleDate.ToString("yyyy-MM-dd")}\"\n" +
                                $"\t\t\t\t}}\n" +
                                $"\t\t\t]\n" +
                                $"\t\t}}\n" +
                                $"\t]\n" +
                                $"}}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.CreateItems("/api/PM/SetScheduleData", payload);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            //Assert.AreEqual(asset_id, response2.equipment_id);
            //Assert.AreEqual(frequency_id, response2.frequency_id);
            Assert.AreEqual(scheduleDate, response2.schedule_date);
            //Assert.AreEqual(frequency_id, response2.frequency_id);
        }
        [TestMethod]
        public void VerifyCancelPMTask()
        {
            string payload = "{\n\t\"id\" : 53,\n\t\"comment\" : \"Cancelled\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/CancelPMTask", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            /*Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CANCELLED, response2.status);*/
        }


        [TestMethod]
        public void VerifyCancelApprovedPMTaskExecution()
        {
            string payload = @"{
                                ""ID"":338,
                                ""comment"": ""Cancelled Approved""
                         
                               }";

            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);

            var response = pmService.PutService("/api/PMScheduleView/CancelApprovedPMTaskExecution", payload);
            Assert.IsNotNull(response[0]);

            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            string responseMessage = response[0].message;
            Assert.AreEqual("PM Task Approved  successfully.", responseMessage);
        }

        [TestMethod]
        public void VerifyCancelRejectedPMTaskExecution()
        {
            string payload = @"{
                                ""ID"":335,
                                ""comment"": ""Cancelled Rejected""
                         
                               }";

            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);

            var response = pmService.PutService("/api/PMScheduleView/CancelRejectedPMTaskExecution", payload);
            Assert.IsNotNull(response[0]);

            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            string responseMessage = response[0].message;
            Assert.AreEqual("PM Task Cancel Rejected successfully.", responseMessage);
        }

        [TestMethod]
        public void VerifyLinkToPTW()
        {
            string payload = @"
                            {
                                ""facility_id"" : 1736,
                                ""blockId"": 1738,
                                ""lotoId"": 3,
                                ""start_datetime"":""2023-07-15T08:00:00Z"",
                                ""start_datetime"":""2023-07-15T16:00:00Z"",
                                ""title"":""PM5334"",
                                ""description"":""PM5334"",
                                ""job_type_id"":12,
                                ""typeId"":29,
                                ""sop_type_id"":28,  
                                ""issuer_id"":1,
                                ""approver_id"":5,
                                ""category_ids"":[123111,123112],
                                ""is_isolation_required"":true,
                                ""isolated_category_ids"":[2,4,30],
                                ""Loto_list"":[
                                    {
                                        ""Loto_id"":123111,
                                        ""Loto_Key"":""lototest1""
                                    },
                                    {
                                        ""Loto_id"":123112,
                                        ""Loto_Key"":""lototest2""
                                    }
                                ],
                                ""employee_list"":[
                                    {
                                       ""employeeId"":16,
                                       ""responsibility"":""check""
                                    },
                                    {
                                       ""employeeId"":64,
                                       ""responsibility"":""testing""
                                    }       
                                ],
                                ""safety_question_list"":[
                                    {
                                        ""safetyMeasureId"":255,
                                        ""safetyMeasureValue"":""Yes""
                                    },
                                    {
                                        ""safetyMeasureId"":258,
                                        ""safetyMeasureValue"":""on""
                                    }
                                ]
                            }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.CreateItem("/api/Permit/CreatePermit", payload);

            int ptw_id = response.id[0];

            int task_id = 329;

            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response2 = pmService.PutService($"/api/PMScheduleView/LinkPermitToPMTask?task_id={task_id}&permit_id={ptw_id}", "");
            Assert.IsNotNull(response2[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response2[0].return_status);

            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response3 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={task_id}");

            Assert.IsNotNull(response3);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_LINKED_TO_PTW, response3.status);
            Assert.AreEqual(ptw_id, response3.permit_id);
        }

        [TestMethod]
        public void VerifyGetPermitDetails()
        {
            int id = 156;
            var permitService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>();
            var response = permitService.GetItem(EP_GetPermitDetails + "?permit_id=" + id);

            int myNewItemId = response.approver_id;
            Assert.AreEqual(myNewItemId, id);
            Assert.IsNotNull(response, "GO details should not be null.");
        }

        [TestMethod]
        public void VerifyPermitApprove()
        {
            string payload = @"{
                                ""ID"":25,
                                ""comment"": ""pm plan Approval"",
                                ""jobId"": 0,
                                ""permitId"": null,
                                ""ptwStatus"":""121""
                         
                               }";
            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = pmPlanService.PutService("/api/Permit/PermitApprove", payload);
            string responseMessage = response[0].message;
            int permit_id = response[0].id[0];
            Assert.AreEqual($" Permit  {permit_id}  Approved", responseMessage);

            
            Assert.IsTrue(permit_id > 0, "The Permit ID should be greater than 0.");

            /*var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>();
            var planResponse = getPlanItem.GetItem($"/api/permit/GetPermitDetails?permit_id={permit_id}");

            Assert.AreEqual((int)CMMS.CMMS_Status.PTW_APPROVED, planResponse.ptwStatus);

            Assert.AreEqual(planResponse.approvedByName, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)planResponse.approve_at;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS approved timestamp should match.");

            Assert.AreEqual(permit_id, 25);*/
        }


        [TestMethod]
        public void SetPMTask()
        {
            int pm_schedule_id = 84;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PostService($"/api/PMScheduleView/SetPMTask?task_id={pm_schedule_id}", "");
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_START, response2.status);
            Assert.IsNotNull(response2.schedule_check_points);
            Assert.AreNotEqual(0, response2.schedule_check_points.Count);
        }
        [TestMethod]
        public void AddCustomCheckpoint()
        {
            int pm_schedule_id = 76;
            string payload = @$"{{
                                    ""schedule_id"": {pm_schedule_id},
                                    ""check_point_name"": ""Check all switches"",
                                    ""requirement"": ""All switches should work smoothly.""
                                }}";

            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>(true);
            var response = pmService.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.task_code);
            Assert.AreEqual("SMB", response.category_name);
            //int expectedCPs = response.schedule_check_points.Count + 1;
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response2 = pmService2.PostService("/api/PMScheduleView/AddCustomCheckpoint", payload);
            Assert.IsNotNull(response2[0]);
            var pmService3 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>(true);
            var response3 = pmService3.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            Assert.IsNotNull(response3);
            Assert.IsNotNull(response3);
            Assert.AreEqual("SMB", response3.categoryName);
        }
        [TestMethod]
        public void ClosePMTaskExecution()
        {
            string payload = "{\n\t\"id\" : 34, \n\t\"comment\" : \"Closed\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/ClosePMTaskExecution", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CLOSED, response2.status);
        }
        [TestMethod]
        public void RejectPMTaskExecution()
        {
            string payload = "{\n\t\"id\" : 34, \n\t\"comment\" : \"Not OK\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/RejectPMTaskExecution", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CLOSE_REJECTED, response2.status);
        }
        [TestMethod]
        public void ApprovePMTaskExecution()
        {
            string payload = "{\n\t\"id\" : 127, \n\t\"comment\" : \"OK\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/ApprovePMTaskExecution", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CLOSE_APPROVED, response2.status);
        }

        [TestMethod]
        public void UpdatePMTaskExecution()
        {
            int pm_schedule_id = 5334;
            List<string> fileList = new List<string>()
            {
                @"C:\Users\lenovo\OneDrive\Pictures\Saved Pictures\download.jpeg",
                @"C:\Users\lenovo\OneDrive\Pictures\unnamed.jpg",
                @"C:\Users\lenovo\OneDrive\Pictures\Saved Pictures\Harmony Dark.jpeg"
            };
            Dictionary<string, Tuple<bool, List<string>>> fileUpload = new Dictionary<string, Tuple<bool, List<string>>>()
            {
                { "files", new Tuple<bool, List<string>>(true, fileList) },
                { "facility_id", new Tuple<bool, List<string>>(false, new List<string> { "1736" }) },
                { "module_type", new Tuple<bool, List<string>>(false, new List<string> { "JOB" }) },
                { "module_ref_id", new Tuple<bool, List<string>>(false, new List<string> { $"{pm_schedule_id}" }) },
                { "file_category", new Tuple<bool, List<string>>(false, new List<string> { "12" }) },
            };
            var fileService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var fileResponse = fileService.FormPostService($"/api/FileUpload/UploadFile", fileUpload);
            Assert.IsNotNull(fileResponse);
            Assert.AreEqual(fileList.Count, fileResponse[0].id.Count);

            string payload = @$"
                                {{
                                    ""schedule_id"": {pm_schedule_id},
                                    ""add_observations"": [
                                        {{
                                            ""execution_id"": 10734,
                                            ""isOK"" : 1,
                                            ""observation"": ""OK"",
                                            ""job_create"":0,
                                            ""pm_files"": []
                                        }},
                                        {{
                                            ""execution_id"": 10735,
                                            ""observation"": ""Inverter is very noisy. Fan needs to be dusted."",
                                            ""job_create"": 1,
                                            ""pm_files"": [
                                                {{
                                                    ""file_id"": {fileResponse[0].id[0]},
                                                    ""file_desc"": ""Before"",
                                                    ""pm_event"": 1
                                                }},
                                                {{
                                                    ""file_id"": {fileResponse[0].id[1]},
                                                    ""file_desc"": ""During"",
                                                    ""pm_event"": 2
                                                }},
                                                {{
                                                    ""file_id"": {fileResponse[0].id[2]},
                                                    ""file_desc"": ""After"",
                                                    ""pm_event"": 3
                                                }}
                                            ]
                                        }}
                                    ]
                                }}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PatchServiceList("/api/PMScheduleView/UpdatePMTaskExecution", payload);
            Assert.IsNotNull(response);

            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.IsNotNull(response2.schedule_check_points);
            Assert.AreNotEqual(0, response2.schedule_check_points.Count);
            Dictionary<dynamic, CMMSAPIs.Models.PM.ScheduleCheckList> checkpoints = response2.schedule_check_points.SetPrimaryKey("execution_id");
            Assert.AreEqual("OK", checkpoints[10734].observation);
            Assert.AreEqual(0, checkpoints[10734].linked_job_id);
            Assert.AreEqual(0, checkpoints[10734].files.Count);
            Assert.AreEqual("Inverter is very noisy. Fan needs to be dusted.", checkpoints[10735].observation);
            Assert.AreNotEqual(0, checkpoints[10735].linked_job_id);
            Assert.AreEqual(3, checkpoints[10735].files.Count);
            Dictionary<dynamic, CMMSAPIs.Models.PM.ScheduleFiles> files2 = checkpoints[10735].files.SetPrimaryKey("_event");
            Assert.AreNotEqual(0, response2.schedule_link_job.Count);
        }








        [TestMethod]
        public void VerifyPMPlanFunctionalTest()
        {
            // Step 1: Create the PM Plan
            var planDetail = new CMMSAPIs.Models.PM.CMPMPlanDetail
            {
                plan_name = "plan flow test",
                plan_date = DateTime.Parse("2024-10-02"),
                facility_id = 17,
                category_id = 8,
                plan_freq_id = 4,
                plan_id = 0,
                assigned_to_id = 194,
                mapAssetChecklist = new List<AssetCheckList>
            {
            new CMMSAPIs.Models.PM.AssetCheckList
            {
                id = 2533,
                parentName = "",
                module_qty = 0,
                checklist_id = 39,
                checklist_name = "",
                //asset_name = "",
                parentId = 0
            }
            }
            };

            string jsonPayload = JsonConvert.SerializeObject(planDetail);


            var pmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var createResponse = pmPlanService.CreateItem(EP_CreatePMPlan, jsonPayload);

            string expectedCreateMessage = "Plan added successfully";
            Assert.AreEqual(expectedCreateMessage, createResponse.message, "The PM Plan creation message is not as expected.");

            int planId = createResponse.id[0];

            Assert.IsTrue(planId > 0, "The PM Plan ID should be greater than 0.");


            // Step 2: Reject the PM Plan
            string rejectPayload = $@"{{
                                ""ID"":{planId},
                                ""comment"": ""Rejecting PM plan""
                             }}";

            var rejectResponse = pmPlanService.PutService(EP_RejectPMPlan, rejectPayload);
            string rejectMessage = rejectResponse[0].message;
            Assert.AreEqual("PM Plan Rejected Successfully", rejectMessage, "The PM Plan rejection message is not as expected.");

            var getPlanItem = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMPlanDetail>();
            var rejectPlanResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_REJECTED, rejectPlanResponse.status_id, "The PM Plan should be in 'rejected' status.");



            // Step 3: Update the PM Plan
            var updatePayload = new CMMSAPIs.Models.PM.CMPMPlanDetail
            {
                plan_name = "plan flow test 1",
                plan_date = DateTime.Parse("2024-10-02"),
                facility_id = 17,
                category_id = 8,
                plan_freq_id = 4,
                plan_id = planId,
                assigned_to_id = 194,
                mapAssetChecklist = new List<AssetCheckList>
            {
            new CMMSAPIs.Models.PM.AssetCheckList
            {
                id = 2533,
                parentName = "",
                module_qty = 0,
                checklist_id = 39,
                checklist_name = "",
                //asset_name = "",
                parentId = 0
            }
            }
            };
            string jsonPayload1 = JsonConvert.SerializeObject(updatePayload);
            var updatepmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var updateResponse = updatepmPlanService.CreateItem(EP_UpdatePMPlan, jsonPayload1);
            string expectedUpdateMessage = "Plan Updated Successfully ";
            Assert.AreEqual(expectedUpdateMessage, updateResponse.message, "The PM Plan update message is not as expected.");

            var updatedPlanResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);
            Assert.AreEqual("plan flow test 1", updatedPlanResponse.plan_name, "The updated plan name is not as expected.");
            Assert.AreEqual("2024-10-02", updatedPlanResponse.plan_date.ToString("yyyy-MM-dd"), "The updated plan date is not as expected.");
            Assert.AreEqual(17, updatedPlanResponse.facility_id, "The updated facility ID is not as expected.");
            Assert.AreEqual(8, updatedPlanResponse.category_id, "The updated category ID is not as expected.");
            Assert.AreEqual(4, updatedPlanResponse.plan_freq_id, "The updated plan frequency ID is not as expected.");


            // Step 4: Approve the PM Plan
            string approvePayload = $@"{{
                                ""ID"":{planId},
                                ""comment"": ""Approving PM plan""
                             }}";
            var ApprovepmPlanService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var approveResponse = ApprovepmPlanService.PutService(EP_ApprovePMPlan, approvePayload);
            string approveMessage = approveResponse[0].message;
            Assert.AreEqual("PM Plan Approved Successfully", approveMessage, "The PM Plan approval message is not as expected.");

            var approvedPlanResponse = getPlanItem.GetItem(EP_GetPMPlanDetail + "?planId=" + planId);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_PLAN_APPROVED, approvedPlanResponse.status_id, "The PM Plan should be in 'approved' status.");

            var task_id = approveResponse[0].task_id;



            // Step 5: Create the Permit and linked it to task
            var createPermitPayload = new CMMSAPIs.Models.Permits.CMCreatePermit
            {
                facility_id = 1736,
                blockId = 1738,
                lotoId = 3,
                start_datetime = DateTime.Now,
                end_datetime = DateTime.Now,
                title = "PM5334",
                description = "PM5334",
                job_type_id = 12,
                permitTypeId = 29,
                sop_type_id = 28,
                issuer_id = 1,
                approver_id = 5,
                category_ids = new List<int> { 123111, 123112 },
                is_loto_required = true,
                isolated_category_ids = new List<int> { 2, 4, 30 },
                Loto_list = new List<CMMSAPIs.Models.Permits.CMLotoList>
            {
                new CMMSAPIs.Models.Permits.CMLotoList { Loto_id = 123111,  Loto_Key = "lototest1" },
                new CMMSAPIs.Models.Permits.CMLotoList { Loto_id = 123112,  Loto_Key = "lototest2" }
            },
                employee_list = new List<CMPermitEmpList>
            {
                new CMPermitEmpList { employeeId = 16, responsibility = "check" },
                new CMPermitEmpList { employeeId = 64, responsibility = "testing" }
            },
                safety_question_list = new List<CMPermitSaftyQueList>
            {
                new CMPermitSaftyQueList { safetyMeasureId = 255, safetyMeasureValue = "Yes" },
                new CMPermitSaftyQueList { safetyMeasureId = 258, safetyMeasureValue = "on" }
            }
            };


            // Serialize the object into JSON
            string CreatePermitpayloadjson = JsonConvert.SerializeObject(createPermitPayload);

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.CreateItem("/api/Permit/CreatePermit", CreatePermitpayloadjson);

            int ptw_id = response.id[0];
            
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response2 = pmService.PutService($"/api/PMScheduleView/LinkPermitToPMTask?task_id={task_id}&permit_id={ptw_id}", "");
            Assert.IsNotNull(response2[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response2[0].return_status);

            /*var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMTaskView>(true);
            var response3 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={task_id}");
            
            Assert.IsNotNull(response3);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_LINKED_TO_PTW, response3.status);
            Assert.AreEqual(ptw_id, response3.permit_id);*/




            //STEP 6: Cancel PM Task
            string cancelPMTaskpayload = $@"{{
                                       ""id"":{task_id},
                                       ""comment"": ""Cancelled""
                                   }}";

            var pmService1 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);

            var Cancelresponse = pmService1.PutService("/api/PMScheduleView/CancelPMTask", cancelPMTaskpayload);

            Assert.IsNotNull(Cancelresponse[0]);

            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, Cancelresponse[0].return_status);

            //var cancelreponse1 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?task_id={task_id}");
            //Assert.AreEqual((int)CMMS.CMMS_Status.PM_CANCELLED, cancelreponse1.status, "The PM task should be in 'cancelled' status.");





            //STEP 7: Cancelled Reject Task
            string rejectCancelpayload = $@"{{
                                ""ID"":{task_id},
                                ""comment"": ""Cancelled Rejected""
                               }}";

            var cancelRejectService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);

            var CancelRejectresponse = cancelRejectService.PutService("/api/PMScheduleView/CancelRejectedPMTaskExecution", rejectCancelpayload);
            Assert.IsNotNull(CancelRejectresponse[0]);

            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, CancelRejectresponse[0].return_status);

            //int cancel_id = CancelRejectresponse[0].id[0];
            string responseMessage = CancelRejectresponse[0].message;
            Assert.AreEqual("PM Task Cancel Rejected successfully.", responseMessage);





            //STEP 8: Reject Permit
            string rejectPermitpayload = $@"{{
                                ""ID"":{ptw_id},
                                ""comment"": ""pm plan Approval""
                               }}";
            var rejectPermitServuice = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var rejectpermitresponse = rejectPermitServuice.PutService("/api/Permit/PermitReject", rejectPermitpayload);
            string rppresponseMessage = rejectpermitresponse[0].message;
            int permit_id = rejectpermitresponse[0].id[0];
            Assert.AreEqual($" Permit  {permit_id} Rejected", rppresponseMessage);
            Assert.IsTrue(permit_id > 0, "The Permit ID should be greater than 0.");




            //STEP 9:Update permit
            var updatePermitPayload = new CMUpdatePermit
            {
                permit_id = ptw_id,
                comment = "test",
                approver_id = 0,
                issuer_id = 0,
                facility_id = 17,
                blockId = 19,
                latitude = 0,
                job_type_id = 0,
                lotoId = 8,
                typeId = 7,
                sop_type_id = 0,
                TBT_Done_By = 0,
                start_datetime = DateTime.Now,
                end_datetime = DateTime.Now,
                description = "test",
                physical_iso_remark = "test",
                //title = "plan flow test 1",
                is_isolation_required = false,
                resubmit = true,
                Loto_list = new List<CMPermitLotoList>(),
                LotoOtherDetails = new List<CMPermitLotoOtherList>(),
                employee_list = new List<CMPermitEmpList>(),
                safety_question_list = new List<CMPermitSaftyQueList>
            {
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 54,
                    safetyMeasureValue = "Crane / Slings is inspected before use",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 55,
                    safetyMeasureValue = "Operator is qualified & trained for the lifting job",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 56,
                    safetyMeasureValue = "Adequate PPEs (Hard hat, gloves, goggles etc.) have been provided",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 59,
                    safetyMeasureValue = "Outriggers fully extended & positioned on proper outrigger pads",
                    ischeck = 0
                }
            },
                block_ids = new List<int>(),
                isolated_category_ids = new List<int>(),
                category_ids = new List<int> { 8 },
                uploadfile_ids = new List<int>()
            };

            // Serialize the object into JSON
            string updatePermitPayloadjsonPayload = JsonConvert.SerializeObject(updatePermitPayload);

            var updatePermitServuice = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var updatePermitresponse = updatePermitServuice.PatchService($"/api/Permit/UpdatePermit?resubmit=true", updatePermitPayloadjsonPayload);

            string upPermitresponse = updatePermitresponse[0].message;

            Assert.AreEqual("Permit Resubmitted for Approval", upPermitresponse);
            Assert.IsTrue(permit_id > 0, "The Permit ID should be greater than 0.");




            //STEP 10: Approve Permit 
            string approvePermitpayload = $@"{{
                                ""ID"":{ptw_id},
                                ""comment"": ""pm plan Approval"",
                                ""jobId"": 0,
                                ""permitId"": null,
                                ""ptwStatus"":""121""
                         
                               }}";
            var approvepermitService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var approvepermitresponse = approvepermitService.PutService("/api/Permit/PermitApprove", approvePermitpayload);

            string appresponseMessage = approvepermitresponse[0].message;
            int ap_permit_id = approvepermitresponse[0].id[0];

            Assert.AreEqual($" Permit  {ptw_id}  Approved", appresponseMessage);
            Assert.IsTrue(ap_permit_id > 0, "The Permit ID should be greater than 0.");



            //STEP 11: Go for TBT
            var goForTBTPayload = new CMUpdatePermit
            {
                permit_id = ptw_id,
                comment = "",
                approver_id = 0,
                issuer_id = 0,
                facility_id = 17,
                blockId = 18,
                latitude = 0,
                job_type_id = 0,
                lotoId = 8,
                typeId = 2,
                sop_type_id = 0,
                start_datetime = DateTime.Now,
                end_datetime = DateTime.Now,
                TBT_Done_At = DateTime.Now,
                description = "ttse",
                //title = "plan flow test 1",
                //is_loto_required = false,
                resubmit = false,
                Loto_list = new List<CMPermitLotoList>(),
                LotoOtherDetails = new List<CMPermitLotoOtherList>(),
                employee_list = new List<CMPermitEmpList>
            {
                new CMPermitEmpList
                {
                    employeeId = 194,
                    responsibility = null
                },
                new CMPermitEmpList
                {
                    employeeId = 24,
                    responsibility = "Solar Site Engineer - Operator"
                }
            },
                        safety_question_list = new List<CMPermitSaftyQueList>
            {
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 5,
                    safetyMeasureValue = "Confined space - entry / exit logsheet is being maintained",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 6,
                    safetyMeasureValue = "Adequate PPEs (Respiratory mask, etc.) have been provided",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 7,
                    safetyMeasureValue = "No compressed cylinders are stored inside the confined space",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 8,
                    safetyMeasureValue = "First-aid facility & Emergency rescue plan are in place",
                    ischeck = 0
                },
                new CMPermitSaftyQueList
                {
                    safetyMeasureId = 9,
                    safetyMeasureValue = "Signages & Barriers are in place",
                    ischeck = 0
                }
            },
                block_ids = new List<int> { 194, 24 },
                isolated_category_ids = new List<int>(),
                category_ids = new List<int> { 8 },
                uploadfile_ids = new List<int>(),
                TBT_Done_By = 194,
                physical_iso_remark = "test",
                //is_physical_iso_required = false
            };

           
            string gofortbtjsonPayload = JsonConvert.SerializeObject(goForTBTPayload);
            var goForTBTService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var goForTBTresponse = goForTBTService.PatchService("/api/Permit/UpdatePermit?resubmit=false", gofortbtjsonPayload);

            Assert.IsNotNull(goForTBTresponse, "The API response should not be null.");
            Assert.AreEqual("Permit Updated Successfully with TBT ", goForTBTresponse[0].message, "The permit update message should be correct.");

            int permitId = goForTBTresponse[0].id[0];
            Assert.IsTrue(permitId > 0, "The Permit ID should be greater than 0.");



            //STEP 12: Start PM Task
            int facility_id = 17;
            dynamic payload = "null";
            var startPMService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var startPMresponse = startPMService.PostService($"/api/PMScheduleView/StartPMTask?task_id={task_id}&facility_id:{facility_id}", payload);

            Assert.IsNotNull(startPMresponse, "The API response should not be null.");
            Assert.AreEqual($"Execution PMTASK{task_id} Started Successfully", startPMresponse[0].message, "The permit start message should be match.");
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, startPMresponse[0].return_status);

            int startpm_id = startPMresponse[0].id[0];
            Assert.IsTrue(startpm_id > 0, "The Permit ID should be greater than 0.");



            //STEP 13: Execute PM Task Execution
            string ExecuteTaskPayload = $@"
                                {{
                                    ""task_id"": {task_id},
                                    ""comment"": ""test""
                                 }}";
            var executeTaskService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Utils.CMDefaultResponse>>();
            var executetaskresponse = executeTaskService.PatchService("/api/PMScheduleView/UpdatePMTaskExecution?facility_id=17", ExecuteTaskPayload);

            Assert.IsNotNull(executetaskresponse, "The API response should not be null.");
            Assert.AreEqual($"PM Task Updated successfully", executetaskresponse[0][0].message, "The permit update message should be correct.");
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, executetaskresponse[0][0].return_status);

            int exetask_id = executetaskresponse[0][0].id[0];
            Assert.IsTrue(exetask_id > 0, "The Permit ID should be greater than 0.");



            // STEP 14: Close PM Task Execution
            string CloseTaskPayload = $@"
                                {{
                                    ""id"": {task_id},
                                    ""facility_id"":17,
                                    ""comment"": ""test""
                              }}";
            var CloseTaskService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var closetaskresponse = CloseTaskService.PutService("/api/PMScheduleView/ClosePMTaskExecution", CloseTaskPayload);
            Assert.IsNotNull(closetaskresponse, "The API response should not be null.");
            Assert.AreEqual($"PM Task Close Requested successfully", closetaskresponse[0].message, "The permit update message should be correct.");
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, closetaskresponse[0].return_status);
            int closetask_id = closetaskresponse[0].id[0];
            Assert.IsTrue(closetask_id > 0, "The Permit ID should be greater than 0.");



            // STEP 15: Close Approve PM Task Execution
            string CloseApproveTaskPayload = $@"
                                {{
                                    ""id"": {task_id},
                                    ""facility_id"":17,
                                    ""comment"": ""test"",
                              }}";
            var CloseApprovedTaskService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMRescheduleApprovalResponse>();

            var closeApprovedtaskresponse = CloseApprovedTaskService.PutService("/api/PMScheduleView/ApprovePMTaskExecution", CloseApproveTaskPayload);
            Assert.IsNotNull(closetaskresponse, "The API response should not be null.");
            //Assert.AreEqual($" Permit  {ptw_id} Closed", closeApprovedtaskresponse[0].message, "The permit update message should be correct.");
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, closeApprovedtaskresponse[0].return_status);
            /*int closeapprovetask_id = closeApprovedtaskresponse[0].id[0];
            Assert.IsTrue(closeapprovetask_id > 0, "The Permit ID should be greater than 0.");*/

        }
    }
}
