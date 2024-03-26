using System;

namespace CMMSAPIs.Models.Grievance
{
    public class CMGrievance
    {
        public int id { get; set; }
        //public string name { get; set; }
        //public DateTime? added_at { get; set; }
        //public string added_by { get; set; }
        public int facilityId { get; set; }
        public int grievanceTypeId { get; set; }
        public string grievanceType { get; set; }
        public string concern { get; set; }
        public string description { get; set; }
        public string actionTaken { get; set; }
        public int resolutionLevel { get; set; }
        public DateTime? closedDate { get; set; }
        public string closedBy { get; set; }
        public string closedByName { get; set; }
        public int createdBy { get; set; }
        public string createdByName { get; set; }
        public dynamic createdAt { get; set; }
        public int updatedBy { get; set; }
        public string updatedByName{ get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime deletedAt { get; set; }
        public string deletedBy { get; set; }
        public string deletedByName { get; set; }
        public dynamic closedAt { get; set; }
        //        public int addedBy { get; set; }
        //      public DateTime? updated_at { get; set; }
        //       public string updated_by { get; set; }
        //       public DateTime? deleted_at { get; set; }
        //       public string deleted_by { get; set; }
        //      public string status_long { get; set; }
        public int statusId { get; set; }
        public string statusShort { get; set; }
        public string statusLong { get; set; }
        public int status { get; set; }
    }

    public class CMUpdateGrievance
    {
        public int id { get; set; }
        public int facilityId { get; set; }
        public int grievanceType { get; set; }
        //public string grievance { get; set; }
        public string concern { get; set; }
        public string description { get; set; }
        public string actionTaken { get; set; }
        public int resolutionLevel { get; set; }
        public int updatedBy { get; set; }
//        public DateTime? closedDate { get; set; }
 //       public DateTime? createdAt { get; set;}
 //       public int updatedBy { get; set; }

    }
}
