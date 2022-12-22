using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Permits
{
    public class CreatePermitModel
    {
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public List<int> category_ids { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set;}
        public string description { get; set; }
        public Boolean is_isolation_required { get; set; }
        public List<int> isolated_category_ids { get; set; }
        public List<KeyValuePairs> Loto_list { get; set; }
        public List<KeyValuePairs> employee_list { get; set; }
        public List<KeyValuePairs> safety_question_list { get; set; }
        public int job_type_id { get; set; }
        public int sop_type_id { get; set;}
        public List<FileUploadFormModel> file_upload_form { get; set; }
        public int issuer_id { get; set; }
        public int approver_id { get; set;}
        public int user_id { get; set; }

    }
}
