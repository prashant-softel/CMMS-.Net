using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{
    public class CMTrainingCourse
    {

        public int Id { get; set; }
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
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string Traning_category { get; set; }
        public string Targated_group { get; set; }
        public List<int> uploadfile_ids { get; set; }
        public List<CMTRAININGFILE> ImageDetails { get; set; }


    }
    public class CMTRAININGCATE
    {
        public int id { get; set; }
        public string name { set; get; }
        public string description { get; set; }
        public int status { get; set; }
    }
    public class CMTRAININGFILE
    {
        public int id { get; set; }

        public string description { get; set; }
        public string fileName { get; set; }

    }

}
