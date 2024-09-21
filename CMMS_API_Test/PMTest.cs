using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class PMTest
    {
        [TestMethod]
        public void PMFunctionalTest()
        {

        }

        [TestMethod]
        public void VerifyScheduleData()
        {
            int facilityId = 1736;
            int categoryId = 2;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMScheduleData>(true);
            var response = pmService.GetItemList($"/api/PM/GetScheduleData?facility_id={facilityId}&category_id={categoryId}");
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(123111, response[0].asset_id);
            Assert.AreEqual("RISEN_WIND_BLOCK_1_INV_1", response[0].asset_name);
            Assert.IsNotNull(response[0].frequency_dates);
            Assert.AreNotEqual(0, response[0].frequency_dates.Count);
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
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(5335, response[0].id);
            Assert.AreEqual(123111, response[0].equipment_id);
            Assert.AreEqual("RISEN_WIND_BLOCK_1_INV_1", response[0].equipment_name);
        }
        [TestMethod]
        public void VerifyPMTaskDetail()
        {
            int pm_schedule_id = 5335;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response = pmService.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response);
            Assert.AreEqual(123111, response.equipment_id);
            Assert.AreEqual("RISEN_WIND_BLOCK_1_INV_1", response.equipment_name);
        }
        [TestMethod]
        public void SetScheduleData()
        {
            int facility_id = 1736;
            int category_id = 4;
            int asset_id = 123123;
            int frequency_id = 7;
            DateTime scheduleDate = new DateTime(2023, 7, 7);
            string payload = $"{{\n" +
                                $"\t\"facility_id\": {facility_id}\n," +
                                $"\t\"asset_schedules\": [\n" +
                                $"\t\t{{\n" +
                                $"\t\t\t\"asset_id\": {asset_id},\n" +
                                $"\t\t\t\"frequency_dates\": [\n" +
                                $"\t\t\t\t{{\n" +
                                $"\t\t\t\t\t\"frequency_id\": {frequency_id},\n" +
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
            Assert.AreEqual(asset_id, response2.equipment_id);
            Assert.AreEqual(frequency_id, response2.frequency_id);
            Assert.AreEqual(scheduleDate, response2.schedule_date);
            Assert.AreEqual(frequency_id, response2.frequency_id);
        }
        [TestMethod]
        public void CancelPMTask()
        {
            string payload = "{\n\t\"id\" : 5335,\n\t\"comment\" : \"Cancelled\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/CancelPMTask", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CANCELLED, response2.status);
        }
        [TestMethod]
        public void LinkToPTW()
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
            int pm_schedule_id = 5334;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response2 = pmService.PutService($"/api/PMScheduleView/LinkPermitToPMTask?schedule_id={pm_schedule_id}&permit_id={ptw_id}", "");
            Assert.IsNotNull(response2[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response2[0].return_status);
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response3 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response3);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_LINK_PTW, response3.status);
            Assert.AreEqual(ptw_id, response3.permit_id);
        }
        [TestMethod]
        public void SetPMTask()
        {
            int pm_schedule_id = 5334;
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PostService($"/api/PMScheduleView/SetPMTask?schedule_id={pm_schedule_id}", "");
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_START, response2.status);
            Assert.IsNotNull(response2.schedule_check_points);
            Assert.AreNotEqual(0, response2.schedule_check_points.Count);
        }
        [TestMethod]
        public void AddCustomCheckpoint()
        {
            int pm_schedule_id = 5334;
            string payload = @$"{{
                                    ""schedule_id"": {pm_schedule_id},
                                    ""check_point_name"": ""Check all switches"",
                                    ""requirement"": ""All switches should work smoothly.""
                                }}";

            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response = pmService.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}"); 
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.schedule_check_points);
            Assert.AreNotEqual(0, response.schedule_check_points.Count);
            int expectedCPs = response.schedule_check_points.Count + 1;
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response2 = pmService2.PostService("/api/PMScheduleView/AddCustomCheckpoint", payload);
            Assert.IsNotNull(response2[0]);
            var pmService3 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response3 = pmService3.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response3);
            Assert.IsNotNull(response3.schedule_check_points);
            Assert.AreEqual(expectedCPs, response3.schedule_check_points.Count);
        }
        [TestMethod]
        public void ClosePMTaskExecution()
        {
            string payload = "{\n\t\"id\" : 5334, \n\t\"comment\" : \"Closed\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/ClosePMTaskExecution", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CLOSED, response2.status);
        }
        [TestMethod]
        public void RejectPMTaskExecution()
        {
            string payload = "{\n\t\"id\" : 5334, \n\t\"comment\" : \"Not OK\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/RejectPMTaskExecution", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
            Assert.IsNotNull(response2);
            Assert.AreEqual((int)CMMS.CMMS_Status.PM_CLOSE_REJECTED, response2.status);
        }
        [TestMethod]
        public void ApprovePMTaskExecution()
        {
            string payload = "{\n\t\"id\" : 5334, \n\t\"comment\" : \"OK\"\n}";
            var pmService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = pmService.PutService("/api/PMScheduleView/ApprovePMTaskExecution", payload);
            Assert.IsNotNull(response[0]);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response[0].return_status);
            int pm_schedule_id = response[0].id[0];
            var pmService2 = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMPMScheduleViewDetail>(true);
            var response2 = pmService2.GetItem($"/api/PMScheduleView/GetPMTaskDetail?schedule_id={pm_schedule_id}");
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
        /**/
    }
}
