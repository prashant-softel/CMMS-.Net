using CMMSAPIs.BS.EM;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CMMSAPIs.Controllers.EM
{
    [Route("api/[controller]")]
    [ApiController]
    public class EMController : ControllerBase
    {
        private readonly IEMBS _EMBS;

        public EMController(IEMBS em)
        {
            _EMBS = em;
        }

        [Authorize]
        [Route("SetEscalationMatrix")]
        [HttpPost]
        public async Task<IActionResult> SetEscalationMatrix(List<CMSetMasterEM> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _EMBS.SetEscalationMatrix(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetEscalationMatrix")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationMatrix(CMMS.CMMS_Modules module)
        {
            try
            {
                var data = await _EMBS.GetEscalationMatrix(module);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("Escalate")]
        [HttpPost]
        public async Task<IActionResult> Escalate(CMMS.CMMS_Modules module, int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _EMBS.Escalate(module, id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ShowEscalationLog")]
        [HttpGet]
        public async Task<IActionResult> ShowEscalationLog(CMMS.CMMS_Modules module, int id)
        {
            try
            {
                var data = await _EMBS.ShowEscalationLog(module, id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
