using System;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.JC
{
    public class CMJCUpdate
    {
        public int id { get; set; }
        //public int status { get; set; }
        public string comment { get; set; }
        public Boolean is_isolation_required { get; set; }
        public List<CMEmpList> employee_list { get; set; }
        public List<CMIsolatedCategoryId> isolated_list { get; set; }
        public List<CMLotoList> loto_list { get; set; }
        public List<CMFileUploadForm> file_upload_form { get; set; }
        public List<CMHistoryList> history_list { get; set; }
        public List<IFormFile> Attachments { get; set; }

        public List<int> uploadfile_ids { get; set; }
    }
    public class CMIsolatedCategoryId
    {
        public int isolation_id { get; set; }
        public int normalisedStatus { get; set; }
        public DateTime normalisedDate { get; set; }
    }
    public class CMLotoList
    {
        public int loto_id { get; set; }
        public int lotoRemovedStatus { get; set; }
        public DateTime lotoRemovedDate { get; set; }
    }    
    public class CMEmpList
    {
        public int empId { get; set; }
        public int employeeId { get; set; }
        public string responsibility { get; set; }
    }
    public class file_upload_form
    {

    }
    public class CMHistoryList
    {
        public int ModuleRefId { get; set; }
        public string Comment { get; set; }
    }
}
