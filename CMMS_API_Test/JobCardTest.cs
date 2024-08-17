using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class JobCardTest
    {
        [TestMethod]
        public void CreateNewJC()
        {
            int job_id = 3381;
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jcService.CreateItem($"/api/JC/CreateJC?job_id={job_id}","");
            int newJCId = response.id[0];

            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobView>(true);
            var response2 = jobService.GetItem("/api/Job/GetJobDetails?job_id=" + job_id);

            var jcService2 = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response3 = jcService2.GetItemList("/api/JC/GetJCDetail?jc_id=" + newJCId);

            Assert.AreEqual(newJCId, response3[0].id);
            Assert.AreEqual(job_id, response3[0].jobid);
            Assert.AreEqual(response2.current_ptw_id, response3[0].ptwId);
            Assert.AreEqual(response2.job_title, response3[0].title);
            Assert.AreEqual(response2.job_description, response3[0].description);
        }

        [TestMethod]
        public void VerifyJCDetail()
        {
            int jc_id = 2616;
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response = jcService.GetItemList("/api/JC/GetJCDetail?jc_id=" + jc_id);

            Assert.AreEqual(response[0].jobid, 3381);
            Assert.AreEqual(response[0].ptwId, 59960);
            Assert.AreEqual(response[0].title, "Inverter Failure");
        }

        
        [TestMethod]
        public void CloseJC()
        {
            string close =  @"{
                                ""id"":2616,
                                ""isolationId"":1405,
                                ""lotoId"":1415,
                                ""comment"":""job card close"",
                                ""employee_id"":16,
                                ""normalisedStatus"":1,
                                ""lotoStatus"":1
                               }";
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jcService.PutService("/api/JC/CloseJC", close);
            int jc_id = response[0].id[0];
            var jcService2 = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response2 = jcService2.GetItemList("/api/JC/GetJCDetail?jc_id=" + jc_id);
            Assert.AreEqual(response2[0].status, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.JC_CLOSED);
        }

        [TestMethod]
        public void ApproveJC()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "2616" },
                { "comment", "Approved" }
            };
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jcService.FormPutService("/api/JC/ApproveJC", approval);
            var jcService2 = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response2 = jcService2.GetItemList("/api/JC/GetJCDetail?jc_id=" + approval["id"]);
            Assert.AreEqual(response2[0].JC_Approved, (int)CMMSAPIs.Helper.CMMS.ApprovalStatus.APPROVED);
        }

        [TestMethod]
        public void RejectJC()
        {
            Dictionary<string, string> approval = new Dictionary<string, string>()
            {
                { "id", "2616" },
                { "commnet", "reject" },
                { "employee_id", "0" }
            };
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jcService.FormPutService("/api/JC/RejectJC", approval);
            var jcService2 = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response2 = jcService2.GetItemList("/api/JC/GetJCDetail?jc_id=" + approval["id"]);
            Assert.AreEqual(response2[0].JC_Approved, (int)CMMSAPIs.Helper.CMMS.ApprovalStatus.REJECTED);
        }
        [TestMethod]
        public void StartJC()
        {
            int jc_id = 2616;
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jcService.PutService("/api/JC/StartJC?jc_id="+jc_id, "");
            var jcService2 = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response2 = jcService2.GetItemList("/api/JC/GetJCDetail?jc_id=" + jc_id);
            Assert.AreEqual(response2[0].status, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.JC_STARTED);
        }
        [TestMethod]
        public void CarryForwardJC()
        {
            string cf =     @"{
                                ""id"":2616,
                                ""comment"":""Carry forward""
                              }";
            var jcService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jcService.PutService("/api/JC/CarryForwardJC", cf);
            int jc_id = response[0].id[0];
            var jcService2 = new CMMS_Services.APIService<CMMSAPIs.Models.JC.CMJCDetail>(true);
            var response2 = jcService2.GetItemList("/api/JC/GetJCDetail?jc_id=" + jc_id);
            Assert.AreEqual(response2[0].status, (int)CMMSAPIs.Helper.CMMS.CMMS_Status.JC_CARRY_FORWARDED);
        }
        /**/
    }
}
