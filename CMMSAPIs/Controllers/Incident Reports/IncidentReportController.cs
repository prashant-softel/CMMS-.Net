using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.BS.Incident_Reports;

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
        public async Task<IActionResult> CreateIncidentReport(CMCreateIncidentReport request, int userId)
        {
            try
            {
                var data = await _IncidentReportBS.CreateIncidentReport(request,userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetIncidentDetailsReport")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentDetailsReport(int id)
        {
            try
            {
                var data = await _IncidentReportBS.GetIncidentDetailsReport(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> UpdateIncidentReport(CMCreateIncidentReport request, int userId)
        {
            try
            {
                var data = await _IncidentReportBS.UpdateIncidentReport(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> ApproveIncidentReport(int incidentId,int userId)
        {
            try
            {
                var data = await _IncidentReportBS.ApproveIncidentReport(incidentId,userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> RejectIncidentReport([FromForm] CMApproveIncident request,int userId)
        {
            try
            {
                var data = await _IncidentReportBS.RejectIncidentReport(request,userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
