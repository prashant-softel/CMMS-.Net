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
using Newtonsoft.Json;
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


        ////[Authorize] 
        [Route("GetPlantStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetPlantStockReport(string facility_id, DateTime StartDate, DateTime EndDate, string assetMasterIDs)
        {
            try
            {
                var data = await _SMReportsBS.GetPlantStockReport(facility_id, StartDate, EndDate, assetMasterIDs);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid facility id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Obsolete("This API is obsoleted, use this api/SMReports/GetStockReport?!", true)]
        [Route("GetEmployeeStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeStockReport(int facility_id, int Emp_id, DateTime StartDate, DateTime EndDate, string itemID)
        {
            //try
            //{
            //    var data = await _SMReportsBS.GetEmployeeStockReport(facility_id, Emp_id, StartDate, EndDate, itemID);
            //    return Ok(data);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionResponse item = new ExceptionResponse();
            //    item.Status = 400;
            //    item.Message = "Invalid employee id or facility id is sent.";
            //    return Ok(item);
            //}
            throw new NotImplementedException();
        }


        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Data failed while fetching.";
                return Ok(item);
            }
        }


        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid employee id or facility id is sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("GetEmployeeStock")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeStock(int facility_ID, int emp_id)
        {
            try
            {
                var data = await _SMReportsBS.GetEmployeeStock(facility_ID, emp_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid employee id or facility id is sent.";
                return Ok(item);
            }
        }
        //changes
        [Route("GetTaskStock")]
        [HttpGet]
        public async Task<IActionResult> GetpmTaskStock(int facility_ID, int task_id)
        {
            try
            {
                var data = await _SMReportsBS.GetpmTaskStock(facility_ID,task_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid employee id or facility id is sent.";
                return Ok(item);
            }
        }


        [Route("GetTransactionReport")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionReport(string facility_ID, int actorType, int actorID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                int facility_IDs = facility_ID.ToInt();
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_IDs)?.timezone;
                

              var data = await _SMReportsBS.GetTransactionReport(facility_ID, actorType, actorID, fromDate, toDate, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid employee id or facility id is sent.";
                return Ok(item);
            }
        }

        [Route("GetAssetMasterStockItems")]
        [HttpGet]
        public async Task<IActionResult> GetAssetMasterStockItems(int assetMasterID)
        {
            try
            {
                var data = await _SMReportsBS.GetAssetMasterStockItems(assetMasterID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Master id sent.";
                return Ok(item);
            }
        }

        [Route("GetStockReport")]
        [HttpGet]
        public async Task<IActionResult> GetStockReport(string facility_id, int actorTypeID, int actorID, DateTime StartDate, DateTime EndDate, string assetMasterIDs)
        {
            try
            {
                var data = await _SMReportsBS.GetStockReport(facility_id, actorTypeID, actorID, StartDate, EndDate, assetMasterIDs);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Some error occured.";
                return Ok(item);
            }
        }


        [Route("GetPlantItemTransactionReport")]
        [HttpGet]
        public async Task<IActionResult> GetPlantItemTransactionReport(string facility_ID, int assetItemId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                int facility_IDs = facility_ID.ToInt();
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_IDs)?.timezone;


                var data = await _SMReportsBS.GetPlantItemTransactionReport(facility_ID, assetItemId, fromDate, toDate, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid employee id or facility id is sent.";
                return Ok(item);
            }
        }
    }
}
