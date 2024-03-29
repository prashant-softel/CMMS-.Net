﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using CMMSAPIs.Models.MC;
using CMMSAPIs.BS.Cleaning;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using CMMSAPIs.Helper;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using static CMMSAPIs.Helper.CMMS;
using Newtonsoft.Json;
using System.Linq;

namespace CMMSAPIs.Controllers.Vegetation
{
    [Route("api/[controller]")]
    [ApiController]
    public class VegetationController : Controller
    {
        private  CleaningBS _CleaningBS;

        public VegetationController(CleaningBS Cleaning)
        {         
            _CleaningBS = Cleaning;
            _CleaningBS.setModuleType(cleaningType.Vegetation);

        }

        [Route("GetVegetationPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationPlanList(int facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _CleaningBS.GetPlanList(facilityId, facilitytimeZone);
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

                var data = await _CleaningBS.GetTaskList(facilityId, facilitytimeZone);
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
                var data = await _CleaningBS.CreatePlan(request,userId);
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
                var data = await _CleaningBS.UpdatePlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetVegetationPlanDetails")]
        [HttpGet]
        public async Task<IActionResult> GetVegetationPlanDetails(int planId, int  facility_id)
        {
            try
            {
              
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _CleaningBS.GetPlanDetails(planId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> ApproveVegetationPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ApprovePlan(request, userId);
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
                var data = await _CleaningBS.RejectPlan(request,userId);
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
                var data = await _CleaningBS.DeletePlan(planId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> StartVegetationExecution(int planId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.StartExecution(planId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartVegScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> StartVegScheduleExecution(int scheduleId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.StartScheduleExecution(scheduleId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("UpadateVegScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> UpadateVegScheduleExecution(CMMCGetScheduleExecution schedule)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.UpdateScheduleExecution(schedule, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetVegExecutionDetails")]
        [HttpGet]
        public async Task<IActionResult> GetVegExecutionDetails(int executionId,int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CleaningBS.GetExecutionDetails(executionId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AbandonVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> AbandonVegetationExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.AbandonExecution(request, userId);
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
                var data = await _CleaningBS.GetVegEquipmentList(facilityId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Route("StartMCExecution")]
        //[HttpPost]
        //public async Task<IActionResult> StartMCExecution(int id)
        //{
        //    try
        //    {
        //        var data = await _CleaningBS.StartMCExecution(id);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}



        //[Route("CompleteMCExecution")]
        //[HttpPost]
        //public async Task<IActionResult> CompleteMCExecution(CMMCExecution request)
        //{
        //    try
        //    {
        //        var data = await _CleaningBS.CompleteMCExecution(request);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //[Route("ApproveMCExecution")]
        //[HttpPut]
        //public async Task<IActionResult> ApproveMCExecution(CMApproval request)
        //{
        //    try
        //    {
        //        var data = await _CleaningBS.ApproveMCExecution(request);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //[Route("RejectMCExecution")]
        //[HttpPut]
        //public async Task<IActionResult> RejectMCExecution(CMApproval request)
        //{
        //    try
        //    {
        //        var data = await _CleaningBS.RejectMCExecution(request);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
