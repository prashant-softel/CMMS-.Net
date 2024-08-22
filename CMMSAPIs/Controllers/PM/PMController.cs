using CMMSAPIs.BS.PM;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //[Authorize]
        [Route("CreatePMPlan")]
        [HttpPost]
        public async Task<IActionResult> CreatePMPlan(CMPMPlanDetail pm_plan,int facility_id)
        {
            try
            {
                string facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.CreatePMPlan(pm_plan, userID, facilitytimeZone);
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

        [Route("UpdatePMPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdatePMPlan(CMPMPlanDetail request,int facility_id)
        {
            try
            {
                string facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.UpdatePMPlan(request, userID, facilitytimeZone);
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
        [Route("GetPMPlanDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPMPlanDetail(int facility_id, int planId)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;


                var data = await _PMBS.GetPMPlanDetail(planId, facilitytimeZone);
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
        [Route("ApprovePMPlan")]
        [HttpPut]
        public async Task<IActionResult> ApprovePMPlan(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.ApprovePMPlan(request, userID, facilitytimeZone);
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
        [Route("RejectPMPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectPMPlan(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.RejectPMPlan(request, userID, facilitytimeZone);
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
        [Route("DeletePMPlan")]
        [HttpPut]
        public async Task<IActionResult> DeletePMPlan(int planId,int facility_id)

        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.DeletePMPlan(planId, userID,facilitytimeZone);
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
        [Route("GetPMPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetPMPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _PMBS.GetPMPlanList(facility_id, category_id, frequency_id, start_date, end_date, facilitytimeZone);
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
        [Route("GetScheduleData")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleData(int facility_id, int category_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _PMBS.GetScheduleData(facility_id, category_id, facilitytimeZone);
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

        ////[Authorize]
        //[Route("RejectPMPlan")]
        //[HttpPut]
        //internal async Task<IActionResult> RejectPMPlan(CMApproval request)
        //{
        //    try
        //    {
        //        int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
        //        var data = await _PMBS.RejectPMPlan(request, userID);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        ////[Authorize]
        //[Route("deleteHaresh")]
        //[HttpPut]
        //internal async Task<IActionResult> DeletePMPlan(int planId)
        //{
        //    try
        //    {
        //        int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
        //        var data = await _PMBS.DeletePMPlan(planId, userID);
        //        return Ok(data);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        [Route("ImportPMPlanFile")]
        [HttpPost]
        public async Task<IActionResult> ImportPMPlanFile(int file_id, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.ImportPMPlanFile(file_id, facility_id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeletePMTask")]
        [HttpPost]
        public async Task<IActionResult> DeletePMTask(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMBS.DeletePMTask(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
