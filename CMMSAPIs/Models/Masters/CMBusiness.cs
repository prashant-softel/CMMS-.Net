using System;

namespace CMMSAPIs.Models.Masters
{
    public class CMBusinessType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    public class CMBusiness
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int type { get; set; }
        public string typeName { get; set; }
        public string contactPerson { get; set; }
        public string contactNumber { get; set; }
        public string website { get; set; }
        public string location { get; set; }
        public string address { get; set; }
        public int cityId { get; set; }
        public string city { get; set; }
        public int stateId { get; set; }
        public string state { get; set; }
        public int countryId { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public DateTime addedAt { get; set; }
    }
}
