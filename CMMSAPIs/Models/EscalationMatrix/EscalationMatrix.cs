using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.EscalationMatrix
{
    public class EscalationMatrixModel
    {
        public int SrNo { get; set; }
        public string Module{ get; set; }
        public string Status{ get; set; }
        public List<EscalationLevelwithDays> EscalationLevelList { get; set; }
  
        public int createdBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime? updatedAt { get; set; }
        public int isActive { get; set; }
        public int isDone { get; set; }
        public long DayDifference { get; set; }
        public long NoOfDays { get; set; }
        public long Levels { get; set; }
    }

    public class EscalationLevelwithDays
    {
        public int Level { get; set; }
        public int NoOfDays { get; set; }
    }

}
