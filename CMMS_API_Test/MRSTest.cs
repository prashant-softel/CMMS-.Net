using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{

    [TestClass]
    public class MRSTest
    {
        //◆◇◆◆◆ MRS ◆◆◆◆◇◆
        string EP_CreateMRS = "/api/MRS/CreateMRS";
        string EP_getMRSList = "/api/MRS/getMRSList";
        string EP_getMRSItems = "/api/MRS/getMRSItems";
        string EP_getMRSItemsBeforeIssue = "/api/MRS/getMRSItemsBeforeIssue";
        string EP_getLastTemplateData = "/api/MRS/getLastTemplateData";
        string EP_getMRSItemsWithCode = "/api/MRS/getMRSItemsWithCode";
        string EP_getIssuedAssetItems = "/api/MRS/getIssuedAssetItems";
        string EP_getMRSDetails = "/api/MRS/getMRSDetails";
        string EP_mrsApproval = "/api/MRS/mrsApproval";
        string EP_mrsReject = "/api/MRS/mrsReject";
        string EP_CreateMRSIssue = "/api/MRS/MRSIssue";
        string EP_ApproveMRSIssue = "/api/MRS/ApproveMRSIssue";
        string EP_RejectMRSIssue = "/api/MRS/RejectMRSIssue";
        string EP_getMRSListByModule = "/api/MRS/getMRSListByModule";
        string EP_updateMRS = "/api/MRS/updateMRS";
        string EP_GetAssetItems = "/api/MRS/GetAssetItems";
        string EP_getMRSReturnStockItems = "/api/MRS/getMRSReturnStockItems";
        string EP_GetAvailableQuantityinPlant = "/api/MRS/GetAvailableQuantityinPlant";


        //◆◇◆◆◆ RETURN MRS ◆◆◆◆◇◆
        string EP_getReturnDataByID = "/api/MRS/getReturnDataByID";
        string EP_getAssetTypeByItemID = "/api/MRS/getAssetTypeByItemID";
        string EP_CreateReturnMRS = "/api/MRS/CreateReturnMRS";
        string EP_UpdateReturnMRS = "/api/MRS/UpdateReturnMRS";
        string EP_getMRSReturnList = "/api/MRS/getMRSReturnList";
        string EP_ApproveMRSReturn = "/api/MRS/ApproveMRSReturn";
        string EP_RejectMRSReturn = "/api/MRS/RejectMRSReturn";
        string EP_CreateReturnFaultyMRS = "/api/MRS/CreateReturnFaultyMRS";




        //◆◇◆◆◆ MRS ◆◆◆◆◇◆
        /*[TestMethod]
        public void VerifyCreateMRS()
        {
            
            string payload = @"{
                                  ""ID"": 0,
                                  ""to_actor_id"": 205,
                                  ""to_actor_type_id"": 3,
                                  ""from_actor_id"": 1,
                                  ""from_actor_type_id"": 2,
                                  ""activity"": ""Switchgear Quarterly Check"",
                                  ""facility_ID"": 1,
                                  ""isEditMode"": 0,
                                  ""rejected_by_emp_ID"": 1,
                                  ""remarks"": ""rererere"",
                                  ""setAsTemplate"": """",
                                  ""whereUsedType"": 27,
                                  ""whereUsedRefID"": 205,
                                  ""cmmrsItems"": [
                                      {
                                          ""issued_qty"": 0,
                                          ""requested_qty"": 3,
                                          ""asset_code"": ""S423741003"",
                                          ""asset_type_ID"": 1,
                                          ""asset_item_ID"": 83,
                                          ""id"": 83,
                                          ""available_qty"": 5,
                                          ""serial_number"": null
                                      },
                                      {
                                          ""issued_qty"": 0,
                                          ""requested_qty"": 1,
                                          ""asset_code"": ""S071804001"",
                                          ""asset_type_ID"": 2,
                                          ""asset_item_ID"": 121,
                                          ""id"": 121,
                                          ""available_qty"": 2,
                                          ""serial_number"": null
                                      }

                                  ]
                              }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_CreateMRS, payload);
            string expectedMessgae = "Request has been submitted.";
            string responseMessage = response.message;
            Assert.AreEqual(expectedMessgae, responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);
            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_SUBMITTED, mrsResponse.status);

            // Deserialize payload into CMMRS object
            var expectedCMMRS = JsonConvert.DeserializeObject<CMMRSList>(payload);

            // Compare properties
            Assert.AreEqual(mrsId, mrsResponse.ID);
            Assert.AreEqual(expectedCMMRS.to_actor_id, mrsResponse.to_actor_id);
            Assert.AreEqual(expectedCMMRS.to_actor_type_id, mrsResponse.to_actor_type_id);
            Assert.AreEqual(expectedCMMRS.from_actor_id, mrsResponse.from_actor_id);
            Assert.AreEqual(expectedCMMRS.from_actor_type_id, mrsResponse.from_actor_type_id);
            //Assert.AreEqual(expectedCMMRS.facilityId, mrsResponse.facilityId);
            Assert.AreEqual(expectedCMMRS.activity, mrsResponse.activity);
            Assert.AreEqual(expectedCMMRS.remarks, mrsResponse.remarks);
            Assert.AreEqual(expectedCMMRS.whereUsedRefID, mrsResponse.whereUsedRefID);

            // Compare cmmrsItems
            Assert.AreEqual(expectedCMMRS.CMMRSItems.Count, mrsResponse.CMMRSItems.Count);
            for (int i = 0; i < expectedCMMRS.CMMRSItems.Count; i++)
            {
                var expectedItem = expectedCMMRS.CMMRSItems[i];
                var actualItem = mrsResponse.CMMRSItems[i];

                Assert.AreEqual(expectedItem.issued_qty, actualItem.issued_qty);
                Assert.AreEqual(expectedItem.requested_qty, actualItem.requested_qty);
                Assert.AreEqual(expectedItem.asset_code, actualItem.asset_code);
                Assert.AreEqual(expectedItem.asset_type_ID, actualItem.asset_type_ID);
                Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_item_ID);
                Assert.AreEqual(expectedItem.asset_type_ID, actualItem.asset_type_ID);
                Assert.AreEqual(expectedItem.available_qty, actualItem.available_qty);
                //Assert.AreEqual(expectedItem.serial_number, actualItem.serial_number);
            }


        }*/


        [TestMethod]
        public void VerifyCreateMRS()
        {

            var mrsRequest = new CMMSAPIs.Models.SM.CMMRSList
            {
                ID = 0,
                to_actor_id = 205,
                to_actor_type_id = 3,
                from_actor_id = 1,
                from_actor_type_id = 2,
                activity = "Switchgear Quarterly Check",
                facilityId = 1,
                rejected_by_emp_ID = 1,
                remarks = "rererere",
                //setAsTemplate = 1,
                //isEditMode = 1,
                //whereUsedType = 27,
                whereUsedRefID = 205,
                CMMRSItems = new List<CMMSAPIs.Models.SM.CMMRSItems>
        {
            new CMMSAPIs.Models.SM.CMMRSItems
            {
                issued_qty = 0,
                requested_qty = 3,
                asset_code = "S423741003",
                asset_type_ID = 1,
                asset_item_ID = 83,
                available_qty = 5,
                serial_number = null
            },
            new CMMSAPIs.Models.SM.CMMRSItems
            {
                issued_qty = 0,
                requested_qty = 1,
                asset_code = "S071804001",
                asset_type_ID = 2,
                asset_item_ID = 121,
                available_qty = 2,
                serial_number = null
            }
        }
            };


            string payload = JsonConvert.SerializeObject(mrsRequest);

            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_CreateMRS, payload);

            string expectedMessage = "Request has been submitted.";
            string responseMessage = response.message;
            Assert.AreEqual(expectedMessage, responseMessage);

            int mrsId = response.id[0];
            var getItemService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse = getItemService.GetItem(EP_getMRSDetails + "?ID=" + mrsId);
            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_SUBMITTED, mrsResponse.status);

            Assert.AreEqual(mrsId, mrsResponse.ID);
            /*Assert.AreEqual(mrsRequest.to_actor_id, mrsResponse.to_actor_id);
            Assert.AreEqual(mrsRequest.to_actor_type_id, mrsResponse.to_actor_type_id);
            Assert.AreEqual(mrsRequest.from_actor_id, mrsResponse.from_actor_id);
            Assert.AreEqual(mrsRequest.from_actor_type_id, mrsResponse.from_actor_type_id);*/
            Assert.AreEqual(mrsRequest.activity, mrsResponse.activity);
            Assert.AreEqual(mrsRequest.facilityId, mrsResponse.facilityId);
            Assert.AreEqual(mrsRequest.remarks, mrsResponse.remarks);
            Assert.AreEqual(mrsRequest.whereUsedRefID, mrsResponse.whereUsedRefID);
            //Assert.AreEqual(mrsRequest.whereUsedType, mrsResponse.whereUsedType);


            Assert.AreEqual(mrsRequest.CMMRSItems.Count, mrsResponse.CMMRSItems.Count);
            for (int i = 0; i < mrsRequest.CMMRSItems.Count; i++)
            {
                var expectedItem = mrsRequest.CMMRSItems[i];
                var actualItem = mrsResponse.CMMRSItems[i];

                Assert.AreEqual(expectedItem.issued_qty, actualItem.issued_qty);
                Assert.AreEqual(expectedItem.requested_qty, actualItem.requested_qty);
                Assert.AreEqual(expectedItem.asset_code, actualItem.asset_code);
                Assert.AreEqual(expectedItem.asset_type_ID, actualItem.asset_type_ID);
                Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_item_ID);
                Assert.AreEqual(expectedItem.available_qty, actualItem.available_qty);

            }
        }


        [TestMethod]
        public void VerifyUpdateMRS()
        {


            string payload = @$"{{
                              ""ID"": 203,
                              ""to_actor_id"": 205,
                              ""to_actor_type_id"": 3,
                              ""from_actor_id"": 1,
                              ""from_actor_type_id"": 2,
                              ""activity"": ""Switchgear Quarterly Check Updated"",
                              ""facility_ID"": 1,
                              ""isEditMode"": 1,
                              ""remarks"": ""Updated Remarks"",
                              ""setAsTemplate"": """",
                              ""whereUsedType"": 27,
                              ""whereUsedRefID"": 205,
                              ""cmmrsItems"": [
                                  {{
                                      ""issued_qty"": 1,
                                      ""requested_qty"": 2,
                                      ""asset_code"": ""S423741003"",
                                      ""asset_type_ID"": 1,
                                      ""asset_item_ID"": 83,
                                      ""id"": 83,
                                      ""available_qty"": 4,
                                      ""serial_number"": null
                                  }},
                                  {{
                                      ""issued_qty"": 1,
                                      ""requested_qty"": 1,
                                      ""asset_code"": ""S071804001"",
                                      ""asset_type_ID"": 2,
                                      ""asset_item_ID"": 121,
                                      ""id"": 121,
                                      ""available_qty"": 1,
                                      ""serial_number"": null
                                  }}
                              ]
                          }}";


            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_updateMRS, payload);


            string expectedMessage = "Request has been updated.";
            string responseMessage = response.message;
            Assert.AreEqual(expectedMessage, responseMessage);


            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);


            Assert.AreEqual(mrsId, mrsResponse.ID);
            /*Assert.AreEqual(205, mrsResponse.to_actor_id);
            Assert.AreEqual(3, mrsResponse.to_actor_type_id);
            Assert.AreEqual(1, mrsResponse.from_actor_id);
            Assert.AreEqual(2, mrsResponse.from_actor_type_id);*/
            Assert.AreEqual("Switchgear Quarterly Check Updated", mrsResponse.activity);
            Assert.AreEqual(1, mrsResponse.facilityId);
            Assert.AreEqual("Updated Remarks", mrsResponse.remarks);
            Assert.AreEqual(205, mrsResponse.whereUsedRefID);

            // Compare the first item in cmmrsItems
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].issued_qty);
            Assert.AreEqual(2, mrsResponse.CMMRSItems[0].requested_qty);
            Assert.AreEqual("S423741003", mrsResponse.CMMRSItems[0].asset_code);
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].asset_type_ID);
            Assert.AreEqual(83, mrsResponse.CMMRSItems[0].asset_item_ID);
            Assert.AreEqual(4, mrsResponse.CMMRSItems[0].available_qty);


            Assert.AreEqual(1, mrsResponse.CMMRSItems[1].issued_qty);
            Assert.AreEqual(1, mrsResponse.CMMRSItems[1].requested_qty);
            Assert.AreEqual("S071804001", mrsResponse.CMMRSItems[1].asset_code);
            Assert.AreEqual(2, mrsResponse.CMMRSItems[1].asset_type_ID);
            Assert.AreEqual(121, mrsResponse.CMMRSItems[1].asset_item_ID);
            Assert.AreEqual(1, mrsResponse.CMMRSItems[1].available_qty);
        }

        [TestMethod]
        public void VerifygetMRSList()
        {
            var facilityID = 1;
            var emp_id = 1;
            var fromDate = "2024-09-09";
            var toDate = "2024-09-16";

            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItemList(EP_getMRSList + "?facility_ID=" + facilityID + "&emp_id=" + emp_id + "&toDate=" + toDate + "&fromDate=" + fromDate);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 6);*/

            Assert.IsNotNull(response, "MRS list should not be null.");
            Assert.IsTrue(response.Count > 0, "MRS list should contain at least one order.");
        }

        [TestMethod]
        public void VerifygetMRSItemsBeforeIssue()
        {
            int ID = 196;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getMRSItemsBeforeIssue + "?ID=" + ID);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/

            Assert.IsNotNull(response, "MRS items before issue should not be null.");
            //Assert.IsTrue(response.Count > 0, "MRS items before issue contain at least one order.");
        }

        [TestMethod]
        public void VerifygetLastTemplateData()
        {
            int ID = 200;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getLastTemplateData + "?ID=" + ID);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/

            Assert.IsNotNull(response, "MRS Last template data should not be null.");
        }

        [TestMethod]
        public void VerifyGetAssetItems()
        {
            int facility_ID = 45;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItemList(EP_GetAssetItems + "?facility_ID=" + facility_ID);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/

            Assert.IsNotNull(response, "MRS Asset Items should not be null.");
            Assert.IsTrue(response.Count > 0, "MRS asset item contain at least one item.");
        }

        [TestMethod]
        public void VerifygetMRSItemsWithCode()
        {
            int ID = 196;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getMRSItemsWithCode + "?ID=" + ID);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/

            Assert.IsNotNull(response, "MRS items with code should not be null.");

        }

        [TestMethod]
        public void VerifygetIssuedAssetItems()
        {
            int ID = 1;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getIssuedAssetItems + "?ID=" + ID);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/

            Assert.IsNotNull(response, "MRS Issued Assets name should not be null.");

        }

        [TestMethod]
        public void VerifygetMRSReturnStockItems()
        {

            var mrs_id = 5;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItemList(EP_getMRSReturnStockItems + "?mrs_id=" + mrs_id);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 6);*/

            Assert.IsNotNull(response, "MRS Return Stock Items should not be null.");
            Assert.IsTrue(response.Count > 0, "MRS Return Stock Items should contain at least one order.");
        }

        [TestMethod]
        public void VerifyGetAvailableQuantityinPlant()
        {

            var mrs_id = 82;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItemList(EP_GetAvailableQuantityinPlant + "?mrs_id=" + mrs_id);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 6);*/

            Assert.IsNotNull(response, "MRS Avaialable Quantity in plant should not be null.");
            Assert.IsTrue(response.Count > 0, "MRS Avaialable Quantity in plant should contain at least one order.");
        }

        [TestMethod]
        public void VerifygetAssetTypeByItemID()
        {
            int ItemID = 1;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getAssetTypeByItemID + "?ItemID=" + ItemID);

            Assert.IsNotNull(response, "Asset type should not be null.");
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/
        }

        [TestMethod]
        public void VerifygetMRSListByModule()
        {
            int ItemID = 1;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getMRSListByModule + "?ItemID=" + ItemID);

            Assert.IsNotNull(response, "MRS List By Module should not be null.");
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/
        }

        [TestMethod]
        public void VerifygetMRSItems()
        {
            int ID = 204;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItemList(EP_getMRSItems + "?ID=" + ID);
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 10);*/
            Assert.IsNotNull(response, "MRS list should not be null.");
            Assert.IsTrue(response.Count > 0, "MRS list should contain at least one order.");
        }

        [TestMethod]
        public void VerifygetMRSDetails()
        {
            var id = 189;
            var facilityID = 1;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getMRSDetails + "?ID=" + id + "&facility_id=" + facilityID);
            Assert.IsNotNull(response);
            Assert.AreEqual(response.ID, id);
            Assert.IsNotNull(response, "MRS list should not be null.");

        }

        [TestMethod]
        public void VerifyMRSApproval()
        {
            string payload = @"{
                                ""ID"":195,
                                ""comment"": ""MRS approval for MRS""
                         
                        }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_mrsApproval, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);
            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED, mrsResponse.status);

            Assert.AreEqual(mrsResponse.approver_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.approval_date;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS approved timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);
        }

        [TestMethod]
        public void VerifyMRSReject()
        {
            string payload = @"{
                                ""ID"":273,
                                ""comment"": ""MRS reject for MRS""
                        }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_mrsReject, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED, mrsResponse.status);

            //Assert.AreEqual(mrsResponse.request_rejected_by_name, "Admin HFE");  //property added in model class
            DateTime expectedGeneratedAt = DateTime.Now;
            /*DateTime actualGeneratedAt = (DateTime)mrsResponse.request_rejected_at;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS rejected timestamp should match.");*/

            Assert.AreEqual(mrsId, response.id[0]);
        }

        [TestMethod]
        public void VerifyMRSIssue()
        {
            string payload = @"{
                          ""ID"": 199,
                          ""issue_comment"": ""test"",
                          ""cmmrsItems"": [
                              {
                                  ""mrs_item_id"": 551,
                                  ""issued_qty"": 1,
                                  ""asset_item_ID"": 211,
                                  ""serial_number"": """"
                              }
                          ]
                      }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_CreateMRSIssue, payload);
            string responseMessage = response.message;
            Assert.AreEqual("MRS Request Issued.", responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED, mrsResponse.status);

            //Assert.AreEqual(mrsResponse.issued_name, "Admin HFE");  //property added in model class
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.issued_date;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The issued timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);
        }

        [TestMethod]
        public void VerifyApprovedMRSIssue()
        {
            string payload = @"{
                          ""ID"": 199,
                          ""issue_comment"": ""test"",
                          ""cmmrsItems"": [
                              {
                                  ""mrs_item_id"": 551,
                                  ""issued_qty"": 1,
                                  ""asset_item_ID"": 211,
                                  ""serial_number"": """"
                              }
                          ]
                      }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_ApproveMRSIssue, payload);
            string responseMessage = response.message;
            string actualMessage = "Status updated.";
            Assert.AreEqual(actualMessage, responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED, mrsResponse.status);

            // Assert.AreEqual(mrsResponse.issue_approved_by_name, "Admin HFE");    //property added in model class and notification object
            DateTime expectedGeneratedAt = DateTime.Now;
            //DateTime actualGeneratedAt = (DateTime)mrsResponse.issue_approved_date;


            //expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            //actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            //Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The issue approved timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);
        }

        [TestMethod]
        public void VerifyRejectedMRSIssue()
        {
            string payload = @"{
                          ""ID"": 199,
                          ""issue_comment"": ""test"",
                          ""cmmrsItems"": [
                              {
                                  ""mrs_item_id"": 551,
                                  ""issued_qty"": 1,
                                  ""asset_item_ID"": 211,
                                  ""serial_number"": """"
                              }
                          ]
                      }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_RejectMRSIssue, payload);
            string responseMessage = response.message;
            string actualMessage = "Status updated.";
            Assert.AreEqual(actualMessage, responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED, mrsResponse.status);

            //Assert.AreEqual(mrsResponse.issue_rejected_by_name, "Admin HFE");    //property added in model class 
            //DateTime expectedGeneratedAt = DateTime.Now;
            //DateTime actualGeneratedAt = (DateTime)mrsResponse.issue_rejected_date; //property added in model class


            //expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            //actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);

            //Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The issue rejected timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);
        }





        //◆◇◆◆◆ RETURN MRS ◆◆◆◆◇◆

        [TestMethod]
        public void VerifygetReturnDataByID()
        {
            int ID = 186;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var response = mrsService.GetItem(EP_getReturnDataByID + "?ID=" + ID);
            Assert.IsNotNull(response, "MRS list should not be null.");
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/
        }


        [TestMethod]
        public void VerifygetMRSReturnList()
        {
            int ID = 186;
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = mrsService.GetItem(EP_getMRSReturnList + "?ID=" + ID);
            Assert.IsNotNull(response, "MRS return should not be null.");
            /*int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);*/
        }

        [TestMethod]
        public void VerifyCreateReturnMRS()
        {
            string payload = @"{
                                    ""ID"": 0,
                                    ""to_actor_type_id"": 2,
                                    ""from_actor_type_id"": 4,
                                    ""to_actor_id"": 1,
                                    ""from_actor_id"": 15,
                                    ""facilityID"": 1,
                                    ""activity"": ""job test 123"",
                                    ""whereUsedRefID"": 15,
                                    ""whereUsedType"": 4,
                                    ""setAsTemplate"": """",
                                    ""remarks"": ""cmnttt"",
                                    ""faultyItems"": [
                                        {
                                            ""assetMasterItemID"": 12,
                                            ""serial_number"": ""ss31"",
                                            ""return_remarks"": ""remark of faulty"",
                                            ""returned_qty"": 1,
                                            ""faulty_item_asset_id"": 4,
                                            ""mrs_item_ID"": 0
                                        }
                                    ],
                                    ""cmmrsItems"": [
                                        {
                                            ""mrs_item_ID"": 210,
                                            ""return_remarks"": ""returned unused"",
                                            ""returned_qty"": 1
                                        }
                                    ]
                                }";

            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_CreateReturnMRS, payload);
            string responseMessage = response.message;
            string actualMessage = "MRS return submitted.";
            Assert.AreEqual(actualMessage, responseMessage);

            int RmrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse = getItem.GetItem(EP_getReturnDataByID + "?ID=" + RmrsId);

            var expectedCMMRS = JsonConvert.DeserializeObject<CMMRSReturnList>(payload);

            Assert.AreEqual(RmrsId, mrsResponse.ID, "The MRS ID should match the input.");
            Assert.AreEqual(expectedCMMRS.to_actor_type_id, mrsResponse.to_actor_type_id, "The to_actor_type_id should match.");
            Assert.AreEqual(expectedCMMRS.from_actor_type_id, mrsResponse.from_actor_type_id, "The from_actor_type_id should match.");
            Assert.AreEqual(expectedCMMRS.to_actor_id, mrsResponse.to_actor_id, "The to_actor_id should match.");
            Assert.AreEqual(expectedCMMRS.from_actor_id, mrsResponse.from_actor_id, "The from_actor_id should match.");
            //Assert.AreEqual(expectedCMMRS.facilityId, mrsResponse.facilityId, "The facility_ID should match.");
            Assert.AreEqual(expectedCMMRS.activity, mrsResponse.activity, "The activity field should match.");
            Assert.AreEqual(expectedCMMRS.whereUsedRefID, mrsResponse.whereUsedRefID, "The whereUsedRefID should match.");
            //Assert.AreEqual(expectedCMMRS.whereUsedType, mrsResponse.whereUsedType, "The whereUsedType should match.");

            // Validate faultyItems
            Assert.AreEqual(12, mrsResponse.CMMRSFaultyItems[0].asset_item_ID, "The mrsItemID of the faulty item should match.");
            Assert.AreEqual("ss31", mrsResponse.CMMRSFaultyItems[0].serial_number, "The assetMasterItemID of the faulty item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSFaultyItems[0].returned_qty, "The returned_qty of the faulty item should match.");
            Assert.AreEqual("remark of faulty", mrsResponse.CMMRSFaultyItems[0].return_remarks, "The return_remarks of the faulty item should match.");
            //Assert.AreEqual("99125", mrsResponse.CMMRSFaultyItems[0].faulty_item_asset_id, "The sr_no of the faulty item should match.");
            //Assert.AreEqual(830, mrsResponse.CMMRSFaultyItems[0].mrs_item_id, "The sr_no of the faulty item should match.");


            // Validate cmmrsItems
            //Assert.AreEqual(842, mrsResponse.CMMRSItems[0].mrs_item_id, "The asset_item_ID of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].returned_qty, "The returned_qty of the first item should match.");
            Assert.AreEqual("returned unused", mrsResponse.CMMRSItems[0].return_remarks, "The return_remarks of the first item should match.");
        }


        [TestMethod]
        public void VerifyUpdateReturnMRS()
        {
            string payload = @"{
                                    ""ID"": 301,
                                    ""to_actor_type_id"": 2,
                                    ""from_actor_type_id"": 4,
                                    ""to_actor_id"": 1,
                                    ""from_actor_id"": 15,
                                    ""facilityID"": 1,
                                    ""activity"": ""job test 123"",
                                    ""whereUsedRefID"": 15,
                                    ""whereUsedType"": 4,
                                    ""setAsTemplate"": """",
                                    ""remarks"": ""cmnttt"",
                                    ""faultyItems"": [
                                        {
                                            ""assetMasterItemID"": 12,
                                            ""serial_number"": ""ss31"",
                                            ""return_remarks"": ""remark of faulty"",
                                            ""returned_qty"": 1,
                                            ""faulty_item_asset_id"": 4,
                                            ""mrs_item_ID"": 0
                                        }
                                    ],
                                    ""cmmrsItems"": [
                                        {
                                            ""mrs_item_ID"": 210,
                                            ""return_remarks"": ""returned unused"",
                                            ""returned_qty"": 1
                                        }
                                    ]
                                }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_UpdateReturnMRS, payload);
            string responseMessage = response.message;
            string actualMessage = "MRS return submitted.";
            Assert.AreEqual(actualMessage, responseMessage);

            int RmrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse = getItem.GetItem(EP_getReturnDataByID + "?ID=" + RmrsId);

            Assert.AreEqual(RmrsId, mrsResponse.ID, "The MRS ID should match the input.");
            Assert.AreEqual("job test 123", mrsResponse.activity, "The activity field should match.");
            /*Assert.AreEqual(2, mrsResponse.to_actor_type_id, "The to_actor_type_id should match.");
            Assert.AreEqual(4, mrsResponse.from_actor_type_id, "The from_actor_type_id should match.");
            Assert.AreEqual(1, mrsResponse.to_actor_id, "The to_actor_id should match.");
            Assert.AreEqual(15, mrsResponse.from_actor_id, "The from_actor_id should match.");*/
            //Assert.AreEqual(1779, mrsResponse.facilityId, "The facility_ID should match.");
            Assert.AreEqual(15, mrsResponse.whereUsedRefID, "The whereUsedRefID should match.");
            //Assert.AreEqual(27, mrsResponse.whereUsedTypeName, "The whereUsedType should match.");
            //Assert.AreEqual("", mrsResponse, "The setAsTemplate should match.");
            Assert.AreEqual("cmnttt", mrsResponse.remarks, "The remarks should match.");


            //Verify CMMRS Items 
            //Assert.AreEqual(10, mrsResponse.CMMRSItems[0].mrs_item_id, "The asset_item_ID of the first item should match.");
            //Assert.AreEqual(1, mrsResponse.CMMRSItems[0].return_remarks, "The issued_qty of the first item should match.");
            //Assert.AreEqual(1, mrsResponse.CMMRSItems[0].returned_qty, "The returned_qty of the first item should match.");


            // Validate faultyItems
            //Assert.AreEqual(128, mrsResponse.CMMRSFaultyItems[0].faulty_item_asset_id, "The mrsItemID of the faulty item should match.");
            //Assert.AreEqual(10, mrsResponse.CMMRSFaultyItems[0].assetMasterID, "The assetMasterItemID of the faulty item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSFaultyItems[0].returned_qty, "The returned_qty of the faulty item should match.");
            Assert.AreEqual("remark of faulty", mrsResponse.CMMRSFaultyItems[0].return_remarks, "The return_remarks of the faulty item should match.");
            Assert.AreEqual("ss31", mrsResponse.CMMRSFaultyItems[0].serial_number, "The sr_no of the faulty item should match.");
        }



        [TestMethod]
        public void VerifyReturnMRSApproval()
        {
            string payload = @"{
                                ""ID"":58,
                                ""comment"": "" Return MRS approval""
                         
                        }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_ApproveMRSReturn, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Equipment returned to store.", responseMessage);
            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse = getItem.GetItem(EP_getReturnDataByID + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED, mrsResponse.status);

            Assert.AreEqual(mrsResponse.approver_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.approved_date;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The Return MRS approved timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);
        }

        [TestMethod]
        public void VerifyReturnMRSReject()
        {
            string payload = @"{
                                ""ID"":168,
                                ""comment"": ""Return MRS reject for MRS 131""
                        }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_RejectMRSReturn, payload);
            string responseMessage = response.message;
            //string actualMessage = $"MRS Return Rejected. Reason : {}"
            Assert.AreEqual("MRS Return Rejected. Reason : Return MRS reject for MRS 131", responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse = getItem.GetItem(EP_getReturnDataByID + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED, mrsResponse.status);

            Assert.AreEqual(mrsResponse.request_rejected_by_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.rejected_date;

            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The Return MRS rejected timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);
        }

        [TestMethod]
        public void VerifyCreateReturnFaultyMRS()
        {

            string payload = @"{
                      ""ID"": 48,
                      ""facility_ID"": 1779,
                      ""whereUsedRefID"": 19,
                      ""whereUsedType"": 27,
                      ""setAsTemplate"": """",
                      ""remarks"": """",
                      ""activity"":""testActivity"",
                      ""cmmrsItems"": [
                        {
                          ""issued_qty"": 80,
                          ""requested_qty"": 0,
                          ""asset_item_ID"": 2,
                          ""approval_required"": 0,
                          ""is_faulty"": 1,
                          ""return_remarks"": ""return faulty"",
                          ""returned_qty"": 2
                        }
                      ]
                  }";


            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_CreateReturnFaultyMRS, payload);


            string expectedMessage = "MRS return faluty submitted.";
            string responseMessage = response.message;
            Assert.AreEqual(expectedMessage, responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual(mrsId, mrsResponse.ID);
            //Assert.AreEqual(1779, mrsResponse.facility_ID);
            Assert.AreEqual(19, mrsResponse.whereUsedRefID);
            //Assert.AreEqual(27, mrsResponse.whereUsedTypeName);
            Assert.AreEqual("testActivity", mrsResponse.activity);

            var faultyItem = mrsResponse.cmmrsItems[0];
            Assert.AreEqual(80, mrsResponse.cmmrsItems[0].issued_qty);
            Assert.AreEqual(0, faultyItem.requested_qty);
            Assert.AreEqual(2, faultyItem.asset_item_ID);
            //Assert.AreEqual(1, faultyItem.is_faulty);
            Assert.AreEqual("return faulty", faultyItem.return_remarks);
            Assert.AreEqual(2, faultyItem.returned_qty);
        }





        //◆◇◆◆◆ MRS FUNCTIONAL TEST◆◆◆◆◇◆
        [TestMethod]
        public void VerifyFunctionalTestMRS()
        {

            //STEP 1: Create MRS
            var createmrs = new CMMSAPIs.Models.SM.CMMRSList
            {
                ID = 0,
                to_actor_id = 205,
                to_actor_type_id = 3,
                from_actor_id = 1,
                from_actor_type_id = 2,
                activity = "Switchgear Quarterly Check",
                facilityId = 1,
                rejected_by_emp_ID = 1,
                remarks = "rererere",
                //setAsTemplate = 1,
                //isEditMode = 1,
                //whereUsedType = 27,
                whereUsedRefID = 205,
                CMMRSItems = new List<CMMSAPIs.Models.SM.CMMRSItems>
        {
            new CMMSAPIs.Models.SM.CMMRSItems
            {
                issued_qty = 0,
                requested_qty = 3,
                asset_code = "S423741003",
                asset_type_ID = 1,
                asset_item_ID = 83,
                available_qty = 5,
                serial_number = null
            },
            new CMMSAPIs.Models.SM.CMMRSItems
            {
                issued_qty = 0,
                requested_qty = 1,
                asset_code = "S071804001",
                asset_type_ID = 2,
                asset_item_ID = 121,
                available_qty = 2,
                serial_number = null
            }
        }
            };


            string payload = JsonConvert.SerializeObject(createmrs);

            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_CreateMRS, payload);

            string expectedMessage = "Request has been submitted.";
            string responseMessage = response.message;
            Assert.AreEqual(expectedMessage, responseMessage);

            int mrsId = response.id[0];
            var getItemService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse = getItemService.GetItem(EP_getMRSDetails + "?ID=" + mrsId);
            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_SUBMITTED, mrsResponse.status);

            Assert.AreEqual(mrsId, mrsResponse.ID);
            Assert.AreEqual(createmrs.to_actor_id, mrsResponse.to_actor_id);
            Assert.AreEqual(createmrs.to_actor_type_id, mrsResponse.to_actor_type_id);
            Assert.AreEqual(createmrs.from_actor_id, mrsResponse.from_actor_id);
            Assert.AreEqual(createmrs.from_actor_type_id, mrsResponse.from_actor_type_id);
            Assert.AreEqual(createmrs.activity, mrsResponse.activity);
            //Assert.AreEqual(createmrs.facilityId, mrsResponse.facilityId);
            Assert.AreEqual(createmrs.remarks, mrsResponse.remarks);
            Assert.AreEqual(createmrs.whereUsedRefID, mrsResponse.whereUsedRefID);
            //Assert.AreEqual(createmrs.whereUsedType, mrsResponse.whereUsedType);


            Assert.AreEqual(createmrs.CMMRSItems.Count, mrsResponse.CMMRSItems.Count);
            for (int i = 0; i < createmrs.CMMRSItems.Count; i++)
            {
                var expectedItem = createmrs.CMMRSItems[i];
                var actualItem = mrsResponse.CMMRSItems[i];

                Assert.AreEqual(expectedItem.issued_qty, actualItem.issued_qty);
                Assert.AreEqual(expectedItem.requested_qty, actualItem.requested_qty);
                Assert.AreEqual(expectedItem.asset_code, actualItem.asset_code);
                Assert.AreEqual(expectedItem.asset_type_ID, actualItem.asset_type_ID);
                Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_item_ID);
                Assert.AreEqual(expectedItem.available_qty, actualItem.available_qty);

            }


            //STEP 2: Reject MRS
            string rejectmrs = $@"{{
                                ""ID"":{mrsId},
                                ""comment"": ""MRS reject for MRS""
                                }}";
            var RejectService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Rejectresponse = RejectService.CreateItem(EP_mrsReject, rejectmrs);
            string Rejectmessage = Rejectresponse.message;
            Assert.AreEqual("Status updated.", Rejectmessage);
            var getItemService2 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse2 = getItemService2.GetItem(EP_getMRSDetails + "?ID=" + mrsId);
            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED, mrsResponse2.status);

            Assert.AreEqual(mrsResponse2.request_rejected_by_name, "Admin HFE");  
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse2.rejected_date;

            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS rejected timestamp should match.");




            //STEP 3: Update MRS
            var Updatemrs = new CMMSAPIs.Models.SM.CMMRSList
            {
                ID = mrsId ,
                to_actor_id = 205,
                to_actor_type_id = 3,
                from_actor_id = 1,
                from_actor_type_id = 2,
                activity = "Switchgear Quarterly Check",
                facilityId = 1,
                rejected_by_emp_ID = 1,
                remarks = "rererere",
                //setAsTemplate = 1,
                //isEditMode = 1,
                //whereUsedType = 27,
                whereUsedRefID = 205,
                CMMRSItems = new List<CMMSAPIs.Models.SM.CMMRSItems>
            {
            new CMMSAPIs.Models.SM.CMMRSItems
            {
                issued_qty = 0,
                requested_qty = 3,
                asset_code = "S423741003",
                asset_type_ID = 1,
                asset_item_ID = 83,
                available_qty = 5,
                serial_number = null
            },
            new CMMSAPIs.Models.SM.CMMRSItems
            {
                issued_qty = 0,
                requested_qty = 1,
                asset_code = "S071804001",
                asset_type_ID = 2,
                asset_item_ID = 121,
                available_qty = 2,
                serial_number = null
            }
            }
            };


            string Updatepayload = JsonConvert.SerializeObject(Updatemrs);

            var mrsUpdateService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var mrsUpdateResponse = mrsUpdateService.CreateItem(EP_updateMRS, Updatepayload);

            Assert.AreEqual("Request has been updated.", mrsUpdateResponse.message);
            var getItemService3 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse3 = getItemService3.GetItem(EP_getMRSDetails + "?ID=" + mrsId);
            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_SUBMITTED, mrsResponse3.status);

            Assert.AreEqual(mrsId, mrsResponse3.ID);
            Assert.AreEqual(Updatemrs.to_actor_id, mrsResponse3.to_actor_id);
            Assert.AreEqual(Updatemrs.to_actor_type_id, mrsResponse3.to_actor_type_id);
            Assert.AreEqual(Updatemrs.from_actor_id, mrsResponse3.from_actor_id);
            Assert.AreEqual(Updatemrs.from_actor_type_id, mrsResponse3.from_actor_type_id);
            Assert.AreEqual(Updatemrs.activity, mrsResponse3.activity);
            //Assert.AreEqual(Updatemrs.facilityId, mrsResponse3.facilityId);
            Assert.AreEqual(Updatemrs.remarks, mrsResponse3.remarks);
            Assert.AreEqual(Updatemrs.whereUsedRefID, mrsResponse3.whereUsedRefID);
            //Assert.AreEqual(createmrs.whereUsedType, mrsResponse.whereUsedType);


            Assert.AreEqual(Updatemrs.CMMRSItems.Count, mrsResponse3.CMMRSItems.Count);
            for (int i = 0; i < Updatemrs.CMMRSItems.Count; i++)
            {
                var expectedItem = Updatemrs.CMMRSItems[i];
                var actualItem = mrsResponse3.CMMRSItems[i];

                Assert.AreEqual(expectedItem.issued_qty, actualItem.issued_qty);
                Assert.AreEqual(expectedItem.requested_qty, actualItem.requested_qty);
                Assert.AreEqual(expectedItem.asset_code, actualItem.asset_code);
                Assert.AreEqual(expectedItem.asset_type_ID, actualItem.asset_type_ID);
                Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_item_ID);
                Assert.AreEqual(expectedItem.available_qty, actualItem.available_qty);

            }



            //STEP 4: Approve MRS
            string approvemrs = $@"{{
                                ""ID"":{mrsId},
                                ""comment"": ""MRS approval for MRS""
                         
                        }}";
            var mrsApproveService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Approveresponse = mrsApproveService.CreateItem(EP_mrsApproval, approvemrs);
            Assert.AreEqual("Status updated.", Approveresponse.message);

            var getItemService4 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse4 = getItemService4.GetItem(EP_getMRSDetails + "?ID=" + mrsId);
            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED, mrsResponse4.status);

            Assert.AreEqual(mrsResponse4.approver_name, "Admin HFE");
            DateTime expectedGeneratedAt11 = DateTime.Now;
            DateTime actualGeneratedAt22 = DateTime.Parse(mrsResponse4.approval_date);

            expectedGeneratedAt11 = new DateTime(expectedGeneratedAt11.Year, expectedGeneratedAt11.Month, expectedGeneratedAt11.Day, expectedGeneratedAt11.Hour, expectedGeneratedAt11.Minute, 0);
            actualGeneratedAt22 = new DateTime(actualGeneratedAt22.Year, actualGeneratedAt22.Month, actualGeneratedAt22.Day, actualGeneratedAt22.Hour, actualGeneratedAt22.Minute, 0);

            Assert.AreEqual(expectedGeneratedAt11, actualGeneratedAt22, "The MRS approved timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);



            //STEP 5: MRS Issue
            var mrsIssuePayload = new CMMSAPIs.Models.SM.CMMRSList
            {
                ID = mrsId,
                CMMRSItems = new List<CMMSAPIs.Models.SM.CMMRSItems>
            {
                new CMMSAPIs.Models.SM.CMMRSItems
                {
                    mrs_item_id = 551,
                    issued_qty = 1,
                    asset_item_ID = 211,
                    serial_number = ""
                }
            }
            };
            string Issuepayload = JsonConvert.SerializeObject(mrsIssuePayload);
            var mrsIssueService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Issueresponse = mrsIssueService.CreateItem(EP_CreateMRSIssue, Issuepayload);
            Assert.AreEqual("MRS Request Issued.", Issueresponse.message);

            var getItem5 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse5 = getItem5.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED, mrsResponse5.status);

            Assert.AreEqual(mrsResponse5.issued_name, "Admin HFE");  //property added in model class
            DateTime expectedGeneratedAt5 = DateTime.Now;
            DateTime actualGeneratedAt5 = DateTime.Parse(mrsResponse5.issued_date);

            expectedGeneratedAt5 = new DateTime(expectedGeneratedAt5.Year, expectedGeneratedAt5.Month, expectedGeneratedAt5.Day, expectedGeneratedAt5.Hour, expectedGeneratedAt5.Minute, 0);
            actualGeneratedAt5 = new DateTime(actualGeneratedAt5.Year, actualGeneratedAt5.Month, actualGeneratedAt5.Day, actualGeneratedAt5.Hour, actualGeneratedAt5.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt5, actualGeneratedAt5, "The issued timestamp should match.");

            Assert.AreEqual(mrsId, response.id[0]);




            //STEP 6: MRS Issue
            var IssueApprovePayload = new CMMSAPIs.Models.SM.CMMRSList
            {
                ID = mrsId,
                CMMRSItems = new List<CMMSAPIs.Models.SM.CMMRSItems>
            {
                new CMMSAPIs.Models.SM.CMMRSItems
                {
                    mrs_item_id = 551,
                    issued_qty = 1,
                    asset_item_ID = 211,
                    serial_number = ""
                }
            }
            };
            string IssueApprovepayload = JsonConvert.SerializeObject(IssueApprovePayload);
            var issueApproveService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var IssueApproveresponse = issueApproveService.CreateItem(EP_ApproveMRSIssue, IssueApprovepayload);
            Assert.AreEqual("Status updated.", IssueApproveresponse.message);

            var getItem6 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var mrsResponse6 = getItem6.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED, mrsResponse6.status);

            Assert.AreEqual(mrsResponse6.issue_approved_by_name, "Admin HFE");   
            DateTime expectedGeneratedAt6 = DateTime.Now;
            DateTime actualGeneratedAt6 = (DateTime)mrsResponse6.issue_approved_date;

            expectedGeneratedAt6 = new DateTime(expectedGeneratedAt6.Year, expectedGeneratedAt6.Month, expectedGeneratedAt6.Day, expectedGeneratedAt6.Hour, expectedGeneratedAt6.Minute, 0);
            actualGeneratedAt6 = new DateTime(actualGeneratedAt6.Year, actualGeneratedAt6.Month, actualGeneratedAt6.Day, actualGeneratedAt6.Hour, actualGeneratedAt6.Minute, 0);

            Assert.AreEqual(expectedGeneratedAt6, actualGeneratedAt6, "The issue approved timestamp should match.");

        }




        //◆◇◆◆◆ MRS FUNCTIONAL TEST◆◆◆◆◇◆
        [TestMethod]
        public void VerifyFunctionalTestReturnMRS()
        {
            // Step 1: Create Return MRS
            var returnPayload = new CMMSAPIs.Models.SM.CMMRS
            {
                ID = 0,
                to_actor_type_id = 2,
                from_actor_type_id = 4,
                to_actor_id = 1,
                from_actor_id = 15,
                //facilityId = 1,
                activity = "job test 123",
                whereUsedRefID = 15,
                //whereUsedType = 4,
                //setAsTemplate = "",
                remarks = "aditya",
                faultyItems = new List<CMFaultyItems>
            {
            new CMMSAPIs.Models.SM.CMFaultyItems
            {
                assetMasterItemID = 12,
                serial_number = "ss31",
                return_remarks = "remark of faulty",
                returned_qty = 1,
                faulty_item_asset_id = 4,
                mrs_item_ID = 0
            }
            },
                cmmrsItems = new List<CMEquipments>
            {
            new CMMSAPIs.Models.SM.CMEquipments
            {
                mrs_item_id = 210,
                return_remarks = "returned unused",
                returned_qty = 1
            }
            }
            };

            string createPayload = JsonConvert.SerializeObject(returnPayload);
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var responseCreate = mrsService.CreateItem(EP_CreateReturnMRS, createPayload);
            int RmrsId = responseCreate.id[0];
            Assert.AreEqual("MRS return submitted.", responseCreate.message);
            var getItem1 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse1 = getItem1.GetItem(EP_getReturnDataByID + "?ID=" + RmrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_SUBMITTED, mrsResponse1.status);




            // Step 2: Reject the created MRS
            string rejectPayload = $@"{{
                                ""ID"":{RmrsId},
                                ""comment"": "" Return MRS approval""
                         
                        }}";

            var responseReject = mrsService.CreateItem(EP_RejectMRSReturn, rejectPayload);
            Assert.AreEqual("MRS Return Rejected. Reason :  Return MRS approval", responseReject.message);
            var getItem2 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse2 = getItem2.GetItem(EP_getReturnDataByID + "?ID=" + RmrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED, mrsResponse2.status);




            // Step 3: Update the same MRS (after rejection)
            var UpdatereturnPayload = new CMMSAPIs.Models.SM.CMMRS
            {
                ID = RmrsId,
                to_actor_type_id = 2,
                from_actor_type_id = 4,
                to_actor_id = 1,
                from_actor_id = 15,
                //facilityId = 1,
                activity = "adi test",
                whereUsedRefID = 15,
                //whereUsedType = 4,
                //setAsTemplate = "",
                remarks = "aditya",
                faultyItems = new List<CMFaultyItems>
            {
            new CMMSAPIs.Models.SM.CMFaultyItems
            {
                assetMasterItemID = 12,
                serial_number = "ss31",
                return_remarks = "remark of faulty",
                returned_qty = 1,
                faulty_item_asset_id = 4,
                mrs_item_ID = 0
            }
            },
                cmmrsItems = new List<CMEquipments>
            {
            new CMMSAPIs.Models.SM.CMEquipments
            {
                mrs_item_id = 210,
                return_remarks = "returned unused",
                returned_qty = 1
            }
            }
            };
            string updatePayload = JsonConvert.SerializeObject(UpdatereturnPayload);
            var responseUpdate = mrsService.CreateItem(EP_UpdateReturnMRS, updatePayload);
            Assert.AreEqual("MRS return submitted.", responseUpdate.message);
            var getItem3 = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse3 = getItem3.GetItem(EP_getReturnDataByID + "?ID=" + RmrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_SUBMITTED, mrsResponse3.status);



            // Step 4: Approve the same MRS (after update)
            string approvePayload = $@"{{
                                ""ID"":{RmrsId},
                                ""comment"": "" Return MRS approval""
                         
                        }}";

           
            var responseApprove = mrsService.CreateItem(EP_ApproveMRSReturn, approvePayload);
            Assert.AreEqual("Equipment returned to store.", responseApprove.message);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSReturnList>();
            var mrsResponse = getItem.GetItem(EP_getReturnDataByID + "?ID=" + RmrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED, mrsResponse.status);
            Assert.AreEqual(mrsResponse.approver_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.approved_date;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The Return MRS approved timestamp should match.");
        }


    }
}
