using CMMSAPIs.BS.SMB;
using CMMSAPIs.Models.SMB;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.SMB
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMBController : ControllerBase
    {
        private readonly ISMBBS _SMBBS;

        public SMBController(ISMBBS SMB)
        {
            _SMBBS = SMB;
        }


        //[Authorize]
        
        [Route("GetSMBListOfPlant")]
        [HttpGet]
        public async Task<IActionResult> GetSMBlistofPlant(int PlantId)
        {
            try
            {
                var data = await _SMBBS.GetSMBlistofPlant(PlantId);
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
        
        [Route("GetSMBDataByDate")]
        [HttpGet]
        public async Task<IActionResult> GetSMBDataByDate(int SMBId, string Date)
        {
            try
            {
                var data = await _SMBBS.GetSMBDataByDate(SMBId, Date);
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
