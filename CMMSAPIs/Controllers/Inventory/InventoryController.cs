using CMMSAPIs.BS.Inventory;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private readonly IInventoryBS _InventoryBS;
        public InventoryController(IInventoryBS inventory)
        {
            _InventoryBS = inventory;
        }

        //[Authorize]
        [Route("ImportInventories")]
        [HttpPost]
        public async Task<IActionResult> ImportInventories(int file_id, int facility_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.ImportInventories(file_id, facility_id, userID);
                return Ok(data);
            }

            catch (Exception ex)
            {
                throw;
                /*ExceptionResponse item = new ExceptionResponse();

                item.Message = ex.Message;
                return Ok(item);*/
            }
        }

        //[Authorize]
        [Route("GetInventoryList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId)?.timezone;

                var data = await _InventoryBS.GetInventoryList(facilityId, linkedToBlockId, status, categoryIds, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();

                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("SetParentAsset")]
        [HttpPut]
        public async Task<IActionResult> SetParentAsset(int parentID, int childID)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.SetParentAsset(parentID, childID, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetInventoryDetails")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryDetails(int id, int facility_id)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                var data = await _InventoryBS.GetInventoryDetails(id, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();

                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("AddInventory")]
        [HttpPost]
        public async Task<IActionResult> AddInventory(List<CMAddInventory> request, int facility_id)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facility_id)?.timezone;
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.AddInventory(request, userID, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();

                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("UpdateInventory")]
        [HttpPut]
        public async Task<IActionResult> UpdateInventory(CMAddInventory request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.UpdateInventory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();

                item.Message = ex.Message;
                return Ok(item);
            }
        }

        //[Authorize]
        [Route("DeleteInventory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.DeleteInventory(id, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("DeleteInventoryByFacilityId")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInventoryByFacilityId(int facilityId)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.DeleteInventoryByFacilityId(facilityId, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetInventoryCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryCategoryList(int block_id)
        {
            try
            {
                var data = await _InventoryBS.GetInventoryCategoryList(block_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddInventoryCategory")]
        [HttpPost]
        public async Task<IActionResult> AddInventoryCategory(CMInventoryCategoryList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.AddInventoryCategory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateInventoryCategory")]
        [HttpPatch]
        public async Task<IActionResult> UpdateInventoryCategory(CMInventoryCategoryList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.UpdateInventoryCategory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteInventoryCategory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInventoryCategory(int id)
        {
            try
            {
                var data = await _InventoryBS.DeleteInventoryCategory(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetInventoryTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryTypeList()
        {
            try
            {
                var data = await _InventoryBS.GetInventoryTypeList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddInventoryType")]
        [HttpPost]
        public async Task<IActionResult> AddInventoryType(CMInventoryTypeList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.AddInventoryType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateInventoryType")]
        [HttpPatch]
        public async Task<IActionResult> UpdateInventoryType(CMInventoryTypeList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.UpdateInventoryType(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteInventoryType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInventoryType(int id)
        {
            try
            {
                var data = await _InventoryBS.DeleteInventoryType(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetInventoryStatusList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryStatusList()
        {
            try
            {
                var data = await _InventoryBS.GetInventoryStatusList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("AddInventoryStatus")]
        [HttpPost]
        public async Task<IActionResult> AddInventoryStatus(CMInventoryStatusList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.AddInventoryStatus(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("UpdateInventoryStatus")]
        [HttpPatch]
        public async Task<IActionResult> UpdateInventoryStatus(CMInventoryStatusList request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.UpdateInventoryStatus(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("DeleteInventoryStatus")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInventoryStatus(int id)
        {
            try
            {
                var data = await _InventoryBS.DeleteInventoryStatus(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetWarrantyTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetWarrantyTypeList()
        {
            try
            {
                var data = await _InventoryBS.GetWarrantyTypeList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("GetWarrantyUsageTermList")]
        [HttpGet]
        public async Task<IActionResult> GetWarrantyUsageTermList()
        {
            try
            {
                var data = await _InventoryBS.GetWarrantyUsageTermList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[Authorize]
        [Route("GetWarrantyCertificate")]
        [HttpGet]
        public async Task<IActionResult> GetWarrantyCertificate(string facility_id, DateTime from_date, DateTime to_date)
        {
            try
            {
                var data = await _InventoryBS.GetWarrantyCertificate(facility_id, from_date, to_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Authorize]
        [Route("GetCalibrationList")]
        [HttpGet]
        public async Task<IActionResult> GetCalibrationList(string facilityId)
        {
            try
            {
                var facilitytimeZone = JsonConvert.DeserializeObject<List<CMFacilityInfo>>(HttpContext.Session.GetString("FacilitiesInfo")).FirstOrDefault(x => x.facility_id == facilityId.ToInt())?.timezone;

                var data = await _InventoryBS.GetCalibrationList(facilityId, facilitytimeZone);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionResponse item = new ExceptionResponse();

                item.Message = ex.Message;
                return Ok(item);
            }
        }

    }
}
