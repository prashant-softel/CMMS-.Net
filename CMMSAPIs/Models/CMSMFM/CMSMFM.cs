using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.MFM
{

    public class MFMDetails
    {
        public int MFMId { get; set; }
        public string MFMname { get; set; }
        public int PlantId { get; set; }
       
       
    }
    public class MFMGraphData
    {
        public DateTime DateTime { get; set; }  // Timestamp for the data
        public double ActivePower { get; set; }
        public double ReactivePower { get; set; }
        public double ApparentPower { get; set; }
        public double PowerFactor { get; set; }

        public double TodaysImport { get; set; }
        public double TotalImport { get; set; }
        public double ApparentExport { get; set; }
        public double TodaysExport { get; set; }
        public double TotalExport { get; set; }

        public double ACFreq { get; set; }

        // Voltage parameters
        //public double VR_PH { get; set; }    
        //public double VY_PH { get; set; }    
        //public double VB_PH { get; set; }   
        //public double VPH_AVG { get; set; }  =
        // public double VR_Y { get; set; }   
        //public double VY_B { get; set; }    
        //public double VB_R { get; set; }  
        //public double VLL_AVG { get; set; }  

        //// Current parameters
        //public double IR { get; set; }
        //public double IY { get; set; }   
        //public double IB { get; set; }   
        //public double IN { get; set; }  
        //public double I_AVG { get; set; }  
        //public double I_Total { get; set; }   
        //public double R_kW { get; set; }   
        //public double Y_kW { get; set; }   
        //public double B_kW { get; set; }  
        //public double AC_Freq { get; set; }  
        //public double Rkva { get; set; }

        //public double YkVA { get; set; }   
        //public double BkVA { get; set; }   
        //public double RkVAR { get; set; }  
        //public double YkVAR { get; set; }   
        //public double BkVAR { get; set; }   


        //public double PF_R { get; set; }  
        //public double PF_Y { get; set; }   
        //public double PF_B { get; set; } 

        // Power parameters

    }

}
