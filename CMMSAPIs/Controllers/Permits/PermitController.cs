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
        [Authorize]
        [Route("GetPermitTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetPermitTypeList(int facility_id)
        {
            try
            {
                var data = await _PermitBS.GetPermitTypeList(facility_id);
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
        [Route("CreatePermitType")]
        [HttpPost]
        public async Task<IActionResult> CreatePermitType(CMCreatePermitType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.CreatePermitType(request, userID);
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
        [Route("UpdatePermitType")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePermitType(CMCreatePermitType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.UpdatePermitType(request, userID);
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
        [Route("DeletePermitType")]
        [HttpDelete]
        public async Task<IActionResult> DeletePermitType(int id)
        {
            try
            {
                var data = await _PermitBS.DeletePermitType(id);
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
        [Route("GetSafetyMeasurementQuestionList")]
        [HttpGet]
        public async Task<IActionResult> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            try
            {
                var data = await _PermitBS.GetSafetyMeasurementQuestionList(permit_type_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateSafetyMeasure")]
        [HttpPost]
        public async Task<IActionResult> CreateSafetyMeasure(CMCreateSafetyMeasures request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.CreateSafetyMeasure(request, userID);
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
        [Route("UpdateSafetyMeasure")]
        [HttpPatch]
        public async Task<IActionResult> UpdateSafetyMeasure(CMCreateSafetyMeasures request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.UpdateSafetyMeasure(request, userID);
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
        [Route("DeleteSafetyMeasure")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSafetyMeasure(int id)
        {
            try
            {
                var data = await _PermitBS.DeleteSafetyMeasure(id);
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
        [Route("GetJobTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetJobTypeList(int facility_id)
        {
            try
            {
                var data = await _PermitBS.GetJobTypeList(facility_id);
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
        [Route("CreateJobType")]
        [HttpPost]
        public async Task<IActionResult> CreateJobType(CMCreateJobType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.CreateJobType(request, userID);
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
        [Route("UpdateJobType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateJobType(CMCreateJobType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.UpdateJobType(request, userID);
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
        [Route("DeleteJobType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteJobType(int id)
        {
            try
            {
                var data = await _PermitBS.DeleteJobType(id);
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
        [Route("GetSOPList")]
        [HttpGet]
        public async Task<IActionResult> GetSOPList(int job_type_id)
        {
            try
            {
                var data = await _PermitBS.GetSOPList(job_type_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateSOP")]
        [HttpPost]
        public async Task<IActionResult> CreateSOP(CMCreateSOP request)
        {
            try
            {
                var data = await _PermitBS.CreateSOP(request);
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
        [Route("UpdateSOP")]
        [HttpPatch]
        public async Task<IActionResult> UpdateSOP(CMCreateSOP request)
        {
            try
            {
                var data = await _PermitBS.UpdateSOP(request);
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
        [Route("DeleteSOP")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSOP(int id)
        {
            try
            {
                var data = await _PermitBS.DeleteSOP(id);
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
        /*
         * Permit Main Feature End Points
        */
        [Authorize]
        [Route("GetPermitList")]
        [HttpGet]
        public async Task<IActionResult> GetPermitList(int facility_id, bool self_view)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.GetPermitList(facility_id, userID, self_view);
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
        [Route("CreatePermit")]
        [HttpPost]
        public async Task<IActionResult> CreatePermit(CMCreatePermit request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.CreatePermit(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetPermitDetails")]
        [HttpGet]
        public async Task<IActionResult> GetPermitDetails(int permit_id)
        {
            try
            {
                var data = await _PermitBS.GetPermitDetails(permit_id);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MissingMemberException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
         * Permit Issue/Approval/Rejection/Cancel End Points
        */
        [Authorize]
        [Route("PermitIssue")]
        [HttpPut]
        public async Task<IActionResult> PermitIssue([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitIssue(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitApprove")]
        [HttpPut]
        public async Task<IActionResult> PermitApprove([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitApprove(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitExtend")]
        [HttpPut]
        public async Task<IActionResult> PermitExtend([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitExtend(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitExtendApprove")]
        [HttpPut]
        public async Task<IActionResult> PermitExtendApprove([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitExtendApprove(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitExtendCancel")]
        [HttpPut]
        public async Task<IActionResult> PermitExtendCancel([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitExtendCancel(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitClose")]
        [HttpPut]
        public async Task<IActionResult> PermitClose([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitClose(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitReject")]
        [HttpPut]
        public async Task<IActionResult> PermitReject([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitReject(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [Authorize]
        [Route("PermitIssueReject")]
        [HttpPut]
        public async Task<IActionResult> PermitIssueReject([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitIssueReject(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [Authorize]
        [Route("PermitCancelRequest")]
        [HttpPut]
        public async Task<IActionResult> PermitCancelRequest([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitCancelRequest(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitCancelReject")]
        [HttpPut]
        public async Task<IActionResult> PermitCancelReject([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitCancelReject(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitCancelByApprover")]
        [HttpPut]
        public async Task<IActionResult> PermitCancelByApprover([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitCancelByApprover(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitCancelByHSE")]
        [HttpPut]
        public async Task<IActionResult> PermitCancelByHSE([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitCancelByHSE(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("PermitCancelByIssuer")]
        [HttpPut]
        public async Task<IActionResult> PermitCancelByIssuer([FromForm] CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.PermitCancelByIssuer(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdatePermit")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePermit(CMUpdatePermit request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PermitBS.UpdatePermit(request, userID);
                return Ok(data);
            }
            catch (AccessViolationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (MissingFieldException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
