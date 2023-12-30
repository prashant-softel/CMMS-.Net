using CMMSAPIs.BS.Audits;
using CMMSAPIs.Models.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Http;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.PM;

namespace CMMSAPIs.Controllers.Audits
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditPlanController : ControllerBase
    {
        private readonly IAuditPlanBS _AuditPlanBS;

        public AuditPlanController(IAuditPlanBS audit_plan)
        {
            _AuditPlanBS = audit_plan;
        }


        //[Authorize]
        [Route("GetAuditPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _AuditPlanBS.GetAuditPlanList(facility_id, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetAuditPlanByID")]
        [HttpGet]
        public async Task<IActionResult> GetAuditPlanByID(int id)
        {
            try
            {
                var data = await _AuditPlanBS.GetAuditPlanByID(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateAuditPlan(CMCreateAuditPlan request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.CreateAuditPlan(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            try
            {
                var data = await _AuditPlanBS.UpdateAuditPlan(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> DeleteAuditPlan([FromForm] int audit_plan_id)
        {
            try
            {
                var data = await _AuditPlanBS.DeleteAuditPlan(audit_plan_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> ApproveAuditPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.ApproveAuditPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("RejectAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> RejectAuditPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.RejectAuditPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }
                [Route("CreatePlan")]
        [HttpPost]
        public async Task<IActionResult> CreatePlan(CMPMPlanDetail request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.CreateAuditPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("StartAuditTask")]
        [HttpPost]
        public async Task<IActionResult> StartAuditTask(int task_id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.StartAuditTask(task_id, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid task id sent.";
                return Ok(item);
            }
        }

        [Route("UpdateAuditTaskExecution")]
        [HttpPost]
        public async Task<IActionResult> UpdateAuditTaskExecution(CMPMExecutionDetail request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.UpdateAuditTaskExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("ApprovePlan")]
        [HttpPost]
        public async Task<IActionResult> ApprovePlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.ApprovePlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("RejectPlan")]
        [HttpPost]
        public async Task<IActionResult> RejectPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.RejectPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("DeletePlan")]
        [HttpPut]
        public async Task<IActionResult> DeletePlan(int planId)

        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.DeletePlan(planId, userID);
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
    }
}
