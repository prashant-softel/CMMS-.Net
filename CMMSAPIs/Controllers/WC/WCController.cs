using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Models.WC;

namespace CMMSAPIs.Controllers.WC
{
    public class WCController : ControllerBase
    {
        private readonly IWCBS _WCBS;
        public WCController(IWCBS wc)
        {
            _WCBS = wc;            
        }

        [Route("GetWCList")]
        [HttpGet]
        public async Task<IActionResult> GetWCList(int facility_id)
        {
            try
            {
                var data = await _WCBS.GetWCList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateWC")]
        [HttpPost]
        public async Task<IActionResult> CreateWC(CMWCCreate request)
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

        [Route("ViewWC")]
        [HttpGet]
        public async Task<IActionResult> ViewWC(int wc_id)
        {
            try
            {
                var data = await _WCBS.ViewWC(wc_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
