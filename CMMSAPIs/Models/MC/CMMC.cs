using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace CMMSAPIs.Models.MC
{
    public class CMMC
    {
        public int id { get; set; }
        public string scheduler_number { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int asset_category_id { get; set; }
        public string asset_category_name { get; set;}
        public int total_module { get; set; }
        public int operational_year { get; set; }
        public int annual_cycle_no { get; set; }
        public int duration_day { get; set; }
        public string status { get; set; }
        public int created_by_id { get; set; }
        public string created_by_name { get; set; }
        public DateTime created_at { get; set;}        

    }
    public class CMMCPlanList
    {
        public int id { get; set;}
        public int asset_id { get; set; }
        public int module_quantity { get; set; }
        public DateTime plan_start_date { get; set; }
        public DateTime plan_end_date { get; set; }
        public DateTime execution_start_date { get; set; }
        public DateTime execution_end_date { get; set; }
        public int water_used { get; set; }
    }

    public class CMMCFilter 
    {
        public int facility_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set;}
        public bool isCompleted { get; set; }
        public bool isOpen { get; set; }
        public bool isAbandoned { get; set; }
        public bool isRejected { get; set; }
        public bool isInProgress { get; set; }
    }

    public class CMMCPlan : CMMC
    {
       public List<CMMCPlanList> plan_list { get; set; }
    }

    public class CMMCExecutionList 
    {
        public int id { get; set; }
        public int schedule_id { get; set; }
        public int completed_quantity { get; set; }
        public int water_used { get; set; }
    }

    public class CMMCExecution 
    {
        public int module_cleaning_id { get; set; }
        public int status { get; set; }
        public List<CMMCExecutionList> executionList { get; set; }
    }
}
