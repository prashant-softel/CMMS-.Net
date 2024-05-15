using CMMSAPIs.BS.Audits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.PM;
using Microsoft.AspNetCore.Http;

namespace CMMSAPIs.Controllers.Audits
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditScheduleViewController : ControllerBase
    {
        private readonly IAuditScheduleViewBS _AuditScheduleViewBS;

        public AuditScheduleViewController(IAuditScheduleViewBS audit_schedule_view)
        {
            _AuditScheduleViewBS = audit_schedule_view;
        }

        //[Authorize]
        [Route("GetAuditScheduleViewList")]
        [HttpGet]
        public async Task<IActionResult> GetAuditScheduleViewList(CMAuditListFilter request)
        {
            try
            {
                var data = await _AuditScheduleViewBS.GetAuditScheduleViewList(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetAuditScheduleDetail")]
        [HttpGet]
        public async Task<IActionResult> GetAuditScheduleDetail(int audit_id)
        {
            try
            {
                var data = await _AuditScheduleViewBS.GetAuditScheduleDetail(audit_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ExecuteAuditSchedule")]
        [HttpPost]
        public async Task<IActionResult> ExecuteAuditSchedule(CMPMExecutionDetail request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _AuditScheduleViewBS.ExecuteAuditSchedule(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("ApproveAuditSchedule")]
        [HttpPut]
        public async Task<IActionResult> ApproveAuditSchedule(CMApproval request)
        {
            try
            {
                var data = await _AuditScheduleViewBS.ApproveAuditSchedule(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("RejectAuditSchedule")]
        [HttpPut]
        public async Task<IActionResult> RejectAuditSchedule(CMApproval request)
        {
            try
            {
                var data = await _AuditScheduleViewBS.RejectAuditSchedule(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
