using CMMSAPIs.BS.PM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        [Authorize]
        [Route("GetPMTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date)
        {
            try
            {
                var data = await _PMScheduleViewBS.GetPMTaskList(facility_id, start_date, end_date);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
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

        [Authorize]
        [Route("GetPMTaskDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskDetail(int schedule_id)
        {
            try
            {
                var data = await _PMScheduleViewBS.GetPMTaskDetail(schedule_id);
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

        [Authorize]
        [Route("LinkPermitToPMTask")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToPMTask(int schedule_id, int permit_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.LinkPermitToPMTask(schedule_id, permit_id, userID);
                return Ok(data);
            }
            catch(Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("SetPMTask")]
        [HttpPost]
        public async Task<IActionResult> SetPMTask(int schedule_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.SetPMTask(schedule_id, userID);
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

        [Authorize]
        [Route("UpdatePMTaskExecution")]
        [HttpGet]
        public async Task<IActionResult> UpdatePMTaskExecution(CMPMScheduleExecution request)
        {
            try
            {
                var data = await _PMScheduleViewBS.UpdatePMTaskExecution(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApprovePMTaskExecution")]
        [HttpGet]
        public async Task<IActionResult> ApprovePMTaskExecution(CMApproval request)
        {
            try
            {
                var data = await _PMScheduleViewBS.ApprovePMTaskExecution(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectPMTaskExecution")]
        [HttpGet]
        public async Task<IActionResult> RejectPMTaskExecution(CMApproval request)
        {
            try
            {
                var data = await _PMScheduleViewBS.RejectPMTaskExecution(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
