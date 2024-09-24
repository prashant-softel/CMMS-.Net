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
        public List<CMSMConsumptionByGoods> StockAvailbleByGoods { get; set; }
        public List<CMSMConsumptionByGoods> StockAvailbleBySites { get; set; }
        public List<CMWATERUESD> WaterUsedTotal { get; set; }

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
        public string type_of_incident { get; set; }
        public string location_of_incident { get; set; }
        public string severity { get; set; }
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
        public string MC_Type { get; set; }

        public DateTime End_Date_done { get; set; }
        public dynamic TotalWaterUsed { get; set; }
        public dynamic plan_days { get; set; }
        public dynamic no_of_cleaned { get; set; }
        public dynamic Scheduled { get; set; }
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
    public class CMWATERUESD
    {
        public string site_name { get; set; }
        public dynamic TotalWaterUsed { get; set; }
        public dynamic plan_days { get; set; }
        public dynamic no_of_cleaned { get; set; }

    }
    public class CMDashboadModuleWiseList_byQuery
    {
        public int moduleId { get; set; }                    // Maps to moduleId from the query
        public string module_name { get; set; }              // Maps to module_name from the query
        public int facilityId { get; set; }                  // Maps to facilityId from the query
        public int total { get; set; }                       // Maps to total from the query
        public int created { get; set; }                     // Maps to created from the query
        public int submitted { get; set; }                   // Maps to submitted from the query
        public int assigned { get; set; }                    // Maps to assigned from the query
        public int rejected { get; set; }                    // Maps to rejected from the query
        public int approved { get; set; }                    // Maps to approved from the query
        public int issued { get; set; }                      // Maps to issued from the query
        public int completed { get; set; }                   // Maps to completed from the query
        public int pending { get; set; }                     // Maps to pending from the query
        public int schedule_compliance_total { get; set; }   // Maps to schedule_compliance_total from the query
        public int schedule_compliance_completed { get; set; } // Maps to schedule_compliance_completed from the query
        public int schedule_compliance_pending { get; set; } // Maps to schedule_compliance_pending from the query
        public int wo_on_time { get; set; }                  // Maps to wo_on_time from the query
        public int wo_delay { get; set; }                    // Maps to wo_delay from the query
        public int wo_backlog { get; set; }                  // Maps to wo_backlog from the query
        public int low_stock_items { get; set; }             // Maps to low_stock_items from the query
        public int po_items_awaited { get; set; }            // Maps to po_items_awaited from the query
        public int pm_closed_count { get; set; }             // Maps to pm_closed_count from the query
        public int bm_closed_count { get; set; }             // Maps to bm_closed_count from the query
        public int mc_closed_count { get; set; }             // Maps to mc_closed_count from the query

        public int wo_number { get; set; }                   // Maps to wo_number from the query
        public string wo_decription { get; set; }            // Maps to wo_decription from the query
        public string asset_name { get; set; }               // Maps to asset_name from the query
        public int status { get; set; }                      // Maps to status from the query
        public string status_long { get; set; }              // Maps to status_long from the query
        public string asset_category { get; set; }           // Maps to asset_category from the query
        public DateTime? start_date { get; set; }            // Maps to start_date from the query
        public DateTime? end_date { get; set; }              // Maps to end_date from the query
        public int latest_jc_status { get; set; }            // Maps to latest_jc_status from the query
        public int on_time_status { get; set; }              // Maps to on_time_status from the query
        public decimal? total_water_used { get; set; }
    }
}
