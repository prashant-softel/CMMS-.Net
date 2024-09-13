using CMMSAPIs.BS.Cleaning;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Controllers.Vegetation
{
    [Route("api/[controller]")]
    [ApiController]
    public class VegetationController : Controller
    {
//        private readonly vegetaion _vegetationBS;
        private readonly IMCVCBS _vegetationBS;



        public VegetationController(IMCVCBS vegetationBS)
        {
            _vegetationBS = vegetationBS;
            _vegetationBS.setModuleType(cleaningType.Vegetation);
        }


        [Route("GetVegetationPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationPlanList(int facilityId, string startDate, string endDate)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _vegetationBS.GetPlanList(facilityId, facilitytimeZone,startDate, endDate);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetVegetationTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationTaskList(int facility_Id, string startDate, string endDate)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_Id)?.timezone;

                var data = await _vegetationBS.GetTaskList(facility_Id, facilitytimeZone, startDate, endDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateVegetationPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateVegetationPlan(List<CMMCPlan> request)
        {
            try
            {
                // Assuming the facilityId is the same for all plans in the request list,
                // we can just take the facilityId from the first item in the list.
                int facilityId = request.FirstOrDefault()?.facilityId ?? 0;

                if (facilityId == 0)
                {
                    return BadRequest("Invalid facility ID.");
                }

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo"))
                    .FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                if (facilitytimeZone == null)
                {
                    return NotFound("Facility timezone not found.");
                }

                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.CreatePlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("UpdateVegetationPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdateVegetationPlan(List<CMMCPlan> request)
        {
            try
            {

                // Extract the facilityId from the first item in the request list
                int facilityId = request.FirstOrDefault()?.facilityId ?? 0;

                if (facilityId == 0)
                {
                    return BadRequest("Invalid facility ID.");
                }

                // Get the facility timezone based on the extracted facilityId
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo"))
                    .FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                if (facilitytimeZone == null)
                {
                    return NotFound("Facility timezone not found.");
                }

                // Get the user ID from the session
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.UpdatePlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetVegetationPlanDetails")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationPlanDetails(int planId, int facilityId)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _vegetationBS.GetPlanDetails(planId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetVegExecutionDetails")]
        [HttpGet]
        public async Task<IActionResult> GetVegExecutionDetails(int executionId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _vegetationBS.GetExecutionDetails(executionId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetVegScheduleDetails")]
        [HttpGet]
        public async Task<IActionResult> GetVegScheduleDetails(int scheduleId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _vegetationBS.GetScheduleDetails(scheduleId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        [Route("GetVegEquipmentList")]
        [HttpGet]
        public async Task<IActionResult> GetVegEquipmentList(int facilityId)
        {
            try
            {
                var data = await _vegetationBS.GetEquipmentList(facilityId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetVegTaskExecutionEquipmentList")]
        [HttpGet]
        public async Task<IActionResult> GetVegTaskExecutionEquipmentList(int taskId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _vegetationBS.GetTaskEquipmentList(taskId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveExecutionVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectExecutionVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectEndExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectEndExecutionVegetation(ApproveMC request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectEndExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("LinkPermitToVegetation")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToVegetation(int scheduleId, int permit_id, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.LinkPermitToMCVC(scheduleId, permit_id, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("StartExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> StartExecutionVegetation(int executionId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecution(executionId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("EndExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> EndExecutionVegetation(int executionId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.EndExecution(executionId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> StartScheduleExecutionVegetation(int scheduleId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartScheduleExecution(scheduleId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("UpdateScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> UpdateScheduleExecutionVegetation(CMMCGetScheduleExecution schedule, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.UpdateScheduleExecution(schedule, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AbandonExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> AbandonExecutionVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("AbandonScheduleVegetation")]
        [HttpPut]
        public async Task<IActionResult> AbandonScheduleVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonSchedule(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveAbandonExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveAbandonExecutionVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveAbandonExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectAbandonExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectAbandonExecutionVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectAbandonExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       /* [Route("CompleteExecutionVegetation")]
        [HttpPost]
        public async Task<IActionResult> CompleteExecutionVegetation(CMMCExecution request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.CompleteExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/
        [Route("ApproveVegetationPlan")]
        [HttpPost]
        public async Task<IActionResult> ApproveVegetationPlan(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApprovePlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveEndExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveEndExecutionVegetation(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveEndExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("EndScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> EndScheduleExecutionVegetation(int scheduleId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.EndScheduleExecution(scheduleId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectScheduleExecutionVegetation(ApproveMC request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectScheduleExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveScheduleExecutionVegetation(ApproveMC request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveScheduleExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectVegetationPlan(CMApproval request, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectPlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> DeleteVegetationPlan(int planId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.DeletePlan(planId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> StartVegetationExecution(int executionId, int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecution(executionId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ReAssignTaskVegetation")]
        [HttpPut]
        public async Task<IActionResult> ReAssignTaskVegetation(int task_id, int assign_to, int facilityId)
        {

            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ReAssignTask(task_id, assign_to, userID, facilitytimeZone);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
