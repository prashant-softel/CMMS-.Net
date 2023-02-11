using CMMSAPIs.BS.Audits;
using CMMSAPIs.Models.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace CMMSAPIs.Controllers.Audits
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditPlanController : ControllerBase
    {
        private readonly IAuditPlanBS _AuditPlanBS;

        public AuditPlanController(IAuditPlanBS audit_plan)
        {
            _AuditPlanBS = audit_plan;
        }

        [Authorize]
        [Route("GetAuditPlanList")]
        [HttpGet]
        public async Task<IActionResult> GetAuditPlanList(int facility_id)
        {
            try
            {
                var data = await _AuditPlanBS.GetAuditPlanList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateAuditPlan")]
        [HttpGet]
        public async Task<IActionResult> CreateAuditPlan(CMCreateAuditPlan request)
        {
            try
            {
                var data = await _AuditPlanBS.CreateAuditPlan(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateAuditPlan")]
        [HttpGet]
        public async Task<IActionResult> UpdateAuditPlan(CMCreateAuditPlan request)
        {
            try
            {
                var data = await _AuditPlanBS.UpdateAuditPlan(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("DeleteAuditPlan")]
        [HttpGet]
        public async Task<IActionResult> DeleteAuditPlan(int audit_plan_id)
        {
            try
            {
                var data = await _AuditPlanBS.DeleteAuditPlan(audit_plan_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
