
using CMMSAPIs.Helper;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Utils
{
    public class CMDefaultResponse
    {
        public List<int> id { get; set; }
        public CMMS.RETRUNSTATUS return_status;
        public string message { get; set; }
       //public List<CMJobDetailsList> LstJobDetails { get; set; }
       //public List<CMEmpDetailsList> LstEmpDetails { get; set; }
        public CMDefaultResponse(int id,  CMMS.RETRUNSTATUS return_status, string message)
        {
            this.id = new List<int>();
            this.id.Add(id);
            this.return_status = return_status;
            this.message = message;
        }
        public CMDefaultResponse(List<int> id, CMMS.RETRUNSTATUS return_status, string message)
        {
            this.id = id;
            this.return_status = return_status;
            this.message = message;
        }
        public CMDefaultResponse()
        {
        }
    }
   

    }
