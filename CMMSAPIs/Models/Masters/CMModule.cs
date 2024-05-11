using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMModule
    {
        public int id { get; set; }
        public int software_id { get; set; }
        public string moduleName { get; set; }
        public string featureName { get; set; }
        public string menuImage { get; set; }
        public int? add { get; set; }
        public int? edit { get; set; }
        public int? delete { get; set; }
        public int? view { get; set; }
        public int? approve { get; set; }
        public int? issue { get; set; }
        public int? selfView { get; set; }
    }
    public class CMStatus
    {
        public int module_id { get; set; }
        public int module_software_id { get; set; }
        public string module_name { get; set; }
        public int status_id { get; set; } // only software id will be shown
        public string status_name { get; set; }
    }

    public class CMDashboadModuleWiseList
    {
        public string module_name { get; set; }
        public CMDashboadDetails CMDashboadDetails { get; set; }
    }
    public class CMDashboadDetails
    {
        public int created { get; set; }
        public int submitted { get; set; }
        public int assigned { get; set; }
        public int rejected { get; set; }
        public int approved { get; set; }        
        public int issued { get; set; }
        public int total { get; set; }
        public int completed { get; set; }
        public int pending { get; set; }
        public int wo_on_time { get; set; }
        public int wo_delay { get; set; }
        public int wo_backlog { get; set; }
        public int low_stock_items { get; set; }
        public int po_items_awaited { get; set; }
        public List<CMDashboadItemList> item_list { get; set; }
    }

    public class CMDashboadItemList
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int wo_number { get; set; }
        public int status { get; set; }
        public string status_long { get; set; }
        public string asset_category { get; set; }
        public string asset_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int ptw_id { get; set; }
        public int latestJCStatus { get; set; }
        public int latestJCid { get; set; }
        public int latestJCPTWStatus { get; set; }
        public int latestJCApproval { get; set; }
    }
}
