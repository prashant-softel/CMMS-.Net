using System;

namespace CMMSAPIs.Models.Audit
{
    public class CMAudit
    {
        
    }

    public class CMCreateAuditCheckListMap
    {
        public int facility_id { get; set; }
        public int plan_id { get; set; }
        public int checklist_id { get; set; }
        public DateTime schedule_date { get; set; }
    }
}
