using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.BS.Calibration;

namespace CMMSAPIs.Controllers.Calibration
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalibrationController : ControllerBase
    {
        private readonly ICalibrationBS _CalibrationBS;
        public CalibrationController(ICalibrationBS calibration)
        {
            _CalibrationBS = calibration;
        }

        [Authorize]
        [Route("GetCalibrationList")]
        [HttpGet]
        public async Task<IActionResult> GetCalibrationList(int facility_id)
        {
            try
            {
                var data = await _CalibrationBS.GetCalibrationList(facility_id);
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
        [Route("RequestCalibration")]
        [HttpPost]
        public async Task<IActionResult> RequestCalibration(CMRequestCalibration request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.RequestCalibration(request, userID);
                return Ok(data);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApproveRequestCalibration")]
        [HttpPut]
        public async Task<IActionResult> ApproveRequestCalibration(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.ApproveRequestCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectRequestCalibration")]
        [HttpPut]
        public async Task<IActionResult> RejectRequestCalibration(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.RejectRequestCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetPreviousCalibration")]
        [HttpGet]
        public async Task<IActionResult> GetPreviousCalibration(int asset_id)
        {
            try
            {
                var data = await _CalibrationBS.GetPreviousCalibration(asset_id);
                return Ok(data);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("StartCalibration")]
        [HttpPost]
        public async Task<IActionResult> StartCalibration(int calibration_id)
        {
            try
            {
                var data = await _CalibrationBS.StartCalibration(calibration_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CompleteCalibration")]
        [HttpPut]
        public async Task<IActionResult> CompleteCalibration(CMCompleteCalibration request)
        {
            try
            {
                var data = await _CalibrationBS.CompleteCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CloseCalibration")]
        [HttpPut]
        public async Task<IActionResult> CloseCalibration(CMCloseCalibration request)
        {
            try
            {
                var data = await _CalibrationBS.CloseCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApproveCalibration")]
        [HttpPut]
        public async Task<IActionResult> ApproveCalibration(CMApproval request)
        {
            try
            {
                var data = await _CalibrationBS.ApproveCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectCalibration")]
        [HttpPut]
        public async Task<IActionResult> RejectCalibration(CMApproval request)
        {
            try
            {
                var data = await _CalibrationBS.RejectCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
