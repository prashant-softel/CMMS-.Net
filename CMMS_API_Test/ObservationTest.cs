using CMMSAPIs.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_API_Test
{
    [TestClass]
    public class ObservationTest
    {
        string Ep_GetObservationList = "/api/MISMaster/GetObservationList";
        string Ep_GetObservationDetails = "/api/MISMaster/GetObservationDetails";
        string Ep_CreateObservation = "/api/MISMaster/CreateObservation";
        string Ep_AssingtoObservation = "/api/MISMaster/AssingtoObservation";
        string Ep_UpdateObservation = "/api/MISMaster/UpdateObservation";
        string Ep_CloseObservation = "/api/MISMaster/CloseObservation";
        string Ep_ApproveObservation = "/api/MISMaster/ApproveObservation";
        string Ep_RejectObservation = "/api/MISMaster/RejectObservation";



        [TestMethod]
        public void VerifyGetObservationList()
        {
            int facilityId = 17;
            string fromDate = "2024-09-30";
            string toDate = "2024-10-07";

            var observationService = new CMMS_Services.APIService<List<CMMSAPIs.Models.Masters.CMObservation>>();
            var response = observationService.GetItem(Ep_GetObservationList + "?facility_id=" + facilityId + "&fromDate=" + fromDate + "&toDate=" + toDate);

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.IsTrue(response.Count > 0, "The observation list should not be empty.");
            Assert.AreEqual(facilityId, response[0].facility_id, "The facility ID should match.");
        }


        [TestMethod]
        public void VerifyGetObservationDetails()
        {
            int observation_id = 1764;
            int check_point_type_id = 2;

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var response = observationService.GetItem(Ep_GetObservationDetails + "?observation_id=" + observation_id + "&check_point_type_id=" + check_point_type_id );

            Assert.IsNotNull(response, "The API response should not be null.");
            Assert.AreEqual(observation_id, response.id, "The observation ID should match.");
            Assert.AreEqual(194, response.assigned_to_id, "The check point type ID should match.");
        }

        [TestMethod]
        public void VerifyCreateObservation()
        {
            var payload = new CMMSAPIs.Models.Masters.CMObservation
            {
                id = 0,
                facility_id = 17,
                risk_type_id = 1,
                date_of_observation = DateTime.Parse("2024-10-07"),
                type_of_observation = 1,
                location_of_observation = "cnhalli",
                source_of_observation = 1,
                observation_description = "test",
                uploadfileIds = new List<int>(),
                action_taken = "test"
                
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(Ep_CreateObservation, jsonPayload);

            int obs_id = Response.id[0];

            Assert.AreEqual(Response.message, "Observation data saved successfully.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_CREATED, getItemresponse.status_code);

            Assert.AreEqual(payload.facility_id, getItemresponse.facility_id, "facility_id should be match");
            Assert.AreEqual(payload.risk_type_id, getItemresponse.risk_type_id, "risk_type_id should be match");
            Assert.AreEqual(payload.date_of_observation, getItemresponse.date_of_observation, "date_of_observation should be match");
            Assert.AreEqual(payload.type_of_observation, getItemresponse.type_of_observation, "type_of_observation should be match");
            Assert.AreEqual(payload.location_of_observation, getItemresponse.location_of_observation, "location_of_observation should be match");
            Assert.AreEqual(payload.source_of_observation, getItemresponse.source_of_observation, "source_of_observation should be match");
            Assert.AreEqual(payload.observation_description, getItemresponse.observation_description, "observation_description should be match");
            Assert.AreEqual(payload.action_taken, getItemresponse.action_taken, "action_taken should be match");
        }



        [TestMethod]
        public void VerifyAssingtoObservation()
        {
            var payload = new CMMSAPIs.Models.Masters.AssignToObservation
            {
                id = 45,
                contractor_name = "Meera Corporation",
                risk_type_id = 1,
                preventive_action = "test",
                assigned_to_id = 194,
                contact_number = "8696907375",
                cost_type = 1,
                date_of_observation = DateTime.Parse("2024-10-07"),
                type_of_observation = 1,
                location_of_observation = "cnhalli",
                source_of_observation = 1,
                target_date = DateTime.Parse("2024-10-08"),
                observation_description = "test",
                action_taken = "For New Location"

            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(Ep_AssingtoObservation + "?check_point_type_id=1", jsonPayload);

            int obs_id = Response.id[0];

            Assert.AreEqual(Response.message, "Assigned Observation ");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_ASSIGNED, getItemresponse.status_code);

            Assert.AreEqual(payload.contractor_name, getItemresponse.operator_name, "contractor_name should be match");
            Assert.AreEqual(payload.risk_type_id, getItemresponse.risk_type_id, "risk_type_id should be match");
            Assert.AreEqual(payload.assigned_to_id, getItemresponse.assigned_to_id, "assigned_to_id should be match");
            Assert.AreEqual(payload.contact_number, getItemresponse.contact_number, "contact_number should be match");
            Assert.AreEqual(payload.cost_type, getItemresponse.cost_type, "cost_type should be match");
            Assert.AreEqual(payload.preventive_action, getItemresponse.preventive_action, "preventive_action should be match");
            Assert.AreEqual(payload.date_of_observation, getItemresponse.date_of_observation, "date_of_observation should be match");
            Assert.AreEqual(payload.type_of_observation, getItemresponse.type_of_observation, "type_of_observation should be match");
            Assert.AreEqual(payload.location_of_observation, getItemresponse.location_of_observation, "location_of_observation should be match");
            Assert.AreEqual(payload.source_of_observation, getItemresponse.source_of_observation, "source_of_observation should be match");
            Assert.AreEqual(payload.target_date, getItemresponse.target_date, "target_date should be match");
            Assert.AreEqual(payload.observation_description, getItemresponse.observation_description, "observation_description should be match");
            Assert.AreEqual(payload.action_taken, getItemresponse.action_taken, "action_taken should be match");
        }




        [TestMethod]
        public void VerifyUpdateObservation()
        {
            var payload = new CMMSAPIs.Models.Masters.CMObservation
            {
                id = 44,
                operator_name = "Meera Corporation",
                risk_type_id = 2,
                preventive_action = "test",
                assigned_to_id = 194,
                contact_number = "8696907375",
                cost_type = 1,
                date_of_observation = DateTime.Parse("2024-10-07"),
                type_of_observation = 1,
                location_of_observation = "cnhalli update",
                source_of_observation = 1,
                target_date = DateTime.Parse("2024-10-08"),
                observation_description = "test",
                uploadfileIds = new List<int>(),
                action_taken = "For New Location"

            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(Ep_UpdateObservation , jsonPayload);

            int obs_id = Response.id[0];

            Assert.AreEqual(Response.message, "Observation data updated successfully.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_CREATED, getItemresponse.status_code);

            Assert.AreEqual(payload.operator_name, getItemresponse.operator_name, "contractor_name should be match");
            Assert.AreEqual(payload.risk_type_id, getItemresponse.risk_type_id, "risk_type_id should be match");
            Assert.AreEqual(payload.assigned_to_id, getItemresponse.assigned_to_id, "assigned_to_id should be match");
            Assert.AreEqual(payload.contact_number, getItemresponse.contact_number, "contact_number should be match");
            Assert.AreEqual(payload.cost_type, getItemresponse.cost_type, "cost_type should be match");
            Assert.AreEqual(payload.preventive_action, getItemresponse.preventive_action, "preventive_action should be match");
            Assert.AreEqual(payload.date_of_observation, getItemresponse.date_of_observation, "date_of_observation should be match");
            Assert.AreEqual(payload.type_of_observation, getItemresponse.type_of_observation, "type_of_observation should be match");
            Assert.AreEqual(payload.location_of_observation, getItemresponse.location_of_observation, "location_of_observation should be match");
            Assert.AreEqual(payload.source_of_observation, getItemresponse.source_of_observation, "source_of_observation should be match");
            Assert.AreEqual(payload.target_date, getItemresponse.target_date, "target_date should be match");
            Assert.AreEqual(payload.observation_description, getItemresponse.observation_description, "observation_description should be match");
            Assert.AreEqual(payload.action_taken, getItemresponse.action_taken, "action_taken should be match");
        }



        [TestMethod]
        public void VerifyCloseObservation()
        {
            int check_point_type_id = 1;
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 44,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(Ep_CloseObservation + "?check_point_type_id=" + check_point_type_id, jsonPayload);

            int obs_id = Response.id[0];

            Assert.AreEqual(Response.message, $"Observation {obs_id} closed");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservationDetails>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_CLOSED, getItemresponse.status_code);

        }



        [TestMethod]
        public void VerifyApproveObservation()
        {
            int check_point_type_id = 1;
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 43,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.PutService(Ep_ApproveObservation + "?check_point_type_id=" + check_point_type_id, jsonPayload);

            int obs_id = Response[0].id[0];

            Assert.AreEqual(Response[0].message, "Observation Approved");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservationDetails>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_APPROVED, getItemresponse.status_code);
        }


        [TestMethod]
        public void VerifyRejectObservation()
        {
            int check_point_type_id = 1;
            var payload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 44,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.PutService(Ep_RejectObservation + "?check_point_type_id=" + check_point_type_id, jsonPayload);

            int obs_id = Response[0].id[0];

            Assert.AreEqual(Response[0].message, "Observation Rejected");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservationDetails>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=" + check_point_type_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_REJECTED, getItemresponse.status_code);
        }








        [TestMethod]
        public void VerifyFunctionTestObservation()
        {



            //STEP 1: Create Observation
            var Createpayload = new CMMSAPIs.Models.Masters.CMObservation
            {
                id = 0,
                facility_id = 17,
                risk_type_id = 1,
                date_of_observation = DateTime.Parse("2024-10-07"),
                type_of_observation = 1,
                location_of_observation = "cnhalli",
                source_of_observation = 1,
                observation_description = "test",
                uploadfileIds = new List<int>(),
                action_taken = "test"

            };

            string createjsonPayload = JsonConvert.SerializeObject(Createpayload);

            var observationService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var Response = observationService.CreateItem(Ep_CreateObservation, createjsonPayload);

            int obs_id = Response.id[0];

            Assert.AreEqual(Response.message, "Observation data saved successfully.");

            var getItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var getItemresponse = getItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_CREATED, getItemresponse.status_code);





            //STEP 2: Assign Observation
            var Assignpayload = new CMMSAPIs.Models.Masters.AssignToObservation
            {
                id = obs_id,
                contractor_name = "Meera Corporation",
                risk_type_id = 1,
                preventive_action = "test",
                assigned_to_id = 194,
                contact_number = "8696907375",
                cost_type = 1,
                date_of_observation = DateTime.Parse("2024-10-07"),
                type_of_observation = 1,
                location_of_observation = "cnhalli",
                source_of_observation = 1,
                target_date = DateTime.Parse("2024-10-08"),
                observation_description = "test",
                action_taken = "For New Location"

            };

            string AssignjsonPayload = JsonConvert.SerializeObject(Assignpayload);

            var AssignService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var AssignResponse = AssignService.CreateItem(Ep_AssingtoObservation + "?check_point_type_id=1", AssignjsonPayload);

            Assert.AreEqual(AssignResponse.message, "Assigned Observation ");

            var AssigngetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var AssigngetItemresponse = AssigngetItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_ASSIGNED, AssigngetItemresponse.status_code);





            //STEP 3: Update Observation by Assign person
            var Updatepayload = new CMMSAPIs.Models.Masters.CMObservation
            {
                id = obs_id,
                operator_name = "Meera Corporation",
                risk_type_id = 2,
                preventive_action = "test",
                assigned_to_id = 194,
                contact_number = "8696907375",
                cost_type = 1,
                date_of_observation = DateTime.Parse("2024-10-07"),
                type_of_observation = 1,
                location_of_observation = "cnhalli update",
                source_of_observation = 1,
                target_date = DateTime.Parse("2024-10-08"),
                observation_description = "test",
                uploadfileIds = new List<int>(),
                action_taken = "For New Location"

            };

            string UpdatejsonPayload = JsonConvert.SerializeObject(Updatepayload);

            var UpdateService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var UpdateResponse = UpdateService.CreateItem(Ep_UpdateObservation, UpdatejsonPayload);

            Assert.AreEqual(UpdateResponse.message, "Observation data updated successfully.");

            var UpdatedgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservation>();
            var UpdatedgetItemresponse = UpdatedgetItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_CREATED, UpdatedgetItemresponse.status_code);





            //STEP 4: Close Observation
            int check_point_type_id = 1;
            var Closepayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = obs_id,
                comment = "test"
            };

            string ClosejsonPayload = JsonConvert.SerializeObject(Closepayload);

            var CloseService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var CloseResponse = CloseService.CreateItem(Ep_CloseObservation + "?check_point_type_id=" + check_point_type_id, ClosejsonPayload);

            Assert.AreEqual(CloseResponse.message, $"Observation {obs_id} closed");

            var ClosegetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservationDetails>();
            var ClosegetItemresponse = ClosegetItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_CLOSED, ClosegetItemresponse.status_code);




            //STEP 5: Close Reject Observation
            var Rejectpayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = 44,
                comment = "test"
            };

            string jsonPayload = JsonConvert.SerializeObject(Rejectpayload);

            var RejectService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var RejectResponse = RejectService.PutService(Ep_RejectObservation + "?check_point_type_id=" + check_point_type_id, jsonPayload);

            Assert.AreEqual(RejectResponse[0].message, "Observation Rejected");

            var RejectgetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservationDetails>();
            var RejectgetItemresponse = RejectgetItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=" + check_point_type_id);

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_REJECTED, RejectgetItemresponse.status_code);




            //STEP 6: Close Approved Observation
            var Approvepayload = new CMMSAPIs.Models.Utils.CMApproval
            {
                id = obs_id,
                comment = "test"
            };

            string ApprovejsonPayload = JsonConvert.SerializeObject(Approvepayload);

            var ApproveService = new CMMS_Services.APIService<CMMSAPIs.Models.Utils.CMDefaultResponse>();
            var ApproveResponse = ApproveService.PutService(Ep_ApproveObservation + "?check_point_type_id=" + check_point_type_id, ApprovejsonPayload);

            Assert.AreEqual(ApproveResponse[0].message, "Observation Approved");

            var ApprovegetItem = new CMMS_Services.APIService<CMMSAPIs.Models.Masters.CMObservationDetails>();
            var ApprovegetItemresponse = ApprovegetItem.GetItem(Ep_GetObservationDetails + "?observation_id=" + obs_id + "&check_point_type_id=1");

            Assert.AreEqual((int)CMMS.CMMS_Status.OBSERVATION_APPROVED, ApprovegetItemresponse.status_code);


        }
    }
}
