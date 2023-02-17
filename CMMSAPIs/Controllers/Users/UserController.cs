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
