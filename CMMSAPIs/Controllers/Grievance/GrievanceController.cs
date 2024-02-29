using CMMSAPIs.BS.Grievance;
using CMMSAPIs.Models.Grievance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CMMSAPIs.Controllers.Grievance
{
    [Route("api/[controller]")]
    [ApiController]
   
        public class GrievanceController : ControllerBase
        {

            private readonly IGrievanceBS _Grievance;
            public GrievanceController(IGrievanceBS grievance)
            {
                _Grievance = grievance;
            }

           
            //[Authorize]
          [Route("GetGrievanceList")]
            [HttpGet]
            public async Task<IActionResult> GetGrievanceList(string facilityId, string status, string startDate, string endDate, int selfView, int userID)
            {
             try
             {
                var data = await _Grievance.GetGrievanceList(facilityId, status, startDate, endDate, selfView, userID);
                return Ok(data);
            }
                 catch (Exception ex)
                {
                    // Handle the exception as needed
                    return BadRequest(); // or return another IActionResult
                }
            }

            //[Authorize]
            [Route("GetGrievanceDetails")]
            [HttpGet]
            public async Task<IActionResult> GetInventoryDetails(int id)
            {
                try
                {
                    var data = await _Grievance.GetGrievanceDetails(id);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //[Authorize]
            [Route("CreateGrievance")]
            [HttpPost]
            public async Task<IActionResult> CreateGrievance(CMCreateGrievance request, int userID)
            {
                if (request is null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                try
                {
                    var data = await _Grievance.CreateGrievance(request, userID);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //[Authorize]
            [Route("UpdateGrievance")]
            [HttpPut]
            public async Task<IActionResult> UpdateGrievance(CMUpdateGrievance request)
            {
                try
                {
                    int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                    var data = await _Grievance.UpdateGrievance(request, userID);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //[Authorize]
            [Route("DeleteGrievance")]
            [HttpDelete]
            public async Task<IActionResult> DeleteGrievance(int id, int userID)
            {
                try
                {
                var data = await _Grievance.DeleteGrievance(id, userID);
                return Ok(data);
            }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }