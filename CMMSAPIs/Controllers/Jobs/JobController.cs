﻿using CMMSAPIs.BS.Jobs;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //[Authorize]
        [Route("GetJobList")]
        [HttpGet]
        public async Task<IActionResult> GetJobList(string facility_id, string start_date, string end_date, CMMS.CMMS_JobType jobType, bool selfView, string status, string categoryid)
        {
            try
            {
                var facilityIds = facility_id.Split(',');

                // Use the first facility ID from the array and convert it to an integer
                int firstFacilityId = int.Parse(facilityIds.FirstOrDefault());

                // Get the time zone for the first facility ID
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo"))
                    .FirstOrDefault(x => x.facility_id == firstFacilityId)?.timezone;

                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.GetJobList(facility_id, start_date, end_date, jobType, selfView, userID, status, facilitytimeZone, categoryid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetJobListByPermitId")]
        [HttpGet]
        public async Task<IActionResult> GetJobListByPermitId(int permitId, int facility_id)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _JobBS.GetJobListByPermitId(permitId, facilitytimeZone);
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

        //[Authorize]
        [Route("GetJobDetails")]
        [HttpGet]
        public async Task<IActionResult> GetJobDetails(int job_id, int facility_id)
        {

            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _JobBS.GetJobDetails(job_id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateNewJob")]
        [HttpPost]
        public async Task<IActionResult> CreateNewJob(CMCreateJob request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.CreateNewJob(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateJob")]
        [HttpPatch]
        public async Task<IActionResult> UpdateJob(CMCreateJob request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.UpdateJob(request, userID);
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
        //[Authorize]
        [Route("ReAssignJob")]
        [HttpPut]
        public async Task<IActionResult> ReAssignJob(int job_id, int assignedTo)
        {

            try
            {
                int updatedBy = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.ReAssignJob(job_id, assignedTo, updatedBy);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //[Authorize]
        [Route("CancelJob")]
        [HttpPut]
        public async Task<IActionResult> CancelJob(int job_id, string Cancelremark)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.CancelJob(job_id, userID, Cancelremark);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*
        //[Authorize]
        [Route("DeleteJob")]
        [HttpDelete]
        public async Task<IActionResult> DeleteJob(int job_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.DeleteJob(job_id, userID);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /**/
        //[Authorize]
        [Route("LinkToPTW")]
        [HttpPut]
        public async Task<IActionResult> LinkToPTW(int job_id, int ptw_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _JobBS.LinkToPTW(job_id, ptw_id, userID);
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
