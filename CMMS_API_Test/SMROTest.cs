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
        public void VerifygetROItemById()
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


        /*[TestMethod]
        public void VerifyUpdateRO()
        {

            int roId = 88;
            // Define the payload with the updated values
            string payload = @"{
                      ""facilityID"": 1,
                      ""request_order_items"": [
                          {
                              ""currencyId"": 4,
                              ""itemID"": 0,
                              ""assetMasterItemID"": 12,
                              ""cost"": 4545,  // Updated cost
                              ""ordered_qty"": 70,  // Updated ordered_qty
                              ""comment"": ""updated cost and qty for test""  // Updated comment
                          },
                          {
                              ""currencyId"": 2,
                              ""itemID"": 0,
                              ""assetMasterItemID"": 31,
                              ""cost"": 6765, 
                              ""ordered_qty"": 23, 
                              ""comment"": ""aditya""  
                          }
                      ],
                      ""comment"": ""Updated overall comment"",
                      ""request_order_id"": 88
                  }";

            // Create the service instance and update the request order
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateRequestOrder, payload);

            // Assert that the response message is as expected
            string actualMessage = response.message;
            string expectedMessage = "Request order updated successfully.";
            Assert.AreEqual(expectedMessage, actualMessage, "The response message should confirm the update.");

            // Fetch the updated RO details after the update
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId);

            // Verify the updated properties
            Assert.AreEqual(1, updatedRO[0].facilityID, "FacilityID should be 1");
            Assert.AreEqual("Updated overall comment", updatedRO[0].comment, "Comment should be updated to 'Updated overall comment'");

            // Verify first item changes
            var firstItem = updatedRO[0].request_order_items[0];
            Assert.AreEqual(12, firstItem.id, "ID for the first item should be 12");
            Assert.AreEqual(4545, firstItem.cost, "Cost for the first item should be updated to 4545");
            Assert.AreEqual(70, firstItem.ordered_qty, "Ordered quantity for the first item should be updated to 70");
            Assert.AreEqual("updated cost and qty for test", firstItem.comment, "Comment for the first item should be updated");

            // Verify second item changes
            var secondItem = updatedRO[0].request_order_items[1];
            Assert.AreEqual(31, secondItem.id, "ID for the second item should be 31");
            Assert.AreEqual(6765, secondItem.cost, "Cost for the second item should remain unchanged at 6765");
            Assert.AreEqual(23, secondItem.ordered_qty, "Ordered quantity for the second item should remain unchanged at 23");
            Assert.AreEqual("aditya", secondItem.comment, "Comment for the second item should be 'aditya'");

            // Optionally, if the second item comment was expected to remain "rreere", correct the payload or assertions accordingly.
        }*/


        /*
                [TestMethod]
                public void VerifyUpdateRO()
                {

                    int requestOrderId = 90;


                    // Update the Request Order with new values
                    string payload = @"{
                                  ""facilityID"": 1,
                                  ""request_order_items"": [
                                      {
                                          ""currencyId"": 10,
                                          ""itemID"": 0,
                                          ""assetMasterItemID"": 18,
                                          ""cost"": 87453,
                                          ""ordered_qty"": 70,
                                          ""comment"": ""test comment""
                                      },
                                      {
                                          ""currencyId"": 7,
                                          ""itemID"": 0,
                                          ""assetMasterItemID"": 31,
                                          ""cost"": 672345,
                                          ""ordered_qty"": 93,
                                          ""comment"": ""aditya sa""
                                      }
                                  ],
                                  ""comment"": ""aditya"",
                                  ""request_order_id"": 90
                              }";

                    var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
                    var response = ptwService.CreateItem(EP_UpdateRequestOrder, payload);


                    Assert.IsNotNull(response, "Update response should not be null.");

                    string actualMessage = response.message;
                    string expectedMessage = "Request order updated successfully.";

                    Assert.AreEqual(expectedMessage, actualMessage);


                    // Fetch the updated RO details after the update
                    var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + requestOrderId);

                    Assert.IsNotNull(updatedRO, "Updated RO should not be null.");
                    Assert.IsTrue(updatedRO.Count > 0, "Updated RO list should not be empty.");
                    Assert.IsNotNull(updatedRO[0].request_order_items, "request_order_items should not be null.");
                    Assert.IsTrue(updatedRO[0].request_order_items.Count > 0, "request_order_items should have at least 1 items.");

                    // Verify updated properties
                    Assert.AreEqual(1, updatedRO[0].facilityID, "FacilityID did not update correctly.");
                    Assert.AreEqual("aditya", updatedRO[0].comment, "Comment did not update correctly.");

                    var updatedItem1 = updatedRO[0].request_order_items[0];
                    Assert.AreEqual(4, updatedItem1.currencyId, "CurrencyId of item 1 did not update correctly.");
                    Assert.AreEqual(12, updatedItem1.id, "AssetMasterItemID of item 1 did not update correctly.");
                    Assert.AreEqual(4334, updatedItem1.cost, "Cost of item 1 did not update correctly.");
                    Assert.AreEqual(67, updatedItem1.ordered_qty, "OrderedQty of item 1 did not update correctly.");
                    Assert.AreEqual("test", updatedItem1.comment, "Comment of item 1 did not update correctly.");

                    var updatedItem2 = updatedRO[0].request_order_items[1];
                    Assert.AreEqual(7, updatedItem2.currencyId, "CurrencyId of item 2 did not update correctly.");
                    Assert.AreEqual(31, updatedItem2.id, "AssetMasterItemID of item 2 did not update correctly.");
                    Assert.AreEqual(672345, updatedItem2.cost, "Cost of item 2 did not update correctly.");
                    Assert.AreEqual(93, updatedItem2.ordered_qty, "OrderedQty of item 2 did not update correctly.");
                    Assert.AreEqual("aditya sa", updatedItem2.comment, "Comment of item 2 did not update correctly.");


                }
        */
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
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED, updatedRO[0].status);

            Assert.AreEqual(updatedRO[0].approvedBy, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)updatedRO[0].approvedAt;

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The rejected timestamp should be the same");

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
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED, updatedRO[0].status);

            Assert.AreEqual(updatedRO[0].rejectedBy, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)updatedRO[0].rejectedAt;

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
            string expectedMessage = $"Request order {roId} closed.";

            Assert.AreEqual(expectedMessage, response.message);
            
            var facility_id = 1;

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrderGET>();
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + roId + "&facility_id=" + facility_id);
            Assert.AreEqual((int)CMMS.CMMS_Status.SM_RO_CLOSED, updatedRO[0].status);

            Assert.AreEqual(updatedRO[0].closed_by, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            string actualGeneratedAt = updatedRO[0].closed_at;

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The rejected timestamp should be the same");

            Assert.AreEqual(86, response.id[0]);
        }

    }

    
}
