using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.Authentication;
using CMMSAPIs.Models.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace CMMSAPIs.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManagerBS _JwtTokenManagerBS;
        
        public TokenController(IJwtTokenManagerBS jwtTokenManagerBS)
        {
            _JwtTokenManagerBS = jwtTokenManagerBS;
            
        }

        [AllowAnonymous]
        [Route("Authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromForm] UserCrentialsModel credential)
        {
            try
            {
                var data = await _JwtTokenManagerBS.Authenticate(credential);
                if (data == null)
                    return Unauthorized();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }


}
