﻿using CMMSAPIs.BS.Masters;
using CMMSAPIs.BS.SM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.SM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.SM
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReOrderController : ControllerBase
    {
        private readonly IReOrderBS _ReOrderBS;
        private readonly ILogger<MRSController> _logger;
        private readonly AddLog _AddLog;
        public ReOrderController(IReOrderBS Master, ILogger<MRSController> ilogger, IConfiguration configuration)
        {
            _ReOrderBS = Master;
            _logger = ilogger;
            _AddLog = new AddLog(configuration);
        }

        [Route("GetReorderDataByID")]
        [HttpGet]
        public async Task<IActionResult> GetReorderDataByID(int assetID, int plantID)
        {
            try
            {
                var data = await _ReOrderBS.GetReorderDataByID(assetID, plantID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
        [Route("submitReorderForm")]
        [HttpPost]
        public async Task<IActionResult> submitReorderForm(ReOrder request)
        {
            try
            {
                var data = await _ReOrderBS.submitReorderForm(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
        [Route("updateReorderData")]
        [HttpPost]
        public async Task<IActionResult> updateReorderData(ReOrder request)
        {
            try
            {
                var data = await _ReOrderBS.updateReorderData(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString()); 
                throw ex;
            }
        }

        [Route("getReorderAssetsData")]
        [HttpGet]
        public async Task<IActionResult> getReorderAssetsData(int plantID)
        {
            try
            {
                var data = await _ReOrderBS.getReorderAssetsData(plantID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
        [Route("getReorderItems")]
        [HttpGet]
        public async Task<IActionResult> getReorderItems(int plantID)
        {
            try
            {
                var data = await _ReOrderBS.getReorderItems(plantID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
        [Route("reorderAssets")]
        [HttpPost]
        public async Task<IActionResult> reorderAssets(ReOrder request)
        {
            try
            {
                var data = await _ReOrderBS.reorderAssets(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ex;
            }
        }
    }
}