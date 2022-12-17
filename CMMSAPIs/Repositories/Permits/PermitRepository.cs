using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories.Permits
{
    public class PermitRepository : GenericRepository
    {
        public PermitRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        /* 
         * Permit Create Form Required End Points 
        */

        internal Task<List<DefaultListModel>> GetPermitTypeList(int facility_id)
        {
            /*
             * return permit_type_id, name from PermitTypeLists table for requsted facility_id 
            */
            return null;
        }

        internal Task<List<DefaultListModel>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            /*
             * return id, title from PermitTypeSafetyMeasures table for requested permit_type_id
             * input 1 - checkbox, 2 - radio, 3 - text, 4 - Ok
            */
            return null;
        }

        internal Task<List<DefaultListModel>> GetJobTypeList(int facility_id)
        {
            /*
             * return id, title from PermitJobTypeList table for requested facility_id
            */
            return null;
        }

        internal Task<List<DefaultListModel>> GetSOPList(int job_type_id)
        {
            /*
             * return * from PermitTBTJobList table for requested job_type_id
            */
            return null;
        }

        /*
         * Permit Main Feature End Points
        */

        internal Task<List<PermitListModel>> GetPermitList(int facility_id)
        {
            /*
             * Return id as well as string value
             * Use Permits, Assets, AssetsCategory, Users table to fetch below fields
             * Permit id, site Permit No., Permit Type, Equipment Categories, Working Area/Equipment, Description, Permit requested by
             * Request Date/Time, Approved By, Approved Date/Time, Current Status(Approved, Rejected, closed).
            */
            return null;
        }

        internal Task<List<PermitModel>> CreatePermit()
        {
            /*
             * Create Form data will go in several tables
             * Permits, PermitBlocks, PermitIsolatedAssetCategories, PermitLOTOAssets, PermitEmployeeLists, PermitSafetyQuestions
             * Permits                       - Basic details
             * PermitBlocks                  - One Pemrit can be created for multiple blocks
             * PermitIsolatedAssetCategories - If Isolation is required. They can select multiple Equipment Categories
             * PermitLOTOAssets              - List of assets 
             * PermitEmployeeLists           - Employee list those going to work on Permit
             * PermitSafetyQuestions         - Safety question they answered while creating Permit
             * Once you saved the records
             * Return GetPermitDetails(permit_id);
            */
            return null;
        }

        internal Task<List<PermitModel>> GetPermitDetails(int permit_id)
        {
            /*
             * Return id and string values which are stored in 
             * Permits, PermitBlocks, PermitIsolatedAssetCategories, PermitLOTOAssets, PermitEmployeeLists, PermitSafetyQuestions table
             * for request permit_id Join with below tables to get string value from
             * Assets, AssetsCategory, Facility, Users, PermitTypeSafetyMeasures, PermitTypeList, PermitJobTypeList, PermitTBTJobList
            */
            return null;
        }

        /*
         * Permit Issue/Approval/Rejection/Cancel End Points
        */

        internal Task<List<DefaultResponseModel>> PermitApprove(ApprovalModel request)
        {
            /*
             * Update Permit Table reccomendationsByApprover, approvedStatus, approvedDate
             * Return Message Approved successfully
            */
            return null;
        }

        internal Task<List<DefaultResponseModel>> PermitReject(ApprovalModel request)
        {
            /*
             * Pending
            */
            return null;
        }

        internal Task<List<DefaultResponseModel>> PermitIssue(ApprovalModel request)
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */
            return null;
        }

        internal Task<List<DefaultResponseModel>> PermitCancel(ApprovalModel request)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */
            return null;
        }


    }
}
