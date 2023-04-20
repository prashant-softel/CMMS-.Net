using CMMSAPIs.BS;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GOController : ControllerBase
    {
        private readonly IGOBS _GOBS;
        public GOController(IGOBS GO)
        {
            _GOBS = GO;
        }

        [Route("GetGOList")]
        [HttpGet]
        public async Task<IActionResult> GetGOList()
        {
            try
            {
                var data = await _GOBS.GetGOList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetAssetCodeDetails")]
        [HttpGet]
        public async Task<IActionResult> GetAssetCodeDetails(int asset_code)
        {
            try
            {
                var data = await _GOBS.GetAssetCodeDetails(asset_code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateGO")]
        [HttpPost]
        public async Task<IActionResult> CreateGO(CMGO request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.CreateGO(request,  userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateGO")]
        [HttpPut]
        public async Task<IActionResult> UpdateGO(CMGO request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.UpdateGO(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteGO")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGO(int GOid)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.DeleteGO(GOid, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("WithdrawGO")]
        [HttpDelete]
        public async Task<IActionResult> WithdrawGO(CMGO request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.WithdrawGO(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
