
using System.Collections.Generic;

namespace CMMSAPIs.Models.Utils
{
    public class CMDefaultResponse
    {
        public int id { get; set; }
        public int status_code { get; set; }
        public string message { get; set; }
        public List<CMJobDetailsList> LstJobDetails { get; set; }
        public List<CMEmpDetailsList> LstEmpDetails { get; set; }
        public CMDefaultResponse(int id, int status_code, string message)
        {
            this.id = id;
            this.status_code = status_code;
            this.message = message;
        }
    }
    public class CMJobDetailsList
    {
        public int jobid { get; set; }
        public int ptw_id { get; set; }
        public int JC_Start_By_id { get; set; }      
        public int JC_End_By_id { get; set; }
        public int JC_Issued_By_id { get; set; }
        public int JC_Approved_By_id { get; set; }
    }
    public class CMEmpDetailsList
    {
        public int employeeId { get; set; }
        public string responsibility { get; set; }
    }


    }
