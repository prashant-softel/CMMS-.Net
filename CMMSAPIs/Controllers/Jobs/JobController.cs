﻿using CMMSAPIs.BS.Jobs;
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
    public class JobController : ControllerBase
    {
        private readonly IJobBS _JobBS;
        private readonly IJobWorkTypeBS _JobWorkTypeBS;
        public JobController(IJobBS job, IJobWorkTypeBS jobWorkTypeBS)
        {
            _JobBS = job;
            _JobWorkTypeBS = jobWorkTypeBS;
        }

        [Authorize]
        [Route("GetJobList")]
        [HttpGet]
        public async Task<IActionResult> GetJobList(int facility_id, int userId)
        {
            try
            {
                var data = await _JobBS.GetJobList(facility_id, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetJobDetail")]
        [HttpGet]
        public async Task<IActionResult> GetJobDetails(int job_id)
        {
            try
            {
                var data = await _JobBS.GetJobDetails(job_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateNewJob")]
        [HttpPost]
        public async Task<IActionResult> CreateNewJob(CMCreateJob request)
        {
            String status;
            try
            {
                var data = await _JobBS.CreateNewJob(request);
                status = "Job Created Successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok(status);
        }

        /*
         * WorkType Crud Operation
        */
        [Authorize]
        [Route("ReAssignJob")]
        [HttpPut]
        public async Task<IActionResult> ReAssignJob(int job_id, int assignedTo)
        {
        
            try
            {
                var data = await _JobBS.ReAssignJob(job_id, assignedTo);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [Authorize]
        [Route("CancelJob")]
        [HttpPut]
        public async Task<IActionResult> CancelJob(int job_id, int user_id, string Cancelremark)
        {
            try
            {
                var data = await _JobBS.CancelJob(job_id, user_id, Cancelremark);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("LinkToPTW")]
        [HttpPut]
        public async Task<IActionResult> LinkToPTW(int job_id, int ptw_id)
        {
            try
            {
                var data = await _JobBS.LinkToPTW(job_id, ptw_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * WorkType Crud Operation
        */
       /* [Route("GetJobWorkTypeList")]
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
        public async Task<IActionResult> CreateJobWorkType(CMJobWorkType request)
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

        [Route("UpdateWorkType")]
        [HttpPut]
        public async Task<IActionResult> UpdateWorkType(CMJobWorkType request)
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

        [Route("DeleteWorkType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWorkType(int id)
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

        *//*
         * Associated Tool to Work Type Crud Operation
        *//*

        [Route("GetJobWorkTypeToolList")]
        [HttpGet]
        public async Task<IActionResult> GetJobWorkTypeToolList()
        {
            try
            {
                var data = await _JobWorkTypeBS.GetJobWorkTypeToolList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        *//*
         * Master Tool List of Required by Work Type
        *//*

        [Route("GetMasterToolList")]
        [HttpDelete]
        public async Task<IActionResult> GetMasterToolList()
        {
            try
            {
                var data = await _JobWorkTypeBS.GetMasterToolList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateJobWorkTypeTool")]
        [HttpPost]
        public async Task<IActionResult> CreateJobWorkTypeTool()
        {
            try
            {
                var data = await _JobWorkTypeBS.CreateJobWorkTypeTool();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateJobWorkTypeTool")]
        [HttpPut]
        public async Task<IActionResult> UpdateJobWorkTypeTool()
        {
            try
            {
                var data = await _JobWorkTypeBS.UpdateJobWorkTypeTool();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteJobWorkTypeTool")]
        [HttpDelete]
        public async Task<IActionResult> DeleteJobWorkTypeTool()
        {
            try
            {
                var data = await _JobWorkTypeBS.DeleteJobWorkTypeTool();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
*/
    }
}
