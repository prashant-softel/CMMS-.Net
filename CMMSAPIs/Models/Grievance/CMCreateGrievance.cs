using iTextSharp.text.pdf;
using System;

namespace CMMSAPIs.Models.Grievance
{
    public class CMCreateGrievance
    {
        internal int id;

        public int facilityId {  get; set; }
        public int grievanceType { get; set; }
        public string grievance { get; set; }
        public string concern { get; set; }
        public string actionTaken { get; set; }
        public int resolutionLevel { get; set; }
        public DateTime? closedDate { get; set; }
        public int createdBy { get; set; }
        public string description { get; set; }
        public DateTime? updatedAt { get; set; }


    }
}
