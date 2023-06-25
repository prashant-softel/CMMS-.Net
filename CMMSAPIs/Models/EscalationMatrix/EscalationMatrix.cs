using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.EscalationMatrix
{
    public class CMEscalationMatrixModel
    {
        public int Id { get; set; }
        public string Module{ get; set; }
        public string Status{ get; set; }
        public int Status_ID{ get; set; }
        public List<CMEscalationLevelwithDays> EscalationLevelList { get; set; }
        public List<CMEscalationMail> EscalationMail { get; set; }
  
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

    public class CMEscalationLevelwithDays
    {
        public int Level { get; set; }
        public int NoOfDays { get; set; }
    }

    public class CMEscalationMail
    {
        public int Level { get; set; }
        public string Role { get; set; }
        public string MailTo { get; set; }
    }

}
