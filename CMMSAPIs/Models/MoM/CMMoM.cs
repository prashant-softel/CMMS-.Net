using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.MoM
{
    public class CMMoM
    {
        public int Id { get; set; }
        public string Issue { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public string AssignTo { get; set; }
        public int Status { get; set; }
        public string Status_long { get; set; }
        public string ActionPlan { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<CMMoMAssignTo> mom_assignto  { get; set; }
        public List<CMMoMTargetDate> mom_target_date { get; set; }
        public List<CMMoMFacilities> mom_facilities { get; set; }
}

public class CMMoMAssignTo
{
       public int mom_id { get; set; }
       public int assign_to_id { get; set; }
       public string assign_to_name { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
    }
    public class CMMoMTargetDate
    {
        public int mom_id { get; set; }
        public DateTime? target_date { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
    }
    public class CMMoMFacilities
    {
        public int mom_id { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
