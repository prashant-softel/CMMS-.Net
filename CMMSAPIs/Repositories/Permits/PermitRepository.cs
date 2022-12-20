using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
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

        internal Task<List<Permit>> GetPermitTypeList(int facility_id)
        {
            /*
             * return permit_type_id, name from PermitTypeLists table for requsted facility_id 
            */
            return null;
        }

        internal Task<List<Permit>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            /*
             * return id, title from PermitTypeSafetyMeasures table for requested permit_type_id
             * input 1 - checkbox, 2 - radio, 3 - text, 4 - Ok
            */
            return null;
        }

        internal Task<List<Permit>> GetJobTypeList(int facility_id)
        {
            /*
             * return id, title from PermitJobTypeList table for requested facility_id
            */
            return null;
        }

        internal Task<List<Permit>> GetSOPList(int job_type_id)
        {
            /*
             * return * from PermitTBTJobList table for requested job_type_id
            */
            return null;
        }

        /*
         * Permit Main Feature End Points
        */

        internal async Task<List<Permit>> GetPermitList(int facility_id, int userID)
        {
            /*
             * Return id as well as string value
             * Use Permits, Assets, AssetsCategory, Users table to fetch below fields
             * Permit id, site Permit No., Permit Type, Equipment Categories, Working Area/Equipment, Description, Permit requested by
             * Request Date/Time, Approved By, Approved Date/Time, Current Status(Approved, Rejected, closed).           
            */
            string myQuery = "SELECT " +
                                 "ptw.id as permitId, ptw.permitNumber as sitePermitNo, permitType.id as permitType,  permitType.title as PermitTypeName, asset_cat.name as equipmentCat, facilities.id as workingAreaId, facilities.name as workingAreaName, ptw.description as description, ptw.acceptedById as ptwRequestedBy, ptw.acceptedDate as ptwRequestDate, ptw.approvedById as approvedBy, ptw.approvedDate as approvedDate, ptw.status as currentStatus " +
                                 " FROM " +
                                        "permits as ptw " +                                 
                                  "JOIN " +
                                        "facilities as facilities ON ptw.blockId = facilities.id " +
                                  "JOIN " +
                                        "assetcategories as asset_cat ON ptw.id = asset_cat.id " +
                                  "LEFT JOIN " +
                                         "permittypelists as permitType ON ptw.typeId = permitType.id " +
                                  "LEFT JOIN " +
                                         "jobs as job ON ptw.id = job.linkedPermit " +
                                  "LEFT JOIN " +
                                        "users as user ON user.id = ptw.issuedById or user.id = ptw.approvedById" +
                                  " WHERE ptw.facilityId = " + facility_id + " and user.id = " + userID;          
            List<Permit> _PermitList = await Context.GetData<Permit>(myQuery).ConfigureAwait(false);
            return _PermitList;
        }

        internal Task<List<Permit>> CreatePermit()
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

        internal  Task<List<Permit>> GetPermitDetails(int permit_id)
        {
            /*
             * Return id and string values which are stored in 
             * Permits, PermitBlocks, PermitIsolatedAssetCategories, PermitLOTOAssets, PermitEmployeeLists, PermitSafetyQuestions table
             * for request permit_id Join with below tables to get string value from
             * Assets, AssetsCategory, Facility, Users, PermitTypeSafetyMeasures, PermitTypeList, PermitJobTypeList, PermitTBTJobList
            */
         /*   string myQuery = "SELECT " +
                                   "ptw.PTW_id as permitId, ptw.PTW_code as sitePermitNo, ptw.id as permitType, ptw.PTW_Title as PermitTypeName, asset_cat.name as equipmentCat, asset.id as workingAreaId, asset.name as workingAreaName, job.description as description, ptw.File_added_by as ptwRequestedBy, ptw.File_added_date as ptwRequestDate, ptw.File_updated_by as approvedBy, ptw.File_updated_date as approvedDate, ptw.status as currentStatus " +
                                   " FROM " +
                                          "fleximc_ptw_files as ptw " +
                                   "JOIN " +
                                          "assets as asset ON  ptw.File_Category_id  =  asset.id " +
                                   "JOIN " +
                                          "assetcategories as asset_cat ON ptw.File_Category_id = asset_cat.id " +
                                   "LEFT JOIN " +
                                          "users as user ON user.id = ptw.File_added_by or user.id = ptw.File_updated_by" +
                                    " WHERE ptw.Facility_id= " + facility_id + " and user.id= " + userID;
*/
/*            List<Permit> _PermitList = await Context.GetData<Permit>(myQuery).ConfigureAwait(false);
*/            return null;
        }

        /*
         * Permit Issue/Approval/Rejection/Cancel End Points
        */

        internal Task<List<Permit>> PermitApprove()
        {
            /*
             * Update Permit Table reccomendationsByApprover, approvedStatus, approvedDate
             * Return Message Approved successfully
            */
            return null;
        }

        internal Task<List<Permit>> PermitReject()
        {
            /*
             * Pending
            */
            return null;
        }

        internal Task<List<Permit>> PermitIssue()
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */
            return null;
        }

        internal Task<List<Permit>> PermitCancel()
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */
            return null;
        }


    }
}
