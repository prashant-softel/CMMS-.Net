using System.Collections.Generic;
using System;

namespace CMMSAPIs.Helper
{
    public class CMMS
    {
        // Token expiration time
        public const int TOKEN_EXPIRATION_TIME = 30;
	public const int TOKEN_RENEW_TIME = 30;
        public Dictionary<CMMS_Modules, int> module_dict = new Dictionary<CMMS_Modules, int>()
        {
            { CMMS_Modules.DASHBOARD, 1 },
            { CMMS_Modules.JOB, 2 },
            { CMMS_Modules.PTW, 3 },
            { CMMS_Modules.JOBCARD, 4 },
            { CMMS_Modules.CHECKLIST_NUMBER, 5 },
            { CMMS_Modules.CHECKPOINTS, 6 },
            { CMMS_Modules.CHECKLIST_MAPPING, 7 },
            { CMMS_Modules.PM_SCHEDULE, 8 },
            { CMMS_Modules.PM_SCEHDULE_VIEW, 9 },
            { CMMS_Modules.PM_EXECUTION, 10 },
            { CMMS_Modules.PM_SCHEDULE_REPORT, 11 },
            { CMMS_Modules.PM_SUMMARY, 12 },
            { CMMS_Modules.SM_MASTER, 13 },
            { CMMS_Modules.SM_GO, 14 },
            { CMMS_Modules.SM_MRS, 15 },
            { CMMS_Modules.SM_MRS_RETURN, 16 },
            { CMMS_Modules.SM_S2S, 17 },
            { CMMS_Modules.AUDIT_PLAN, 18 },
            { CMMS_Modules.AUDIT_SCHEDULE, 19 },
            { CMMS_Modules.AUDIT_SCEHDULE_VIEW, 20 },
            { CMMS_Modules.AUDIT_EXECUTION, 21 },
            { CMMS_Modules.AUDIT_SUMMARY, 22 },
            { CMMS_Modules.HOTO_PLAN, 23 },
            { CMMS_Modules.HOTO_SCHEDULE, 24 },
            { CMMS_Modules.HOTO_SCEHDULE_VIEW, 25 },
            { CMMS_Modules.HOTO_EXECUTION, 26 },
            { CMMS_Modules.HOTO_SUMMARY, 27 },
            { CMMS_Modules.INVENTORY, 28 },
            { CMMS_Modules.WARRANTY_CLAIM, 30 },
            { CMMS_Modules.CALIBRATION, 31 },
            { CMMS_Modules.MODULE_CLEANING, 32 },
            { CMMS_Modules.VEGETATION, 33 },
            { CMMS_Modules.INCIDENT_REPORT, 34 },
            { CMMS_Modules.DSM, 35 }
        };
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

        public enum Gender
        {
            Male = 1,
            Female = 2,
            Transgender = 3,
            Other = 4
        }

        public static Dictionary<int, string> BLOOD_GROUPS = new Dictionary<int, string>()
        {
            { 1, "A+" },
            { 2, "A-" },
            { 3, "B+" },
            { 4, "B-" },
            { 5, "AB+" },
            { 6, "AB-" },
            { 7, "O+" },
            { 8, "O-" }
        };
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
            PM_PLAN,
            PM_TASK,

            SM_MASTER = 31,
            SM_GO,
            SM_MRS,
            SM_MRS_RETURN,
            SM_S2S,          
            SM_RO,

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
            ESCALATION_MATRIX = 161,

            USER = 171,
            USER_MODULE,
            USER_NOTIFICATIONS, 
            ROLE_DEFAULT_NOTIFICATIONS, 
            ROLE_DEFAULT_ACCESS_MODULE,

            DSM = 181,
        }

        public enum INCIDENT_RISK_LEVEL
        {
            HIGH = 1,
            MEDIUM = 2,
            LOW = 3
        }
        
        public static Dictionary<string, int> INCIDENT_RISK_TYPE =  new Dictionary<string, int>() { {"First-Aids Injury", 1 }, { "Electric Short", 2} };

        public static Dictionary<string, int> INCIDENT_SEVERITY = new Dictionary<string, int>() 
        { { "Critical", 1 }, { "High", 2 } , { "Medium", 3 }, { "Low", 4 } };

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
            HOTO,
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
            //JOB_IN_PROGRESS,
            //JOB_CARRY_FORWARDED,
            JOB_CLOSED,
            JOB_CANCELLED,
            JOB_UPDATED,    //For notification purpose. Do not update JOB_UPDATED in databbase.

            PTW_CREATED = 121,
            PTW_REJECTED_BY_ISSUER,
            PTW_ISSUED,
            PTW_REJECTED_BY_APPROVER,
            PTW_APPROVED,
            PTW_CLOSED,
            PTW_CANCELLED_BY_ISSUER,
            PTW_CANCELLED_BY_HSE,
            PTW_CANCELLED_BY_APPROVER,
            PTW_CANCEL_REQUESTED,
            PTW_CANCEL_REQUEST_REJECTED,
            PTW_CANCEL_REQUEST_APPROVED,
            PTW_EXTEND_REQUESTED,
            PTW_EXTEND_REQUEST_REJECTED,
            PTW_EXTEND_REQUEST_APPROVE,
            PTW_LINKED_TO_JOB,
            PTW_LINKED_TO_PM,
            PTW_LINKED_TO_AUDIT,
            PTW_LINKED_TO_HOTO,
            PTW_EXPIRED,
            PTW_UPDATED,

            //JC_CREATED = 151,
            //JC_STARTED,
            //JC_CARRRY_FORWARDED,
            //JC_CLOSED,
            //JC_REJECTED,
            //JC_APPROVED,
            //JC_UPDATED,                 //only for notification purpose

            JC_CREATED = 151,
            JC_STARTED,
            JC_CARRY_FORWARDED,
            JC_CF_REJECTED,
            JC_CF_APPROVED,
            JC_CLOSED,
            JC_CLOSE_REJECTED,
            JC_CLOSE_APPROVED,
            JC_UPDATED,                 //only for notification purpose

            PM_SCHEDULED = 161,
            PM_ASSIGNED,
            PM_LINKED_TO_PTW,
            PM_START,
            PM_COMPLETED,
            PM_REJECTED,
            PM_APPROVED,
            PM_CANCELLED,
            PM_DELETED,
            PM_UPDATED, //Only for notification purpose
            
            PM_SUBMIT,//for temp only
            PM_LINK_PTW,
           
           
            IR_CREATED = 181,
            IR_APPROVED,
            IR_REJECTED,
            IR_UPDATED,

            WC_DRAFT = 191,
            WC_SUBMITTED,
            //WC_WAITING_FOR_SUBMIT_APPROVAL,
            WC_SUBMIT_REJECTED,
            WC_SUBMIT_APPROVED,
            WC_DISPATCHED,
            WC_REJECTED_BY_MANUFACTURER,
            WC_APPROVED_BY_MANUFACTURER,
            WC_ITEM_REPLENISHED,
            //WC_WAITING_FOR_CLOSE_APPROVAL,
            WC_CLOSE_REJECTED,
            WC_CLOSE_APPROVED,
            WC_CANCELLED,

            CALIBRATION_REQUEST = 211,
            CALIBRATION_REQUEST_REJECTED,
            CALIBRATION_REQUEST_APPROVED,
            CALIBRATION_STARTED,
            CALIBRATION_COMPLETED,
            CALIBRATION_CLOSED,
            CALIBRATION_REJECTED,
            CALIBRATION_APPROVED,
            
			INVENTORY_IMPORTED = 221,
            INVENTORY_ADDED,
            INVENTORY_UPDATED,
            INVENTORY_DELETED,


            GO_DRAFT = 301,
            GO_SUBMITTED,
            GO_DELETED,
            GO_CLOSED,
            GO_REJECTED,
            GO_APPROVED,
            GO_RECEIVE_DRAFT,
            GO_RECEIVED_SUBMITTED,
            GO_RECEIVED_REJECTED,
            GO_RECEIVED_APPROVED,
            //SM_PO_DRAFT = 301,          //1         
            //SM_PO_SUBMITTED,            //2
            //SM_PO_IN_PROCESS,           //3
            //SM_PO_DELETED,              //4
            //SM_PO_CLOSED,               //5
            //SM_PO_CLOSED_REJECTED,      //6
            //SM_PO_CLOSED_APPROVED,      //7


            MRS_SUBMITTED =321,
            MRS_REQUEST_REJECTED,
            MRS_REQUEST_APPROVED,             
            MRS_REQUEST_ISSUED ,
            MRS_REQUEST_ISSUED_REJECTED ,
            MRS_REQUEST_ISSUED_APPROVED ,
	        MRS_REQUEST_RETURN,
            

   
            
            //GO_SHORT_CLOSED_BY_STORE_KEEPER,
            GO_WITHDRAW_BY_ADMINISTRATOR,

         
            SM_RO_DRAFT = 341,
            SM_RO_SUBMITTED,
            SM_RO_SUBMIT_REJECTED,
            SM_RO_SUBMIT_APPROVED,
            SM_RO_DELETED,
            SM_RO_CLOSED,

            MC_PLAN_DRAFT = 350,
            MC_PLAN_SUBMITTED,
            MC_PLAN_REJECTED,
            MC_PLAN_APPROVED,
            MC_PLAN_DELETED,

            MC_TASK_SCHEDULED = 360,
            MC_TASK_STARTED,
            MC_TASK_COMPLETED,
            MC_TASK_ABANDONED,

            VEG_PLAN_DRAFT = 370,
            VEG_PLAN_SUBMITTED,
            VEG_PLAN_REJECTED,
            VEG_PLAN_APPROVED,
            VEG_PLAN_DELETED,

            VEG_TASK_SCHEDULED = 380,
            VEG_TASK_STARTED,
            VEG_TASK_COMPLETED,
            VEG_TASK_ABANDONED,

            PM_PLAN_CREATED = 401,
            PM_PLAN_DRAFT,
            PM_PLAN_UPDATED,
            PM_PLAN_DELETED,
            PM_PLAN_APPROVED,
            PM_PLAN_REJECTED,
        }

        public enum ApprovalStatus
        {
            WAITING_FOR_APPROVAL = 0,
            APPROVED,
            REJECTED
        }

        public enum checklist_type
        {
            PM=1,
            HOTO,
            Audit,
        }
        public enum SM_OrderByType
        {
            SM_OWNER = 1,
            SM_OPERATOR

        }
        public enum cleaningType
        {
            ModuleCleaning = 1,
            Vegetation
        }

        public enum SM_Types
    {
        Vendor = 1, 
        Store = 2,
        Engineer = 3,
        Inventory = 4
    }
    }
}
