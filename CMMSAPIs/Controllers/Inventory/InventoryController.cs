using CMMSAPIs.BS.Inventory;
using CMMSAPIs.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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

        [Route("GetInventoryList")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryList(int facility_id)
        {
            try
            {
                var data = await _InventoryBS.GetInventoryList(facility_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("ViewInventory")]
        [HttpGet]
        public async Task<IActionResult> ViewInventory(int id)
        {
            try
            {
                var data = await _InventoryBS.ViewInventory(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("AddInventory")]
        [HttpPost]
        public async Task<IActionResult> AddInventory(CMAddInventory request)
        {
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

    }
}
