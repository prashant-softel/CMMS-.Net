using CMMSAPIs.Cleaning;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Controllers.Vegetation
{
    [Route("api/[controller]")]
    [ApiController]
    public class VegetationController : Controller
    {
        private readonly vegetaion _vegetationBS;


        public VegetationController(vegetaion vge)
        {
            _vegetationBS = vge;
            _vegetationBS.setModuleType(cleaningType.ModuleCleaning);

        }
        /*
                [Route("GetVegetationPlanList")]
                [HttpGet]
                public async Task<IActionResult> GetVegetationPlanList(int facilityId)
                {
                    try
                    {
                        var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                        var data = await _vegetationBS.GetPlanList(facilityId, facilitytimeZone);
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                [Route("GetVegetationTaskList")]
                [HttpGet]
                public async Task<IActionResult> GetVegetationTaskList(int facilityId)
                {
                    try
                    {
                        var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                        var data = await _vegetationBS.GetTaskList(facilityId, facilitytimeZone);
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                [Route("CreateVegetationPlan")]
                [HttpPost]
                public async Task<IActionResult> CreateVegetationPlan(List<CMMCPlan> request)
                {
                    try
                    {
                        int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                        var data = await _vegetationBS.CreatePlan(request, userId);
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }*/

        [Route("UpdateVegetationPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdateVegetationPlan(List<CMMCPlan> request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.UpdatePlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*
                [Route("GetVegetationPlanDetails")]
                [HttpGet]
                public async Task<IActionResult> GetVegetationPlanDetails(int planId, int facility_id)
                {
                    try
                    {

                        var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                        var data = await _vegetationBS.GetPlanDetails(planId, facilitytimeZone);
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                [Route("ApproveVegetationPlan")]
                [HttpPut]
                public async Task<IActionResult> ApproveVegetationPlan(CMApproval request)
                {
                    try
                    {
                        int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                        var data = await _vegetationBS.ApprovePlan(request, userId);
                        return Ok(data);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }*/
        [Route("ApproveVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveVegetationExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectVegetationPlan(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectPlan(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectVegetationExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteVegetationPlan")]
        [HttpPut]
        public async Task<IActionResult> DeleteVegetationPlan(int planId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.DeletePlan(planId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("LinkPermitToVegetation")]
        [HttpPut]
        public async Task<IActionResult> LinkPermitToVegetation(int task_id, int permit_id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.LinkPermitToVegetation(task_id, permit_id, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("StartVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> StartVegetationExecution(int executionId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecution(executionId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("EndVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> EndVegetationExecution(int executionId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.EndExecution(executionId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartVegScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> StartVegScheduleExecution(int scheduleId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartScheduleExecution(scheduleId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*  [Route("UpdateVegScheduleExecution")]
          [HttpPut]
          public async Task<IActionResult> UpdateVegScheduleExecution(CMMCGetScheduleExecution schedule)
          {
              try
              {
                  int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                  var data = await _vegetationBS.UpdateScheduleExecution(schedule, userId);
                  return Ok(data);
              }
              catch (Exception ex)
              {
                  throw;
              }
          }*/

        /*   [Route("GetVegExecutionDetails")]
           [HttpGet]
           public async Task<IActionResult> GetVegExecutionDetails(int executionId, int facility_id)
           {
               try
               {
                   var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                   var data = await _vegetationBS.GetExecutionDetails(executionId, facilitytimeZone);
                   return Ok(data);
               }
               catch (Exception ex)
               {
                   throw;
               }
           }*/

        [Route("AbandonVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> AbandonVegetationExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("AbandonVegetationSchedule")]
        [HttpPut]
        public async Task<IActionResult> AbandonVegetationSchedule(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonSchedule(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* [Route("GetVegEquipmentList")]
         [HttpGet]
         public async Task<IActionResult> GetVegEquipmentList(int facilityId)
         {
             try
             {
                 var data = await _vegetationBS.GetVegEquipmentList(facilityId);
                 return Ok(data);
             }
             catch (Exception ex)
             {
                 throw;
             }
         }
         [Route("GetVegTaskExecutionEquipmentList")]
         [HttpGet]
         public async Task<IActionResult> GetVegTaskExecutionEquipmentList(int taskId, int facility_id)
         {
             try
             {
                 var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                 var data = await _vegetationBS.GetTaskEquipmentList(taskId, facilitytimeZone);
                 return Ok(data);
             }
             catch (Exception ex)
             {
                 throw;
             }
         }*/
        [Route("StartMCExecution")]
        [HttpPost]
        public async Task<IActionResult> StartMCExecution(int id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecution(id, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("CompleteMCExecution")]
        [HttpPost]
        public async Task<IActionResult> CompleteMCExecution(CMMCExecution request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.CompleteExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMCExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectMCExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectMCExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectExecution(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
