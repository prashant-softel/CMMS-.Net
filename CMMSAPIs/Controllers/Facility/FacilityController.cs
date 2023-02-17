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
        public async Task<IActionResult> GetFacilityList(int facility_id)
        {
            try
            {
                var data = await _FacilityBS.GetFacilityList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetFacilityList")]
        [HttpGet]
        public async Task<IActionResult> GetFacilityList()
        {
            try
            {
                var data = await _FacilityBS.GetFacilityList();
                return Ok(data);
            }
            catch (Exception ex)
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
                var data = await _FacilityBS.GetFacilityDetails(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateNewFacility")]
        [HttpPost]
        public async Task<IActionResult> CreateNewFacility(CMCreateFacility request)
        {
            try
            {
                var data = await _FacilityBS.CreateNewFacility(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateFacility")]
        [HttpPost]
        public async Task<IActionResult> UpdateFacility(CMUpdateFacility request)
        {
            try
            {
                var data = await _FacilityBS.UpdateFacility(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("DeleteFacility")]
        [HttpPut]
        public async Task<IActionResult> DeleteFacility(int facility_id)
        {
            try
            {
                var data = await _FacilityBS.DeleteFacility(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}