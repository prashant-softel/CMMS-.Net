using System;

namespace CMMSAPIs.Models.PM
{
    public class CMScheduleData
    {
        public int id { get; set; }
        public string PM_Schedule_Code { get; set; }
        public string PM_Schedule_Name { get; set; }
        public dynamic PM_Schedule_date { get; set; }
        public int checklist_type { get; set; }
        public int plan_id { get; set; }
        public string PM_Frequecy_Name { get; set; }
        public int PM_Frequecy_id { get; set; }
        public string PM_Frequecy_Code { get; set; }
        public int Facility_id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Code { get; set; }
        public int Asset_Category_id { get; set; }
        public string Asset_Category_Code { get; set; }
        public string Asset_Category_name { get; set; }
        public int Asset_id { get; set; }
        public string Asset_Code { get; set; }
        public string Asset_Name { get; set; }
        public string Asset_Sno { get; set; }
        public string PM_Schedule_Number { get; set; }
        public string PM_Maintenance_Order_Number { get; set; }
        public int PM_Schedule_User_id { get; set; }
        public string PM_Schedule_User_Name { get; set; }
        public int PM_Schedule_Emp_id { get; set; }
        public string PM_Schedule_Emp_name { get; set; }
        public dynamic PM_Schedule_created_date { get; set; }
        public int PM_Schedule_Start_Flag { get; set; }
        public int PM_Schedule_issued_by_id { get; set; }
        public string PM_Schedule_issued_by_Code { get; set; }
        public string PM_Schedule_issued_by_name { get; set; }
        public int PM_Schedule_issued_requested_to_id { get; set; }
        public string PM_Schedule_issued_requested_to_Code { get; set; }
        public string PM_Schedule_issued_requested_to_Name { get; set; }
        public int PM_Schedule_issued_status { get; set; }
        public string PM_Schedule_issued_Reccomendations { get; set; }
        public dynamic PM_Schedule_issued_date { get; set; }
        public int PM_Schedule_accepted_by_id { get; set; }
        public string PM_Schedule_accepted_by_Code { get; set; }
        public int PM_Schedule_accepted_status { get; set; }
        public dynamic PM_Schedule_accepted_date { get; set; }
        public string PM_Schedule_accepted_by_name { get; set; }
        public string PM_Schedule_accepted_requested_to_name { get; set; }
        public int PM_Schedule_accepted_requested_to_id { get; set; }
        public int PM_Schedule_Approved_by_id { get; set; }
        public int PM_Schedule_Approved_Status { get; set; }
        public dynamic PM_Schedule_Approved_date { get; set; }
        public string PM_Schedule_Reccomendations_by_Approver { get; set; }
        public string PM_Schedule_Approved_by_name { get; set; }
        public string PM_Schedule_Approved_by_Code { get; set; }
        public string PM_Schedule_Approve_requested_to_name { get; set; }
        public int PM_Schedule_Approve_requested_to_id { get; set; }
        public string PM_Schedule_Approve_requested_to_Code { get; set; }
        public int PM_Schedule_final_Signature { get; set; }
        public int Status { get; set; }
        public dynamic PM_Schedule_Completed_date { get; set; }
        public int PM_Schedule_Completed_Status { get; set; }
        public int PM_Schedule_Completed_by_id { get; set; }
        public string PM_Schedule_Completed_by_Name { get; set; }
        public string PM_Schedule_Completed_by_Code { get; set; }
        public int PTW_id { get; set; }
        public string PTW_Code { get; set; }
        public string PTW_Ttitle { get; set; }
        public int PTW_by_id { get; set; }
        public int PTW_Status { get; set; }
        public dynamic PTW_Attached_At { get; set; }
        public int Job_Card_Status { get; set; }
        public dynamic Job_Card_date { get; set; }
        public int Job_Card_id { get; set; }
        public string Job_Card_Name { get; set; }
        public string Job_Card_Code { get; set; }
        public int PM_Schedule_cancel_Request_by_id { get; set; }
        public int PM_Schedule_cancel_Request_status { get; set; }
        public string PM_Schedule_cancel_Request_by_name { get; set; }
        public string PM_Schedule_cancel_Request_by_Code { get; set; }
        public dynamic PM_Schedule_cancel_Request_date { get; set; }
        public int PM_Schedule_cancel_Request_approve_by_id { get; set; }
        public string PM_Schedule_cancel_Request_approve_by_Name { get; set; }
        public string PM_Schedule_cancel_Request_approve_by_Code { get; set; }
        public string PM_Schedule_cancel_Request_approve_date { get; set; }
        public int PM_Schedule_cancel_Request_approve_status { get; set; }
        public string PM_Schedule_cancel_Reccomendations { get; set; }
        public double PM_Schedule_lat { get; set; }
        public double PM_Schedule_long { get; set; }
        public string PM_Schedule_ip { get; set; }
        public string PM_Schedule_UA { get; set; }
        public dynamic PTW_Complete_Date { get; set; }
        public int PM_Rescheduled { get; set; }
        public int Prev_Schedule_id { get; set; }
    }

    public class CMSetScheduleData 
    {
        public DateTime schedule_date { get; set; }
        public int facility_id { get; set; }
        public int frequency_id { get; set; }
        public int asset_id { get; set; }
    }

}
