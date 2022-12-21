using CMMSAPIs.BS.Masters;
using CMMSAPIs.BS.Utils;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.utils
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly IUtilsBS _UtilsBS;
        public UtilsController(IUtilsBS utils)
        {
            _UtilsBS = utils;
        }

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
        public async Task<IActionResult> AddLog([FromForm] Log log)
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

        [Route("GetLog")]
        [HttpGet]
        public async Task<IActionResult> GetLog(int module_type, int id)
        {
            try
            {
                var data = await _UtilsBS.GetLog(module_type, id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}