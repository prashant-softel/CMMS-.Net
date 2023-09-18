using System;
using System.Collections.Generic;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.Permits
{
    public class CMPermitDetail
    {
        public Int64 isExpired { get; set; }
        public int insertedId { get; set; }
        public int permitNo { get; set; }
        public int sitePermitNo { get; set; }
        public int permitTypeid { get; set; }
        public string PermitTypeName { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int facility_id { get; set; }
        public int blockId { get; set; }
        public string siteName { get; set; }
        public string BlockName { get; set; }
        public DateTime? start_datetime { get; set; }
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
        public int issuer_id { get; set; }
        public string issuedByName { get; set; }
        public DateTime? issue_at { get; set; }
        public int issueRejectedby_id { get; set; }
        public string issueRejectedByName{ get; set; }
        public DateTime? issueRejected_at { get; set; }
        public int closedby_id { get; set; }
        public string closedByName { get; set; }
        public DateTime? close_at { get; set; }
        public int approver_id { get; set; }
        public string approvedByName { get; set; }
        public DateTime? approve_at { get; set; }
        public int rejecter_id { get; set; }
        public string rejectedByName { get; set; }
        public DateTime? rejected_at { get; set; }
        public int cancelRequestby_id { get; set; }
        public string cancelRequestByName { get; set; }
        public DateTime? cancel_at { get; set; }
        public int cancelRequestApprovedby_id { get; set; }
        public string cancelRequestApprovedByName { get; set; }
        public int cancelRequestRejectedby_id { get; set; }
        public string cancelRequestRejectedByName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<int> category_ids { get; set; }
        public List<CMCategory> LstCategory { get; set; }
        public Boolean is_isolation_required { get; set; }
        public List<int> isolated_category_ids { get; set; }
        public List<CMDefaultList> LstIsolationCategory { get; set; }
        public List<CMSaftyQuestion> safety_question_list { get; set; }
        public List<CMFileDetail> file_list { get; set; }
        public List<CMLoto> Loto_list { get; set; }
        public List<CMEMPLIST> employee_list { get; set; }
        public List<CMIsolationList> LstIsolation { get; set; }
        public List<CMAssociatedList> LstAssociatedJobs { get; set; }
        public int ptwStatus { get; set; }
        public string current_status_short { get; set; }
        public string current_status_long { get; set; }
    }
    public class CMAssociatedList
    {
        public int jobId { get; set; }
        public int permitId { get; set; }
        public string title { get; set; }
        public string equipmentCat { get; set; }
        public string equipment { get; set; }
        public dynamic breakdownTime { get; set; }
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
    }

    public class CMPermitExtend
    {
        public int id { get; set; }
        public int extend_by_minutes { get; set; }
        public string comment { get; set; }
        public int[] conditionIds { get; set; }
        public int fileId { get; set; }

    }

    public class CMPermitApproval
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int[] conditionIds { get; set; }
        public int fileId { get; set; }
        //public string filePath { get; set; }
    }

}