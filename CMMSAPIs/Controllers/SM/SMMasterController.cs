using CMMSAPIs.BS.SM;
using CMMSAPIs.Models.SM;
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
         * 1. Asset Type
         * 2. Asset Category
         * 3. Measurement Units
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
                var data = await _SMMasterBS.AddAssetType(request);
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
                var data = await _SMMasterBS.UpdateAssetType(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetType(int id)
        {
            try
            {
                var data = await _SMMasterBS.DeleteAssetType(id);
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
        public async Task<IActionResult> AddAssetCategory(CMSMMaster request)
        {
            try
            {
                var data = await _SMMasterBS.AddAssetCategory(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateAssetCategory")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetCategory(CMSMMaster request)
        {
            try
            {
                var data = await _SMMasterBS.UpdateAssetCategory(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetCategory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetCategory(int id)
        {
            try
            {
                var data = await _SMMasterBS.DeleteAssetCategory(id);
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
        public async Task<IActionResult> AddUnitMeasurement(CMSMUnitMaster request)
        {
            try
            {
                var data = await _SMMasterBS.AddUnitMeasurement(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateUnitMeasurement")]
        [HttpPut]
        public async Task<IActionResult> UpdateUnitMeasurement(CMSMUnitMaster request)
        {
            try
            {
                var data = await _SMMasterBS.UpdateUnitMeasurement(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteUnitMeasurement")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUnitMeasurement(int id)
        {
            try
            {
                var data = await _SMMasterBS.DeleteUnitMeasurement(id);
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
        public async Task<IActionResult> AddAssetMaster(CMSMAssetMaster request)
        {
            try
            {
                var data = await _SMMasterBS.AddAssetMaster(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateAssetMaster")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetMaster(CMSMAssetMaster request)
        {
            try
            {
                var data = await _SMMasterBS.UpdateAssetMaster(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetMaster")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetMaster(int id)
        {
            try
            {
                var data = await _SMMasterBS.DeleteAssetMaster(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* Stock List */

        [Route("GetStockList")]
        [HttpGet]
        public async Task<IActionResult> GetStockList(int facility_id)
        {
            try
            {
                var data = await _SMMasterBS.GetStockList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateStockList")]
        [HttpPut]
        public async Task<IActionResult> UpdateStockList(List<CMStock> request)
        {
            try
            {
                var data = await _SMMasterBS.UpdateStockList(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
