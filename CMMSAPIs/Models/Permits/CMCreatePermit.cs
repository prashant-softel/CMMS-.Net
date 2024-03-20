using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Permits
{
    public class CMCreatePermit
    {
        public int facility_id { get; set; }
        public int blockId { get; set; }
        public int lotoId { get; set; }
        public int permitTypeId { get; set; }
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
        public Boolean is_grid_isolation_required { get; set; }
        public DateTime grid_start_datetime { get; set; }
        public DateTime grid_stop_datetime { get; set; }
        public string grid_remark { get; set; }
        public Boolean is_loto_required { get; set; }
        public List<int> isolated_category_ids { get; set; }
        //public List<CMPermitLotoList> Loto_list { get; set; }
        public List<CMLotoList> Loto_list { get; set; }

        public string loto_remark { get; set; }
        public Boolean is_physical_iso_required { get; set; }
        public int[] physical_iso_equip_ids { get; set; }
        public string physical_iso_remark { get; set; }
        public List<CMPermitEmpList> employee_list { get; set; }
        public List<CMPermitSaftyQueList> safety_question_list { get; set; }
        public List<int> uploadfile_ids { get; set; }
        public int TBT_Done_By { get; set; }
        public DateTime? TBT_Done_At { get; set; }
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
    public class CMLotoList
    {
        public int equipment_id { get; set; }
        public string Loto_Key { get; set; }
        public int Loto_lock_number { get; set; }
        public int employee_id { get; set; }
    }

    public class CMPermitEmpList
    {
        public int employeeId { get; set; }
        public string responsibility { get; set; }
    }
}
