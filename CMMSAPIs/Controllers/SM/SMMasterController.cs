using CMMSAPIs.BS.SM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.SM;
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
        
        [Route("GetAssetTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetTypeList()
        {
            try
            {
                var data = await _SMMasterBS.GetAssetTypeList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

        [Route("UpdateAssetType")]
        [HttpPut]
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

        [Route("DeleteAssetType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetType(int Id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteAssetType(Id, userID);
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

        [Route("GetAssetCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetCategoryList()
        {
            try
            {
                var data = await _SMMasterBS.GetAssetCategoryList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AddAssetCategory")]
        [HttpPost]
        public async Task<IActionResult> AddAssetCategory(ItemCategory request)
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

        [Route("UpdateAssetCategory")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetCategory(ItemCategory request)
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

        [Route("DeleteAssetCategory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetCategory(int acID)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteAssetCategory(acID, userID);
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

        [Route("GetUnitMeasurementList")]
        [HttpGet]
        public async Task<IActionResult> GetUnitMeasurementList()
        {
            try
            {
                var data = await _SMMasterBS.GetUnitMeasurementList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AddUnitMeasurement")]
        [HttpPost]
        public async Task<IActionResult> AddUnitMeasurement(UnitMeasurement request)
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

        [Route("UpdateUnitMeasurement")]
        [HttpPut]
        public async Task<IActionResult> UpdateUnitMeasurement(UnitMeasurement request)
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

        [Route("DeleteUnitMeasurement")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUnitMeasurement(int umID)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.DeleteUnitMeasurement(umID, userID);
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

        [Route("GetAssetMasterList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetMasterList()
        {
            try
            {
                var data = await _SMMasterBS.GetAssetMasterList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AddAssetMaster")]
        [HttpPost]
        public async Task<IActionResult> AddAssetMaster(CMSMMaster request, AssetMasterFiles fileData)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.AddAssetMaster(request, fileData, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateAssetMaster")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetMaster(CMSMMaster request, AssetMasterFiles fileData)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _SMMasterBS.UpdateAssetMaster(request, fileData, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetMaster")]
        [HttpDelete]
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
    }
}
