using CMMSAPIs.BS.Masters;
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
        TrainingController(TrainingCourseBS cmms)
        {
            TrainingCourseBS = cmms;

        }
        [Route("CreateTrainingCourse")]
        [HttpPost]
        public async Task<IActionResult> CreateTrainingCourse()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.CreateTrainingCourse();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetCourseList")]
        [HttpGet]
        public async Task<IActionResult> GetCourseList()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.GetCourseList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateCourseList")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCourseList()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.UpdateCourseList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("DleteCourseList")]
        [HttpPatch]
        public async Task<IActionResult> DeleteCourseList()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await TrainingCourseBS.DeleteCourseList();
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
