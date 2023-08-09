using CMMSAPIs.BS.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.PM;
using CMMSAPIs.BS.PM;
using System.Collections.Generic;

namespace CMMSAPIs.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMController : ControllerBase
    {
        private readonly IPMBS _PMBS;

        public PMController(IPMBS pm)
        {
            _PMBS = pm;
        }

        [Authorize]
        [Route("GetScheduleData")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleData(int facility_id, int category_id)
        {
            try
            {
                var data = await _PMBS.GetScheduleData(facility_id, category_id);
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
        [Route("SetScheduleData")]
        [HttpPost]
        public async Task<IActionResult> SetScheduleData(CMSetScheduleData request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.SetScheduleData(request, userID);
                return Ok(data);
            }/*
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }*/
            catch (Exception)
            {
                throw;
            }
        }
    }
}
