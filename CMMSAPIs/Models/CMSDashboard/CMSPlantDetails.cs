using System;
using System.Collections.Generic;
using CMMSAPIs.Models.Inverter;

namespace CMMSAPIs.Models.Dashboard
{
    public class PlantPerformanceDetails
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public double TodaysExport { get; set; }       // MWh
        public double TodaysImport { get; set; }       // MWh
        public double PeakPower { get; set; }          // MW
        public double ActivePower { get; set; }        // MW
        public double TotalIradiance { get; set; }     // kWh/m²
        public double ForeIradiance { get; set; }      // kWh/m²
        public double HMIIradiance { get; set; }       // kWh/m²                                              
        public double SpecificYield { get; set; }      // Yield

        public double PlantAvailability { get; set; }  // %
        public double GridAvailability { get; set; }   // %
        public double PerformanceRatio { get; set; }   // %
        public double CUF { get; set; }                // %

        public double EstimatedPR { get; set; }        // %
        public double EstimatedPRMonthly { get; set; } // %
        public double WeatherCorrectedPR { get; set; } // %
        public double ContractualPR { get; set; }      // %
        public double GuaranteedPRYearly { get; set; } // %

        public Int64 ActiveInverters { get; set; }
        public Int64 TotalInverters { get; set; }
        public List<InverterDetails> Inverters { get; set; }

    }


    

}
