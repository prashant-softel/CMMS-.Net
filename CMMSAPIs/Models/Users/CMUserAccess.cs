using System.Collections.Generic;

namespace CMMSAPIs.Models.Users
{
    public class CMUserAccess
    {
        public int user_id { get; set; }
        public int feature_id { get; set; }
        public int add { get; set; }
        public int edit { get; set; }
        public int delete { get; set; }
        public int view { get; set; }
        public int issue { get; set; }
        public int approve { get; set; }
        public int selfView { get; set; }
    }

    public class CMUserNotifications
    {
       public int user_id { get; set; }
       public int notification_id { get; set; }
       public int can_change { get; set; }
       public int user_preference { get; set; }

    }
}
