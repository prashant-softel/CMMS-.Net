using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Repositories.Jobs
{
    public class JobRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;

        public JobRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<List<CMJobModel>> GetJobList(int facility_id, int userId)
        {
            /*
            * Fetch data from Job table for provided facility_id.
            * If facility_id 0 then fetch jobs for all facility (No. Facility assigned to employee)
            * Joins these table for relationship using ids Users, Assets, AssetCategory, Facility
            * id and it string value should be there in list
            * ignore standard action, site permit no

            /*Your code goes here*/
            string myQuery = "SELECT " +
                                 "facilities.name as plantName, job.status, job.createdAt as jobDate, DATE_FORMAT(job.breakdownTime, '%Y-%m-%d') as breaKdownTime, job.id as id, asset_cat.name as equipmentCat, asset.name as workingArea, job.title as jobDetails, workType.workTypeName as workType, permit.code as permitId, job.createdBy as raisedBy, CONCAT(user.firstName + ' ' + user.lastName) as assignedTo, IF(job.breakdownTime = '', 'Non Breakdown Maintenance', 'Breakdown Maintenance') as breakdownType" +
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

            List<CMJobModel> _JobList = await Context.GetData<CMJobModel>(myQuery).ConfigureAwait(false);
            return _JobList;
        }

        internal async Task<List<CMJobView>> GetJobDetail(int job_id)
        {
            /*
             * Fetch data from Job table and joins these table for relationship using ids Users, Assets, AssetCategory, Facility
             * id and it string value should be there in list
            */

            /*Your code goes here*/

            string myQuery = "SELECT " +
                                    "facilities.id as block_id, job.status as JobStatus, facilities.name as block_name, job.status as status, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, workType.workTypeName as workType,  job.title as job_title, job.description as job_description " +
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
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            //get equipmentCat list
            string myQuery1 = "SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat " +
                "JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMequipmentCatList> _equipmentCatList = await Context.GetData<CMequipmentCatList>(myQuery1).ConfigureAwait(false);

            //get workingArea_name list 
            string myQuery2 = "SELECT asset.id as workingArea_id, asset.name as workingArea_name FROM assets as asset " +
             "JOIN jobmappingassets as mapAssets ON mapAssets.assetId  =  asset.id  JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMworkingAreaNameList> _WorkingAreaNameList = await Context.GetData<CMworkingAreaNameList>(myQuery2).ConfigureAwait(false);

            //get Associated permits
            string myQuery3 = "SELECT ptw.title as title, ptw.permitNumber as sitePermitNo, CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, ptw.status as ptwStatus FROM permits as ptw JOIN jobs as job ON ptw.id = job.linkedPermit " +
                "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                "LEFT JOIN fleximc_jc_files as jobCard ON jobCard.JC_id = ptw.id " +
            "WHERE job.id= " + job_id;
            List<CMAssociatedPermitList> _AssociatedpermitList = await Context.GetData<CMAssociatedPermitList>(myQuery3).ConfigureAwait(false);

            _ViewJobList[0].LstCMequipmentCatList = _equipmentCatList;
            _ViewJobList[0].LstCMworkingAreaNameList = _WorkingAreaNameList;
            _ViewJobList[0].LstAssociatedPermit = _AssociatedpermitList;

            return _ViewJobList;
        }

        internal async Task<int> CreateNewJob(CMCreateJob request)
        {
            /*
             * Job basic details will go to Job table
             * Job associated assets and category will go in JobMappingAssets table (one to many relation)
             * Job associated work type will go in JobAssociatedWorkTypes
             * return value will be inserted record. Use GetJobDetail() function
            */

            /*Your code goes here*/
            string qryJobBasic = "insert into jobs(facilityId, blockId,title, description, createdAt, createdBy, breakdownTime,assignedId, linkedPermit) values" +
            $"({ request.facility_id }, { request.block_id }, '{ request.title }', '{ request.description }', '{UtilsRepository.GetUTCTime() }','{ request.createdBy }','{ UtilsRepository.GetUTCTime() }','{ request.assigned_id }','{ request.permit_id }')";
            int jobPrimaryKey = 3158;
            await Context.ExecuteNonQry<int>(qryJobBasic).ConfigureAwait(false);

            foreach (var data in request.AssetsIds)
            {
                string qryAssetsIds = $"insert into jobmappingassets(jobId, assetId, categoryId ) value ({ jobPrimaryKey }, { data.asset_id },{ data.category_ids })";
                await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            }
            foreach (var data in request.WorkType_Ids)
            {
                string qryCategoryIds = $"insert into jobassociatedworktypes(jobId, workTypeId ) value ( { jobPrimaryKey }, { data } )";
                await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
            }
            // Add previous setting to log table
            CMLog _log = new CMLog();
            _log.module_type = Constant.JOB;
            _log.module_ref_id = jobPrimaryKey;
            _log.comment = "Job Added";
            _log.status = Constant.JOB_CREATED;
            await _utilsRepo.AddLog(_log);
            return jobPrimaryKey;
        }

        internal async Task<int> ReAssignJob(int job_id, int user_id, int changed_by)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false*/
            string updateQry = $"update jobs set assignedId = { user_id }, updatedBy = { changed_by } where id = { job_id } ";
            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

        }
        internal async Task<int> CancelJob(int job_id, int user_id, string Cancelremark)
        {
            /*Your code goes here*/
            int Cancelstatus = 1;
            string updateQry = $"update jobs set updatedBy = { user_id },  cancellationRemarks = '{ Cancelremark }',  cancelStatus = { Cancelstatus }  where id = { job_id };";
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
            string updateQry = $"update jobs set linkedPermit = { ptw_id }  where id =  { job_id };";
            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
        }

    }
}
