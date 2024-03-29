﻿using Microsoft.VisualBasic;
using System.Collections.Generic;
namespace CMMSAPIs.Models.JC
{
    public class CMJCList
    {
        public int id { get; set; }
        public int jobCardId { get; set; }
        public string jobCardNo { get; set; }
        public int jobid { get; set;  }
        public int permit_id { get; set; }
        public string permit_no { get; set; }
        public int current_status { get; set; }
        public string description { get; set; }
        public string job_assinged_to { get; set; }
        public dynamic job_card_date { get; set; }
        public dynamic start_time { get; set; }
        public dynamic end_time { get; set; }
        public int status { get; set; }
        public int approvedStatus { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public List<equipmentCatList> LstequipmentCatList { get; set; }
    }

    public class CMJCListForJob
    {
        public int jobCardId { get; set; }
        public string jobCardNo { get; set; }
        public int jobId { get; set; }
        public int permitId { get; set; }
        public string permitNo { get; set; }
        public string jobAssingedTo { get; set; }
        public dynamic jobCardDate { get; set; }
        public dynamic endTime { get; set; }
        public int status { get; set; }
        public int approvedStatus { get; set; }
        public string status_short { get; set; }


    }

}
public class equipmentCatList
{
    public int equipmentCat_id { get; set; }
    public string equipmentCat_name { get; set; }
}