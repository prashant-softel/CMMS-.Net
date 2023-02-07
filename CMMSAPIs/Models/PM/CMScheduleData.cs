using System;

namespace CMMSAPIs.Models.PM
{
    public class CMScheduleData
    {
        public int id { get; set; }
        public DateTime schedule_date { get; set; }
        public int frequency_id { get; set; }
        public int asset_id { get; set; }
        public int status { get; set; }
    }

    public class CMSetScheduleData 
    {
        public int id { get; set; }
        public DateTime schedule_date { get; set;}
    }

}
