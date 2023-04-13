using Microsoft.AspNetCore.Mvc;
using CMMSAPIs.BS.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.EscalationMatrix;
using CMMSAPIs.BS.EscalationMatrix;
using System.Collections.Generic;
using CMMSAPIs.BS.PM;
using CMMSAPIs.Models.PM;

namespace CMMSAPIs.Controllers.EscalationMatrix
{
    [Route("api/[controller]")]
    [ApiController]
    public class EscalationMatrixController : Controller
    {
        private readonly iEM _iEM;

        public EscalationMatrixController(iEM em)
        {
            _iEM = em;
        }

        [Route("InsertEscalationMatrixData")]
        [HttpPost]
        public async Task<IActionResult> InsertEscalationMatrixData(EscalationMatrixModel request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _iEM.InsertEscalationMatrixData(request, userID);
               
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
