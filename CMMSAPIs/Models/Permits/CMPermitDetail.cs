using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Permits
{
    public class CMPermitDetail
    {
        public Int64 isExpired { get; set; }
        public dynamic tbt_start { get; set; }
        public int isExtended { get; set; }
        public int insertedId { get; set; }
        public int permitNo { get; set; }
        public string sitePermitNo { get; set; }
        public int permitTypeid { get; set; }
        public string PermitTypeName { get; set; }
        public string title { get; set; }
        public TimeSpan extendByMinutes { get; set; }
        public string description { get; set; }
        public int facility_id { get; set; }
        public int blockId { get; set; }
        public string siteName { get; set; }
        public string BlockName { get; set; }
        public DateTime? start_datetime { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? end_datetime { get; set; }
        //Loto?
        public string permitArea { get; set; }
        public int workingTime { get; set; }
        public int job_type_id { get; set; }
        public string job_type_name { get; set; }
        public int sop_type_id { get; set; }
        public string sop_type_name { get; set; }
        public int requester_id { get; set; }
        public string requestedByName { get; set; }
        public string requesterDesignation { get; set; }
        public string requesterCompany { get; set; }

        public int issuer_id { get; set; }
        public string issuedByName { get; set; }
        public string issuerDesignation { get; set; }
        public string issuerCompany { get; set; }
        public DateTime? request_datetime { get; set; }
        public DateTime? issue_at { get; set; }
        public int issueRejectedby_id { get; set; }
        public string issueRejectedByName { get; set; }
        public string issueRejecterDesignation { get; set; }
        public string issueRejecterCompany { get; set; }

        public DateTime? issueRejected_at { get; set; }
        public int closedby_id { get; set; }
        public string closedByName { get; set; }
        public string closedByDesignation { get; set; }
        public string closedByCompany { get; set; }

        public DateTime? close_at { get; set; }
        public int approver_id { get; set; }
        public string approvedByName { get; set; }
        public string approverDesignation { get; set; }
        public string approverCompany { get; set; }

        public DateTime? approve_at { get; set; }
        public int rejecter_id { get; set; }
        public string rejectedByName { get; set; }
        public string rejecterDesignation { get; set; }
        public string rejecterCompany { get; set; }

        public DateTime? rejected_at { get; set; }
        public int cancelRequestby_id { get; set; }
        public string cancelRequestByName { get; set; }
        public string cancelRequestByDesignation { get; set; }
        public string cancelRequestByCompany { get; set; }

        public DateTime? cancel_at { get; set; }
        public int cancelRequestApprovedby_id { get; set; }
        public string cancelRequestApprovedByName { get; set; }
        public string cancelRequestApprovedByDesignation { get; set; }
        public string cancelRequestApprovedByCompany { get; set; }

        public int cancelRequestRejectedby_id { get; set; }
        public string cancelRequestRejectedByName { get; set; }
        public string cancelRequestRejectedByDesignation { get; set; }
        public string cancelRequestRejectedByCompany { get; set; }

        public int extendRequestby_id { get; set; }
        public string extendRequestByName { get; set; }
        public int extendRequestRejectedby_id { get; set; }

        public string extendRequestRejectedByName { get; set; }

        public DateTime? extend_at { get; set; }
        public int extendRequestApprovedby_id { get; set; }
        public string extendRequestApprovedByName { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<int> category_ids { get; set; }
        public List<CMCategory> LstCategory { get; set; }
        public int is_grid_isolation_required { get; set; }
        public DateTime? grid_start_datetime { get; set; }
        public DateTime? grid_stop_datetime { get; set; }
        public string grid_remark { get; set; }
        public int is_physical_iso_required { get; set; }
        public List<CMDefaultList> physical_iso_equips { get; set; }
        public string physical_iso_remark { get; set; }
        public int is_loto_required { get; set; }
        public List<CMLotoListDetail> Loto_list { get; set; }
        public string loto_remark { get; set; }
        //public List<int> isolated_category_ids { get; set; }
        public List<CMDefaultList> LstIsolationCategory { get; set; }
        public List<CMSaftyQuestion> safety_question_list { get; set; }
        public List<CMFileDetail> file_list { get; set; }
        //public List<CMLoto> Loto_list { get; set; }
        public List<CMEMPLIST> employee_list { get; set; }
        public List<CMIsolationList> LstIsolation { get; set; }
        public List<CMAssociatedList> LstAssociatedJobs { get; set; }
        public List<CMAssociatedPMList> LstAssociatedPM { get; set; }
        public List<CMAssociatedListMC> ListAssociatedMC { get; set; }
        public List<CMAssociatedPMListVC> ListAssociatedvc { get; set; }

        public int ptwStatus { get; set; }
        public string current_status_short { get; set; }
        public string current_status_long { get; set; }
        public CMPermitConditionDetails closeDetails { get; set; }

        public CMPermitConditionDetails cancelDetails { get; set; }

        public CMPermitConditionDetails extendDetails { get; set; }
        public int TBT_Done_By_id { get; set; }
        public string TBT_Done_By { get; set; }
        public DateTime? TBT_Done_At { get; set; }
        public bool is_TBT_Expire { get; set; }
        public int TBT_Done_Check { get; set; }
        public List<CMPermitLotoOtherList> LotoOtherDetails { get; set; }
    }
    public class CMAssociatedListMC
    {
        public int permitId { get; set; }
        public int plan_id { get; set; }
        public int executionId { get; set; }
        public string title { get; set; }
        public string equipmentCat { get; set; }
        public string equipment { get; set; }
        public dynamic start_date { get; set; }
        public string assignedTo { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
    }
    public class CMAssociatedPMListVC
    {
        public int permitId { get; set; }
        public string title { get; set; }
        public int plan_id { get; set; }
        public int executionId { get; set; }
        public string equipmentCat { get; set; }
        public string equipment { get; set; }
        public dynamic start_date { get; set; }
        public string assignedTo { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
    }
    public class CMLotoListDetail
    {
        public string equipment_name { get; set; }
        public string equipment_cat { get; set; }
        public string Loto_Key { get; set; }
        public string Loto_lock_number { get; set; }
        public string employee_name { get; set; }

        public int Loto_id { get; set; }
    }
    public class CMAssociatedList
    {
        public int jobId { get; set; }
        public int jc_id { get; set; }
        public int permitId { get; set; }
        public string title { get; set; }
        public string equipmentCat { get; set; }
        public string equipment { get; set; }
        public dynamic breakdownTime { get; set; }
        public string assignedTo { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
    }
    public class CMAssociatedPMList
    {
        public int pmId { get; set; }
        public int permitId { get; set; }
        public string title { get; set; }
        public string equipmentCat { get; set; }
        public string equipment { get; set; }
        public dynamic startDate { get; set; }
        public string assignedTo { get; set; }
        public int status { get; set; }
        public string status_short { get; set; }
    }
    public class CMLoto
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public string locksrno { get; set; }
    }

    public class CMCategory
    {
        public string equipmentCat { get; set; }
    }

    public class CMEMPLIST
    {
        public string empName { get; set; }
        public string resp { get; set; }
    }

    public class CMIsolationList
    {
        public int IsolationAssetsCatID { get; set; }
        public string IsolationAssetsCatName { get; set; }
    }

    public class CMSaftyQuestion
    {
        public int saftyQuestionId { get; set; }
        public string SaftyQuestionName { get; set; }
        public int input { get; set; }
        public int ischeck { get; set; }
    }

    public class CMPermitExtend
    {
        public int id { get; set; }
        public int extend_by_minutes { get; set; }
        public string comment { get; set; }
        public int[] conditionIds { get; set; }
        public string otherCondition { get; set; }
        public int[] fileIds { get; set; }

    }

    public class CMPermitApproval
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int[] conditionIds { get; set; }
        public string otherCondition { get; set; }
        public int[] fileIds { get; set; }
        //public string filePath { get; set; }
    }
    public class CMPermitConditions
    {
        public int id { get; set; }
        public int value { get; set; }
        public string name { get; set; }

    }

    public class CMPermitConditionDetails
    {
        public List<CMPermitConditions> conditions { get; set; }
        public List<files> files { get; set; }

    }
    public class files
    {
        public int fileId { get; set; }
        public string path { get; set; }
    }

}
