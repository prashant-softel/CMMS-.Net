using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMCreateIncidentReport
    {
        public int facility_id { get; set; }
        public int block_id { get; set; }
        public int reporting_type { get; set; }
        public int incident_type { get; set; }
        public int risk_level { get; set; }
        public DateTime incident_datetime { get; set; }
        public int equipment_id { get; set; }
        public string observation { get; set; }
        public List<IFormFile> attachment { get; set; }
    }
}
