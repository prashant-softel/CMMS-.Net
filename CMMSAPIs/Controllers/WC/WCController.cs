using Microsoft.AspNetCore.Mvc;
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
            catch (Exception ex)
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
                var data = await _WCBS.CreateWC(request);
                return Ok(data);
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<IActionResult> UpdateWC(CMWCCreate request)
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

        internal async Task<IActionResult> ApproveWC(CMApproval request)
        {
            try
            {
                var data = await _WCBS.ApproveWC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<IActionResult> RejectWC(CMApproval request)
        {
            try
            {
                var data = await _WCBS.RejectWC(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
