using CMMSAPIs.BS.Permits;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CMMSAPIs.Controllers.Permits
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermitController : ControllerBase
    {
        private readonly IPermitBS _PermitBS;
        public PermitController(IPermitBS Permit)
        {
            _PermitBS = Permit;
        }

        /* 
         * Permit Create Form Required End Points 
         */
       // [Authorize]
        [Route("GetPermitTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetPermitTypeList(int facility_id)
        {
            try
            {
                var data = await _PermitBS.GetPermitTypeList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetSafetyMeasurementQuestionList")]
        [HttpGet]
        public async Task<IActionResult> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            try
            {
                var data = await _PermitBS.GetSafetyMeasurementQuestionList(permit_type_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetJobTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetJobTypeList(int facility_id)
        {
            try
            {
                var data = await _PermitBS.GetJobTypeList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetSOPList")]
        [HttpGet]
        public async Task<IActionResult> GetSOPList(int job_type_id)
        {
            try
            {
                var data = await _PermitBS.GetSOPList(job_type_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Permit Main Feature End Points
        */
       // [Authorize]
        [Route("GetPermitList")]
        [HttpGet]
        public async Task<IActionResult> GetPermitList(int facility_id, int userID)
        {
            try
            {
                var data = await _PermitBS.GetPermitList(facility_id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreatePermit")]
        [HttpPost]
        public async Task<IActionResult> CreatePermit(CMCreatePermit request)
        {
            try
            {
                var data = await _PermitBS.CreatePermit(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetPermitDetails")]
        [HttpGet]
        public async Task<IActionResult> GetPermitDetails(int permit_id)
        {
            try
            {
                var data = await _PermitBS.GetPermitDetails(permit_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Permit Issue/Approval/Rejection/Cancel End Points
        */
        //[Authorize]
        [Route("PermitIssue")]
        [HttpPost]
        public async Task<IActionResult> PermitIssue([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitIssue(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("PermitApprove")]
        [HttpPost]
        public async Task<IActionResult> PermitApprove([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitApprove(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("PermitExtend")]
        [HttpPut]
        public async Task<IActionResult> PermitExtend([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitExtend(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("PermitExtendApprove")]
        [HttpPut]
        public async Task<IActionResult> PermitExtendApprove([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitExtendApprove(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("PermitExtendCancel")]
        [HttpPut]
        public async Task<IActionResult> PermitExtendCancel([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitExtendCancel(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("PermitClose")]
        [HttpPut]
        public async Task<IActionResult> PermitClose([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitClose(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("PermitReject")]
        [HttpPost]
        public async Task<IActionResult> PermitReject([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitReject(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //[Authorize]
        [Route("PermitCancel")]
        [HttpPost]
        public async Task<IActionResult> PermitCancel([FromForm] CMApproval request)
        {
            try
            {
                var data = await _PermitBS.PermitCancel(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdatePermit")]
        [HttpPost]
        public async Task<IActionResult> UpdatePermit(CMUpdatePermit request)
        {
            try
            {
                var data = await _PermitBS.UpdatePermit(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
