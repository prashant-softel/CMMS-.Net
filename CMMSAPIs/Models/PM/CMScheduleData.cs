using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.PM
{
    public class CMPMPlanList
    {
        public int plan_id { get; set; }
        public string plan_name { get; set; }
        public int status_id { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public DateTime plan_date { get; set; }
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int assigned_to_id { get; set; }
        public string assigned_to_name { get; set; }
        public int plan_freq_id { get; set; }
        public string plan_freq_name { get; set; }
        public int created_by_id { get; set; }
        public string created_by_name { get; set; }
        public DateTime created_at { get; set; }
        public int approved_by_id { get; set; }
        public string approved_by_name { get; set; }
        public DateTime approved_at { get; set; }
        public int rejected_by_id { get; set; }
        public string rejected_by_name { get; set; }
        public DateTime rejected_at { get; set; }
        public int updated_by_id { get; set; }
        public string updated_by_name { get; set; }
        public DateTime updated_at { get; set; }
        public string rejected_close_by_name { get; set; }
        public DateTime rejected_close_Date { get; set; }
        public string approved_close_by_name { get; set; }
        public DateTime approved_close_Date { get; set; }
        public DateTime next_schedule_date { get; set; }
        public string close_by_name { get; set; }
        public DateTime close_Date { get; set; }
        public string close_comment { get; set; }
    }
    public class CMPMPlanDetail : CMPMPlanList
    {
        public int isDraft { get; set; }
        public int type_id { get; set; }
        public List<AssetCheckList> mapAssetChecklist { get; set; }
    }
    public class AssetCheckList
    {
        // public int asset_id { get; set; }
        //  public string asset_name { get; set; }
        //  public int parent_id { get; set; }
        //  public string parent_name { get; set; }

        public int module_qty { get; set; }
        public int checklist_id { get; set; }
        public string checklist_name { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public string parentName { get; set; }
    }
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
        public int task_id { get; set; }
        public int schedule_id { get; set; }
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int asset_id { get; set; }
        public int category_id { get; set; }
        public int frequency_id { get; set; }
        public int checklist_id { get; set; }
        public DateTime schedule_date { get; set; }
    }
}
