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

        [Route("CreatePermit")]
        [HttpPost]
        public async Task<IActionResult> CreatePermit(CreatePermitModel request)
        {
            try
            {
                var data = await _PermitBS.CreatePermit(request);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

       
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

        [Route("PermitIssue")]
        [HttpPost]
        public async Task<IActionResult> PermitIssue([FromForm] ApprovalModel request)
        {
            String status;
            try
            {
                var data = await _PermitBS.PermitIssue(request);
                status = "Issued Successfully";
            }
            catch (Exception ex)
            {
                status = "something went wrong " + ex;
                throw;
            }
            return Ok(status);

        }

        [Route("PermitApprove")]
        [HttpPut]
        public async Task<IActionResult> PermitApprove([FromForm] ApprovalModel request)
        {
           String status;
            try
            {
                var data = await _PermitBS.PermitApprove(request);
                status = "Approved Successfully";
/*                return Ok(data);
*/            }
            catch (Exception ex)
            {
                status = "something went wrong "+ex;
                throw;
            }
            return Ok(status);
        }

        [Route("PermitReject")]
        [HttpPost]
        public async Task<IActionResult> PermitReject(ApprovalModel request)
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

        [Route("PermitCancel")]
        [HttpPost]
        public async Task<IActionResult> PermitCancel([FromForm] ApprovalModel request)
        {
            String status;
            try
            {
                var data = await _PermitBS.PermitCancel(request);
                status = "Cancelled Successfully";
            }
            catch (Exception ex)
            {
                status = "something went wrong " + ex;
                throw;
            }
            return Ok(status);

        }


        [Route("UpdatePermit")]
        [HttpPost]
        public async Task<IActionResult> UpdatePermit(UpdatePermitModel request)
        {
            String status;
            try
            {
                var data = await _PermitBS.UpdatePermit(request);
                status = "Update permit Successfully";
                /*                return Ok(data);
                */
            }
            catch (Exception ex)
            {
                status = "something went wrong " + ex;
                throw;
            }
            return Ok(status);
        }

    }
}
