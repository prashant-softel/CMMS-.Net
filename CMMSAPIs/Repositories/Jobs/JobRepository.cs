    using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;
using System.Data;
using System;

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
                                 "job.id, job.facilityId, user.id, facilities.name as plantName, job.status as status, job.createdAt as jobDate, DATE_FORMAT(job.breakdownTime, '%Y-%m-%d') as breakdown_time, job.id as id, asset_cat.name as equipmentCat, asset.name as workingArea, job.title as jobDetails, workType.workTypeName as workType, permit.code as permitId, job.createdBy as raisedBy, CONCAT(user.firstName , ' ' , user.lastName) as assignedToName, user.id as assignedToId, IF(job.breakdownTime = '', 'Non Breakdown Maintenance', 'Breakdown Maintenance') as breakdownType , job.description as description" +
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

        public static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = "Created";
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = "Assigned";
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = "Linked to PTW";
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = "Closed";
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = "Cancelled";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }


        public string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJobView jobObj)
        {
                string retValue = "My job subject";
                int jobId = jobObj.id;

                switch (notificationID)
                {
                    case CMMS.CMMS_Status.JOB_CREATED:     //Created
                                                           //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                        string desc = jobObj.job_description;
                        retValue = String.Format("Job <{0}><{1}> created", jobId, desc);
                        break;
                    case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                        retValue = String.Format("Job <{0}> assigned to <{1}>", jobObj.job_title, jobObj.assigned_name);
                        break;
                    case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                        retValue = String.Format("Job <{0}> linked to PTW <{1}>", jobObj.job_title, jobObj.current_ptw_id);
                        break;
                    case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                        retValue = String.Format("Job <{0}> closed", jobObj.job_title);
                        break;
                    case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                        retValue = String.Format("Job <{0}> Cancelled", jobObj.job_title);
                        break;
                    default:
                        break;
                }
                return retValue;

            }

            internal async Task<CMJobView> GetJobDetails(int job_id)
            {
            /*
             * Fetch data from Job table and joins these table for relationship using ids Users, Assets, AssetCategory, Facility
             * id and it string value should be there in list
            */

            /*Your code goes here*/

            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as facility_id, facilities.name as facility_name, blocks.id as block_id, blocks.name as block_name,                                     job.jobType as job_type,         job.status as status, created_user.id as created_by_id, CONCAT(created_user.firstName, created_user.lastName) as created_by_name, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, workType.workTypeName as workType,  job.title as job_title, job.description as job_description, job.breakdownTime as breakdown_time, ptw.id as current_ptw_id, ptw.title as current_ptw_title " +
                                      "FROM " +
                                            "jobs as job " +
                                      "JOIN " +
                                            "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                      "JOIN " +
                                            "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                      "LEFT JOIN " +
                                            "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                      "JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "JOIN " +
                                            "facilities as blocks ON job.blockId = blocks.id " +
                                      "LEFT JOIN " +
                                            "permits as ptw ON job.linkedPermit = ptw.id " +
                                      "LEFT JOIN " +
                                            "users as created_user ON created_user.id = job.createdby " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId " +
                                      "WHERE job.id = " + job_id;
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_ViewJobList[0].status);
			if(_ViewJobList[0].status < 100)
			{
                _Status = (CMMS.CMMS_Status)(_ViewJobList[0].status + 100);
			}
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
            _ViewJobList[0].status_short = _shortStatus;
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOB, _Status, _ViewJobList[0]);
            _ViewJobList[0].status_long = _longStatus;

            //get equipmentCat list
            string myQuery1 = "SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat " +
                "JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMequipmentCatList> _equipmentCatList = await Context.GetData<CMequipmentCatList>(myQuery1).ConfigureAwait(false);

            //get workingArea_name list 
            string myQuery2 = "SELECT asset.id as workingArea_id, asset.name as workingArea_name FROM assets as asset " +
             "JOIN jobmappingassets as mapAssets ON mapAssets.assetId  =  asset.id  JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMworkingAreaNameList> _WorkingAreaNameList = await Context.GetData<CMworkingAreaNameList>(myQuery2).ConfigureAwait(false);

            //get Associated permits
            string myQuery3 = "SELECT ptw.id as permitId, ptw.title as title, ptw.code as sitePermitNo, ptw.startDate, ptw.endDate,  CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, ptw.status as ptwStatus FROM permits as ptw JOIN jobs as job ON ptw.id = job.linkedPermit " +
                "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                "LEFT JOIN fleximc_jc_files as jobCard ON jobCard.JC_id = ptw.id " +
            "WHERE job.id= " + job_id;
            List<CMAssociatedPermitList> _AssociatedpermitList = await Context.GetData<CMAssociatedPermitList>(myQuery3).ConfigureAwait(false);
            _AssociatedpermitList[0].ptwStatus_short = "Linked";    //temp till JOIN is made
            _ViewJobList[0].equipment_cat_list = _equipmentCatList;
            _ViewJobList[0].working_area_name_list = _WorkingAreaNameList;
            _ViewJobList[0].associated_permit = _AssociatedpermitList;

            return _ViewJobList[0];
        }

        internal async Task<CMDefaultResponse> CreateNewJob(CMCreateJob request)
        {
            /*
             * Job basic details will go to Job table
             * Job associated assets and category will go in JobMappingAssets table (one to many relation)
             * Job associated work type will go in JobAssociatedWorkTypes
             * return value will be inserted record. Use GetJobDetail() function
            */

            /*Your code goes here*/
            int status = (int)CMMS.CMMS_Status.CREATED;
            if (request.assigned_id > 0)
            {
                status = (int)CMMS.CMMS_Status.ASSIGNED;
            }
            //int created_by = Utils.UtilsRepository.GetUserID();

            if (request.jobType == 0)
            {
                request.jobType = (int)CMMS.CMMS_JobType.BreakdownMaintenance;
            }
            else if(request.jobType == 1)
            {
                request.jobType = (int)CMMS.CMMS_JobType.PreventiveMaintenance;
            }
            else if (request.jobType == 2)
            {
                request.jobType = (int)CMMS.CMMS_JobType.Audit;
            }
            string qryJobBasic = "insert into jobs(facilityId, blockId, title, description, createdAt, createdBy, breakdownTime, JobType, status, assignedId, linkedPermit) values" +
            $"({ request.facility_id }, { request.block_id }, '{ request.title }', '{ request.description }', '{UtilsRepository.GetUTCTime() }','{ request.createdBy }','{ UtilsRepository.GetUTCTime() }',{request.jobType},'{ status }','{ request.assigned_id }','{ request.permit_id }')";
            qryJobBasic = qryJobBasic + ";" + "select LAST_INSERT_ID(); ";
            string qry = "select id as id , title as job_title, description as job_description from jobs order by id desc limit 1";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int retID = System.Convert.ToInt32(dt.Rows[0][0]);

            

            //Pending : modify this query to get assignedd_id and assigned_name

            /*string myNewJobQuery = "SELECT " +
                        "job.id as id, facilities.id as block_id, job.status as JobStatus, " + 
                          "facilities.name as block_name, job.status as status, user.id as assigned_id, " + 
                          "CONCAT(user.firstName, user.lastName) as assigned_name, workType.workTypeName as workType, "+
                          "job.title as job_title, job.description as job_description " +
                          "FROM " +
                                "jobs as job " +
                          "JOIN " +
                                "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                          "JOIN " +
                                "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                          "LEFT JOIN " +
                                "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                          "JOIN " +
                                "facilities as facilities ON job.facilityId = facilities.id " +
                          "LEFT JOIN " +
                                "users as user ON user.id = job.assignedId order by job.id desc limit 1";
*/
            List<CMJobView> newJob = await Context.GetData<CMJobView>(qry).ConfigureAwait(false);
            int newJobID = newJob[0].id;
            
            foreach (var data in request.AssetsIds)
            {
                string qryAssetsIds = $"insert into jobmappingassets(jobId, assetId, categoryId ) value ({ newJobID }, { data.asset_id },{ data.category_ids })";
                await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            }
            foreach (var data in request.WorkType_Ids)
            {
                string qryCategoryIds = $"insert into jobassociatedworktypes(jobId, workTypeId ) value ( { newJobID }, { data } )";
                await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Created", CMMS.CMMS_Status.CREATED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.CREATED, newJob[0]);

            string strJobStatusMsg = "Job {newJobID} Created";
            if (newJob[0].assigned_id > 0)
            {             
                strJobStatusMsg = "Job {newJobID} Created and Assigned to " + newJob[0].assigned_name;
            }

            CMDefaultResponse response = new CMDefaultResponse(newJobID, CMMS.RETRUNSTATUS.SUCCESS, strJobStatusMsg);

            return response;
        }

        internal async Task<CMDefaultResponse> ReAssignJob(int job_id, int assignedTo)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false*/
            int changed_by = Utils.UtilsRepository.GetUserID();
            string updateQry = $"update jobs set assignedId = { assignedTo }, updatedBy = { changed_by } where id = { job_id } ";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if(retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as block_id, facilities.name as block_name, facilities1.name as block_name, facilities.name as facility_name, job.status as status, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, workType.workTypeName as workType,  job.title as job_title, job.description as job_description " +
                                      "FROM " +
                                            "jobs as job " +
                                      "JOIN " +
                                            "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                      "JOIN " +
                                            "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                      "LEFT JOIN " +
                                            "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                      "JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "JOIN " +
                                            "facilities as facilities1 ON job.blockId = facilities1.id " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId" +
                                      " WHERE job.id= " + job_id;

            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, retVal, 0, 0, "Permit Assigned", CMMS.CMMS_Status.JOB_ASSIGNED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_ASSIGNED, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList[0].id} Assigned");
            return response;
        }

        internal async Task<CMDefaultResponse> CancelJob(int job_id, int user_id, string Cancelremark)
        {
            /*Your code goes here*/
            string updateQry = $"update jobs set updatedBy = { user_id },  cancellationRemarks = '{ Cancelremark }',  cancelStatus = { (int)CMMS.CMMS_Status.JOB_CANCELLED }  where id = { job_id };";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if(retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as block_id, facilities.name as block_name, facilities1.name as block_name, facilities.name as facility_name, job.status as status, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, workType.workTypeName as workType,  job.title as job_title, job.description as job_description " +
                                      "FROM " +
                                            "jobs as job " +
                                      "JOIN " +
                                            "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                      "JOIN " +
                                            "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                      "LEFT JOIN " +
                                            "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                      "JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "JOIN " +
                                            "facilities as facilities1 ON job.blockId = facilities1.id " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId" +
                                      " WHERE job.id= " + job_id;

            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, retValue, 0, 0, "Permit Canceled", CMMS.CMMS_Status.JOB_CANCELLED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CANCELLED, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList[0].id} Canceled");
            return response;
        }
        internal async Task<CMDefaultResponse> LinkToPTW(int job_id, int ptw_id)
        {
            /*
                 *Get id and what parameter changed
                 * AssignedID/ PermitID / CancelJob.Out of 3 we can update any one fields based on request
                 * Re-assigned employee / link permit / Cancel Permit. 3 different end points call this function.     
                 * return boolean true / false    
                 Your code goes here
            */
            string updateQry = $"update jobs set linkedPermit = { ptw_id }  where id =  { job_id };";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as block_id, facilities.name as block_name, facilities1.name as block_name, facilities.name as facility_name, job.status as status, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, workType.workTypeName as workType,  job.title as job_title, job.description as job_description " +
                                      "FROM " +
                                            "jobs as job " +
                                      "JOIN " +
                                            "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                      "JOIN " +
                                            "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                      "LEFT JOIN " +
                                            "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                      "JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "JOIN " +
                                            "facilities as facilities1 ON job.blockId = facilities1.id " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId" +
                                      " WHERE job.id= " + job_id;
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, retVal, 0, 0, "Permit Assigned", CMMS.CMMS_Status.JOB_LINKED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_LINKED, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList[0].id} Linked To Permit");

            return response;        
        }
    }
}
