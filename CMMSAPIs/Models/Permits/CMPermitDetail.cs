using System;
using System.Collections.Generic;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.Permits
{
    public class CMPermitDetail
    {
        public DateTime startDate { get; set; }
        public DateTime tillDate { get; set; }
        public int ptwStatus { get; set; }
        public string siteName { get; set; }
        public string BlockName { get; set; }
        public int permitNo { get; set; }
        public int sitePermitNo { get; set; }
        public int permitTypeid { get; set; }
        public string PermitTypeName { get; set; }
        public string equipmentCat { get; set; }
        public string permitArea { get; set; }
        public int workingTime { get; set; }
        public string description { get; set; }
        public string issuedByName { get; set; }
        public DateTime issue_at { get; set; }
        public string approvedByName { get; set; }
        public DateTime approve_at { get; set; }
        public string completedByName { get; set; }
        public DateTime close_at { get; set; }
        public string cancelRequestByName { get; set; }
        public DateTime cancel_at { get; set; }
        public List<CMSaftyQuestion> safety_question_list { get; set; }
        public List<CMFileDetail> file_list { get; set; }
        public List<CMLoto> LstLoto { get; set; }
        public List<CMEMPLIST> LstEmp { get; set; }
        public List<CMIsolationList> LstIsolation { get; set; }
        public List<CMCategory> LstCategory { get; set; }
        public List<CMAssociatedList> LstAssociatedJob { get; set; }
    }
    public class CMAssociatedList
    {
        public int JobId { get; set; }
        public int JobCardId { get; set; }
        public string JobTitle { get; set; }
        public string JobDes { get; set; }
        public DateTime JobDate { get; set; }
        public int JobStatus { get; set; }
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

}