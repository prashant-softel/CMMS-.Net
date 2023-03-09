using CMMSAPIs.Helper;
using System.Collections.Generic;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Models.Users
{
    public class CMUserAccess
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public List<CMAccessList> access_list { get; set; }
    }
        
    public class CMAccessList
    {
        public int feature_id { get; set; }
        public string feature_name { get; set; }
        public int add { get; set; }
        public int edit { get; set; }
        public int delete { get; set; }
        public int view { get; set; }
        public int issue { get; set; }
        public int approve { get; set; }
        public int selfView { get; set; }
    }

    public class CMUserID
    {
        public int id { get; set; }
    }

    public class CMUserNotifications
    {
       public int user_id { get; set; }
       public string user_name { get; set; }
       public List<CMNotificationList> notification_list { get; set; }
    }

    public class CMNotificationList 
    {
        public int notification_id { get; set; }
        public string notification_name { get; set; }
        public int can_change { get; set; }
        public int flag { get; set; }
    }

    public class CMUserByNotificationId
    {
        public int facility_id { get; set; }
        public CMMS.CMMS_Status notification_id { get; set; }
        public List<int> user_ids { get; set; }
    }
}
