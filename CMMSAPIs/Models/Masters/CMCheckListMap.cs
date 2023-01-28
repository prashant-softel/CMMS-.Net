using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMCreateCheckListMap : CMCheckListMapList
    {
        public int facility_id { get; set; }
        public List<CMCheckListMapList> checklist_map_list { get; set; }
    }

    public class CMCheckListMapList
    {
        public int category_id { get; set; }
        public List<int> checklist_ids { get; set; }
    }
}
