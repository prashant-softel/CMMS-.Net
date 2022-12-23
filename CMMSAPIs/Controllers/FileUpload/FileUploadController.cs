using CMMSAPIs.BS.FileUpload;
using Microsoft.AspNetCore.Mvc;
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

        [Route("UploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] CMFileUpload request)
        {
            try
            {
                var data = await _FileUploadBS.UploadFile(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
