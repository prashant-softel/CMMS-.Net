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
        [Route("GetScheduleViewList")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleViewList(int facility_id, DateTime? start_date, DateTime? end_date)
        {
            try
            {
                var data = await _PMScheduleViewBS.GetScheduleViewList(facility_id, start_date, end_date);
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
        [Route("CancelPMScheduleView")]
        [HttpPut]
        public async Task<IActionResult> CancelPMScheduleView(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.CancelPMScheduleView(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetPMScheduleViewDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPMScheduleViewDetail(int schedule_id)
        {
            try
            {
                var data = await _PMScheduleViewBS.GetPMScheduleViewDetail(schedule_id);
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
        [Route("SetPMScheduleView")]
        [HttpGet]
        public async Task<IActionResult> SetPMScheduleView(CMPMScheduleExecution request)
        {
            try
            {
                var data = await _PMScheduleViewBS.SetPMScheduleView(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdatePMScheduleExecution")]
        [HttpGet]
        public async Task<IActionResult> UpdatePMScheduleExecution(CMPMScheduleExecution request)
        {
            try
            {
                var data = await _PMScheduleViewBS.UpdatePMScheduleExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApprovePMScheduleExecution")]
        [HttpGet]
        public async Task<IActionResult> ApprovePMScheduleExecution(CMApproval request)
        {
            try
            {
                var data = await _PMScheduleViewBS.ApprovePMScheduleExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectPMScheduleExecution")]
        [HttpGet]
        public async Task<IActionResult> RejectPMScheduleExecution(CMApproval request)
        {
            try
            {
                var data = await _PMScheduleViewBS.RejectPMScheduleExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
