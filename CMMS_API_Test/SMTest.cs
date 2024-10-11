using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CMMSAPIs.Models;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.Helper;
using Newtonsoft.Json;

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
        string EP_GetAssetCategoryList = "/api/Inventory/GetInventoryCategoryList";
        string EP_AddAssetCategory = "/api/Inventory/AddInventoryCategory";
        string EP_UpdateAssetCategory = "/api/SMMaster/UpdateAssetCategory";
        string EP_DeleteAssetCategory = "/api/SMMaster/DeleteAssetCategory";

        //Unit Measurement
        string EP_GetUnitMeasurementList = "/api/SMMaster/GetUnitMeasurementList";
        string EP_AddUnitMeasurement = "/api/SMMaster/AddUnitMeasurement";
        string EP_UpdateUnitMeasurement = "/api/SMMaster/UpdateUnitMeasurement";
        string EP_DeleteUnitMeasurement = "/api/SMMaster/DeleteUnitMeasurement";


        //MRS
        string EP_requestMRS = "/api/MRS/CreateMRS";
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

        // GO
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
        string EP_GetPurchaseData = "/api/GO/GetPurchaseData";
        string EP_getPurchaseDetailsByID = "/api/GO/getPurchaseDetailsByID";
        string EP_SubmitPurchaseOrderData = "/api/GO/SubmitPurchaseOrderData";

        [TestMethod]
        public void VerifyListofAssetType()
        {

            int FacilityId = 32;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMAssetTypes>();
            var response = ptwService.GetItemList(EP_GetAssetTypeList + "?ID=" + FacilityId);
            int ptwListCount = response.Count;
            Assert.AreEqual(1, ptwListCount);
            //Assert.AreEqual(ptwId, response[0].permitId);
            //Assert.AreEqual("Consulmable", response[0].description);
        }
        [TestMethod]
        public void VerifyCreateAssetType()
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
            var responseForItem = getItem.GetItem(EP_GetAssetTypeList + "?ID=" + myNewItemId);
            //also write case forf specofoc 
            //verify model all property too
            Assert.AreEqual(myNewItemId, responseForItem.ID);
            Assert.AreEqual("Testing AssetType USING Automation", responseForItem.asset_type);
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
            var getResponseForItem = getItem.GetItem(EP_GetAssetTypeList + "?ID=" + 30);


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
        public void VerifyCreateAssetCategory()
        {
            string payload = @"{
                                ""name"":""Test asset api"",
                                ""description"":""testing Insert query"",
                                ""calibration_required"":1
                            }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_AddAssetCategory, payload);
            int myNewItemId = response.id[0];

            var getList = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMItemCategory>();
            var responseForInsertedVAlue = getList.GetItemList(EP_GetAssetCategoryList + "?ID=" + myNewItemId);
            Assert.AreEqual(78, responseForInsertedVAlue.Count);
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
                                      }
                                  ]
                              }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_requestMRS, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Request has been submitted.", responseMessage);

        }

        [TestMethod]
        public void VerifygetMRSList()
        {
            var facilityID = 1; 
            var emp_id = 1;  
            var fromDate = "2024-09-09";
            var toDate = "2024-09-16";
            
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItemList(EP_getMRSList + "?facility_ID=" + facilityID + "&emp_id=" + emp_id + "&toDate=" + toDate + "&fromDate=" + fromDate);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 6);
        }

        [TestMethod]
        public void VerifygetMRSItems()
        {
            int ID = 100;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItemList(EP_getMRSItems + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 10);
        }
        [TestMethod]
        public void VerifygetMRSItemsBeforeIssue()
        {
            int ID = 100;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItemList(EP_getMRSItemsBeforeIssue + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSItemsWithCode()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItemList(EP_getMRSItemsWithCode + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSDetails()
        {
            var id = 189;
            var facilityID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
            var response = ptwService.GetItem(EP_getMRSDetails + "?ID=" + id + "&facility_id=" + facilityID);
            Assert.IsNotNull(response);
            Assert.AreEqual(response.ID, id);
            Assert.AreEqual(response.whereUsedTypeName, "PMTASK");
            Assert.AreEqual(response.approver_name, "Admin HFE");
        }

        [TestMethod]
        public void VerifymrsApproval()
        {
            string payload = @"{
                                ""ID"":131,
                                ""comment"": ""MRS approval for MRS 131""
                                
                               
                               
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
                                ""ID"":180,
                                ""comment"": ""MRS reject for MRS 131""
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_mrsReject, payload);
            string responseMessage = response.message;
            Assert.AreEqual("Status updated.", responseMessage);
        }

        [TestMethod]
        public void VerifygetReturnDataByID()
        {
            int ID = 100;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMMRS>();
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
                                ""isEditMode"":0,
                                ""plant_ID"":1,
                                ""requested_by_emp_ID"":1,
                                ""requestd_date"":""2023-03-03"",
                                ""flag"":1,
                                ""setAsTemplate"":3,
                                ""asset_item_ID"":4,
                                ""approval_status"":2,
                                ""return_remarks"":""Testing"",
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
            Assert.AreEqual("MRS Request Approved", responseMessage);
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
            Assert.AreEqual("MRS Request Approved", responseMessage);
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




            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.GO.CMGO>();
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




            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.GO.CMGO>();
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
                                ""approvedBy"":12

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
                                ""approvedBy"":12

                        }";

            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(EP_withdrawGO, payload);

            Assert.AreEqual("Goods order withdrawn successfully.", response.message);
        }

        [TestMethod]
        public void GetAssetBySerialNo()
        {

            string serial_number = "3000009";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.CMSMMaster>();
            var response = ptwService.GetItem(EP_GetAssetBySerialNo + "?serial_number=" + serial_number);
            Assert.AreEqual("H41111955100001", response.asset_code);
            //Assert.AreEqual(45, response.plant_ID);

        }
        [TestMethod]
        public void VerifyGetPurchaseData()
        {

            int plantID = 45;
            string empRole = "";
            DateTime fromDate = Convert.ToDateTime("2019-01-01");
            DateTime toDate = Convert.ToDateTime("2022-01-01");
            int status = 1;
            int order_type = 0;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.GO.go_items>();
            var response = ptwService.GetItemList(EP_GetPurchaseData + "?plantID=" + plantID + "&empRole=" + empRole + "&fromDate=" + fromDate + "&toDate=" + toDate + "&status=" + status + "&order_type=" + order_type);
            //Assert.AreEqual("Hero Future Solar Plant 1000MW", response[0].facilityName); 
            //Assert.AreEqual(19, response[0].orderID); 
        }
        [TestMethod]
        public void VerifyGetPurchaseDetailsByID()
        {

            int id = 19;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.GO.CMGO>();
            var response = ptwService.GetItemList(EP_getPurchaseDetailsByID + "?id=" + id);
            Assert.AreEqual(187, response[0].vendorID);

        }

        [TestMethod]
        public void VerifySubmitPurchaseOrderData()
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

    }
}
