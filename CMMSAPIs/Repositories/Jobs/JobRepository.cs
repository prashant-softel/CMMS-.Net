using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

        internal async Task<List<CMJobView>> GetJobView(int jobID, string facilitytimeZone)
        {
            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as facility_id, facilities.name as facility_name, blocks.id as block_id, blocks.name as block_name, job.status as status, created_user.id as created_by_id, CONCAT(created_user.firstName, ' ', created_user.lastName) as created_by_name, user.id as assigned_id, CONCAT(user.firstName, ' ', user.lastName) as assigned_name, job.title as job_title, job.description as job_description, job.breakdownTime as breakdown_time, ptw.id as current_ptw_id, ptw.title as current_ptw_title " +
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
            foreach (var list in _ViewJobList)
            {
                if (list != null && list.breakdown_time != null)
                    list.breakdown_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.breakdown_time);
                if (list != null && list.Breakdown_end_time != null)
                    list.Breakdown_end_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.Breakdown_end_time);
                if (list != null && list.Job_closed_on != null)
                    list.Job_closed_on = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.Job_closed_on);
                if (list != null && list.created_at != null)
                    list.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.created_at);
            }
            return _ViewJobList;
        }

        internal async Task<List<CMJobList>> GetJobListByPermitId(int permitId, string facilitytimeZone)
        {
            string myQuery = $"Select job.id as jobid, job.status as status, concat(user.firstname, ' ', user.lastname) as assignedto, job.title as title,  job.breakdowntime, job.linkedpermit as permitid, group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipmentcat, group_concat(distinct assets.name order by assets.id separator ', ') as equipment from jobs as job left join jobmappingassets as jobassets on job.id = jobassets.jobid left join assetcategories as asset_cat on asset_cat.id = jobassets.categoryid left join assets on assets.id = jobassets.assetid left join users as user on user.id = job.assignedid where job.linkedpermit = {permitId} group by job.id; ";

            List<CMJobList> _ViewJobList = await Context.GetData<CMJobList>(myQuery).ConfigureAwait(false);


            foreach (var job in _ViewJobList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(job.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
                job.status_short = _shortStatus;

                if (job != null && job.breakdownTime != null)
                    job.breakdownTime = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, job.breakdownTime);
            }

            return _ViewJobList;

        }

        //internal async Task<List<CMJobModel>> GetJobList(int facility_id, int userId)
        internal async Task<List<CMJobModel>> GetJobList(string facility_id, string startDate, string endDate, CMMS.CMMS_JobType jobType, bool selfView, int userId, string status, string facilitytimeZone, string categoryid)

        {
            /*
            * Fetch data from Job table for provided facility_id.
            * If facility_id 0 then fetch jobs for all facility (No. Facility assigned to employee)
            * Joins these table for relationship using ids Users, Assets, AssetCategory, Facility
            * id and it string value should be there in list
            * ignore standard action, site permit no

            /*Your code goes here*/
            string myQuery = "SELECT " +

                                 "job.id, job.facilityId as facilityId, facilities.name as facilityName,mapAssets.categoryId, " +
                                 "group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipmentCat, " +
                                 "group_concat(distinct asset.name order by asset.id separator ', ') as workingArea, job.title as jobDetails, " +
                                 "job.description as description, job.createdBy as raisedBy, " +
                                 "CONCAT(rasiedByUser.firstName , ' ' , rasiedByUser.lastName) as raisedByName, job.createdAt as jobDate, " +
                                 "CONCAT(user.firstName , ' ' , user.lastName) as assignedToName, user.id as assignedToId, job.status as status, " +
                                 " job.latestJC as latestJCid , jc.JC_Status as latestJCStatus, jc.JC_Approved as latestJCApproval, job.breakdownTime as breakdownTime, IF(job.breakdownTime = null, 'Non Breakdown Maintenance', 'Breakdown Maintenance') as breakdownType, group_concat(distinct workType.workTypeName order by workType.id separator ', ') as workType, jc.PTW_id as ptw_id, jc.PTW_Code as permitId, permit.status as  latestJCPTWstatus" +
                                 " ,permittypelists.title as permitType,Isolation FROM " +
                                        "jobs as job " +
                                "LEFT JOIN " +
                                       " jobcards as jc ON job.id = jc.jobId " +
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
                                        "jobworktypes as workType ON workType.id = mapWorkTypes.workTypeId " +
                                 "LEFT JOIN " +
                                        "users as rasiedByUser ON rasiedByUser.id = job.createdBy " +
                                 "LEFT JOIN " +
                                        "users as user ON user.id = job.assignedId " +
                                 "LEFT JOIN " +
                                        "permittypelists on permittypelists.id = permit.typeId";
            if (facility_id != null)
            {
                myQuery += $" Where job.facilityId IN ({facility_id})";
                if ((int)jobType > 0)
                {
                    myQuery += " AND job.JobType = " + (int)jobType;
                }

                if (startDate?.Length > 0 && endDate?.Length > 0)
                {
                    DateTime start = DateTime.Parse(startDate);
                    DateTime end = DateTime.Parse(endDate);
                    if (DateTime.Compare(start, end) < 0)
                        myQuery += " AND DATE_FORMAT(job.createdAt,'%Y-%m-%d') BETWEEN \'" + start.ToString("yyyy-MM-dd") + "\' AND \'" + end.ToString("yyyy-MM-dd") + "\'";
                }

                if (selfView)
                {
                    myQuery += " AND (user.id = " + userId + " OR job.createdBy = " + userId + " OR job.assignedId = " + userId + ")";
                }
                if (categoryid != "" && categoryid != null)
                {

                    myQuery += $"AND mapAssets.categoryId in ({categoryid}) ";
                }

                if (status?.Length > 0)
                    myQuery += " AND job.status IN (" + status + ")";
            }
            else
            {
                throw new ArgumentException("facility id <" + facility_id + "> cannot be empty or 0");
            }
            myQuery += "  GROUP BY   job.id, jc.JC_Approved order by job.id DESC;";

            List<CMJobModel> _JobList = await Context.GetData<CMJobModel>(myQuery).ConfigureAwait(false);

            foreach (CMJobModel _Job in _JobList)
            {
                if (_Job.status == 3)
                {
                    _Job.latestJCStatusShort = "Job Cancelled ";
                }
                else if (_Job.ptw_id == 0)
                {
                    _Job.latestJCStatusShort = "Permit not linked";
                }
                else if (_Job.latestJCid != 0)
                {

                    //if permit status is not yet approved
                    if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                    {
                        _Job.latestJCStatusShort = "Permit - rejected";
                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_CREATED)
                    {
                        _Job.latestJCStatusShort = "Permit - Waiting For Approval";
                    }
                    else //if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                    {
                        _Job.latestJCStatusShort = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_Job.latestJCStatus, (CMMS.ApprovalStatus)_Job.latestJCApproval);
                    }
                }
                else
                {
                    if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                    {

                        _Job.latestJCStatusShort = "Permit - Approved";
                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                    {
                        _Job.latestJCStatusShort = "Permit - rejected";
                    }

                    else
                    {
                        _Job.latestJCStatusShort = "Permit - Pending";
                    }
                }
            }
            foreach (var list in _JobList)
            {
                if (list != null && list.breakdownTime != null)
                    list.breakdownTime = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.breakdownTime);
                if (list != null && list.jobDate != null)
                    list.jobDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.jobDate);

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
                        retValue = String.Format("Job{0} created by {1}", jobId, jobObj.created_by_name);
                    }
                    else
                    {
                        retValue = String.Format("Job{0} created by {1} and assigned to {2}", jobId, jobObj.created_by_name, jobObj.assigned_name);
                    }
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = String.Format("Job{0} assigned to {1}", jobId, jobObj.assigned_name);
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = String.Format("Job{0} linked to PTW{1}", jobId, jobObj.current_ptw_id);
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = String.Format("Job{0} closed", jobId);
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = String.Format("Job{0} cancelled by <{1}>", jobId, jobObj.cancelled_by_name);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<CMJobView> GetJobDetails(int job_id, string facilitytimeZone)
        {
            /*
             * Fetch data from Job table and joins these table for relationship using ids Users, Assets, AssetCategory, Facility
             * id and it string value should be there in list
            */

            /*Your code goes here*/

            string myQuery = "SELECT " +
                                    "job.id as id, facilities.id as facility_id, facilities.name as facility_name, blocks.id as block_id, blocks.name as block_name, job.status as status, job.createdAt as created_at,created_user.id as created_by_id," +
                                    " CONCAT(created_user.firstName, ' ', created_user.lastName) as created_by_name,bus_user.name as Company, user.id as assigned_id, CONCAT(user.firstName, ' ', user.lastName) as assigned_name, job.title as job_title, " +
                                    "job.description as job_description, job.breakdownTime as breakdown_time, ptw.id as current_ptw_id, ptw.title as current_ptw_title, ptw.description as current_ptw_desc, jc.id as latestJCid, " +
                                    " passt.name as Isolated_equipments, CONCAT(tbtDone.firstName, ' ', tbtDone.lastName) as TBT_conducted_by_name, ptw.TBT_Done_At as TBT_done_time,ptw.startDate Start_time, job.cancelledAt as cancelled_at, job.cancelledBy as cancelled_by_id, " +
                                    " jc.JC_Status as latestJCStatus, jc.JC_Approved as latestJCApproval, jc.JC_Date_Stop as Job_closed_on, CONCAT(cancelledByUser.firstName, ' ', cancelledByUser.lastName) as cancelled_by_name, " +
                                    " jc.JC_Date_Stop as Breakdown_end_time,job.breakdownTime as Breakdown_start_time,ptw.status as status_PTW, jobC.JC_End_By_id, CONCAT(isotak.firstName, ' ', isotak.lastName) as Isolation_taken, CONCAT(closedByUser.firstName, ' ', closedByUser.lastName) as closedByName   " +
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
                                            "business as bus_user ON bus_user.id = job.createdby " +
                                      "LEFT JOIN " +
                                            "jobcards as jobC ON jobC.jobId = job.id " +
                                      "LEFT JOIN " +
                                            "users as closedByUser ON jobC.JC_End_By_id = closedByUser.id " +
                                            "LEFT join  assets as passt on ptw.physicalIsoEquips = passt.id " +
                                            "Left join users as isotak on ptw.physicalIsolation = isotak.id  " +
                                            "left join users as tbtDone on ptw.TBT_Done_By = tbtDone.id " +
                                            "LEFT join users as cancelledByUser ON cancelledByUser.id = job.cancelledBy " +
                                      "LEFT JOIN " +
                                            "users as user ON user.id = job.assignedId " +
                                      "WHERE job.id = " + job_id;
            List<CMJobView> _ViewJobList = await Context.GetData<CMJobView>(myQuery).ConfigureAwait(false);

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, (CMMS.CMMS_Status)_ViewJobList[0].status);
            _ViewJobList[0].status_short = _shortStatus;
            _ViewJobList[0].breakdown_type = $"{(CMMS.CMMS_JobType)_ViewJobList[0].job_type}";

            ////get equipmentCat list
            string myQuery12 = "SELECT asset_cat.id as id, asset_cat.name as name  FROM assetcategories as asset_cat " +
                "JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id " +
                "WHERE job.id =" + job_id;
            List<CMequipmentCatList> _equipmentCatList1 = await Context.GetData<CMequipmentCatList>(myQuery12).ConfigureAwait(false);

            //get workingArea_name list 
            string myQuery2 = "SELECT asset.id as id, asset.name as name FROM assets as asset " +
             "JOIN jobmappingassets as mapAssets ON mapAssets.assetId  =  asset.id  JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id =" + job_id;
            List<CMworkingAreaNameList> _WorkingAreaNameList = await Context.GetData<CMworkingAreaNameList>(myQuery2).ConfigureAwait(false);

            //get Associated permits
            string myQuery3 = "SELECT ptw.id as permitId,ptw.status as ptwStatus ,permittypelists.title  as permitTypeName,ptw.title as title, ptw.code as sitePermitNo, ptw.startDate, ptw.endDate,  CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, ptw.status as ptwStatus FROM permits as ptw JOIN jobs as job ON ptw.id = job.linkedPermit " +
                "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                 "left join permittypelists on permittypelists.id = ptw.typeId " +
                "LEFT JOIN st_jc_files as jobCard ON jobCard.JC_id = ptw.id " +
            "WHERE job.id= " + job_id;
            List<CMAssociatedPermitList> _AssociatedpermitList = await Context.GetData<CMAssociatedPermitList>(myQuery3).ConfigureAwait(false);
            if (_AssociatedpermitList.Count > 0)
            {
                _AssociatedpermitList[0].ptwStatus_short = "Linked";    //temp till JOIN is made
            }
            string myQuery4 = "SELECT distinct(workType.id) AS id, workType.workTypeName as workType FROM jobs AS job " +
                "left JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
                "left JOIN assetcategories AS asset_cat ON mapAssets.categoryId = asset_cat.id " +
                "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
                "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
                $"WHERE job.id = {job_id} ";
            List<CMWorkType> _WorkType = await Context.GetData<CMWorkType>(myQuery4).ConfigureAwait(false);

            string myQuery5 = "SELECT Distinct tools.id as id, tools.assetName as linkedToolName FROM jobs AS job " +
                "JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
                "JOIN assets ON mapAssets.assetId = assets.id " +
                "JOIN assetcategories AS asset_cat ON assets.categoryId = asset_cat.id " +
                "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
                "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
                "LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId=workType.id " +
                "LEFT JOIN worktypemasterassets AS tools ON tools.id=mapTools.ToolId " +
                $"WHERE job.id = {job_id} AND  tools.status=1 group by tools.id ;";
            List<CMWorkTypeTool> _Tools = await Context.GetData<CMWorkTypeTool>(myQuery5).ConfigureAwait(false);
            //$" tools.id IS NOT NULL AND tools.assetName IS NOT NULL GROUP BY tools.id, tools.assetName";

            if (_ViewJobList[0].current_ptw_id == 0)
            {
                _ViewJobList[0].latestJCStatusShort = "Permit not linked";
            }
            else if (_ViewJobList[0].latestJCid != 0)
            {
                //if permit status is not yet approved
                if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - rejected";
                }
                else if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_CREATED)
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - Waiting For Approval";
                }
                else
                {
                    _ViewJobList[0].latestJCStatusShort = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_ViewJobList[0].latestJCStatus, (CMMS.ApprovalStatus)_ViewJobList[0].latestJCApproval);
                }

            }
            else
            {
                if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                {

                    _ViewJobList[0].latestJCStatusShort = "Permit - Approved";
                }
                else if (_AssociatedpermitList[0].ptwStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - rejected";
                }
                else
                {
                    _ViewJobList[0].latestJCStatusShort = "Permit - Pending";
                }
            }
            //get equipmentCat list
            string myQuery1 = "SELECT asset_cat.id as id, asset_cat.name as name  " +
                "FROM assetcategories as asset_cat" +
                " where asset_cat.id in ( select distinct equipmentCategoryId from " +
                "jobworktypes where id in (" + String.Join(",", _WorkType.Select(x => x.id).ToList()) + "));";
            List<CMequipmentCatList> _WorktypeCatList = await Context.GetData<CMequipmentCatList>(myQuery1).ConfigureAwait(false);
            _equipmentCatList1.AddRange(_WorktypeCatList);
            _equipmentCatList1 = _equipmentCatList1.GroupBy(x => x.id).Select(g => g.First()).ToList();
            _ViewJobList[0].equipment_cat_list = _equipmentCatList1;
            _ViewJobList[0].working_area_name_list = _WorkingAreaNameList;
            _ViewJobList[0].associated_permit_list = _AssociatedpermitList;
            _ViewJobList[0].work_type_list = _WorkType;
            _ViewJobList[0].tools_required_list = _Tools;

            //add worktype and tools ka collection
            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_ViewJobList[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOB, _Status, _ViewJobList[0]);
            _ViewJobList[0].status_long = _longStatus;
            foreach (var list in _ViewJobList)
            {
                if (list != null && list.breakdown_time != null)
                    list.breakdown_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.breakdown_time);
                if (list != null && list.Breakdown_end_time != null)
                    list.Breakdown_end_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.Breakdown_end_time);
                if (list != null && list.Job_closed_on != null)
                    list.Job_closed_on = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.Job_closed_on);
                if (list != null && list.created_at != null)
                    list.created_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.created_at);
            }

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
            $"({request.facility_id}, {request.block_id}, '{request.title}', '{request.description}', '{UtilsRepository.GetUTCTime()}','{UtilsRepository.GetUTCTime()}',{userId},'{request.breakdown_time.ToString("yyyy-MM-dd HH:mm:ss")}',{(int)request.jobType},{status},{request.assigned_id},{(request.permit_id == null ? 0 : request.permit_id)})";
            qryJobBasic = qryJobBasic + ";" + "select LAST_INSERT_ID(); ";

            DataTable dt = await Context.FetchData(qryJobBasic).ConfigureAwait(false);
            int newJobID = Convert.ToInt32(dt.Rows[0][0]);

            //List<CMJobView> newJob = await Context.GetData<CMJobView>(qry).ConfigureAwait(false);
            //            int newJobID = newJob[0].id;
            if (request.AssetsIds == null)
                request.AssetsIds = new List<int>();

            foreach (var data in request.AssetsIds)
            {
                string qryAssetsIds = $"insert into jobmappingassets(jobId, assetId ) values ({newJobID}, {data});";
                await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            }
            string setCat = $"UPDATE jobmappingassets, assets SET jobmappingassets.categoryId = assets.categoryId WHERE " +
                            $"jobmappingassets.assetId = assets.id;";
            await Context.ExecuteNonQry<int>(setCat).ConfigureAwait(false);
            if (request.WorkType_Ids == null)
                request.WorkType_Ids = new List<int>();

            foreach (var data in request.WorkType_Ids)
            {
                string qryCategoryIds = $"insert into jobassociatedworktypes(jobId, workTypeId) value ( {newJobID}, {data});";
                await Context.ExecuteNonQry<int>(qryCategoryIds).ConfigureAwait(false);
            }
            CMJobView _ViewJobList = await GetJobDetails(newJobID, "");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Created", CMMS.CMMS_Status.JOB_CREATED, userId);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CREATED, new[] { userId }, _ViewJobList);

            string strJobStatusMsg = $"Job{newJobID} Created";
            int assigned_id = 1;
            if (_ViewJobList.assigned_id > 0)
            {
                strJobStatusMsg = $"Job {newJobID} Created and Assigned to " + _ViewJobList.assigned_name;

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, newJobID, 0, 0, "Job Assigned", CMMS.CMMS_Status.JOB_ASSIGNED, userId);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_ASSIGNED, new[] { userId }, _ViewJobList);
            }

            // File Upload code for JOB
            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={newJobID} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
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
            CMJobView _ViewJobList = await GetJobDetails(jobID, "");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, jobID, 0, 0, "Job Updated", CMMS.CMMS_Status.JOB_UPDATED, userId);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_UPDATED, new[] { userId }, _ViewJobList);

            string strJobStatusMsg = $"Job{jobID} Updated";
            CMDefaultResponse response = new CMDefaultResponse(jobID, CMMS.RETRUNSTATUS.SUCCESS, strJobStatusMsg);

            return response;
        }



        internal async Task<CMDefaultResponse> ReAssignJob(int job_id, int assignedTo, int updatedBy)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false*/

            string getstatusqry = $"select status from jobs where id = {job_id} ";
            List<CMJobView> status = await Context.GetData<CMJobView>(getstatusqry).ConfigureAwait(false);

            string updateQry = $"update jobs set assignedId = {assignedTo}, statusUpdatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = {updatedBy}";

            if (status[0].status == (int)CMMS.CMMS_Status.JOB_CREATED)
            {
                updateQry += $", status = {(int)CMMS.CMMS_Status.JOB_ASSIGNED}";
            }

            updateQry += $" where id = {job_id}";

            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMJobView _ViewJobList = await GetJobDetails(job_id, "");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, 0, 0, $"Job Assigned to {_ViewJobList.assigned_name}", CMMS.CMMS_Status.JOB_ASSIGNED, updatedBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_ASSIGNED, new[] { assignedTo }, _ViewJobList);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList.id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList.id} Assigned");
            return response;
        }
        /*
        internal async Task<CMDefaultResponse> DeleteJob(int job_id, int deleteBy)
        {
            string deleteQry = $"delete from jobs where id = {job_id}; " + 
            $"delete from jobassociatedworktypes where jobassociatedworktypes.jobId = {job_id}; " + 
            $"delete from jobmappingassets where jobmappingassets.jobId = {job_id}; ";
            
            List<CMJobView> _ViewJobList = await GetJobDetails(job_id);
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
            string updateQry = $"update jobs set updatedBy = {cancelledBy}, statusUpdatedAt = '{UtilsRepository.GetUTCTime()}', cancelledBy = {cancelledBy}, cancelledAt = '{UtilsRepository.GetUTCTime()}',cancellationRemarks = '{Cancelremark}',  status = {(int)CMMS.CMMS_Status.JOB_CANCELLED}, cancelStatus = 'Y'  where id = {job_id};";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMJobView _ViewJobList = await GetJobDetails(job_id, "");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, 0, 0, "Job Cancelled", CMMS.CMMS_Status.JOB_CANCELLED, cancelledBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CANCELLED, new[] { _ViewJobList.assigned_id }, _ViewJobList);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList.id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {_ViewJobList.id} Cancelled");
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
            string updateQry = $"update jobs set updatedBy = {updatedBy},status = {(int)CMMS.CMMS_Status.JOB_LINKED}, statusUpdatedAt = '{UtilsRepository.GetUTCTime()}', linkedPermit = {ptw_id}  where id =  {job_id};";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMJobView _ViewJobList = await GetJobDetails(job_id, "");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, job_id, CMMS.CMMS_Modules.PTW, ptw_id, $"Permit <{ptw_id}> Assigned to Job <{job_id}>", CMMS.CMMS_Status.JOB_LINKED, updatedBy);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_LINKED, new[] { _ViewJobList.assigned_id }, _ViewJobList);

            CMDefaultResponse response = new CMDefaultResponse(_ViewJobList.id, CMMS.RETRUNSTATUS.SUCCESS, $"Job <{_ViewJobList.id}> Linked To Permit <{ptw_id}> ");

            return response;
        }
    }
}
