using Microsoft.VisualBasic;

namespace CMMSAPIs.Models.JC
{
    public class CMJCList
    {
        public int id { get; set; }
        public int job_id { get; set; }
        public int permit_id { get; set; }
        public string permit_no { get; set; }
        public string category_name { get; set; }
        public string details { get; set; }
        public string current_status { get; set; }
        public string description { get; set; }
        public string job_assinged_to { get; set; }
        public DateAndTime job_card_date { get; set; }
        public DateAndTime start_time { get; set; }
        public DateAndTime end_time { get; set; }
    }
}
