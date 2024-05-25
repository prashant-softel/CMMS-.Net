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
        [Route("UpdateAttendance")]
        [HttpPatch]
        public async Task<IActionResult> UpdateAttendance(CMCreateAttendence requests)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.UpdateAttendance(requests, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
