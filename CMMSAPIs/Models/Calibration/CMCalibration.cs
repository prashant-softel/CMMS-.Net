using System;

namespace CMMSAPIs.Models.Calibration
{
    public class CMCalibration
    {
    }

    public class CMCalibrationList
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public string asset_serial { get; set; }
        public string category_name { get; set; }
        public string calibration_status { get; set; }
        public DateTime last_calibration_date { get; set; }
        public string vendor_name { get; set; }
        public string responsible_person { get; set; }
        public DateTime received_date { get; set; }
        public string asset_health_status { get; set; }
    }

    public class CMRequestCalibration
    {
        public int asset_id { get; set; }
        public int vendor_id { get; set; }
        public DateTime next_calibration_date { get; set; }
    }

    public class CMPreviousCalibration
    {
        public int asset_id { get; set; }
        public int vendor_id { get; set; }
        public DateTime previous_calibration_date { get; set; }
    }

    public class CMCompleteCalibration
    {
        public int calibration_id { get; set; }
        public string comment { get; set; }
        public int? is_damaged { get; set; }
    }

    public class CMCloseCalibration 
    {
        public int calibration_id { get; set; }
        public string comment { get; set; }
    }
}
