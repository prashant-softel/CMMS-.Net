using CMMSAPIs.BS;
using CMMSAPIs.Models;
using CMMSAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobBS _JobBS;
        public JobController(IJobBS job)
        {
            _JobBS = job;
        }

        [Route("GetJobList")]
        [HttpGet]
        public async Task<IActionResult> GetJobList(int facility_id)
        {
            try
            {
                var data = await _JobBS.GetJobList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetJobDetail")]
        [HttpGet]
        public async Task<IActionResult> GetJobDetail(int job_id)
        {
            try
            {
                var data = await _JobBS.GetJobList(job_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateNewJob")]
        [HttpPost]
        public async Task<IActionResult> CreateNewJob()
        {
            try
            {
                var data = await _JobBS.CreateNewJob();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateJob")]
        [HttpPost]
        public async Task<IActionResult> UpdateJob()
        {
            try
            {
                var data = await _JobBS.UpdateJob();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
