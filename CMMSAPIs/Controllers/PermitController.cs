using CMMSAPIs.BS;
using CMMSAPIs.Models;
using CMMSAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers
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
        public async Task<IActionResult> GetPermitList(int facility_id)
        {
            try
            {
                var data = await _PermitBS.GetPermitList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreatePermit")]
        [HttpPost]
        public async Task<IActionResult> CreatePermit()
        {
            try
            {
                var data = await _PermitBS.CreatePermit();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
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
        public async Task<IActionResult> PermitIssue()
        {
            try
            {
                var data = await _PermitBS.PermitIssue();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("PermitApprove")]
        [HttpPost]
        public async Task<IActionResult> PermitApprove()
        {
            try
            {
                var data = await _PermitBS.PermitApprove();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("PermitReject")]
        [HttpPost]
        public async Task<IActionResult> PermitReject()
        {
            try
            {
                var data = await _PermitBS.PermitReject();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("PermitCancel")]
        [HttpPost]
        public async Task<IActionResult> PermitCancel()
        {
            try
            {
                var data = await _PermitBS.PermitCancel();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
