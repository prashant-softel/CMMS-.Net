using System.Collections.Generic;
namespace CMMSAPIs.Models.Utils
{
    public class CMApproval
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int facility_id { get; set; }
        public int claim_status { get; set; }
        public List<int> uploadfile_ids { get; set; }

    }

}
