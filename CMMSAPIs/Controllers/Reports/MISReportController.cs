using CMMSAPIs.BS.ReportBS;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class MISReportController : ControllerBase
    {
        private readonly ReportBS reportBS;
        public MISReportController(ReportBS report)
        {
            reportBS = report;
        }
        [Route("GetMisSummary")]
        [HttpGet]
        public async Task<IActionResult> GetMisSummary(string year, int facility_id)
        {
            try
            {

                var data = await reportBS.GetMisSummary(year, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
        [Route("GeEnvironmentalSummary")]
        [HttpGet]
        public async Task<IActionResult> GeEnvironmentalSummary(string year, int facility_id)
        {
            try
            {

                var data = await reportBS.GeEnvironmentalSummary(year, facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse data = new ExceptionResponse();
                data.Status = 200;
                data.Message = ex.Message;
                return Ok(data);
            }
        }
    }
}
