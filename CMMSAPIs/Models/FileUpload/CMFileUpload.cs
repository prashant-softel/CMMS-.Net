using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.FileUpload
{
    public class CMFileUpload
    {
        public List<IFormFile>? files { get; set; }
    }
}
