using CMMSAPIs.Models.Utils;
using System.Collections.Generic;

namespace CMMSAPIs.Models.WC
{
    public class CMWCDetail : CMWCCreate
    {
        public string status { get; set; }
        public List<CMLog> log { get; set; }
    }
}
