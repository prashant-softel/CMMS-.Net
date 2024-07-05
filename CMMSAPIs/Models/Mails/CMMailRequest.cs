using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Mails
{
    public class CMMailRequest
    {
        public List<string> ToEmail { get; set; }
        public string emailtraingn { get; set; }
        public List<string> CcEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public string Headers { get; internal set; }
    }

}
