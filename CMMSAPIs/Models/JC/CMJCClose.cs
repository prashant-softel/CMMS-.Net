﻿using System.Collections.Generic;

namespace CMMSAPIs.Models.JC
{
    public class CMJCClose
    {
        public int id { get; set; }
        public int status { get; set; }
        public string comment { get; set; }
        public int employee_id { get; set; }
        public int lotoId { get; set; }
        public int isolationId { get; set; }
        public int normalisedStatus { get; set; }
        public int lotoStatus { get; set; }

        public List<int> uploadfile_ids { get; set; }

    }
}
