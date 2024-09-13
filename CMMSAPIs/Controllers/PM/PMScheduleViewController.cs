using CMMSAPIs.BS.PM;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMScheduleViewController : ControllerBase
    {
        private readonly IPMScheduleViewBS _PMScheduleViewBS;

        public PMScheduleViewController(IPMScheduleViewBS pm_schedule_view)
        {
            _PMScheduleViewBS = pm_schedule_view;
        }

        //[Authorize]
        [Route("CancelPMTask")]
        [HttpPut]
        public async Task<IActionResult> CancelPMTask(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.CancelPMTask(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetPMTaskDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskDetail(int task_id, int facility_id)
        {

            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _PMScheduleViewBS.GetPMTaskDetail(task_id, facilitytimeZone);
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

        //[Authorize]
        [Route("GetPMTaskList")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskList(int facility_id, DateTime? start_date, DateTime? end_date, string frequencyIds, string categoryIds, bool self_view)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _PMScheduleViewBS.GetPMTaskList(facility_id, start_date, end_date, frequencyIds, categoryIds, userID, self_view, facilitytimeZone);
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

        //[Authorize]
        [Route("LinkPermitToPMTask")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToPMTask(int task_id, int permit_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.LinkPermitToPMTask(task_id, permit_id, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddCustomCheckpoint")]
        [HttpPost]
        public async Task<IActionResult> AddCustomCheckpoint(CMCustomCheckPoint request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.AddCustomCheckpoint(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("StartPMTask")]
        [HttpPost]
        public async Task<IActionResult> StartPMTask(int task_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.StartPMTask(task_id, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (FieldAccessException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdatePMTaskExecution")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePMTaskExecution(CMPMExecutionDetail request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.UpdatePMTaskExecution(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ClosePMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> ClosePMTaskExecution(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.ClosePMTaskExecution(request, userID, facilitytimeZone);
                //request.comment = "Approved";
                //var data2 = _PMScheduleViewBS.ApprovePMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("CancelApprovedPMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> CancelApprovedPMTaskExecution(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.CancelApprovedPMTaskExecution(request, userID, facilitytimeZone);
                //request.comment = "Approved";
                //var data2 = _PMScheduleViewBS.ApprovePMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("CancelRejectedPMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> CancelRejectedPMTaskExecution(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.CancelRejectedPMTaskExecution(request, userID, facilitytimeZone);
                //request.comment = "Approved";
                //var data2 = _PMScheduleViewBS.ApprovePMTaskExecution(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApprovePMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> ApprovePMTaskExecution(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.ApprovePMTaskExecution(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectPMTaskExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectPMTaskExecution(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.RejectPMTaskExecution(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("AssignPMTask")]
        [HttpPut]
        public async Task<IActionResult> AssignPMTask(int task_id, int assign_to, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.AssignPMTask(task_id, assign_to, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("UpdatePMScheduleExecution")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePMScheduleExecution(CMPMExecutionDetail request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.UpdatePMScheduleExecution(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetPMTaskScheduleDetail")]
        [HttpGet]
        public async Task<IActionResult> GetPMTaskScheduleDetail(int task_id, int schedule_id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.GetPMTaskScheduleDetail(task_id, schedule_id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("cloneSchedule")]
        [HttpPatch]
        public async Task<IActionResult> cloneSchedule(int facility_id, int task_id, int from_schedule_id, int to_schedule_id, int cloneJobs)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.cloneSchedule(facility_id, task_id, from_schedule_id, to_schedule_id, cloneJobs, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message })
                {
                    StatusCode = 200
                };
            }
        }

        [Route("getAssetListForClone")]
        [HttpPatch]
        public async Task<IActionResult> getAssetListForClone(int task_id, int schedule_id)
        {
            {
                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _PMScheduleViewBS.getAssetListForClone(task_id, schedule_id);
                    return Ok(data);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        [Route("GetScheduleData")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleData(int facility_id, int category_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _PMScheduleViewBS.GetScheduleData(facility_id, category_id, facilitytimeZone);
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

        [Route("SetScheduleData")]
        [HttpPost]
        public async Task<IActionResult> SetScheduleData(CMSetScheduleData request, int task_id, int schedule_id)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.SetScheduleData(request, userID, task_id, schedule_id, facilitytimeZone);
                return Ok(data);
            }/*
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }*/
            catch (Exception)
            {
                throw;
            }
        }

        [Route("DeletePMTask")]
        [HttpPost]
        public async Task<IActionResult> DeletePMTask(CMApproval request)
        {
            try
            {
                int facility_id = request.facility_id;
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PMScheduleViewBS.DeletePMTask(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
