using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using CMMS_Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;

namespace CMMS_API_Test
{
    [TestClass]
    public class FacilityTest
    {
        [TestMethod]
        public void VerifyListOfFacilities()
        {
            //int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Facility.CMFacilityList>();
            var response = facilityService.GetItemList("/api/Facility/GetFacilityList");
            int facilityCount = response.Count;
            Assert.AreEqual(facilityCount, 3);
            Assert.AreEqual(response[0].id, 45);
            Assert.AreEqual("Hero Future Solar Plant 1000MW", response[0].name);
        }

        [TestMethod]
        public void VerifyFacility()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Facility.CMFacilityDetails>();
            var response = facilityService.GetItem("/api/Facility/GetFacilityDetails?id=" + facilityId);
            Assert.AreEqual(facilityId, response.id);
            Assert.AreEqual("Hero Future Solar Plant 1000MW", response.blockName);


        }
        [TestMethod]
        public void CreateNewFacility()
        {
            string payload = @"{
                                ""name"": ""Goregaon Plant"",
                                ""AC_Capacity"": ""100000""
                               }";

            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMFacility>();
            var response = facilityService.CreateItems("/api/CMMS/CreateFacility", payload);
            Assert.AreEqual("Goregaon Plant", response[0].name);
            Assert.AreEqual("100000", response[0].pin);
        }

        [TestMethod]
        public void VerifyListOfBlocks()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMFacility>();
            var response = facilityService.GetItemList("/api/CMMS/GetBlockList?facility_id=" + facilityId);
            int blockCount = response.Count;
            Assert.AreEqual(5, blockCount);
            Assert.AreEqual("HFE_Block_1", response[0].name);
        }

        [TestMethod]
        public void VerifyListOfAssetCategory()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMAssetCategory>();
            var response = facilityService.GetItemList("/api/CMMS/GetAssetCategoryList?facility_id=" + facilityId);
            int AssetCategoryCount = response.Count;
            Assert.AreEqual(170, AssetCategoryCount);
            Assert.AreEqual("Inverter", response[0].name);
        }

        [TestMethod]
        public void VerifyListOfAsset()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMAsset>();
            var response = facilityService.GetItemList("/api/CMMS/GetAssetsList?facility_id=" + facilityId);
            int AssetCount = response.Count;
            Assert.AreEqual(3513, AssetCount);
            Assert.AreEqual("HFE_Block_1_ACDB_1", response[0].name);
        }

        [TestMethod]
        public void VerifyListOfSupplier()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMFacility>();
            var response = facilityService.GetItemList("/api/CMMS/GetSupplierList?facility_id=" + facilityId);
            int SupplierCount = response.Count;
            Assert.AreEqual(SupplierCount, 213);
            Assert.AreEqual("Supplier", response[0].name);
        }

        [TestMethod]
        public void VerifyListOfEmployee()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMFacility>();
            var response = facilityService.GetItemList("/api/CMMS/GetEmployeeList?facility_id=" + facilityId);
            int EmployeeCount = response.Count;
            Assert.AreEqual(EmployeeCount, 24);
            Assert.AreEqual("Aditya Gupta", response[0].name);
        }

        [TestMethod]
        public void VerifyListOfFrequency()
        {
            int facilityId = 45;
            var facilityService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMFacility>();
            var response = facilityService.GetItemList("/api/CMMS/GetFrequency?facility_id=" + facilityId);
            int frequencyCount = response.Count;
            Assert.AreEqual(frequencyCount, 215);
            Assert.AreEqual("Frequency List", response[0].name);

        }
    }
}