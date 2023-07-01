using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.PM
{
    public class CMSetScheduleData
    {
        public int facility_id { get; set; }
        public List<CMScheduleData> asset_schedules { get; set; }

    }
    public class CMScheduleData
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public List<ScheduleFrequencyData> frequency_dates { get; set; }
    }

    public class ScheduleFrequencyData
    {
        public int schedule_id { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
        public DateTime? schedule_date { get; set; }
    }

    public class ScheduleIDData
    {
        public int schedule_id { get; set; }
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int asset_id { get; set; }
        public int category_id { get; set; }
        public int frequency_id { get; set; }
        public DateTime schedule_date { get; set; }
    }
}
