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

        public UserToken(string _token) 
        {
            token = _token;
        }
    }
}
