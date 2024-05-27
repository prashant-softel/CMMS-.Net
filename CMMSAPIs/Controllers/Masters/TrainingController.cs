﻿using CMMSAPIs.BS.Masters;
using CMMSAPIs.Models.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private TrainingCourseBS TrainingCourseBS;
        public TrainingController(TrainingCourseBS cmms)
        {
            TrainingCourseBS = cmms;

        }
        [Route("CreateTrainingCourse")]
        [HttpPost]
        public async Task<IActionResult> CreateTrainingCourse(CMTrainingCourse request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.CreateTrainingCourse(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetCourseList")]
        [HttpGet]
        public async Task<IActionResult> GetCourseList(int facility_id, DateTime from_date, DateTime to_date)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.GetCourseList(facility_id, from_date, to_date);

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetCourseDetailById")]
        [HttpGet]
        public async Task<IActionResult> GetCourseDetailById(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.GetCourseDetailById(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateCourse")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCourse(CMTrainingCourse request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.UpdateCourseList(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DeleteCourse")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.DeleteCourseList(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("CreateScheduleCourse")]
        [HttpPost]
        public async Task<IActionResult> CreateScheduleCourse(TrainingSchedule request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.CreateScheduleCourse(request, userID);
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetScheduleCourse")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleCourse()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.GetScheduleCourse();
                return Ok(data);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //[Authorize]
        [Route("ExecuteScheduleCourse")]
        [HttpPost]
        public async Task<IActionResult> ExecuteScheduleCourse()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.ExecuteScheduleCourse();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Master Of training
        [Route("GetTrainingCategorty")]
        [HttpGet]

        public async Task<IActionResult> GetTrainingCategorty()
        {
            try
            {

                var data = await TrainingCourseBS.GetTrainingCategorty();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [Route("CreateTrainingCategorty")]
        [HttpPost]
        public async Task<IActionResult> CreateTrainingCategorty(CMTRAININGCATE request)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.CreateTrainingCategorty(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Route("UpdateTrainingCategorty")]
        [HttpPut]
        public async Task<IActionResult> UpdateTrainingCategorty(CMTRAININGCATE request)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.UpdateTrainingCategorty(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Route("DeleteTrainingCategorty")]
        public async Task<IActionResult> DeleteTrainingCategorty(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.DeleteTrainingCategorty(id, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }
        //Master Of Targeted Group
        [Route("GetTargetedGroup")]
        [HttpGet]

        public async Task<IActionResult> GetTargetedGroup()
        {
            try
            {

                var data = await TrainingCourseBS.GetTargetedGroupList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [Route("CreateTargetedGroup")]
        [HttpPost]
        public async Task<IActionResult> CreateTargetedGroup(CMTRAININGCATE request)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.CreateTargetedGroup(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Route("UpdateTargetedGroup")]
        [HttpPut]
        public async Task<IActionResult> UpdateTargetedGroup(CMTRAININGCATE request)
        {

            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.UpdateTargetedGroup(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Route("DeleteTargetedGroup")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTargetedGroup(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.DeleteTargetedGroup(id, userID);
                return Ok(data);
            }
            catch
            {
                throw;
            }
        }

    }
}
