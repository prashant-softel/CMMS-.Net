using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories.Jobs
{
    public class JobRepository : GenericRepository
    {
        public JobRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<JobModel>> GetJobList(int facility_id, int userId)
        {
            /*
            * Fetch data from Job table for provided facility_id.
            * If facility_id 0 then fetch jobs for all facility (No. Facility assigned to employee)
            * Joins these table for relationship using ids Users, Assets, AssetCategory, Facility
            * id and it string value should be there in list
            * ignore standard action, site permit no
            *  job.createdAt as jobDate,DATE_FORMAT(job.breakdownTime, '%d-%m-%Y') as breaKdownTime job.breaKdownTime as brealdownDate,
           */
            /*Your code goes here*/
            string myQuery = "SELECT " +
                                 "facilities.name as plantName, job.createdAt as jobDate,DATE_FORMAT(job.breakdownTime, '%Y-%m-%d') as breaKdownTime, job.id as id, asset_cat.name as equipmentCat, asset.name as workingArea, job.title as jobDetails, workType.workTypeName as workType,  permit.code as permitId, job.createdBy as raisedBy, CONCAT(user.firstName + ' ' + user.lastName) as assignedTo , IF(job.breakdownTime = '', 'Non Breakdown Maintenance', 'Breakdown Maintenance') as breakdownType" +
                                 " FROM " +
                                        "jobs as job " +
                                 "JOIN " +
                                        "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                 "JOIN " +
                                        "assets as asset ON mapAssets.assetId  =  asset.id " +
                                 "JOIN " +
                                        "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                 "LEFT JOIN " +
                                        "permits as permit ON permit.id = job.linkedPermit " +
                                 "LEFT JOIN " +
                                        "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                 "JOIN " +
                                        "facilities as facilities ON job.facilityId = facilities.id " +
                                 "LEFT JOIN " +
                                        "users as user ON user.id = job.createdBy or user.id = job.assignedId";
            if (facility_id != 0)
            {
                myQuery += " WHERE job.facilityId= " + facility_id + " and user.id= " + userId;

            }
        
            List<JobModel> _JobList = await Context.GetData<JobModel>(myQuery).ConfigureAwait(false);
            return _JobList;
        }

        internal async Task<List<JobView>> GetJobDetail(int job_id)
        {
            /*
             * Fetch data from Job table and joins these table for relationship using ids Users, Assets, AssetCategory, Facility
             * id and it string value should be there in list
             * job_id = 3064
            */

            /*Your code goes here*/

            string myQuery = "SELECT " +
                                    "facilities.id as block_id, facilities.name as block_name, asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name, asset.id as workingArea_id, asset.name as workingArea_name, job.status as status, user.id as assigned_id , CONCAT(user.firstName, user.lastName) as assigned_name , workType.workTypeName as workType,  job.title as job_title, job.description as job_description " +
                                      "FROM " +
                                            "jobs as job " +
                                      "JOIN " +
                                            "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                      "JOIN " +
                                            "assets as asset ON mapAssets.assetId  =  asset.id " +
                                      "JOIN " +
                                            "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                      "LEFT JOIN " +
                                            "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                      "JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId" +           
                                      " WHERE job.id= " + job_id;

            List<JobView> _ViewJobList = await Context.GetData<JobView>(myQuery).ConfigureAwait(false);
            return _ViewJobList;

        }

        internal async Task<int> CreateNewJob(CreateJob request)
        {
            /*
             * Job basic details will go to Job table
             * Job associated assets and category will go in JobMappingAssets table (one to many relation)
             * Job associated work type will go in JobAssociatedWorkTypes
             * return value will be inserted record. Use GetJobDetail() function
            */

            /*Your code goes here*/

            string qryJobBasic = "insert into jobs(facilityId,blockId,title,description,assignedId,breakdownTime) values" +
            "('" + request.facility_id + "','" + request.block_id + "','" + request.title + "','" + request.description + "','" + request.assigned_id + "','" + request.breakdown_time + "')";
            int jobPrimaryKey = 3168;
            await Context.ExecuteNonQry<int>(qryJobBasic).ConfigureAwait(false);

            foreach (var data in request.AssetsIds)
            {
                string qryAssetsIds = "insert into jobmappingassets(jobId, assetId, categoryId ) value ('" + jobPrimaryKey + "','" + data.asset_id + "','" + data.category_ids + "')";
                await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            }
            foreach (var data in request.JobType_Ids)
            {
                string qryCategoryIds = "insert into jobassociatedworktypes(jobId, workTypeId ) value ('" + jobPrimaryKey + "','" + data + "')";
                await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
            }

            return jobPrimaryKey;
        }

        internal async Task<int> ReAssignJob(int job_id, int user_id, int changed_by)
         {
            /*
             * update history also
             * Get id and what parameter changed
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
            */
            /*Your code goes here*/
            string updateQry = "update jobs set assignedId = '" + user_id + "', updatedBy = '" + changed_by + "' where id = " + job_id + ";";
            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
        }
        internal async Task<int> CancelJob(int job_id, int user_id, string Cancelremark)
        {
            /*
             * update history also
             * Get id and what parameter changed
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
            */
            /*Your code goes here*/
            int Cancelstatus = 1;
            string updateQry = "update jobs set updatedBy = '" + user_id + "',  cancellationRemarks = '" + Cancelremark + "' ,  cancelStatus = '" + Cancelstatus + "'  where id = " + job_id + ";";
            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
        }



        internal async Task<int> LinkToPTW(int job_id, int ptw_id)
        {
            /*
                 *Get id and what parameter changed
                 * AssignedID/ PermitID / CancelJob.Out of 3 we can update any one fields based on request
                 * Re-assigned employee / link permit / Cancel Permit. 3 different end points call this function.     
                 * return boolean true / false
    
                 //update history table
                 Your code goes here
            */
            string updateQry = "update jobs set linkedPermit = '" + ptw_id + "'  where id = " + job_id + ";";
            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
        }
             

    }
}
