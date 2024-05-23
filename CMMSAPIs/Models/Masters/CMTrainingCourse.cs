using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMTrainingCourse
    {

        public int id { get; set; }
        public int facility_id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public int group_id { get; set; }
        public int number_of_days { get; set; }
        public int duration { get; set; }
        public int max_cap { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public int UpdateBy { get; set; }

        public List<int> uploadfile_ids { get; set; }

    }

}
