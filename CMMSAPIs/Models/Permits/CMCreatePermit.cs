using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Permits
{
    public class CMCreatePermit
    {
        public int facility_id { get; set; }
        public int blockId { get; set; }
        //public int lotoId { get; set; }
        public int typeId { get; set; }
        public DateTime start_datetime { get; set; }
        public DateTime end_datetime { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int job_type_id { get; set; }
        public int sop_type_id { get; set; }
        public int issuer_id { get; set; }
        public int approver_id { get; set; }
        //public int user_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        //public List<int> block_ids { get; set; }
        public List<int> category_ids { get; set; }
        public Boolean is_isolation_required { get; set; }
        public List<int> isolated_category_ids { get; set; }
        public List<CMPermitLotoList> Loto_list { get; set; }
        public List<CMPermitEmpList> employee_list { get; set; }
        public List<CMPermitSaftyQueList> safety_question_list { get; set; }
    }
    /*public class CMPermitLotoList
    {
        public int Loto_id { get; set; }
        public int category_ids { get; set; }

    }*/
    public class CMPermitSaftyQueList
    {
        public int safetyMeasureId { get; set; }
        public string safetyMeasureValue { get; set; }
    }
    public class CMPermitLotoList
    {
        public int Loto_id { get; set; }
        public string Loto_Key { get; set; }
    }
    public class CMPermitEmpList
    {
        public int employeeId { get; set; }
        public string responsibility { get; set; }
    }
}
