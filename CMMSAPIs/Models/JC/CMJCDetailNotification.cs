using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.JC
{
    public class CMJCDetailNotification
    {
        public string plant_name { get; set; }
        public string asset_category_name { get; set; }
        public string JC_Approved_By_Name { get; set; }
        public int id { get; set; }
        public int currentEmpID { get; set; }
        public string current_status { get; set; }
        public string block { get; set; }
        public string JC_Closed_by_Name { get; set; }
        public string JC_Rejected_By_Name { get; set; }
        public string description { get; set; }
        public int jobid { get; set; }
        public int ptwId { get; set; }
    }
}
