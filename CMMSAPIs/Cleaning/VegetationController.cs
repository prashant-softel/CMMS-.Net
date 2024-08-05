﻿using CMMSAPIs.Cleaning;
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
        private readonly vegetaion _vegetationBS;


        public VegetationController(VegBS vegetationBS)
        {
            _vegetationBS = (vegetaion)vegetationBS;
            _vegetationBS.setModuleType(cleaningType.Vegetation);
        }


        [Route("GetVegetationPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationPlanList(int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _vegetationBS.GetPlanList(facilityId, facilitytimeZone);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetVegetationTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationTaskList(int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _vegetationBS.GetTaskList(facilityId, facilitytimeZone);
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
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.CreatePlan(request, userId);
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
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.UpdatePlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetVegetationPlanDetails")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationPlanDetails(int planId, int facility_id)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

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
        public async Task<IActionResult> GetVegExecutionDetails(int executionId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _vegetationBS.GetExecutionDetails(executionId, facilitytimeZone);
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
                var data = await _vegetationBS.GetVegEquipmentList(facilityId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetVegTaskExecutionEquipmentList")]
        [HttpGet]
        public async Task<IActionResult> GetVegTaskExecutionEquipmentList(int taskId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

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
        public async Task<IActionResult> ApproveExecutionVegetation(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectExecutionVegetation(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectEndExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectEndExecutionVegetation(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectEndExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("LinkPermitToVegetation")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToVegetation(int scheduleId, int permit_id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.LinkPermitToVegetation(scheduleId, permit_id, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("StartExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> StartExecutionVegetation(int executionId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecutionVegetation(executionId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("EndExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> EndExecutionVegetation(int executionId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.EndExecutionVegetation(executionId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> StartScheduleExecutionVegetation(int scheduleId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartScheduleExecutionVegetation(scheduleId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("UpdateScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> UpdateScheduleExecutionVegetation(CMMCGetScheduleExecution schedule)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.UpdateScheduleExecutionVegetation(schedule, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AbandonExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> AbandonExecutionVegetation(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("AbandonScheduleVegetation")]
        [HttpPut]
        public async Task<IActionResult> AbandonScheduleVegetation(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonScheduleVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveAbandonExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveAbandonExecutionVegetation(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveAbandonExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectAbandonExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectAbandonExecutionVegetation(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectAbandonExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("CompleteExecutionVegetation")]
        [HttpPost]
        public async Task<IActionResult> CompleteExecutionVegetation(CMMCExecution request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.CompleteExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveVegetationPlan")]
        [HttpPost]
        public async Task<IActionResult> ApproveVegetationPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveVegetationPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveEndExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveEndExecutionVegetation(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveEndExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("EndScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> EndScheduleExecutionVegetation(int scheduleId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.EndScheduleExecutionVegetation(scheduleId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> RejectScheduleExecutionVegetation(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectScheduleExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveScheduleExecutionVegetation")]
        [HttpPut]
        public async Task<IActionResult> ApproveScheduleExecutionVegetation(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveScheduleExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectVegetationPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> DeleteVegetationPlan(int planId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.DeletePlan(planId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> StartVegetationExecution(int executionId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecutionVegetation(executionId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ReAssignTaskVegetation")]
        [HttpPut]
        public async Task<IActionResult> ReAssignTaskVegetation(int task_id, int assign_to)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ReAssignTaskVegetation(task_id, assign_to, userID);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
