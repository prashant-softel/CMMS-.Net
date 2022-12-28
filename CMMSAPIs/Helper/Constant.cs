﻿namespace CMMSAPIs.Helper
{
    public class Constant
    {
        public const int TOKEN_EXPIRATION_TIME = 30;

        // Possible actions in each modules
        public const int CREATED = 1, UPDATED = 2, DELETED = 3, CANCELLED = 4, ASSIGNED = 5, ISSUED = 6, APPROVED = 7, REJECTED = 8;

        // Module Prefix
        public const string PREFIX_JOB = "JOB", PREFIX_PERMIT = "PERMIT", PREFIX_JC = "JC";

        public const int JOB = 1, PERMIT = 2, JOBCARD = 3;
        
    }
}
