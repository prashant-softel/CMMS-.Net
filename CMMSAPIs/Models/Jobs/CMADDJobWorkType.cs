using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Jobs
{
    public class CMADDJobWorkType
    {
        public int id { get; set; }
        public int categoryid { get; set; }
        public string workType { get; set; }
    }
    public class CMADDJobWorkTypeTool
    {
        public int id { get; set; }
        public string Toolname   { get; set; }
        public int workTypeId     { get; set; }
        public int equipmentCategoryId { get; set; }
        public int  status  { get; set; }
        public DateTime createdAt    { get; set; }
        public DateTime  createdBy    { get; set; }
        public int toolId { get; set; }
    }
}
