namespace CMMSAPIs.Models.Audit
{
    public class CMAuditPlan
    {
        public int id { get; set; }
        public int Facility_id { get; set; }
        public string plan_number { get; set; }
        public int auditee_id { get; set; }
        public string auditee_name { get; set; }
        public int auditor_id { get; set; }
        public int Status { get; set; }
        public string auditor_name { get; set; }
    }

    public class CMCreateAuditPlan : CMAuditPlan 
    {

    }
    

}
