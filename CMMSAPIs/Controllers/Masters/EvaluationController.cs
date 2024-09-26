using CMMSAPIs.BS.MISEvaluation;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Evaluation
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IMISEvaluationBS _IMISEvaluationBS;
        public EvaluationController(IMISEvaluationBS cmms)
        {
            _IMISEvaluationBS = cmms;
        }

        [Route("GetEvaluationList")]
        [HttpGet]
        public async Task<IActionResult> GetEvaluationList(string facility_id, DateTime fromDate, DateTime toDate, int selfView)
        {
            try
            {
                var facilityIds = facility_id.Split(',');

                // Use the first facility ID from the array and convert it to an integer
                int firstFacilityId = int.Parse(facilityIds.FirstOrDefault());

                // Get the time zone for the first facility ID
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo"))
                    .FirstOrDefault(x => x.facility_id == firstFacilityId)?.timezone;

                var data = await _IMISEvaluationBS.GetEvaluationList(facility_id, fromDate, toDate, selfView, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetEvaluationDetails")]
        [HttpGet]
        public async Task<IActionResult> GetEvaluationDetails(int id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _IMISEvaluationBS.GetEvaluationDetails(id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("CreateEvaluationPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateEvaluationPlan(CMEvaluationCreate request)
            {
                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _IMISEvaluationBS.CreateEvaluationPlan(request, userID);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    ExceptionResponse data = new ExceptionResponse();
                    data.Status = 200;
                    data.Message = ex.Message;
                    return Ok(data);
                }
            }

            [Route("ApproveEvaluationPlan")]
            [HttpPatch]
            public async Task<IActionResult> ApproveEvaluationPlan(CMApproval request)
            {
                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _IMISEvaluationBS.ApproveEvaluationPlan(request, userID);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            [Route("RejectEvaluationPlan")]
            [HttpPatch]
            public async Task<IActionResult> RejectEvaluationPlan(CMApproval request)
            {
                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _IMISEvaluationBS.RejectEvaluationPlan(request, userID);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    ExceptionResponse data = new ExceptionResponse();
                    data.Status = 200;
                    data.Message = ex.Message;
                    return Ok(data);
                }
            }
            [Route("UpdateEvaluationPlan")]
            [HttpPost]
            public async Task<IActionResult> UpdateEvaluationPlan(List<CMEvaluationUpdate> request)
            {
                try
                {
                int facilityId = request.FirstOrDefault()?.facility_id ?? 0;
                    //FirstOrDefault()?.facilityId ?? 0;
                if (facilityId == 0)
                {
                    return BadRequest("Invalid facility ID.");
                }

                // Get the facility timezone based on the extracted facilityId
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo"))
                    .FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                if (facilitytimeZone == null)
                {
                    return NotFound("Facility timezone not found.");
                }
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISEvaluationBS.UpdateEvaluationPlan(request, userID, facilitytimeZone);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    ExceptionResponse data = new ExceptionResponse();
                    data.Status = 200;
                    data.Message = ex.Message;
                    return Ok(data);
                }
            }

            [Route("DeleteEvaluationPlan")]
            [HttpPut]
            public async Task<IActionResult> DeleteEvaluationPlan(int id)
            {
                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _IMISEvaluationBS.DeleteEvaluationPlan(id, userID);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    ExceptionResponse data = new ExceptionResponse();
                    data.Status = 200;
                    data.Message = ex.Message;
                    return Ok(data);
                }
            }
            
        }
    }

