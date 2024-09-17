using CMMSAPIs.BS.EM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.EM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace CMMSAPIs.Controllers.EM
{
    [Route("api/[controller]")]
    [ApiController]
    public class EMController : ControllerBase
    {
        private readonly IEMBS _EMBS;

        public EMController(IEMBS em)
        {
            _EMBS = em;
        }


        #region Notification

        //[Authorize]
        [Route("SendNotification")]
        [HttpPost]
        public async Task<IActionResult> SendNotification(CMNotification request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == request.facilityId)?.timezone;
                var data = await _EMBS.SendNotification(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
                /*
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = ex.Message;
                return Ok(item);
                */
            }
        }


        #endregion //Notification functions

        #region Escalation


        //[Authorize]
        [Route("SetEscalationMatrix")]
        [HttpPost]
        public async Task<IActionResult> SetEscalationMatrix(List<CMSetMasterEM> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _EMBS.SetEscalationMatrix(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetEscalationMatrixbystatusId")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationMatrixbystatusId(CMMS.CMMS_Modules module, int status_id)
        {
            try
            {
                var data = await _EMBS.GetEscalationMatrixbystatusId(module, status_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("GetEscalationMatrixList")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationMatrixList(CMMS.CMMS_Modules module)
        {
            try
            {
                var data = await _EMBS.GetEscalationMatrixList(module);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("Escalate")]
        [HttpPost]
        public async Task<IActionResult> Escalate(CMMS.CMMS_Modules moduleId, CMMS.CMMS_Status statusId, int facilityId, string additionalUserIds)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _EMBS.Escalate(moduleId, statusId, additionalUserIds, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetEscalationLog")]
        [HttpGet]
        public async Task<IActionResult> GetEscalationLog(CMMS.CMMS_Modules module, int id, int facilityId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;
                var data = await _EMBS.GetEscalationLog(module, id, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion //Escalation functions
    }
}
