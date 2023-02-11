using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;


namespace CMMSAPIs.Repositories.JC
{
    public class JCRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public JCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        internal async Task<List<CMJCList>> GetJCList(int facility_id)
        {
            /* Return all field mentioned in JCListModel model
            *  tables are JobCards, Jobs, Permit, Users
            */
           
            /*Your code goes here*/
            string myQuery1 = $"select jc.id as jobCardId, jc.JC_Date_Start as job_card_date, jc.JC_Date_Stop as end_time, job.id as jobid, job.title as description, CONCAT(user.firstName, user.lastName) as job_assinged_to, ptw.id as permit_id, ptw.Permitnumber as permit_no,JC_Status as current_status  from jobcards as jc JOIN jobs as job ON JC.jobid = job.id JOIN permits as ptw ON JC.PTW_id = PTW.ID LEFT JOIN users as user ON user.id = job.assignedId WHERE job.facilityId = { facility_id }";
            List<CMJCList> _ViewJobCardList = await Context.GetData<CMJCList>(myQuery1).ConfigureAwait(false);

            //job equipment category
            string myQuery2 = $"SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id = {_ViewJobCardList[0].jobid } and job.facilityId = { facility_id }";
            List<equipmentCatList> _equipmentCatList = await Context.GetData<equipmentCatList>(myQuery1).ConfigureAwait(false);

            _ViewJobCardList[0].LstequipmentCatList = _equipmentCatList;
             return _ViewJobCardList;
        }

        internal async Task<List<CMJCDetail>> GetJCDetail(int jc_id)
        {
            /*
             * Fetch data from JobCards table and joins these table for relationship using ids 
             * Users, Assets, AssetCategory, Facility, PermiEmployeeLists, PermitLotoAssets, PermitIsolatedAssetCategories
             * Return all the field listed in JCDetailModel 
            */

            //plant details 
            string myQuery1 = $"SELECT jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = { jc_id }";

            List<CMJCDetail> _plantDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);

            //job details
            string myQuery2 = $"SELECT job.id as job_id , job.title as job_title , CONCAT(user.firstName, user.lastName) as job_assigned_employee_name , job.description as job_description , workType.workTypeName as work_type FROM jobs as job JOIN jobmappingassets as mapAssets ON mapAssets.jobId = job.id JOIN assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id LEFT JOIN jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id JOIN facilities as facilities ON job.facilityId = facilities.id LEFT JOIN users as user ON user.id = job.assignedId JOIN jobcards as jc on jc.jobId = job.id where jc.id = {jc_id}";
            List<CMJCJobDetail> _jobDetails = await Context.GetData<CMJCJobDetail>(myQuery2).ConfigureAwait(false);

            //permit details
            string myQuery3 = $"SELECT ptw.id as permit_id, ptw.permitNumber as site_permit_no, permitType.title as permit_type, ptw.description as permit_description, CONCAT(user.firstName, user.lastName) as job_created_by_name, CONCAT(user1.firstName + ' ' + user1.lastName) as permit_issued_by_name, CONCAT(user2.firstName + ' ' + user2.lastName) as permit_approved_by_name FROM permits as ptw LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId JOIN jobs as job ON ptw.id = job.linkedPermit LEFT JOIN users as user ON user.id = job.assignedId LEFT JOIN users as user1 ON user1.id = ptw.issuedById LEFT JOIN users as user2 ON user2.id = ptw.approvedById JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id}";
             List<CMJCPermitDetail> _permitDetails = await Context.GetData<CMJCPermitDetail>(myQuery3).ConfigureAwait(false);

            // isolated details
            string myQuery4 = $"SELECT asset_cat.name as isolated_assestName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id JOIN jobcards as jc on ptwISOCat.permitId = jc.PTW_id  where jc.id = { jc_id }";
            List<CMJCIsolatedDetail> _isolatedDetails = await Context.GetData<CMJCIsolatedDetail>(myQuery4).ConfigureAwait(false);
           
            //loto list
            string myQuery5 = $"SELECT  assets_cat.name as isolated_assest_loto FROM assetcategories as assets_cat LEFT JOIN permits as ptw on ptw.id = assets_cat.id LEFT JOIN permitlotoassets AS LOTOAssets on LOTOAssets.PTW_id = ptw.id JOIN jobcards as jc on jc.jobId = ptw.id where jc.id = { jc_id }";
            List<CMJCLotoDetail> _lotoList = await Context.GetData<CMJCLotoDetail>(myQuery5).ConfigureAwait(false);

            // emp list
            string myQuery6 = $" SELECT CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList JOIN permits as ptw ON ptw.id = ptwEmpList.pwtId LEFT JOIN users as user ON user.id = ptwEmpList.employeeId JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
            List<CMJCEmpDetail> _empList = await Context.GetData<CMJCEmpDetail>(myQuery6).ConfigureAwait(false);

            // file upload 
            string myQuery7 = $"SELECT jc.id as id, PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory, PTWFiles.File_Size as fileSize, PTWFiles.status as status FROM fleximc_ptw_files AS PTWFiles LEFT JOIN permits as ptw on  ptw.id = PTWFiles.PTW_id JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
            List<CMFileDetail> _fileUpload = await Context.GetData<CMFileDetail>(myQuery7).ConfigureAwait(false);

            _plantDetails[0].LstCMJCJobDetailList = _jobDetails;
            _plantDetails[0].LstPermitDetailList = _permitDetails;
            _plantDetails[0].LstCMJCIsolatedDetailList = _isolatedDetails;
            _plantDetails[0].LstCMJCLotoDetailList = _lotoList;
            _plantDetails[0].LstCMJCEmpList = _empList;
            _plantDetails[0].file_list = _fileUpload;

            return _plantDetails;
        }

        internal async Task<CMDefaultResponse> CreateJC(int job_id)
        {
            /* 
             * Data will be inserted in Following tables
             * JobCards - Primary table (All basic details in inserted)
             * PermiEmployeeLists - All jc linked employee
             * JCFiles - All uploaded file data
             * Please check the JobCard above tables to get idea what values need to insert. 
             * Get details from job, permit and add it to Jobcards table
             * Return the All Properties in DefaultResponse model
            */

            //jc basic details
            string myQuery = $"SELECT job.id as job_id, ptw.id as ptw_id FROM permits AS ptw JOIN jobs as job on job.linkedPermit = ptw.id where job.id = { job_id }";
            List<CMJCCreate> _JobList = await Context.GetData<CMJCCreate>(myQuery).ConfigureAwait(false);
            int jc_id = 0;

            //_JobList.Count(); pending: add check for only one record is returned
            foreach (var data in _JobList)
            {
                string qryJCBasic = "insert into jobcards" +
                                     "(" +
                                             "jobId, PTW_id, JC_Date_Start, JC_Date_Stop, JC_Added_Date" +
                                      ")" +
                                      "values" +
                                     "(" +
                                            $"{ data.job_id }, { data.ptw_id }, '{ UtilsRepository.GetUTCTime() }', '{ UtilsRepository.GetUTCTime() }', '{ UtilsRepository.GetUTCTime() }' " +
                                     ")";

                jc_id = await Context.ExecuteNonQry<int>(qryJCBasic).ConfigureAwait(false);
            }
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (jc_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id order by jc.id desc limit 1";

            List<CMJCDetail> _jcDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, 0, 0, "Job Card Opened", CMMS.CMMS_Status.JC_OPENED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_OPENED, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job Card {_jcDetails[0].id} Start");

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateJC(CMJCUpdate request)
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
                    string qryisolatedCategory = $"update permitisolatedassetcategories set normalisedStatus = { data.normalisedStatus } , normalisedDate = '{ UtilsRepository.GetUTCTime() }' where id = {data.isolation_id} ";                                                
                    await Context.ExecuteNonQry<int>(qryisolatedCategory).ConfigureAwait(false);
                }
            }            

            //LOTOT LIST
            foreach (var data in request.loto_list)
            {
                string qryPermitlotoAssets = $"update permitlotoassets set lotoRemovedStatus = { data.lotoRemovedStatus } , lotoRemovedDate = '{ UtilsRepository.GetUTCTime() }' where id = {data.loto_id}";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            //EMP LIST
            foreach (var data in request.employee_list)
            {
                string qryPermitEmpList = $"update permitemployeelists set employeeId ={data.employeeId } , responsibility ='{ data.responsibility }' where id ={data.empId}";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

            // JCFILES PENDINGidand history 

            string myQuery1 = $"SELECT id as currentEmpID, CONCAT(firstName + ' ' + lastName) as UpdatedByName from users where id = { UtilsRepository.GetUserID() }";

            List<CMJCDetail> _jcDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);

            //who updated it means = current emp id and name
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, _jcDetails[0].currentEmpID, 0, 0, "Job Card Updated", CMMS.CMMS_Status.JC_UPDADATED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_UPDADATED, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job Card { _jcDetails[0].id } Updated");

            return response;
        }

        internal async Task<CMDefaultResponse> CloseJC(CMJCClose request)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
             *                 
            /*Your code goes here*/
     
            string queryCloseJc = $"update jobcards set JC_Update_by={request.employee_id}, JC_Update_date ='{UtilsRepository.GetUTCTime()}', JC_Status={(int)CMMS.CMMS_Status.JC_CLOSED} where id ={request.id};";
            await Context.ExecuteNonQry<int>(queryCloseJc).ConfigureAwait(false);

            string queryIsolated = $"update permitisolatedassetcategories set normalisedStatus={request.normalisedStatus}, normalisedDate ='{UtilsRepository.GetUTCTime()}' where id ={request.isolationId};";
            await Context.ExecuteNonQry<int>(queryIsolated).ConfigureAwait(false);

            string queryLoto = $"update permitlotoassets set lotoRemovedStatus={request.lotoStatus}, lotoRemovedDate ='{UtilsRepository.GetUTCTime()}' where id ={request.lotoId};";
            await Context.ExecuteNonQry<int>(queryLoto).ConfigureAwait(false);

            string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSED, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job Card {_jcDetails[0].id } Closed");

            return response;
        }

        internal async Task<CMDefaultResponse> ApproveJC(CMJCApprove request)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return CMDefaultResponse                       	
            */
            
            string approveQuery = $"Update jobcards set JC_Status = {(int)CMMS.CMMS_Status.JC_APPROVED} where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            // eng = user id = job created id , job appr

            string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_APPROVED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_APPROVED, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job Card {_jcDetails[0].id } Approve");

            return response;
        }

        internal async Task<CMDefaultResponse> RejectJC(CMJCReject request)
        {           
            //CREATE DIFF MODEL FOR REJECT AND APPROVE
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return boolean true/false
             */

            string approveQuery = $"Update jobcards set JC_Rejected_By_id={request.employee_id}, JC_Rejected_Reason='{request.commnet}', JC_Status = {(int)CMMS.CMMS_Status.JC_REJECTED5 },JC_Rejected_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.commnet, CMMS.CMMS_Status.JC_REJECTED5);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_REJECTED5, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, $"Job Card {_jcDetails[0].id} Rejected");

            return response;

        }
        //carry forward jobcard?
    }
}
