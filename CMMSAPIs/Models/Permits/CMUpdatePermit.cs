using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.Permits
{
    public class CMUpdatePermit
    {
     
        public int permit_id { get; set; }
        public int blockId { get; set; }
        public int lotoId { get; set; }
        public int typeId { get; set; }
        public int facility_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string description { get; set; }
        public int job_type_id { get; set; }
        public int sop_type_id { get; set; }
        public int issuer_id { get; set; }
        public int approver_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<int> block_ids { get; set; }
        public List<int> category_ids { get; set; }
        public Boolean? is_isolation_required { get; set; }
        public List<int> isolated_category_ids { get; set; }
        public List<CMPermitLotoList> Loto_list { get; set; }
        public List<CMPermitEmpList> employee_list { get; set; }
        public List<CMPermitSaftyQueList> safety_question_list { get; set; }
        public List<CMFileUploadForm> file_upload_form { get; set; }
        public List<CMUpdatePermit> PermitHistory { get; set; }

    }
}
