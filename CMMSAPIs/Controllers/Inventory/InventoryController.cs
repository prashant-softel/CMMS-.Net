using CMMSAPIs.BS.Inventory;
using CMMSAPIs.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
        public async Task<IActionResult> AddInventory(List<CMAddInventory> request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var data = await _InventoryBS.AddInventory(request);
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
                var data = await _InventoryBS.UpdateInventory(request);
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
                var data = await _InventoryBS.DeleteInventory(id);
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
