using System.Collections.Generic;
using System;

namespace CMMSAPIs.Helper
{
    public class CMMS
    {
        // Token expiration time
        public const int TOKEN_EXPIRATION_TIME = 30;

        public enum RETRUNSTATUS
        {
            SUCCESS = 0,
            FAILURE,
            INVALID_ARG
        }
        public enum Day
        {
            Sunday = 1,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday
        }

        public enum Month
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        public enum Month_Short
        {
            Jan = 1,
            Feb,
            Mar,
            Apr,
            May,
            Jun,
            Jul,
            Aug,
            Sep,
            Oct,
            Nov,
            Dec
        }


        // Possible actions in each modules
        //        public const int CREATED = 1, UPDATED = 2, DELETED = 3, CANCELLED = 4, ASSIGNED = 5, ISSUED = 6, APPROVED = 7, REJECTED = 8;
        /*
                public enum CMMS_Module_Action
                {
                    CREATED = 1,
                    UPDATED = 2,
                    DELETED = 3,
                    CANCELLED = 4,
                    ASSIGNED = 5,
                    ISSUED = 6,
                    APPROVED = 7,
                    REJECTED = 8
                }
        */
        // Module Prefix
        //        public const string PREFIX_JOB = "JOB", PREFIX_PERMIT = "PERMIT", PREFIX_JC = "JC";

        //Business Type
        //        public const int OWNER = 1, OPERATOR = 2, CUSTOMER = 3, MANUFACTURER = 4, SUPPLIER = 5;
        public enum CMMS_BusinessType
        {
            OWNER = 1,
            OPERATOR = 2,
            CUSTOMER = 3,
            MANUFACTURER = 4,
            SUPPLIER = 5,
            VENDOR=6,
        }

        public enum CMMS_Events
        {
            BEFORE = 1,
            DURING = 2,
            AFTER = 3
        }

        public enum CMMS_Input
        {
            Checkbox = 1,
            Radio,
            Text,
            OK
        }
        /* Features Constant START */

        /*
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

        // Invetory Assets
        public const int INVENTORY = 52;
        */
        public enum CMMS_Modules
        {
            DASHBOARD = 1,
            JOB,
            PTW,
            JOBCARD,

            CHECKLIST_NUMBER = 11,
            CHECKPOINTS,
            CHECKLIST_MAPPING,

            PM_SCHEDULE = 21,
            PM_SCEHDULE_VIEW,
            PM_EXECUTION,
            PM_SCHEDULE_REPORT,
            PM_SUMMARY,

            SM_MASTER = 31,
            SM_PO,
            SM_MRS,
            SM_MRS_RETURN,
            SM_S2S,

            AUDIT_PLAN = 41,
            AUDIT_SCHEDULE,
            AUDIT_SCEHDULE_VIEW,
            AUDIT_EXECUTION,
            AUDIT_SUMMARY,

            HOTO_PLAN = 61,
            HOTO_SCHEDULE,
            HOTO_SCEHDULE_VIEW,
            HOTO_EXECUTION,
            HOTO_SUMMARY,

            MODULE_CLEANING = 81,
            CORRECTIVE_MAINTENANCE = 91,
            CALIBRATION = 101,
            VEGETATION = 111,
            WARRANTY_CLAIM = 121,
            INCIDENT_REPORT = 131,
            INVENTORY = 141,
            PLANT = 151,

            USER = 171,
            USER_MODULE,
            USER_NOTIFICATIONS, 
            ROLE_DEFAULT_NOTIFICATIONS, 
            ROLE_DEFAULT_ACCESS_MODULE
        }

        enum INCIDENT_RISK_LEVEL
        {
            HIGH = 1,
            MEDIUM = 2,
            LOW = 3
        }

        public static Dictionary<string, int> INCIDENT_RISK_TYPE =  new Dictionary<string, int>() { {"First-Aids Injury", 1 }, { "Electric Short", 2} };

        /* Features Constant END */

        /* Feature Status */
        /*
                // Job
                public const int JOB_CREATED = 1, JOB_ASSIGNED = 2, JOB_LINKED = 3, JOB_IN_PROGRESS = 4, JOB_CANCELLED = 5;

                // Permit
                public const int PTW_CREATED = 1, PTW_ISSUED = 2, PTW_REJECTED_BY_ISSUER = 3, PTW_APPROVE = 4, PTW_REJECTED_BY_APPROVER = 5,
                                 PTW_CLOSED = 6, PTW_CANCELLED_BY_ISSUER = 7, PTW_CANCELLLED_BY_HSE = 8, PTW_CANCELLED_BY_APPROVER = 9,
                                 PTW_EDIT = 10, PTW_EXTEND_REQUESTED = 11, PTW_EXTEND_REQUEST_APPROVE = 12, PTW_EXTEND_REQUEST_REJECTED = 13,
                                 PTW_LINKED_TO_JOB = 14, PTW_LINKED_TO_PM = 15, PTW_LINKED_TO_AUDIT = 16, PTW_LINKED_TO_HOTO = 17, PTW_EXPIRED = 18;

                // JOBCARD
                public const int JC_OPENED = 0, JC_UPDADATED = 1, JC_CLOSED = 2, JC_CARRRY_FORWARDED = 3, JC_APPROVED = 4, JC_REJECTED = 5, JC_PTW_TIMED_OUT = 6;
        */
        public enum CMMS_JobType
        {
            BreakdownMaintenance = 0,
            PreventiveMaintenance,
            Audit
        }
        [Flags]
        public enum CMMS_Access
        {
            ADD = 1,
            EDIT = 2,
            DELETE = 4,
            VIEW = 8,
            ISSUE = 16,
            APPROVE = 32,
            SELF_VIEW = 64
        }

        public enum CMMS_Status
        {
            Invalid = 0,
            CREATED = 1,
            UPDATED,
            DELETED,
            CANCELLED,
            ASSIGNED,
            ISSUED,
            APPROVED,
            REJECTED,

            JOB_CREATED = 101,
            JOB_ASSIGNED,
            JOB_LINKED,
            JOB_IN_PROGRESS,
            JOB_CLOSED,
            JOB_CANCELLED,
            JOB_DELETED,

            PTW_CREATED = 121,
            PTW_ISSUED,
            PTW_REJECTED_BY_ISSUER,
            PTW_APPROVE,
            PTW_REJECTED_BY_APPROVER,
            PTW_CLOSED,
            PTW_CANCELLED_BY_ISSUER,
            PTW_CANCELLED_BY_HSE,
            PTW_CANCELLED_BY_APPROVER,
            PTW_CANCEL_REQUESTED,
            PTW_CANCEL_REQUEST_REJECTED,
            PTW_EDIT,
            PTW_EXTEND_REQUESTED,
            PTW_EXTEND_REQUEST_APPROVE,
            PTW_EXTEND_REQUEST_REJECTED,
            PTW_LINKED_TO_JOB,
            PTW_LINKED_TO_PM,
            PTW_LINKED_TO_AUDIT,
            PTW_LINKED_TO_HOTO,
            PTW_EXPIRED,

            JC_OPENED = 151,
            JC_UPDADATED,
            JC_CLOSED,
            JC_CARRRY_FORWARDED,
            JC_APPROVED,
            JC_REJECTED5,
            JC_PTW_TIMED_OUT,


            PM_START = 161,
            PM_UPDATE,
            PM_SUBMIT,
            PM_LINK_PTW,
            PM_COMPLETED,
            PM_APPROVE,
            PM_REJECT,
            PM_CANCELLED,
            PM_PTW_TIMEOUT,
            PM_DELETED,


            IR_CREATED = 181,
            IR_APPROVED,
            IR_REJECTED,
            IR_UPDATED,

            WC_DRAFT = 191,
            WC_CREATED,
            //WC_WAITING_FOR_SUBMIT_APPROVAL,
            WC_SUBMIT_REJECTED,
            WC_SUBMITTED,
            WC_DISPATCHED,
            WC_REJECTED_BY_MANUFACTURER,
            WC_APPROVED_BY_MANUFACTURER,
            WC_ITEM_REPLENISHED,
            //WC_WAITING_FOR_CLOSE_APPROVAL,
            WC_CLOSE_REJECTED,
            WC_CLOSE_APPROVED,
            WC_CANCELLED,

            CALIBRATION_REQUEST = 211,
            CALIBRATION_REQUEST_APPROVED,
            CALIBRATION_REQUEST_REJECTED,
            CALIBRATION_STARTED,
            CALIBRATION_COMPLETED,
            CALIBRATION_CLOSED,
            CALIBRATION_APPROVED,
            CALIBRATION_REJECTED,
        }

        public enum checklist_type
        {
            PM=1,
            Audit,
        }

    }
}
