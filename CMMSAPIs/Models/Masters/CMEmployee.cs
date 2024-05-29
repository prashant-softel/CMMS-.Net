using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Masters
{

    public class CMEmployee
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
        public int[] responsibilityIds { get; set; }
        public string designation { get; set; }
        public dynamic experince { get; set; }
        public List<CMResposibility> responsibility { get; set; }

    }

    public class CMResposibility
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime since { get; set; }
        public int experianceYears { get; set; }

    }

}
