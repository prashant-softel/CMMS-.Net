using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMCreateCheckListMap
    {
        public int facility_id { get; set; }
        public List<CMCheckListMap> checklist_map_list { get; set; }
    }

    public class CMCheckListMap
    {
        public int category_id { get; set; }
        public int status { get; set; }
        public int plan_id { get; set; }
        public List<int> checklist_ids { get; set; }
		       /* Audit specific fields start */
        //public DateTime audit_schedule_date { get; set; }

        /* End */
    }
    public class CMCheckListIdName
    { 
        public int checklist_id { get; set; }
        public string checklist_name { get; set; }
        public int type { get; set; }
        public int mapping_id { get; set; }
    }
    public class CMCheckListMapList
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int? status { get; set; }
        public int plan_id { get; set; }
        public List<CMCheckListIdName> checklists { get; set; }
        /* Audit specific fields start */
        //public DateTime audit_schedule_date { get; set; }

        /* End */
    }
}
