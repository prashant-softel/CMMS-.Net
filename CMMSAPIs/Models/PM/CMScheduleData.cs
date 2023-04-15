using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.PM
{
    public class CMScheduleData
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public List<ScheduleFrequencyData> frequency_dates { get; set; }
    }

    public class ScheduleFrequencyData
    {
        public int schedule_id { get; set; }
        public string maintenance_order_number { get; set; }
        public string frequency_name { get; set; }
        public DateTime schedule_date { get; set; }
    }

    public class CMSetScheduleData 
    {
        public DateTime schedule_date { get; set; }
        public int facility_id { get; set; }
        public int frequency_id { get; set; }
        public int asset_id { get; set; }
    }

}
