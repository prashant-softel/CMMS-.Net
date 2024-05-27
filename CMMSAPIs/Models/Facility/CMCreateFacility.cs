using System;

namespace CMMSAPIs.Models.Facility
{
    public class CMCreateFacility
    {
        public int id { get; set; }
        public string name { get; set; }
        public int spvId { get; set; }
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
    public class FacilityListEmployee
    {
        public int id { get; set; }
        public string login_id { get; set; }
        public string name { get; set; }
        public DateTime birthdate { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int pin { get; set; }

        public string designation { get; set; }

        public int[] responsibilityIds { get; set; }


    }
}
