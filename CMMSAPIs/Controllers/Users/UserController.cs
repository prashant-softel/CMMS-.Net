using CMMSAPIs.BS.Users;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [AllowAnonymous]
        [Route("Authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromForm] CMUserCrentials credential)
        {
            try
            {
                var data = await _userAccessBs.Authenticate(credential);
                if (data == null)
                    return Unauthorized();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
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

        //[Authorize]
        [Route("ImportUsers")]
        [HttpPost]
        public async Task<IActionResult> ImportUsers(int file_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.ImportUsers(file_id, userID);
                return Ok(data);
            }/*
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;
            }
        }

        // Create User
        //[Authorize]
        [Route("CreateUser")]
        [HttpPost]

        public async Task<IActionResult> CreateUser(List<CMCreateUser> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.CreateUser(request, userID);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Update User
        //[Authorize]
        [Route("UpdateUser")]
        [HttpPatch]

        public async Task<IActionResult> UpdateUser(CMUpdateUser request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.UpdateUser(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Delete User
        //[Authorize]
        [Route("DeleteUser")]
        [HttpDelete]

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.DeleteUser(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Get User By Notification id
        [Route("GetUserByNotificationId")]
        [HttpGet]

        public async Task<IActionResult> GetUserByNotificationId(CMUserByNotificationId request)
        {
            try
            {
                var data = await _userAccessBs.GetUserByNotificationId(request);
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

        //[Authorize]
        [Route("SetUserAccess")]
        [HttpPost]
        public async Task<IActionResult> SetUserAccess(CMUserAccess request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.SetUserAccess(request, userID);
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

        //[Authorize]
        [Route("SetUserNotifications")]
        [HttpPost]
        public async Task<IActionResult> SetUserNotifications(CMUserNotifications request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.SetUserNotifications(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetCompetencyList")]
        [HttpGet]
        public async Task<IActionResult> GetCompetencyList()
        {
            try
            {
                var data = await _userAccessBs.GetCompetencyList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddCompetency")]
        [HttpPost]
        public async Task<IActionResult> AddCompetency(CMCompetency request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.AddCompetency(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateCompetency")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCompetency(CMCompetency request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _userAccessBs.UpdateCompetency(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("DeleteCompetency")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCompetency(int id)
        {
            try
            {
                var data = await _userAccessBs.DeleteCompetency(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
