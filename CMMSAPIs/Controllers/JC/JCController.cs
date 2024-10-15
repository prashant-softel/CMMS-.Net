using CMMSAPIs.BS.JC;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.JC
{
    [Route("api/[controller]")]
    [ApiController]
    public class JCController : ControllerBase
    {
        private readonly IJCBS _JCBS;
        public JCController(IJCBS jc)
        {
            _JCBS = jc;
        }

        /*
         * JobCard Basic End Points
        */
        //[Authorize]
        [Route("GetJCList")]
        [HttpGet]
        public async Task<IActionResult> GetJCList(string facility_id, bool self_view, string start_date, string end_date)
        {
            try
            {

                // Split the comma-delimited string into an array of facility IDs (string array)
                var facilityIds = facility_id.Split(',');

                // Use the first facility ID from the array and convert it to an integer
                int firstFacilityId = int.Parse(facilityIds.FirstOrDefault());

                // Get the time zone for the first facility ID
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo"))
                    .FirstOrDefault(x => x.facility_id == firstFacilityId)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.GetJCList(facility_id, userID, self_view, start_date, end_date, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetJCListByJobId")]
        [HttpGet]
        public async Task<IActionResult> GetJCListByJobId(int jobId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.GetJCListByJobId(jobId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetJCDetail")]
        [HttpGet]
        public async Task<IActionResult> GetJCDetail(int jc_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _JCBS.GetJCDetail(jc_id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Authorize]
        [Route("CreateJC")]
        [HttpPost]
        public async Task<IActionResult> CreateJC(int job_id)

        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.CreateJC(job_id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateJC")]
        [HttpPut]
        public async Task<IActionResult> UpdateJC(CMJCUpdate request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.UpdateJC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CloseJC")]
        [HttpPut]
        public async Task<IActionResult> CloseJC(CMJCClose request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.CloseJC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApproveJC")]
        [HttpPut]
        public async Task<IActionResult> ApproveJC(CMJCApprove request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.ApproveJC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectJC")]
        [HttpPut]
        public async Task<IActionResult> RejectJC(CMJCReject request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.RejectJC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("StartJC")]
        [HttpPost]
        public async Task<IActionResult> StartJC(CMJCRequest request)
        {
            try
            {
                string facilitytime = "";
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.StartJC(request, userID, facilitytime);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        //[Authorize]
        [Route("CarryForwardJC")]
        [HttpPut]
        public async Task<IActionResult> CarryForwardJC(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.CarryForwardJC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApproveJCCF")]
        [HttpPut]
        public async Task<IActionResult> ApproveJCCF(CMJCApprove request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.ApproveJCCF(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectJCCF")]
        [HttpPut]
        public async Task<IActionResult> RejectJCCF(CMJCReject request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.RejectJCCF(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
