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
        public async Task<IActionResult> GetRequestOrderList(int plantID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _IRequestOrderBS.GetRequestOrderList(plantID, fromDate, toDate);
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
        [Route("UpdateRO")]
        [HttpPost]
        public async Task<IActionResult> UpdateRO(CMRequestOrder request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.UpdateRO(request, userID);
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
        public async Task<IActionResult> DeleteRequestOrder([FromForm] int RO_ID)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IRequestOrderBS.DeleteRequestOrder(RO_ID, userID);
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
                var data = await _IRequestOrderBS.ApproveRequestOrder(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectGoodsOrder")]
        [HttpPost]
        public async Task<IActionResult> RejectGoodsOrder(CMApproval request)
        {
            try
            {
                var data = await _IRequestOrderBS.RejectGoodsOrder(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
