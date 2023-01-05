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

        [Route("RejectIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> RejectIncidentReport(int id)
        {
            try
            {
                var data = await _IncidentReportBS.RejectIncidentReport(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
