using CMMSAPIs.BS;
using CMMSAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;
        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                var data = await mailService.SendEmailAsync(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
