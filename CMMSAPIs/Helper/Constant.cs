using System.Collections.Generic;

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

        /* Features Constant START */

        // BreakDown Maintenance
        public const int JOB = 1, PTW = 2, JOBCARD = 3;

        // Preventive Maintenance
        public const int PM_CHECKLIST_NUMBER = 4, PM_CHECKPOINTS = 5, PM_MAPPING = 6, PM_SCHEDULE = 7, PM_SCEHDULE_VIEW = 8, PM_EXECUTION = 9, PM_SCHEDULE_REPORT = 10, PM_SUMMARY = 11;

        // Stock Management
        public const int SM_MASTER = 12, SM_PO = 13, SM_MRS = 14, SM_MRS_RETURN = 15, SM_S2S = 16;

        // Module Cleaning
        public const int MODULE_CLEANING = 17;

        // Warranty Claim
        public const int WARRANTY_CLAIM = 18;

        // Vegetation
        public const int VEGETATION = 19;

        // Corrective Maintenance
        public const int CORRECTIVE_MAINTENANCE = 20;

        // Audit
        public const int AUDIT_PLAN = 21, AUDIT_CHECKLIST_NUMBER = 22, AUDIT_CHECKPOINTS = 23, AUDIT_MAPPING = 24, AUDIT_SCHEDULE = 25, AUDIT_SCEHDULE_VIEW = 26, AUDIT_EXECUTION = 27, AUDIT_SUMMARY = 28;

        // HOTO
        public const int HOTO_PLAN = 29, HOTO_CHECKLIST_NUMBER = 30, HOTO_CHECKPOINTS = 31, HOTO_MAPPING = 32, HOTO_SCHEDULE = 33, HOTO_SCEHDULE_VIEW = 34, HOTO_EXECUTION = 35, HOTO_SUMMARY = 36;

        // Calibration 
        public const int CALIBRATION = 37;

        // Plant
        public const int PLANT = 38;

        // Users
        public const int USER = 39, USER_NOTIFICATIONS = 40, ROLE_DEFAULT_NOTIFICATIONS = 41, ROLE_DEFAULT_ACCESS_MODULE = 50;

        // Incident Report
        public const int INCIDENT = 51;

        enum INCIDENT_RISK_LEVEL
        {
            HIGH = 1,
            MEDIUM = 2,
            LOW = 3
        }

        public Dictionary<string, int> INCIDENT_RISK_TYPE =  new Dictionary<string, int>() { {"First-Aids Injury", 1 }, { "Electric Short", 2} };
        

        /* Features Constant END */

        /* Feature Status */

        // Job
        public const int JOB_CREATED = 1, JOB_ASSIGNED = 2, JOB_LINKED = 3, JOB_IN_PROGRESS = 4, JOB_CANCELLED = 5;

        // Permit
        public const int PTW_CREATED = 1, PTW_ISSUED = 2, PTW_REJECTED_BY_ISSUER = 3, PTW_APPROVE = 4, PTW_REJECTED_BY_APPROVER = 5,
                         PTW_CLOSED = 6, PTW_CANCELLED_BY_ISSUER = 7, PTW_CANCELLLED_BY_HSE = 8, PTW_CANCELLED_BY_APPROVER = 9,
                         PTW_EDIT = 10, PTW_EXTEND_REQUESTED = 11, PTW_EXTEND_REQUEST_APPROVE = 12, PTW_EXTEND_REQUEST_REJECTED = 13,
                         PTW_LINKED_TO_JOB = 14, PTW_LINKED_TO_PM = 15, PTW_LINKED_TO_AUDIT = 16, PTW_LINKED_TO_HOTO = 17, PTW_EXPIRED = 18;

        // JOBCARD
        public const int JC_OPENED = 0, JC_UPDADATED = 1, JC_CLOSED = 2, JC_CARRRY_FORWARDED = 3, JC_APPROVED = 4, JC_REJECTED = 5, JC_PTW_TIMED_OUT = 6;       
    }
}
