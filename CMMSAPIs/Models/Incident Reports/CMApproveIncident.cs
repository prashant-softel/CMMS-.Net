﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMApproveIncident
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int is_why_why_required { get; set; }
        public int is_investigation_required { get; set; }
    }
}
