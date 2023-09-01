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
        public void PTWFunctionalTest()
        {
            //GetListofPTW
            //remember the count
            //add ptw
            //remmber the id of newly added PTW
            //Use this PTWID and call GetPTWDetails and verify with the attributes passed to create PTW
            //Call GetPTWLIst
            //Make sure your list increased by 1 from the count you got at begining
            //then You put request to issue permit
            //then get PTWDetails
            //Verify the status and other relevant properties 
            //Then you reject PTW issue
            //Verify the status and other relevant properties 
            //Then you issue PTW again
            //Verify the status and other relevant properties 
            //Then put request to approve
            //then get PTWDetails
            //Verify the status and other relevant properties 
            //Then you reject PTW approve
            //Verify the status and other relevant properties 
            //Then you approve PTW again
            //Verify the status and other relevant properties 
            //same with Extend
            //same with Cancel
            //same with Close 

        }
        [TestMethod]
        public void VerifyListOfPTWs()
        {
            int ptwId = 59886;
            int FacilityId = 45;
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitList>(true);
            var response = ptwService.GetItemList("/api/Permit/GetPermitList?facility_id=" + FacilityId);
            int ptwListCount = response.Count;
            //Assert.AreEqual(ptwListCount, 48);
            Assert.AreEqual(ptwId, response[ptwListCount-1].permitId);
            Assert.AreEqual("INVERTER FAILURE", response[ptwListCount-1].description);
        }

        [TestMethod]
        public void VerifyPTWDetail()
        {

            int ptwId = 59909;   //check the valid permit id from database table and ue here
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response = ptwService.GetItem("/api/Permit/GetPermitDetails?permit_id=" + ptwId);
            Assert.AreEqual(126, response.ptwStatus);
//            Assert.AreEqual(ptwId, response.permitId);
            Assert.AreEqual("new permit 06&#47;05&#47;2023", response.description);
        }
        [TestMethod]
        public void CreateNewPTW()
        {
            string payload = @"{
                                ""facility_id"" : 45,
                                ""blockId"": 76,
                                ""lotoId"": 3,
                                ""start_datetime"":""2023-12-26T08:00:00Z"",
                                ""end_datetime"":""2023-12-26T16:00:00Z"",
                                ""title"":""test permit 1"",
                                ""description"":""test permit 1"",
                                ""job_type_id"":3,
                                ""typeId"":7,
                                ""sop_type_id"":5,  
                                ""issuer_id"":35,
                                ""approver_id"":7,
                                ""category_ids"":[13219,13220],
                                ""is_isolation_required"":true,
                                ""isolated_category_ids"":[2,3],
                                ""Loto_list"":[
                                        {
                                            ""Loto_id"":12687,
                                            ""Loto_Key"":""lototest1""
                                        },
                                        {
                                            ""Loto_id"":13220,
                                            ""Loto_Key"":""lototest2""
                                        }
                                    ],
                                ""employee_list"":[
                                        {
                                            ""employeeId"":3,
                                            ""responsibility"":""check""
                                        },
                                        {
                                            ""employeeId"":4,
                                            ""responsibility"":""testing""
                                        }       
                                    ],
                                ""safety_question_list"":[
                                        {
                                            ""safetyMeasureId"":48,
                                            ""safetyMeasureValue"":""Yes""
                                        },
                                        {
                                            ""safetyMeasureId"":139,
                                            ""safetyMeasureValue"":""on""
                                        }
                                    ]
                                }";
            //Change this payload to that of PTW


            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.CreateItem("/api/Permit/CreatePermit", payload);
            int myNewItemId = response.id[0];

            //pending : now get same item
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + myNewItemId);
            Assert.AreEqual("test permit 1", response2.description);
            Assert.AreEqual(myNewItemId, response2.permitNo);
        }
        
        [TestMethod]
        public void VerifyPermitApprove()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Approved" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitApprove", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_APPROVED);
        }
        [TestMethod]
        public void VerifyPermitExtend()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Please Extend" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitExtend", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_EXTEND_REQUESTED);
        }
        [TestMethod]
        public void VerifyPermitExtendApprove()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Extend Approved" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitExtendApprove", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE);
        }
        [TestMethod]
        public void VerifyPermitExtendCancel()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Extend Cancelled" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitExtendCancel", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED);
        }
        [TestMethod]
        public void VerifyPermitClose()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Permit Closed" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitClose", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_CLOSED);
        }
        [TestMethod]
        public void VerifyPermitReject()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Permit Closed" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitReject", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER);
        }
        [TestMethod]
        public void VerifyPermitIssue()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Permit Issued" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitIssue", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_ISSUED);
        }
        [TestMethod]
        public void VerifyPermitIssueReject()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Permit Issue Rejected" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitIssueReject", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER);
        }
        [TestMethod]
        public void VerifyPermitCancelRequest()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59915" },
                { "comment", "Cancel Requested" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitCancelRequest", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_CANCEL_REQUESTED);
        }
        [TestMethod]
        public void VerifyPermitCancelReject()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Cancel Rejected" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitCancelReject", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED);
        }
        [TestMethod]
        public void VerifyPermitCancelByApprover()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Cancel Approved" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitCancelByApprover", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER);
        }
        [TestMethod]
        public void VerifyPermitCancelByHSE()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Cancel Approved" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitCancelByHSE", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE);
        }
        [TestMethod]
        public void VerifyPermitCancelByIssuer()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "59909" },
                { "comment", "Cancel Approved" }
            };
            var ptwService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = ptwService.FormPutService("/api/Permit/PermitCancelByIssuer", approval);
            var ptwService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Permits.CMPermitDetail>(true);
            var response2 = ptwService2.GetItem("/api/Permit/GetPermitDetails?permit_id=" + approval["id"]);
            Assert.AreEqual(response2.ptwStatus, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER);
        }
        /**/
    }
}
