using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
   public class PTWTest
    {
        [TestMethod]
        public void VerifyListOfPTWs()
        {
            int ptwId = 45;
            int FacilityId = 20;
            int userId = 33;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitList>();
            var response = ptwService.GetItemList("/api/Permit/GetPermitList?facility_id" + FacilityId + "&userID=" + userId);
            int ptwListCount = response.Count;
            Assert.AreEqual(ptwListCount, 200);
            Assert.AreEqual(ptwId, response[0].permitId);
            Assert.AreEqual("PT title here", response[0].description);
        }

        [TestMethod]
        public void VerifyPTWDetail()
        {

            int ptwId = 3134;   //check the valid permit id from database table and ue here
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>();
            var response = ptwService.GetItem("/api/Permit/GetPermitDetails?permit_id=" + ptwId);
            Assert.AreEqual(1, response.ptwStatus);
//            Assert.AreEqual(ptwId, response.permitId);
            Assert.AreEqual("Update desc", response.description);
        }
        [TestMethod]
        public void CreateNewPTW()
        {
            string payload = @"{
                            ""title"":""Inverter Failure"",
                            ""description"":""Inverter is not working. Please check and fix it"",
                            ""facilityId"":""45"",
                            ""blockId"":""30"",
                            ""assignedTo"":""1"",
                            ""breakdownTime"":""2022-04-21 10:00"",
                            ""userId"":""36"",
                            ""workTypes"":[24],

                            ""equipments"":[{
                                        ""assetId"":14427,
                                        ""categoryId"":2
                                           }, 
                                           {
                                        ""assetId"":14428,
                                        ""categoryId"":2
                                        }],
                            ""otherWorkTypeName"":""""
                        }";
            //Change this payload to that of PTW


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var response = ptwService.CreateItems("/api/Permit/CreatePermit", payload);
            int myNewItemId = response[0].id[0];

            //pending : now get same item
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitList>();
            var response2 = ptwService2.GetItem("/api/Permit/GetptwDetail?ptw_id=" + myNewItemId);
            Assert.AreEqual("Update desc", response2.description);
            Assert.AreEqual(myNewItemId, response2.permitId);
        }

    }
}
