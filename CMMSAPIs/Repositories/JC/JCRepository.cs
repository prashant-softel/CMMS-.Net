using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Repositories.JC
{
    public class JCRepository : GenericRepository
    {
        public JCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }
        internal async Task<List<CMJCList>> GetJCList(int facility_id)
        {
            /* Return all field mentioned in JCListModel model
            *  tables are JobCards, Jobs, Permit, Users
            */
            //fid=679
            /*Your code goes here*/
            string myQuery1 = $"select jc.id as jobCardId, jc.JC_Date_Start as job_card_date, jc.JC_Date_Stop as end_time, job.id as jobid, job.title as description, CONCAT(user.firstName, user.lastName) as job_assinged_to, ptw.id as permit_id, ptw.Permitnumber as permit_no,JC_Status as current_status  from jobcards as jc JOIN jobs as job ON JC.jobid = job.id JOIN permits as ptw ON JC.PTW_id = PTW.ID LEFT JOIN users as user ON user.id = job.assignedId WHERE job.facilityId = { facility_id }";
            List<CMJCList> _ViewJobCardList = await Context.GetData<CMJCList>(myQuery1).ConfigureAwait(false);

            int job_id = 2533;

            //job equipment category
            string myQuery2 = $"SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id = {job_id} and job.facilityId = { facility_id }";
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

            //plant details = jc.id = 155
            string myQuery1 = $"SELECT facilities.name as plant_name, assets_cat.name as asset_category_name FROM assetcategories as assets_cat LEFT JOIN jobs as job on job.id = assets_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id  where jc.id = { jc_id }";
            List<CMJCDetail> _plantDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);

            //job details jc.id= 2533
            string myQuery2 = $"SELECT job.id as job_id , job.title as job_title , CONCAT(user.firstName, user.lastName) as job_assigned_employee_name , job.description as job_description , workType.workTypeName as work_type FROM jobs as job JOIN jobmappingassets as mapAssets ON mapAssets.jobId = job.id JOIN assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id LEFT JOIN jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id JOIN facilities as facilities ON job.facilityId = facilities.id LEFT JOIN users as user ON user.id = job.assignedId JOIN jobcards as jc on jc.jobId = job.id where jc.id = {jc_id}";
            List<CMJCJobDetail> _jobDetails = await Context.GetData<CMJCJobDetail>(myQuery2).ConfigureAwait(false);

            //permit details jc.id = 2339
            string myQuery3 = $"SELECT ptw.id as permit_id, ptw.permitNumber as site_permit_no, permitType.title as permit_type, ptw.description as permit_description, CONCAT(user.firstName, user.lastName) as job_created_by_name, CONCAT(user1.firstName + ' ' + user1.lastName) as permit_issued_by_name, CONCAT(user2.firstName + ' ' + user2.lastName) as permit_approved_by_name FROM permits as ptw LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId JOIN jobs as job ON ptw.id = job.linkedPermit LEFT JOIN users as user ON user.id = job.assignedId LEFT JOIN users as user1 ON user1.id = ptw.issuedById LEFT JOIN users as user2 ON user2.id = ptw.approvedById JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id}";
             List<CMJCPermitDetail> _permitDetails = await Context.GetData<CMJCPermitDetail>(myQuery3).ConfigureAwait(false);

            // isolated details = 2489
            string myQuery4 = $"SELECT asset_cat.name as isolated_assestName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id JOIN jobcards as jc on ptwISOCat.permitId = jc.PTW_id  where jc.id = { jc_id }";
            List<CMJCIsolatedDetail> _isolatedDetails = await Context.GetData<CMJCIsolatedDetail>(myQuery4).ConfigureAwait(false);
           
            //loto list jc.id = 152
            string myQuery5 = $"SELECT  assets_cat.name as isolated_assest_loto FROM assetcategories as assets_cat LEFT JOIN permits as ptw on ptw.id = assets_cat.id LEFT JOIN permitlotoassets AS LOTOAssets on LOTOAssets.PTW_id = ptw.id JOIN jobcards as jc on jc.jobId = ptw.id where jc.id = { jc_id }";
            List<CMJCLotoDetail> _lotoList = await Context.GetData<CMJCLotoDetail>(myQuery5).ConfigureAwait(false);

            // emp list jc.id 2489
            string myQuery6 = $"CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList JOIN permits as ptw ON ptw.id = ptwEmpList.pwtId LEFT JOIN users as user ON user.id = ptwEmpList.employeeId JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
            List<CMJCEmpDetail> _empList = await Context.GetData<CMJCEmpDetail>(myQuery6).ConfigureAwait(false);

            // file upload jc.id= 2464
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

        internal async Task<int> CreateJC(CMJCCreate request)
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
            //jc basic details
            string myQuery = $"SELECT job.id as jobid, ptw.id as ptw_id, ptw.acceptedById as JC_Start_By_id, ptw.acceptedById as JC_End_By_id, ptw.issuedById as JC_Issued_By_id, ptw.issuedById as JC_Approved_By_id  FROM permits AS ptw JOIN jobs as job on job.linkedPermit = ptw.id where job.id = { request.job_id }";
            List<CMJCCreate> _JobList = await Context.GetData<CMJCCreate>(myQuery).ConfigureAwait(false);
            int jc_id = 2534;
            foreach (var data in _JobList)
            {
                string qryJCBasic = "insert into jobcards" +
                                     "(" +
                                             "jobId, PTW_id, JC_Date_Start, JC_Date_Stop, JC_Start_By_id, JC_End_By_id, JC_Issued_By_id, JC_Approved_By_id, JC_Added_by, JC_Added_Date" +
                                      ")" +
                                      "values" +
                                     "(" +
                                              $"{ data.jobid }, { data.ptw_id }, '{ UtilsRepository.GetUTCTime() }', '{ UtilsRepository.GetUTCTime() }',{ data.JC_Start_By_id }, { data.JC_End_By_id }, { data.JC_Issued_By_id }, { data.JC_Approved_By_id },{ data.JC_Approved_By_id },  '{ UtilsRepository.GetUTCTime() }' " +
                                     ")";

                jc_id = await Context.ExecuteNonQry<int>(qryJCBasic).ConfigureAwait(false);
            }

            //add emp list
            foreach (var data in request.LstEmpDetails)
            {
                string qryPermitEmpList = "insert into permitemployeelists " +
                                          "(" +
                                                " pwtId , JC_id, employeeId , responsibility " +
                                          ")" +
                                          " value " +
                                          "(" +
                                                   $"{ _JobList[0].ptw_id }, { jc_id }, { data.employeeId }, '{ data.responsibility }'" +
                                           ")";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

            //jc files pending 
            return jc_id;
        }

        internal async Task<List<CMDefaultResponse>> UpdateJC(CMJCUpdate request)
        {
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
                    string qryisolatedCategory = $"update permitisolatedassetcategories set normalisedStatus = { data.normalisedStatus }  normalisedDate = { data.normalisedDate} ";                                                
                    await Context.ExecuteNonQry<int>(qryisolatedCategory).ConfigureAwait(false);
                }
            }

            //LOTOT LIST
            foreach (var data in request.loto_list)
            {
                string qryPermitlotoAssets = $"update permitlotoassets set lotoRemovedStatus = { data.lotoRemovedStatus } lotoRemovedDate = {data.lotoRemovedDate }";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            //EMP LIST
            foreach (var data in request.employee_list)
            {
                string qryPermitEmpList = $"update permitemployeelists set employeeId ={data.employeeId } , responsibility ={ data.responsibility })";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

           

            // JCFILES PENDING
                        
            /*Your code goes here*/
            return null;
        }

        internal Task<List<CMDefaultResponse>> CloseJC(CMJCClose request)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<CMDefaultResponse>> ApproveJC(CMApproval request)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return CMDefaultResponse
            */

            /*Your code goes here*/
            return null;
        }

        internal Task<List<CMDefaultResponse>> RejectJC(CMApproval request)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return boolean true/false
            */

            /*Your code goes here*/
            return null;
        }
        //carry forward jobcard?
    }
}
