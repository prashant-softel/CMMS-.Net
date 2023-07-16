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


        [Authorize] 
        [Route("GetPlantStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetPlantStockReport(string facility_id, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetPlantStockReport(facility_id, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Authorize]
        [Route("GetEmployeeStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeStockReport(int facility_id, int Emp_id, DateTime StartDate, DateTime EndDate, string itemID)
        {
            try
            {
                var data = await _SMReportsBS.GetEmployeeStockReport(facility_id, Emp_id, StartDate, EndDate, itemID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Authorize]
        [Route("GetFaultyMaterialReport")]
        [HttpGet]
        public async Task<IActionResult> GetFaultyMaterialReport(string facility_id, string itemID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetFaultyMaterialReport(facility_id, itemID, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Authorize]
        [Route("GetEmployeeTransactionReport")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeTransactionReport(int isAllEmployees, string facility_id, int Emp_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _SMReportsBS.GetEmployeeTransactionReport(isAllEmployees, facility_id, Emp_ID, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
