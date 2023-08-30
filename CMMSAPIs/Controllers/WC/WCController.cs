using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Controllers.WC
{
    [Route("api/[controller]")]
    [ApiController]
    public class WCController : ControllerBase
    {
        private readonly IWCBS _WCBS;
        public WCController(IWCBS wc)
        {
            _WCBS = wc;            
        }

        [Authorize]
        [Route("GetWCList")]
        [HttpGet]
        public async Task<IActionResult> GetWCList(int facilityId, string startDate, string endDate, int statusId)
        {
            try
            {
                var data = await _WCBS.GetWCList(facilityId, startDate, endDate, statusId);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateWC")]
        [HttpPost]
        public async Task<IActionResult> CreateWC(List<CMWCCreate> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.CreateWC(request,userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetWCDetails")]
        [HttpGet]
        public async Task<IActionResult> GetWCDetails(int wc_id)
        {
            try
            {
                var data = await _WCBS.GetWCDetails(wc_id);
                return Ok(data);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateWC")]
        [HttpPatch]
        public async Task<IActionResult> UpdateWC(CMWCCreate request)
        {
            try
            {
                var data = await _WCBS.UpdateWC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApproveWC")]
        [HttpPut]
        internal async Task<IActionResult> ApproveWC(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.ApproveWC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectWC")]
        [HttpPut]
        internal async Task<IActionResult> RejectWC(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _WCBS.RejectWC(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
