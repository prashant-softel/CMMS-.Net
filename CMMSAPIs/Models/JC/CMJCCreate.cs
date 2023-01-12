using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.JC
{
    public class CMJCCreate
    {
        public int job_id { get; set; }
        public int jobid { get; set; }
        public int ptw_id { get; set; }
        public int JC_Start_By_id { get; set; }
        public int JC_End_By_id { get; set; }
        public int JC_Issued_By_id { get; set; }
        public int JC_Approved_By_id { get; set; }
        public List<CMEmpDetailsList> LstEmpDetails { get; set; }

    }
   
    public class CMEmpDetailsList
    {
        public int employeeId { get; set; }
        public string responsibility { get; set; }
    }
}


