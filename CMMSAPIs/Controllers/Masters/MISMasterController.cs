using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CMMSAPIs.BS.MISMasters;

namespace CMMSAPIs.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class MISMasterController : ControllerBase
    {
        private readonly IMISMasterBS _IMISMasterBS;
        public MISMasterController(IMISMasterBS cmms)
        {
            _IMISMasterBS = cmms;
        }     

        #region helper

        [Route("GetSourceOfObservation")]
        [HttpGet]
        public async Task<IActionResult> GetSourceOfObservation(int source_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetSourceOfObservation(source_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetSourceOfObservationList")]
        [HttpGet]
        public async Task<IActionResult> GetSourceOfObservationList()
        {
            try
            {
                var data = await _IMISMasterBS.GetSourceOfObservationList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("AddSourceOfObservation")]
        [HttpPost]
        public async Task<IActionResult> AddSourceOfObservation(MISSourceOfObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AddSourceOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateSourceOfObservation")]
        [HttpPatch]
        public async Task<IActionResult> UpdateSourceOfObservation(MISSourceOfObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateSourceOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteSourceOfObservation")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSourceOfObservation(int id, int userId)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteSourceOfObservation(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* TYPE OF OBSERVATION *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/

        [Route("GetTypeOfObservation")]
        [HttpGet]
        public async Task<IActionResult> GetTypeOfObservation(int type_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetTypeOfObservation(type_id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("GetTypeOfObservationList")]
        [HttpGet]
        public async Task<IActionResult> GetTypeOfObservationList()
        {
            try
            {
                var data = await _IMISMasterBS.GetTypeOfObservationList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[Authorize]
        [Route("AddTypeOfObservation")]
        [HttpPost]
        public async Task<IActionResult> AddTypeOfObservation(MISTypeObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.AddTypeOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateTypeOfObservation")]
        [HttpPatch]
        public async Task<IActionResult> UpdateTypeOfObservation(MISTypeObservation request, int userId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateTypeOfObservation(request, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteTypeOfObservation")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTypeOfObservation(int id, int userId)
        {
            try
            {
                var data = await _IMISMasterBS.DeleteTypeOfObservation(id, userId);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* RISK TYPE *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        [Route("GetRiskType")]
        [HttpGet]
        public async Task<IActionResult> GetRiskType(int risk_id)
        {
            try
            {
                var data = await _IMISMasterBS.GetRiskType(risk_id);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("GetRiskTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetRiskTypeList()
        {
            try
            {
                var data = await _IMISMasterBS.GetRiskTypeList();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        
        //[Authorize]
        [Route("CreateRiskType")]
        [HttpPost]
        public async Task<IActionResult> CreateRiskType(MISRiskType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.CreateRiskType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("UpdateRiskType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateRiskType(MISRiskType request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.UpdateRiskType(request, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("DeleteRiskType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRiskType(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _IMISMasterBS.DeleteRiskType(id, userID);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
       

        
        #endregion //helper functions

        /*
        [Route("GetWindDailyGenSummary")]
        [HttpGet]

        
        [Route("eQry/{qry}")]
        [HttpGet]
        public async Task<IActionResult> eQry(string qry)
        {
            try
            {
                var data = await _CMMSBS.eQry(qry);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        */

    }
}
