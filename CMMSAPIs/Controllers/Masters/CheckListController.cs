using CMMSAPIs.BS.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMMSAPIs.Models.Masters;
using System.Collections.Generic;

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
        public async Task<IActionResult> GetCheckList(int facility_id, string type)
        {
            try
            {
                var data = await _CheckListBS.GetCheckList(facility_id, type);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateChecklist")]
        [HttpPost]
        public async Task<IActionResult> CreateChecklist(List<CMCreateCheckList> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.CreateChecklist(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateCheckList")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCheckList(CMCreateCheckList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.UpdateCheckList(request, userID);
                return Ok(data);
            }
            catch (Exception)
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
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.DeleteChecklist(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetCheckListMap")]
        [HttpGet]
        public async Task<IActionResult> GetCheckListMap(int facility_id, int category_id, int? type)
        {
            try
            {
                var data = await _CheckListBS.GetCheckListMap(facility_id, category_id, type);
                return Ok(data);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
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
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.CreateCheckListMap(request, userID);
                return Ok(data);
            }
            catch (Exception)
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
                var data = await _CheckListBS.UpdateCheckListMap(request);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetCheckPointList")]
        [HttpGet]
        public async Task<IActionResult> GetCheckPointList(int checklist_id)
        {
            try
            {
                var data = await _CheckListBS.GetCheckPointList(checklist_id);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("CreateCheckPoint")]
        [HttpPost]
        public async Task<IActionResult> CreateCheckPoint(List<CMCreateCheckPoint> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.CreateCheckPoint(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateCheckPoint")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCheckPoint(CMCreateCheckPoint request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.UpdateCheckPoint(request, userID);
                return Ok(data);
            }
            catch (Exception)
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
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.DeleteCheckPoint(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ValidateChecklistCheckpoint")]
        [HttpGet]
        public async Task<IActionResult> ValidateChecklistCheckpoint(int file_id)
        {
            try
            {
                var data = await _CheckListBS.ValidateChecklistCheckpoint(file_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("ImportChecklistCheckpoint")]
        [HttpPost]
        public async Task<IActionResult> ImportChecklistCheckpoint(int file_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _CheckListBS.ImportChecklistCheckpoint(file_id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
