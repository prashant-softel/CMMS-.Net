using CMMSAPIs.Models.Utils;
using CMMSAPIs.Helper;
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

    public class CMPMScheduleViewDetail : CMPMScheduleView
    {   
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int checklist_id { get; set; }
        public string checklist_name { get; set; }
        public List<ScheduleCheckList> schedule_check_points { get; set; }
        public List<ScheduleLinkJob> schedule_link_job { get; set; }
        public List<CMLog> history_log { get; set; }
    }

    public class ScheduleCheckList 
    {
        public int execution_id { get; set; }
        public int check_point_id { get; set; }
        public string check_point_name { get; set; }
        public string requirement { get; set; }
        public string observation { get; set; }
        public int linked_job_id { get; set; }
        public int is_custom_check_point { get; set; }
        public int is_file_required { get; set; }
        public List<ScheduleFiles> files { get; set; }
    }

    public class ScheduleFiles
    {
        public string _event { get; set; }
        public string file_path { get; set; }
        public string file_description { get; set; }
    }

    public class ScheduleLinkJob 
    {
        public int job_id { get; set; }
        public string job_title { get; set; }
        public string job_description { get; set;}
        public DateTime job_date { get; set; }
        public string job_status { get; set; }

    }

    public class CMCustomCheckPoint
    {
        public int schedule_id { get; set; }
        public string check_point_name { get; set; }
        public int is_document_required { get; set; }
        public string requirement { get; set; }
    }

    public class CMPMExecutionDetail
    {
        public int schedule_id { get; set; }
        public List<AddObservation> add_observations { get; set; }
    }
    public class AddObservation
    {
        public int execution_id { get; set; }
        public int isOK { get; set; }
        public string observation { get; set; }
        public int job_create { get; set; }
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

}
