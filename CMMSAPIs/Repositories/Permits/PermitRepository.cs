using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;

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

        internal async Task<List<CMDefaultList>> GetPermitTypeList(int facility_id)
        {
            /*
             * return permit_type_id, name from PermitTypeLists table for requsted facility_id 
            */
            string myQuery = $"SELECT id, title as name FROM permittypelists WHERE facilityId = { facility_id }";
            List<CMDefaultList> _PermitTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _PermitTypeList;
        }

        internal async Task<List<CMDefaultList>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            /*
             * return id, title from PermitTypeSafetyMeasures table for requested permit_type_id
             * input 1 - checkbox, 2 - radio, 3 - text, 4 - Ok
            */

            string myQuery5 = "SELECT permitsaftymea.id as saftyQuestionId, permitsaftymea.title as SaftyQuestionName, permitsaftymea.input as input FROM                      permitsafetyquestions  as  permitsaftyques " +
                             "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                             "JOIN permits as ptw ON ptw.id = permitsaftymea.permitTypeId " +
                             $"where ptw.id =  { permit_type_id }";
            List<CMDefaultList> _QuestionList = await Context.GetData<CMDefaultList>(myQuery5).ConfigureAwait(false);
            return _QuestionList;
        }

        internal async Task<List<CMDefaultList>> GetJobTypeList(int facility_id)
        {
            /*
             * return id, title from PermitJobTypeList table for requested facility_id
            */
            string myQuery = $"SELECT id as id, title as name FROM permitjobtypelist WHERE facilityId =  { facility_id } ";
            List<CMDefaultList> _JobTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        internal async Task<List<CMDefaultList>> GetSOPList(int job_type_id)
        {
            /*
             * return * from PermitTBTJobList table for requested job_type_id
            */
            string myQuery = $"SELECT id as id, title as name FROM permittbtjoblist WHERE jobTypeId =  { job_type_id } ";
            List<CMDefaultList> _JobTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        /*
         * Permit Main Feature End Points
        */

        internal async Task<List<CMPermitList>> GetPermitList(int facility_id, int userID)
        {
            /*
             * Return id as well as string value
             * Use Permits, Assets, AssetsCategory, Users table to fetch below fields
             * Permit id, site Permit No., Permit Type, Equipment Categories, Working Area/Equipment, Description, Permit requested by
             * Request Date/Time, Approved By, Approved Date/Time, Current Status(Approved, Rejected, closed).           
            */
            string myQuery = "SELECT " +
                                 "ptw.id as permitId, ptw.permitNumber as permit_site_no, permitType.id as permit_type,  permitType.title as PermitTypeName, asset_cat.id as equipment_category, asset_cat.name as equipment, facilities.id as workingAreaId, facilities.name as workingAreaName, ptw.description as description, CONCAT(user.firstName + ' ' + user.lastName) as request_by_name, ptw.acceptedDate as request_datetime, CONCAT(user.firstName + ' ' + user.lastName) as approved_by_name, ptw.approvedDate as approved_datetime, ptw.status as currentStatus " +
                                 " FROM " +
                                        "permits as ptw " +
                                  "JOIN " +
                                        "facilities as facilities ON ptw.blockId = facilities.id " +
                                  "JOIN " +
                                        "assetcategories as asset_cat ON ptw.LOTOId = asset_cat.id " +
                                  "LEFT JOIN " +
                                         "permittypelists as permitType ON ptw.typeId = permitType.id " +
                                  "LEFT JOIN " +
                                         "jobs as job ON ptw.id = job.linkedPermit " +
                                  "LEFT JOIN " +
                                        "users as user ON user.id = ptw.issuedById or user.id = ptw.approvedById" +
                                  $" WHERE ptw.facilityId = { facility_id } and user.id = { userID } ";
            List<CMPermitList> _PermitList = await Context.GetData<CMPermitList>(myQuery).ConfigureAwait(false);
            return _PermitList;
        }

        internal async Task<int> CreatePermit(CMCreatePermit request)
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
            string qryPermitBasic = "insert into permits(facilityId, blockId, startDate, endDate, description, jobId, LOTOId, typeId, TBTId, issuedById, approvedById, acceptedById) values" +
             $"({ request.facility_id }, { request.blockId }, '{ UtilsRepository.GetUTCTime() }', '{ UtilsRepository.GetUTCTime() }', '{ request.description }', { request.work_type_id }, { request.lotoId }, '{ request.typeId }', { request.sop_type_id }, { request.issuer_id }, { request.approver_id }, { request.user_id })";
            int permitPrimaryKey = 59613;
            await Context.ExecuteNonQry<int>(qryPermitBasic).ConfigureAwait(false);

            foreach (var data in request.block_ids)
            {
                string qryPermitBlock = $"insert into permitblocks(ptw_id, block_id ) value ({ permitPrimaryKey }, { data })";
                await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);
            }

            foreach (int data in request.category_ids)
            {
                string qryPermitCategory = $"insert into permitassetlists (ptwId, assetId ) value ({ permitPrimaryKey }, { data })";
                await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
            }

            if (request.is_isolation_required == true)
            {
                foreach (int data in request.isolated_category_ids)
                {
                    string qryPermitisolatedCategory = $"insert into permitisolatedassetcategories (permitId , assetCategoryId ) value ({ permitPrimaryKey },{ data })";
                    await Context.ExecuteNonQry<int>(qryPermitisolatedCategory).ConfigureAwait(false);
                }
            }

            foreach (var data in request.Loto_list)
            {
                string qryPermitlotoAssets = $"insert into permitlotoassets ( PTW_id, Loto_Asset_id, Loto_Key ) value ({ permitPrimaryKey },{ data.Loto_id }, '{ data.Loto_Key }')";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            foreach (var data in request.employee_list)
            {
                string qryPermitEmpList = $"insert into permitemployeelists ( pwtId , employeeId , responsibility ) value ({ permitPrimaryKey },{ data.employeeId }, '{ data.responsibility }')";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

            foreach (var data in request.safety_question_list)
            {
                string qryPermitSaftyQuestion = $"insert into permitsafetyquestions ( permitId , safetyMeasureId, safetyMeasureValue) value ({ permitPrimaryKey }, { data.safetyMeasureId }, '{ data.safetyMeasureValue }')";
                await Context.ExecuteNonQry<int>(qryPermitSaftyQuestion).ConfigureAwait(false);
            }
            return permitPrimaryKey;
            //file_upload_form pending 
        }

        internal async Task<List<CMPermitDetail>> GetPermitDetails(int permit_id)
        {
            /*
             * Return id and string values which are stored in 
             * Permits, PermitBlocks, PermitIsolatedAssetCategories, PermitLOTOAssets, PermitEmployeeLists, PermitSafetyQuestions table
             * for request permit_id Join with below tables to get string value from
             * Assets, AssetsCategory, Facility, Users, PermitTypeSafetyMeasures, PermitTypeList, PermitJobTypeList, PermitTBTJobList
            */

            string myQuery = "SELECT ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "JOIN facilities as facilities  ON ptw.blockId = facilities.id and facilities.isBlock = 1 " +
               "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
               "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
               "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
               "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
                $"where ptw.id = { permit_id }";
            List<CMPermitDetail> _PermitDetailsList = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            //get employee list
            string myQuery1 = "SELECT  CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList " +
                               "JOIN permits as ptw  ON ptw.id = ptwEmpList.pwtId " +
                              $"LEFT JOIN users as user ON user.id = ptwEmpList.employeeId where ptw.id = { permit_id }";
            List<CMEMPLIST> _EmpList = await Context.GetData<CMEMPLIST>(myQuery1).ConfigureAwait(false);

            //get isolation list
            string myQuery2 = "SELECT asset_cat.id as IsolationAssetsCatID, asset_cat.name as IsolationAssetsCatName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id where ptwISOCat.permitId = " + permit_id;
            List<CMIsolationList> _IsolationList = await Context.GetData<CMIsolationList>(myQuery2).ConfigureAwait(false);

            //get loto
            string myQuery3 = "SELECT assets_cat.id as asset_id, assets_cat.name as asset_name, ptw.lockSrNo as locksrno FROM assetcategories as assets_cat " +
                               "LEFT JOIN permits as ptw on ptw.id = assets_cat.id " +
                               "LEFT JOIN permitlotoassets AS LOTOAssets on LOTOAssets.PTW_id = ptw.id " +
                               $"where ptw.id =  { permit_id }";
            List<CMLoto> _LotoList = await Context.GetData<CMLoto>(myQuery3).ConfigureAwait(false);

            //get upload file
            string myQuery4 = "SELECT PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory,PTWFiles.File_Size as fileSize,                              PTWFiles.status as status FROM fleximc_ptw_files AS PTWFiles " +
                               $"LEFT JOIN permits ptw on  ptw.id = PTWFiles.PTW_id where ptw.id = { permit_id }";
            List<CMFileDetail> _UploadFileList = await Context.GetData<CMFileDetail>(myQuery4).ConfigureAwait(false);

            //get safty question
            string myQuery5 = "SELECT permitsaftymea.id as saftyQuestionId, permitsaftymea.title as SaftyQuestionName, permitsaftymea.input as input FROM                       permitsafetyquestions  as permitsaftyques " +
                               "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                               "JOIN permits as ptw ON ptw.id = permitsaftymea.permitTypeId " +
                               $"where ptw.id = { permit_id }";

            //get Associated Job
            string myQuery6 = "SELECT job.id as JobId, jobCard.id as JobCardId, job.title as JobTitle , job.description as JobDes, job.createdAt as JobDate, job.status as JobStatus FROM jobs as job JOIN permits as ptw ON job.linkedPermit = " + permit_id +
            " LEFT JOIN fleximc_jc_files as jobCard ON jobCard.JC_id = job.id " +
              $"where ptw.id = { permit_id }";
            List<CMAssociatedList> _AssociatedJobList = await Context.GetData<CMAssociatedList>(myQuery6).ConfigureAwait(false);

            //get category list
            string myQuery7 = "SELECT assets_cat.name as equipmentCat FROM assetcategories as assets_cat " +
                               "LEFT JOIN permits as ptw on ptw.id = assets_cat.id " +
                               $"where ptw.id = { permit_id }";
            List<CMCategory> _CategoryList = await Context.GetData<CMCategory>(myQuery7).ConfigureAwait(false);


            List<CMSaftyQuestion> _QuestionList = await Context.GetData<CMSaftyQuestion>(myQuery5).ConfigureAwait(false);
            List<CMSaftyQuestion> LstKeyValue = new List<CMSaftyQuestion>();

            _PermitDetailsList[0].LstLoto = _LotoList;
            _PermitDetailsList[0].LstEmp = _EmpList;
            _PermitDetailsList[0].LstIsolation = _IsolationList;
            _PermitDetailsList[0].file_list = _UploadFileList;
            _PermitDetailsList[0].safety_question_list = _QuestionList;
            _PermitDetailsList[0].LstAssociatedJob = _AssociatedJobList;
            _PermitDetailsList[0].LstCategory = _CategoryList;

            return _PermitDetailsList;
        }


        /*         * Permit Issue/Approval/Rejection/Cancel End Points
        */
        internal async Task<List<CMDefaultResp>> PermitExtend(CMApproval request)
        {
            string updateQry = $"update permits set extendReason = '{ request.commnet }', extendTime = { request.Time }, extendStatus = { request.status }                        where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitExtendApprove(CMApproval request)
        {
            string updateQry = $"update permits set extendStatus = '{ request.status }', extendApproveTime = '{ UtilsRepository.GetUTCTime() }' where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitExtendCancel(CMApproval request)
        {
            string updateQry = $"update permits set extendStatus = { request.status }, extendRejectReason = '{ request.commnet }' where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitIssue(CMApproval request)
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */
            string updateQry = $"update permits set issuedReccomendations = '{ request.commnet }', issuedStatus = { request.status }, issuedDate = '{ UtilsRepository.GetUTCTime() }', issuedById = { request.employee_id }  where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitApprove(CMApproval request)
        {
            /*Update Permit Table reccomendationsByApprover, approvedStatus, approvedDate
                       * Return Message Approved successfully*/

            string updateQry = $"update permits set reccomendationsByApprover = '{ request.commnet }', approvedStatus = { request.status }, approvedDate = '{ UtilsRepository.GetUTCTime() }', approvedById = { request.employee_id }  where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitClose(CMApproval request)
        {
            string updateQry = $"update permits set completedDate = '{ UtilsRepository.GetUTCTime() }', completedStatus = { request.status }, completedById = { request.employee_id }  where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitReject(CMApproval request)
        {
            /*
             * Pending
            */
            string updateQry = $"update permits set issuedReccomendations = '{ request.commnet }', issuedStatus = { request.status }, issuedDate ='{ UtilsRepository.GetUTCTime() }', issuedById = { request.employee_id }  where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);

            return _Employee;
        }

        internal async Task<List<CMDefaultResp>> PermitCancel(CMApproval request)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */
            string updateQry = $"update permits set cancelReccomendations = '{ request.commnet }', cancelRequestStatus = { request.status }, cancelRequestDate = '{ UtilsRepository.GetUTCTime() }', cancelRequestById = { request.employee_id }  where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);

            return _Employee;
        }

        internal async Task<int> UpdatePermit(CMUpdatePermit request)
        {
            string updatePermitQry = $"update permits set facilityId = { request.facility_id },blockId = { request.blockId }, startDate = '{ UtilsRepository.GetUTCTime() }', endDate = '{ UtilsRepository.GetUTCTime() }', description = '{ request.description }', jobId = { request.job_type_id }, LOTOId = { request.lotoId }, typeId = { request.typeId }, TBTId = { request.sop_type_id }, issuedById = { request.issuer_id }, approvedById = { request.approver_id }, acceptedById = { request.user_id } where id = { request.permit_id };";
            await Context.ExecuteNonQry<int>(updatePermitQry).ConfigureAwait(false);
            int updatePrimaryKey = request.permit_id;

            string DeleteQry = $"delete from permitblocks where ptw_id = { request.permit_id };";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

            foreach (var data in request.block_ids)
            {
                string qryPermitBlock = $"insert into permitblocks(ptw_id, block_id ) value ({ updatePrimaryKey }, { data })";
                await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);
            }
            string DeleteQry1 = $"delete from permitassetlists where ptwId = {request.permit_id};";
            await Context.ExecuteNonQry<int>(DeleteQry1).ConfigureAwait(false);

            foreach (var data in request.category_ids)
            {     
                string qryPermitCategory = $"insert into permitassetlists (ptwId, assetId ) value ({ updatePrimaryKey }, { data })";
                await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
            }

            string DeleteQry2 = $"delete from permitisolatedassetcategories where permitId = { request.permit_id };";
            await Context.ExecuteNonQry<int>(DeleteQry2).ConfigureAwait(false);
            if (request.is_isolation_required == true)
            {
                foreach (int data in request.isolated_category_ids)
                {
                    string qryPermitisolatedCategory = $"insert into permitisolatedassetcategories (permitId , assetCategoryId ) value ({ updatePrimaryKey },{ data })";
                    await Context.ExecuteNonQry<int>(qryPermitisolatedCategory).ConfigureAwait(false);
                }
            }

            string DeleteQry3 = $"delete from permitlotoassets where PTW_id = { request.permit_id };";
            await Context.ExecuteNonQry<int>(DeleteQry3).ConfigureAwait(false);
            foreach (var data in request.Loto_list)
            {
                string qryPermitlotoAssets = $"insert into permitlotoassets ( PTW_id , Loto_Asset_id, Loto_Key ) value ({ updatePrimaryKey }, { data.Loto_id }, '{ data.Loto_Key }')";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            string DeleteQry4 = $"delete from permitemployeelists where pwtId = { request.permit_id };";
            await Context.ExecuteNonQry<int>(DeleteQry4).ConfigureAwait(false);
            foreach (var data in request.employee_list)
            {
                string qryPermitEmpList = $"insert into permitemployeelists ( pwtId , employeeId , responsibility ) value ({ updatePrimaryKey },{ data.employeeId }, '{ data.responsibility}')";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

            string DeleteQry5 = $"delete from permitsafetyquestions where permitId = { request.permit_id };";
            await Context.ExecuteNonQry<int>(DeleteQry5).ConfigureAwait(false);
            foreach (var data in request.safety_question_list)
            {
                string qryPermitSaftyQuestion = $"insert into permitsafetyquestions ( permitId , safetyMeasureId, safetyMeasureValue) value ({ updatePrimaryKey }, { data.safetyMeasureId }, '{ data.safetyMeasureValue }')";
                await Context.ExecuteNonQry<int>(qryPermitSaftyQuestion).ConfigureAwait(false);
            }

            return updatePrimaryKey;

        }
    }
}