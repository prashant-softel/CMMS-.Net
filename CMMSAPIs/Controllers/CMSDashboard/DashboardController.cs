using CMMSAPIs.BS.Dashboard;
using CMMSAPIs.Models.Dashboard;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardBS _DashboardBS;

        public DashboardController(IDashboardBS Dashboard)
        {
            _DashboardBS = Dashboard;
        }


        //[Authorize]
        
        [Route("GetPlantPerformanceDetails")]
        [HttpGet]
        public async Task<IActionResult> GetPlantPerformanceDetails(int PlantId, string Date)
        {
            try
            {
                var data = await _DashboardBS.GetPlantPerformanceDetails(PlantId, Date);
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
        
        [Route("GetPowerGraphDataByMinute")]
        [HttpGet]
        public async Task<IActionResult> GetPowerGraphDataByMinute(int PlantId, string Date)
        {
            try
            {
                var data = await _DashboardBS.GetPowerGraphDataByMinute(PlantId, Date);
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
        
        [Route("GetEnegryGraphDataByMinute")]
        [HttpGet]
        public async Task<IActionResult> GetEnegryGraphDataByMinute(int PlantId, string Date)
        {
            try
            {
                var data = await _DashboardBS.GetEnegryGraphDataByMinute(PlantId, Date);
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
        
        [Route("GetWeatherDataByMinute")]
        [HttpGet]
        public async Task<IActionResult> GetWeatherDataByMinute(int PlantId, string Date)
        {
            try
            {
                var data = await _DashboardBS.GetWeatherDataByMinute(PlantId, Date);
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
