using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.Users;
using CMMSAPIs.Models.Users;
using System.Collections.Generic;

namespace CMMSAPIs.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAccessController : ControllerBase
    {
        private readonly IRoleAccessBS _roleAcceesBs;
        public RoleAccessController(IRoleAccessBS roleAcceesBs)
        {
            _roleAcceesBs = roleAcceesBs;
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

        [Route("SetRoleAccess")]
        [HttpPost]
        public async Task<IActionResult> SetRoleAccess(CMRoleAccess request)
        {
            try
            {
                var data = await _roleAcceesBs.SetRoleAccess(request);
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
        public async Task<IActionResult> SetRoleNotifications(List<CMRoleNotifications> request)
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

        //[Route("SetRoleAccessAll")]
        //[HttpPost]
        //public async Task<IActionResult> SetRoleAccessAll(CMRoleAccess request)
        //{
        //    try
        //    {
        //        var data = await _roleAcceesBs.SetRoleAccessAll(request);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }


}
