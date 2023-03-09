
using System;
using CMMSAPIs.Helper;

namespace CMMSAPIs.Models.Utils
{
    public class CMHistoryLogList
    {
        public int id { get; set; }
        public CMMS.CMMS_Modules module_type { get; set; }
        public int module_ref_id { get; set; }
        public CMMS.CMMS_Modules sec_module { get; set; }
        public int sec_ref_id { get; set; }
        public string comment { get; set; }
        public CMMS.CMMS_Status status { get; set; }
        //add timestamp
        public DateTime timestamp { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class CMLog
    {
        public int id { get; set; }
        public CMMS.CMMS_Modules module_type { get; set; }
        public int module_ref_id { get; set; }
        public CMMS.CMMS_Modules secondary_module_type { get; set; }
        public int secondary_module_ref_id { get; set; }
        public string comment { get; set; }
        public CMMS.CMMS_Status status { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
