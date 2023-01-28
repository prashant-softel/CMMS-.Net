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
    public class CheckListController : ControllerBase
    {
        private readonly ICheckListBS _CheckListBS;

        public CheckListController(ICheckListBS checklist)
        {
            _CheckListBS = checklist;
        }

        [Authorize]
        [Route("GetCheckList")]
        [HttpGet]
        public async Task<IActionResult> GetCheckList(int facility_id, int type)
        {
            try
            {
                var data = await _CheckListBS.GetCheckList(facility_id, type);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateChecklist")]
        [HttpPost]
        public async Task<IActionResult> CreateChecklist(CMCreateCheckList request)
        {
            try
            {
                var data = await _CheckListBS.CreateChecklist(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateCheckList")]
        [HttpPut]
        public async Task<IActionResult> UpdateCheckList(CMCreateCheckList request)
        {
            try
            {
                var data = await _CheckListBS.UpdateCheckList(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("DeleteChecklist")]
        [HttpDelete]
        public async Task<IActionResult> DeleteChecklist(int id)
        {
            try
            {
                var data = await _CheckListBS.DeleteChecklist(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
