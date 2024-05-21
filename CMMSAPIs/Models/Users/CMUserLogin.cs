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
        public int signatureId  { get; set; }
        public string signaturePath { get; set; }
        public  dynamic createdAt { get; set; }
        public dynamic updatedAt { get; set; }
        public List<CMPlant> Facilities { get; set; }
        
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
        public int company_id { get; set; }
        public int designation_id { get; set; }
        public string designation { get; set; }

        public string company_name { get; set; }
        public int blood_group_id { get; set; }
        public string blood_group_name { get; set; }
        public CMUser report_to { get; set; }
        public List<CMPlantAccess> plant_list { get; set; }
        public List<CMResposibility> responsibility { get; set; }

    }

    public class CMResposibility
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string responsibility { get; set; }
        public DateTime since_when { get; set; }

    }
    public class CMUpdateUser : CMUserDetail
    {
        public List<CMUserPlant> facility_list { get; set; }
        public CMUserCrentials credentials { get; set; }
        public int report_to_id { get; set; }
        public bool flagUpdateAccess { get; set; }
        public bool flagUpdateNotifications { get; set; }
        public List<CMUserResponsibilityList> user_responsibility_list { get; set; }
    }
    public class CMCreateUser : CMUserDetail
    {
        
        public List<CMUserPlant> facility_list { get; set; }
        public CMUserCrentials credentials { get; set; }
        public int report_to_id { get; set; }
        public List<CMAccessList> access_list { get; set; }
        public List<CMNotificationList> notification_list { get; set; }
        public List<CMUserResponsibilityList> user_responsibility_list { get; set; }
        public List<int> facilitiesid { get; set; }

    }
    public class CMUserResponsibilityList
    {
        public int responsibility_id { get; set; }
        public string responsibility { get; set; }
        public DateTime since_when { get; set; }
    }
    public class CMPlantAccess
    {
        public int id { get; set; }
        public string name { get; set; }
        public int spv_id { get; set; }
        public dynamic isEmployees { get; set; }
        public string spv { get; set; }
        public string location { get; set; }
    }
    public class CMCompetency
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; } 
    }
    public class CMPlant
    {
        public string name { get; set; }
    }
    public class CMUserPlant
    {
        public  int id { get; set; }
        public string name { get; set; }
        public string  address { get; set; }
        public bool isEmployees { get; set; }
    }
}
