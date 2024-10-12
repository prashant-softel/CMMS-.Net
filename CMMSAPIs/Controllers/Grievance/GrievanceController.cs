using CMMSAPIs.BS.Grievance;
using CMMSAPIs.Models.Grievance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models.Utils;
using Newtonsoft.Json;
using System.Linq;
using CMMSAPIs.BS.Facility;

namespace CMMSAPIs.Controllers.Grievance
{
    [Route("api/[controller]")]
    [ApiController]
   
        public class GrievanceController : ControllerBase
        {

            private readonly IGrievanceBS _Grievance;
            public GrievanceController(IGrievanceBS grievance)
            {
                _Grievance = grievance;
            }





        //[Authorize]
        [Route("GetGrievanceList")]
            [HttpGet]
            public async Task<IActionResult> GetGrievanceList(string facilityId, string status, string startDate, string endDate, int selfView)
            {
             try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => facilityId == facilityId)?.timezone;
                var data = await _Grievance.GetGrievanceList(facilityId, status, startDate, endDate, selfView, facilitytimeZone);
                return Ok(data);
            }
                 catch (Exception ex)
                {
                    // Handle the exception as needed
                    return BadRequest(); // or return another IActionResult
                }
            }

            //[Authorize]
            [Route("GetGrievanceDetails")]
            [HttpGet]
            public async Task<IActionResult> GetGrievanceDetails(int id, int facilityId)
            {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => facilityId == facilityId)?.timezone;
                var data = await _Grievance.GetGrievanceDetails(id, facilityId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
                {
                    throw;
                }
            }

            //[Authorize]
            [Route("CreateGrievance")]
            [HttpPost]
            public async Task<IActionResult> CreateGrievance(CMCreateGrievance request, string facilityId)
            {
                if (request is null)
            {
               
                throw new ArgumentNullException(nameof(request));
                }

                try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => facilityId == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                int status = 1;
                var data = await _Grievance.CreateGrievance(request,userID, facilityId, facilitytimeZone, status);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //[Authorize]
            [Route("UpdateGrievance")]
            [HttpPost]
            public async Task<IActionResult> UpdateGrievance(CMUpdateGrievance request, string facilityId)
            {
                try
                {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => facilityId == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _Grievance.UpdateGrievance(request, userID, facilityId, facilitytimeZone);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //[Authorize]
            [Route("DeleteGrievance")]
            [HttpDelete]
            public async Task<IActionResult> DeleteGrievance(int id, int userID, string facilityId)
            {
                try
                {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => facilityId == facilityId)?.timezone;
                var data = await _Grievance.DeleteGrievance(id, userID, facilityId, facilitytimeZone);
                return Ok(data);
            }
                catch (Exception ex)
                {
                    throw;
                }
            }

        [Route("CloseGrievance")]
            [HttpPut]
            public async Task<IActionResult> CloseGrievannce(CMGrievance request, string facilityId)
            {
                try
                {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => facilityId == facilityId)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _Grievance.CloseGrievance(request, userID, facilityId, facilitytimeZone);
                return Ok(data);
            }
                catch (Exception ex)
                {
                    throw;
                }
            }

        [Route("GrievanceSummaryReport")]
        [HttpGet]

        public async Task<IActionResult> GetObservationSummaryReport(string facilityId, string fromDate, string toDate)
        {
            try
            {
                var data = await _Grievance.GrievanceSummaryReport(facilityId, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}