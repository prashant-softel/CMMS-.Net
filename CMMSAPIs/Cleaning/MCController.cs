using CMMSAPIs.BS.Cleaning;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Controllers.MC
{

    [Route("api/[controller]")]
    [ApiController]
    public class MCController : ControllerBase
    {
        private readonly IMCVCBS _CleaningBS;
        public MCController(IMCVCBS Cleaning)
        {
            _CleaningBS = Cleaning;
            _CleaningBS.setModuleType(cleaningType.ModuleCleaning);
        }

        [Route("GetMCPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetMCPlanList(int facilityId, string startDate, string endDate)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _CleaningBS.GetPlanList(facilityId, facilitytimeZone,  startDate,  endDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetMCTaskList")]
        [HttpGet]

        public async Task<IActionResult> GetMCTaskList(int facility_Id, string startDate, string endDate)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_Id)?.timezone;
                var data = await _CleaningBS.GetTaskList(facility_Id, facilitytimeZone,  startDate,  endDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetMCPlanDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMCPlanDetails(int planId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CleaningBS.GetPlanDetails(planId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*[Route("GetMCPlanDetailsSummary")]
        [HttpGet]
        public async Task<IActionResult> GetMCPlanDetailsSummary(int planId, CMMCPlanSummary request)
        {
            try
            {
                var data = await _CleaningBS.GetPlanDetailsSummary(planId, request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        //[Authorize]
        [Route("CreateMCPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateMCPlan(List<CMMCPlan> request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.CreatePlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("UpdateMCPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdateMCPlan(List<CMMCPlan> request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.UpdatePlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApproveMCPlan")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCPlan(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ApprovePlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveEndExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveEndExecution(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ApproveEndExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ReAssignMcTask")]
        [HttpPut]
        public async Task<IActionResult> ReAssignMcTask(int task_id, int assign_to, int facility_id)
        {

            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ReAssignTask(task_id, assign_to, userID, facilitytimeZone);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [Route("RejectEndExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectEndExecution(ApproveMC request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.RejectEndExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveScheduleExecution(ApproveMC request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ApproveScheduleExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectScheduleExecution(ApproveMC request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.RejectScheduleExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectMCPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectMCPlan(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.RejectPlan(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("DeleteMCPlan")]
        [HttpPut]
        public async Task<IActionResult> DeleteMCPlan(int planId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.DeletePlan(planId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("LinkPermitToModuleCleaning")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToModuleCleaning(int scheduleId, int permit_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.LinkPermitToMCVC(scheduleId, permit_id, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("StartMCExecution")]
        [HttpPut]
        public async Task<IActionResult> StartMCExecution(int executionId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.StartExecution(executionId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("StartMCScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> StartMCScheduleExecution(int scheduleId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.StartScheduleExecution(scheduleId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateMCScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> UpdateMCScheduleExecution(CMMCGetScheduleExecution schedule, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.UpdateScheduleExecution(schedule, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
     /*   [Route("GetMCScheduleExecutionSummary")]
        [HttpPut]
        public async Task<IActionResult> GetMCScheduleExecutionSummary(CMMCGetScheduleExecution schedule, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CleaningBS.GetScheduleExecutionSummary(schedule, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
*/
        [Route("GetMCExecutionDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMCExecutionDetails(int executionId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CleaningBS.GetExecutionDetails(executionId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 200;
                item.Message = "No Data Found For This executionId";
                return Ok(item);

            }
        }

        [Route("GetMCScheduleDetails")]
        [HttpGet]
        public async Task<IActionResult> GetMCScheduleDetails(int scheduleId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _CleaningBS.GetScheduleDetails(scheduleId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 200;
                item.Message = "No Data Found For This executionId";
                return Ok(item);

            }
        }

        //[Authorize]
        [Route("AbandonMcExecution")]
        [HttpPut]
        public async Task<IActionResult> AbandonMcExecution(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.AbandonExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectAbandonExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectAbandonExecution(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.RejectAbandonExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("ApproveAbandonExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveAbandonExecution(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ApproveAbandonExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AbandonMcSchedule")]
        [HttpPut]
        public async Task<IActionResult> AbandonMcSchedule(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.AbandonSchedule(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetMCEquipmentList")]
        [HttpGet]
        public async Task<IActionResult> GetMCEquipmentList(int facility_Id)
        {
            try
            {
                var data = await _CleaningBS.GetEquipmentList(facility_Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetMCTaskEquipmentList")]
        [HttpGet]
        public async Task<IActionResult> GetMCTaskEquipmentList(int taskId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _CleaningBS.GetTaskEquipmentList(taskId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMCExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCExecution(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.ApproveExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectMCPExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectMCPExecution(CMApproval request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.RejectExecution(request, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("EndMCScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> EndMCScheduleExecution(int scheduleId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.EndScheduleExecution(scheduleId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("EndMCExecution")]
        [HttpPut]
        public async Task<IActionResult> EndMCExecution(int executionId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CleaningBS.EndExecution(executionId, userId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
