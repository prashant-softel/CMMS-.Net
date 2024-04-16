using CMMSAPIs.BS.JC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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
        public async Task<IActionResult> GetJCList(int facility_id, bool self_view)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.GetJCList(facility_id,  userID,  self_view, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetJCListByJobId")]
        [HttpGet]
        public async Task<IActionResult> GetJCListByJobId(int jobId,int facility_id)
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
        public async Task<IActionResult> GetJCDetail(int jc_id)
        {
            try
            {
                var data = await _JCBS.GetJCDetail(jc_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
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
        [HttpPut]
        public async Task<IActionResult> StartJC(int jc_id, CMJCDetail request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JCBS.StartJC( jc_id,  request,  userID);
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
