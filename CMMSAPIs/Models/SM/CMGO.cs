using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models
{
    // Create GO Order
    public class CMGOCreate
    {
        public int id { get; set; }
        public int vendor_id { get; set; }
        public DateTime order_date { get; set; }
        public List<CMGOCreateOrder> lst_orders { get; set; }
        public int status { get; set; }
    }

    public class CMGOCreateOrder
    {
        public int asset_id { get; set; }
        public int type { get; set; }
        public int cost { get; set; }
        public int o_qty { get; set; }
    }

    // Common Models

    public class CMGOConsumableOrder : CMGOCreateOrder
    {        
        public int r_qty { get; set; }
        public int d_qty { get; set; }
        public int a_qty { get; set; }
        public bool is_order_pending { get; set; }
        public List<CMGOFile> lst_file { get; set; }
    }

    public class CMGOSpareOrder : CMGOCreateOrder
    {
        public int serial_number { get; set; }
        public string remark { get; set; }
        public bool is_accepted { get; set; }
        public bool is_order_pending { get; set; }
        public List<CMGOFile> lst_file { get; set; }
    }

    public class CMGOFile
    {
        public string name { get; set; }
        public string path { get; set; }    
    }

    public class CMGOCommonFile : CMGOFile 
    {
        public int document_type { get; set; }
        public string remark { get; set; }
    }

    // GO Receive
    public class CMGOReceive
    {
        public string challan_number { get; set; }
        public DateTime challan_date { get; set; }
        public string po { get; set; }
        public DateTime po_date { get; set; }
        public string freight { get; set; }
        public string transport { get; set; }
        public int number_of_pkg_received { get; set; }
        public int l_r_number { get; set; }
        public string pkg_condition_received { get; set; }
        public string vehicle_number { get; set; }
        public string gir_number { get; set; }
        public int job_ref { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public List<CMGOConsumableOrder> lst_consumable_orders { get; set; }
        public List<CMGOSpareOrder> lst_spare_orders { get; set; }
        public int status { get; set; }
    }

    /* GO List */

    public class CMGOList 
    {
        public int id { get; set; }
        public string status { get; set; }
        public DateTime purchase_date { get; set; }
        public string vendor { get; set; }
    }

    public class CMAssetDetail
    {
        public string code { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string image_path { get; set; }
    }
}
