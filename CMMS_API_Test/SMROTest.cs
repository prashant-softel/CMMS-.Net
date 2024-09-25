using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMMS_API_Test
{
    [TestClass]
    public class SMROTest
    {

        string EP_CreateRequestOrder = "/api/RequestOrder/CreateRequestOrder";
        string EP_UpdateRequestOrder = "/api/RequestOrder/UpdateRequestOrder";
        string EP_GetRequestOrderList = "/api/RequestOrder/GetRequestOrderList";
        string EP_GetRODetailsByID = "/api/RequestOrder/GetRODetailsByID";
        string EP_ApproveRequestOrder = "/api/RequestOrder/ApproveRequestOrder";
        string EP_RejectRequestOrder = "/api/RequestOrder/RejectRequestOrder";
        string EP_CloseRequestOrder = "/api/RequestOrder/CloseRequestOrder";
        string EP_DeleteRequestOrder = "/api/RequestOrder/DeleteRequestOrder";






        [TestMethod]
        public void VerifygetROList()
        {
            int facilityID = 1;
            string fromDate = "2024-09-09";
            string toDate = "2024-09-16";
            
            var roService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrder>();
            var response = roService.GetItemList(EP_GetRequestOrderList + "?facilityID=" + facilityID + "&fromDate=" + fromDate + "&toDate=" + toDate);


            /*var expectedItemCount = 6;
            var actualItemCount = response.Count;
            Assert.AreEqual(expectedItemCount, actualItemCount, "The number of items returned is not as expected.");*/

            Assert.IsNotNull(response, "Request Order list should not be null.");
            Assert.IsTrue(response.Count > 0, "Request Order list should contain at least one order.");
            
        }

        [TestMethod]
        public void VerifygetRODetailsById()
        {
            int IDs = 68;
            int facility_id = 1;
            var roService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrder>();
            var response = roService.GetItemList(EP_GetRODetailsByID + "?IDs=" + IDs + "&facility_id=" + facility_id);
            Assert.IsNotNull(response, "The response should not be null.");

            var expectedItemCount = 2;
            var actualItemCount = response.Count;
            Assert.AreEqual(expectedItemCount, actualItemCount, "The number of items returned is not as expected.");

            Assert.IsNotNull(response, "Request Order list should not be null.");
            Assert.IsTrue(response.Count > 0, "Request Order list should contain at least one order.");

           
            var firstOrder = response.FirstOrDefault();

            Assert.AreEqual(IDs, firstOrder.request_order_id, "The returned request_order_id should match the expected ID.");
        }

        [TestMethod]
        public void VerifyCreateRO()
        {
            
            string payload = @"{
                          ""facilityID"": 1,
                          ""request_order_items"": [
                              {
                                  ""currencyId"": 4,
                                  ""itemID"": 0,
                                  ""assetMasterItemID"": 12,
                                  ""cost"": 4334,
                                  ""ordered_qty"": 67,
                                  ""comment"": ""test""
                              },
                              {
                                  ""currencyId"": 2,
                                  ""itemID"": 0,
                                  ""assetMasterItemID"": 31,
                                  ""cost"": 6765,
                                  ""ordered_qty"": 23,
                                  ""comment"": ""rreere""
                              }
                          ],
                          ""comment"": ""test"",
                          ""request_order_id"": 0
                      }";

           
            var roService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = roService.CreateItem(EP_CreateRequestOrder, payload);

            string actualMessage = response.message;

            string expectedMessage = $"Request order created successfully.";

            Assert.AreEqual(expectedMessage, actualMessage);

            var goId = response.id[0];
            var facility_id = 1;

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + goId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMITTED, updatedRO[0].status);

            Assert.AreEqual(updatedRO[0].generatedBy, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)updatedRO[0].generatedAt;

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The submitted timestamp should be the same");

            Assert.AreEqual(11099, updatedRO[0].cost);

            Assert.AreEqual("test", updatedRO[0].comment);

            Assert.AreEqual(4, updatedRO[0].request_order_items[0].currencyId);
            Assert.AreEqual(12, updatedRO[0].request_order_items[0].id);
            Assert.AreEqual(4334, updatedRO[0].request_order_items[0].cost);
            Assert.AreEqual(67, updatedRO[0].request_order_items[0].ordered_qty);
            Assert.AreEqual("test", updatedRO[0].request_order_items[0].comment);

            Assert.AreEqual(2, updatedRO[0].request_order_items[1].currencyId);
            Assert.AreEqual(31, updatedRO[0].request_order_items[1].id);
            Assert.AreEqual(6765, updatedRO[0].request_order_items[1].cost);
            Assert.AreEqual(23, updatedRO[0].request_order_items[1].ordered_qty);
            Assert.AreEqual("rreere", updatedRO[0].request_order_items[1].comment);
        }

        [TestMethod]
        public void VerifyupdateRO()
        {

            string payload = @"{
                          ""facilityID"": 1,
                          ""request_order_items"": [
                              {
                                  ""currencyId"": 4,
                                  ""itemID"": 0,
                                  ""assetMasterItemID"": 12,
                                  ""cost"": 4334,
                                  ""ordered_qty"": 67,
                                  ""comment"": ""test""
                              },
                              {
                                  ""currencyId"": 2,
                                  ""itemID"": 0,
                                  ""assetMasterItemID"": 31,
                                  ""cost"": 6765,
                                  ""ordered_qty"": 23,
                                  ""comment"": ""rreere""
                              }
                          ],
                          ""comment"": ""test"",
                          ""request_order_id"": 83
                      }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateRequestOrder, payload);

            int goId = response.id[0];
            string actualMessage = response.message;

            string expectedMessage = "Request order updated successfully.";

            Assert.AreEqual(expectedMessage, actualMessage);
            Assert.AreEqual(83, response.id[0]);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + goId);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED, updatedRO[0].status);
            
            Assert.AreEqual(1, updatedRO[0].facilityID, "FacilityID should be 1");
            Assert.AreEqual("test", updatedRO[0].comment, "Comment should be 'test'");

            // Verify the first item in the request_order_items
            Assert.AreEqual(12, updatedRO[0].request_order_items[0].id, "AssetMasterItemID for the first item should be 12");
            Assert.AreEqual(4334, updatedRO[0].request_order_items[0].cost, "Cost for the first item should be 4334");
            Assert.AreEqual(67, updatedRO[0].request_order_items[0].ordered_qty, "Ordered quantity for the first item should be 67");
            Assert.AreEqual("test", updatedRO[0].request_order_items[0].comment, "Comment for the first item should be 'test'");

            // Verify the second item in the request_order_items
            Assert.AreEqual(31, updatedRO[0].request_order_items[1].id, "AssetMasterItemID for the second item should be 31");
            Assert.AreEqual(6765, updatedRO[0].request_order_items[1].cost, "Cost for the second item should be 6765");
            Assert.AreEqual(23, updatedRO[0].request_order_items[1].ordered_qty, "Ordered quantity for the second item should be 23");
            Assert.AreEqual("rreere", updatedRO[0].request_order_items[1].comment, "Comment for the second item should be 'rreere'");
        }

        [TestMethod]
        public void VerifyApproveRO()
        {
            string payload = @"{
                                ""id"": 75,
                                ""comment"": ""test"",
                                ""facilityId"": 1
                                 }";


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_ApproveRequestOrder, payload);

            int roId = response.id[0];

            string expectedMessage = $"Approved request order  {roId}  successfully.";

            Assert.AreEqual(expectedMessage, response.message);
            var facility_id = 1;

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var RO_obj = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED, RO_obj[0].status);

            Assert.AreEqual(RO_obj[0].approvedBy, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)RO_obj[0].approvedAt;

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The approved timestamp should be the same");

            Assert.AreEqual(75, response.id[0]);
        }


        [TestMethod]
        public void VerifyRejectRO()
        {
            string payload = @"{
                                ""id"": 86,
                                ""comment"": ""test reject api"",
                                ""facilityId"": 1
                                 }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_RejectRequestOrder, payload);

            string expectedMessage = $"Rejected request order.";

            Assert.AreEqual(expectedMessage, response.message);
            int roId = response.id[0];
            var facility_id = 1;

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var RO_obj = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED, RO_obj[0].status);

            Assert.AreEqual(RO_obj[0].rejectedBy, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)RO_obj[0].rejectedAt;

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The rejected timestamp should be the same");

            Assert.AreEqual(86, response.id[0]);
        }

        [TestMethod]
        public void VerifyCloseRO()
        {
            string payload = @"{
                                ""id"": 86,
                                ""comment"": ""test close api"",
                                ""facilityId"": 1
                                 }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_CloseRequestOrder, payload);
            int roId = response.id[0];
            
            string expectedMessage = $"Request order {{{roId}}} closed.";  

            Console.WriteLine($"Actual Response: '{response.message}'");

            // Trim both strings to remove any extra spaces
            Assert.AreEqual(expectedMessage.Trim(), response.message.Trim(), ignoreCase: true, "The expected message does not match the actual response.");

            var facility_id = 1;

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var RO_obj = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_CLOSED, RO_obj[0].status);

            /*Assert.AreEqual(updatedRO[0].closed_by, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            string actualGeneratedAt = updatedRO[0].closed_at;*/

            //Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The closed timestamp should be the same");

            Assert.AreEqual(86, response.id[0]);
        }

        [TestMethod]
        public void VerifyDeleteRO()
        {
            string payload = @"{
                                ""id"": 86,
                                ""comment"": ""deleted"",
                                ""facilityId"": 1
                                 }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_DeleteRequestOrder, payload);
            int roId = response.id[0];

            string expectedMessage = $"Request order deleted.";
            string actualMessage = response.message;
            Assert.AreEqual(expectedMessage, actualMessage);

            var facility_id = 1;

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var RO_obj = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_DELETED, RO_obj[0].status);

            /*Assert.AreEqual(updatedRO[0].closed_by, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            string actualGeneratedAt = updatedRO[0].closed_at;*/

            //Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The deleted timestamp should be the same");

            Assert.AreEqual(86, response.id[0]);
        }

        [TestClass]
        public class RequestOrderFunctionalTest
        {

            string EP_CreateRequestOrder = "/api/RequestOrder/CreateRequestOrder";
            string EP_UpdateRequestOrder = "/api/RequestOrder/UpdateRequestOrder";
            string EP_GetRequestOrderList = "/api/RequestOrder/GetRequestOrderList";
            string EP_GetRODetailsByID = "/api/RequestOrder/GetRODetailsByID";
            string EP_ApproveRequestOrder = "/api/RequestOrder/ApproveRequestOrder";
            string EP_RejectRequestOrder = "/api/RequestOrder/RejectRequestOrder";
            string EP_CloseRequestOrder = "/api/RequestOrder/CloseRequestOrder";
            string EP_DeleteRequestOrder = "/api/RequestOrder/DeleteRequestOrder";



            //◆◇◆◆◆ To check Appproval flow of Request Order ◆◆◆◆◇◆
            [TestMethod]
            public void VerifyApproveROFlow()
            {
                // Step 1: Create Request Order
                var createPayload = @"{
                                ""facilityID"": 1,
                                ""request_order_items"": [
                                    {
                                        ""currencyId"": 4,
                                        ""itemID"": 0,
                                        ""assetMasterItemID"": 12,
                                        ""cost"": 4334,
                                        ""ordered_qty"": 67,
                                        ""comment"": ""test""
                                    },
                                    {
                                        ""currencyId"": 2,
                                        ""itemID"": 0,
                                        ""assetMasterItemID"": 31,
                                        ""cost"": 6765,
                                        ""ordered_qty"": 23,
                                        ""comment"": ""rreere""
                                    }
                                ],
                                ""comment"": ""Initial Order Creation"",
                                ""request_order_id"": 0
                             }";

                var roService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
                var createResponse = roService.CreateItem(EP_CreateRequestOrder, createPayload);

                int createdROId = createResponse.id[0];
                Assert.AreEqual("Request order created successfully.", createResponse.message, "RO creation message mismatch");

                // Verify the created RO by fetching its details
                var getItemService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
                var createdRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + createdROId + "&facility_id = 1");

                Assert.AreEqual(1, createdRO[0].facilityID, "FacilityID should be 1 after creation.");
                Assert.AreEqual(createdROId, createdRO[0].request_order_id,"createdROId and Id getting from response should be same");
                Assert.AreEqual("Initial Order Creation", createdRO[0].comment);

                //RO Item 1
                Assert.AreEqual(12, createdRO[0].request_order_items[0].id, "Updated asset master id of the first item should be same");
                //Assert.AreEqual(281, createdRO[0].request_order_items[0].itemID, "Updated item id of the first item should be same");
                Assert.AreEqual(4, createdRO[0].request_order_items[0].currencyId, "Updated currency id of the first item should be same");
                Assert.AreEqual(4334, createdRO[0].request_order_items[0].cost, "Updated cost of the first item should be same");
                Assert.AreEqual(67, createdRO[0].request_order_items[0].ordered_qty, "Updated ordered_qty of the first item should be same");
                Assert.AreEqual("test", createdRO[0].request_order_items[0].comment, "RO Comment should match after update");

                //RO item 2
                Assert.AreEqual(31, createdRO[0].request_order_items[1].id, "Updated asset master id of the first item should be same");
                //Assert.AreEqual(282, createdRO[0].request_order_items[1].itemID, "Updated item id of the first item should be same");
                Assert.AreEqual(2, createdRO[0].request_order_items[1].currencyId, "Updated currency id of the first item should be same");
                Assert.AreEqual(6765, createdRO[0].request_order_items[1].cost, "Updated cost of the first item should be same");
                Assert.AreEqual(23, createdRO[0].request_order_items[1].ordered_qty, "Updated ordered_qty of the first item should be same");
                Assert.AreEqual("rreere", createdRO[1].request_order_items[1].comment, "RO Comment should match after update");




                // Step 2: Update Request Order
                // We need to capture the itemIDs from the created RO
                int firstItemId = (int)createdRO[0].request_order_items[0].itemID;
                int secondItemId = (int)createdRO[0].request_order_items[1].itemID;

                var updatePayload = @"{
                            ""facilityID"": 1,
                            ""request_order_items"": [
                                {
                                    ""currencyId"": 69,
                                    ""itemID"": " + firstItemId + @",
                                    ""assetMasterItemID"": 12,
                                    ""cost"": 5000,
                                    ""ordered_qty"": 70,
                                    ""comment"": ""updated item 1""
                                },
                                {
                                    ""currencyId"": 69,
                                    ""itemID"": " + secondItemId + @",
                                    ""assetMasterItemID"": 31,
                                    ""cost"": 6000,
                                    ""ordered_qty"": 30,
                                    ""comment"": ""updated item 2""
                                }
                            ],
                            ""comment"": ""Updated Order"",
                            ""request_order_id"": " + createdROId + @"
                         }";

                var rouService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
                var response = rouService.CreateItem(EP_UpdateRequestOrder, updatePayload);

                if (response == null)
                {
                    Console.WriteLine("Response is null! Check the endpoint or request.");
                }
                else
                {
                    Assert.AreEqual("Request order updated successfully.", response.message, "RO update message mismatch");
                }

                // Verify the updated RO by fetching its details
                var updatedRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + createdROId + "&facility_id=1");

                if (updatedRO == null || updatedRO.Count == 0)
                {
                    Console.WriteLine("No data returned for updated RO. Possible issue with API or request.");
                }
                else
                {
                    Assert.AreEqual(1, updatedRO[0].facilityID, "FacilityID should be 1 after creation.");
                    Assert.AreEqual(createdROId, updatedRO[0].request_order_id, "createdROId and Id getting from response should be same");
                    Assert.AreEqual("Updated Order", updatedRO[0].comment);

                    //RO item 1
                    Assert.AreEqual(12, updatedRO[0].request_order_items[0].id, "Updated currency id of the first item should be same");
                    Assert.AreEqual(firstItemId, updatedRO[0].request_order_items[0].itemID, "Updated item id of the first item should be same");
                    Assert.AreEqual(69, updatedRO[0].request_order_items[0].currencyId, "Updated currency id of the first item should be same");
                    Assert.AreEqual(5000, updatedRO[0].request_order_items[0].cost, "Updated cost of the first item should be same");
                    Assert.AreEqual(70, updatedRO[0].request_order_items[0].ordered_qty, "Updated ordered_qty of the first item should be same");
                    Assert.AreEqual("updated item 1", updatedRO[0].request_order_items[0].comment, "RO Comment should match after update");

                    //RO item 2
                    Assert.AreEqual(31, updatedRO[0].request_order_items[1].id, "Updated currency id of the second item should be same");
                    Assert.AreEqual(secondItemId, updatedRO[0].request_order_items[1].itemID, "Updated item id of the second item should be same");
                    Assert.AreEqual(69, updatedRO[0].request_order_items[1].currencyId, "Updated currency id of the second item should be same");
                    Assert.AreEqual(6000, updatedRO[0].request_order_items[1].cost, "Updated cost of the second item should be same");
                    Assert.AreEqual(30, updatedRO[0].request_order_items[1].ordered_qty, "Updated ordered_qty of the second item should be same");
                    Assert.AreEqual("updated item 2", updatedRO[1].request_order_items[1].comment, "RO Comment should match after update");
                }



                //Step 3: Approve Request Order
                var approvePayload = @"{
                                  ""id"": " + createdROId + @",
                                  ""comment"": ""Approving the order"",
                                  ""facilityId"": 1
                               }";

                var approveResponse = roService.CreateItem(EP_ApproveRequestOrder, approvePayload);
                Assert.AreEqual($"Approved request order  {createdROId}  successfully.", approveResponse.message, "Approval message mismatch");

                // Verify the approved status
                var approvedRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + createdROId + "&facility_id=1");
                Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED, approvedRO[0].status);

                Assert.AreEqual(approvedRO[0].approvedBy, "Admin HFE");
                DateTime expectedGeneratedAt = DateTime.Today;
                DateTime actualGeneratedAt = (DateTime)approvedRO[0].approvedAt;

                Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The approved timestamp should be the same");

                Assert.AreEqual(createdROId, response.id[0],"id and reponse id should be match");

            }



            //◆◇◆◆◆ To check Rejection Flow of Request Order ◆◆◆◆◇◆
            [TestMethod]
            public void VerifyRejectROFlow()
            {
                // Step 1: Create Request Order
                var createPayload = @"{
                                ""facilityID"": 1,
                                ""request_order_items"": [
                                    {
                                        ""currencyId"": 67,
                                        ""itemID"": 0,
                                        ""assetMasterItemID"": 15,
                                        ""cost"": 1234,
                                        ""ordered_qty"": 45,
                                        ""comment"": ""test""
                                    },
                                    {
                                        ""currencyId"": 68,
                                        ""itemID"": 0,
                                        ""assetMasterItemID"": 30,
                                        ""cost"": 6789,
                                        ""ordered_qty"": 76,
                                        ""comment"": ""rreere""
                                    }
                                ],
                                ""comment"": ""Initial Order Creation"",
                                ""request_order_id"": 0
                             }";

                var roService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
                var createResponse = roService.CreateItem(EP_CreateRequestOrder, createPayload);

                int createdROId = createResponse.id[0];
                Assert.AreEqual("Request order created successfully.", createResponse.message, "RO creation message mismatch");

                // Verify the created RO by fetching its details
                var getItemService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
                var createdRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + createdROId + "&facility_id = 1");

                Assert.AreEqual(1, createdRO[0].facilityID, "FacilityID should be 1 after creation.");
                Assert.AreEqual(createdROId, createdRO[0].request_order_id, "createdROId and Id getting from response should be same");
                Assert.AreEqual("Initial Order Creation", createdRO[0].comment);

                //RO Item 1
                Assert.AreEqual(15, createdRO[0].request_order_items[0].id, "Updated asset master id of the first item should be same");
                //Assert.AreEqual(281, createdRO[0].request_order_items[0].itemID, "Updated item id of the first item should be same");
                Assert.AreEqual(67, createdRO[0].request_order_items[0].currencyId, "Updated currency id of the first item should be same");
                Assert.AreEqual(1234, createdRO[0].request_order_items[0].cost, "Updated cost of the first item should be same");
                Assert.AreEqual(45, createdRO[0].request_order_items[0].ordered_qty, "Updated ordered_qty of the first item should be same");
                Assert.AreEqual("test", createdRO[0].request_order_items[0].comment, "RO Comment should match after update");

                //RO item 2
                Assert.AreEqual(30, createdRO[0].request_order_items[1].id, "Updated asset master id of the first item should be same");
                //Assert.AreEqual(282, createdRO[0].request_order_items[1].itemID, "Updated item id of the first item should be same");
                Assert.AreEqual(68, createdRO[0].request_order_items[1].currencyId, "Updated currency id of the first item should be same");
                Assert.AreEqual(6789, createdRO[0].request_order_items[1].cost, "Updated cost of the first item should be same");
                Assert.AreEqual(76, createdRO[0].request_order_items[1].ordered_qty, "Updated ordered_qty of the first item should be same");
                Assert.AreEqual("rreere", createdRO[1].request_order_items[1].comment, "RO Comment should match after update");






                //step 3 : Reject RO
                var rejectPayload = @"{
                                ""id"": " + createdROId + @",
                                ""comment"": ""test reject api"",
                                ""facilityId"": 1
                             }";

                var rejectResponse = roService.CreateItem(EP_RejectRequestOrder, rejectPayload);

                string expectedRejectMessage = $"Rejected request order.";
                Assert.AreEqual(expectedRejectMessage, rejectResponse.message, "Rejection message mismatch");

                int rejectedROId = rejectResponse.id[0];
                var rejectedRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + rejectedROId + "&facility_id=1");

                Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED, rejectedRO[0].status, "RO status should be rejected");

                Assert.AreEqual("Admin HFE", rejectedRO[0].rejectedBy, "Rejected By should be 'Admin HFE'");
                DateTime expectedRejectedAt = DateTime.Today;
                DateTime actualRejectedAt = (DateTime)rejectedRO[0].rejectedAt;

                Assert.AreEqual(expectedRejectedAt, actualRejectedAt, "The rejected timestamp should match");





                // Step 3: Update Request Order
                // We need to capture the itemIDs from the created RO
                int firstItemId = (int)createdRO[0].request_order_items[0].itemID;
                int secondItemId = (int)createdRO[0].request_order_items[1].itemID;

                var updatePayload = @"{
                            ""facilityID"": 1,
                            ""request_order_items"": [
                                {
                                    ""currencyId"": 69,
                                    ""itemID"": " + firstItemId + @",
                                    ""assetMasterItemID"": 12,
                                    ""cost"": 5000,
                                    ""ordered_qty"": 70,
                                    ""comment"": ""updated item 1""
                                },
                                {
                                    ""currencyId"": 69,
                                    ""itemID"": " + secondItemId + @",
                                    ""assetMasterItemID"": 31,
                                    ""cost"": 6000,
                                    ""ordered_qty"": 30,
                                    ""comment"": ""updated item 2""
                                }
                            ],
                            ""comment"": ""Updated Order"",
                            ""request_order_id"": " + rejectedROId + @"
                         }";

                var rouService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
                var response = rouService.CreateItem(EP_UpdateRequestOrder, updatePayload);

                if (response == null)
                {
                    Console.WriteLine("Response is null! Check the endpoint or request.");
                }
                else
                {
                    Assert.AreEqual("Request order updated successfully.", response.message, "RO update message mismatch");
                }

                // Verify the updated RO by fetching its details
                var updatedRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + createdROId + "&facility_id=1");

                if (updatedRO == null || updatedRO.Count == 0)
                {
                    Console.WriteLine("No data returned for updated RO. Possible issue with API or request.");
                }
                else
                {
                    Assert.AreEqual(1, updatedRO[0].facilityID, "FacilityID should be 1 after creation.");
                    Assert.AreEqual(createdROId, updatedRO[0].request_order_id, "createdROId and Id getting from response should be same");
                    Assert.AreEqual("Updated Order", updatedRO[0].comment);

                    //RO item 1
                    Assert.AreEqual(12, updatedRO[0].request_order_items[0].id, "Updated currency id of the first item should be same");
                    Assert.AreEqual(firstItemId, updatedRO[0].request_order_items[0].itemID, "Updated item id of the first item should be same");
                    Assert.AreEqual(69, updatedRO[0].request_order_items[0].currencyId, "Updated currency id of the first item should be same");
                    Assert.AreEqual(5000, updatedRO[0].request_order_items[0].cost, "Updated cost of the first item should be same");
                    Assert.AreEqual(70, updatedRO[0].request_order_items[0].ordered_qty, "Updated ordered_qty of the first item should be same");
                    Assert.AreEqual("updated item 1", updatedRO[0].request_order_items[0].comment, "RO Comment should match after update");

                    //RO item 2
                    Assert.AreEqual(31, updatedRO[0].request_order_items[1].id, "Updated currency id of the second item should be same");
                    Assert.AreEqual(secondItemId, updatedRO[0].request_order_items[1].itemID, "Updated item id of the second item should be same");
                    Assert.AreEqual(69, updatedRO[0].request_order_items[1].currencyId, "Updated currency id of the second item should be same");
                    Assert.AreEqual(6000, updatedRO[0].request_order_items[1].cost, "Updated cost of the second item should be same");
                    Assert.AreEqual(30, updatedRO[0].request_order_items[1].ordered_qty, "Updated ordered_qty of the second item should be same");
                    Assert.AreEqual("updated item 2", updatedRO[1].request_order_items[1].comment, "RO Comment should match after update");
                }



                //Step 4: Approve Request Order
                var approvePayload = @"{
                                  ""id"": " + rejectedROId + @",
                                  ""comment"": ""Approving the order"",
                                  ""facilityId"": 1
                               }";

                var approveResponse = roService.CreateItem(EP_ApproveRequestOrder, approvePayload);
                Assert.AreEqual($"Approved request order  {rejectedROId}  successfully.", approveResponse.message, "Approval message mismatch");

                // Verify the approved status
                var approvedRO = getItemService.GetItemList(EP_GetRODetailsByID + "?IDs=" + rejectedROId + "&facility_id=1");
                Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED, approvedRO[0].status);

                int approvedROId = response.id[0];
                Assert.AreEqual(approvedRO[0].approvedBy, "Admin HFE");
                DateTime expectedGeneratedAt = DateTime.Today;
                DateTime actualGeneratedAt = (DateTime)approvedRO[0].approvedAt;

                Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The approved timestamp should be the same");

                Assert.AreEqual(createdROId, approvedROId, "id and reponse id should be match");


            }

        }
    }
}
