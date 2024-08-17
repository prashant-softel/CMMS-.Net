using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMUpdateJobWorkTypeTool
    {
        //public string toolName { get; set; }
        // public string workTypeName { get; set; }
        // public string CategoryName { get; set; }
        public int id { get; set; }
        public List<CMToolIds1> ToolIds { get; set; }
    }
    public class CMToolIds1
    {
        public int toolId { get; set; }
        public int workTypeId { get; set; }
    }
}
    

