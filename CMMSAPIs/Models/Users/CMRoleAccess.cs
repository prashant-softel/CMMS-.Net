﻿using System.Collections.Generic;

namespace CMMSAPIs.Models.Users
{
    public class CMRoleAccess
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public List<CMAccessList> access_list { get; set; }
    }


    public class CMDesignation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }



    public class CMSetRoleAccess : CMRoleAccess
    {
        public bool set_existing_users { get; set; }
        public bool set_role { get; set; }
    }

    public class CMRoleNotifications
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public List<CMNotificationList> notification_list { get; set; }
    }

    public class CMSetRoleNotifications : CMRoleNotifications 
    {
        public bool set_existing_users { get; set; }
        public bool set_role { get; set; }
    }


}
