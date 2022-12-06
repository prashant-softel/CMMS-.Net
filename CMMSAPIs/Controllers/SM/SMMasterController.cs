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
        public async Task<IActionResult> AddAssetType()
        {
            try
            {
                var data = await _SMMasterBS.AddAssetType();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateAssetType")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetType()
        {
            try
            {
                var data = await _SMMasterBS.UpdateAssetType();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetType()
        {
            try
            {
                var data = await _SMMasterBS.DeleteAssetType();
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
        public async Task<IActionResult> AddAssetCategory()
        {
            try
            {
                var data = await _SMMasterBS.AddAssetCategory();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateAssetCategory")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetCategory()
        {
            try
            {
                var data = await _SMMasterBS.UpdateAssetCategory();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetCategory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetCategory()
        {
            try
            {
                var data = await _SMMasterBS.DeleteAssetCategory();
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
        public async Task<IActionResult> AddUnitMeasurement()
        {
            try
            {
                var data = await _SMMasterBS.AddUnitMeasurement();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateUnitMeasurement")]
        [HttpPut]
        public async Task<IActionResult> UpdateUnitMeasurement()
        {
            try
            {
                var data = await _SMMasterBS.UpdateUnitMeasurement();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteUnitMeasurement")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUnitMeasurement()
        {
            try
            {
                var data = await _SMMasterBS.DeleteUnitMeasurement();
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

        [Route("AddAssetMastert")]
        [HttpPost]
        public async Task<IActionResult> AddAssetMastert()
        {
            try
            {
                var data = await _SMMasterBS.AddAssetMastert();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateAssetMaster")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssetMaster()
        {
            try
            {
                var data = await _SMMasterBS.UpdateAssetMaster();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteAssetMaster")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetMaster()
        {
            try
            {
                var data = await _SMMasterBS.DeleteAssetMaster();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
