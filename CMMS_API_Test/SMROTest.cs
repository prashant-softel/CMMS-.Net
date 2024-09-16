using CMMSAPIs.Models.SM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrder>();
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + goId + "&facility_id=" + facility_id);
            Assert.AreEqual(11099, updatedRO[0].cost);
            Assert.AreEqual("test", updatedRO[0].comment);
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
                          ""request_order_id"": 73
                      }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateRequestOrder, payload);

            int goId = response.id[0];
            string actualMessage = response.message;

            string expectedMessage = "Request order updated successfully.";

            Assert.AreEqual(expectedMessage, actualMessage);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMCreateRequestOrder>();
            var updatedRO = getItem.GetItemList(EP_GetRODetailsByID + "?IDs=" + goId);

            //Assert.AreEqual(33297, updatedRO[0].cost);
            Assert.AreEqual("test", updatedRO[0].comment);
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
        }


        [TestMethod]
        public void VerifyRejectRO()
        {
            string payload = @"{
                                ""id"": 74,
                                ""comment"": ""test reject api"",
                                ""facilityId"": 1
                                 }";


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_RejectRequestOrder, payload);

            int goId = response.id[0];

            string expectedMessage = $"Rejected request order.";

            Assert.AreEqual(expectedMessage, response.message);
        }

    }

    
}
