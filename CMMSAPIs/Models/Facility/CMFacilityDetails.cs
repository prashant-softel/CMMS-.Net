using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Facility
{
    public class CMFacilityDetails
    {
        public int id { get; set; }
        public string parentName { get; set; }
        public string blockName { get; set; }
        public string spvName { get; set; }
        public string ownerName { get; set; }
        public int customerId { get; set; }
        public int ownerId { get; set; }
        public int operatorId { get; set; }
        public int isBlock { get; set; }
        public int parentId { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int zipcode { get; set; }
       // public string latitude { get; set; }
       // public string  longitude { get; set; }
        public int createdById { get; set; }
        public string createdByName { get; set; }
        public DateTime createdAt { get; set; }
        public int status { get; set; }
        public int photoId { get; set; }
        public string description { get; set; }
        public string timezone { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
}
