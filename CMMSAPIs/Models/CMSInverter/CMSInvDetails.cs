using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Inverter
{

    public class InverterDetails
    {
        public int InvId { get; set; }
        public string Invname { get; set; }
        public int status { get; set; }
        public double PR { get; set; }
        public double TodaysEnergy { get; set; }
        public double TotalEnergy { get; set; }
        public double Yield { get; set; }
        public double PRDeviation { get; set; }
       
    }
    public class InverterGraphData
    {
        public int InvId { get; set; }
        public string Invname { get; set; }
        public double PR { get; set; }
        public double Energy { get; set; }
        public double Yield { get; set; }
       
    }

}
