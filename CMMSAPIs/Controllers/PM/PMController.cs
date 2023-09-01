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
        [Route("CreatePMPlan")]
        [HttpPost]
        public async Task<IActionResult> CreatePMPlan(CMPMPlanDetail pm_plan)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.CreatePMPlan(pm_plan, userID);
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
        [Route("GetPMPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetPMPlanList(int facility_id, int category_id, int frequency_id, DateTime? start_date, DateTime? end_date)
        {
            try
            {
                var data = await _PMBS.GetPMPlanList(facility_id, category_id, frequency_id, start_date, end_date);
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
        [Route("GetPMPlanDetail")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleData(int id)
        {
            try
            {
                var data = await _PMBS.GetPMPlanDetail(id);
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
