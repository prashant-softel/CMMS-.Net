using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.FileUpload
{
    public class CMFileUpload
    {
        public List<IFormFile>? files { get; set; }
        public int facility_id { get; set; }
        public int module_id { get; set; }
        public int id { get; set; }
    }
}
