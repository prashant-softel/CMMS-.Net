using CMMSAPIs.Models.Utils;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
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
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string contact_no { get; set; }
        public string status { get; set; }
        public int photoId { get; set; }
        public string photoPath { get; set; }
        public int signatureId { get; set; }
        public string signaturePath { get; set; }
    }

    public class CMUserDetail : CMUser
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string secondaryEmail { get; set; }
        public string landline_number { get; set; }
        public CMMS.Gender gender_id { get; set; }
        public string gender_name { get; set; }
        public DateTime? DOB { get; set; }
        public string country_name { get; set; }
        public int country_id { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int zipcode { get; set; }
        public int? isEmployee { get; set; }
        public DateTime? joiningDate { get; set; }
        public int blood_group_id { get; set; }
        public string blood_group_name { get; set; }
        public List<CMPlantAccess> plant_list { get; set; }
    }
    public class CMUpdateUser : CMUserDetail
    {
        public List<int> facilities { get; set; }
        public CMUserCrentials credentials { get; set; }
        public bool flagUpdateAccess { get; set; }
        public bool flagUpdateNotifications { get; set; }
    }
    public class CMCreateUser : CMUserDetail
    {
        public List<int> facilities { get; set; }
        public CMUserCrentials credentials { get; set; }
        public List<CMAccessList> access_list { get; set; }
        public List<CMNotificationList> notification_list { get; set; }
    }
    public class CMPlantAccess
    {
        public int plant_id { get; set; }
        public string plant_name { get; set; }
        public int spv_id { get; set; }
        public string spv_name { get; set; }
    }
    public class CMCompetency
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; } 
    }
}
