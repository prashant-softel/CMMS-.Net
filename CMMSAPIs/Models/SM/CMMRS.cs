using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;

namespace CMMSAPIs.Models.SM
{
    public class CMMRS
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime mrs_date { get; set; }

    }

    public class CMCreateMRS
    {
        public DateTime issue_datetime { get; set; }
        public bool is_template { get; set; }
        public List<CMMRSOrder> order_list { get; set; }
    }

    public class CMMRSOrder
    {
        public int asset_id { get; set; }
        public int requested_qty { get; set; }
    }

    public class CMMRSApprobeOrder : CMMRSOrder
    {
        public int issued_qty { get; set; }
    }

    public class CMApproveMRS
    {
        public int mrs_id { get; set; }
        public List<CMMRSApprobeOrder> approve_order { get; set; }
    }

    public class CMRejectMRS
    {
        public int mrs_id { get; set; }
        public string remark { get; set; }
    }

    /* MRS Return */

    public class CMMRSReturn
    {
        List<CMMRSReturnOrder> return_order { get; set; }
    }

    public class CMMRSReturnOrder
    {
        public int mrs_id { get; set; }
        public string asset_name { get; set; }
        public int available_qty { get; set; }
        public int return_qty { get; set; }
        public string serial_number { get; set; }
        public int return_remark_id { get; set; }
        public string remark_in_brief { get; set; }
    }

    public class CMMRSReturnApproveOrder : CMMRSReturnOrder
    {
        public int received_qty { get; set; }
        public string receive_remark { get; set; }
    }

    public class CMCreateMRSReturn
    {
        List<CMMRSReturnOrder> return_order { get; set; }
    }

    public class CMApproveMRSReturn
    {
        public int msr_return_id { get; set; }
        public List<CMMRSReturnApproveOrder> approve_order { get; set; }
    }

    public class CMRejectMRSReturn
    {
        public int msr_return_id { get; set; }
        public string return_remark { get; set; }
    }

    public class CMBucket
    {
        public int asset_id { get; set; }
        public int available_qty { get; set; }
    }

    public class CMConsumeAssets : CMBucket
    {
        public int used_qty { get; set; }
        public string remark { get; set; }
    }

    public class CMViewConsumeAssets : CMConsumeAssets
    {
        public string asset_name { get; set; }
    }
}
