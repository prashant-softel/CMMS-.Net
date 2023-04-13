using CMMSAPIs.BS.FileUpload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using CMMSAPIs.Models.FileUpload;

namespace CMMSAPIs.Controllers.FileUpload
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly IFileUploadBS _FileUploadBS;
        public FileUploadController(IFileUploadBS fileUpload)
        {
            _FileUploadBS = fileUpload;            
        }

        [Authorize]
        [Route("UploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] CMFileUpload request)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("_User_Id"));
                var data = await _FileUploadBS.UploadFile(request, userID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
