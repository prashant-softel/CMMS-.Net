using CMMSAPIs.BS.Jobs;
using CMMSAPIs.Models.Jobs;
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
        public async Task<IActionResult> GetJobWorkTypeList()
        {
            try
            {
                var data = await _JobWorkTypeBS.GetJobWorkTypeList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateJobWorkType")]
        [HttpPost]
        public async Task<IActionResult> CreateJobWorkType(CMADDJobWorkType request)
        {
            try
            {
                var data = await _JobWorkTypeBS.CreateJobWorkType(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateJobWorkType")]
        [HttpPut]
        public async Task<IActionResult> UpdateJobWorkType(CMUpdateJobWorkType request)
        {
            try
            {
                var data = await _JobWorkTypeBS.UpdateJobWorkType(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
        public async Task<IActionResult> GetMasterToolList(int id)
        {
            try
            {
                var data = await _JobWorkTypeBS.GetMasterToolList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
