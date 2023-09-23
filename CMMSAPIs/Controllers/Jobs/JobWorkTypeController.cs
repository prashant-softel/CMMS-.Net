using CMMSAPIs.BS.Jobs;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CMMSAPIs.Controllers.Jobs
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobWorkTypeController : ControllerBase
    {
        private readonly IJobWorkTypeBS _JobWorkTypeBS;
        public JobWorkTypeController(IJobWorkTypeBS jobWorkTypeBS)
        {
            _JobWorkTypeBS = jobWorkTypeBS;
        }


        /*
         * WorkType Crud Operation
        */
        [Route("GetJobWorkTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetJobWorkTypeList(string categoryIds = "")
        {
            try
            {
                var data = await _JobWorkTypeBS.GetJobWorkTypeList(categoryIds);
                return Ok(data);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateJobWorkType")]
        [HttpPost]
        public async Task<IActionResult> CreateJobWorkType(CMADDJobWorkType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobWorkTypeBS.CreateJobWorkType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateJobWorkType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateJobWorkType(CMADDJobWorkType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobWorkTypeBS.UpdateJobWorkType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteJobWorkType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteJobWorkType(int id)
        {
            try
            {
                var data = await _JobWorkTypeBS.DeleteJobWorkType(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Associated Tool to Work Type Crud Operation
        */

        [Route("GetJobWorkTypeToolList")]
        [HttpGet]
        public async Task<IActionResult> GetJobWorkTypeToolList(int jobId)
        {
            try
            {
                var data = await _JobWorkTypeBS.GetJobWorkTypeToolList(jobId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Master Tool List of Required by Work Type
        */

        [Route("GetMasterToolList")]
        [HttpGet]
        public async Task<IActionResult> GetMasterToolList(string worktypeIds)
        {
            try
            {
                var data = await _JobWorkTypeBS.GetMasterToolList(worktypeIds);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateMasterTool")]
        [HttpPost]
        public async Task<IActionResult> CreateMasterTool(string name)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobWorkTypeBS.CreateMasterTool(name, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateMasterTool")]
        [HttpPatch]
        public async Task<IActionResult> UpdateMasterTool(CMDefaultList tool)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobWorkTypeBS.UpdateMasterTool(tool, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteMasterTool")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMasterTool(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobWorkTypeBS.DeleteMasterTool(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateJobWorkTypeTool")]
        [HttpPost]
        public async Task<IActionResult> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request)
        {
            try
            {
                var data = await _JobWorkTypeBS.CreateJobWorkTypeTool(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateJobWorkTypeTool")]
        [HttpPut]
        public async Task<IActionResult> UpdateJobWorkTypeTool(CMUpdateJobWorkTypeTool request)
        {
            try
            {
                var data = await _JobWorkTypeBS.UpdateJobWorkTypeTool(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteJobWorkTypeTool")]
        [HttpDelete]
        public async Task<IActionResult> DeleteJobWorkTypeTool(int id)
        {
            try
            {
                var data = await _JobWorkTypeBS.DeleteJobWorkTypeTool(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
