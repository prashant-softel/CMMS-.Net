using CMMSAPIs.BS.EM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.EM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        //[Authorize]
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
        [Route("GetEscalationMatrixbystatusId")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationMatrixbystatusId(CMMS.CMMS_Modules module, int status_id)
        {
            try
            {
                var data = await _EMBS.GetEscalationMatrixbystatusId(module, status_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("GetEscalationMatrixList")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationMatrixList(CMMS.CMMS_Modules module)
        {
            try
            {
                var data = await _EMBS.GetEscalationMatrixList(module);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
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

        //[Authorize]
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
