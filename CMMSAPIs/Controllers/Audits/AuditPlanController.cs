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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using CMMSAPIs.Models.Users;

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
        public async Task<IActionResult> GetAuditPlanList(int facility_id, DateTime fromDate, DateTime toDate,int module_type_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _AuditPlanBS.GetAuditPlanList(facility_id, fromDate, toDate, facilitytimeZone, module_type_id);
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
        public async Task<IActionResult> GetAuditPlanByID(int id,int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _AuditPlanBS.GetAuditPlanByID(id, facilitytimeZone);
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
                int facility_id = request.Facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.CreateAuditPlan(request, userID, facilitytimeZone);
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
        public async Task<IActionResult> DeleteAuditPlan(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.DeleteAuditPlan(request, userId, facilitytimeZone);
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

        [Route("ApproveAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> ApproveAuditPlan(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.ApproveAuditPlan(request, userId, facilitytimeZone);
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
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.RejectAuditPlan(request, userId, facilitytimeZone);
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
        public async Task<IActionResult> StartAuditTask(int task_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.StartAuditTask(task_id, userId, facilitytimeZone);
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
        [Route("GetPlanDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPlanDetail(int planId,int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                // int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.GetPlanDetail(planId, facilitytimeZone);
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

        [Route("GetPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _AuditPlanBS.GetPlanList(facility_id, category_id, frequency_id, start_date, end_date, facilitytimeZone);
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

        [Route("GetTaskDetail")]
        [HttpGet]
        public async Task<IActionResult> GetTaskDetail(int task_id,int facility_id)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _AuditPlanBS.GetTaskDetail(task_id, facilitytimeZone);
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

        [Route("GetTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, int module_type_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _AuditPlanBS.GetTaskList(facility_id, start_date, end_date, frequencyIds, facilitytimeZone, module_type_id);
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

        [Route("CreateAuditSkip")]
        [HttpPost]
        public async Task<IActionResult> CreateAuditSkip(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.CreateAuditSkip(request, userId, facilitytimeZone);
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
        [Route("RejectAuditSkip")]
        [HttpPost]
        public async Task<IActionResult> RejectAuditSkip(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.RejectAuditSkip(request, userId, facilitytimeZone);
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
        [Route("ApproveAuditSkip")]
        [HttpPost]
        public async Task<IActionResult> ApproveAuditSkip(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.ApproveAuditSkip(request, userId, facilitytimeZone);
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

        [Route("CloseAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> CloseAuditPlan(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.CloseAuditPlan(request, userId, facilitytimeZone);
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

        [Route("RejectCloseAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> RejectCloseAuditPlan(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.RejectCloseAuditPlan(request, userId, facilitytimeZone);
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

        [Route("ApproveClosedAuditPlan")]
        [HttpPost]
        public async Task<IActionResult> ApproveClosedAuditPlan(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.ApproveClosedAuditPlan(request, userId, facilitytimeZone);
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

        [Route("AuditLinkToPermit")]
        [HttpPut]
        public async Task<IActionResult> AuditLinkToPermit(int audit_id, int ptw_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.AuditLinkToPermit(audit_id, ptw_id, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("AssignAuditTask")]
        [HttpPut]
        public async Task<IActionResult> AssignAuditTask(int task_id, int assign_to)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditPlanBS.AssignAuditTask(task_id, assign_to, userID);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        
        [Route("CreateSubTaskForEvaluation")]
        [HttpPost]
        public async Task<IActionResult> CreateSubTaskForEvaluation(List<CMCreateAuditPlan> auditPlanList)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                int task_id = auditPlanList[0].task_id;
                var data = await _AuditPlanBS.CreateSubTaskForEvaluation(task_id, auditPlanList, userID);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
