using CMMSAPIs.BS.SM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.SM;
using Microsoft.AspNetCore.Authorization;
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
    public class SMReportsController : ControllerBase
    {
        private readonly ISMReportsBS _SMReportsBS;
        public SMReportsController(ISMReportsBS reportsBS)
        {
            _SMReportsBS = reportsBS;
        }

         
        [Route("GetPlantStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetPlantStockReport(int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetPlantStockReport(plant_ID, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         
        [Route("GetEmployeeStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeStockReport(int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetEmployeeStockReport(plant_ID, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         
        [Route("GetFaultyMaterialReport")]
        [HttpGet]
        public async Task<IActionResult> GetFaultyMaterialReport(int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetFaultyMaterialReport(plant_ID, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         
        [Route("GetEmployeeTransactionReport")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeTransactionReport(int isAllEmployees, int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetEmployeeTransactionReport(isAllEmployees, plant_ID, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
