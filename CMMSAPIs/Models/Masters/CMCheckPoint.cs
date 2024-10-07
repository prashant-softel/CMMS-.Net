using System;

namespace CMMSAPIs.Models.Masters
{
    public class CMCreateCheckPoint
    {
        public int id { get; set; }
        public string check_point { get; set; }
        public int checklist_id { get; set; }
        public string requirement { get; set; }
        public DateTime schedule_date { get; set; }
        public int? is_document_required { get; set; }
        public string action_to_be_done { get; set; }
        public int? failure_weightage { get; set; } // in per
        public CMCPType? checkpoint_type { get; set; } // bool,renge
        public int? risk_type { get; set; }
        public int assign_to { get; set; }
        public int? cost_type { get; set; }
        public int? type_of_observation { get; set; }
        public int? type { get; set; }
        public int? status { get; set; }
    }

    public class CMCPType
    {
        public int id { get; set; } //1 = bool, 2 = renge, 0 = text;
        public int name { get; set; } //1 = bool, 2 = renge;
        public int value { get; set; }
        public int type { get; set; }//only required if type is 1 
        public dynamic min { get; set; } // only required if type is 2 
        public dynamic max { get; set; } // only required if type is 2 

    }
    public class CMCheckPointList
    {
        public int id { get; set; }
        public string check_point { get; set; }
        public int checklist_id { get; set; }
        public string checklist_name { get; set; }
        public string requirement { get; set; }
        public int? is_document_required { get; set; }
        public string action_to_be_done { get; set; }
        public int? failure_weightage { get; set; } // in per
        public string? checkpoint_type { get; set; } // bool,renge
        public double? min { get; set; } // bool,renge
        public double? max { get; set; } // bool,renge

        public string type_name { get; set; }
        public int created_by_id { get; set; }
        public string created_by_name { get; set; }
        public dynamic created_at { get; set; }
        public int updated_by_id { get; set; }
        public string updated_by_name { get; set; }
        public dynamic updated_at { get; set; }
        public int? status { get; set; }
        public int? type { get; set; }
        public string type_of_observation { get; set; }
        public string risk_type { get; set; }
        public int Cost_Type { get; set; }

        public string Cost_type_name { get; set; }
    }
}
