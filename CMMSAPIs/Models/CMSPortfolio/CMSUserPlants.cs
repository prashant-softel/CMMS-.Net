using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Portfolio
{
    public class CMSPlantList
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int PlantStatus { get; set; }
        public string Location { get; internal set; }
        public PlantDetails Details { get; set; }


    }
    public class PlantDetails
    {
        public double PR { get; set; } // Performance Ratio
        public double iradiance { get; set; }
        public double Capacity { get; set; }
        public double TodayEnergy { get; set; }
        public double TotalEnergy { get; set; }
        public double ActivePower { get; set; }
        public double PRDeviation { get; set; }
        public Int64 ActiveInverters { get; set; }
    }

    public class PlantData
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public string Location { get; internal set; }

        public int PlantStatus { get; set; }
        public DateTime TodayDate { get; set; }
        public string DateId { get; set; }
        public double PR { get; set; }
        public double Iradiance { get; set; }
        public double Capacity { get; set; }
        public double TodayEnergy { get; set; }
        public double TotalEnergy { get; set; }
        public double ActivePower { get; set; }
        public double PRDeviation { get; set; }
        public Int64 ActiveInverters { get; set; }
    }
}
     