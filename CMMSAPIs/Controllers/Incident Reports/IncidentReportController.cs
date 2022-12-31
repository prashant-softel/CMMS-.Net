using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.BS.Incident_Reports;

namespace CMMSAPIs.Controllers.Incident_Reports
{
    public class IncidentReportController : ControllerBase
    {
        private readonly IIncidentReportBS _IncidentReportBS;
        public IncidentReportController(IIncidentReportBS incident_report)
        {
            _IncidentReportBS = incident_report;
        }


        [Route("GetIncidentList")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentList(int facility_id)
        {
            try
            {
                var data = await _IncidentReportBS.GetIncidentList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

        [Route("FeedBackIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> FeedBackIncidentReport(CMFeedBackIncidentReport request)
        {
            try
            {
                var data = await _IncidentReportBS.FeedBackIncidentReport(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CloseIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> CloseIncidentReport(int id)
        {
            try
            {
                var data = await _IncidentReportBS.CloseIncidentReport(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
