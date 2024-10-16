using CMMSAPIs.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class SMGOTest
    {

        //◆◇◆◆◆ GO End Points ◆◆◆◆◇◆
        string EP_getGOList = "/api/GO/GetGOList";
        string EP_getGOItemByID = "/api/GO/GetGOItemByID";
        string EP_GetGODetailsByID = "/api/GO/GetGODetailsByID";
        string EP_createGO = "/api/GO/CreateGO";
        string EP_updateGO = "/api/GO/UpdateGO";
        string EP_GOApproval = "/api/GO/ApproveGO";
        string EP_RejectGO = "/api/GO/RejectGO";
        string EP_UpdateGOReceive = "/api/GO/UpdateGOReceive";
        string EP_ApproveGOReceive = "/api/GO/ApproveGOReceive";
        string EP_RejectGOReceive = "/api/GO/RejectGOReceive";
        string EP_CloseGO = "/api/GO/CloseGO";
        string EP_DeleteGO = "/api/GO/DeleteGO";
        string EP_GetGoodsOrderData = "/api/GO/GetGoodsOrderData";
        string EP_SubmitGoodsOrderData = "/api/GO/SubmitGoodsOrderData";


        //◆◇◆◆◆ GO Test Methods ◆◆◆◆◇◆
        [TestMethod]
        public void VerifygetGOList()
        {
            int facilityID = 1;
            string fromDate = "2024-09-13";
            string toDate = "2024-09-14";
            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOListByFilter>();
            var response = goService.GetItemList(EP_getGOList + "?facility_id=" + facilityID + "&fromDate=" + fromDate + "&toDate=" + toDate);

            int vendorID = response[0].vendorID;

            Assert.AreEqual(vendorID, 7);
            Assert.IsNotNull(response, "GO list should not be null.");
            Assert.IsTrue(response.Count > 0, "GO list should contain at least one order.");
        }

        [TestMethod]
        public void VerifyGetGoodsOrderData()
        {
            int facilityID = 1;
            int empRole = 1;
            string fromDate = "2024-09-13";
            string toDate = "2024-09-14";
            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOListByFilter>();
            var response = goService.GetItemList(EP_GetGoodsOrderData + "?facility_id=" + facilityID + "&fromDate=" + fromDate + "&toDate=" + toDate + "&empRole=" + empRole);
            Assert.IsNotNull(response, "GO Data should not be null.");
            Assert.IsTrue(response.Count > 0, "GO Data should contain at least one order.");
        }

        [TestMethod]
        public void VerifyGetGODetailsByID()
        {
            int id = 156;
            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderList>();
            var response = goService.GetItem(EP_GetGODetailsByID + "?ID=" + id);

            int myNewItemId = response.id;
            Assert.AreEqual(myNewItemId, id);
            Assert.IsNotNull(response, "GO details should not be null.");
        }

        [TestMethod]
        public void VerifyDeleteGO()
        {
            string payload = @"{
                                ""id"": 155,
                                ""comment"": ""test"",
                                ""facilityId"": 1
                                 }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_DeleteGO, payload);

            Assert.AreEqual("Goods order deleted.", response.message);

            int goId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_DELETED, goResponse.status);

            Assert.AreEqual(goResponse.Id, goId);
        }

        [TestMethod]
        public void VerifySubmitGoodsOrderData()
        {
            string payload = @"{
                        ""facility_id"": 45,
                        ""purchaseID"": 1,
                        ""submitItems"": [
                            {
                                ""assetCode"": ""S102102001"",
                                ""assetItemID"": 1,
                                ""orderedQty"": 10,
                                ""cost"": 345
                            }
                        ]
                      }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_SubmitGoodsOrderData, payload);

            Assert.AreEqual("Goods order submitted successfully.", response.message);

            int goId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_SUBMITTED, goResponse.status);

            Assert.AreEqual(goResponse.Id, goId);
        }

        [TestMethod]
        public void VerifygetGOItemById()
        {
            int id = 1;
            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderList>();
            var response = goService.GetItem(EP_getGOItemByID + "?ID=" + id);
            Console.WriteLine("Expected ID: " + id);
            Console.WriteLine("Actual ID from response: " + response.purchaseID);
            int myNewItemId = response.purchaseID;
            Assert.AreEqual(myNewItemId, id);
            Assert.IsNotNull(response, "GO item should not be null.");
        }


        [TestMethod]
        public void VerifyCreateGO()
        {

            var payload = new CMMSAPIs.Models.CMGoodsOrderList

            {
                facility_id = 1,
                vendorID = 325,
                po_no = "23123",
                amount = 34422,
                currencyID = 69,
                id = 0,
                is_submit = 1,
                go_items = new List<CMMSAPIs.Models.CMGO_ITEMS>
                {
                    new CMMSAPIs.Models.CMGO_ITEMS
                    {
                        assetMasterItemID = 1,
                        cost = 1,
                        ordered_qty = 1,
                        paid_by_ID = 2,
                        requested_qty = 1,

                    }
                }

            };

            string Jsonpayload = JsonConvert.SerializeObject(payload);

            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = goService.CreateItem(EP_createGO, Jsonpayload);
            int goId = response.id[0];

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var responseForItem = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_SUBMITTED, responseForItem.status);

            Assert.AreEqual(payload.facility_id, responseForItem.facility_id);
            Assert.AreEqual(payload.vendorID, responseForItem.vendorID);
            Assert.AreEqual(payload.po_no, responseForItem.po_no);
            //Assert.AreEqual(payload.po_date, responseForItem.po_date);
            Assert.AreEqual(payload.amount, responseForItem.amount);
            Assert.AreEqual(payload.currencyID, responseForItem.currencyID);
            //Assert.AreEqual(payload.currency, responseForItem.currency);


            for(int i = 0; i < payload.go_items.Count; i++)
            {
                var expected = payload.go_items[i];
                var actual = responseForItem.GODetails[i];

                
                Assert.AreEqual(expected.assetMasterItemID, actual.assetMasterItemID);
                Assert.AreEqual(expected.cost, actual.cost);
                Assert.AreEqual(expected.ordered_qty, actual.ordered_qty);
                Assert.AreEqual(expected.paid_by_ID, actual.paid_by_ID);
                Assert.AreEqual(expected.requested_qty, actual.requested_qty);
                Assert.AreEqual(expected.paid_by_ID, actual.paid_by_ID);
                
            }

           
            Assert.AreEqual(responseForItem.submitted_by_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)responseForItem.submitted_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The submitted timestamp should be the same");

            Assert.AreEqual(responseForItem.Id, goId);
        }


        [TestMethod]
        public void VerifyupdateGO()
        {

            var payload = new CMMSAPIs.Models.CMGoodsOrderList

            {
                facility_id = 1,
                vendorID = 325,
                po_no = "23123",
                amount = 34422,
                currencyID = 69,
                id = 0,
                is_submit = 1,
                go_items = new List<CMMSAPIs.Models.CMGO_ITEMS>
                {
                    new CMMSAPIs.Models.CMGO_ITEMS
                    {
                        assetMasterItemID = 1,
                        cost = 1,
                        ordered_qty = 1,
                        paid_by_ID = 2,
                        requested_qty = 1,

                    }
                }

            };

            string Jsonpayload = JsonConvert.SerializeObject(payload);

            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = goService.CreateItem(EP_updateGO, Jsonpayload);

            int goId = response.id[0];
            string actualMessage = response.message;

            string expectedMessage = "Goods order updated successfully.";

            Assert.AreEqual(expectedMessage, actualMessage);
            

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var updatedGO = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_SUBMITTED, updatedGO.status);

            Assert.AreEqual(87878, updatedGO.amount);
            Assert.AreEqual("434343", updatedGO.po_no);
            Assert.AreEqual(1, updatedGO.facility_id);
            Assert.AreEqual(0, updatedGO.location_ID);
            Assert.AreEqual(325, updatedGO.vendorID);
            //Assert.AreEqual("0001-01-01", updatedGO.purchaseDate);
            //Assert.AreEqual("2024-09-14", updatedGO.po_date);
            Assert.AreEqual(69, updatedGO.currencyID);
            Assert.AreEqual("Indian Rupee", updatedGO.currency);

            var goItem = updatedGO.GODetails[0];
            //Assert.AreEqual(2599, updatedGO.GODetails[0].requestOrderId);
            //Assert.AreEqual(2599, updatedGO.GODetails[0].assetMasterItemID);
            Assert.AreEqual(1, updatedGO.GODetails[0].cost);
            Assert.AreEqual(1, updatedGO.GODetails[0].ordered_qty);
            Assert.AreEqual(2, updatedGO.GODetails[0].paid_by_ID);
            Assert.AreEqual(1, updatedGO.GODetails[0].requested_qty);
            Assert.AreEqual(0, updatedGO.GODetails[0].accepted_qty);
            Assert.AreEqual(0, updatedGO.GODetails[0].received_qty);
            Assert.AreEqual("DC Disconnector Switch", updatedGO.GODetails[0].assetItem_Name);
            Assert.AreEqual("operator", updatedGO.GODetails[0].paid_by_name);
            Assert.AreEqual(0, updatedGO.GODetails[0].receive_later);
            Assert.AreEqual(2, updatedGO.GODetails[0].asset_type_ID);
            Assert.AreEqual("SMB", updatedGO.GODetails[0].cat_name);
            Assert.AreEqual("Spare", updatedGO.GODetails[0].asset_type);
            Assert.AreEqual("S043213001", updatedGO.GODetails[0].asset_code);
        }


        [TestMethod]
        public void VerifyApproveGO()
        {
            string payload = @"{
                                ""id"": 127,
                                ""comment"": ""test"",
                                ""facilityId"": 1
                                 }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_GOApproval, payload);

            Assert.AreEqual("Approval Successful.", response.message);

            int goId = response.id[0];
            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_APPROVED, goResponse.status);

            Assert.AreEqual(goResponse.approved_by_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)goResponse.approved_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The approved timestamp should be the same");

            Assert.AreEqual(goResponse.Id,goId);
        }

        [TestMethod]
        public void VerifyRejectGO()
        {
            string payload = @"{
                                ""id"": 128,
                                ""comment"": ""test reject api"",
                                ""facilityId"": 1
                                 }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_RejectGO, payload);

            int goId = response.id[0];

            string expectedMessage = $"Goods Order {goId} rejected successfully.";

            Assert.AreEqual(expectedMessage, response.message);


            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_REJECTED, goResponse.status);

            Assert.AreEqual(goResponse.rejected_by_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)goResponse.rejected_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The rejected timestamp should be the same");

            Assert.AreEqual(goResponse.Id, goId);
        }


        [TestMethod]
        public void VerifyUpdateGOrecieve()
        {

            string payload = @"{
                          ""facility_id"": 1,
                          ""order_type"": 1,
                          ""location_ID"": 1,
                          ""vendorID"": 1,
                          ""purchaseDate"": ""2024-09-14"",
                          ""challan_no"": ""3232"",
                          ""challan_date"": ""2024-09-14"",
                          ""po_no"": ""434332"",
                          ""po_date"": ""2024-09-14"",
                          ""freight"": ""2332"",
                          ""receivedAt"": ""2024-09-14"",
                          ""no_pkg_received"": ""23"",
                          ""lr_no"": ""324"",
                          ""freight_value"": ""432432"",
                          ""inspection_report"": ""3243"",
                          ""condition_pkg_received"": ""3343"",
                          ""vehicle_no"": ""MH02FZ2312"",
                          ""gir_no"": ""9887"",
                          ""closedBy"": null,
                          ""job_ref"": ""5543"",
                          ""amount"": 323232,
                          ""currencyID"": 69,
                          ""id"": 139,
                          ""is_submit"": 1,
                          ""go_items"": [
                              {
                                  ""assetMasterItemID"": 1,
                                  ""storage_rack_no"": ""null"",
                                  ""storage_row_no"": ""null"",
                                  ""storage_column_no"": ""null"",
                                  ""goItemID"": 2601,
                                  ""cost"": 1,
                                  ""ordered_qty"": 1,
                                  ""paid_by_ID"": 2,
                                  ""requested_qty"": 1,
                                  ""accepted_qty"": 0,
                                  ""received_qty"": 1,
                                  ""lost_qty"": 0,
                                  ""damaged_qty"": 0,
                                  ""id"": 0,
                                  ""assetItem_Name"": """",
                                  ""asset_cat"": """",
                                  ""spare_status"": 0,
                                  ""remarks"": """",
                                  ""receive_later"": 0,
                                  ""asset_type_ID"": 0,
                                  ""paid_by_name"": """",
                                  ""cat_name"": """",
                                  ""asset_type"": """",
                                  ""asset_code"": """",
                                  ""sr_no"": ""null""
                              }
                          ]
                    }";

            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = goService.CreateItem(EP_UpdateGOReceive, payload);

            int goId = response.id[0];

            string actualMesage = response.message;

            string expectedMessage = "Goods order updated successfully.";

            Assert.AreEqual(actualMesage, expectedMessage);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual("2332", goResponse.freight);
            Assert.AreEqual(323232, goResponse.amount);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED, goResponse.status);

            Assert.AreEqual(goResponse.receive_submitted_by_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)goResponse.receive_submitted_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The receive submitted timestamp should be the same");

            Assert.AreEqual(goResponse.Id, goId);
        }

        [TestMethod]
        public void VerifyApproveGOReceive()
        {
            string payload = @"{
                                ""id"": 139,
                                ""comment"": ""test"",
                                ""facilityId"": 1
                                 }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_ApproveGOReceive, payload);

            int goId = response.id[0];

            var actualMessage = response.message;

            var expectedMessage = $"Goods order receive {goId} approved successfully";

            Assert.AreEqual(expectedMessage, actualMessage);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_RECEIVED_APPROVED, goResponse.status);

            Assert.AreEqual(goResponse.receive_approved_by_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)goResponse.receive_approved_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The receive approved timestamp should be the same");

            Assert.AreEqual(goResponse.Id, goId);


        }

        [TestMethod]
        public void VerifyRejectGOReceive()
        {
            string payload = @"{
                                ""id"": 138,
                                ""comment"": ""test"",
                                ""facilityId"": 1
                                 }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_RejectGOReceive, payload);

            int goId = response.id[0];

            var actualMessage = response.message;

            var expectedMessage = $"Goods order receive {goId} rejected successfully.";

            Assert.AreEqual(expectedMessage, actualMessage);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_RECEIVED_REJECTED, goResponse.status);

            Assert.AreEqual(goResponse.receive_rejected_by_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)goResponse.receive_rejected_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The receive rejected timestamp should be the same");

            Assert.AreEqual(goResponse.Id, goId);


        }

        [TestMethod]
        public void VerifyCloseGo()
        {
            string payload = @"{
                                ""id"": 139,
                                ""comment"": ""test close"",
                                ""facilityId"": 1
                                 }";


            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_CloseGO, payload);

            int goId = response.id[0];

            var actualMessage = response.message;

            var expectedMessage = $"Goods order withdrawn successfully.";

            Assert.AreEqual(expectedMessage, actualMessage);

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var goResponse = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_CLOSED, goResponse.status);

            Assert.AreEqual(goResponse.closed_by_name, "Admin HFE");

            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)goResponse.closed_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The closed timestamp should be the same");

            Assert.AreEqual(goResponse.Id, goId);

        }




        [TestMethod]
        public void VerifyFunctionalTestGO()
        {


            //STEP 1: Create GO
            var CreatePayload = new CMMSAPIs.Models.CMGoodsOrderList
            {
                facility_id = 1,
                vendorID = 325,
                po_no = "23123",
                amount = 34422,
                currencyID = 69,
                id = 0,
                is_submit = 1,
                go_items = new List<CMMSAPIs.Models.CMGO_ITEMS>
                {
                    new CMMSAPIs.Models.CMGO_ITEMS
                    {
                        assetMasterItemID = 1,
                        cost = 1,
                        ordered_qty = 1,
                        paid_by_ID = 2,
                        requested_qty = 1,

                    }
                }
            };

            var CreateJsaonPayload = JsonConvert.SerializeObject(CreatePayload);

            var CreateGOService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var CreateGOResponse = CreateGOService.CreateItem(EP_createGO, CreateJsaonPayload);

            int goId = CreateGOResponse.id[0];

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var responseForItem = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_SUBMITTED, responseForItem.status, "status should be same");

            Assert.AreEqual(responseForItem.submitted_by_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)responseForItem.submitted_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The submitted timestamp should be the same");

            Assert.AreEqual(responseForItem.Id, goId);




            //STEP 2: Reject GO
            var RejectPayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = goId,
                comment = "test "
            };

            string RejectJsonPayload = JsonConvert.SerializeObject(RejectPayload);

            var goService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = goService.CreateItem(EP_RejectGO, RejectJsonPayload);

            string expectedMessage = $"Goods Order {goId} rejected successfully.";

            Assert.AreEqual(expectedMessage, response.message);

            var RejectgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var RejectgoResponse = RejectgetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_REJECTED, RejectgoResponse.status);

            Assert.AreEqual(RejectgoResponse.rejected_by_name, "Admin HFE");

            DateTime RejectexpectedGeneratedAt = DateTime.Today;
            DateTime RejectactualGeneratedAt = (DateTime)RejectgoResponse.rejected_at;
            RejectexpectedGeneratedAt = new DateTime(RejectexpectedGeneratedAt.Year, RejectexpectedGeneratedAt.Month, RejectexpectedGeneratedAt.Day, 0, 0, 0);
            RejectactualGeneratedAt = new DateTime(RejectactualGeneratedAt.Year, RejectactualGeneratedAt.Month, RejectactualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(RejectexpectedGeneratedAt, RejectactualGeneratedAt, "The rejected timestamp should be the same");

            Assert.AreEqual(RejectgoResponse.Id, goId);





            //STEP 3: Update Go
            var Updatepayload = new CMMSAPIs.Models.CMGoodsOrderList
            {
                facility_id = 1,
                order_by_type = 1,
                location_ID = 1,
                vendorID = 325,
                purchaseDate = DateTime.Parse("0001-01-01"),
                challan_no = "",
                challan_date = DateTime.Parse("0001-01-01"),
                po_no = "1234",
                po_date = DateTime.Parse("2024-10-10"),
                freight = "",
                receivedAt = DateTime.Parse("0001-01-01"),
                no_pkg_received = "",
                lr_no = "",
                freight_value = "",
                inspection_report = "",
                condition_pkg_received = "",
                vehicle_no = "",
                gir_no = "",
                job_ref = "",
                amount = 34422,
                currencyID = 69,
                id = goId,
                is_submit = 1,
                go_items = new List<CMMSAPIs.Models.CMGO_ITEMS>
                {
                    new CMMSAPIs.Models.CMGO_ITEMS
                    {
                        assetMasterItemID = 5007,
                        goItemID = 5007,
                        cost = 15433,
                        ordered_qty = 143,
                        paid_by_ID = 2,
                        requested_qty = 123,
                        accepted_qty = 0,
                        received_qty = 0,
                        lost_qty = 0,
                        damaged_qty = 0,
                        asset_name = "",
                        remarks = "",
                        asset_type_ID = 0,
                        cat_name = "",
                        asset_type = "",
                        asset_code = "",
                        sr_no = ""
                    }
                }
            };

            string UpdateJsonpayload = JsonConvert.SerializeObject(Updatepayload);

            var UpdategoService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Updateresponse = UpdategoService.CreateItem(EP_updateGO, UpdateJsonpayload);

            Assert.AreEqual("Goods order updated successfully.", Updateresponse.message);


            var UpdategetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var updatedGO = UpdategetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_SUBMITTED, updatedGO.status);





            //STEP 4: Approve GO
            var ApprovePayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = goId,
                comment = "test "
            };

            string ApproveJsonPayload = JsonConvert.SerializeObject(ApprovePayload);


            var ApprovegoService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var Approveresponse = ApprovegoService.CreateItem(EP_GOApproval, ApproveJsonPayload);

            Assert.AreEqual($"Goods Order {goId} approved successfully.", Approveresponse.message);

            var ApprovegetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var ApprovegoResponse = ApprovegetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_APPROVED, ApprovegoResponse.status);

            Assert.AreEqual(ApprovegoResponse.approved_by_name, "Admin HFE");

            DateTime ApproveexpectedGeneratedAt = DateTime.Today;
            DateTime ApproveactualGeneratedAt = (DateTime)ApprovegoResponse.approved_at;
            ApproveexpectedGeneratedAt = new DateTime(ApproveexpectedGeneratedAt.Year, ApproveexpectedGeneratedAt.Month, ApproveexpectedGeneratedAt.Day, 0, 0, 0);
            ApproveactualGeneratedAt = new DateTime(ApproveactualGeneratedAt.Year, ApproveactualGeneratedAt.Month, ApproveactualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(ApproveexpectedGeneratedAt, ApproveactualGeneratedAt, "The approved timestamp should be the same");

            Assert.AreEqual(ApprovegoResponse.Id, goId);





            //STEP 5: Submit for GO Recieve
            var UpdateReceivepayload = new CMMSAPIs.Models.CMGoodsOrderList
            {
                facility_id = 17,
                order_by_type = 1,
                location_ID = 1,
                vendorID = 1,
                purchaseDate = DateTime.Parse("2024-10-16"),
                challan_no = "635463",
                challan_date = DateTime.Parse("2024-10-16"),
                po_no = "143143",
                po_date = DateTime.Parse("2024-10-16"),
                freight = "72636463",
                receivedAt = DateTime.Parse("2024-10-16"),
                no_pkg_received = "123",
                lr_no = "6375859",
                freight_value = "838384",
                inspection_report = "837373",
                condition_pkg_received = "27667",
                vehicle_no = "MH02BD5253",
                gir_no = "34343",
                job_ref = "98889",
                amount = 143143,
                currencyID = 69,
                id = goId,
                is_submit = 1,
                go_items = new List<CMMSAPIs.Models.CMGO_ITEMS>
            {
                new CMMSAPIs.Models.CMGO_ITEMS
                {
                    assetMasterItemID = 112,
                    storage_rack_no = "1",
                    storage_row_no = "1",
                    storage_column_no = "1",
                    goItemID = 5007,
                    cost = 15433,
                    ordered_qty = 143,
                    paid_by_ID = 2,
                    requested_qty = 123,
                    accepted_qty = 1,
                    received_qty = 1,
                    lost_qty = 0,
                    damaged_qty = 1,
                    cat_name = "",
                    asset_type = "",
                    asset_code = "",
                    sr_no = ""
                }
            }
            };



            string UpdateReceiveJsonPayload = JsonConvert.SerializeObject(UpdateReceivepayload);

            var UpdateRecService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var UpdateRecresponse = UpdateRecService.CreateItem(EP_UpdateGOReceive, UpdateReceiveJsonPayload);

            Assert.AreEqual($"Goods order updated successfully.", UpdateRecresponse.message);

            var UpdateRecgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var UpdateRecResponse = UpdateRecgetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED, UpdateRecResponse.status);

            Assert.AreEqual(ApprovegoResponse.approved_by_name, "Admin HFE");

            DateTime UpdateRecexpectedGeneratedAt = DateTime.Today;
            DateTime UpdateRecactualGeneratedAt = (DateTime)UpdateRecResponse.approved_at;
            UpdateRecexpectedGeneratedAt = new DateTime(UpdateRecexpectedGeneratedAt.Year, UpdateRecexpectedGeneratedAt.Month, UpdateRecexpectedGeneratedAt.Day, 0, 0, 0);
            UpdateRecactualGeneratedAt = new DateTime(UpdateRecactualGeneratedAt.Year, UpdateRecactualGeneratedAt.Month, UpdateRecactualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(UpdateRecexpectedGeneratedAt, UpdateRecactualGeneratedAt, "The approved timestamp should be the same");

            Assert.AreEqual(UpdateRecResponse.Id, goId);





            //STEP 6: Reject GO Receive
            var RejectRecPayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = goId,
                comment = "test"
            };

            string RejectRecJsonPayload = JsonConvert.SerializeObject(RejectRecPayload);


            var RejectRecService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var RejectRecresponse = RejectRecService.CreateItem(EP_RejectGOReceive, RejectRecJsonPayload);

            Assert.AreEqual($"Goods order receive {goId} rejected successfully.", RejectRecresponse.message);

            var RejectRecgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var RejectRecResponse = RejectRecgetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_RECEIVED_REJECTED, RejectRecResponse.status);

            Assert.AreEqual(RejectRecResponse.approved_by_name, "Admin HFE");

            DateTime RejectRecexpectedGeneratedAt = DateTime.Today;
            DateTime RejectRecactualGeneratedAt = (DateTime)ApprovegoResponse.approved_at;
            RejectRecexpectedGeneratedAt = new DateTime(RejectRecexpectedGeneratedAt.Year, RejectRecexpectedGeneratedAt.Month, RejectRecexpectedGeneratedAt.Day, 0, 0, 0);
            RejectRecactualGeneratedAt = new DateTime(RejectRecactualGeneratedAt.Year, RejectRecactualGeneratedAt.Month, RejectRecactualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(RejectRecexpectedGeneratedAt, RejectRecactualGeneratedAt, "The approved timestamp should be the same");

            Assert.AreEqual(RejectRecResponse.Id, goId);





            //STEP 7: Update For Approval
            var UpdateRecService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var UpdateRecresponse2 = UpdateRecService2.CreateItem(EP_UpdateGOReceive, UpdateReceiveJsonPayload);

            Assert.AreEqual($"Goods order updated successfully.", UpdateRecresponse2.message);




            //STEP 8: Approve GO receive
            var ApproveRecPayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = goId,
                comment = "test"
            };

            string ApproveRecJsonPayload = JsonConvert.SerializeObject(ApproveRecPayload);


            var ApproveRecService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var ApproveRecresponse = ApproveRecService.CreateItem(EP_ApproveGOReceive, ApproveRecJsonPayload);

            Assert.AreEqual($"Goods order receive {goId} approved successfully", ApproveRecresponse.message);

            var ApproveRecgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var ApproveRecResponse = ApproveRecgetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_RECEIVED_APPROVED, ApproveRecResponse.status);

            Assert.AreEqual(ApproveRecResponse.approved_by_name, "Admin HFE");

            DateTime ApproveRecexpectedGeneratedAt = DateTime.Today;
            DateTime ApproveRecactualGeneratedAt = (DateTime)ApproveRecResponse.approved_at;
            ApproveRecexpectedGeneratedAt = new DateTime(ApproveRecexpectedGeneratedAt.Year, ApproveRecexpectedGeneratedAt.Month, ApproveRecexpectedGeneratedAt.Day, 0, 0, 0);
            ApproveRecactualGeneratedAt = new DateTime(ApproveRecactualGeneratedAt.Year, ApproveRecactualGeneratedAt.Month, ApproveRecactualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(ApproveRecexpectedGeneratedAt, ApproveRecactualGeneratedAt, "The approved timestamp should be the same");

            Assert.AreEqual(ApproveRecResponse.Id, goId);





            //STEP 9: Close GO
            var ClosePayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = goId,
                comment = "test"
            };

            string CloseJsonPayload = JsonConvert.SerializeObject(ClosePayload);


            var CloseService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var Closeresponse = CloseService.CreateItem(EP_CloseGO, CloseJsonPayload);

            Assert.AreEqual($"Goods order withdrawn successfully.", Closeresponse.message);

            var ClosegetItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var CloseItemResponse = ClosegetItem.GetItem(EP_GetGODetailsByID + "?ID=" + goId);

            Assert.AreEqual((int)CMMS.CMMS_Status.GO_CLOSED, CloseItemResponse.status);

            Assert.AreEqual(CloseItemResponse.closed_by_name, "Admin HFE");

            DateTime CloseexpectedGeneratedAt = DateTime.Today;
            DateTime CloseactualGeneratedAt = (DateTime)CloseItemResponse.closed_at;
            CloseexpectedGeneratedAt = new DateTime(CloseexpectedGeneratedAt.Year, CloseexpectedGeneratedAt.Month, CloseexpectedGeneratedAt.Day, 0, 0, 0);
            CloseactualGeneratedAt = new DateTime(CloseactualGeneratedAt.Year, CloseactualGeneratedAt.Month, CloseactualGeneratedAt.Day, 0, 0, 0);
            Assert.AreEqual(CloseexpectedGeneratedAt, CloseactualGeneratedAt, "The closed timestamp should be the same");

            Assert.AreEqual(CloseItemResponse.Id, goId);
        }
    }
}
