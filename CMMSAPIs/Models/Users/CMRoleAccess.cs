using System.Collections.Generic;

namespace CMMSAPIs.Models.Users
{
    public class CMRoleAccess
    {
        public int role_id { get; set; }
        public List<CMAccess> access_list { get; set; }
    }

    public class CMAccess 
    {
        public int feature_id { get; set; }
        public int add { get; set; }
        public int edit { get; set; }
        public int delete { get; set; }
        public int view { get; set; }
        public int issue { get; set; }
        public int approve { get; set; }
        public int selfView { get; set; }
    }
}
