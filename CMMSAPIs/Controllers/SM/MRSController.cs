using CMMSAPIs.BS.SM;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        //[Authorize]
        /* [Route("CreateMRS")]
         [HttpPost]
         public async Task<IActionResult> CreateMRS(CMMRS request)
         {
             try
             {
                 int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                 var data = await _MRSBS.CreateMRS(request, userID);
                 return Ok(data);
             }
             catch (Exception ex)
             {
                 _logger.LogInformation(ex.ToString());
                 _logger.LogError(ex.ToString());
                 ExceptionResponse item = new ExceptionResponse();
                 item.Status = 400;
                 item.Message = "Invalid data sent.";
                 return Ok(item);
             }
         }*/
        [Route("CreateMRS")]
        [HttpPost]
        public async Task<IActionResult> CreateMRS(CMMRS request, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.CreateMRS(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                _logger.LogError(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("updateMRS")]
        [HttpPost]
        public async Task<IActionResult> updateMRS(CMMRS request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.updateMRS(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                _logger.LogError(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("getMRSList")]
        [HttpGet]
        public async Task<IActionResult> getMRSList(int facility_ID, int emp_id, string toDate, string fromDate, int status)
        {
            try
            {

                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_ID)?.timezone;
                var data = await _MRSBS.getMRSList(facility_ID, emp_id, Convert.ToDateTime(toDate), Convert.ToDateTime(fromDate), status, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {

                _AddLog.ErrorLog(ex.ToString());
                _AddLog.ErrorLog("Exception got using ILOGGER " + ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("getMRSListByModule")]
        [HttpGet]
        public async Task<IActionResult> getMRSListByModule(int jobId, int pmId, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _MRSBS.getMRSListByModule(jobId, pmId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {

                _AddLog.ErrorLog(ex.ToString());
                _AddLog.ErrorLog("Exception got using ILOGGER " + ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("getMRSItems")]
        [HttpGet]
        public async Task<IActionResult> getMRSItems(int ID, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _MRSBS.getMRSItems(ID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("getMRSItemsBeforeIssue")]
        [HttpGet]
        public async Task<IActionResult> getMRSItemsBeforeIssue(int ID, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;

                var data = await _MRSBS.getMRSItemsBeforeIssue(ID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("getMRSItemsWithCode")]
        [HttpGet]
        public async Task<IActionResult> getMRSItemsWithCode(int ID, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _MRSBS.getMRSItemsWithCode(ID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("getMRSDetails")]
        [HttpGet]
        public async Task<IActionResult> getMRSDetails(int ID, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _MRSBS.getMRSDetails(ID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("mrsApproval")]
        [HttpPost]
        public async Task<IActionResult> mrsApproval(CMMrsApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.mrsApproval(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("mrsReject")]
        [HttpPost]
        public async Task<IActionResult> mrsReject(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.mrsReject(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("getReturnDataByID")]
        [HttpGet]
        public async Task<IActionResult> getReturnDataByID(int ID, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _MRSBS.getReturnDataByID(ID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }

        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
        [Route("CreateReturnMRS")]
        [HttpPost]
        public async Task<IActionResult> CreateReturnMRS(CMMRS request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.CreateReturnMRS(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("CreateReturnFaultyMRS")]
        [HttpPost]
        public async Task<IActionResult> CreateReturnFaultyMRS(CMMRS request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.CreateReturnFaultyMRS(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("UpdateReturnMRS")]
        [HttpPost]
        public async Task<IActionResult> UpdateReturnMRS(CMMRS request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.UpdateReturnMRS(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("ApproveMRSReturn")]
        [HttpPost]
        public async Task<IActionResult> ApproveMRSReturn(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.ApproveMRSReturn(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }
        //[Authorize]
        [Route("RejectMRSReturn")]
        [HttpPost]
        public async Task<IActionResult> RejectMRSReturn(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.RejectMRSReturn(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }



        //[Authorize]
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
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("GetAssetItems")]
        [HttpGet]
        public async Task<IActionResult> GetAssetItems(int facility_ID, bool isGroupByCode = false)
        {
            try
            {
                var data = await _MRSBS.GetAssetItems(facility_ID, isGroupByCode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("MRSIssue")]
        [HttpPost]
        public async Task<IActionResult> MRSIssue(CMMRS request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.MRSIssue(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("ApproveMRSIssue")]
        [HttpPost]
        public async Task<IActionResult> ApproveMRSIssue(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.ApproveMRSIssue(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("RejectMRSIssue")]
        [HttpPost]
        public async Task<IActionResult> RejectMRSIssue(CMApproval request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.RejectMRSIssue(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("GetMRSReturnList")]
        [HttpGet]
        public async Task<IActionResult> GetMRSReturnList(int facility_ID, bool self_view)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_ID)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _MRSBS.GetMRSReturnList(facility_ID, self_view, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("TransferItems")]
        [HttpPost]
        public async Task<IActionResult> TransferItems(List<CMTransferItems> request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                //var result = await _MRSBS.updateUsedQty(request);
                var data = await _MRSBS.TransactionDetails(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }

        [Route("getIssuedAssetItems")]
        [HttpGet]
        public async Task<IActionResult> getIssuedAssetItems(int asset_ID)
        {
            try
            {
                var data = await _MRSBS.getIssuedAssetItems(asset_ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }
        [Route("getMRSReturnStockItems")]
        [HttpGet]
        public async Task<IActionResult> getMRSReturnStockItems(int mrs_id)
        {
            try
            {
                var data = await _MRSBS.getMRSReturnStockItems(mrs_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }
        [Route("GetAvailableQuantityinPlant")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableQuantityinPlant(int smassetid)
        {
            try
            {
                var data = await _MRSBS.GetAvailableQuantityinPlant(smassetid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _AddLog.ErrorLog(ex.ToString());
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data sent.";
                return Ok(item);
            }
        }
    }
}
