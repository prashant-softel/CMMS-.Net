using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.JC
{
    public class JCUpdateModel
    {
        public int id { get; set; }
        public bool status { get; set; }
        public string comment { get; set; }
        public List<EmployeeFormModel> employee_list { get; set; }
        public List<FileUploadFormModel> file_upload_form { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
