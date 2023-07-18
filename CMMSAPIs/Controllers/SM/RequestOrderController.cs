using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS;
using CMMSAPIs.BS.SM;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Http;
using static System.Reflection.Metadata.BlobBuilder;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using CMMSAPIs.BS.Facility;

namespace CMMSAPIs.Controllers.SM
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestOrderController : Controller
    {
        private readonly IRequestOrderBS _IRequestOrderBS;
        public RequestOrderController(IRequestOrderBS RO)
        {
            _IRequestOrderBS = RO;
        }

        [Authorize]
        [Route("GetRequestOrderList")]
        [HttpGet]
        public async Task<IActionResult> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _IRequestOrderBS.GetRequestOrderList(facilityID, fromDate, toDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetRODetailsByID")]
        [HttpGet]
        public async Task<IActionResult> GetRODetailsByID(int requestID)
        {
            try
            {
                var data = await _IRequestOrderBS.GetRODetailsByID(requestID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Authorize]
        [Route("CreateRequestOrder")]
        [HttpPost]
        public async Task<IActionResult> CreateRequestOrder(CMRequestOrder request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.CreateRequestOrder(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateRequestOrder")]
        [HttpPost]
        public async Task<IActionResult> UpdateRequestOrder(CMRequestOrder request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.UpdateRequestOrder(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("DeleteRequestOrder")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequestOrder(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.DeleteRequestOrder(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApproveRequestOrder")]
        [HttpPost]
        public async Task<IActionResult> ApproveRequestOrder(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.ApproveRequestOrder(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectRequestOrder")]
        [HttpPost]
        public async Task<IActionResult> RejectGoodsOrder(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.RejectRequestOrder(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
