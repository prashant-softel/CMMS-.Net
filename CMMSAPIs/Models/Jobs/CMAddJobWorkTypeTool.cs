using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMAddJobWorkTypeTool
    {
        public List<CMToolIds> ToolIds { get; set; }
    }
    public class CMToolIds
    {
        public int toolId { get; set; }
        public int workTypeId { get; set; }
    }

}
