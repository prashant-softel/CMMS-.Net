using CMMSAPIs.BS.Inverter;
using CMMSAPIs.Models.Inverter;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Inverter
{
    [Route("api/[controller]")]
    [ApiController]
    public class InverterController : ControllerBase
    {
        private readonly IInverterBS _InverterBS;

        public InverterController(IInverterBS Inverter)
        {
            _InverterBS = Inverter;
        }


        //[Authorize]
        
        [Route("GetAllInvertersDataByDate")]
        [HttpGet]
        public async Task<IActionResult> GetAllInvertersDataByDate(int PlantId, string Date)
        {
            try
            {
                var data = await _InverterBS.GetAllInvertersDataByDate(PlantId, Date);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [Route("GetInvertersDataById")]
        [HttpGet]
        public async Task<IActionResult> GetInvertersDataById(int InvId, string Date)
        {
            try
            {
                var data = await _InverterBS.GetInvertersDataById(InvId, Date);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("GetInvGraphData")]
        [HttpGet]
        public async Task<IActionResult> GetInvGraphData(int PlantId, string Date)
        { 
            try
            {
                var data = await _InverterBS.GetInvGraphData(PlantId, Date);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[Authorize]

    } 
}
