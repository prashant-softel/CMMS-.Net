using CMMSAPIs.BS.Calibration;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //[Authorize]
        [Route("GetCalibrationList")]
        [HttpGet]
        public async Task<IActionResult> GetCalibrationList(int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CalibrationBS.GetCalibrationList(facility_id, facilitytimeZone);
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

        //[Authorize]
        [Route("GetCalibrationDetails")]
        [HttpGet]
        public async Task<IActionResult> GetCalibrationDetails(int id, int facilty_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilty_id)?.timezone;
                var data = await _CalibrationBS.GetCalibrationDetails(id, facilitytimeZone);
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

        //[Authorize]
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
            catch (ArgumentException ex)
            {

                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }

        }

        //[Authorize]
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
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
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
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("GetPreviousCalibration")]
        [HttpGet]
        public async Task<IActionResult> GetPreviousCalibration(int asset_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _CalibrationBS.GetPreviousCalibration(asset_id, facilitytimeZone);
                return Ok(data);
            }

            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("StartCalibration")]
        [HttpPut]
        public async Task<IActionResult> StartCalibration(int calibration_id, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CalibrationBS.StartCalibration(calibration_id, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CompleteCalibration")]
        [HttpPut]
        public async Task<IActionResult> CompleteCalibration(CMCompleteCalibration request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.CompleteCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("CloseCalibration")]
        [HttpPut]
        public async Task<IActionResult> CloseCalibration(CMCloseCalibration request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.CloseCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApproveCalibration")]
        [HttpPut]
        public async Task<IActionResult> ApproveCalibration(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.ApproveCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("RejectCalibration")]
        [HttpPut]
        public async Task<IActionResult> RejectCalibration(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.RejectCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        [Route("SkipCalibration")]
        [HttpPut]
        public async Task<IActionResult> SkipCalibration(CMCloseCalibration request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CalibrationBS.SkipCalibration(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }
    }
}
