using System;
using System.Collections.Generic;

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
        public int Checklist_id { get; set; }
        public string auditor_name { get; set; }
        public string Description { get; set; }
        public DateTime Schedule_Date { get; set; }
        public string assignedTo { get; set; }
        public List<string> Employees { get; set; }
        public bool is_PTW { get; set; }
        public int Module_Type_id { get; set; }
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
        public string frequency_name { get; set; }
        public int status { get; set; }
        public string short_status { get; set; }
        public string FrequencyApplicable { get; set; }
        public int Checklist_id { get; set; }
        public string checklist_name { get; set; }
        public string Description { get; set; }
        public DateTime Schedule_Date { get; set; }
        public DateTime created_at { get; set; }
        public string created_by { get; set; }
        public DateTime approved_Date { get; set; }
        public string approved_by { get; set; }
        public int Module_Type_id { get; set; }
        public string Module_Type { get; set; }
        public string assignedTo { get; set; }
        public string Employees { get; set; }
        public string is_PTW { get; set; }
    }
}
