using System;

namespace CMMSAPIs.Models.Utils
{
    public class CMDefaultResponse
    {
        public int id { get; set; }
        public int status_code { get; set; }
        public string message { get; set; }

        public CMDefaultResponse(int id, int status_code, string message)
        {
            this.id = id;
            this.status_code = status_code;
            this.message = message;
        }
    }

}
