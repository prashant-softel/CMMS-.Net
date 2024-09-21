using CMMSAPIs.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        string EP_withdrawGO = "/api/GO/WithdrawGO";
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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOListByFilter>();
            var response = ptwService.GetItemList(EP_getGOList + "?facility_id=" + facilityID + "&fromDate=" + fromDate + "&toDate=" + toDate);

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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOListByFilter>();
            var response = ptwService.GetItemList(EP_GetGoodsOrderData + "?facility_id=" + facilityID + "&fromDate=" + fromDate + "&toDate=" + toDate + "&empRole=" + empRole);
            Assert.IsNotNull(response, "GO Data should not be null.");
            Assert.IsTrue(response.Count > 0, "GO Data should contain at least one order.");
        }

        [TestMethod]
        public void VerifyGetGODetailsByID()
        {
            int id = 156;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderList>();
            var response = ptwService.GetItem(EP_GetGODetailsByID + "?ID=" + id);

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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderList>();
            var response = ptwService.GetItem(EP_getGOItemByID + "?ID=" + id);
            Console.WriteLine("Expected ID: " + id);
            Console.WriteLine("Actual ID from response: " + response.purchaseID);
            int myNewItemId = response.purchaseID;
            Assert.AreEqual(myNewItemId, id);
            Assert.IsNotNull(response, "GO item should not be null.");
        }


        [TestMethod]
        public void VerifyCreateGO()
        {

            string payload = @"{
                                ""facility_id"": 1,
                                ""vendorID"": 325,                               
                                ""po_no"": ""434332"",
                                ""po_date"": ""2024-09-14"",
                                ""amount"": 323232,
                                ""currencyID"": 69,
                                ""id"": 0,
                                ""is_submit"": 1,
                                
                                ""go_items"": [
                                    {
                                        ""assetMasterItemID"": 1,
                                        ""storage_rack_no"": """",
                                        ""storage_row_no"": """",
                                        ""storage_column_no"": """",
                                        ""goItemID"": 0,
                                        ""cost"": 1,
                                        ""ordered_qty"": 1,
                                        ""paid_by_ID"": 2,
                                        ""requested_qty"": 1,
                                        ""accepted_qty"": 0,
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
                                        ""sr_no"": """"
                                    }
                                ]
                            }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_createGO, payload);
            int myNewItemId = response.id[0];

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var responseForItem = getItem.GetItem(EP_GetGODetailsByID + "?ID=" + myNewItemId);


            Assert.AreEqual(myNewItemId, responseForItem.Id);
            Assert.AreEqual((int)CMMS.CMMS_Status.GO_SUBMITTED, responseForItem.status);

            Assert.AreEqual(1, responseForItem.facility_id);
            Assert.AreEqual(325, responseForItem.vendorID);
            Assert.AreEqual("434332", responseForItem.po_no);
            Assert.AreEqual(new DateTime(2024, 9, 14), responseForItem.po_date);
            Assert.AreEqual(323232, responseForItem.amount);
            Assert.AreEqual(69, responseForItem.currencyID);
            Assert.AreEqual("Indian Rupee", responseForItem.currency);

            Assert.AreEqual(1, responseForItem.GODetails[0].assetMasterItemID);
            Assert.AreEqual(0, responseForItem.GODetails[0].requestOrderId);
            Assert.AreEqual(1, responseForItem.GODetails[0].cost);
            Assert.AreEqual(1, responseForItem.GODetails[0].ordered_qty);
            Assert.AreEqual(2, responseForItem.GODetails[0].paid_by_ID);
            Assert.AreEqual(1, responseForItem.GODetails[0].requested_qty);
            Assert.AreEqual("DC Disconnector Switch", responseForItem.GODetails[0].assetItem_Name);
            Assert.AreEqual(2, responseForItem.GODetails[0].spare_status);
            Assert.AreEqual("operator", responseForItem.GODetails[0].paid_by_name);
            Assert.AreEqual("SMB", responseForItem.GODetails[0].cat_name);
            Assert.AreEqual("Spare", responseForItem.GODetails[0].asset_type);
            Assert.AreEqual("S043213001", responseForItem.GODetails[0].asset_code);


            Assert.AreEqual(responseForItem.submitted_by_name, "Admin HFE");
            DateTime expectedGeneratedAt = DateTime.Today;
            DateTime actualGeneratedAt = (DateTime)responseForItem.submitted_at;
            expectedGeneratedAt = new DateTime(expectedGeneratedAt.Year, expectedGeneratedAt.Month, expectedGeneratedAt.Day, 0, 0, 0);
            actualGeneratedAt = new DateTime(actualGeneratedAt.Year, actualGeneratedAt.Month, actualGeneratedAt.Day, 0, 0, 0);

            Assert.AreEqual(expectedGeneratedAt, actualGeneratedAt, "The submitted timestamp should be the same");

            Assert.AreEqual(responseForItem.Id, response.id[0]);
        }


        [TestMethod]
        public void VerifyupdateGO()
        {

            string payload = @"{
                                  ""facility_id"": 1,
                                  ""order_type"": 1,
                                  ""location_ID"": 1,
                                  ""vendorID"": 0,
                                  ""purchaseDate"": ""0001-01-01"",
                                  ""challan_no"": """",
                                  ""challan_date"": ""0001-01-01"",
                                  ""po_no"": ""434343"",
                                  ""po_date"": ""2024-09-14"",
                                  ""freight"": """",
                                  ""receivedAt"": ""0001-01-01"",
                                  ""no_pkg_received"": """",
                                  ""lr_no"": """",
                                  ""freight_value"": """",
                                  ""inspection_report"": """",
                                  ""condition_pkg_received"": """",
                                  ""vehicle_no"": """",
                                  ""gir_no"": """",
                                  ""closedBy"": null,
                                  ""job_ref"": """",
                                  ""amount"": 87878,
                                  ""currencyID"": 69,
                                  ""id"": 157,
                                  ""is_submit"": 1,
                                  ""go_items"": [
                                      {
                                          ""assetMasterItemID"": 2599,
                                          ""storage_rack_no"": """",
                                          ""storage_row_no"": """",
                                          ""storage_column_no"": """",
                                          ""goItemID"": 2599,
                                          ""cost"": 1,
                                          ""ordered_qty"": 1,
                                          ""paid_by_ID"": 2,
                                          ""requested_qty"": 1,
                                          ""accepted_qty"": 0,
                                          ""received_qty"": 0,
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
                                          ""sr_no"": """"
                                      }
                                  ]
                            }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_updateGO, payload);

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


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_GOApproval, payload);

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

            Assert.AreEqual(goResponse.Id, response.id[0]);
        }

        /*[TestMethod]
        public void VerifyApproveGO()
        {
            string payload = @"{
                                ""id"": 156,
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

            Assert.AreEqual(goResponse.Id, response.id[0]);
        }*/

        [TestMethod]
        public void VerifyRejectGO()
        {
            string payload = @"{
                                ""id"": 128,
                                ""comment"": ""test reject api"",
                                ""facilityId"": 1
                                 }";


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_RejectGO, payload);

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

            Assert.AreEqual(goResponse.Id, response.id[0]);
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

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateGOReceive, payload);

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

            Assert.AreEqual(goResponse.Id, response.id[0]);
        }

        [TestMethod]
        public void VerifyApproveGOReceive()
        {
            string payload = @"{
                                ""id"": 139,
                                ""comment"": ""test"",
                                ""facilityId"": 1
                                 }";


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_ApproveGOReceive, payload);

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


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_RejectGOReceive, payload);

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

            Assert.AreEqual(goResponse.Id, response.id[0]);


        }

        [TestMethod]
        public void VerifyCloseGo()
        {
            string payload = @"{
                                ""id"": 139,
                                ""comment"": ""test close"",
                                ""facilityId"": 1
                                 }";


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_CloseGO, payload);

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

            Assert.AreEqual(goResponse.Id, response.id[0]);


        }

        

    }
}
