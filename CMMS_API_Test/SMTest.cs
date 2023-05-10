using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CMMSAPIs.Models;
using CMMSAPIs.BS.Facility;

namespace CMMS_API_Test
{
    //define endpoints here
    [TestClass]
   public class SMTest
    {
        //Asset Type Master
        string EP_GetAssetTypeList = "/api/SMMaster/GetAssetTypeList";
        string AddAssetType = "/api/SMMaster/AddAssetType";
        string UpdateAssetType = "/api/SMMaster/UpdateAssetType";
        string DeleteAssetType = "/api/SMMaster/DeleteAssetType";

        //Asset Category Master
        string GetAssetCategoryList = "/api/SMMaster/GetAssetCategoryList";
        string AddAssetCategory = "/api/SMMaster/AddAssetCategory";
        string UpdateAssetCategory = "/api/SMMaster/UpdateAssetCategory";
        string DeleteAssetCategory = "/api/SMMaster/DeleteAssetCategory";

        //Unit Measurement
        string GetUnitMeasurementList = "api/SMMaster/GetUnitMeasurementList";
        string AddUnitMeasurement = "api/SMMaster/AddUnitMeasurement";
        string UpdateUnitMeasurement = "api/SMMaster/UpdateUnitMeasurement";
        string DeleteUnitMeasurement = "api/SMMaster/DeleteUnitMeasurement";



        
        [TestMethod]
        public void VerifyListofAssetType()
        {

            int FacilityId = 1;            
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitList>();
            var response = ptwService.GetItemList(EP_GetAssetTypeList + "?ID=" + FacilityId );
            int ptwListCount = response.Count;
            Assert.AreEqual( 1, ptwListCount);
            //Assert.AreEqual(ptwId, response[0].permitId);
            //Assert.AreEqual("Consulmable", response[0].description);
        }
        [TestMethod]
        public void CreateAssetType()
        {
            string payload = @"{
                            ""asset_type"":""Testing AssetType USING Automation""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(AddAssetType, payload);
            int myNewItemId = response.id[0];

            var responseForInsertedVAlue = ptwService.GetItemList(EP_GetAssetTypeList + "?ID=" + myNewItemId);
            Assert.AreEqual(1, responseForInsertedVAlue.Count);
         
        }
        [TestMethod]
        public void VerifyUpdateAssetType()
        {
            string payload = @"{
                            ""asset_type"":""Testing AssetType"",
                            ""ID"":""1""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItems(UpdateAssetType, payload);
            int myNewItemId = response[0].id[0];
        }
        [TestMethod]
        public void VerifyDeleteAssetType()
        {
            string payload = @"{
                            ""ID"":""1""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItems(DeleteAssetType, payload);
            int myNewItemId = response[0].id[0];
        }

        // Asset Category

        [TestMethod]
        public void VerifyListofAssetCategory()
        {
            int ptwId = 45;
            int FacilityId = 20;
            int userId = 33;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitList>();
            var response = ptwService.GetItemList(GetAssetCategoryList + "?ID=" + FacilityId);
            int ptwListCount = response.Count;
            Assert.AreEqual(ptwListCount, 200);
            //Assert.AreEqual(ptwId, response[0].permitId);
            //Assert.AreEqual("Consulmable", response[0].description);
        }
        [TestMethod]
        public void CreateAssetCategory()
        {
            string payload = @"{
                            ""cat_name"":""Testing AssetType""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(AddAssetCategory, payload);
            int myNewItemId = response.id[0];
        }
        [TestMethod]
        public void VerifyUpdateAssetCategory()
        {
            string payload = @"{
                            ""cat_name"":""Testing Asset Category"",
                            ""ID"":""1""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItems(UpdateAssetCategory, payload);
            int myNewItemId = response[0].id[0];
        }
        [TestMethod]
        public void VerifyDeleteAssetCategory()
        {
            string payload = @"{
                            ""acID"":""1""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItems(DeleteAssetCategory, payload);
            int myNewItemId = response[0].id[0];
        }


        //Unit measurement

        [TestMethod]
        public void VerifyUnitMeasurementList()
        {
            int ptwId = 45;
            int FacilityId = 20;
            int userId = 33;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitList>();
            var response = ptwService.GetItemList(GetUnitMeasurementList + "?ID=" + FacilityId);
            int ptwListCount = response.Count;
            Assert.AreEqual(ptwListCount, 200);
            //Assert.AreEqual(ptwId, response[0].permitId);
            //Assert.AreEqual("Consulmable", response[0].description);
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
            var response = ptwService.CreateItem(AddUnitMeasurement, payload);
            int myNewItemId = response.id[0];
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
            var response = ptwService.CreateItem(UpdateUnitMeasurement, payload);
            int myNewItemId = response.id[0];
        }
        [TestMethod]
        public void VerifyDeleteUnitMeasurement()
        {
            string payload = @"{
                            ""umID"":""1""
                           
                        }";
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItem(DeleteUnitMeasurement, payload);
            int myNewItemId = response.id[0];
        }

    }
}
