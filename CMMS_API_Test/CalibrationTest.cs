using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Helper;
using Newtonsoft.Json;

namespace CMMS_API_Test
{
    [TestClass]
    public class CalibrationTest
    {
        [TestMethod]
        public void CalibrationFunctionalTest()
        {
            //Verification of calibration List
            int facilityId = 1736;
            int assetId = 123102;
            int vendorId = 3;

            var calibService1 = new CMMS_Services.APIService<CMCalibrationList>(true);
            var response1 = calibService1.GetItemList("/api/Calibration/GetCalibrationList?facility_id=" + facilityId);
            Assert.IsNotNull(response1);
            Assert.AreEqual(6, response1.Count);
            Dictionary<dynamic, CMCalibrationList> calibitems = response1.SetPrimaryKey("asset_id");

            //Request Calibration
            CMRequestCalibration requestCalib = new CMRequestCalibration()
            {
                asset_id = assetId,
                vendor_id = vendorId,
                next_calibration_date = (DateTime) calibitems[assetId].next_calibration_due_date
            };
            string payload1 = JsonConvert.SerializeObject(requestCalib);
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response2 = calibService2.PostService("/api/Calibration/RequestCalibration", payload1);
            Assert.IsNotNull(response2);
            int calib_id = response2[0].id[0];
            var calibService3 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response3 = calibService3.GetItem("/api/Calibration/GetCalibrationDetails?id="+calib_id);
            Assert.IsNotNull(response3);
            try
            {
                Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response2[0].return_status);
                Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_REQUEST, response3.statusID);
                Assert.AreEqual(assetId, response3.asset_id);
                Assert.AreEqual(vendorId, response3.vendor_id);
                Assert.AreEqual(requestCalib.next_calibration_date, response3.calibration_due_date);
            }
            catch (AssertFailedException)
            {
                Assert.AreEqual(assetId, response3.asset_id);
            }

            //Reject Calibration Request
            string payload2 = $"{{\n\t\"id\" : {calib_id}, \n\t\"comment\" : \"Rejected\" \n}}";
            var calibService4 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response4 = calibService4.PutService("/api/Calibration/RejectRequestCalibration", payload2);
            Assert.IsNotNull(response4);
            var calibService5 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response5 = calibService5.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response5);
            try
            {
                Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response4[0].return_status);
                Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED, response5.statusID);
            }
            catch (AssertFailedException)
            {

            }

            //Approve Calibration Request
            string payload3 = $"{{\n\t\"id\" : {calib_id}, \n\t\"comment\" : \"Approved\" \n}}";
            var calibService6 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response6 = calibService6.PutService("/api/Calibration/ApproveRequestCalibration", payload3);
            Assert.IsNotNull(response6);
            var calibService7 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response7 = calibService7.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response7);
            try
            {
                Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response6[0].return_status);
                Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, response7.statusID);
            }
            catch (AssertFailedException)
            {

            }

            //Start Calibration
            var calibService8 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response8 = calibService8.PutService("/api/Calibration/StartCalibration?calibration_id=" + calib_id, "");
            Assert.IsNotNull(response8);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response8[0].return_status);
            var calibService9 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response9 = calibService9.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response9);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_STARTED, response9.statusID);

            //Complete Calibration
            string payload4 = $"{{\n\t\"calibration_id\" : {calib_id}, \n\t\"comment\" : \"Completed\" \n}}";
            var calibService10 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response10 = calibService10.PutService("/api/Calibration/CompleteCalibration", payload4);
            Assert.IsNotNull(response10);
            Assert.AreEqual(CMMS.RETRUNSTATUS.SUCCESS, response10[0].return_status);
            var calibService11 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response11 = calibService11.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response11);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_COMPLETED, response11.statusID);

            //Close Calibration
            string payload5 = $"{{\n\t\"calibration_id\" : {calib_id}, \n\t\"comment\" : \"Closed\", \n\t\"is_damaged\" : 1 \n}}";
            var calibService12 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response12 = calibService12.PutService("/api/Calibration/CloseCalibration", payload5);
            Assert.IsNotNull(response12);
            var calibService13 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response13 = calibService13.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response13);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_CLOSED, response13.statusID);

            //Reject Calibration
            string payload6 = $"{{\n\t\"id\" : {calib_id}, \n\t\"comment\" : \"Not OK\" \n}}";
            var calibService14 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response14 = calibService14.PutService("/api/Calibration/RejectCalibration", payload6);
            Assert.IsNotNull(response14);
            var calibService15 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response15 = calibService15.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response15);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_REJECTED, response15.statusID);

            //Close Calibration
            List<string> fileList = new List<string>()
            {
                @"C:\Users\lenovo\OneDrive\Pictures\Saved Pictures\download.jpeg",
                @"C:\Users\lenovo\OneDrive\Pictures\unnamed.jpg",
                @"C:\Users\lenovo\OneDrive\Pictures\Saved Pictures\Harmony Dark.jpeg"
            };
            Dictionary<string, Tuple<bool, List<string>>> fileUpload = new Dictionary<string, Tuple<bool, List<string>>>()
            {
                { "files", new Tuple<bool, List<string>>(true, fileList) },
                { "facility_id", new Tuple<bool, List<string>>(false, new List<string> { "1736" }) },
                { "module_type", new Tuple<bool, List<string>>(false, new List<string> { $"{CMMS.CMMS_Modules.CALIBRATION}" }) },
                { "module_ref_id", new Tuple<bool, List<string>>(false, new List<string> { $"{calib_id}" }) },
                { "file_category", new Tuple<bool, List<string>>(false, new List<string> { "12" }) },
            };
            var fileService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var fileResponse = fileService.FormPostService($"/api/FileUpload/UploadFile", fileUpload);
            Assert.IsNotNull(fileResponse);
            Assert.AreEqual(fileList.Count, fileResponse[0].id.Count);
            string payload7 = $"{{\n\t\"calibration_id\" : {calib_id}, \n\t\"comment\" : \"Closed\", \n\t\"is_damaged\" : 0 \n}}";
            var calibService16 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response16 = calibService16.PutService("/api/Calibration/CloseCalibration", payload7);
            Assert.IsNotNull(response16);
            var calibService17 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response17 = calibService17.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response17);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_CLOSED, response17.statusID);

            //Approve Calibration
            string payload8 = $"{{\n\t\"id\" : {calib_id}, \n\t\"comment\" : \"OK\" \n}}";
            var calibService18 = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMRescheduleApprovalResponse>(true);
            var response18 = calibService18.PutService("/api/Calibration/ApproveCalibration", payload8);
            Assert.IsNotNull(response18);
            var calibService19 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response19 = calibService19.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calib_id);
            Assert.IsNotNull(response19);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_APPROVED, response19.statusID);
            int new_calib_id = response18[0].new_id[0];
            var calibService20 = new CMMS_Services.APIService<CMCalibrationDetails>(true);
            var response20 = calibService20.GetItem("/api/Calibration/GetCalibrationDetails?id=" + new_calib_id);
            Assert.IsNotNull(response20);
            Assert.AreEqual(requestCalib.asset_id, response20.asset_id);
            Assert.AreEqual(requestCalib.vendor_id, response20.vendor_id);
            Assert.AreNotEqual(requestCalib.next_calibration_date, response20.calibration_due_date);
            Assert.AreEqual((int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, response20.statusID);
            var calibService21 = new CMMS_Services.APIService<CMPreviousCalibration>(true);
            var response21 = calibService21.GetItem("/api/Calibration/GetPreviousCalibration?asset_id=" + requestCalib.asset_id);
            Assert.IsNotNull(response20);
            Assert.AreEqual(requestCalib.asset_id, response21.asset_id);
            Assert.AreEqual(requestCalib.vendor_id, response21.vendor_id);
            Assert.AreEqual(requestCalib.next_calibration_date, response21.previous_calibration_date);
        }

        [TestMethod]
        public void VerifyCalibrationList()
        {
            int facilityId = 1736;
            int calibId1 = 123099;
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationList>(true);
            var response = calibService.GetItemList("/api/Calibration/GetCalibrationList?facility_id=" + facilityId);
            Assert.IsNotNull(response);
            Assert.AreEqual(6, response.Count);
            Assert.AreEqual(calibId1, response[0].asset_id);
        }

        [TestMethod]
        public void VerifyCalibrationDetails()
        {
            int calibID = 7299;
            int asset_id = 123100;
            string asset_name = "RISEN_WIND_BLOCK_2_WMS";
            string vendor_name = "APSEB";
            DateTime next_calibration_date = new DateTime(2023,08,03);
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response = calibService.GetItem("/api/Calibration/GetCalibrationDetails?id="+calibID);
            Assert.AreEqual(asset_id, response.asset_id);
            Assert.AreEqual(asset_name, response.asset_name);
            Assert.AreEqual(vendor_name, response.vendor_name);
            Assert.AreEqual(next_calibration_date, response.calibration_due_date);
        }

        [TestMethod]
        public void VerifyPreviousCalibration()
        {
            int asset_id = 123104;
            string asset_name = "RISEN_WIND_BLOCK_6_WMS";
            int vendor_id = 3;
            string vendor_name = "APSEB";
            DateTime prev_calibration_date = new DateTime(2023, 05, 31);
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMPreviousCalibration>(true);
            var response = calibService.GetItem("/api/Calibration/GetPreviousCalibration?asset_id=" + asset_id);
            Assert.AreEqual(asset_id, response.asset_id);
            Assert.AreEqual(asset_name, response.asset_name);
            Assert.AreEqual(vendor_id, response.vendor_id);
            Assert.AreEqual(vendor_name, response.vendor_name);
            Assert.AreEqual(prev_calibration_date, response.previous_calibration_date);
        }

        [TestMethod]
        public void RequestCalibration()
        {
            string payload = "{\n\t\"asset_id\" : 123099, \n\t\"vendor_id\" : 3, \n\t\"next_calibration_date\" : \"2023-07-26\" \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PostService("/api/Calibration/RequestCalibration", payload);
            int calibID = response[0].id[0];
            int asset_id = 123099;
            string asset_name = "RISEN_WIND_BLOCK_1_WMS";
            string vendor_name = "APSEB";
            DateTime next_calibration_date = new DateTime(2023, 07, 26);
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual(asset_id, response2.asset_id);
            Assert.AreEqual(asset_name, response2.asset_name);
            Assert.AreEqual(vendor_name, response2.vendor_name);
            Assert.AreEqual(next_calibration_date, response2.calibration_due_date);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_REQUEST, response2.statusID);
        }

        [TestMethod]
        public void RejectCalibrationRequest()
        {
            string payload = "{\n\t\"id\" : 7299, \n\t\"comment\" : \"Rejected\" \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/RejectRequestCalibration", payload);
            int calibID = response[0].id[0];
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED, response2.statusID);
        }

        [TestMethod]
        public void ApproveCalibrationRequest()
        {
            string payload = "{\n\t\"id\" : 7299, \n\t\"comment\" : \"Approved\" \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/ApproveRequestCalibration", payload);
            int calibID = response[0].id[0];
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, response2.statusID);
        }

        [TestMethod]
        public void StartCalibration()
        {
            int calibID = 7299;
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/StartCalibration?calibration_id="+calibID, "");
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_STARTED, response2.statusID);
        }

        [TestMethod]
        public void CompleteCalibration()
        {
            string payload = "{\n\t\"calibration_id\" : 7299, \n\t\"comment\" : \"Completed\" \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/CompleteCalibration", payload);
            int calibID = response[0].id[0];
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_COMPLETED, response2.statusID);
        }

        [TestMethod]
        public void CloseCalibration()
        {
            string payload = "{\n\t\"calibration_id\" : 7299, \n\t\"comment\" : \"Closed\", \n\t\"is_damaged\" : 0 \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/CloseCalibration", payload);
            int calibID = response[0].id[0];
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_CLOSED, response2.statusID);
        }

        [TestMethod]
        public void ApproveCalibration()
        {
            string payload = "{\n\t\"id\" : 7299, \n\t\"comment\" : \"OK\" \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/ApproveCalibration", payload);
            int calibID = response[0].id[0];
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_APPROVED, response2.statusID);
        }

        [TestMethod]
        public void RejectCalibration()
        {
            string payload = "{\n\t\"id\" : 7299, \n\t\"comment\" : \"Not OK\" \n}";
            var calibService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>(true);
            var response = calibService.PutService("/api/Calibration/RejectCalibration", payload);
            int calibID = response[0].id[0];
            var calibService2 = new CMMS_Services.APIService<CMMSAPIs.Models.Calibration.CMCalibrationDetails>(true);
            var response2 = calibService2.GetItem("/api/Calibration/GetCalibrationDetails?id=" + calibID);
            Assert.AreEqual((int)CMMSAPIs.Helper.CMMS.CMMS_Status.CALIBRATION_REJECTED, response2.statusID);
        }
    }
}
