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

        [TestMethod]
        public void VerifyListofAssetType()
        {

            int FacilityId = 32;            
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.smassettypes>();
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


            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.smassettypes>();
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

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.SM.smassettypes>();
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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.ItemCategory>();
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

            var getList = new CMMS_Services.APIService<CMMSAPIs.Models.SM.SMItemCategory>();
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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.UnitMeasurement>();
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
           
            var getList = new CMMS_Services.APIService<CMMSAPIs.Models.SM.UnitMeasurement>();
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
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
            var response = ptwService.GetItemList(EP_getMRSList + "?plant_ID=" + plant_ID+ "&emp_id="+emp_id+ "&toDate="+toDate+ "&fromDate="+fromDate);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }

        [TestMethod]
        public void VerifygetMRSItems()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
            var response = ptwService.GetItemList(EP_getMRSItems + "?ID=" + ID );
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSItemsBeforeIssue()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
            var response = ptwService.GetItemList(EP_getMRSItemsBeforeIssue + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSItemsWithCode()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
            var response = ptwService.GetItemList(EP_getMRSItemsWithCode + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }
        [TestMethod]
        public void VerifygetMRSDetails()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
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
        public void VerifygetReturnDataByID()
        {
            int ID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
            var response = ptwService.GetItemList(EP_getReturnDataByID + "?ID=" + ID);
            int ListCount = response.Count;
            Assert.AreEqual(ListCount, 1);
        }

        [TestMethod]
        public void VerifygetAssetTypeByItemID()
        {
            int ItemID = 1;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.SM.MRS>();
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
    }
}
