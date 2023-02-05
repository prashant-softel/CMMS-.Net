using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class CMMSController : ControllerBase
    {
        private readonly ICMMSBS _CMMSBS;
        public CMMSController(ICMMSBS cmms)
        {
            _CMMSBS = cmms;
        }


        #region helper

        [Route("GetFacility")]
        [HttpGet]
        public async Task<IActionResult> GetFacility(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetFacility(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetFacilityList")]
        [HttpGet]
        public async Task<IActionResult> GetFacilityList()
        {
            try
            {
                var data = await _CMMSBS.GetFacilityList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetBlockList")]
        [HttpGet]
        public async Task<IActionResult> GetBlockList(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetBlockList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetAssetCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetCategoryList()
        {
            try
            {
                var data = await _CMMSBS.GetAssetCategoryList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetAssetsList")]
        [HttpGet]
        public async Task<IActionResult> GetAssetsList(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetAssetList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetEmployeeList")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeList(int facility_id)
        {
            try
            {
                var data = await _CMMSBS.GetEmployeeList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetBusinessTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetBusinessTypeList()
        {
            try
            {
                var data = await _CMMSBS.GetBusinessTypeList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetBusinessList")]
        [HttpGet]
        public async Task<IActionResult> GetBusinessList(CMMS.CMMS_BusinessType businessType)
        {
            try
            {
                var data = await _CMMSBS.GetBusinessList(businessType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("GetFinancialYear")]
        [HttpGet]
        public async Task<IActionResult> GetFinancialYear()
        {
            try
            {
                var data = await _CMMSBS.GetFinancialYear();
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
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
