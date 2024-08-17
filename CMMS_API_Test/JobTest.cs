using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using CMMS_Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using CMMSAPIs.Helper;

namespace CMMS_API_Test
{
    [TestClass]
    public class JobTest
    {
        const string EP_GetJobList = "/api/Job/GetJobList?facility_id";
        const string EP_GetJobDetails = "/api/Job/GetJobDetails?job_id=";
        [TestMethod]
        public void VerifyListOfJobs()
        {
            int JobId = 45;
            int FacilityId = 45;
            int userId = 23;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobModel>(true);
            var response = jobService.GetItemList(EP_GetJobList + FacilityId);
            Assert.IsNotNull(response);
            if (response != null)
            {
                int JobListCount = response.Count;
                Assert.AreEqual(JobListCount, 44);
                Assert.AreEqual("Job title here", response[0].jobDetails);
            }
        }
        [TestMethod]
        public void VerifyListOfInventory()
        {
            int JobId = 45;
            int FacilityId = 20;
            string categoryIds = "2,3";
            int expJobCount = 200;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Inventory.CMInventoryList>(true);
            //jobService.SetToken(token);
            var itemList = jobService.GetItemList("/api/Inventory/GetInventoryList?facilityId=45&categoryIds=" + categoryIds);
            int AssetListCount = itemList.Count;
            Assert.AreEqual(expJobCount, AssetListCount);
            //            Assert.AreEqual("Job title here", response[0].job_title);
        }

        [TestMethod]
        public void VerifyJobDetail()
        {
            int JobId = 3298;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobView>(true);
            var item = jobService.GetItem(EP_GetJobDetails + JobId);
            Assert.AreEqual("Inverter Breakdown", item.job_title);
            Assert.AreEqual("Inverter is not working. Please repair it", item.job_description);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.JOB_ASSIGNED, item.status);
        }
        [TestMethod]
        public void CreateNewJob()
        {
            string payload = @"{
                            ""title"":""Inverter Failure"",
                            ""description"":""Inverter is not working. Please check and fix it"",
                            ""facility_id"": 45,
                            ""block_id"": 72,
                            ""assigned_id"": 16,
                            ""breakdown_time"":""2022-04-21T10:00:00Z"",
                            ""WorkType_Ids"":[24],
                            ""AssetsIds"":[14427,14428]
                        }";


            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jobService.CreateItem("/api/Job/CreateNewJob", payload);
            int myNewItemId = response.id[0];

            //pending : now get same item
            var jobService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobView>(true);
            var response2 = jobService2.GetItem("/api/Job/GetJobDetails?job_id=" + myNewItemId);
            Assert.AreEqual("Inverter Failure", response2.job_title);
            Assert.AreEqual(myNewItemId, response2.id);
        }

        [TestMethod]
        public void VerifyReAssignJob()
        {
            int JobId = 3381;
            int assignedTo_Id = 66;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jobService.PutService($"/api/Job/ReAssignJob?job_id={JobId}&assignedTo={assignedTo_Id}", "");  //pending : pass jobid and assigned to  and userid.. see signature in jobcontrollee

            var jobService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobView>(true);
            var response2 = jobService2.GetItem("/api/Job/GetJobDetails?job_id=" + JobId);
            Assert.AreEqual(assignedTo_Id, response2.assigned_id);
        }

        [TestMethod]
        public void VerifyOfCancelJob()
        {
            int JobId = 45;
            int UserId = 23;
            string cancelremark = "";
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobView>(true);
            var response = jobService.GetItem("/api/Job/CancelJob");       //pending : pass jobid, userid, cancel remart. See the sinature of this functio in jobcontroller
            Assert.AreEqual("Job title here", response.job_title);
        }

        [TestMethod]
        public void VerifyLinkToPTW()
        {
            int JobId = 3381;
            int ptw_id = 59960;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = jobService.PutService($"/api/Job/LinkToPTW?job_id={JobId}&ptw_id={ptw_id}",""); //pending : change this to linkToPtw api and pass jobid and ptwid\\

            //Pending : Then call GetJobDetails by passng same job id and veridy its ptwid attruibute is changed to what you set abbove
            var jobService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobView>(true);
            var response2 = jobService2.GetItem("/api/Job/GetJobDetails?job_id=" + JobId);
            Assert.AreEqual(ptw_id, response2.current_ptw_id);
        }

        [TestMethod]
        public void VerifyListOfWorkType()
        {
            int JobId = 45;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkType>(true);
            var response = jobService.GetItemList("/api/Job/GetWorkTypeList?job_id=" + JobId);
            int WorkTypeTypeListCount = response.Count;
            Assert.AreEqual(WorkTypeTypeListCount, 7);
            Assert.AreEqual("List of Job type", response[0].categoryid);
            Assert.AreEqual("List of Job type", response[0].workType);
            Assert.AreEqual("List of Job type", response[0].categoryName);
        }

        [TestMethod]
        public void VerifyListOfAddWorkType()
        {
            int JobId = 45;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkType>(true);
            var response = jobService.GetItemList("/api/Job/GetAddWorkType?job_id=" + JobId);
            int AddWorkJobCount = response.Count;
            Assert.AreEqual(AddWorkJobCount, 24);
            Assert.AreEqual("WorkType name", response[0].workType);
        }

        [TestMethod]
        public void VerifyUpdateWorkType()
        {
            int JobId = 45;
            var jobService = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkType>(true);
            var response = jobService.GetItemList("/api/Job/UpdateWorkType?job_id=" + JobId);
            int UpdateWorkTypeCount = response.Count;
            Assert.AreEqual(UpdateWorkTypeCount, 215);
            Assert.AreEqual("WorkType name", response[0].workType);

        }

        [TestMethod]
        public void VerifyDeleteWorkType()
        {
            int JobId = 55;
            var facilitySevice = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkType>(true);
            var response = facilitySevice.GetItemList("/api/Job/GetDeleteWorkType?job_id=" + JobId);
            int DaleteWorkTypeCount = response.Count;
            Assert.AreEqual(DaleteWorkTypeCount, 200);
            Assert.AreEqual("Work Type Tool name", response[0].workType);
        }

        [TestMethod]
        public void VerifyListOfJobWorkTypeTool()
        {
            int JobId = 55;
            var facilitySevice = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkTypeTool>(true);
            var response = facilitySevice.GetItemList("/api/Job/GetDeleteWorkType?job_id=" + JobId);    //Pending : correct the api name
            int JobWorkTypeToolListCount = response.Count;
            Assert.AreEqual(JobWorkTypeToolListCount, 200);
            Assert.AreEqual("Work Type Tool name", response[0].workTypeName);
        }

        [TestMethod]
        public void VerifyDeleteJobWorkTypeTool()
        {
            int JobId = 55;
            var facilitySevice = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkTypeTool>(true);
            var response = facilitySevice.GetItemList("/api/Job/GetUpdateWorkTypeTool?job_id=" + JobId);    //Pending : inCorrect api name
            int DeleteJobWorkTypeToolCount = response.Count;
            Assert.AreEqual(DeleteJobWorkTypeToolCount, 200);
            Assert.AreEqual("Work Type Tool name", response[0].workTypeName);
        }
        //pending mastertoollist not done?

        [TestMethod]
        public void VerifyListOfMaster77Tool()
        {
            int JobId = 55;
            var facilitySevice = new CMMS_Services.APIService<CMMSAPIs.Models.Jobs.CMJobWorkTypeTool>(true);
            var response = facilitySevice.GetItemList("/api/Job/GetMasterToolList?job_id=" + JobId);    //Pending : inCorrect api name
            int MasterToolListCount = response.Count;
            Assert.AreEqual(MasterToolListCount, 200);
            Assert.AreEqual("Master Tool List", response[0].workTypeName);
        }
    }
}