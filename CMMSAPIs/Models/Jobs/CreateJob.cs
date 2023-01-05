using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.Models.Jobs
{
    public class CreateJob
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int assigned_id { get; set; }
        public int permit_id { get; set; }
        public DateTime breakdown_time { get; set; }
        public DateTime job_create_Date { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }

        /*  public string facility_name { get; set; }
          public string block_name { get; set; }
          public string assigned_name { get; set; }
          public string permit_code { get; set; }*/

        public string cancellation_remark { get; set; }
        public List<int> JobType_Ids { get; set; }
        public List<CMAssetsIds> AssetsIds { get; set; }

    }
    public class CMAssetsIds
    {
        public int asset_id { get; set; }
        public int category_ids { get; set; }


    }
}