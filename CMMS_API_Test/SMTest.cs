using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CMMSAPIs.Models;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.Helper;

namespace CMMS_API_Test
{
    //define endpoints here
    [TestClass]
   public class SMTest
    {
        //Asset Type Master
        string EP_GetAssetTypeList = "/api/SMMaster/GetAssetTypeList";
        string EP_AddAssetType = "/api/SMMaster/AddAssetType";
        string EP_UpdateAssetType = "/api/SMMaster/UpdateAssetType";
        string EP_DeleteAssetType = "/api/SMMaster/DeleteAssetType";
        string EP_GetAssetBySerialNo = "/api/SMMaster/GetAssetBySerialNo";
        string EP_GetAssetDataList = "/api/SMMaster/GetAssetDataList";

        //Asset Category Master
        string EP_GetAssetCategoryList = "/api/SMMaster/GetAssetCategoryList";
        string EP_AddAssetCategory = "/api/SMMaster/AddAssetCategory";
        string EP_UpdateAssetCategory = "/api/SMMaster/UpdateAssetCategory";
        string EP_DeleteAssetCategory = "/api/SMMaster/DeleteAssetCategory";

        //Unit Measurement
        string EP_GetUnitMeasurementList = "/api/SMMaster/GetUnitMeasurementList";
        string EP_AddUnitMeasurement = "/api/SMMaster/AddUnitMeasurement";
        string EP_UpdateUnitMeasurement = "/api/SMMaster/UpdateUnitMeasurement";
        string EP_DeleteUnitMeasurement = "/api/SMMaster/DeleteUnitMeasurement";


        //MRS
        string EP_CreateMRS = "/api/MRS/CreateMRS";
        string EP_updateMRS = "/api/MRS/updateMRS";
        string EP_getMRSList = "/api/MRS/getMRSList";
        string EP_getMRSItems = "/api/MRS/getMRSItems";
        string EP_getMRSItemsBeforeIssue = "/api/MRS/getMRSItemsBeforeIssue";
        string EP_getMRSItemsWithCode = "/api/MRS/getMRSItemsWithCode";
        string EP_getMRSDetails = "/api/MRS/getMRSDetails";
        string EP_mrsApproval = "/api/MRS/mrsApproval";
        string EP_mrsReject = "/api/MRS/mrsReject";
        string EP_getReturnDataByID = "/api/MRS/getReturnDataByID";
        string EP_getAssetTypeByItemID = "/api/MRS/getAssetTypeByItemID";
        string EP_mrsReturn = "/api/MRS/mrsReturn";
        string EP_mrsReturnApproval = "/api/MRS/mrsReturnApproval";
        string EP_getLastTemplateData = "/api/MRS/getLastTemplateData";
        string EP_GetAssetItems = "/api/MRS/GetAssetItems";

        // GO
        string EP_getGOList = "/api/GO/GetGOList";
        string EP_getGOItemByID = "/api/GO/GetGOItemByID";
        string EP_createGO = "/api/GO/CreateGO";
        string EP_updateGO = "/api/GO/UpdateGO";
        string EP_GOApproval = "/api/GO/ApproveGO";
        string EP_withdrawGO = "/api/GO/CloseGO";
        string EP_GetPurchaseData = "/api/GO/GetPurchaseData";
        string EP_getPurchaseDetailsByID = "/api/GO/GetGODetailsByID";
        string EP_SubmitPurchaseOrderData = "/api/GO/SubmitPurchaseOrderData";

        //Reports

        string EP_GetPlantStockReport = "/api/SMReports/GetPlantStockReport";
        string EP_GetEmployeeStockReport = "/api/SMReports/GetEmployeeStockReport";
        string EP_GetFaultyMaterialReport = "/api/SMReports/GetFaultyMaterialReport";
        string EP_GetEmployeeTransactionReport = "/api/SMReports/GetEmployeeTransactionReport";

        //Request Order

        string EP_CreateRequestOrder = "/api/RequestOrder/CreateRequestOrder";
        string EP_UpdateRO = "/api/RequestOrder/UpdateRO";
        string EP_DeleteRequestOrder = "/api/RequestOrder/DeleteRequestOrder";
        string EP_ApproveRequestOrder = "/api/RequestOrder/ApproveRequestOrder";
        string EP_RejectGoodsOrder = "/api/GO/RejectGO";
        string EP_GetRequestOrderList = "/api/RequestOrder/GetRequestOrderList";



        [TestMethod]
        public void VerifyListofAssetType()
        {

            int FacilityId = 32;            
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetTypes>();
            var response = ptwService.GetItemList(EP_GetAssetTypeList + "?ID=" + FacilityId );
            int ptwListCount = response.Count;
            Assert.AreEqual( 0, ptwListCount);
            //Assert.AreEqual(ptwId, response[0].permitId);
            //Assert.AreEqual("Consulmable", response[0].description);
        }
        [TestMethod]
        public void CreateAssetType()
        {
            var asset_type = "";
            string payload = @"{
                            ""asset_type"":""Testing AssetType USING Automation""
                           
                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_AddAssetType, payload);
            int myNewItemId = response.id[0];

            //var getList = new CMMS_Services.APIService<CMMSAPIs.Models.PM.CMScheduleData>();
            //var responseForInsertedVAlue = getList.GetItemList(EP_GetAssetTypeList + "?ID=" + myNewItemId);
            ////also write case forf specofoc 
            //Assert.AreEqual(1, responseForInsertedVAlue.Count);


            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetTypes>();
            var responseForItem = getItem.GetItemList(EP_GetAssetTypeList + "?ID=" + myNewItemId);
            //also write case forf specofoc 
            //verify model all property too
            Assert.AreEqual(myNewItemId, responseForItem[0].ID);
            Assert.AreEqual("Testing AssetType USING Automation", responseForItem[0].asset_type);
        }
        [TestMethod]
        public void VerifyUpdateAssetType()
        {
            string payload = @"{
                            ""asset_type"":""Testing AssetType"",
                            ""ID"":30
                           
                        }";


            //Get the specific AssetType
            //ReadOnlyMemory its attributes

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetTypes>();
            var getResponseForItem = getItem.GetItemList(EP_GetAssetTypeList + "?ID=" + 30);


            //Now we will update the attributes
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();

            var response = ptwService.CreateItem(EP_UpdateAssetType, payload);
            string testMessage = response.message;
            Assert.AreEqual("Asset type updated successfully.", testMessage);

            //Get thre same item again
            //Get its asset_type
            Assert.AreEqual("Asset type updated successfully.", testMessage);
        }
        [TestMethod]
        public void VerifyDeleteAssetType()
        {
            //follow same proce
            string payload = @"{
                            ""ID"":33
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_DeleteAssetType, payload);
            string responseMessage = response.message;
            string testMessage = response.message;
            Assert.AreEqual("Asset type deleted successfully.", responseMessage);
        }

        // Asset Category

        [TestMethod]
        public void VerifyListofAssetCategory()
        {

            int id = 2;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMItemCategory>();
            var response = ptwService.GetItemList(EP_GetAssetCategoryList + "?ID=" + id);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);

        }
        [TestMethod]
        public void CreateAssetCategory()
        {
            string payload = @"{
                            ""cat_name"":""Testing AssetType FRom test automation""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_AddAssetCategory, payload);
            int myNewItemId = response.id[0];

            var getList = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMItemCategory>();
            var responseForInsertedVAlue = getList.GetItemList(EP_GetAssetCategoryList + "?ID=" + myNewItemId);
            Assert.AreEqual(1, responseForInsertedVAlue.Count);
        }
        [TestMethod]
        public void VerifyUpdateAssetCategory()
        {
            string payload = @"{
                            ""cat_name"":""Testing Asset Category"",
                            ""ID"":1
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateAssetCategory, payload);        
            string testMessage = response.message;
            Assert.AreEqual("Asset category updated successfully.", testMessage);
        }
        [TestMethod]
        public void VerifyDeleteAssetCategory()
        {
            string payload = @"{
                            ""acID"":1
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_DeleteAssetCategory, payload);
            string testMessage = response.message;
            Assert.AreEqual("Asset category deleted.", testMessage);
        }


        //Unit measurement

        [TestMethod]
        public void VerifyUnitMeasurementList()
        {
            int ID = 12;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMUnitMeasurement>();
            var response = ptwService.GetItemList(EP_GetUnitMeasurementList + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifyCreateUnitMeasurement()
        {
            string payload = @"{
                            ""name"":""testing insert query"",
                            ""decimal_status"":1,
                            ""spare_multi_selection"":1
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_AddUnitMeasurement, payload);
            int myNewItemId = response.id[0];
           
            var getList = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMUnitMeasurement>();
            var responseForInsertedVAlue = getList.GetItemList(EP_GetUnitMeasurementList + "?ID=" + myNewItemId);
            int ListCount = responseForInsertedVAlue.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifyUpdateUnitMeasurement()
        {
            string payload = @"{
                            ""name"":""testing insert query"",
                            ""decimal_status"":1,
                            ""spare_multi_selection"":1,
                            ""ID"":13
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateUnitMeasurement, payload);
            string testMessage = response.message;
            Assert.AreEqual("Unit measurement updated successfully.", testMessage);
        }
        [TestMethod]
        public void VerifyDeleteUnitMeasurement()
        {
            string payload = @"{
                            ""umID"":1
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_DeleteUnitMeasurement, payload);
            string testMessage = response.message;
            Assert.AreEqual("Unit measurement deleted.", testMessage);
        }


        //MRS category

        [TestMethod]
        public void VerifyrequestMRS()
        {
            string payload = @"{
                                   ""facility_ID"":45,                            
                                 ""setAsTemplate"":4,
                                ""activity"":""111222"",
                                ""whereUsedType"":0,
                                ""whereUsedTypeId"":3356,
                                ""return_remarks"":""Testing on live"",
                                    ""equipments"":[{
                                                ""id"":5,
                                                ""equipmentID"":12,
                                                ""approval_required"":1,
                                                ""asset_type_ID"":10,
                                                ""asset_code"":""M0001"",
                                                ""requested_qty"":10,
                                                ""issued_qty"":15 
                                         }]
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_CreateMRS, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Request has been submitted.", responseMessage);
        }

        [TestMethod]
        public void VerifyUpdateMRS()
        {
            string payload = @"{
                                 ""ID"":122,
                                ""facility_ID"":45,                            
                                 ""setAsTemplate"":3,
                                ""activity"":""23"",
                                ""whereUsedType"":7432,
                                ""whereUsedTypeId"":35,
                                ""return_remarks"":""Testing on live for update"",                          
                                ""equipments"":[{
                                    ""id"":12,
                                    ""equipmentID"":10,
                                    ""approval_required"":1,
                                    ""asset_type_ID"":10,
                                    ""asset_code"":""M0005"",
                                    ""requested_qty"":50,
                                    ""issued_qty"":15 
                                }]
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_updateMRS, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Request has been updated.", responseMessage);
        }


        [TestMethod]
        public void VerifygetMRSList()
        {
            int facility_ID = 45;
            int emp_id = 2;
            string toDate = "2023-07-30";
            string fromDate = "2022-04-23";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var response = ptwService.GetItemList(EP_getMRSList + "?facility_ID=" + facility_ID + "&emp_id="+emp_id+ "&toDate="+toDate+ "&fromDate="+fromDate);
            int ListCount = response.Count;
            Assert.AreEqual(response[0].ID, 110);
            Assert.AreEqual(response[0].status, 321);
            Assert.AreEqual(response[0].activity, "111222");
        }

        [TestMethod]
        public void VerifygetMRSItems()
        {
            int ID = 110;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSItems>();
            var response = ptwService.GetItemList(EP_getMRSItems + "?ID=" + ID );
            int ListCount = response.Count;
            Assert.AreEqual(response[0].ID, 251);
            Assert.AreEqual(response[0].asset_item_ID, 12);
            Assert.AreEqual(response[0].asset_MDM_code, "H39121703100011");
            Assert.AreEqual(response[0].asset_type_ID, 1);
            Assert.AreEqual(response[0].approval_status, 2);
            Assert.AreEqual(response[0].asset_name, "Cable Tie UV Protected 200MM");
        }
        [TestMethod]
        public void VerifygetMRSItemsBeforeIssue()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSItemsBeforeIssue>();
            var response = ptwService.GetItemList(EP_getMRSItemsBeforeIssue + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSItemsWithCode()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSItemsBeforeIssue>();
            var response = ptwService.GetItemList(EP_getMRSItemsWithCode + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSDetails()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var response = ptwService.GetItemList(EP_getMRSDetails + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }

        [TestMethod]
        public void VerifymrsApproval()
        {
            string payload = @"{
                                  ""id"": 122,
                                   ""comment"": ""MRS Approval""
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_mrsApproval, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);
        }

        [TestMethod]
        public void VerifymrsReject()
        {
            string payload = @"{
                                  ""id"": 122,
                                   ""comment"": ""MRS Rejected""
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_mrsReject, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);
        }


        [TestMethod]
        public void VerifygetReturnDataByID()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMRETURNMRSDATA>();
            var response = ptwService.GetItem(EP_getReturnDataByID + "?ID=" + ID);
           
            Assert.AreEqual(1,response.ID);
            Assert.AreEqual(1,response.mrs_ID);
            Assert.AreEqual(5,response.requested_qty);
            Assert.AreEqual(500,response.available_qty);
        }

        [TestMethod]
        public void VerifygetAssetTypeByItemID()
        {
            int ItemID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSAssetTypeList>();
            var response = ptwService.GetItem(EP_getAssetTypeByItemID + "?ItemID=" + ItemID);
            
            Assert.AreEqual(1, response.ID);
        }

        [TestMethod]
        public void VerifymrsReturn()
        {
            string payload = @"{
                            ""ID"":10,
                            ""isEditMode"":1,
                            ""plant_ID"":1,
                            ""requested_by_emp_ID"":1,
                            ""returnDate"":""2023-03-03"",
                            ""requestd_date"":""2023-04-04"",
                            ""flag"":1,
                            ""setAsTemplate"":3,
                            ""asset_item_ID"":4,
                            ""approval_status"":2,
                            ""return_remarks"":""Testing"",
                            ""item_condition"":1,
                            ""status"":5,
                            ""equipments"":[{
                                ""id"":4,
                                ""equipmentID"":12,
                                ""approval_required"":1,
                                ""asset_type_ID"":10,
                                ""return_remarks"":""Test remarks"",
                                ""qty"":10,
                                ""requested_qty"":15 
                            }]
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_mrsReturn, payload);
            string responseMessage = response.message;
            Assert.AreEqual("MRS return submitted.", responseMessage);
        }

        [TestMethod]
        public void VerifymrsReturnApproval()
        {
            string payload = @"{
                                 ""ID"":10,
                                 ""approved_by_emp_ID"":2,
                                 ""approved_date"":""2023-04-04"",
                                 ""return_remarks"":""Testing return remarks"",
                                 ""approval_status"":1,
                                 ""equipments"":[{
                                       ""id"":4,
                                       ""returned_qty"":10,
                                       ""return_remarks"":""testing""
                                   }]
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_mrsReturnApproval, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Transaction details failed.", responseMessage);
        }

        // GOODS ORDER API'S

        [TestMethod]
        public void VerifygetGOList()
        {
            int facility_id = 45;
            string fromDate = "2001-01-01";
            string toDate = "2023-07-30";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderDetailList>();
            var response = ptwService.GetItemList(EP_getGOList + "?facility_id=" + facility_id + "&fromDate="+ fromDate+ "&toDate="+ toDate);
            Assert.AreEqual(response[0].ID, 4);
            Assert.AreEqual(response[0].purchaseID, 2);
            Assert.AreEqual(response[0].asset_name, "Fuse - String DC 32Amps 1000V");
            Assert.AreEqual(response[0].facility_name, "Hero Future Solar Plant 1000MW");
            Assert.AreEqual(response[0].asset_code, "H39121625100001");
            Assert.AreEqual(response[0].accepted_qty, 500);
        }

        [TestMethod]
        public void createGO()
        {
            string payload = @"{
                               ""facility_id"":45,
                                 ""order_type"":10,
                                 ""location_ID"":63,
                                 ""vendorID"":3,
                                 ""purchaseDate"":""2023-07-10"",
                                 ""challan_no"":""CH55125"",   
                                 ""challan_date"":""2023-07-15"",
                                 ""po_no"":""PL784554"",
                                 ""po_date"":""2023-07-16"",
                                 ""freight"":""S4"",
                                 ""received_on"":""2023-07-01"",
                                 ""no_pkg_received"":""2"",
                                 ""lr_no"":""56894655"",
                                 ""condition_pkg_received"":""Bad condition"",
                                 ""vehicle_no"":""MH01PL4512"",
                                 ""gir_no"":""PL45454"",
                                 ""amount"":1500,
                                 ""currencyID"":1,
                                 ""go_items"":[{""assetItemID"":3, ""cost"":100,""ordered_qty"":2},
                                        {""assetItemID"":4, ""cost"":1530,""ordered_qty"":2}]
                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_createGO, payload);
            int myNewItemId = response.id[0];




            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderList>();
            var responseForItem = getItem.GetItem(EP_getGOItemByID + "?ID=" + myNewItemId);
            //also write case forf specofoc 
            //verify model all property too
            Assert.AreEqual(myNewItemId, responseForItem.purchaseID);
        }
        [TestMethod]
        public void updateGO()
        {

            string payload = @"{
                                    ""id"":1042,
                                     ""location_ID"":100,
                                     ""vendorID"":8,
                                     ""purchaseDate"":""0001-01-01"",
                                     ""challan_no"":""567"",   
                                     ""challan_date"":""0001-01-01"",
                                     ""po_no"":""786507"",
                                     ""po_date"":""0001-01-01"",
                                     ""freight"":"""",
                                     ""received_on"":""0001-01-01"",
                                     ""no_pkg_received"":"""",
                                     ""lr_no"":""678"",
                                     ""condition_pkg_received"":"""",
                                     ""vehicle_no"":""987"",
                                     ""gir_no"":""987"",
                                     ""job_ref"":""765"",
                                     ""closedBy"":null,
                                     ""amount"":0,
                                     ""currency"":""6"",
                                       ""go_items"":[{""poID"":1,""assetItemID"":3, ""cost"":1,""ordered_qty"":2},
                                                 {""poID"":2,""assetItemID"":4, ""cost"":1,""ordered_qty"":2}]
                                 }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_updateGO, payload);
            int myNewItemId = 1;




            //var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGoodsOrderList>();
            //var responseForItem = getItem.GetItemList(EP_getGOItemByID + "?ID=" + myNewItemId);
            Assert.AreEqual("Goods order updated successfully.", response.message);
        }

        [TestMethod]
        public void GOApproval()
        {

            string payload = @"{
                                 ""id"": 237,
                                 ""comment"": ""TEsting approval""

                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_GOApproval, payload);
           
            Assert.AreEqual("Approved goods order successfully.", response.message);
        }

        [TestMethod]
        public void withdrawGO()
        {

            string payload = @"{
                                ""id"":122,
                                ""remarks"":""TEsting approval""
                           

                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_withdrawGO, payload);

            Assert.AreEqual("Goods order withdrawn successfully.", response.message);
        }

        [TestMethod]
        public void GetAssetBySerialNo()
        {

            string serial_number = "3000009";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetBySerialNo>();
            var response = ptwService.GetItem(EP_GetAssetBySerialNo + "?serial_number=" + serial_number);
            Assert.AreEqual("H41111955100001", response.asset_code);
            Assert.AreEqual(0, response.plant_ID);

        }
        [TestMethod]
        public void GetPurchaseData()
        {

            int facilityID = 45;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMPURCHASEDATA>();
            var response = ptwService.GetItemList(EP_GetPurchaseData + "?facilityID=" + facilityID);
            Assert.AreEqual("Hero Future Solar Plant 1000MW", response[0].facilityName); 
            Assert.AreEqual(1, response[0].orderID); 
            Assert.AreEqual(302, response[0].status); 
            Assert.AreEqual(177, response[0].vendorID); 
        }
        [TestMethod]
        public void GetPurchaseDetailsByID()
        {

            int id = 225;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOMaster>();
            var response = ptwService.GetItem(EP_getPurchaseDetailsByID + "?id=" + id);           
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(45, response.facility_id);
            Assert.AreEqual(3, response.vendorID);
            Assert.AreEqual(304, response.status);
            Assert.AreEqual(2, response.accepted_qty);
            Assert.AreEqual(0, response.location_ID);

        }

        [TestMethod]
        public void SubmitPurchaseOrderData()
        {

            string payload = @"{
                                    ""purchaseID"": 1,
                                    ""submitItems"": [
                                        {
                                            ""assetCode"": ""H39121448100005"",
                                            ""assetItemID"": 1,
                                            ""orderedQty"": 10,
                                            ""type"": 4,
                                            ""cost"": 45
                                        }
                                    ]
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_SubmitPurchaseOrderData, payload);

            Assert.AreEqual("Goods order submitted successfully.", response.message);
        }

        [TestMethod]
        public void getLastTemplateData()
        {

            int id = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItem(EP_getLastTemplateData + "?ID=" + id);
            Assert.AreEqual(Convert.ToDecimal(5), response.requested_qty);
            Assert.AreEqual(1, response.asset_item_ID);

        }
        [TestMethod]
        public void GetAssetItems()
        {

            int id = 45;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetItem>();
            var response = ptwService.GetItemList(EP_GetAssetItems + "?plantID=" + id);
            Assert.AreEqual(0, response.Count);
     

        }

        [TestMethod]
        public void GetPlantStockReport()
        {

            string facility_id = "45,50";
            DateTime fromDate = Convert.ToDateTime("2000-01-01");
            DateTime toDate = Convert.ToDateTime("2022-01-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMPlantStockOpening>();
            var response = ptwService.GetItemList(EP_GetPlantStockReport + "?facility_id=" + facility_id + "&StartDate=" + fromDate+ "&EndDate=" + toDate );
            Assert.AreEqual("Hero Future Solar Plant 1000MW", response[0].facilityName);
            Assert.AreEqual("Cotton Waste", response[0].asset_name);
            Assert.AreEqual("H11121802100002", response[0].asset_code);
            Assert.AreEqual(1, response[0].asset_type_ID);
            Assert.AreEqual(Convert.ToDecimal(306.20), response[0].Opening);
            Assert.AreEqual(Convert.ToDecimal(338.70), response[0].inward);
            Assert.AreEqual(Convert.ToDecimal(32.50), response[0].outward);
        }
        [TestMethod]
        public void GetEmployeeStockReport()
        {

            int facility_id = 45;
            int Emp_id = 45;
            string itemID = "1,2,3";
            DateTime fromDate = Convert.ToDateTime("2002-01-01");
            DateTime toDate = Convert.ToDateTime("2023-05-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMEmployeeStockReport>();
            var response = ptwService.GetItemList(EP_GetEmployeeStockReport + "?facility_id=" + facility_id + "&Emp_id="+Emp_id + "&StartDate=" + fromDate + "&EndDate=" + toDate + "&itemID="+ itemID);
          
            Assert.AreEqual(0, response.Count);
        }
        [TestMethod]
        public void GetFaultyMaterialReport()
        {

            int facility_id = 45;
            string itemID = "1,2,3";
            DateTime fromDate = Convert.ToDateTime("2001-01-01");
            DateTime toDate = Convert.ToDateTime("2023-05-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMFaultyMaterialReport>();
            var response = ptwService.GetItemList(EP_GetFaultyMaterialReport + "?facility_id=" + facility_id + "&itemID="+ itemID + "&StartDate=" + fromDate + "&EndDate=" + toDate);
            Assert.AreEqual(0, response.Count);

        }
        [TestMethod]
        public void GetEmployeeTransactionReport()
        {

            int isAllEmployees = 0;
            int Emp_ID = 185;
            string facility_id = "45,50";
            DateTime fromDate = Convert.ToDateTime("2020-01-01");
            DateTime toDate = Convert.ToDateTime("2023-05-05");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMEmployeeTransactionReport>();
            var response = ptwService.GetItemList(EP_GetEmployeeTransactionReport + "?isAllEmployees="+ isAllEmployees + "&facility_id=" + facility_id + "&Emp_ID="+ Emp_ID + "&StartDate=" + fromDate + "&EndDate=" + toDate);
            Assert.AreEqual(0, response.Count);

        }

        [TestMethod]
        public void GetRequestOrderList()
        {
            int plantID = 45;
            string fromDate = "2020-01-01";
            string toDate = "2023-06-23";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMRequestOrder>();
            var response = ptwService.GetItemList(EP_GetRequestOrderList + "?plantID=" + plantID + "&fromDate=" + fromDate + "&toDate=" + toDate);
            Assert.AreEqual("Trinity Touch", response[0].vendor_name);
            Assert.AreEqual(216, response[0].requestID);
        }

        [TestMethod]
        public void CreateRequestOrder()
        {

            string payload = @"{
                                ""plantID"":45,
                                ""order_type"":1,
                                ""location_ID"":1,
                                ""vendorID"":108,
                                ""requestDate"":""0001-01-01"",
                                ""challan_no"":"""",   
                                ""challan_date"":""0001-01-01"",
                                ""freight"":"""",
                                ""received_on"":""0001-01-01"",
                                ""no_pkg_received"":"""",
                                ""lr_no"":"""",
                                ""condition_pkg_received"":"""",
                                ""vehicle_no"":"""",
                                ""gir_no"":"""",
                                ""job_ref"":"""",
                                ""amount"":0,
                                ""currency"":"""",
                                ""go_items"":[{""assetItemID"":3, ""cost"":1,""ordered_qty"":2, ""asset_type_ID"":1},
                                              {""assetItemID"":4, ""cost"":1,""ordered_qty"":2, ""asset_type_ID"":3}]
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_CreateRequestOrder, payload);

            Assert.AreEqual("Request order created successfully.", response.message);
        }

        [TestMethod]
        public void UpdateRO()
        {
            string payload = @"{
                                 ""id"":216,
                                 ""vendorID"":21,
                                 ""request_Date"":""2023-06-20"",
                                 ""challan_no"":"""",   
                                 ""challan_date"":""2023-06-21"",
                                 ""freight"":"""",
                                 ""received_on"":""2023-06-23"",
                                 ""no_pkg_received"":"""",
                                 ""lr_no"":"""",
                                 ""condition_pkg_received"":"""",
                                 ""vehicle_no"":"""",
                                 ""gir_no"":"""",
                                 ""job_ref"":"""",
                                 ""amount"":0,
                                 ""currency"":"""",
                                   ""go_items"":[{""requestID"":1,""assetItemID"":3, ""cost"":1,""ordered_qty"":99},
                                                 {""requestID"":2,""assetItemID"":4, ""cost"":1,""ordered_qty"":99}]
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_UpdateRO, payload);

            Assert.AreEqual("Request order updated successfully.", response.message);
        }

        [TestMethod]
        public void DeleteRequestOrder()
        {
            string payload = @"{
                                 ""RO_ID"":216
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_DeleteRequestOrder, payload);

            Assert.AreEqual("Request order deleted.", response.message);
        }

        [TestMethod]
        public void ApproveRequestOrder()
        {
            string payload = @"{
                                    ""id"":216,
                                    ""comment"":""Test""
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_ApproveRequestOrder, payload);

            Assert.AreEqual("Approved request order successfully.", response.message);
        }
        [TestMethod]
        public void RejectGoodsOrder()
        {
            string payload = @"{
                                    ""id"":237,
                                    ""comment"":""Rejected""
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_RejectGoodsOrder, payload);

            Assert.AreEqual("Rejected goods order.", response.message);
        }


        [TestMethod]
        public void GetAssetDataList()
        {
            int facility_id = 45;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetTypes>();
            var response = ptwService.GetItemList(EP_GetAssetDataList + "?facility_id=" + facility_id);
            Assert.AreEqual(0, response[0].ID);
            Assert.AreEqual("Consulmable", response[0].asset_type);
        }
    }
}
