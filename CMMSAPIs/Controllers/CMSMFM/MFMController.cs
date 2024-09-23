using CMMSAPIs.BS.MFM;
using CMMSAPIs.Models.MFM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.MFM
{
    [Route("api/[controller]")]
    [ApiController]
    public class MFMController : ControllerBase
    {
        private readonly IMFMBS _MFMBS;

        public MFMController(IMFMBS MFM)
        {
            _MFMBS = MFM;
        }


        //[Authorize]
        
        [Route("GetMFMListOfPlant")]
        [HttpGet]
        public async Task<IActionResult> GetMFMlistofPlant(int PlantId)
        {
            try
            {
                var data = await _MFMBS.GetMFMlistofPlant(PlantId);
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
        
        [Route("GetMFMGraphData")]
        [HttpGet]
        public async Task<IActionResult> GetMFMGraphData(int MFMId, string Date)
        {
            try
            {
                var data = await _MFMBS.GetMFMGraphData(MFMId, Date);
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
