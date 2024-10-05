using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Audit
{
    public class CMAuditListFilter 
    {
        public int facililty_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }

    public class CMAuditScheduleList 
    {
        public int id { get; set; }
        public string audit_order_number { get; set; }
        public DateTime schedule_date { get; set; }
        public string auditee_name { get; set; }
        public string auditor_name { get; set; }
        public string facility_name { get; set; }
        public string status { get; set; }
    }

    public class CMAuditScheduleDetail : CMAuditScheduleList
    {
        public List<ScheduleCheckList> schedule_check_list { get; set; }
        public List<ScheduleLinkJob> schedule_link_job { get; set; }
        public List<CMLog> history_log { get; set; }
    }

    public class CMExecuteAuditSchedule : CMAuditScheduleDetail 
    {

    }


}
