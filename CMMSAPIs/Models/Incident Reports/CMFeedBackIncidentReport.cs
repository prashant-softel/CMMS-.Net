using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Incident_Reports
{
    public class CMFeedBackIncidentReport
    {
        public int id { get; set; }
        public string description_of_action_taken { get; set; }
        public List<IFormFile> attachments { get; set; }
    }
}
