using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Models.Users
{
    public class CMUserLogin
    {
        public string login_id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string user_role { get; set; }
    }

    public class UserToken
    {
        public string token { get; set; }
        public CMUserDetail user_detail { get; set; }

        public UserToken(string _token, CMUserDetail _user_detail) 
        {
            token = _token;
            user_detail = _user_detail;
        }
    }

    public class CMUser 
    {
        public int id { get; set; }
        public string full_name { get; set; }
        public string user_name { get; set; }
        public string role_name { get; set; }
        public string contanct_no { get; set; }        
    }

    public class CMUserDetail : CMUser
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int gender_id { get; set; }
        public string gender_name { get; set; }
        public string DOB { get; set; }
        public string country_name { get; set; }
        public int country_id { get; set; }
        public string state_id { get; set; }
        public string state_name { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public List<KeyValuePairs> plant_list { get; set; }
        public CMUserAccess user_access { get; set; }
        public CMUserNotifications user_notification { get; set; }
    }
}
