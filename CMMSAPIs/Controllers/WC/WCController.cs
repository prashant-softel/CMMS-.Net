using CMMSAPIs.BS.WC;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.WC
{
    [Route("api/[controller]")]
    [ApiController]
    public class WCController : ControllerBase
    {
        private readonly IWCBS _WCBS;
        public WCController(IWCBS wc)
        {
            _WCBS = wc;
        }

        //[Authorize]
        [Route("GetWCList")]
        [HttpGet]
        public async Task<IActionResult> GetWCList(int facilityId, string start_Date, string end_Date, int statusId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _WCBS.GetWCList(facilityId, start_Date, end_Date, statusId, userID, facilitytimeZone);
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
        [Route("CreateWC")]
        [HttpPost]
        public async Task<IActionResult> CreateWC(List<CMWCCreate> request)
        {
            try
            {
                int facilityId = request[0].facilityId;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.CreateWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);

            }
        }

        //[Authorize]
        [Route("GetWCDetails")]
        [HttpGet]
        public async Task<IActionResult> GetWCDetails(int wc_id, int facilityId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _WCBS.GetWCDetails(wc_id, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("UpdateWC")]
        [HttpPatch]
        public async Task<IActionResult> UpdateWC(CMWCCreate request)
        {
            try
            {
                int facilityId = request.facilityId;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.UpdateWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("ApproveWC")]
        [HttpPost]
        public async Task<IActionResult> ApproveWC(CMApproval request)
        {
            try
            {
                int facilityId = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.ApproveWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("updateWCimages")]
        [HttpPost]
        public async Task<IActionResult> updateWCimages(filesforwc request)
        {
            try
            {
                int facilityId = request.facilityId;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.updateWCimages(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("ClosedWC")]
        [HttpPost]
        public async Task<IActionResult> ClosedWC(CMApproval request)
        {
            try
            {
                int facilityId = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.ClosedWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("ApprovedClosedWC")]
        [HttpPost]
        public async Task<IActionResult> ApprovedClosedWC(CMApproval request)
        {
            try
            {
                int facilityId = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.ApprovedClosedWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("RejectClosedWC")]
        [HttpPost]
        public async Task<IActionResult> RejectClosedWC(CMApproval request)
        {
            try
            {
                int facilityId = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.RejectClosedWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        //[Authorize]
        [Route("RejectWC")]
        [HttpPut]
        public async Task<IActionResult> RejectWC(CMApproval request)
        {
            try
            {
                int facilityId = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.RejectWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
        [Route("CancelWC")]
        [HttpPost]
        public async Task<IActionResult> CancelWC(CMApproval request)
        {
            try
            {
                int facilityId = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.CancelWC(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
            }
        }
    }
}
