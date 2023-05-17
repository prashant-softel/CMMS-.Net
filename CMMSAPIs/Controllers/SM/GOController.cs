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
        public async Task<IActionResult> GetGOList(int plantID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _GOBS.GetGOList(plantID, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("GetGOItemByID")]
        [HttpGet]
        public async Task<IActionResult> GetGOItemByID(int id)
        {
            try
            {
                var data = await _GOBS.GetGOItemByID(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetAssetCodeDetails")]
        [HttpGet]
        public async Task<IActionResult> GetAssetCodeDetails(int asset_code, int plantID, DateTime fromDate, DateTime toDate)
        {
            //int plantID, DateTime fromDate, DateTime toDate
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
        [HttpPost]
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
        [HttpPost]
        public async Task<IActionResult> DeleteGO([FromForm] int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.DeleteGO(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("WithdrawGO")]
        [HttpPost]
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

        [Route("GOApproval")]
        [HttpPost]
        public async Task<IActionResult> GOApproval(CMGO request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.GOApproval(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
