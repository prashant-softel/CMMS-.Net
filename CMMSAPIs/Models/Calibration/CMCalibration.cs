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
        public string category_name { get; set; }
        public DateTime last_calibration_date { get; set; }
        public string vendor_name { get; set; }
        public string responsible_person { get; set; }
        public DateTime received_date { get; set; }
        public string asset_heath_status { get; set; }
    }

    public class CMRequestCalibration
    {
        public int asset_id { get; set; }
        public int vendor_id { get; set; }
        public DateTime next_calibration_date { get; set; }
    }

    public class CMPreviousCalibration : CMRequestCalibration
    {
        
    }

    public class CMCompleteCalibration
    {
        public int calibration_id { get; set; }
        public DateTime done_date { get; set; }
        public DateTime received_date { get; set; }
        public string comment { get; set; }
        public string is_damaged { get; set; }
    }

    public class CMCloseCalibration 
    {
        public int calibration_id { get; set; }
        public string comment { get; set; }
    }
}
