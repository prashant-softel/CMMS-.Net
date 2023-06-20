﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        string EP_requestMRS = "/api/MRS/requestMRS";
        string EP_getMRSList = "/api/MRS/getMRSList";
        string EP_getMRSItems = "/api/MRS/getMRSItems";
        string EP_getMRSItemsBeforeIssue = "/api/MRS/getMRSItemsBeforeIssue";
        string EP_getMRSItemsWithCode = "/api/MRS/getMRSItemsWithCode";
        string EP_getMRSDetails = "/api/MRS/getMRSDetails";
        string EP_mrsApproval = "/api/MRS/mrsApproval";
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
        string EP_GOApproval = "/api/GO/GOApproval";
        string EP_withdrawGO = "/api/GO/WithdrawGO";
        string EP_GetPurchaseData = "/api/GO/GetPurchaseData";
        string EP_getPurchaseDetailsByID = "/api/GO/getPurchaseDetailsByID";
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
        string EP_RejectGoodsOrder = "/api/RequestOrder/RejectGoodsOrder";
        string EP_GetRequestOrderList = "/api/RequestOrder/GetRequestOrderList";

        [TestMethod]
        public void VerifyListofAssetType()
        {

            int FacilityId = 32;            
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetTypes>();
            var response = ptwService.GetItemList(EP_GetAssetTypeList + "?ID=" + FacilityId );
            int ptwListCount = response.Count;
            Assert.AreEqual( 1, ptwListCount);
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
                             ""isEditMode"":0,
                             ""plant_ID"":1,
                             ""requested_by_emp_ID"":1,
                             ""requestd_date"":""2023-03-03"",
                             ""flag"":1,
                             ""setAsTemplate"":3,
                             ""equipments"":[{
                                 ""equipmentID"":12,
                                 ""approval_required"":1,
                                 ""asset_type_ID"":10,
                                 ""asset_code"":""M0001""
                             }]
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_requestMRS, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Request has been submitted.", responseMessage);
        }

        [TestMethod]
        public void VerifygetMRSList()
        {
            int plant_ID = 1;
            int emp_id = 1;
            DateTime toDate = Convert.ToDateTime("2023-04-01");
            DateTime fromDate = Convert.ToDateTime("2022-01-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSList>();
            var response = ptwService.GetItemList(EP_getMRSList + "?plant_ID=" + plant_ID+ "&emp_id="+emp_id+ "&toDate="+toDate+ "&fromDate="+fromDate);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 0);
        }

        [TestMethod]
        public void VerifygetMRSItems()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRSItems>();
            var response = ptwService.GetItemList(EP_getMRSItems + "?ID=" + ID );
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItemList(EP_getMRSDetails + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }

        [TestMethod]
        public void VerifymrsApproval()
        {
            string payload = @"{
                                ""ID"":10,
                                ""isEditMode"":0,
                                ""plant_ID"":1,
                                ""requested_by_emp_ID"":1,
                                ""requestd_date"":""2023-03-03"",
                                ""flag"":1,
                                ""setAsTemplate"":3,
                                ""asset_item_ID"":4,
                                ""approval_status"":2,
                                ""return_remarks"":""Testing"",
                                ""approved_date"":""2023-03-03"",
                                ""equipments"":[{
                                    ""id"":4,
                                    ""equipmentID"":12,
                                    ""approval_required"":1,
                                    ""asset_type_ID"":10,
                                    ""asset_code"":""M0001"",
                                    ""qty"":10,
                                    ""issued_qty"":15 
                                }]
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_mrsApproval, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);
        }

        [TestMethod]
        public void VerifygetReturnDataByID()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMRETURNMRSDATA>();
            var response = ptwService.GetItemList(EP_getReturnDataByID + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }

        [TestMethod]
        public void VerifygetAssetTypeByItemID()
        {
            int ItemID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItemList(EP_getAssetTypeByItemID + "?ItemID=" + ItemID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
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
            int plantID = 45;
            string fromDate = "2001-01-01";
            string toDate = "2023-05-14";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGO>();
            var response = ptwService.GetItemList(EP_getGOList + "?plantID=" + plantID+ "&fromDate="+ fromDate+ "&toDate="+ toDate);
            int vendorID = response[0].vendorID;
            int status = response[0].status;
            string generatedBy = response[0].generatedBy;
            Assert.AreEqual(vendorID, 177);
            Assert.AreEqual(status, 6);
            Assert.AreEqual(generatedBy, "Jagatpal Singh");
        }

        [TestMethod]
        public void createGO()
        {
            string payload = @"{
                               ""purchaseID"":7,
                               ""order_type"":1,
                               ""location_ID"":1,
                               ""go_items"":[{""assetItemID"":3, ""cost"":1,""ordered_qty"":2},
                                             {""assetItemID"":4, ""cost"":1,""ordered_qty"":2}]
                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_createGO, payload);
            int myNewItemId = response.id[0];




            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGO>();
            var responseForItem = getItem.GetItemList(EP_getGOItemByID + "?ID=" + myNewItemId);
            //also write case forf specofoc 
            //verify model all property too
            Assert.AreEqual(myNewItemId, responseForItem[0].id);
        }
        [TestMethod]
        public void updateGO()
        {

            string payload = @"{
                                 ""id"":1,
                                 ""location_ID"":100,
                                   ""go_items"":[{""poID"":1,""assetItemID"":3, ""cost"":1,""ordered_qty"":2},
                                             {""poID"":2,""assetItemID"":4, ""cost"":1,""ordered_qty"":2}]
                                 }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_updateGO, payload);
            int myNewItemId = 1;




            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.CMGO>();
            var responseForItem = getItem.GetItemList(EP_getGOItemByID + "?ID=" + myNewItemId);
            Assert.AreEqual(0, responseForItem[0].location_ID);
        }

        [TestMethod]
        public void GOApproval()
        {

            string payload = @"{
                                ""id"":1,
                                ""status"":2,
                                ""remarks"":""TEsting approval"",
                                ""approvedBy"":""12""

                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_GOApproval, payload);
           
            Assert.AreEqual("Data Updated.", response.message);
        }

        [TestMethod]
        public void withdrawGO()
        {

            string payload = @"{
                                ""id"":1,
                                ""status"":2,
                                ""remarks"":""TEsting approval"",
                                ""approvedBy"":""12""

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
            Assert.AreEqual(45, response.plant_ID);

        }
        [TestMethod]
        public void GetPurchaseData()
        {

            int plantID = 45;
            string empRole = "";
            DateTime fromDate = Convert.ToDateTime("2019-01-01");
            DateTime toDate = Convert.ToDateTime("2022-01-01");
            int status = 1;
            int order_type = 0;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGOList>();
            var response = ptwService.GetItemList(EP_GetPurchaseData + "?plantID=" + plantID+ "&empRole="+ empRole+ "&fromDate="+ fromDate +"&toDate="+toDate+ "&status="+status+ "&order_type="+ order_type);
            Assert.AreEqual("Noor Abu Dhabi 1177MW", response[0].facilityName); 
            Assert.AreEqual(19, response[0].assetItemID); 
        }
        [TestMethod]
        public void GetPurchaseDetailsByID()
        {

            int id = 19;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.CMGO>();
            var response = ptwService.GetItemList(EP_getPurchaseDetailsByID + "?id=" + id);           
            Assert.AreEqual(187, response[0].vendorID);

        }

        [TestMethod]
        public void SubmitPurchaseOrderData()
        {

            string payload = @"{
                                  ""purchaseID"":1,
                                  ""facilityId"":45,
                                  ""vendor"":2,
                                  ""empId"":10,
                                  ""purchaseDate"":""2023-10-10"",
                                  ""generateFlag"":1,
                                  ""submitItems"":[{""assetCode"":""H39121448100005"", ""assetItemID"":1,""orderedQty"":10,""type"":4,""cost"":45}]
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
            Assert.AreEqual(1, response[0].ID);
            Assert.AreEqual(1, response[0].asset_ID);

        }

        [TestMethod]
        public void GetPlantStockReport()
        {

            int plantID = 45;
            DateTime fromDate = Convert.ToDateTime("2000-01-01");
            DateTime toDate = Convert.ToDateTime("2022-01-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMPlantStockOpening>();
            var response = ptwService.GetItemList(EP_GetPlantStockReport + "?plant_ID=" + plantID + "&StartDate=" + fromDate+ "&EndDate=" + toDate );
            Assert.AreEqual("Noor Abu Dhabi 1177MW", response[0].plant_name);
            Assert.AreEqual(15, response[0].assetItemID);
        }
        [TestMethod]
        public void GetEmployeeStockReport()
        {

            int plantID = 45;
            DateTime fromDate = Convert.ToDateTime("2000-01-01");
            DateTime toDate = Convert.ToDateTime("2022-01-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMEmployeeStockReport>();
            var response = ptwService.GetItemList(EP_GetEmployeeStockReport + "?plant_ID=" + plantID + "&StartDate=" + fromDate + "&EndDate=" + toDate);
            Assert.AreEqual(0, response.Count);
        }
        [TestMethod]
        public void GetFaultyMaterialReport()
        {

            int plantID = 45;
            DateTime fromDate = Convert.ToDateTime("2000-01-01");
            DateTime toDate = Convert.ToDateTime("2022-01-01");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMFaultyMaterialReport>();
            var response = ptwService.GetItemList(EP_GetFaultyMaterialReport + "?plant_ID=" + plantID + "&StartDate=" + fromDate + "&EndDate=" + toDate);
            Assert.AreEqual("3000001", response[0].replaceSerialNo);
            Assert.AreEqual(192, response[0].ID);
        }
        [TestMethod]
        public void GetEmployeeTransactionReport()
        {

            int isAllEmployees = 1;
            int plantID = 45;
            DateTime fromDate = Convert.ToDateTime("2020-01-01");
            DateTime toDate = Convert.ToDateTime("2023-05-05");
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMEmployeeTransactionReport>();
            var response = ptwService.GetItemList(EP_GetEmployeeTransactionReport + "?isAllEmployees="+ isAllEmployees + "&plant_ID=" + plantID + "&StartDate=" + fromDate + "&EndDate=" + toDate);
            Assert.AreEqual(251, response[0].fromActorID);
            Assert.AreEqual("3", response[0].fromActorType);
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
                                    ""id"":216,
                                    ""comment"":""Rejected""
                              }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_RejectGoodsOrder, payload);

            Assert.AreEqual("Rejected request order.", response.message);
        }
    }
}
