using CMMSAPIs.BS.SM;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.SM
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMMasterController : ControllerBase
    {
        private readonly ISMMasterBS _SMMasterBS;
        public SMMasterController(ISMMasterBS Master)
        {
            _SMMasterBS = Master;
        }

        /*
         * Stock Management Master End Points
         * 1. Asset Type --done
         * 2. Asset Category
         * 3. Measurement Units -- done
         * 4. Master Assets
        */

        /*
         * Asset Type CRUD End Points
        */

        //[Authorize]
        [Route("GetAssetTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetTypeList(int id)
        {
            try
            {
                var data = await _SMMasterBS.GetAssetTypeList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("AddAssetType")]
        [HttpPost]
        public async Task<IActionResult> AddAssetType(CMSMMaster request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddAssetType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("UpdateAssetType")]
        [HttpPost]
        public async Task<IActionResult> UpdateAssetType(CMSMMaster request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdateAssetType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("DeleteAssetType")]
        [HttpPost]
        public async Task<IActionResult> DeleteAssetType([FromForm] int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteAssetType(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }




        //[Authorize]
        [Route("GetUnitMeasurementList")]
        [HttpGet]
        public async Task<IActionResult> GetUnitMeasurementList(int id)
        {
            try
            {
                var data = await _SMMasterBS.GetUnitMeasurementList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("AddUnitMeasurement")]
        [HttpPost]
        public async Task<IActionResult> AddUnitMeasurement(CMUnitMeasurement request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddUnitMeasurement(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("UpdateUnitMeasurement")]
        [HttpPost]
        public async Task<IActionResult> UpdateUnitMeasurement(CMUnitMeasurement request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdateUnitMeasurement(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("DeleteUnitMeasurement")]
        [HttpPost]
        public async Task<IActionResult> DeleteUnitMeasurement([FromForm] int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteUnitMeasurement(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }

        /*
         * Asset Masters CRUD End Points
        */


        //[Authorize]
        [Route("GetAssetMasterList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetMasterList(string facility_id)
        {
            try
            {
                var data = await _SMMasterBS.GetAssetMasterList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("AddAssetMaster")]
        [HttpPost]
        public async Task<IActionResult> AddAssetMaster(CMSMMaster request)
        {
            try
            {
                CMAssetMasterFiles fileData = request.fileData;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddAssetMaster(request, fileData, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("UpdateAssetMaster")]
        [HttpPost]
        public async Task<IActionResult> UpdateAssetMaster(CMSMMaster request)
        {
            try
            {
                CMAssetMasterFiles fileData = request.fileData;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdateAssetMaster(request, fileData, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("DeleteAssetMaster")]
        [HttpPost]
        public async Task<IActionResult> DeleteAssetMaster(CMSMMaster request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteAssetMaster(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }

        //Get Asset List by plant ID

        //[Authorize]
        [Route("GetAssetDataList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetDataList(int facility_id)
        {
            try
            {
                var data = await _SMMasterBS.GetAssetDataList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid facility id is sent.";
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("GetVendorList")]
        [HttpGet]
        public async Task<IActionResult> GetVendorList()
        {
            try
            {
                var data = await _SMMasterBS.GetVendorList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Failed to load data.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("GetAssetBySerialNo")]
        [HttpGet]
        public async Task<IActionResult> GetAssetBySerialNo(string serial_number)
        {
            try
            {
                var data = await _SMMasterBS.GetAssetBySerialNo(serial_number);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid serial number is sent.";
                return Ok(item);
            }
        }


        // Paid By API's
        //[Authorize]
        [Route("GetPaidByList")]
        [HttpGet]
        public async Task<IActionResult> GetPaidByList(int id)
        {
            try
            {
                var data = await _SMMasterBS.GetPaidByList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("AddPaidBy")]
        [HttpPost]
        public async Task<IActionResult> AddPaidBy(CMPaidBy request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddPaidBy(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("UpdatePaidBy")]
        [HttpPost]
        public async Task<IActionResult> UpdatePaidBy(CMPaidBy request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdatePaidBy(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("DeletePaidBy")]
        [HttpPost]
        public async Task<IActionResult> DeletePaidBy(CMPaidBy request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeletePaidBy(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }

        [Route("ImportMaterialFile")]
        [HttpPost]
        public async Task<IActionResult> ImportMaterialFile(int file_id, int facilityID)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.ImportMaterialFile(file_id, facilityID, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 200;
                item.Message = ex.Message;
                return Ok(item);
            }
        }

        /*
         * Asset Category CRUD End Points
        */
        //[Authorize]
        [Route("GetMaterialCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetMaterialCategoryList(int id)
        {
            try
            {
                var data = await _SMMasterBS.GetMaterialCategoryList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("AddMaterialCategory")]
        [HttpPost]
        public async Task<IActionResult> AddMaterialCategory(CMItemCategory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddMaterialCategory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("UpdateMaterialCategory")]
        [HttpPost]
        public async Task<IActionResult> UpdateMaterialCategory(CMItemCategory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdateMaterialCategory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid data is sent.";
                return Ok(item);
            }
        }


        //[Authorize]
        [Route("DeleteMaterialCategory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterialCategory(int acID)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteMaterialCategory(acID, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();
                item.Status = 400;
                item.Message = "Invalid id is sent.";
                return Ok(item);
            }
        }
        /*
         * Asset Category CRUD End Points
        */
    }
}
