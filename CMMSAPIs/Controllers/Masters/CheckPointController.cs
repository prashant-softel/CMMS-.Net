using CMMSAPIs.BS.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMMSAPIs.Models.Masters;

using System;

namespace CMMSAPIs.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckPointController : ControllerBase
    {
        private readonly ICheckPointBS _CheckPointBS;

        public CheckPointController(ICheckPointBS checkpoint)
        {
            _CheckPointBS = checkpoint;
        }

        [Authorize]
        [Route("GetCheckPointList")]
        [HttpGet]
        public async Task<IActionResult> GetCheckPointList(int checklist_id)
        {
            try
            {
                var data = await _CheckPointBS.GetCheckPointList(checklist_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateCheckPoint")]
        [HttpPost]
        public async Task<IActionResult> CreateCheckPoint(CMCreateCheckPoint request)
        {
            try
            {
                var data = await _CheckPointBS.CreateCheckPoint(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateCheckPoint")]
        [HttpPut]
        public async Task<IActionResult> UpdateCheckPoint(CMCreateCheckPoint request)
        {
            try
            {
                var data = await _CheckPointBS.UpdateCheckPoint(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("DeleteCheckPoint")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCheckPoint(int id)
        {
            try
            {
                var data = await _CheckPointBS.DeleteCheckPoint(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
