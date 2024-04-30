using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using CMMSAPIs.BS.Permits;
using CMMSAPIs.BS.MoM;
using CMMSAPIs.Models.MoM;

namespace CMMSAPIs.Controllers.MoM
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoMController : Controller
    {
        private readonly IMoMBS _MoMBS;
        public MoMController(IMoMBS MoM)
        {
            _MoMBS = MoM;
        }

        [Route("GetMoMList")]
        [HttpGet]
        public async Task<IActionResult> GetMoMList(int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _MoMBS.GetMoMList(facility_id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetMoMDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMoMDetails(int facility_id, int mom_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _MoMBS.GetMoMDetails(facility_id, mom_id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("CreateMoM")]
        [HttpPost]
        public async Task<IActionResult> CreateMoM(CMMoM request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MoMBS.CreateMoM(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateMoM")]
        [HttpPost]
        public async Task<IActionResult> UpdateMoM(CMMoM request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MoMBS.UpdateMoM(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("DeleteMoM")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMoM(int momId)
        {
            try
            {
                var data = await _MoMBS.DeleteMoM(momId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("CloseMoM")]
        [HttpPut]
        public async Task<IActionResult> CloseMoM(int momId)
        {
            try
            {
                var data = await _MoMBS.CloseMoM(momId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetMoMAssignToDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMoMAssignToDetails(int mom_id)
        {
            try
            {
                var data = await _MoMBS.GetMoMAssignToDetails(mom_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetMoMTargetDateDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMoMTargetDateDetails(int mom_id)
        {
            try
            {
                var data = await _MoMBS.GetMoMTargetDateDetails(mom_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
