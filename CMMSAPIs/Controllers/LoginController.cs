using CMMSAPIs.BS;
using CMMSAPIs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly iLoginBS _loginBs;
        public LoginController(iLoginBS login)
        {
            _loginBs = login;
        }
        [Route("UserLogin")]
        [HttpGet]
       // public async Task<IActionResult> UserLogin(string username, string password)
        public async Task<IActionResult> UserLogin(string username, string password)
        {
           try
            {
              
                var data =await _loginBs.GetUserLogin(username, password);
                return Ok(data);
            }
            catch (Exception ex)
            {
                  
                return BadRequest(ex.Message);
            }
        }
        [Route("eQry/{qry}")]
        [HttpGet]
        public async Task<IActionResult> eQry(string qry)
        {
            try
            {
                var data = await _loginBs.eQry(qry);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



    }


}
