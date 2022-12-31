namespace CMMSAPIs.Helper
{
    public class Constant
    {
        // Token expiration time
        public const int TOKEN_EXPIRATION_TIME = 30;

        // Possible actions in each modules
        public const int CREATED = 1, UPDATED = 2, DELETED = 3, CANCELLED = 4, ASSIGNED = 5, ISSUED = 6, APPROVED = 7, REJECTED = 8;

        // Module Prefix
        public const string PREFIX_JOB = "JOB", PREFIX_PERMIT = "PERMIT", PREFIX_JC = "JC";

        //Business Type
        public const int OWNER = 1, OPERATOR = 2, CUSTOMER = 3, MANUFACTURER = 4, SUPPLIER = 5;
        
    }
}
