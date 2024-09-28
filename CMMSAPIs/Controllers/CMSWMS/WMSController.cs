using CMMSAPIs.BS.WMS;
using CMMSAPIs.Models.WMS;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.WMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class WMSController : ControllerBase
    {
        private readonly IWMSBS _WMSBS;

        public WMSController(IWMSBS WMS)
        {
            _WMSBS = WMS;
        }


        //[Authorize]
        
        [Route("GetWMSListOfPlant")]
        [HttpGet]
        public async Task<IActionResult> GetWMSlistofPlant(int PlantId)
        {
            try
            {
                var data = await _WMSBS.GetWMSlistofPlant(PlantId);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [Route("GetWMSDataByDate")]
        [HttpGet]
        public async Task<IActionResult> GetWMSDataByDate(int WMSId, string Date)
        {
            try
            {
                var data = await _WMSBS.GetWMSDataByDate(WMSId, Date);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [Route("GetMinuteWMSGraphData")]
        [HttpGet]
        public async Task<IActionResult> GetMinuteWMSGraphData(int WMSId, string Date)
        {
            try
            {
                var data = await _WMSBS.GetMinuteWMSGraphData(WMSId, Date);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }[Route("GetDailyWMSGraphData")]
        [HttpGet]
        public async Task<IActionResult> GetDailyWMSGraphData(int WMSId, string startDate, string endDate)
        {
            try
            {
                var data = await _WMSBS.GetDailyWMSGraphData(WMSId, startDate, endDate);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }[Route("GetMonthlyWMSGraphData")]
        [HttpGet]
        public async Task<IActionResult> GetMonthlyWMSGraphData(int WMSId, string startDate, string endDate)
        {
            try
            {
                var data = await _WMSBS.GetMonthlyWMSGraphData(WMSId, startDate, endDate);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]

    } 
}
