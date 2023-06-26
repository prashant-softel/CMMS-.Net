using CMMSAPIs.BS.SM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.SM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMMSAPIs.Controllers.SM
{
    [Route("api/[controller]")]
    [ApiController]
    public class MRSController : ControllerBase
    {
        private readonly IMRSBS _MRSBS;
        private readonly ILogger<MRSController> _logger;
        private readonly AddLog _AddLog;
        public MRSController(IMRSBS Master, ILogger<MRSController> ilogger, IConfiguration configuration)
        {
            _MRSBS = Master;
            _logger = ilogger;
            _AddLog = new AddLog(configuration); 
        }

        // First 
        [Authorize]
        [Route("CreateMRS")]
        [HttpPost]       
        public async Task<IActionResult> CreateMRS(CMMRS request)
        {
            try
            {
                var data = await _MRSBS.CreateMRS(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("getMRSList")]
        [HttpGet]
        public async Task<IActionResult> getMRSList(int plant_ID, int emp_id, string toDate, string fromDate, int status)
        {
            try
            {
                var data = await _MRSBS.getMRSList(plant_ID, emp_id, Convert.ToDateTime(toDate), Convert.ToDateTime(fromDate), status);
                return Ok(data);
            }
            catch (Exception ex)
            {

                _AddLog.ErrorLog(ex.ToString());
                _AddLog.ErrorLog("Exception got using ILOGGER "+ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("getMRSItems")]
        [HttpGet]
        public async Task<IActionResult> getMRSItems(int ID)
        {
            try
            {
                var data = await _MRSBS.getMRSItems(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }
        [Authorize]
        [Route("getMRSItemsBeforeIssue")]
        [HttpGet]
        public async Task<IActionResult> getMRSItemsBeforeIssue(int ID)
        {
            try
            {
                var data = await _MRSBS.getMRSItemsBeforeIssue(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("getMRSItemsWithCode")]
        [HttpGet]
        public async Task<IActionResult> getMRSItemsWithCode(int ID)
        {
            try
            {
                var data = await _MRSBS.getMRSItemsWithCode(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("getMRSDetails")]
        [HttpGet]
        public async Task<IActionResult> getMRSDetails(int ID)
        {
            try
            {
                var data = await _MRSBS.getMRSDetails(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("mrsApproval")]
        [HttpPost]
        public async Task<IActionResult> mrsApproval(CMMRS request)
        {
            try
            {
                int ID = 0;
                var data = await _MRSBS.mrsApproval(request);
                return Ok(data);
            }
            catch(Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("getReturnDataByID")]
        [HttpGet]
        public async Task<IActionResult> getReturnDataByID(int ID)
        {
            try
            {
                var data = await _MRSBS.getReturnDataByID(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }

        }

        [Authorize]
        [Route("getAssetTypeByItemID")]
        [HttpGet]
        public async Task<IActionResult> getAssetTypeByItemID(int ItemID)
        {
            try
            {
                var data = await _MRSBS.getAssetTypeByItemID(ItemID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("mrsReturn")]
        [HttpPost] 
        public async Task<IActionResult> mrsReturn(CMMRS request)
        {
            try
            {
                var data = await _MRSBS.mrsReturn(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("mrsReturnApproval")]
        [HttpPost]
        public async Task<IActionResult> mrsReturnApproval(CMMRS request)
        {
            try
            {
                var data = await _MRSBS.mrsReturnApproval(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("getLastTemplateData")]
        [HttpGet]
        public async Task<IActionResult> getLastTemplateData(int ID)
        {
            try
            {
                var data = await _MRSBS.getLastTemplateData(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        [Authorize]
        [Route("GetAssetItems")]
        [HttpGet]
        public async Task<IActionResult> GetAssetItems(int plantID, bool isGroupByCode = false)
        {
            try
            {
                var data = await _MRSBS.GetAssetItems(plantID, isGroupByCode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                throw ex;
            }
        }
    }
}
