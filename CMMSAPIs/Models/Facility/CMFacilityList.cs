using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Facility
{
    public class CMFacilityList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int spv_id { get; set; }
        public string spv { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int pin { get; set; }
    }
}
