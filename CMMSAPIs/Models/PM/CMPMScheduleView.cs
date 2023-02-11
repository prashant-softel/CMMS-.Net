using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.PM
{
    public class CMPMScheduleView
    {
        public int id { get; set; }
        public string maintenance_order_number { get; set; }
        public DateTime schedule_date { get; set; }
        public string completed_date { get; set; }
        public string equipment_name { get; set; }
        public string frequency_name { get; set; }
        public string assigned_to_name { get; set; }
        public int permit_id { get; set; }
        public string status_name { get; set; }
        public int status { get; set; }
    }

    public class CMPMScheduleViewDetail : CMPMScheduleView
    {   
        public string facility_name { get; set; }
        public string category_name { get; set; }
        public List<ScheduleCheckList> schedule_check_list { get; set; }
        public List<ScheduleLinkJob> schedule_link_job { get; set; }
        public List<CMLog> history_log { get; set; }
    }

    public class ScheduleCheckList 
    {
        public int check_point_id { get; set; }
        public string check_point_name { get; set; }
        public string requirement { get; set; }
        public string observation { get; set; }
        public int is_job_created { get; set; }

        public int is_custom_check_point { get; set; } 
    }

    public class ScheduleLinkJob 
    {
        public int job_id { get; set; }
        public string job_title { get; set; }
        public string job_description { get; set;}
        public DateTime job_date { get; set; }
        public string job_status { get; set; }

    }

    public class CMPMScheduleExecution : CMPMScheduleViewDetail
    {

    }

  

    
}
