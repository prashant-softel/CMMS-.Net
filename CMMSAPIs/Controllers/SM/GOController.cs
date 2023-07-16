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

 
        [Route("GetGOList")]
        [HttpGet]
        public async Task<IActionResult> GetGOList(int facility_id, DateTime fromDate, DateTime toDate, int Status)
        {
            try
            {
                var data = await _GOBS.GetGOList(facility_id, fromDate, toDate, Status);
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
        public async Task<IActionResult> CreateGO(CMGoodsOrderList request)
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
        public async Task<IActionResult> UpdateGO(CMGoodsOrderList request)
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
        public async Task<IActionResult> DeleteGO(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.DeleteGO(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


 
        [Route("CloseGO")]
        [HttpPost]
        public async Task<IActionResult> CloseGO(CMGoodsOrderList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.CloseGO(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


 
        [Route("GOApproval")]
        [HttpPost]
        public async Task<IActionResult> GOApproval([FromForm]  CMApproval request )
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.GOApproval(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


 
        [Route("RejectGO")]
        [HttpPost]
        public async Task<IActionResult> RejectGO([FromForm]  CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.RejectGO(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


 
        [Route("GetPurchaseData")]
        [HttpGet]
        public async Task<IActionResult> GetPurchaseData(int facilityID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type)
        {
            try
            {
                var data = await _GOBS.GetPurchaseData(facilityID, empRole, fromDate, toDate, status, order_type);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


 
        [Route("GetGODetailsByID")]
        [HttpGet]
        public async Task<IActionResult> GetGODetailsByID(int id)
        {
            try
            {
                var data = await _GOBS.GetGODetailsByID(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


 
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
