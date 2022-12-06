using CMMSAPIs.BS;
using CMMSAPIs.Models;
using CMMSAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleBS _ModuleBS;
        public ModuleController(IModuleBS Module)
        {
            _ModuleBS = Module;
        }

        [Route("GetModuleList")]
        [HttpGet]
        public async Task<IActionResult> GetModuleList()
        {
            try
            {
                var data = await _ModuleBS.GetModuleList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
