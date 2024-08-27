using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.PM
{
    public class CMPMScheduleView
    {
        public int id { get; set; }
        public string maintenance_order_number { get; set; }
        public DateTime? schedule_date { get; set; }
        public DateTime? completed_date { get; set; }
        public int equipment_id { get; set; }
        public string equipment_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
        public string assigned_to_name { get; set; }
        public int permit_id { get; set; }
        public string permit_code { get; set; }
        public int status { get; set; }
        public string status_name { get; set; }
    }

    public class CMPMTaskList
    {
        public int id { get; set; }
        public int facility_id { get; set; }
        public string Site_name { get; set; }
        public string task_code { get; set; }
        public string plan_title { get; set; }
        public DateTime? last_done_date { get; set; }
        public DateTime? due_date { get; set; }
        public DateTime? done_date { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? close_time { get; set; }
        public string Isolation_taken { get; set; }
        public string permit_type { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int assigned_to_id { get; set; }
        public string assigned_to_name { get; set; }
        public int permit_id { get; set; }
        public string permit_code { get; set; }
        public int status_plan { get; set; }
        public int status { get; set; }
        public int ptw_status { get; set; }
        public string status_short { get; set; }
        public string status_plan_short { get; set; }
        public long ptw_tbt_done { get; set; }
        public string status_short_ptw { get; set; }
    }

    public class CMPMScheduleExecutionDetail
    {
        public int schedule_id { get; set; }
        public int clone_of_asset { get; set; }
        public string clone_of_name { get; set; }
        public int assetsID { get; set; }
        public string asset_name { get; set; }
        public string checklist_name { get; set; }
        public string categoryname { get; set; }
        public string PM_Execution_Started_by_name { get; set; }
        public string rejectedbyName { get; set; }
        public List<ScheduleCheckList> checklist_observation { get; set; }
        public List<ScheduleLinkJob> schedule_link_job { get; set; }
        public string startedbyName { get; set; }
        public int completedBy_id { get; set; }
        public string completedBy_name { get; set; }
        public string approvedbyName { get; set; }
        public string updatedbyName { get; set; }
        public DateTime PM_Schedule_date { get; set; }
        public string PM_Frequecy_Name { get; set; }
        public string createdbyName { get; set; }
        public int status {  get; set; }
        public string status_long { get; set; }
        public string cancelledrejectedbyName { get; set; }
        public string PM_Schedule_updated_by { get; set; }
        public string cancelledapprovedbyName { get; set; }
        public string submittedByName { get; set; }

    }
    public class CMPMTaskView : CMPMTaskList
    {
        public List<CMPMScheduleExecutionDetail> schedules { get; set; }
        public List<Materialconsumption> Material_consumption { get; set; }

        public int plan_id { get; set; }

        public DateTime Schedule_Date { get; set; }
        public int started_by_id { get; set; }
        public string started_by_name { get; set; }
        public DateTime started_at { get; set; }
        public int closed_by_id { get; set; }
        public string closed_by_name { get; set; }
        public DateTime closed_at { get; set; }
        public int cancelled_by_id { get; set; }
        public string cancelled_by_name { get; set; }
        public DateTime cancelled_at { get; set; }
        public int approved_by_id { get; set; }
        public string approved_by { get; set; }
        public DateTime approved_at { get; set; }
        public int rejected_by_id { get; set; }
        public string rejected_by_name { get; set; }
        public DateTime rejected_at { get; set; }
        public int updated_by_id { get; set; }
        public string updated_by_name { get; set; }
        public DateTime updated_at { get; set; }
        public string status_long { get; set; }
        public string permit_type { get; set; }
        public int Employee_ID { get; set; }
        public string workdescription { get; set; }
        public string Employee_name { get; set; }
        public string Company { get; set; }
        public string new_remark { get; set; }
        public string employee_list { get; set; }
        public string is_PTW { get; set; }
        public string TBT_conducted_by_name { get; set; }
        public List<string> Employees { get; set; }
        public string Isolation_taken { get; set; }
        public string Isolated_equipment { get; set; }
        public int cancelledbyName { get; set; }
        public int cancelledrejectedbyName { get; set; }
        public DateTime TBT_done_time { get; set; }
        public DateTime Start_time { get; set; }
        public int Status_PTW { get; set; }
        public string completedbyName { get; set; }
        public string cancelledapprovedbyName { get; set; }
        public string createdbyName { get; set; }
        public string deletedbyName { get; set; }
        public int status { get; set; }


    }
    public class CMPMScheduleViewDetail : CMPMScheduleView
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int checklist_id { get; set; }
        public string checklist_name { get; set; }
        public List<ScheduleCheckList> schedule_check_points { get; set; }
        public List<ScheduleLinkJob> schedule_link_job { get; set; }
        public List<CMLog> history_log { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public DateTime? last_done_date { get; set; }
        public DateTime? started_at { get; set; }
        public string started_by { get; set; }
        public DateTime? updated_at { get; set; }
        public string updated_by { get; set; }
        public DateTime? submitted_at { get; set; }
        public string submitted_by { get; set; }
        public DateTime? completed_at { get; set; }
        public DateTime? approved_at { get; set; }
        public string approved_by { get; set; }
        public DateTime? rejected_at { get; set; }
        public string rejected_by { get; set; }
        public DateTime? cancelled_at { get; set; }
        public string cancelled_by { get; set; }
        public DateTime? deleted_at { get; set; }
        public string deleted_by { get; set; }
        //public DateTime? ptw_timed_out_at { get; set; }



    }

    public class ScheduleCheckList
    {
        public int execution_id { get; set; }
        public int check_point_id { get; set; }
        public string check_point_name { get; set; }
        public string requirement { get; set; }
        public int failure_waightage { get; set; }
        public List<ScheduleFiles> files { get; set; }
        public int check_point_type { get; set; }
        public int cp_ok { get; set; }
        public int type_range { get; set; }
        public int type_bool { get; set; }
        public string type_text { get; set; }
        public string observation { get; set; }
        public int linked_job_id { get; set; }
        public int is_custom_check_point { get; set; }
        public int is_file_required { get; set; }
        public int failure_score { get; set; }
        public double min_range { get; set; }
        public double max_range { get; set; }
    }

    public class ScheduleFiles
    {
        public int file_id { get; set; }
        public string _event { get; set; }
        public string file_path { get; set; }
        public string file_description { get; set; }
    }

    public class ScheduleLinkJob
    {
        public int job_id { get; set; }
        public string job_title { get; set; }
        public string job_description { get; set; }
        public DateTime? job_date { get; set; }
        public string job_status { get; set; }
        public int No_of_tools { get; set; }
        public string Tool_name { get; set; }

    }

    public class Materialconsumption
    {
        public int Material_ID { get; set; }
        public string Material_name { get; set; }
        public dynamic used_qty { get; set; }
        public dynamic issued_qty { get; set; }
        public string Material_type { get; set; }
        public int Equipment_ID { get; set; }
    }

    public class CMCustomCheckPoint
    {
        public int schedule_id { get; set; }
        public string check_point_name { get; set; }
        public int is_document_required { get; set; }
        public string requirement { get; set; }
        public int task_id { get; set; }
    }

    public class CMPMExecutionDetail
    {
        public int task_id { get; set; }
        public List<CMPMScheduleObservation> schedules { get; set; }
        public string comment { get; set; }

    }

    public class CMPMScheduleObservation
    {
        public int task_id { get; set; }
        public int schedule_id { get; set; }
        public List<AddObservation> add_observations { get; set; }
    }
    public class AddObservation
    {
        public int job_create { get; set; }
        public int execution_id { get; set; }
        public string observation { get; set; }
        public int range { get; set; }
        public int cp_ok { get; set; }
        public int is_ok { get; set; }
        public int boolean { get; set; }
        public string text { get; set; }
        public List<PMFileUpload> pm_files { get; set; }
    }
    public class PMFileUpload
    {
        public int file_id { get; set; }
        public string file_desc { get; set; }
        public CMMS.CMMS_Events pm_event { get; set; }
    }
    public class ScheduleLinkedPermit
    {
        public int ptw_id { get; set; }
        public string ptw_code { get; set; }
        public string ptw_title { get; set; }
        public int status { get; set; }
    }

    public class AssetList
    {
        public int schedule_id { get; set; }
        public string asset_name { get; set; }
        public string checklist_name { get; set; }
    }

}
