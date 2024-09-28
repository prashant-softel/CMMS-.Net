using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.WMS
{

    public class WMSDetails
    {
        public int WMSId { get; set; }
        public string WMSname { get; set; }
        public int PlantId { get; set; }
       
       
    }
    public class WMSData
    {
        public int WMSId { get; set; }
        public DateTime DateTime { get; set; }  // Timestamp for the data
        public double GHI { get; set; }
        public double POA { get; set; }
        public double ambTemp { get; set; }
        public double modTemp { get; set; }
        public double windSpeed { get; set; }
        public double windDirection { get; set; }
        public double dailyRain { get; set; }

    }
    public class WMSGraphData
    {
        public int WMSId { get; set; }
        public DateTime DateTime { get; set; }  // Timestamp for the data
        public string Month { get; set; }  // Timestamp for the data
        public double GHI { get; set; }
        public double GHIirradiance { get; set; }
        public double POAirradiance { get; set; }
        public double batteryVoltage { get; set; }

    }

}
