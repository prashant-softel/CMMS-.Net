using CMMSAPIs.BS;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [Route("GetGOList")]
        [HttpGet]
        public async Task<IActionResult> GetGOList(int plantID, DateTime fromDate, DateTime toDate, int Status)
        {
            try
            {
                var data = await _GOBS.GetGOList(plantID, fromDate, toDate, Status);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [Route("UpdateRO")]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [Route("GOApproval")]
        [HttpPost]
        public async Task<IActionResult> GOApproval(CMApproval request)
        {
            try
            {
                var data = await _GOBS.GOApproval(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectGO")]
        [HttpPost]
        public async Task<IActionResult> RejectGO(CMApproval request)
        {
            try
            {
                var data = await _GOBS.RejectGO(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetPurchaseData")]
        [HttpGet]
        public async Task<IActionResult> GetPurchaseData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type)
        {
            try
            {
                var data = await _GOBS.GetPurchaseData(plantID, empRole, fromDate, toDate, status, order_type);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetPurchaseDetailsByID")]
        [HttpGet]
        public async Task<IActionResult> getPurchaseDetailsByID(int id)
        {
            try
            {
                var data = await _GOBS.getPurchaseDetailsByID(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("SubmitPurchaseOrderData")]
        [HttpPost]
        public async Task<IActionResult> SubmitPurchaseData(CMSUBMITPURCHASEDATA request)
        {
            try
            {
                var data = await _GOBS.SubmitPurchaseData(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
