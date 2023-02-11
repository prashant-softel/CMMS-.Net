﻿using CMMSAPIs.BS.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.PM;
using CMMSAPIs.BS.PM;
using System.Collections.Generic;

namespace CMMSAPIs.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMController : ControllerBase
    {
        private readonly IPMBS _PMBS;

        public PMController(IPMBS pm)
        {
            _PMBS = pm;
        }

        [Authorize]
        [Route("GetScheduleData")]
        [HttpGet]
        public async Task<IActionResult> GetScheduleData(int facility_id, int category_id)
        {
            try
            {
                var data = await _PMBS.GetScheduleData(facility_id, category_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("SetScheduleData")]
        [HttpPost]
        public async Task<IActionResult> SetScheduleData(List<CMSetScheduleData> request)
        {
            try
            {
                var data = await _PMBS.SetScheduleData(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
