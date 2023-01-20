using Microsoft.VisualBasic;
using System.Collections.Generic;
namespace CMMSAPIs.Models.JC
{
    public class CMJCList
    {
        public int id { get; set; }
        public int jobCardId { get; set; }
        public int jobid { get; set;  }
        public int permit_id { get; set; }
        public string permit_no { get; set; }
        public int current_status { get; set; }
        public string description { get; set; }
        public string job_assinged_to { get; set; }
        public dynamic job_card_date { get; set; }
        public dynamic start_time { get; set; }
        public dynamic end_time { get; set; }
        public List<equipmentCatList> LstequipmentCatList { get; set; }
    }
   
}
public class equipmentCatList
{
    public int equipmentCat_id { get; set; }
    public string equipmentCat_name { get; set; }
}