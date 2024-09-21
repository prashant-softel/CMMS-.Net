using CMMSAPIs.BS.Portfolio;
using CMMSAPIs.Models.Portfolio;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers.Portfolio
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPorfolioBS _PortfolioBS;

        public PortfolioController (IPorfolioBS Portfolio)
        {
            _PortfolioBS = Portfolio;
        }


        //[Authorize]
        [Route("GetUserPlantsDetail")]
        [HttpGet]
        public async Task<IActionResult> GetUserPlantsDetail()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PortfolioBS.GetUserPlantsDetail(userID);
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
        
        [Route("GetUserPlantsEnergyDetails")]
        [HttpGet]
        public async Task<IActionResult> GetUserPlantsEnergyDetails(string Date)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _PortfolioBS.GetUserPlantsEnergyDetails(userID, Date);
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
        
        //[Route("GetPlantPerformanceDetails")]
        //[HttpGet]
        //public async Task<IActionResult> GetPlantPerformanceDetails(int PlantId, string Date)
        //{
        //    try
        //    {
        //        var data = await _PortfolioBS.GetPlantPerformanceDetails(PlantId, Date);
        //        return Ok(data);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        
        //[Route("GetDashboardGraphDataByDate")]
        //[HttpGet]
        //public async Task<IActionResult> GetDashboardGraphDataByDate(int PlantId, string Date)
        //{
        //    try
        //    {
        //        var data = await _PortfolioBS.GetDashboardGraphDataByDate(PlantId, Date);
        //        return Ok(data);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        
        //[Route("GetAllInvertersDataByDate")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllInvertersDataByDate(int PlantId, string Date)
        //{
        //    try
        //    {
        //        var data = await _PortfolioBS.GetAllInvertersDataByDate(PlantId, Date);
        //        return Ok(data);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        
        //[Route("GetInvertersDataById")]
        //[HttpGet]
        //public async Task<IActionResult> GetInvertersDataById(int InvId, string Date)
        //{
        //    try
        //    {
        //        var data = await _PortfolioBS.GetInvertersDataById(InvId, Date);
        //        return Ok(data);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        ////[Authorize]

    } 
}
