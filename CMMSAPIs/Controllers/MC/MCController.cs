using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using CMMSAPIs.Models.MC;
using CMMSAPIs.BS.MC;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Controllers.MC
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MCController : Controller
    {
        private readonly IMCBS _MCBS;
        public MCController(IMCBS mc)
        {
            _MCBS = mc;
        }

        [Route("GetCMList")]
        [HttpGet]
        public async Task<IActionResult> GetCMList(CMMCFilter request)
        {
            try
            {
                var data = await _MCBS.GetCMList(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateMCPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateMCPlan(CMMCPlan request)
        {
            try
            {
                var data = await _MCBS.CreateMCPlan(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMCPlan")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCPlan(CMApproval request)
        {
            try
            {
                var data = await _MCBS.ApproveMCPlan(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectMCPlan")]
        [HttpPut]
        public async Task<IActionResult> RejectMCPlan(CMApproval request)
        {
            try
            {
                var data = await _MCBS.RejectMCPlan(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("StartMCExecution")]
        [HttpPost]
        public async Task<IActionResult> StartMCExecution(int id)
        {
            try
            {
                var data = await _MCBS.StartMCExecution(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("MCExecution")]
        [HttpPost]
        public async Task<IActionResult> MCExecution(CMMCExecution request)
        {
            try
            {
                var data = await _MCBS.MCExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CompleteMCExecution")]
        [HttpPost]
        public async Task<IActionResult> CompleteMCExecution(CMMCExecution request)
        {
            try
            {
                var data = await _MCBS.CompleteMCExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMCExecution")]
        [HttpPut]
        public async Task<IActionResult> ApproveMCExecution(CMApproval request)
        {
            try
            {
                var data = await _MCBS.ApproveMCExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectMCExecution")]
        [HttpPut]
        public async Task<IActionResult> RejectMCExecution(CMApproval request)
        {
            try
            {
                var data = await _MCBS.RejectMCExecution(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
