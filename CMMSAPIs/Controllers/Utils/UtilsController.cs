using CMMSAPIs.BS.Utils;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.utils
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly IUtilsBS _UtilsBS;
        private readonly AddLog _AddLog;
        public UtilsController(IUtilsBS utils, IConfiguration configuration)
        {
            _UtilsBS = utils;
            _AddLog = new AddLog(configuration);
        }

        [Authorize]
        [Route("GetCountryList")]
        [HttpGet]
        public async Task<IActionResult> GetCountryList()
        {
            try
            {
                var data = await _UtilsBS.GetCountryList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog($"{DateTime.Now} : Failed to get country list");
                throw;
            }
        }

        [Route("GetStateList")]
        [HttpGet]
        public async Task<IActionResult> GetStateList(int country_id)
        {
            try
            {
                var data = await _UtilsBS.GetStateList(country_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetCityList")]
        [HttpGet]
        public async Task<IActionResult> GetCityList(int state_id)
        {
            try
            {
                var data = await _UtilsBS.GetCityList(state_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetCurrencyList")]
        [HttpGet]
        public async Task<IActionResult> GetCurrencyList()
        {
            try
            {
                var data = await _UtilsBS.GetCurrencyList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetTimeZoneList")]
        [HttpGet]
        public async Task<IActionResult> GetTimeZoneList()
        {
            try
            {
                var data = await _UtilsBS.GetTimeZoneList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AddLog")]
        [HttpGet]
        public async Task<IActionResult> AddLog([FromForm] CMLog log)
        {
            try
            {
                var data = await _UtilsBS.AddLog(log);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetHistoryLog")]
        [HttpGet]
        public async Task<IActionResult> GetHistoryLog(int module_type, int id)
        {
            try
            {
                var data = await _UtilsBS.GetHistoryLog(module_type, id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}