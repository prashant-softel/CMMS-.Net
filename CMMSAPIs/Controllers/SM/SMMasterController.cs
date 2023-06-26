using CMMSAPIs.BS.SM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.SM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        /*
         * Asset Category CRUD End Points
        */

        [Authorize]
        [Route("GetAssetCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetCategoryList(int id)
        {
            try
            {
                var data = await _SMMasterBS.GetAssetCategoryList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("AddAssetCategory")]
        [HttpPost]
        public async Task<IActionResult> AddAssetCategory(CMItemCategory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddAssetCategory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("UpdateAssetCategory")]
        [HttpPost]
        public async Task<IActionResult> UpdateAssetCategory(CMItemCategory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdateAssetCategory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("DeleteAssetCategory")]
        [HttpPost]
        public async Task<IActionResult> DeleteAssetCategory([FromForm] int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteAssetCategory(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Asset Category CRUD End Points
        */

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        /*
         * Asset Masters CRUD End Points
        */

        [Authorize]
        [Route("GetAssetMasterList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetMasterList(int id)
        {
            try
            {
                var data = await _SMMasterBS.GetAssetMasterList(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        //Get Asset List by plant ID
        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }
    }
}
