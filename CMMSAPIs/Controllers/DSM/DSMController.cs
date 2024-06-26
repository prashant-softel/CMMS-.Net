﻿using CMMSAPIs.BS.DSM;
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

        ////[Authorize]
        [Route("getDSMData")]
        [HttpGet]
        public async Task<IActionResult> getDSMDatagetDSMData(string fy, string month, string stateId, string spvId, string siteId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _DSMBS.getDSMData(fy, month, stateId, spvId, siteId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        ////[Authorize]
        [Route("importDSMFile")]
        [HttpPost]
        public async Task<IActionResult> importDSMFile(int file_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _DSMBS.importDSMFile(file_id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //    //[Authorize]
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

        //    //[Authorize]
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
