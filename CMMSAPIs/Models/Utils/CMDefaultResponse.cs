using System;

namespace CMMSAPIs.Models.Utils
{
    public class CMDefaultResponse
    {
        public int id { get; set; }
        public int status_code { get; set; }

        public CMDefaultResponse(int _id, int _status_code)
        {
            id = _id;
            status_code = _status_code;
        }
    }

}
