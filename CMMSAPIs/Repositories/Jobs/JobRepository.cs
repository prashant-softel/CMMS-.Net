using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;
using System.Data;
using System;
using CMMSAPIs.Models.Users;
using System.Linq;

namespace CMMSAPIs.Repositories.Jobs
{
    public class JobRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;

        private MYSQLDBHelper _conn;
        public JobRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _conn = sqlDBHelper;
        }

        internal async Task<List<CMJobView>> GetJobView(int jobID)
        {
            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as facility_id, facilities.name as facility_name, blocks.id as block_id, blocks.name as block_name, job.status as status, created_user.id as created_by_id, CONCAT(created_user.firstName, created_user.lastName) as created_by_name, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, job.title as job_title, job.description as job_description, job.breakdownTime as breakdown_time, ptw.id as current_ptw_id, ptw.title as current_ptw_title " +
                                      "FROM " +
                                            "jobs as job " +
                                      "LEFT JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "LEFT JOIN " +
                                            "facilities as blocks ON job.blockId = blocks.id " +
                                      "LEFT JOIN " +
                                            "permits as ptw ON job.linkedPermit = ptw.id " +
                                      "LEFT JOIN " +
                                            "users as created_user ON created_user.id = job.createdby " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId " +
                                      "WHERE job.id = " + jobID;
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);
            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_ViewJobList[0].status);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
            _ViewJobList[0].status_short = _shortStatus;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewJobList[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOB, _Status_long, _ViewJobList[0]);
            _ViewJobList[0].status_long = _longStatus;

            return _ViewJobList;
        }

        internal async Task<List<CMJobList>> GetJobListByPermitId(int permitId)
        {
            string myQuery = $"Select job.id as jobid, job.status as status, concat(user.firstname, ' ', user.lastname) as assignedto, job.title as title,  job.breakdowntime, job.linkedpermit as permitid, group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipmentcat, group_concat(distinct assets.name order by assets.id separator ', ') as equipment from jobs as job left join jobmappingassets as jobassets on job.id = jobassets.jobid left join assetcategories as asset_cat on asset_cat.id = jobassets.categoryid left join assets on assets.id = jobassets.assetid left join users as user on user.id = job.assignedid where job.linkedpermit = {permitId} group by job.id; ";

            List<CMJobList> _ViewJobList = await Context.GetData<CMJobList>(myQuery).ConfigureAwait(false);

            
            foreach (var job in _ViewJobList)
            {          
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(job.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
                job.status_short = _shortStatus;
            }

            return _ViewJobList;

        }

        //internal async Task<List<CMJobModel>> GetJobList(int facility_id, int userId)
        internal async Task<List<CMJobModel>> GetJobList(int facility_id, string startDate, string endDate, CMMS.CMMS_JobType jobType, int selfView, int userId, string status)

        {
            /*
            * Fetch data from Job table for provided facility_id.
            * If facility_id 0 then fetch jobs for all facility (No. Facility assigned to employee)
            * Joins these table for relationship using ids Users, Assets, AssetCategory, Facility
            * id and it string value should be there in list
            * ignore standard action, site permit no

            /*Your code goes here*/
            string myQuery = "SELECT " +
                                 //                                 "job.id, job.facilityId, user.id, facilities.name as plantName, job.status as status, job.createdAt as jobDate, DATE_FORMAT(job.breakdownTime, '%Y-%m-%d') as breakdown_time, job.id as id, asset_cat.name as equipmentCat, asset.name as workingArea, job.title as jobDetails, workType.workTypeName as workType, permit.code as permitId, job.createdBy as raisedBy, CONCAT(user.firstName , ' ' , user.lastName) as assignedToName, user.id as assignedToId, IF(job.breakdownTime = '', 'Non Breakdown Maintenance', 'Breakdown Maintenance') as breakdownType , job.description as description" +
                                 "job.id, job.facilityId as facilityId, facilities.name as facilityName, group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipmentCat, group_concat(distinct asset.name order by asset.id separator ', ') as workingArea, job.title as jobDetails, job.description as description, job.createdBy as raisedBy, CONCAT(rasiedByUser.firstName , ' ' , rasiedByUser.lastName) as raisedByName, job.createdAt as jobDate, CONCAT(user.firstName , ' ' , user.lastName) as assignedToName, user.id as assignedToId, job.status as status, jc.id as latestJCid, jc.JC_Status as latestJCStatus, jc.JC_Approved as latestJCApproval, DATE_FORMAT(job.breakdownTime, '%Y-%m-%d') as breakdown_time, IF(job.breakdownTime = '', 'Non Breakdown Maintenance', 'Breakdown Maintenance') as breakdownType, group_concat(distinct workType.workTypeName order by workType.id separator ', ') as workType, permit.id as ptw_id, permit.code as permitId, permit.status as  latestJCPTWstatus" +
                                 " FROM " +
                                        "jobs as job " +
                                "LEFT JOIN " +
                                        "jobcards as jc ON job.latestJC = jc.id " +
                                "LEFT JOIN  " +
                                        "facilities as facilities ON job.facilityId = facilities.id " +
                                "LEFT JOIN " +
                                        "users as created_user ON created_user.id = job.createdBy " +
                                "LEFT JOIN " +
                                        "jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                 "LEFT JOIN " +
                                        "assets as asset ON mapAssets.assetId  =  asset.id " +
                                 "LEFT JOIN " +
                                        "assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                 "LEFT JOIN " +
                                        "permits as permit ON permit.id = job.linkedPermit " +
                                "LEFT JOIN " +
                                        "jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
                                 "LEFT JOIN " +
                                        "jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                                 "LEFT JOIN " +
                                        "users as rasiedByUser ON rasiedByUser.id = job.createdBy " +
                                 "LEFT JOIN " +
                                        "users as user ON user.id = job.assignedId ";
            if (facility_id > 0)
            {
                myQuery += " WHERE job.facilityId = " + facility_id + " AND job.JobType = " + (int)jobType;
                if (startDate?.Length > 0 && endDate?.Length > 0)
                {
                    DateTime start = DateTime.Parse(startDate);
                    DateTime end = DateTime.Parse(endDate);
                    if (DateTime.Compare(start, end) < 0)
                        myQuery += " AND DATE_FORMAT(job.createdAt,'%Y-%m-%d') BETWEEN \'" + startDate + "\' AND \'" + endDate + "\'";
                }

                if (selfView > 0)
                    myQuery += " AND (user.id = " + userId + " OR created_user.id = " + userId + ")";

                if (status?.Length > 0)
                    myQuery += " AND job.status IN ("+status+")";
            }
            else
            {
                throw new ArgumentException("facility id <" + facility_id + "> cannot be empty or 0");
            }
            myQuery += " GROUP BY job.id DESC;";

            List<CMJobModel> _JobList = await Context.GetData<CMJobModel>(myQuery).ConfigureAwait(false);

            foreach(CMJobModel _Job in _JobList)
            {
                if (_Job.ptw_id == 0)
                {
                    _Job.latestJCStatusShort = "Permit not linked";
                }
                else if (_Job.latestJCid != 0)
                {
                    //if permit status is not yet approved
                    if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                    {
                        _Job.latestJCStatusShort = "job card created";
                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                    {
                        _Job.latestJCStatusShort = "Permit - rejected";
                    }
                    else
                    {
                        _Job.latestJCStatusShort = "Permit - Waiting For Approval";
                    }
                }
                else
                {
                    _Job.latestJCStatusShort = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_Job.latestJCStatus, (CMMS.ApprovalStatus)_Job.latestJCApproval);
                }
            }

            return _JobList;
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = "Job Created";
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = "Job Assigned";
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = "Job Linked to PTW";
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = "Job Closed";
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = "Job Cancelled";
                    break;
                default:
                    retValue = "Unknown Status";
                    break;
            }
            return retValue;

        }


        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJobView jobObj)
        {
                string retValue = "Job";
                int jobId = jobObj.id;

                switch (notificationID)
                {
                    case CMMS.CMMS_Status.JOB_CREATED:     //Created
                                                           //description is sent at 1 index of arg for this notification, so developer fetch it and use to format the subject
                        string desc = jobObj.job_description;
                    if (string.IsNullOrEmpty(jobObj.assigned_name))
                    {
                        retValue = String.Format("Job {0} created by", jobObj.created_by_name);
                    }
                    else
                    {
                        retValue = String.Format("Job {0} Created by and Assigned to", jobObj.created_by_name, jobObj.assigned_name);
                    }
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
                                    "job.id as id, facilities.id as facility_id, facilities.name as facility_name, blocks.id as block_id, blocks.name as block_name, job.status as status, created_user.id as created_by_id, CONCAT(created_user.firstName, created_user.lastName) as created_by_name, user.id as assigned_id, CONCAT(user.firstName, user.lastName) as assigned_name, job.title as job_title, job.description as job_description, job.breakdownTime as breakdown_time, ptw.id as current_ptw_id, ptw.title as current_ptw_title, ptw.description as current_ptw_desc, jc.id as latestJCid, jc.JC_Status as latestJCStatus, jc.JC_Approved as latestJCApproval " +
                                      "FROM " +
                                            "jobs as job " +
                                      "LEFT JOIN " +
                                            "jobcards as jc ON job.latestJC = jc.id " +
                                      " LEFT JOIN " +
                                            "facilities as facilities ON job.facilityId = facilities.id " +
                                      "LEFT JOIN " +
                                            "facilities as blocks ON job.blockId = blocks.id " +
                                      "LEFT JOIN " +
                                            "permits as ptw ON job.linkedPermit = ptw.id " +
                                      "LEFT JOIN " +
                                            "users as created_user ON created_user.id = job.createdby " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId " +
                                      "WHERE job.id = " + job_id;
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, (CMMS.CMMS_Status)_ViewJobList[0].status);
            _ViewJobList[0].status_short = _shortStatus;
            _ViewJobList[0].breakdown_type = $"{(CMMS.CMMS_JobType)_ViewJobList[0].job_type}";           

            //get equipmentCat list
            string myQuery1 = "SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat " +
                "JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMequipmentCatList> _equipmentCatList = await Context.GetData<CMequipmentCatList>(myQuery1).ConfigureAwait(false);

            //get workingArea_name list 
            string myQuery2 = "SELECT asset.id as workingArea_id, asset.name as workingArea_name FROM assets as asset " +
             "JOIN jobmappingassets as mapAssets ON mapAssets.assetId  =  asset.id  JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMworkingAreaNameList> _WorkingAreaNameList = await Context.GetData<CMworkingAreaNameList>(myQuery2).ConfigureAwait(false);

            //get Associated permits
            string myQuery3 = "SELECT ptw.id as permitId,ptw.status as ptwStatus ,ptw.title as title, ptw.code as sitePermitNo, ptw.startDate, ptw.endDate,  CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, ptw.status as ptwStatus FROM permits as ptw JOIN jobs as job ON ptw.id = job.linkedPermit " +
                "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                "LEFT JOIN st_jc_files as jobCard ON jobCard.JC_id = ptw.id " +
            "WHERE job.id= " + job_id;
            List<CMAssociatedPermitList> _AssociatedpermitList = await Context.GetData<CMAssociatedPermitList>(myQuery3).ConfigureAwait(false);
            if(_AssociatedpermitList.Count > 0)
			{
				_AssociatedpermitList[0].ptwStatus_short = "Linked";    //temp till JOIN is made
			}
            string myQuery4 = "SELECT workType.id AS workTypeId, workType.workTypeName as workTypeName FROM jobs AS job " +
                "JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
                "JOIN assetcategories AS asset_cat ON mapAssets.categoryId = asset_cat.id " +
                "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
                "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
                $"WHERE job.id = {job_id} GROUP BY workType.id";
            List<CMWorkType> _WorkType = await Context.GetData<CMWorkType>(myQuery4).ConfigureAwait(false);

            string myQuery5 = "SELECT tools.id as toolId, tools.assetName as toolName FROM jobs AS job " + 
                "JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
                "JOIN assets ON mapAssets.assetId = assets.id " +
                "JOIN assetcategories AS asset_cat ON assets.categoryId = asset_cat.id " +
                "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " + 
                "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
                "LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId=workType.id " + 
                "LEFT JOIN worktypemasterassets AS tools ON tools.id=mapTools.ToolId " +
                $"WHERE job.id = {job_id} GROUP BY tools.id";
            List<CMWorkTypeTool> _Tools = await Context.GetData<CMWorkTypeTool>(myQuery5).ConfigureAwait(false);

            if (_ViewJobList[0].current_ptw_id == 0)
            {
                _ViewJobList[0].latestJCStatusShort = "Permit not linked";
            }
            else if (_ViewJobList[0].latestJCid != 0)
            {
                //if permit status is not yet approved
                if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                {
                    _ViewJobList[0].latestJCStatusShort = "job card created";
                }
                else if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - rejected";
                }
                else
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - Waiting For Approval";
                }
            }
            else
            {
                _ViewJobList[0].latestJCStatusShort = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_ViewJobList[0].latestJCStatus, (CMMS.ApprovalStatus)_ViewJobList[0].latestJCApproval);
            }

            _ViewJobList[0].equipment_cat_list = _equipmentCatList;
            _ViewJobList[0].working_area_name_list = _WorkingAreaNameList;
            _ViewJobList[0].associated_permit_list = _AssociatedpermitList;
            _ViewJobList[0].work_type_list = _WorkType;
            _ViewJobList[0].tools_required_list = _Tools;
            //add worktype and tools ka collection
            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewJobList[0].status + 100);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOB, _Status_long, _ViewJobList[0]);
            _ViewJobList[0].status_long = _longStatus;
            return _ViewJobList[0];
        }

        internal async Task<CMDefaultResponse> CreateNewJob(CMCreateJob request, int userId)
        {
            /*
             * Job basic details will go to Job table
             * Job associated assets and category will go in JobMappingAssets table (one to many relation)
             * Job associated work type will go in JobAssociatedWorkTypes
             * return value will be inserted record. Use GetJobDetail() function
            */
            //if(request.AssetsIds.Count > 10)
            //    throw new 
            /*Your code goes here*/
            int status = ((int)CMMS.CMMS_Status.JOB_CREATED);
            if (request.assigned_id > 0)
            {
                status = ((int)CMMS.CMMS_Status.JOB_ASSIGNED);
            }
            //int created_by = Utils.UtilsRepository.GetUserID();
            if (request.jobType == null)
                request.jobType = CMMS.CMMS_JobType.BreakdownMaintenance;
            string qryJobBasic = "insert into jobs(facilityId, blockId, title, description, statusUpdatedAt, createdAt, createdBy, breakdownTime, JobType, status, assignedId, linkedPermit) values" +
            $"({ request.facility_id }, { request.block_id }, '{ request.title }', '{ request.description }', '{UtilsRepository.GetUTCTime() }','{UtilsRepository.GetUTCTime() }',{ userId},'{  request.breakdown_time.ToString("yyyy-MM-dd HH:mm:ss")}',{(int)request.jobType},{ status },{ request.assigned_id },{ (request.permit_id==null?0:request.permit_id) })";
            qryJobBasic = qryJobBasic + ";" + "select LAST_INSERT_ID(); ";

            DataTable dt = await Context.FetchData(qryJobBasic).ConfigureAwait(false);
            int newJobID = Convert.ToInt32(dt.Rows[0][0]);

            //List<CMJobView> newJob = await Context.GetData<CMJobView>(qry).ConfigureAwait(false);
            //            int newJobID = newJob[0].id;
            if (request.AssetsIds == null)
                request.AssetsIds = new List<int>();

            foreach (var data in request.AssetsIds)
            {
                string qryAssetsIds = $"insert into jobmappingassets(jobId, assetId ) values ({ newJobID }, { data });";
                await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            }
            string setCat = $"UPDATE jobmappingassets, assets SET jobmappingassets.categoryId = assets.categoryId WHERE jobmappingassets.assetId = assets.id;";
            await Context.ExecuteNonQry<int>(setCat).ConfigureAwait(false);
            if (request.WorkType_Ids == null)
                request.WorkType_Ids = new List<int>();

            foreach (var data in request.WorkType_Ids)
            {
               string qryCategoryIds = $"insert into jobassociatedworktypes(jobId, workTypeId ) value ( { newJobID }, { data } );";
                await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
            }


            List<CMJobView> _ViewJobList = await GetJobView(newJobID);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Created", CMMS.CMMS_Status.JOB_CREATED,userId);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CREATED, new[] { userId },_ViewJobList[0]);

            string strJobStatusMsg = $"Job {newJobID} Created";
            if (_ViewJobList[0].assigned_id > 0)
            {     
				        strJobStatusMsg = $"Job {newJobID} Created and Assigned to " + _ViewJobList[0].assigned_name;        
        
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Assigned", CMMS.CMMS_Status.JOB_ASSIGNED, userId);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_ASSIGNED, new[]{ userId },_ViewJobList[0]);
            }

            CMDefaultResponse response = new CMDefaultResponse(newJobID, CMMS.RETRUNSTATUS.SUCCESS, strJobStatusMsg);

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateJob(CMCreateJob request, int userId)
        {
            //Build and Add update query here
            string myQuery = "UPDATE jobs SET ";
            if (request.title != null && request.title != "")
                myQuery += $"title = '{request.title}', ";
            if (request.description != null && request.description != "")
                myQuery += $"description = '{request.description}', ";
            if (request.facility_id > 0)
                myQuery += $"facilityId = {request.facility_id}, ";
            if (request.block_id > 0)
                myQuery += $"blockId = {request.block_id}, ";
            if (request.assigned_id > 0)
                myQuery += $"assignedId = {request.assigned_id}, ";
            if (request.permit_id != null)
                myQuery += $"linkedPermit = {request.permit_id}, ";
            if (request.jobType != null)
                myQuery += $"JobType = {request.jobType}, ";
            if (request.breakdown_time != null)
                myQuery += $"breakdownTime = '{request.breakdown_time.ToString("yyyy-MM-dd HH:mm:ss")}', ";
            myQuery += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = {userId} WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            if (request.AssetsIds != null)
            {
                string deleteAssets = $"DELETE FROM jobmappingassets WHERE jobId = {request.id};";
                await Context.ExecuteNonQry<int>(deleteAssets).ConfigureAwait(false);
                if (request.AssetsIds.Count > 0)
                {
                    foreach (var data in request.AssetsIds)
                    {
                        string qryAssetsIds = $"insert into jobmappingassets(jobId, assetId ) values ({request.id}, {data});";
                        await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
                    }
                }
            }
            string setCat = $"UPDATE jobmappingassets, assets SET jobmappingassets.categoryId = assets.categoryId WHERE jobmappingassets.assetId = assets.id;";
            await Context.ExecuteNonQry<int>(setCat).ConfigureAwait(false);
            if (request.WorkType_Ids != null)
            {
                string deleteWorkType = $"DELETE FROM jobassociatedworktypes WHERE jobId = {request.id};";
                await Context.ExecuteNonQry<int>(deleteWorkType).ConfigureAwait(false);
                if (request.WorkType_Ids.Count > 0)
                {
                    foreach (var data in request.WorkType_Ids)
                    {
                        string qryCategoryIds = $"insert into jobassociatedworktypes(jobId, workTypeId ) values ( {request.id}, {data} );";
                        await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
                    }
                }
            }
            int jobID = request.id;
            List<CMJobView> _ViewJobList = await GetJobView(jobID);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, jobID, 0, 0, "Job Updated", CMMS.CMMS_Status.JOB_UPDATED, userId);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_UPDATED, new[] {userId}, _ViewJobList[0]);

            string strJobStatusMsg = $"Job {jobID} Updated";
            CMDefaultResponse response = new CMDefaultResponse(jobID, CMMS.RETRUNSTATUS.SUCCESS, strJobStatusMsg);

            return response;
        }



        internal async Task<CMDefaultResponse> ReAssignJob(int job_id, int assignedTo, int updatedBy)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false*/
            string updateQry = $"update jobs set assignedId = { assignedTo }, statusUpdatedAt = '{UtilsRepository.GetUTCTime()}', status = { (int)CMMS.CMMS_Status.JOB_ASSIGNED }, updatedBy = { updatedBy } where id = { job_id } ";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if(retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }



            List<CMJobView> _ViewJobList = await GetJobView(job_id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, 0, 0, $"Job Assigned to {_ViewJobList[0].assigned_name}", CMMS.CMMS_Status.JOB_ASSIGNED, updatedBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_ASSIGNED, new[] {assignedTo}, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList[0].id} Assigned");
            return response;
        }
        /*
        internal async Task<CMDefaultResponse> DeleteJob(int job_id, int deleteBy)
        {
            string deleteQry = $"delete from jobs where id = {job_id}; " + 
            $"delete from jobassociatedworktypes where jobassociatedworktypes.jobId = {job_id}; " + 
            $"delete from jobmappingassets where jobmappingassets.jobId = {job_id}; ";
            
            List<CMJobView> _ViewJobList = await GetJobView(job_id);
            _ViewJobList[0].status = (int)CMMS.CMMS_Status.DELETED;

            int retVal = await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            
            if(retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, 0, 0, "Job Deleted", CMMS.CMMS_Status.JOB_DELETED, deleteBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_DELETED, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList[0].id} Deleted");
            return response;
        }
        /**/
        internal async Task<CMDefaultResponse> CancelJob(int job_id, int cancelledBy, string Cancelremark)
        {
            /*Your code goes here*/
            string updateQry = $"update jobs set updatedBy = { cancelledBy }, statusUpdatedAt = '{UtilsRepository.GetUTCTime()}', cancellationRemarks = '{ Cancelremark }',  status = { (int)CMMS.CMMS_Status.JOB_CANCELLED }, cancelStatus = 'N'  where id = { job_id };";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if(retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            List<CMJobView> _ViewJobList = await GetJobView(job_id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, 0, 0, "Job Cancelled", CMMS.CMMS_Status.JOB_CANCELLED, cancelledBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CANCELLED,new[] { _ViewJobList[0].assigned_id }, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList[0].id} Cancelled");
            return response;
        }
        internal async Task<CMDefaultResponse> LinkToPTW(int job_id, int ptw_id, int updatedBy)
        {
            /*
                 *Get id and what parameter changed
                 * AssignedID/ PermitID / CancelJob.Out of 3 we can update any one fields based on request
                 * Re-assigned employee / link permit / Cancel Permit. 3 different end points call this function.     
                 * return boolean true / false    
                 Your code goes here
            */
            string updateQry = $"update jobs set updatedBy = { updatedBy },status = { (int) CMMS.CMMS_Status.JOB_LINKED }, statusUpdatedAt = '{UtilsRepository.GetUTCTime()}', linkedPermit = { ptw_id }  where id =  { job_id };";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            List<CMJobView> _ViewJobList = await GetJobView(job_id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, CMMS.CMMS_Modules.PTW, ptw_id, $"Permit <{ptw_id}> Assigned to Job <{job_id}>", CMMS.CMMS_Status.JOB_LINKED, updatedBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_LINKED, new[] { _ViewJobList[0].assigned_id}, _ViewJobList[0]);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job <{_ViewJobList[0].id}> Linked To Permit <{ptw_id}> ");

            return response;        
        }
    }
}
