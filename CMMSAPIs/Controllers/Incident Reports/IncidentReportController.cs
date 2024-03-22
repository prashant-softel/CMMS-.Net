using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.BS.Incident_Reports;
using Microsoft.AspNetCore.Http;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _IncidentReportBS.GetIncidentList(facility_id, start_date, end_date, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("CreateIncidentReport")]
        [HttpPost]
        public async Task<IActionResult> CreateIncidentReport(CMCreateIncidentReport request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.CreateIncidentReport(request,userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("UpdateIncidentInvestigationReport")]
        [HttpPost]
        public async Task<IActionResult> UpdateIncidentInvestigationReport(CMCreateIncidentReport request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.UpdateIncidentInvestigationReport(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetIncidentDetailsReport")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentDetailsReport(int id,int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _IncidentReportBS.GetIncidentDetailsReport(id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateIncidentReport")]
        [HttpPut]
        public async Task<IActionResult> UpdateIncidentReport(CMCreateIncidentReport request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.UpdateIncidentReport(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApproveIncidentReport")]
        [HttpPost]
        public async Task<IActionResult> ApproveIncidentReport(CMApproveIncident request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.ApproveIncidentReport(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectIncidentReport")]
        [HttpPost]
        public async Task<IActionResult> RejectIncidentReport(CMApproveIncident request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.RejectIncidentReport(request,userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveCreateIR")]
        [HttpPost]
        public async Task<IActionResult> ApproveCreateIR(CMApproveIncident request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.ApproveCreateIR(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectCreateIR")]
        [HttpPost]
        public async Task<IActionResult> RejectCreateIR(CMApproveIncident request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.RejectCreateIR(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CancelIR")]
        [HttpPost]
        public async Task<IActionResult> CancelIR(CMApproveIncident request)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IncidentReportBS.CancelIR(request, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
