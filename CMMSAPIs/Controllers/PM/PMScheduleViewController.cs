﻿using CMMSAPIs.BS.PM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.PM;

namespace CMMSAPIs.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMScheduleViewController : ControllerBase
    {
        private readonly IPMScheduleViewBS _PMScheduleViewBS;

        public PMScheduleViewController(IPMScheduleViewBS pm_schedule_view)
        {
            _PMScheduleViewBS = pm_schedule_view;
        }

        //[Authorize]
        [Route("CancelPMTask")]
        [HttpPut]
        public async Task<IActionResult> CancelPMTask(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.CancelPMTask(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetPMTaskDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskDetail(int task_id)
        {
            try
            {
                var data = await _PMScheduleViewBS.GetPMTaskDetail(task_id);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(MissingMemberException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetPMTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds)
        {
            try
            {
                var data = await _PMScheduleViewBS.GetPMTaskList(facility_id,  start_date,  end_date, frequencyIds, categoryIds);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MissingMemberException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("LinkPermitToPMTask")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToPMTask(int task_id, int permit_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.LinkPermitToPMTask(task_id, permit_id, userID);
                return Ok(data);
            }
            catch(Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddCustomCheckpoint")]
        [HttpPost]
        public async Task<IActionResult> AddCustomCheckpoint(CMCustomCheckPoint request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.AddCustomCheckpoint(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("StartPMTask")]
        [HttpPost]
        public async Task<IActionResult> StartPMTask(int task_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.StartPMTask(task_id, userID);
                return Ok(data);
            }
            catch (FieldAccessException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdatePMTaskExecution")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePMTaskExecution(CMPMExecutionDetail request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.UpdatePMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ClosePMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> ClosePMTaskExecution(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.ClosePMTaskExecution(request, userID);
                //request.comment = "Approved";
                //var data2 = _PMScheduleViewBS.ApprovePMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApprovePMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> ApprovePMTaskExecution(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.ApprovePMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectPMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectPMTaskExecution(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.RejectPMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
