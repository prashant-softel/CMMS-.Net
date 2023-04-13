using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CMMSAPIs.Helper;

namespace CMMSAPIs.Models.FileUpload
{
    public class CMFileUpload
    {
        public List<IFormFile>? files { get; set; }
        public int facility_id { get; set; }
        public CMMS.CMMS_Modules module_type { get; set; }
        public int module_ref_id { get; set; }
        public int file_category { get; set; }
    }
}
