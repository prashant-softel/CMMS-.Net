using CMMSAPIs.BS.Masters;
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
        public async Task<IActionResult> GetCourseList(int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.GetCourseList(facility_id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateCourseList")]
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
        [Route("DleteCourseList")]
        [HttpPatch]
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
        public async Task<IActionResult> CreateScheduleCourse()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.CreateScheduleCourse();
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

    }
}
