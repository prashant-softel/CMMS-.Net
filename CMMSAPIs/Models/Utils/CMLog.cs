
using CMMSAPIs.Helper;

namespace CMMSAPIs.Models.Utils
{
    public class CMLog
    {
        public int id { get; set; }
        public CMMS.CMMS_Modules module_type { get; set; }
        public int module_ref_id { get; set; }
        public CMMS.CMMS_Modules secondary_module_type { get; set; }
        public int secondary_module_ref_id { get; set; }
        public string comment { get; set; }
        public CMMS.CMMS_Status status { get; set; }
        public string current_latitude { get; set; }
        public string current_longitude { get; set; }
    }
}
