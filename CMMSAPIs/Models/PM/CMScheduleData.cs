using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.PM
{
    public class CMSetScheduleData
    {
        public int facility_id { get; set; }
        public List<CMScheduleData> asset_schedules { get; set; }

        public CMSetScheduleData(ScheduleIDData scheduleData)
        {
            facility_id = scheduleData.facility_id;
            asset_schedules = new List<CMScheduleData>();
            CMScheduleData asset_schedule = new CMScheduleData();
            asset_schedule.asset_id = scheduleData.asset_id;
            asset_schedule.asset_name = null;
            asset_schedule.category_id = 0;
            asset_schedule.category_name = null;
            asset_schedule.frequency_dates = new List<ScheduleFrequencyData>();
            ScheduleFrequencyData schedule = new ScheduleFrequencyData();
            schedule.frequency_name = null;
            schedule.frequency_id = scheduleData.frequency_id;
            schedule.schedule_id = 0;
            schedule.schedule_date = scheduleData.schedule_date;
            asset_schedule.frequency_dates.Add(schedule);
            asset_schedules.Add(asset_schedule);
        }
    }
    public class CMScheduleData
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }
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
