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
    public class CheckListMapController : ControllerBase
    {
        private readonly ICheckListMapBS _CheckListMapBS;

        public CheckListMapController(ICheckListMapBS check_list_map)
        {
            _CheckListMapBS = check_list_map;
        }

        [Authorize]
        [Route("GetCheckListMap")]
        [HttpGet]
        public async Task<IActionResult> GetCheckListMap(int facility_id, int type)
        {
            try
            {
                var data = await _CheckListMapBS.GetCheckListMap(facility_id, type);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateChecklistMap")]
        [HttpPost]
        public async Task<IActionResult> CreateChecklistMap(CMCreateCheckListMap request)
        {
            try
            {
                var data = await _CheckListMapBS.CreateCheckListMap(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateCheckListMap")]
        [HttpPut]
        public async Task<IActionResult> UpdateCheckListMap(CMCreateCheckListMap request)
        {
            try
            {
                var data = await _CheckListMapBS.UpdateCheckListMap(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }     
    }
}
