﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;
using CMMSAPIs.Models.MC;
using CMMSAPIs.BS.Cleaning;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.Repositories.CleaningRepository;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Controllers.MC
{

    [Route("api/[controller]")]
    [ApiController]
    public class MCController : ControllerBase
    {
        private readonly CleaningBS _CleaningBS;
        public MCController(CleaningBS Cleaning)
        {
            _CleaningBS = Cleaning;
            _CleaningBS.setModuleType(cleaningType.ModuleCleaning);
        }

        [Route("GetMCPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetMCPlanList(int facilityId)
        {
            try
            {
                var data = await _CleaningBS.GetPlanList(facilityId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetMCTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetMCTaskList(int facilityId)
        {
            try
            {
                var data = await _CleaningBS.GetTaskList(facilityId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetMCPlanDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMCPlanDetails(int planId)
        {
            try
            {
                var data = await _CleaningBS.GetPlanDetails(planId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateMCPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateMCPlan(List<CMMCPlan> request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.CreatePlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateMCPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdateMCPlan(CMMCPlan request)
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

        [Route("ApproveMCPlan")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCPlan(CMApproval request)
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

        [Route("RejectMCPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectMCPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.RejectPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteMCPlan")]
        [HttpPut]
        public async Task<IActionResult> DeleteMCPlan(int planId)
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

        [Route("StartMCExecution")]
        [HttpPut]
        public async Task<IActionResult> StartMCExecution(int planId)
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

        [Route("StartMCScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> StartMCScheduleExecution(int scheduleId)
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
        [Route("EndMCScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> EndMCScheduleExecution(CMMCExecutionSchedule schedule)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.EndScheduleExecution(schedule, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetMCExecutionDetails")]
        [HttpPost]
        public async Task<IActionResult> GetMCExecutionDetails(int executionId)
        {
            try
            {
                var data = await _CleaningBS.GetExecutionDetails(executionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AbandonMcExecution")]
        [HttpPut]
        public async Task<IActionResult> AbandonMcExecution(CMApproval request)
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
        [Route("GetMCEquipmentList")]
        [HttpGet]
        public async Task<IActionResult> GetMCEquipmentList(int facilityId)
        {
            try
            {
                var data = await _CleaningBS.GetEquipmentList(facilityId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}