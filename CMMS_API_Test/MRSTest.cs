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
        [TestMethod]
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
            Assert.AreEqual(expectedCMMRS.from_actor_type_id, mrsResponse);
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
                /*Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_code);
                Assert.AreEqual(expectedItem.asset_type_ID, actualItem.asset_type_ID);*/
                Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_item_ID);
                Assert.AreEqual(expectedItem.asset_item_ID, actualItem.asset_item_ID);
                Assert.AreEqual(expectedItem.available_qty, actualItem.available_qty);
                Assert.AreEqual(expectedItem.serial_number, actualItem.serial_number);
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
            Assert.AreEqual(205, mrsResponse.to_actor_id);
            Assert.AreEqual(3, mrsResponse.to_actor_type_id);
            Assert.AreEqual(1, mrsResponse.from_actor_id);
            Assert.AreEqual(2, mrsResponse.from_actor_type_id);
            Assert.AreEqual("Switchgear Quarterly Check Updated", mrsResponse.activity);
            //Assert.AreEqual(1, mrsResponse.facility_ID);
            Assert.AreEqual("Updated Remarks", mrsResponse.remarks);
            //Assert.AreEqual(27, mrsResponse.whereUsedTypeName);
            Assert.AreEqual(205, mrsResponse.whereUsedRefID);

            // Compare the first item in cmmrsItems
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].issued_qty);
            Assert.AreEqual(2, mrsResponse.CMMRSItems[0].requested_qty);
            Assert.AreEqual("S423741003", mrsResponse.CMMRSItems[0]);
            //Assert.AreEqual(1, item1.asset_type_ID);
            Assert.AreEqual(83, mrsResponse.CMMRSItems[0].asset_item_ID);
            Assert.AreEqual(83, mrsResponse.CMMRSItems[0].asset_item_ID);
            Assert.AreEqual(4, mrsResponse.CMMRSItems[0].available_qty);

            // Compare the second item in cmmrsItems
            var item2 = mrsResponse.CMMRSItems[1];
            Assert.AreEqual(1, item2.issued_qty);
            Assert.AreEqual(1, item2.requested_qty);
            //Assert.AreEqual("S071804001", item2.asset_code);
            //Assert.AreEqual(2, item2.asset_type_ID);
            Assert.AreEqual(121, item2.asset_item_ID);
            Assert.AreEqual(121, item2.asset_item_ID);
            Assert.AreEqual(1, item2.available_qty);
            Assert.IsNull(item2.serial_number);
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
                                ""ID"":201,
                                ""comment"": ""MRS approval for MRS 131""
                         
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
                                ""ID"":180,
                                ""comment"": ""MRS reject for MRS 131""
                        }";
            var mrsService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = mrsService.CreateItem(EP_mrsReject, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);

            int mrsId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var mrsResponse = getItem.GetItem(EP_getMRSDetails + "?ID=" + mrsId);

            Assert.AreEqual((int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED, mrsResponse.status);

            Assert.AreEqual(mrsResponse.request_rejected_by_name, "Admin HFE");  //property added in model class
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.request_rejected_at;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The MRS rejected timestamp should match.");

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

            Assert.AreEqual(mrsResponse.issued_name, "Admin HFE");  //property added in model class
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

            Assert.AreEqual(mrsResponse.issue_approved_by_name, "Admin HFE");    //property added in model class and notification object
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.issue_approved_date;


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);


            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The issue approved timestamp should match.");

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

            Assert.AreEqual(mrsResponse.issue_rejected_by_name, "Admin HFE");    //property added in model class 
            DateTime expectedGeneratedAt = DateTime.Now;
            DateTime actualGeneratedAt = (DateTime)mrsResponse.issue_rejected_date; //property added in model class


            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, expectedGeneratedAt.Hour, expectedGeneratedAt.Minute, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, actualGeneratedAt.Hour, actualGeneratedAt.Minute, 0);

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The issue rejected timestamp should match.");

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
                                ""ID"": 39,
                                ""to_actor_type_id"": 1,
                                ""from_actor_type_id"": 1,
                                ""to_actor_id"": 13,
                                ""from_actor_id"": 1779,
                                ""facility_ID"": 1779,
                                ""whereUsedRefID"": 17,
                                ""whereUsedType"": 27,
                                ""setAsTemplate"": """",
                                ""remarks"": """",
                                ""activity"":""testActivity"",
                                ""cmmrsItems"": [
                                    {
                                        ""asset_item_ID"": 10,
                                        ""issued_qty"": 1,
                                        ""requested_qty"": 1,
                                        ""returned_qty"": 1,
                                        ""approval_required"": 0,
                                        ""is_faulty"": 0,
                                        ""return_remarks"": ""return""
                                    },
                                    {
                                        ""asset_item_ID"": 64,
                                        ""issued_qty"": 8,
                                        ""requested_qty"": 10,
                                        ""returned_qty"": 8,
                                        ""approval_required"": 0,
                                        ""is_faulty"": 0,
                                        ""return_remarks"": ""return consumable""
                                    }
                                ],
                                ""faultyItems"":[
                                    {
                                        ""mrsItemID"": 128,
                                        ""assetMasterItemID"": 10,
                                        ""returned_qty"": 1,
                                        ""return_remarks"": ""return faulty"",
                                        ""sr_no"": ""99125""
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

            Assert.AreEqual(RmrsId, mrsResponse.ID, "The MRS ID should match the input.");
            Assert.AreEqual("testActivity", mrsResponse.activity, "The activity field should match.");
            Assert.AreEqual(2, mrsResponse.to_actor_type_id, "The to_actor_type_id should match.");
            Assert.AreEqual(3, mrsResponse.from_actor_type_id, "The from_actor_type_id should match.");
            Assert.AreEqual(65, mrsResponse.to_actor_id, "The to_actor_id should match.");
            Assert.AreEqual(1779, mrsResponse.from_actor_id, "The from_actor_id should match.");
            Assert.AreEqual(1779, mrsResponse.facilityId, "The facility_ID should match.");
            Assert.AreEqual(17, mrsResponse.whereUsedRefID, "The whereUsedRefID should match.");
            Assert.AreEqual(27, mrsResponse.whereUsedTypeName, "The whereUsedType should match.");
            //Assert.AreEqual("", mrsResponse, "The setAsTemplate should match.");
            Assert.AreEqual("", mrsResponse.remarks, "The remarks should match.");

            // Validate cmmrsItems
            Assert.AreEqual(2, mrsResponse.CMMRSItems.Count, "The number of cmmrsItems should match.");
            Assert.AreEqual(10, mrsResponse.CMMRSItems[0].asset_item_ID, "The asset_item_ID of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].issued_qty, "The issued_qty of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].requested_qty, "The requested_qty of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].returned_qty, "The returned_qty of the first item should match.");
            //Assert.AreEqual(0, mrsResponse.cmmrsItems[0], "The approval_required of the first item should match.");
            Assert.AreEqual(0, mrsResponse.CMMRSItems[0].is_faulty, "The is_faulty of the first item should match.");
            Assert.AreEqual("return", mrsResponse.CMMRSItems[0].return_remarks, "The return_remarks of the first item should match.");

            Assert.AreEqual(64, mrsResponse.CMMRSItems[1].asset_item_ID, "The asset_item_ID of the second item should match.");
            Assert.AreEqual(8, mrsResponse.CMMRSItems[1].issued_qty, "The issued_qty of the second item should match.");
            Assert.AreEqual(10, mrsResponse.CMMRSItems[1].requested_qty, "The requested_qty of the second item should match.");
            Assert.AreEqual(8, mrsResponse.CMMRSItems[1].returned_qty, "The returned_qty of the second item should match.");
            //Assert.AreEqual(0, mrsResponse.cmmrsItems[1].approval_required, "The approval_required of the second item should match.");
            Assert.AreEqual(0, mrsResponse.CMMRSItems[1].is_faulty, "The is_faulty of the second item should match.");
            Assert.AreEqual("return consumable", mrsResponse.CMMRSItems[1].return_remarks, "The return_remarks of the second item should match.");

            // Validate faultyItems
            Assert.AreEqual(1, mrsResponse.CMMRSFaultyItems.Count, "The number of faultyItems should match.");
            Assert.AreEqual(128, mrsResponse.CMMRSFaultyItems[0].faulty_item_asset_id, "The mrsItemID of the faulty item should match.");
            Assert.AreEqual(10, mrsResponse.CMMRSFaultyItems[0].assetMasterID, "The assetMasterItemID of the faulty item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSFaultyItems[0].returned_qty, "The returned_qty of the faulty item should match.");
            Assert.AreEqual("return faulty", mrsResponse.CMMRSFaultyItems[0].return_remarks, "The return_remarks of the faulty item should match.");
            Assert.AreEqual("99125", mrsResponse.CMMRSFaultyItems[0].serial_number, "The sr_no of the faulty item should match.");
        }


        [TestMethod]
        public void VerifyUpdateReturnMRS()
        {
            string payload = @"{
                                ""ID"": 39,
                                ""to_actor_type_id"": 1,
                                ""from_actor_type_id"": 1,
                                ""to_actor_id"": 13,
                                ""from_actor_id"": 1779,
                                ""facility_ID"": 1779,
                                ""whereUsedRefID"": 17,
                                ""whereUsedType"": 27,
                                ""setAsTemplate"": """",
                                ""remarks"": """",
                                ""activity"":""testActivity"",
                                ""cmmrsItems"": [
                                    {
                                        ""asset_item_ID"": 10,
                                        ""issued_qty"": 1,
                                        ""requested_qty"": 1,
                                        ""returned_qty"": 1,
                                        ""approval_required"": 0,
                                        ""is_faulty"": 0,
                                        ""return_remarks"": ""return""
                                    },
                                    {
                                        ""asset_item_ID"": 64,
                                        ""issued_qty"": 8,
                                        ""requested_qty"": 10,
                                        ""returned_qty"": 8,
                                        ""approval_required"": 0,
                                        ""is_faulty"": 0,
                                        ""return_remarks"": ""return consumable""
                                    }
                                ],
                                ""faultyItems"":[
                                    {
                                        ""mrsItemID"": 128,
                                        ""assetMasterItemID"": 10,
                                        ""returned_qty"": 1,
                                        ""return_remarks"": ""return faulty"",
                                        ""sr_no"": ""99125""
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
            Assert.AreEqual("testActivity", mrsResponse.activity, "The activity field should match.");
            Assert.AreEqual(2, mrsResponse.to_actor_type_id, "The to_actor_type_id should match.");
            Assert.AreEqual(3, mrsResponse.from_actor_type_id, "The from_actor_type_id should match.");
            Assert.AreEqual(65, mrsResponse.to_actor_id, "The to_actor_id should match.");
            Assert.AreEqual(1779, mrsResponse.from_actor_id, "The from_actor_id should match.");
            Assert.AreEqual(1779, mrsResponse.facilityId, "The facility_ID should match.");
            Assert.AreEqual(17, mrsResponse.whereUsedRefID, "The whereUsedRefID should match.");
            Assert.AreEqual(27, mrsResponse.whereUsedTypeName, "The whereUsedType should match.");
            //Assert.AreEqual("", mrsResponse, "The setAsTemplate should match.");
            Assert.AreEqual("", mrsResponse.remarks, "The remarks should match.");

            // Validate cmmrsItems
            Assert.AreEqual(2, mrsResponse.CMMRSItems.Count, "The number of cmmrsItems should match.");
            Assert.AreEqual(10, mrsResponse.CMMRSItems[0].asset_item_ID, "The asset_item_ID of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].issued_qty, "The issued_qty of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].requested_qty, "The requested_qty of the first item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSItems[0].returned_qty, "The returned_qty of the first item should match.");
            //Assert.AreEqual(0, mrsResponse.cmmrsItems[0], "The approval_required of the first item should match.");
            Assert.AreEqual(0, mrsResponse.CMMRSItems[0].is_faulty, "The is_faulty of the first item should match.");
            Assert.AreEqual("return", mrsResponse.CMMRSItems[0].return_remarks, "The return_remarks of the first item should match.");

            Assert.AreEqual(64, mrsResponse.CMMRSItems[1].asset_item_ID, "The asset_item_ID of the second item should match.");
            Assert.AreEqual(8, mrsResponse.CMMRSItems[1].issued_qty, "The issued_qty of the second item should match.");
            Assert.AreEqual(10, mrsResponse.CMMRSItems[1].requested_qty, "The requested_qty of the second item should match.");
            Assert.AreEqual(8, mrsResponse.CMMRSItems[1].returned_qty, "The returned_qty of the second item should match.");
            //Assert.AreEqual(0, mrsResponse.cmmrsItems[1].approval_required, "The approval_required of the second item should match.");
            Assert.AreEqual(0, mrsResponse.CMMRSItems[1].is_faulty, "The is_faulty of the second item should match.");
            Assert.AreEqual("return consumable", mrsResponse.CMMRSItems[1].return_remarks, "The return_remarks of the second item should match.");

            // Validate faultyItems
            Assert.AreEqual(1, mrsResponse.CMMRSFaultyItems.Count, "The number of faultyItems should match.");
            Assert.AreEqual(128, mrsResponse.CMMRSFaultyItems[0].faulty_item_asset_id, "The mrsItemID of the faulty item should match.");
            Assert.AreEqual(10, mrsResponse.CMMRSFaultyItems[0].assetMasterID, "The assetMasterItemID of the faulty item should match.");
            Assert.AreEqual(1, mrsResponse.CMMRSFaultyItems[0].returned_qty, "The returned_qty of the faulty item should match.");
            Assert.AreEqual("return faulty", mrsResponse.CMMRSFaultyItems[0].return_remarks, "The return_remarks of the faulty item should match.");
            Assert.AreEqual("99125", mrsResponse.CMMRSFaultyItems[0].serial_number, "The sr_no of the faulty item should match.");
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

            var faultyItem = mrsResponse.faultyItems[0];
            Assert.AreEqual(80, faultyItem.returned_qty);
            //Assert.AreEqual(0, faultyItem.requested_qty);
            Assert.AreEqual(2, faultyItem.mrs_item_ID);
            //Assert.AreEqual(1, faultyItem.is_faulty);
            Assert.AreEqual("return faulty", faultyItem.return_remarks);
            Assert.AreEqual(2, faultyItem.returned_qty);
        }
    }
}
