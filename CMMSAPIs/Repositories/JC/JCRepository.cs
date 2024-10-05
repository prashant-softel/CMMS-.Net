using CMMSAPIs.Helper;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.JC
{
    public class JCRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private MYSQLDBHelper _conn;
        public JCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _conn = sqlDBHelper;
        }


        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID, CMMS.ApprovalStatus approval_id)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    retValue = "JC Created";
                    break;
                case CMMS.CMMS_Status.JC_STARTED:
                    retValue = "JC Started";
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = "JC Closed Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = "JC Closed Rejected";
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:
                    retValue = "JC Closed Approved";
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue = "JC Carry Forwarded - Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:
                    retValue = "JC Carry Forwarded Approved";
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = "JC Carry Forwarded Rejected";
                    break;

                //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:     
                //    retValue = "PTW Timed Out";
                //    break;

                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJCDetail jobCardObj)
        {
            string retValue = "My job subject";
            int jobCardId = jobCardObj.id;

            switch (notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    retValue = String.Format("JobCard JC{0} Created by {1}", jobCardId, jobCardObj.created_by);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:
                    retValue = String.Format("JobCard JC{0} Started by at {1}", jobCardId, jobCardObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue = String.Format("JobCard JC{0} Carry Forward by {1} - Waiting for approval ", jobCardId, jobCardObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:
                    retValue = String.Format("JobCard JC{0} Carry Forward and Approved by {1}", jobCardId, jobCardObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = String.Format("JobCard JC{0} Carry Forward but Rejected by {1}", jobCardId, jobCardObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = String.Format("JobCard JC{0} Closed by {1} - Waiting for approval", jobCardId, jobCardObj.JC_Closed_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:
                    retValue = String.Format("JobCard JC{0} Closed and Approved by {1}", jobCardId, jobCardObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = String.Format("JobCard JC{0} Closed but Rejected by {1}", jobCardId, jobCardObj.JC_Rejected_By_Name);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMJCList>> GetJCList(string facility_id, int userID, bool self_view, string facilitytimeZone)
        {
            /* Return all field mentioned in JCListModel model
            *  tables are JobCards, Jobs, Permit, Users
            */
            string myQuery1 = $"select jc.id as jobCardId,jc.JC_Status as status ,jc.JC_Approved as approvedStatus, jc.JC_Date_Start as job_card_date, " +
                $"jc.JC_Date_Start as start_time,jc.JC_Date_Stop as end_time, job.id as jobid, job.title as description, " +
                $"CONCAT(user.firstName, user.lastName) as job_assinged_to,  ptw.id as permit_id, ptw.code as permit_no,  JC_Status as current_status ,  " +
                $" f.name as site_name,jc.JC_Date_Start as jobs_closed,job.breakdownTime,permitType.title as permit_type,CONCAT(isotak.firstName,isotak.lastName) as Isolation_taken " +
                $"from jobcards as jc " +
                $"JOIN jobs as job ON JC.jobid = job.id  " +
                $"JOIN permits as ptw ON JC.PTW_id = PTW.ID " +
                $"Left Join facilities as f on f.id = jc.facility_Id " +
                $"LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
                $" LEFT JOIN users as isotak ON isotak.id = ptw.physicalIsolation  " +
                $"LEFT JOIN users as user ON user.id = job.assignedId ";
            //$"LEFT JOIN  users as user2 ON user2.id = jc.JC_Added_by " +
            //$"LEFT JOIN  users as user3 ON user3.id = jc.JC_Start_By_id " ;

            //if (facility_id > 0)
            if (!string.IsNullOrEmpty(facility_id))
            {
                myQuery1 += $"WHERE job.facilityId IN ({facility_id}) ";

                if (self_view)
                    myQuery1 += $"AND ( jc.JC_Added_by = {userID} OR job.assignedId = {userID}  ) ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }


            List<CMJCList> _ViewJobCardList = await Context.GetData<CMJCList>(myQuery1).ConfigureAwait(false);

            foreach (var jc in _ViewJobCardList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(jc.status);
                CMMS.ApprovalStatus _Approval = (CMMS.ApprovalStatus)(jc.approvedStatus);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOBCARD, _Status, _Approval);
                jc.status_short = _shortStatus;
            }

            foreach (var jc in _ViewJobCardList)
            {
                if (jc.jobid > 0)
                {
                    string myQuery2 = $"SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name,Assets.name as Equipment_name   " +
                                      $"FROM assetcategories as asset_cat JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id  LEFT JOIN assets as Assets ON Assets.id =mapAssets.assetId " +
                                      $" JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id = {jc.jobid} and job.facilityId = {facility_id}";
                    List<equipmentCatList> _equipmentCatList = await Context.GetData<equipmentCatList>(myQuery2).ConfigureAwait(false);
                    _ViewJobCardList[0].LstequipmentCatList = _equipmentCatList;
                }
            }
            foreach (var v in _ViewJobCardList)
            {

                if (v != null && v.job_card_date != null)
                    v.job_card_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, v.job_card_date);
                if (v != null && v.start_time != null)
                    v.start_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, v.start_time);
                if (v != null && v.end_time != null)
                    v.end_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, v.end_time);


            }
            return _ViewJobCardList;
        }
        internal async Task<List<CMJCListForJob>> GetJCListByJobId(int jobId, string facilitytimeZone)
        {


            /*Your code goes here*/
            string myQuery1 = $"SELECT jc.id as jobCardId, jc.JC_code as jobCardNo, jc.JC_Date_Start as jobCardDate, jc.JC_Date_Stop as endTime, " +
                  $"job.id as jobId, CONCAT(user.firstName, ' ', user.lastName) as jobAssignedTo, ptw.id as permitId, ptw.status as permitStatus, " +
                  $"ptw.code as permitNo, JC_Status as status, JC_Approved as approvedStatus, " +
                  $"CONCAT(userTBT.firstName, ' ', userTBT.lastName) as TBT_Done_By, TBT_Done_By as TBT_Done_By_id, " +
                  $"case when TBT_Done_At = '0000-00-00 00:00:00' then null else TBT_Done_At end as TBT_Done_At, " +
                  $"CASE WHEN ptw.endDate < '{UtilsRepository.GetUTCTime()}' AND ptw.status = {(int)CMMS.CMMS_Status.PTW_APPROVED} THEN 1 ELSE 0 END as isExpired " +
                  $"FROM jobcards as jc " +
                  $"LEFT JOIN jobs as job ON jc.jobid = job.id " +
                  $"LEFT JOIN permits as ptw ON jc.PTW_id = ptw.id " +
                  $"LEFT JOIN users as userTBT ON userTBT.id = ptw.TBT_Done_By " +
                  $"LEFT JOIN users as user ON user.id = job.assignedId";


            //$"LEFT JOIN  users as user2 ON user2.id = jc.JC_Added_by " +
            //$"LEFT JOIN  users as user3 ON user3.id = jc.JC_Start_By_id " ;

            if (jobId > 0)
            {
                myQuery1 += $" WHERE JC.jobid = {jobId} ";
            }
            else
            {
                throw new ArgumentException("Invalid Job ID");
            }

            List<CMJCListForJob> _ViewJobCardList = await Context.GetData<CMJCListForJob>(myQuery1).ConfigureAwait(false);

            foreach (var jc in _ViewJobCardList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(jc.status);
                CMMS.ApprovalStatus _Approval = (CMMS.ApprovalStatus)(jc.approvedStatus);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOBCARD, _Status, _Approval);
                jc.status_short = _shortStatus;

                CMMS.CMMS_Status _PermitStatus = (CMMS.CMMS_Status)(jc.permitStatus);
                jc.permit_status_short = PermitRepository.getShortStatus((int)_PermitStatus);

                //Update UTC to timezome time
                if (jc != null && jc.endTime != null)
                    jc.endTime = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, jc.endTime);
                if (jc != null && jc.jobCardDate != null)
                    jc.jobCardDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, jc.jobCardDate);
            }
            //job equipment category
            /* string myQuery2 = $"SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id = {_ViewJobCardList[0].jobid } and job.facilityId = { facility_id }";
             List<equipmentCatList> _equipmentCatList = await Context.GetData<equipmentCatList>(myQuery2).ConfigureAwait(false);

             _ViewJobCardList[0].LstequipmentCatList = _equipmentCatList;*/
            return _ViewJobCardList;

        }

        /*  internal async Task<CMDefaultResponse> StartJC(int jc_id, int userID)
          {
              string myQuery = $"UPDATE jobcards SET JC_Status = {(int)CMMS.CMMS_Status.JC_STARTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Date_Start = '{UtilsRepository.GetUTCTime()}', JC_Start_By_id = {userID} WHERE id = {jc_id};";
              await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

              //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {jc_id}";

              List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id);

              await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, 0, 0, "Job Card Started", CMMS.CMMS_Status.JC_STARTED,userID);

              await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_STARTED, new[] { userID }, _jcDetails[0]);

              CMDefaultResponse response = new CMDefaultResponse(jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Started");

              return response;
          }*/
        internal async Task<CMDefaultResponse> StartJC(CMJCRequest requestList, int userID, string facilitytimeZone)
        {
            int jc_id = requestList.jc_id;
            CMJCDetail request = requestList.request;
            string myQuery = $"UPDATE jobcards SET JC_Status = {(int)CMMS.CMMS_Status.JC_STARTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Date_Start = '{UtilsRepository.GetUTCTime()}', JC_Start_By_id = {userID} WHERE id = {jc_id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {jc_id}";

            //upload Images
            if (request.uploadfile_ids != null)
            {
                string fq = $"SELECT Facility_id from jobcards where id={jc_id}";
                DataTable dt = await Context.FetchData(fq).ConfigureAwait(false);
                int fid = 0;
                if (dt.Rows.Count > 0)
                {
                    fid = Convert.ToInt32(dt.Rows[0][0]);
                    foreach (int data in request.uploadfile_ids)
                    {

                        string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {fid}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={jc_id} where id = {data}";
                        await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                    }
                }
                else
                {
                    int i = 0;
                }
            }


            foreach (var data in request.LstCMJCEmpList)
            {
                string mapChecklistQry = $"insert into  permitemployeelists (employeeId,responsibility,status,JC_id) VALUES ";

                mapChecklistQry += $"({data.id}, '{data.responsibility}', 1 ,{jc_id})";
                await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, 0, 0, "Job Card Started", CMMS.CMMS_Status.JC_STARTED, userID);

            List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_STARTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Started");

            return response;
        }

        internal async Task<List<CMJCDetail>> GetJCDetail(int jc_id, string facilitytimeZone)
        {
            /*
             * Fetch data from JobCards table and joins these table for relationship using ids 
             * Users, Assets, AssetCategory, Facility, PermiEmployeeLists, PermitLotoAssets, PermitIsolatedAssetCategories
             * Return all the field listed in JCDetailModel 
            */

            //plant details 
            string myQuery1 = $"SELECT distinct(jc.id ) as id , jc.PTW_id as ptwId, job.id as jobid, facilities.id as facility_id, facilities.name as plant_name,fc.name as block_name, " +
                                 $" jc.JC_title as title , jc.JC_Description as description, JC_Date_Stop as JC_Closed_At, " +
                                 $"job.assignedId as currentEmpID, JC_Added_by as created_by_id, jc.JC_Added_Date as created_at, jc.JC_Date_Start as JC_Start_At,jc.JC_Done_Description as Remark_new, " +
                                 $"jc.JC_Approved as JC_Approved, jc. JC_Status as status, JC_Approve_Reason, JC_Update_by, JC_Rejected_Reason, JC_Start_By_id, JC_End_By_id as JC_Closed_By_id, " +
                                 $"CONCAT(user.firstName, ' ' , user.lastName) as JC_UpdatedByName, " +
                                 $"CONCAT(user.firstName , ' ' , user.lastName) as UpdatedByName, " +
                                 $"CONCAT(user1.firstName , ' ' , user1.lastName) as JC_Rejected_By_Name, " +
                                 $"CONCAT(user2.firstName , ' ' , user2.lastName) as created_by, " +
                                 $"CONCAT(user3.firstName , ' ' , user3.lastName) as JC_Start_By_Name, " +
                                 $"CONCAT(user4.firstName , ' ' , user4.lastName) as JC_Closed_by_Name, " +
                                 $"CONCAT(user5.firstName , ' ' , user5.lastName) as JC_Approved_By_Name " +
                                 $"FROM jobs as job " +
                                 $"LEFT JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                 $"Left join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                 $"JOIN facilities as facilities ON job.facilityId= facilities.id " +
                                 $"JOIN facilities as fc ON job.blockId = fc.id " +
                                 $"LEFT JOIN jobcards as jc on jc.jobId = job.id " +
                                 $"LEFT JOIN users as user ON user.id = jc.JC_Update_by " +
                                 $"LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id " +
                                 $"LEFT JOIN  users as user2 ON user2.id = jc.JC_Added_by " +
                                 $"LEFT JOIN  users as user3 ON user3.id = jc.JC_Start_By_id " +
                                 $"LEFT JOIN  users as user4 ON user4.id = jc.JC_End_By_id " +
                                 $"LEFT JOIN  users as user5 ON user5.id = jc.JC_Approved_By_id " +
                                 $"where jc.id = {jc_id}";


            List<CMJCDetail> _jcDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);
            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {jc_id} not found");

            //job details
            string myQuery2 = $"SELECT job.id as job_id ,job.status as status, job.title as job_title , " +
                $"jc.JC_Date_Stop as Breakdown_end_time,job.breakdownTime as Breakdown_start_time , jc.JC_Date_Stop as Job_closed_on ," +
                $"CONCAT(user.firstName, user.lastName) as job_assigned_employee_name ,job.createdAt as Job_raised_on,jowt.workTypeName as Type_of_Job, job.description as job_description,CONCAT(apuser.firstName, apuser.lastName) as perform_by,CONCAT(apuser.firstName, apuser.lastName) as Employee_name,apuser.createdBy as Employee_ID ,bus_user.name as Company , " +
                $" TIMESTAMPDIFF(MINUTE, job.breakdownTime, jc.JC_Date_Stop) AS turnaround_time_minutes," +
                $" group_concat(distinct workType.workTypeName order by workType.id separator ', ') as work_type " +
                $" FROM jobs as job " +
                $"JOIN jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                $"JOIN assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                $"LEFT JOIN jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id " +
                $"LEFT JOIN facilities as facilities ON job.facilityId = facilities.id " +
                $"LEFT JOIN users as user ON user.id = job.assignedId " +
                $"LEFT JOIN jobworktypes as jowt ON jowt.id = job.JobType " +
                $"LEFT JOIN users as  apuser ON apuser.id = job.createdBy  " +
                $"LEFT join  business as bus_user ON bus_user.id = job.createdBy " +
                $"JOIN jobcards as jc on jc.jobId = job.id where jc.id = {jc_id}";

            List<CMJCJobDetail> _jobDetails = await Context.GetData<CMJCJobDetail>(myQuery2).ConfigureAwait(false);




            int id = 0;
            foreach (var job in _jobDetails)
            {
                id = job.job_id;
                job.status_short = JobRepository.getShortStatus(CMMS.CMMS_Modules.JOB, (CMMS.CMMS_Status)job.status);
            }

            string myquery12 = $"SELECT distinct (asset_cat.name) as Equipment_category,asst.name as Equipment_name from  assetcategories as asset_cat" +
                              $" Left join jobmappingassets as mapAssets on mapAssets.categoryId = asset_cat.id  " +
                              $" LEFT JOIN assets as asst ON asst.id = mapAssets.assetId " +
                              $"where mapAssets.jobId= {id}";
            List<CMJCAssetName> asset_category_name = await Context.GetData<CMJCAssetName>(myquery12).ConfigureAwait(false);

            //permit details
            string myQuery3 = $"SELECT ptw.id as permit_id,ptw.status as status, ptw.permitNumber as site_permit_no,CASE when ptw.startDate <  now() then 1 else 0 END as tbt_start, " +
                "passt.name as Isolated_equipments, CONCAT(tbtDone.firstName, ' ', tbtDone.lastName) as TBT_conducted_by_name," +
                "ptw.TBT_Done_At as TBT_done_time,ptw.startDate Start_time,ptw.endDate endDate_time, " +
                $"permitType.title as permit_type, ptw.description as permit_description,CONCAT(isotak.firstName,isotak.lastName) as Isolation_taken, " +
                $"CONCAT(user.firstName, user.lastName) as job_created_by_name,CONCAT(tbtDone.firstName, ' ', tbtDone.lastName) as TBT_conducted_by_name, " +
                $"CONCAT(user1.firstName , ' ' , user1.lastName) as permit_issued_by_name, " +
                $"CONCAT(user2.firstName , ' ' , user2.lastName) as permit_approved_by_name,ptw.TBT_Done_Check as TBT_Done_Check   " +
                $"FROM permits as ptw " +
                $"LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
                $" JOIN jobs as job ON ptw.id = job.linkedPermit " +
                $" LEFT JOIN users as user ON user.id = job.assignedId " +
                $" LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                $"LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
                "LEFT join  assets as passt on ptw.physicalIsoEquips = passt.id " +
                "Left join users as isotak on ptw.physicalIsolation = isotak.id  " +
                 "left join users as tbtDone on ptw.TBT_Done_By = tbtDone.id " +
                $"JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = {jc_id}";
            List<CMJCPermitDetail> _permitDetails = await Context.GetData<CMJCPermitDetail>(myQuery3).ConfigureAwait(false);

            foreach (var permit in _permitDetails)
            {
                permit.status_short = PermitRepository.getShortStatus(permit.status);
            }

            // isolated details
            string myQuery4 = $"SELECT asset_cat.name as isolated_assestName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id JOIN jobcards as jc on ptwISOCat.permitId = jc.PTW_id  where jc.id = {jc_id}";
            List<CMJCIsolatedDetail> _isolatedDetails = await Context.GetData<CMJCIsolatedDetail>(myQuery4).ConfigureAwait(false);

            //loto list
            string myQuery5 = $"SELECT  assets_cat.name as isolated_assest_loto FROM assetcategories as assets_cat " +
                $"LEFT JOIN permits as ptw on ptw.id = assets_cat.id " +
                $" LEFT JOIN permitlotoassets AS LOTOAssets on LOTOAssets.PTW_id = ptw.id " +
                $"JOIN jobcards as jc on jc.jobId = ptw.id where jc.id = {jc_id}";
            List<CMJCLotoDetail> _lotoList = await Context.GetData<CMJCLotoDetail>(myQuery5).ConfigureAwait(false);

            // emp list

            //string myQuery6 = $" SELECT CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList JOIN permits as ptw ON ptw.id = ptwEmpList.pwtId LEFT JOIN users as user ON user.id = ptwEmpList.employeeId JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
            string myQuery6 = $" SELECT CONCAT(user.firstName,' ',user.lastName) as name,ptwEmpList.employeeId as id, " +
                $"u.designationName  as designation FROM permitemployeelists as ptwEmpList" +
                $"  LEFT JOIN users as user ON user.id = ptwEmpList.employeeId  " +
                $" LEFT JOIN userdesignation as u ON user.designation_id=u.id " +
                $"where ptwEmpList.JC_id ={jc_id} and ptwEmpList.status=1";
            List<CMJCEmpDetail> _empList = await Context.GetData<CMJCEmpDetail>(myQuery6).ConfigureAwait(false);
            //toollist
            string myQuery7 = "SELECT tools.id as toolId,sum(tools.id) as No_of_tools,tools.assetName as toolName FROM jobs AS job " +
              "JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
              "JOIN assets ON mapAssets.assetId = assets.id " +
              "JOIN assetcategories AS asset_cat ON assets.categoryId = asset_cat.id " +
              "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
              "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
              "LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId=workType.id " +
              "LEFT JOIN worktypemasterassets AS tools ON tools.id=mapTools.ToolId " +
              $"WHERE job.latestJC= {jc_id} GROUP BY tools.id";
            List<CMWorkTypeTools> _ToolsLinked = await Context.GetData<CMWorkTypeTools>(myQuery7).ConfigureAwait(false);

            // file upload 
            //  string myQuery7 = $"SELECT jc.id as id, PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory, PTWFiles.File_Size as fileSize, PTWFiles.status as status FROM st_ptw_files AS PTWFiles LEFT JOIN permits as ptw on  ptw.id = PTWFiles.PTW_id JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
            // string q = "select jobId from  WHERE id ="+jc_id;
            //int job_id   = await Context.FetchData<int>(q).ConfigureAwait(false);
            string myQuery17 = "SELECT jc.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U " +
                              "Left JOIN jobcards as jc on jc.jobid = U.module_ref_id  " +
                              "where module_ref_id =" + id + " and U.module_type = " + (int)CMMS.CMMS_Modules.JOB + ";";

            List<CMFileDetail> _fileUpload = await Context.GetData<CMFileDetail>(myQuery17).ConfigureAwait(false);
            //uploadjobcard
            string myQuery18 = "SELECT jc.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U " +
                              "Left JOIN jobcards as jc on jc.jobid = U.module_ref_id  " +
                              "where module_ref_id =" + jc_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.JOB + ";";

            List<CMFileDetailJc> Jc_image = await Context.GetData<CMFileDetailJc>(myQuery18).ConfigureAwait(false);
            //materal
            //AssocitdMaterial
            string Materialconsumptionofjob = "SELECT sam.ID as Material_ID,sam.asset_name as  Material_name,smi.asset_item_ID as Equipment_ID, " +
                    "smtype.asset_type as  Material_type, smi.used_qty,smi.issued_qty" +
                    " FROM smassetmasters sam LEFT JOIN smrsitems smi ON sam.ID = smi.mrs_ID " +
                    "Left join smmrs as smm on smm.id=smi.mrs_ID " +
                    "Left join smassettypes as smtype on smtype.ID=sam.asset_type_ID " +
                    "left join smassetitems sai on sai.assetMasterID =  sam.id " +
                    $"WHERE  smm.whereUsedRefID={jc_id};";
            List<Materialconsumption> Material = await Context.GetData<Materialconsumption>(Materialconsumptionofjob).ConfigureAwait(false);

            _jcDetails[0].LstCMJCJobDetailList = _jobDetails;
            _jcDetails[0].LstPermitDetailList = _permitDetails;
            _jcDetails[0].asset_category_name = asset_category_name;
            _jcDetails[0].LstCMJCIsolatedDetailList = _isolatedDetails;
            _jcDetails[0].LstCMJCLotoDetailList = _lotoList;
            _jcDetails[0].LstCMJCEmpList = _empList;
            _jcDetails[0].file_list = _fileUpload;
            _jcDetails[0].tool_List = _ToolsLinked;
            _jcDetails[0].file_listJc = Jc_image;
            _jcDetails[0].Material_consumption = Material;


            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_jcDetails[0].status);
            CMMS.ApprovalStatus _Approval = (CMMS.ApprovalStatus)(_jcDetails[0].JC_Approved);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOBCARD, _Status, _Approval);
            _jcDetails[0].status_short = _shortStatus;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_jcDetails[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOBCARD, _Status_long, _jcDetails[0]);
            _jcDetails[0].status_long = _longStatus;
            foreach (var data in _jcDetails)
            {
                if (data != null && data.JC_Start_At != null)
                    data.JC_Start_At = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, data.JC_Start_At);
            }
            return _jcDetails;
        }

        //internal async Task<CMDefaultResponse> CreateJC(int job_id, int userID)
        //{
        //    /* 
        //     * Data will be inserted in Following tables
        //     * JobCards - Primary table (All basic details in inserted)
        //     * PermiEmployeeLists - All jc linked employee
        //     * JCFiles - All uploaded file data
        //     * Please check the JobCard above tables to get idea what values need to insert. 
        //     * Get details from job, permit and add it to Jobcards table
        //     * Return the All Properties in DefaultResponse model
        //    */

        //    //jc basic details
        //    string myQuery = $"SELECT job.id as job_id, ptw.id as ptw_id  FROM permits AS ptw JOIN jobs as job on job.linkedPermit = ptw.id where job.id = { job_id }";
        //    List<CMJCCreate> _JobList = await Context.GetData<CMJCCreate>(myQuery).ConfigureAwait(false);

        //    int jc_id = 0;
        //    string jobQuery = $"SELECT title as job_title, description as job_description,facilityId as facility_id FROM jobs where id = {job_id}";
        //    List<CMJCJobDetail> job1 = await Context.GetData<CMJCJobDetail>(jobQuery).ConfigureAwait(false);
        //    //_JobList.Count(); pending: add check for only one record is returned

        //    foreach (var data in _JobList)
        //    {
        //        int ptw_id = data.ptw_id;
        //        //JC Already exist. Return same one
        //        string jcQuery = $"SELECT id as jc_id FROM jobcards where jobId = { job_id } and PTW_id = { ptw_id } ";
        //        List<CMJCCreate> jcList = await Context.GetData<CMJCCreate>(jcQuery).ConfigureAwait(false);
        //         //jcList
        //        if (jcList.Count > 0)
        //        {
        //            CMDefaultResponse response = new CMDefaultResponse(jcList[0].jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Exists");
        //            return response;
        //        }
        //        else
        //        {
        //            //create new JC
        //            string qryJCBasic = "insert into jobcards" +
        //                                 "(" +
        //                                   "jobId, PTW_id, PTW_Code, JC_title, JC_Description, JC_Added_by, JC_Added_Date, JC_Status, status_updated_at, Facility_id " +
        //                                  ")" +
        //                                  " values" +
        //                                 "(" +
        //                                   $"{ data.job_id }, { data.ptw_id }, 'PTW{ data.ptw_id }', '{ job1[0].job_title }', '{ job1[0].job_description }', {userID}, '{ UtilsRepository.GetUTCTime() }', {(int)CMMS.CMMS_Status.JC_CREATED}, '{UtilsRepository.GetUTCTime()}', {job1[0].facility_id} " +
        //                                 "); SELECT LAST_INSERT_ID();";

        //            DataTable dt = await Context.FetchData(qryJCBasic).ConfigureAwait(false);
        //            jc_id = Convert.ToInt32(dt.Rows[0][0]);
        //        }
        //    }
        //    CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

        //    if (jc_id > 0)
        //    {
        //        string jcCodeQry = "UPDATE jobcards SET JC_Code = CONCAT('JC',id);";
        //        await Context.ExecuteNonQry<int>(jcCodeQry).ConfigureAwait(false);
        //        string latestJCQry = $"UPDATE jobs SET latestJC = {jc_id} WHERE id = {job_id};";
        //        await Context.ExecuteNonQry<int>(latestJCQry).ConfigureAwait(false);
        //        retCode = CMMS.RETRUNSTATUS.SUCCESS;

        //        //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id order by jc.id desc limit 1";

        //        List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id);

        //        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, CMMS.CMMS_Modules.JOB, job_id, "Job Card Created", CMMS.CMMS_Status.JC_CREATED,userID);

        //        await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CREATED, new[] { userID }, _jcDetails[0]);

        //        CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Created");
        //        return response;
        //    }
        //    CMDefaultResponse responseFailure = new CMDefaultResponse(job_id, CMMS.RETRUNSTATUS.FAILURE, "No permit linked to this job");
        //    return responseFailure;

        //}        
        internal async Task<CMDefaultResponse> CreateJC(int job_id, int userID)
        {
            /* 
             * Data will be inserted in Following tables
             * JobCards - Primary table (All basic details in inserted)
             * PermiEmployeeLists - All jc linked employe
             * JCFiles - All uploaded file data
             * Please check the JobCard above tables to get idea what values need to insert. 
             * Get details from job, permit and add it to Jobcards table
             * Return the All Properties in DefaultResponse model
            */

            //jc basic details
            string myQuery = $"SELECT job.id as job_id, ptw.id as ptw_id  FROM permits AS ptw JOIN jobs as job on job.linkedPermit = ptw.id where job.id = {job_id}";
            List<CMJCCreate> _JobList = await Context.GetData<CMJCCreate>(myQuery).ConfigureAwait(false);

            int jc_id = 0;
            string jobQuery = $"SELECT title as job_title, description as job_description,facilityId as facility_id FROM jobs where id = {job_id}";
            List<CMJCJobDetail> job1 = await Context.GetData<CMJCJobDetail>(jobQuery).ConfigureAwait(false);

            string myQuery7 = "SELECT jc.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U " +
                            "Left JOIN jobcards as jc on jc.jobid = U.module_ref_id  " +
                            "where U.module_ref_id =" + job_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.JOB + ";";
            List<CMFileDetail> images = await Context.GetData<CMFileDetail>(myQuery7).ConfigureAwait(false);
            //_JobList.Count(); pending: add check for only one record is returned

            foreach (var data in _JobList)
            {
                int ptw_id = data.ptw_id;
                //JC Already exist. Return same one
                string jcQuery = $"SELECT id as jc_id FROM jobcards where jobId = {job_id} and PTW_id = {ptw_id} ";
                List<CMJCCreate> jcList = await Context.GetData<CMJCCreate>(jcQuery).ConfigureAwait(false);
                //jcList
                if (jcList.Count > 0)
                {
                    CMDefaultResponse response = new CMDefaultResponse(jcList[0].jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Exists");
                    return response;
                }
                else
                {
                    //create new JC
                    string qryJCBasic = "insert into jobcards" +
                                         "(" +
                                           "jobId, PTW_id, PTW_Code, JC_title, JC_Description, JC_Added_by, JC_Added_Date, JC_Status, status_updated_at, Facility_id " +
                                          ")" +
                                          " values" +
                                         "(" +
                                           $"{data.job_id}, {data.ptw_id}, 'PTW{data.ptw_id}', '{job1[0].job_title}', '{job1[0].job_description}', {userID}, '{UtilsRepository.GetUTCTime()}', {(int)CMMS.CMMS_Status.JC_CREATED}, '{UtilsRepository.GetUTCTime()}', {job1[0].facility_id} " +
                                         "); SELECT LAST_INSERT_ID();";

                    DataTable dt = await Context.FetchData(qryJCBasic).ConfigureAwait(false);
                    jc_id = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (jc_id > 0)
            {
                string jcCodeQry = "UPDATE jobcards SET JC_Code = CONCAT('JC',id);";
                await Context.ExecuteNonQry<int>(jcCodeQry).ConfigureAwait(false);
                string latestJCQry = $"UPDATE jobs SET latestJC = {jc_id} WHERE id = {job_id};";
                await Context.ExecuteNonQry<int>(latestJCQry).ConfigureAwait(false);
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

                //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id order by jc.id desc limit 1";

                List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id, "");



                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, CMMS.CMMS_Modules.JOB, job_id, "Job Card Created", CMMS.CMMS_Status.JC_CREATED, userID);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CREATED, new[] { userID }, _jcDetails[0]);

                CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Created");
                return response;
            }
            CMDefaultResponse responseFailure = new CMDefaultResponse(job_id, CMMS.RETRUNSTATUS.FAILURE, "No permit linked to this job");
            return responseFailure;

        }

        internal async Task<CMDefaultResponse> UpdateJC(CMJCUpdate request, int userID)
        {
            //cmdefault response check
            /*
             * Below thing will happen in update function
             * User can upload new files - (JCFiles Tables)
             * Add new comments - History Table (use Utils AddLog functions)
             * Add/Remove employee list - PermiEmployeeLists table
             * Status - JobCards table
             * Return the CMDefaultResponse
            */
            //ISOLATION LIST
            if (request.is_isolation_required == true)
            {
                foreach (var data in request.isolated_list)
                {
                    //pending optimize the query 
                    string qryisolatedCategory = $"update permitisolatedassetcategories set normalisedStatus = {data.normalisedStatus} , normalisedDate = '{UtilsRepository.GetUTCTime()}' where id = {data.isolation_id} ";
                    await Context.ExecuteNonQry<int>(qryisolatedCategory).ConfigureAwait(false);
                }
            }

            //LOTOT LIST
            foreach (var data in request.loto_list)
            {
                string qryPermitlotoAssets = $"update permitlotoassets set lotoRemovedStatus = {data.lotoRemovedStatus} , lotoRemovedDate = '{UtilsRepository.GetUTCTime()}' where id = {data.loto_id}";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            //EMP LIST
            foreach (var data in request.employee_list)
            {
                string delqryPermitEmpList = $"delete from permitemployeelists where JC_id  ={request.id} and employeeId={data.id}";
                await Context.ExecuteNonQry<int>(delqryPermitEmpList).ConfigureAwait(false);

                string qryPermitEmpList = $"insert into  permitemployeelists (employeeId,responsibility,status,JC_id) values({data.id},'{data.responsibility}',1,{request.id}); ";
                int id = await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);

            }
            // JCFILES PENDINGidand history 
            string comment = request.comment;
            if (string.IsNullOrEmpty(request.comment))
            {
                comment = "Job Card Updated";
            }


            string myQuery1 = $"UPDATE `jobcards` set JC_Update_by = {userID}, JC_Update_date = '{UtilsRepository.GetUTCTime()}' where id = {request.id}";

            var result = await Context.ExecuteNonQry<int>(myQuery1);
            //who updated it means = current emp id and name
            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");
            int fid = 0;
            foreach (var id in _jcDetails)
            {
                string fq = $"SELECT id as fid from facilities  where name='{id.plant_name}'";
                DataTable dt = await Context.FetchData(fq).ConfigureAwait(false);
                fid = Convert.ToInt32(dt.Rows[0][0]);
            }

            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {fid}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, comment, CMMS.CMMS_Status.JC_UPDATED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_UPDATED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Updated");

            return response;
        }

        internal async Task<CMDefaultResponse> CloseJC(CMJCClose request, int userID)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
             *                 
            /*Your code goes here*/
            //Pending   add JC_End_date

            string query_bmtime_query = $"select id, breakdownTime from jobs where id in (select jobId from jobcards where id = {request.id} ); ";
            DataTable dt = await Context.FetchData(query_bmtime_query).ConfigureAwait(false);
            DateTime breakdownTime = DateTime.Now;
            int jobs_id = 0;
            if (dt.Rows.Count > 0)
            {
                jobs_id = Convert.ToInt32(dt.Rows[0][0]);
                breakdownTime = Convert.ToDateTime(dt.Rows[0][1]);
            }
            DateTime Utc = DateTime.UtcNow;

            int hour_diff = breakdownTime.Hour - Utc.Hour;

            // keeping on_time_status_flag as default value as 0 = Backlog
            int on_time_status_flag = (int)CMMS.JCDashboardStatus.Backlog;

            if (hour_diff < 8 && hour_diff > -0)
            {
                on_time_status_flag = (int)CMMS.JCDashboardStatus.OnTime;
            }
            else
            {
                on_time_status_flag = (int)CMMS.JCDashboardStatus.Delay;
            }

            string query_update_onstatus_job = $"update jobs set on_time_status={on_time_status_flag} where id ={jobs_id};";
            await Context.ExecuteNonQry<int>(query_update_onstatus_job).ConfigureAwait(false);

            // Code for close jc start here

            string queryCloseJc = $"update jobcards set JC_End_By_id={userID}, JC_Date_Stop ='{UtilsRepository.GetUTCTime()}',JC_Done_Description = '{request.comment}', JC_Approved = {(int)CMMS.ApprovalStatus.WAITING_FOR_APPROVAL}, JC_Status={(int)CMMS.CMMS_Status.JC_CLOSED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' where id ={request.id};";
            await Context.ExecuteNonQry<int>(queryCloseJc).ConfigureAwait(false);

            string queryIsolated = $"update permitisolatedassetcategories set normalisedStatus={request.normalisedStatus}, normalisedDate ='{UtilsRepository.GetUTCTime()}' where id ={request.isolationId};";
            await Context.ExecuteNonQry<int>(queryIsolated).ConfigureAwait(false);

            string queryLoto = $"update permitlotoassets set lotoRemovedStatus={request.lotoStatus}, lotoRemovedDate ='{UtilsRepository.GetUTCTime()}' where id ={request.lotoId};";
            await Context.ExecuteNonQry<int>(queryLoto).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");

            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    int facility_id = 380;
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            if (_jcDetails.Count == 0)
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, $"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;
        }

        internal async Task<List<CMDefaultResponse>> ApproveJC(CMJCApprove request, int userID)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return CMDefaultResponse                       	
            */
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string approveQuery = $"Update jobcards set JC_Approved = 1,JC_Status={(int)CMMS.CMMS_Status.JC_CLOSE_APPROVED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Approved_By_id={userID}, JC_Approve_Reason='{request.comment}', JC_Approved_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            // eng = user id = job created id , job appr

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, jc.JC_Status as status, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");

            if (_jcDetails.Count == 0)
            {
                responseList.Add(new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, $"Job Card with ID {request.id} not found"));
            }

            if (_jcDetails[0].status == (int)CMMS.CMMS_Status.JC_CLOSED)
            {
                string jobCloseQry = $"UPDATE jobs SET status = {(int)CMMS.CMMS_Status.JOB_CLOSED} WHERE id = {_jcDetails[0].jobid};";
                await Context.ExecuteNonQry<int>(jobCloseQry).ConfigureAwait(false);
                List<CMJobView> job = new List<CMJobView>();
                using (var repos = new JobRepository(_conn))
                {
                    job = await repos.GetJobView(_jcDetails[0].jobid, "");
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, _jcDetails[0].jobid, 0, 0, "Job Closed", CMMS.CMMS_Status.JOB_CLOSED, userID);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CLOSED, new[] { userID }, job[0]);
                responseList.Add(new CMDefaultResponse(_jcDetails[0].jobid, CMMS.RETRUNSTATUS.SUCCESS, "Job closed successfully"));
            }


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSE_APPROVED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSE_APPROVED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            responseList.Add(response);
            return responseList;
        }

        internal async Task<CMDefaultResponse> RejectJC(CMJCReject request, int userID)
        {
            //CREATE DIFF MODEL FOR REJECT AND APPROVE
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return boolean true/false
             */

            string approveQuery = $"Update jobcards set JC_Approved = 2,JC_Status={(int)CMMS.CMMS_Status.JC_CLOSE_REJECTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Rejected_By_id={userID}, JC_Rejected_Reason='{request.comment}', JC_Rejected_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");

            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSE_REJECTED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSE_REJECTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;

        }
        //carry forward jobcard?
        internal async Task<CMDefaultResponse> CarryForwardJC(CMApproval request, int userID)
        {
            string myQuery = $"UPDATE jobcards SET JC_End_By_id = {userID}, JC_Date_Stop ='{UtilsRepository.GetUTCTime()}', JC_Done_Description = '{request.comment}', JC_Approved = {(int)CMMS.ApprovalStatus.WAITING_FOR_APPROVAL}, JC_Status = {(int)CMMS.CMMS_Status.JC_CARRY_FORWARDED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");
            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, "Job Card Carry forward Requested", CMMS.CMMS_Status.JC_CARRY_FORWARDED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CARRY_FORWARDED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Carry forward Requested");

            return response;
        }

        internal async Task<CMDefaultResponse> ApproveJCCF(CMJCApprove request, int userID)
        {
            CMDefaultResponse responseList = new CMDefaultResponse();
            string approveQuery = $"Update jobcards set JC_Status = {(int)CMMS.CMMS_Status.JC_CF_APPROVED},JC_Approved = 1, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Approved_By_id={userID}, JC_Approve_Reason='{request.comment}', JC_Approved_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");

            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CF_APPROVED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CF_APPROVED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> RejectJCCF(CMJCReject request, int userID)
        {

            string approveQuery = $"Update jobcards set JC_Status = {(int)CMMS.CMMS_Status.JC_CF_REJECTED}, JC_Approved = 2, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Rejected_By_id={userID}, JC_Rejected_Reason='{request.comment}', JC_Rejected_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);


            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id, "");

            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CF_REJECTED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CF_REJECTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;

        }
    }
}
