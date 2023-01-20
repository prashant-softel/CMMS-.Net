using CMMSAPIs.BS.JC;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;

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
        [Route("GetJCList")]
        [HttpGet]
        public async Task<IActionResult> GetJCList(int facility_id)
        {
            try
            {
                var data = await _JCBS.GetJCList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

        [Route("CreateJC")]
        [HttpPost]
        public async Task<IActionResult> CreateJC(int job_id)
        {
            try
            {
                var data = await _JCBS.CreateJC(job_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateJC")]
        [HttpPut]
        public async Task<IActionResult> UpdateJC(CMJCUpdate request)
        {
            try
            {
                var data = await _JCBS.UpdateJC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CloseJC")]
        [HttpPut]
        public async Task<IActionResult> CloseJC(CMJCClose request)
        {
            try
            {
                var data = await _JCBS.CloseJC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveJC")]
        [HttpPut]
        public async Task<IActionResult> ApproveJC([FromForm] CMJCApprove request)
        {
            try
            {
                var data = await _JCBS.ApproveJC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectJC")]
        [HttpPut]
        public async Task<IActionResult> RejectJC([FromForm] CMJCReject request)
        {
            try
            {
                var data = await _JCBS.RejectJC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
