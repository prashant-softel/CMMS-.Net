using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.BS.Incident_Reports;
using Microsoft.AspNetCore.Authorization;

namespace CMMSAPIs.Controllers.Incident_Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentReportController : ControllerBase
    {
        private readonly IIncidentReportBS _IncidentReportBS;
        public IncidentReportController(IIncidentReportBS incident_report)
        {
            _IncidentReportBS = incident_report;
        }

        [Authorize]
        [Route("GetIncidentList")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var data = await _IncidentReportBS.GetIncidentList(facility_id, start_date, end_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Authorize]
        [Route("CreateIncidentReport")]
        [HttpPost]
        public async Task<IActionResult> CreateIncidentReport(CMCreateIncidentReport request)
        {
            try
            {
                var data = await _IncidentReportBS.CreateIncidentReport(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ViewIncidentReport")]
        [HttpGet]
        public async Task<IActionResult> ViewIncidentReport(int id)
        {
            try
            {
                var data = await _IncidentReportBS.ViewIncidentReport(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> UpdateIncidentReport(CMCreateIncidentReport request)
        {
            try
            {
                var data = await _IncidentReportBS.UpdateIncidentReport(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ApproveIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> ApproveIncidentReport(int id)
        {
            try
            {
                var data = await _IncidentReportBS.ApproveIncidentReport(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("RejectIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> RejectIncidentReport([FromForm] CMApproveIncident request)
        {
            try
            {
                var data = await _IncidentReportBS.RejectIncidentReport(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
