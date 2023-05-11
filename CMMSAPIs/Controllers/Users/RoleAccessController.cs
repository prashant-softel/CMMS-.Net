using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.Users;
using CMMSAPIs.Models.Users;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace CMMSAPIs.Controllers.Users
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAccessController : ControllerBase
    {
        private readonly IRoleAccessBS _roleAcceesBs;
        public RoleAccessController(IRoleAccessBS roleAcceesBs)
        {
            _roleAcceesBs = roleAcceesBs;
        }

        [Route("GetRoleList")]
        [HttpGet]
        public async Task<IActionResult> GetRoleList()
        {
            try
            {
                var data = await _roleAcceesBs.GetRoleList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetRoleAccess")]
        [HttpGet]
        
        public async Task<IActionResult> GetRoleAccess(int role_id)
        {
            try
            {
                var data = await _roleAcceesBs.GetRoleAccess(role_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("SetRoleAccess")]
        [HttpPost]
        public async Task<IActionResult> SetRoleAccess(CMSetRoleAccess request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _roleAcceesBs.SetRoleAccess(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetRoleNotifications")]
        [HttpGet]
        public async Task<IActionResult> GetRoleNotifications(int role_id)
        {
            try
            {
                var data = await _roleAcceesBs.GetRoleNotifications(role_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("SetRoleNotifications")]
        [HttpPost]
        public async Task<IActionResult> SetRoleNotifications(CMSetRoleNotifications request)
        {
            try
            {
                var data = await _roleAcceesBs.SetRoleNotifications(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }


}
