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
    public class AttendenceController : ControllerBase
    {
        private readonly AttendeceBS _attendeceBS;
        public AttendenceController(AttendeceBS cmms)
        {
            _attendeceBS = cmms;
        }
        // For Attendance
        [Route("CreateAttendance")]
        [HttpPost]
        public async Task<IActionResult> CreateAttendance(CMCreateAttendence requset)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.CreateAttendance(requset, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateAttendance")]
        [HttpPost]
        public async Task<IActionResult> UpdateAttendance(CMCreateAttendence requset)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.UpdateAttendance(requset, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetAttendanceList")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceList(int facility_id, int year)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.GetAttendanceList(facility_id, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetAttendanceByDetails")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceByDetails(int facility_id, DateTime date)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.GetAttendanceByDetails(facility_id, date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetAttendanceByDetailsByMonth")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceByDetailsByMonth(int facility_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.GetAttendanceByDetailsByMonth(facility_id, start_date, end_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
