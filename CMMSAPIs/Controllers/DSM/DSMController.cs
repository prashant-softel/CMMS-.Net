using CMMSAPIs.BS.DSM;
using CMMSAPIs.Models.DSM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CMMSAPIs.Controllers.DSM
{
    [Route("api/[controller]")]
    [ApiController]
    public class DSMController : ControllerBase
    {
        private readonly IDSMBS _DSMBS;

        public DSMController(IDSMBS dsm)
        {
            _DSMBS = dsm;
        }

        //[Authorize]
        [Route("getDSMData")]
        [HttpGet]
        public async Task<IActionResult> getDSMData(CMDSMFilter request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _DSMBS.getDSMData(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //    [Authorize]
        //    [Route("GetEscalationMatrix")]
        //    [HttpGet]
        //    public async Task<IActionResult> GetEscalationMatrix(CMMS.CMMS_Modules module)
        //    {
        //        try
        //        {
        //            var data = await _EMBS.GetEscalationMatrix(module);
        //            return Ok(data);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }

        //    [Authorize]
        //    [Route("Escalate")]
        //    [HttpPost]
        //    public async Task<IActionResult> Escalate(CMMS.CMMS_Modules module, int id)
        //    {
        //        try
        //        {
        //            int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
        //            var data = await _EMBS.Escalate(module, id);
        //            return Ok(data);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }

        //    [Authorize]
        //    [Route("ShowEscalationLog")]
        //    [HttpGet]
        //    public async Task<IActionResult> ShowEscalationLog(CMMS.CMMS_Modules module, int id)
        //    {
        //        try
        //        {
        //            var data = await _EMBS.ShowEscalationLog(module, id);
        //            return Ok(data);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}
    }
}
