using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Masters
{
    public class CMAsset
    {

        public int id { get; set; }
        public int facilityId { get; set; }
        public int categoryId { get; set; }
        public string name { get; set; }
    }
}
