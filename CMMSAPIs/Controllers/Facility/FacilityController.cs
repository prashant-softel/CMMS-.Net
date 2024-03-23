using CMMSAPIs.BS.Facility;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Facility;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace CMMSAPIs.Controllers.Facility
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityBS _FacilityBS;
        public FacilityController(IFacilityBS Facility)
        {
            _FacilityBS = Facility;
        }

        [Route("GetFacilityList")]
        [HttpGet]
        public async Task<IActionResult> GetFacilityList()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _FacilityBS.GetFacilityList(userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetFacilityDetails")]
        [HttpGet]
        public async Task<IActionResult> GetFacilityDetails(int id)
        {
            try
            {
                int facility_id = id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _FacilityBS.GetFacilityDetails(id, facilitytimeZone);
                return Ok(data);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
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

        [Route("GetBlockList")]
        [HttpGet]
        public async Task<IActionResult> GetBlockList(int parent_id)
        {
            try
            {
                var data = await _FacilityBS.GetBlockList(parent_id);
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
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateNewFacility")]
        [HttpPost]
        public async Task<IActionResult> CreateNewFacility(CMCreateFacility request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _FacilityBS.CreateNewFacility(request, userID);
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

        //[Authorize]
        [Route("UpdateFacility")]
        [HttpPatch]
        public async Task<IActionResult> UpdateFacility(CMCreateFacility request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _FacilityBS.UpdateFacility(request, userID);
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
        [Route("DeleteFacility")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFacility(int facility_id)
        {
            try
            {
                var data = await _FacilityBS.DeleteFacility(facility_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateNewBlock")]
        [HttpPost]
        public async Task<IActionResult> CreateNewBlock(CMCreateBlock request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _FacilityBS.CreateNewBlock(request, userID);
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
        [Route("UpdateBlock")]
        [HttpPatch]
        public async Task<IActionResult> UpdateBlock(CMCreateBlock request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _FacilityBS.UpdateBlock(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteBlock")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBlock(int block_id)
        {
            try
            {
                var data = await _FacilityBS.DeleteBlock(block_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}