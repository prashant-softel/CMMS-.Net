using System;
using System.Collections.Generic;
namespace CMMSAPIs.Models.Utils
{
    public class CMApproval
    {
        public int id { get; set; }
        public string commnet { get; set; }
        public int status { get; set; }
        public int employee_id { get; set; }
        public int Time { get; set; }
        public DateTime Date{ get; set; }

    }
}
