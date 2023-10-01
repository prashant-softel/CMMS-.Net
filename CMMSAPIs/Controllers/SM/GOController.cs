using CMMSAPIs.BS;
using CMMSAPIs.Models;
using CMMSAPIs.Models.SM;
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

        /// <summary>
        /// This API will return 
        /// </summary>
        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid facility_id is sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Error while fetching data.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("ApproveGO")]
        [HttpPost]
        public async Task<IActionResult> ApproveGO(  CMApproval request )
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.ApproveGO(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("RejectGO")]
        [HttpPost]
        public async Task<IActionResult> RejectGO(  CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.RejectGO(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("GetGoodsOrderData")]
        [HttpGet]
        public async Task<IActionResult> GetGoodsOrderData(int facilityID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type)
        {
            try
            {
                var data = await _GOBS.GetGoodsOrderData(facilityID, empRole, fromDate, toDate, status, order_type);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("SubmitGoodsOrderData")]
        [HttpPost]
        public async Task<IActionResult> SubmitGoodsOrderData(CMSUBMITPURCHASEDATA request)
        {
            try
            {
                var data = await _GOBS.SubmitPurchaseData(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("GetSubmitGoodsOrderList")]
        [HttpGet]
        public async Task<IActionResult> GetSubmitGoodsOrderList(int facility_id, DateTime fromDate, DateTime toDate, int Status)
        {
            try
            {
                var data = await _GOBS.GetSubmitPurchaseOrderList(facility_id, fromDate, toDate, Status);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        // GOODS ORDER RECEIVE MODULE


        //[Authorize]
        [Route("UpdateGOReceive")]
        [HttpPost]
        public async Task<IActionResult> UpdateGOReceive(CMGoodsOrderList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.UpdateGOReceive(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("ApproveGOReceive")]
        [HttpPost]
        public async Task<IActionResult> ApproveGOReceive(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.ApproveGOReceive(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("RejectGOReceive")]
        [HttpPost]
        public async Task<IActionResult> RejectGOReceive(CMApproval request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.RejectGOReceive(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("CloseRO")]
        [HttpPost]
        public async Task<IActionResult> CloseRO(CMGoodsOrderList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _GOBS.CloseRO(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

    }
}
