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
        public int ApplyFrequency { get; set; }
        public string auditor_name { get; set; }
    }

    public class CMCreateAuditPlan : CMAuditPlan 
    {

    }
    
    public class CMAuditPlanList
    {
        public int id { get; set; }
        public string plan_number { get; set; }
        public string facility_name { get; set; }
        public string Auditee_Emp_Name { get; set; }
        public string Auditor_Emp_Name { get; set; }
        public int frequency { get; set; }
        public int status { get; set; }
        public string short_status { get; set; }
        public string FrequencyApplicable { get; set; }

    }
}
