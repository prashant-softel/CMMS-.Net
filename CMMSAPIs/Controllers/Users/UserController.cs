using CMMSAPIs.BS.Users;
using CMMSAPIs.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserAccessBS _userAccessBs;
        public UserController(IUserAccessBS userBs)
        {
            _userAccessBs = userBs;
        }

        //Get Signle User details
        [Route("GetUserDetail")]
        [HttpGet]

        public async Task<IActionResult> GetUserDetail(int user_id)
        {
            try
            {
                var data = await _userAccessBs.GetUserDetail(user_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // List Of Users
        [Route("GetUserList")]
        [HttpGet]

        public async Task<IActionResult> GetUserList(int facility_id)
        {
            try
            {
                var data = await _userAccessBs.GetUserList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Create User
        [Route("CreateUser")]
        [HttpPost]

        public async Task<IActionResult> CreateUser(CMUserDetail request)
        {
            try
            {
                var data = await _userAccessBs.CreateUser(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Update User
        [Route("UpdateUser")]
        [HttpPut]

        public async Task<IActionResult> UpdateUser(CMUserDetail request)
        {
            try
            {
                var data = await _userAccessBs.UpdateUser(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Delete User
        [Route("DeleteUser")]
        [HttpDelete]

        public async Task<IActionResult> DeleteUser(int user_id)
        {
            try
            {
                var data = await _userAccessBs.DeleteUser(user_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Get User By Notification id
        [Route("GetUserByNotificationId")]
        [HttpDelete]

        public async Task<IActionResult> GetUserByNotificationId(int notification_id, int facility_id)
        {
            try
            {
                var data = await _userAccessBs.GetUserByNotificationId(notification_id, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("GetUserAccess")]
        [HttpGet]

        public async Task<IActionResult> GetUserAccess(int user_id)
        {
            try
            {
                var data = await _userAccessBs.GetUserAccess(user_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("SetUserAccess")]
        [HttpPost]
        public async Task<IActionResult> SetUserAccess(CMUserAccess request)
        {
            try
            {
                var data = await _userAccessBs.SetUserAccess(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetUserNotifications")]
        [HttpGet]

        public async Task<IActionResult> GetUserNotifications(int user_id)
        {
            try
            {
                var data = await _userAccessBs.GetUserNotifications(user_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("SetUserNotifications")]
        [HttpPost]
        public async Task<IActionResult> SetUserNotifications(CMUserNotifications request)
        {
            try
            {
                var data = await _userAccessBs.SetUserNotifications(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
