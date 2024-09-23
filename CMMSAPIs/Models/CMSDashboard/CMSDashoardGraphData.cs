using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Dashboard
{
    public class PowerGraphData
    {
        public int PlantId { get; set; }
        public DateTime Timestamp { get; set; }
        public double ACPower { get; set; }
        public double POA { get; set; }
        public double PR { get; set; }
        public double ModuleTemp { get; set; }
    }
    public class EnergyGraphData
    {
        public int PlantId { get; set; }
        public DateTime Timestamp { get; set; }
        public double MeasuredEnergy { get; set; }
        public double EstimatedEnergy { get; set; }
        public double MeasuredPOA { get; set; }
        public double EstimatedPOA { get; set; }
      
    }
    public class WeatherGraphData
    {
        public int PlantId { get; set; }
        public DateTime Timestamp { get; set; }
        public double GHI { get; set; }
        public double POA { get; set; }
        public double Mod_Temp_1 { get; set; }
        public double Amb_Temp { get; set; }
      
    }
}
