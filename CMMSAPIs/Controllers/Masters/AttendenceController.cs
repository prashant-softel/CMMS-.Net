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
        [Route("GetAttendanceList")]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceList(int facility_id, DateTime from_date, DateTime to_date)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.GetAttendanceList(facility_id, from_date, to_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateAttendance")]
        [HttpPatch]
        public async Task<IActionResult> UpdateAttendance(CMCreateAttendence requset)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _attendeceBS.UpdateAttendance(requset);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}