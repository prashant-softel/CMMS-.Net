using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.JC
{
    public class CMJCUpdate
    {
        public int id { get; set; }
        public bool status { get; set; }
        public string comment { get; set; }
        public List<KeyValuePairs> employee_list { get; set; }
        public List<CMFileUploadForm> file_upload_form { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
