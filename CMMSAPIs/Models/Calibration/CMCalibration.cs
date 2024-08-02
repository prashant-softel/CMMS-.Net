using System;
using System.Collections.Generic;

namespace CMMSAPIs.Models.Calibration
{
    public class CMCalibration
    {
    }

    public class CMCalibrationList
    {
        public int calibration_id { get; set; }
        public int asset_id { get; set; }
        public int is_damaged { get; set; }
        public string asset_name { get; set; }
        public string asset_serial { get; set; }
        public string category_name { get; set; }
        public int statusID { get; set; }
        public string calibration_status { get; set; }
        public DateTime? last_calibration_date { get; set; }
        public DateTime? next_calibration_due_date { get; set; }
        public DateTime? calibration_due_date { get; set; }
        public DateTime Schedule_start_date { get; set; }
        public DateTime? last_calibration_due_date { get; set; }
        public DateTime? calibration_date { get; set; }
        public int frequency_id { get; set; }
        public string frequency_name { get; set; }
        public int vendor_id { get; set; }
        public string vendor_name { get; set; }
        public string responsible_person { get; set; }
        public DateTime? received_date { get; set; }
        public string asset_health_status { get; set; }
    }

    public class CMCalibrationDetails : CMCalibrationList
    {
        public DateTime? calibration_due_date { get; set; }
        public string calibration_certificate_path { get; set; }
        public string request_approved_by { get; set; }
        public string request_rejected_by { get; set; }
        public DateTime? request_approved_at { get; set; }
        public DateTime? request_rejected_at { get; set; }
        public string approved_by { get; set; }
        public string rejected_by { get; set; }
        public DateTime? started_at { get; set; }
        public string status_short { get; set; }
        public string status_long { get; set; }
        public DateTime? requested_at { get; set; }
        public int requested_by { get; set; }
        public DateTime? completed_at { get; set; }
        public string completed_by { get; set; }
        public DateTime? Closed_at { get; set; }
        public string Closed_by { get; set; }
        public int is_damaged { get; set; }
        public List<CMFileDetailCalibration> file_list { get; set; }
    }
    public class CMFileDetailCalibration
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public string fileCategory { get; set; }
        public double fileSize { get; set; }
        public int status { get; set; }
        public string description { get; set; }
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
        public string asset_name { get; set; }
        public int vendor_id { get; set; }
        public string vendor_name { get; set; }
        public DateTime? previous_calibration_date { get; set; }
    }

    public class CMCompleteCalibration
    {
        public int calibration_id { get; set; }
        public string comment { get; set; }
        public int is_damaged { get; set; }
        public List<int> uploaded_file_id { get; set; }
    }

    public class CMCloseCalibration
    {
        public int calibration_id { get; set; }
        public string comment { get; set; }
        public int? is_damaged { get; set; }
        public int calibration_certificate_file_id { get; set; }
    }
}
