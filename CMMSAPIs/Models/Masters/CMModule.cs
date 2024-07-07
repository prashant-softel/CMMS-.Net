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
    public class CMStatus1
    {
        public int module_id { get; set; }
        public int module_software_id { get; set; }
        public string module_name { get; set; }
        public List<Statusformodule> status { get; set; }

    }
    public class Statusformodule
    {
        public int status_id { get; set; } // only software id will be shown
        public string status_name { get; set; }

    }
    public class CMDashboadModuleWiseList
    {
        public string module_name { get; set; }
        public int category_mc_count { get; set; }
        public int category_pm_count { get; set; }
        public int category_bm_count { get; set; }
        public int category_total_count { get; set; }
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
        public int unknown_count { get; set; }
        public int schedule_compliance_total { get; set; }
        public int schedule_compliance_completed { get; set; }
        public int schedule_compliance_pending { get; set; }
        public int wo_on_time { get; set; }
        public int wo_delay { get; set; }
        public int wo_backlog { get; set; }
        public int low_stock_items { get; set; }
        public int po_items_awaited { get; set; }
        public int pm_closed_count { get; set; }
        public int bm_closed_count { get; set; }
        public int mc_closed_count { get; set; }

        public List<CMDashboadItemList> item_list { get; set; }
        public List<CMSMConsumptionByGoods> StockOverview { get; set; }
        public List<CMSMConsumptionByGoods> StockConsumptionByGoods { get; set; }
        public List<CMSMConsumptionByGoods> StockConsumptionBySites { get; set; }
    
    }

    public class CMDashboadItemList
    {
        public int facility_id { get; set; }
        public string facility_name { get; set; }
        public int wo_number { get; set; }
        public string wo_decription { get; set; }
        public string assetsname { get; set; }
        public int status { get; set; }
        public string status_long { get; set; }
        public string asset_category { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? schedule_time { get; set; }
        public DateTime? end_date { get; set; }
        public int ptw_id { get; set; }
        public int latestJCStatus { get; set; }
        public int latestJCid { get; set; }
        public int latestJCPTWStatus { get; set; }
        public int latestJCApproval { get; set; }
        public int on_time_status { get; set; }

        // IR module

        public string title { get; set; }
        public string location_of_incident { get; set; }
        public DateTime? incident_datetime { get; set; }
        public DateTime? restoration_datetime { get; set; }

        // SM module
        public int go_id { get; set; }
        public string GRNo { get; set; }
        public string GONo { get; set; }
        public string product_name { get; set; }
        public decimal requested_qty { get; set; }
        public DateTime? gr_date { get; set; }
        public decimal ordered_qty { get; set; }
        public DateTime? go_date { get; set; }
        public decimal unit_amount { get; set; }
        public int total_amount { get; set; }
        public DateTime? grn_date { get; set; }
        public decimal grn_qty { get; set; }
    }
    public class CMROLE
    {
        public int roleId { get; set; }
    }

    public class CMSMConsumptionByGoods
    {
        public string key { get; set; }
        public decimal value { get; set; }
    }
}
