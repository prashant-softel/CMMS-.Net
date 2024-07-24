using CMMSAPIs.Cleaning;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Controllers.Vegetation
{
    [Route("api/[controller]")]
    [ApiController]
    public class VegetationController : Controller
    {
        private readonly vegetaion _vegetationBS;
        private readonly VegBS _vegetationBS1;


        public VegetationController(vegetaion vge)
        {
            _vegetationBS = vge;
            _vegetationBS.setModuleType(cleaningType.ModuleCleaning);

        }
        [Route("ApproveVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveVegetationExecution(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectEndExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectEndExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectVegetationExecution(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.RejectEndExecutionVegetation(request, userId);
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
                var data = await _vegetationBS.StartExecutionVegetation(executionId, userId);
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
                var data = await _vegetationBS.EndExecutionVegetation(executionId, userId);
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
                var data = await _vegetationBS.StartScheduleExecutionVegetation(scheduleId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("UpdateVegScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> UpdateVegScheduleExecution(CMMCGetScheduleExecution schedule)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.UpdateScheduleExecutionVegetation(schedule, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AbandonVegetationExecution")]
        [HttpPut]
        public async Task<IActionResult> AbandonVegetationExecution(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.AbandonExecutionVegetation(request, userId);
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
                var data = await _vegetationBS.AbandonScheduleVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("StartMCExecution")]
        [HttpPost]
        public async Task<IActionResult> StartMCExecution(int id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.StartExecutionVegetation(id, userId);
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
                var data = await _vegetationBS.CompleteExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMCExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCExecution(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.ApproveExecutionVegetation(request, userId);
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
                var data = await _vegetationBS.RejectExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [Route("EndVEGScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> EndVEGScheduleExecution(int scheduleId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS.EndScheduleExecutionVegetation(scheduleId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("RejectVegScheduleExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectVegScheduleExecution(ApproveMC request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _vegetationBS1.RejectScheduleExecutionVegetation(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
