using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.JC;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories.JC
{
    public class JCRepository : GenericRepository
    {
        public JCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal Task<List<JCListModel>> GetJCList(int facility_id)
        {
            /* Return all field mentioned in JCListModel model
            *  tables are JobCards, Jobs, Permit, Users
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<JCDetailModel>> GetJCDetail(int jc_id)
        {
            /*
             * Fetch data from JobCards table and joins these table for relationship using ids 
             * Users, Assets, AssetCategory, Facility, PermiEmployeeLists, PermitLotoAssets, PermitIsolatedAssetCategories
             * Return all the field listed in JCDetailModel 
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<DefaultResponseModel>> CreateJC(int job_id)
        {
            /*
             * 
             * Data will be inserted in Following tables
             * JobCards - Primary table (All basic details in inserted)
             * PermiEmployeeLists - All jc linked employee
             * JCFiles - All uploaded file data
             * Please check the JobCard above tables to get idea what values need to insert. 
             * Get details from job, permit and add it to Jobcards table
             * Return the All Properties in DefaultResponse model
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<DefaultResponseModel>> UpdateJC(JCUpdateModel request)
        {
            /*
             * Below thing will happen in update function
             * User can upload new files - (JCFiles Tables)
             * Add new comments - History Table (use Utils AddLog functions)
             * Add/Remove employee list - PermiEmployeeLists table
             * Status - JobCards table
             * Return the DefaultResponseModel
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<DefaultResponseModel>> CloseJC(JCCloseModel request)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<DefaultResponseModel>> ApproveJC(ApprovalModel request)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return DefaultResponseModel
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<DefaultResponseModel>> RejectJC(ApprovalModel request)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return boolean true/false
            */

            /*Your code goes here*/
            return null;
        }
    }
}
