using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class CalibrationTest
    {
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
            string payload = "{\n\t\"calibration_id\" : 7299, \n\t\"comment\" : \"Completed\", \n\t\"is_damaged\" : 0 \n}";
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
            string payload = "{\n\t\"calibration_id\" : 7299, \n\t\"comment\" : \"Closed\" \n}";
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
