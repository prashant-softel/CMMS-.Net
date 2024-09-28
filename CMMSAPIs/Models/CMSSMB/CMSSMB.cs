using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.SMB
{

    public class SMBs
    {
        public int SMBId { get; set; }
        public string SMBname { get; set; }
        public int PlantId { get; set; }
       
       
    }
    public class SMBDetails
    {
        public int SMBId { get; set; }
        public string SMBName { get; set; }
        public int invId { get; set; }
        public string invName { get; set; }
        public DateTime DateTime { get; set; }  // Timestamp for the data
        public double SMB_temprature { get; set; }
        public double SMB_voltage { get; set; }
        public double SMB_current_total { get; set; }
        public double SMB_status { get; set; }

    }


}
