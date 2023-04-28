using CMMSAPIs.BS.Inventory;
using CMMSAPIs.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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

        [Authorize]
        [Route("ImportInventories")]
        [HttpPost]
        public async Task<IActionResult> ImportInventories(int file_id)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.ImportInventories(file_id, userID);
                return Ok(data);
            }/*
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidCastException ex)
            {
                return Conflict(ex.Message);
            }*/
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetInventoryList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds)
        {
            try
            {
                var data = await _InventoryBS.GetInventoryList(facilityId, linkedToBlockId, status, categoryIds);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("SetParentAsset")]
        [HttpPut]
        public async Task<IActionResult> SetParentAsset(CMSetParentAsset parent_child_group)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _InventoryBS.SetParentAsset(parent_child_group, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("GetInventoryDetails")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryDetails(int id)
        {
            try
            {
                var data = await _InventoryBS.GetInventoryDetails(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [Route("AddInventory")]
        [HttpPost]
        public async Task<IActionResult> AddInventory(List<CMAddInventory> request, int userID)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var data = await _InventoryBS.AddInventory(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
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
                throw;
            }
        }

        [Authorize]
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

        [Authorize]
        [Route("GetInventoryCategoryList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryCategoryList()
        {
            try
            {
                var data = await _InventoryBS.GetInventoryCategoryList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
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

        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        
    }
}
