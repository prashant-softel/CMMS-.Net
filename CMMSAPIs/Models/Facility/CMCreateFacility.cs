using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Facility
{
    public class CMCreateFacility
    {
        public int id { get; set; }
        public string name { get; set; }
        public int customerId { get; set; }
        public int ownerId { get; set; }
        public int operatorId { get; set; }
        public string address { get; set; }
        public int cityId { get; set; }
        public int stateId { get; set; }
        public int countryId { get; set; }
        public int zipcode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int photoId { get; set; }
        public string description { get; set; }
        public string timezone { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

    }
    public class CMCreateBlock
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public int photoId { get; set; }
        public string description { get; set; }
    }
}
