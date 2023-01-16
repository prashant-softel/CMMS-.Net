using CMMSAPIs.BS.JC;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.SM;
using CMMSAPIs.Models.SM;
using System.Collections.Generic;

namespace CMMSAPIs.Controllers.SM
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMMRSController : ControllerBase
    {
        private readonly ISMMRSBS _SMMRSBS;
        public SMMRSController(ISMMRSBS sm_mrs)
        {
            _SMMRSBS = sm_mrs;
        }

        [Route("GetMRSList")]
        [HttpGet]
        public async Task<IActionResult> GetMRSList(int facility_id)
        {
            try
            {
                var data = await _SMMRSBS.GetMRSList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateMRS")]
        [HttpPost]
        public async Task<IActionResult> CreateMRS(CMCreateMRS request)
        {
            try
            {
                var data = await _SMMRSBS.CreateMRS(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("EditMRS")]
        [HttpPut]
        public async Task<IActionResult> EditMRS(CMCreateMRS request)
        {
            try
            {
                var data = await _SMMRSBS.EditMRS(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMRS")]
        [HttpPut]
        public async Task<IActionResult> ApproveMRS(CMApproveMRS request)
        {
            try
            {
                var data = await _SMMRSBS.ApproveMRS(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectMRS")]
        [HttpPut]
        public async Task<IActionResult> RejectMRS(CMRejectMRS request)
        {
            try
            {
                var data = await _SMMRSBS.RejectMRS(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* MRS Return End Points */

        [Route("GetMRSReturnList")]
        [HttpGet]
        public async Task<IActionResult> GetMRSReturnList(int facility_id)
        {
            try
            {
                var data = await _SMMRSBS.GetMRSReturnList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("CreateMRSReturn")]
        [HttpPost]
        public async Task<IActionResult> CreateMRSReturn(CMCreateMRSReturn request)
        {
            try
            {
                var data = await _SMMRSBS.CreateMRSReturn(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("EditMRSReturn")]
        [HttpPut]
        public async Task<IActionResult> EditMRSReturn(CMCreateMRSReturn request)
        {
            try
            {
                var data = await _SMMRSBS.EditMRSReturn(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ApproveMRSReturn")]
        [HttpPut]
        public async Task<IActionResult> ApproveMRSReturn(CMApproveMRSReturn request)
        {
            try
            {
                var data = await _SMMRSBS.ApproveMRSReturn(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("RejectMRSReturn")]
        [HttpPut]
        public async Task<IActionResult> RejectMRSReturn(CMRejectMRSReturn request)
        {
            try
            {
                var data = await _SMMRSBS.RejectMRSReturn(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* My Bucket List */

        [Route("GetBucketAssetList")]
        [HttpGet]
        public async Task<IActionResult> GetBucketAssetList(int facility_id)
        {
            try
            {
                var data = await _SMMRSBS.GetBucketAssetList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ViewConsumeAssets")]
        [HttpGet]
        public async Task<IActionResult> ViewConsumeAssets(int id, int module_id)
        {
            try
            {
                var data = await _SMMRSBS.ViewConsumeAssets(id, module_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("UpdateBucketAssetList")]
        [HttpPut]
        public async Task<IActionResult> UpdateBucketAssetList(CMConsumeAssets request)
        {
            try
            {
                var data = await _SMMRSBS.UpdateBucketAssetList(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
