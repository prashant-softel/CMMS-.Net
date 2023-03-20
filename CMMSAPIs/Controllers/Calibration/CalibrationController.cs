using Microsoft.AspNetCore.Mvc;
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

        
        [Route("GetCalibrationList")]
        [HttpGet]
        public async Task<IActionResult> GetCalibrationList(int facility_id)
        {
            try
            {
                var data = await _CalibrationBS.GetCalibrationList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        [Route("RequestCalibration")]
        [HttpPost]
        public async Task<IActionResult> RequestCalibration(CMRequestCalibration request)
        {
            try
            {
                var data = await _CalibrationBS.RequestCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveRequestCalibration")]
        [HttpPut]
        public async Task<IActionResult> ApproveRequestCalibration(CMApproval request)
        {
            try
            {
                var data = await _CalibrationBS.ApproveRequestCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectRequestCalibration")]
        [HttpPut]
        public async Task<IActionResult> RejectRequestCalibration(CMApproval request)
        {
            try
            {
                var data = await _CalibrationBS.RejectRequestCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetPreviousCalibration")]
        [HttpGet]
        public async Task<IActionResult> GetPreviousCalibration(CMPreviousCalibration request)
        {
            try
            {
                var data = await _CalibrationBS.GetPreviousCalibration(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
