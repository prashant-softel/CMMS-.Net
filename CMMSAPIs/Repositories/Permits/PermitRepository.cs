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

        internal async Task<List<DefaultListModel>> GetPermitTypeList(int facility_id)
        {
            /*
             * return permit_type_id, name from PermitTypeLists table for requsted facility_id 
            */
            string myQuery = $"SELECT id, title as name FROM permittypelists WHERE facilityId = { facility_id }";
            List<DefaultListModel> _PermitTypeList = await Context.GetData<DefaultListModel>(myQuery).ConfigureAwait(false);
            return _PermitTypeList;
        }

        internal async Task<List<DefaultListModel>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            /*
             * return id, title from PermitTypeSafetyMeasures table for requested permit_type_id
             * input 1 - checkbox, 2 - radio, 3 - text, 4 - Ok
            */

            string myQuery5 = "SELECT permitsaftymea.id as saftyQuestionId, permitsaftymea.title as SaftyQuestionName, permitsaftymea.input as input FROM                      permitsafetyquestions  as  permitsaftyques " +
                             "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                             "JOIN permits as ptw ON ptw.id = permitsaftymea.permitTypeId " +
                             "where ptw.id = " + permit_type_id;
            List<DefaultListModel> _QuestionList = await Context.GetData<DefaultListModel>(myQuery5).ConfigureAwait(false);
            return null;
        }

        internal async Task<List<DefaultListModel>> GetJobTypeList(int facility_id)
        {
            /*
             * return id, title from PermitJobTypeList table for requested facility_id
            */
            string myQuery = " SELECT id, title as name FROM permitjobtypelist WHERE facilityId = " + facility_id;
            List<DefaultListModel> _JobTypeList = await Context.GetData<DefaultListModel>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        internal async Task<List<DefaultListModel>> GetSOPList(int job_type_id)
        {
            /*
             * return * from PermitTBTJobList table for requested job_type_id
            */
            string myQuery = " SELECT id, title as name FROM permittbtjoblist WHERE jobTypeId = " + job_type_id;
            List<DefaultListModel> _JobTypeList = await Context.GetData<DefaultListModel>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        /*
         * Permit Main Feature End Points
        */

        internal async Task<List<PermitListModel>> GetPermitList(int facility_id, int userID)
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
                                        "assetcategories as asset_cat ON ptw.id = asset_cat.id " +
                                  "LEFT JOIN " +
                                         "permittypelists as permitType ON ptw.typeId = permitType.id " +
                                  "LEFT JOIN " +
                                         "jobs as job ON ptw.id = job.linkedPermit " +
                                  "LEFT JOIN " +
                                        "users as user ON user.id = ptw.issuedById or user.id = ptw.approvedById" +
                                  " WHERE ptw.facilityId = " + facility_id + " and user.id = " + userID;
            List<PermitListModel> _PermitList = await Context.GetData<PermitListModel>(myQuery).ConfigureAwait(false);
            return _PermitList;
        }

        internal async Task<int> CreatePermit(CreatePermitModel request)
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
            string qryPermitBasic = "insert into permits(facilityId,startDate,endDate,description,jobId,TBTId,issuedById,approvedById,acceptedById) values" +
             "('" + request.facility_id + "','" + request.start_datetime + "','" + request.end_datetime + "','" + request.description + "','" + request.work_type_id + "','" + request.sop_type_id + "','" + request.issuer_id + "','" + request.approver_id + "','" + request.user_id + "')";
            int permitPrimaryKey = 59635;
            await Context.ExecuteNonQry<int>(qryPermitBasic).ConfigureAwait(false);

       /*     string PrimaryKey = "SELECT id FROM `permits` order by id desc limit 1";
            List<int key = await Context.ExecuteNonQry<List<int>>(PrimaryKey).ConfigureAwait(false);
            int permitPrimaryKey = key;*/

            foreach (var data in request.block_ids)
            {
                string qryPermitBlock = "insert into permitblocks(ptw_id, block_id ) value ('" + permitPrimaryKey + "','" + data + "')";
                await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);
            }

            foreach (int data in request.category_ids)
            {
                string qryPermitCategory = "insert into permitassetlists (ptwId	, categoryId ) value ('" + permitPrimaryKey + "','" + data + "')";
                await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
            }

            if (request.is_isolation_required == true)
            {
                foreach (int data in request.isolated_category_ids)
                {
                    string qryPermitisolatedCategory = "insert into permitisolatedassetcategories (permitId , assetCategoryId ) value ('" + permitPrimaryKey + "','" + data + "')";
                    await Context.ExecuteNonQry<int>(qryPermitisolatedCategory).ConfigureAwait(false);
                }
            }

            foreach (var data in request.Loto_list)
            {
                string qryPermitlotoAssets = "insert into permitlotoassets ( PTW_id , Loto_Asset_id, Loto_Key ) value ('" + permitPrimaryKey + "','" + data.Loto_id + "','" + data.Loto_Key + "')";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            foreach (var data in request.employee_list)
            {
                string qryPermitEmpList = "insert into permitemployeelists ( pwtId , employeeId , responsibility ) value ('" + permitPrimaryKey + "','" + data.employeeId + "','" + data.responsibility + "')";
                 await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

            foreach (var data in request.safety_question_list)
            {
                string qryPermitSaftyQuestion = "insert into permitsafetyquestions ( permitId , safetyMeasureId, safetyMeasureValue) value ('" + permitPrimaryKey + "','" + data.safetyMeasureId + "','" + data.safetyMeasureValue + "')";
                await Context.ExecuteNonQry<int>(qryPermitSaftyQuestion).ConfigureAwait(false);
            }
            return permitPrimaryKey;
            //file_upload_form pending 
        }

        internal async Task<List<PermitDetailModel>> GetPermitDetails(int permit_id)
        {
            /*
             * Return id and string values which are stored in 
             * Permits, PermitBlocks, PermitIsolatedAssetCategories, PermitLOTOAssets, PermitEmployeeLists, PermitSafetyQuestions table
             * for request permit_id Join with below tables to get string value from
             * Assets, AssetsCategory, Facility, Users, PermitTypeSafetyMeasures, PermitTypeList, PermitJobTypeList, PermitTBTJobList
            */

            string myQuery = "SELECT ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, asset_cat.name as equipmentCat, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "LEFT JOIN assetcategories as asset_cat ON ptw.id = asset_cat.id " +
              "JOIN facilities as facilities  ON ptw.blockId = facilities.id and facilities.isBlock = 1 " +
               "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
               "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
               "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
               "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
                "where ptw.id = " + permit_id;
            List<PermitDetailModel> _PermitDetailsList = await Context.GetData<PermitDetailModel>(myQuery).ConfigureAwait(false);

            //get employee list
            string myQuery1 = "SELECT  CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList " +
                               "JOIN permits as ptw  ON ptw.id = ptwEmpList.pwtId " +
                              "LEFT JOIN users as user ON user.id = ptwEmpList.employeeId where ptw.id = " + permit_id;
            List<CMEMPLIST> _EmpList = await Context.GetData<CMEMPLIST>(myQuery1).ConfigureAwait(false);

            //get isolation list
            string myQuery2 = "SELECT asset_cat.id as IsolationAssetsCatID, asset_cat.name as IsolationAssetsCatName FROM permitisolatedassetcategories AS ptwISOCat " +
                             "LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id where ptwISOCat.permitId = " + permit_id;
            List<CMIsolationList> _IsolationList = await Context.GetData<CMIsolationList>(myQuery2).ConfigureAwait(false);

            //get loto
            string myQuery3 = "SELECT assets_cat.id as asset_id, assets_cat.name as asset_name, ptw.lockSrNo as locksrno FROM assetcategories as assets_cat " +
                               "LEFT JOIN permits as ptw on ptw.id = assets_cat.id " +
                               "LEFT JOIN permitlotoassets AS LOTOAssets on LOTOAssets.PTW_id = ptw.id " +
                               "where ptw.id = " + permit_id;
            List<CMLoto> _LotoList = await Context.GetData<CMLoto>(myQuery3).ConfigureAwait(false);

            //get upload file
            string myQuery4 = "SELECT PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory,PTWFiles.File_Size as fileSize,                              PTWFiles.status as status FROM fleximc_ptw_files AS PTWFiles " +
                               "LEFT JOIN permits ptw on  ptw.id = PTWFiles.PTW_id where ptw.id = " + permit_id;
            List<FileDetailModel> _UploadFileList = await Context.GetData<FileDetailModel>(myQuery4).ConfigureAwait(false);

            //get safty question
            string myQuery5 = "SELECT permitsaftymea.id as saftyQuestionId, permitsaftymea.title as SaftyQuestionName, permitsaftymea.input as input FROM                       permitsafetyquestions  as permitsaftyques " +
                               "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                               "JOIN permits as ptw ON ptw.id = permitsaftymea.permitTypeId " +
                               "where ptw.id = " + permit_id;
            List<CMSaftyQuestion> _QuestionList = await Context.GetData<CMSaftyQuestion>(myQuery5).ConfigureAwait(false);
            List<PermitDetailModel> LstKeyValue = new List<PermitDetailModel>();

            _PermitDetailsList[0].LstLoto = _LotoList;
            _PermitDetailsList[0].LstEmp = _EmpList;
            _PermitDetailsList[0].LstIsolation = _IsolationList;
            _PermitDetailsList[0].file_list = _UploadFileList;
            _PermitDetailsList[0].safety_question_list = _QuestionList;

            return _PermitDetailsList;
        }


        /*         * Permit Issue/Approval/Rejection/Cancel End Points
        */
        internal async Task<List<DefaultResponseModel>> PermitApprove(ApprovalModel request)
        {

            /*Update Permit Table reccomendationsByApprover, approvedStatus, approvedDate
                       * Return Message Approved successfully*/

            string updateQry = "update permits set reccomendationsByApprover = '" + request.commnet + "', approvedStatus = '" + request.status + "', approvedDate = '" + request.approvedDate + "', approvedById = '" + request.employee_id + "'  where id = " + request.id + ";";
            List<DefaultResponseModel> _Employee = await Context.GetData<DefaultResponseModel>(updateQry).ConfigureAwait(false);

            return _Employee;
        }
       
        internal Task<List<DefaultResponseModel>> PermitReject(ApprovalModel request)
        {
            /*
             * Pending
            */
            return null;
        }

        internal async Task<List<DefaultResponseModel>> PermitIssue(ApprovalModel request)
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */
            string updateQry = "update permits set issuedReccomendations = '" + request.commnet + "', issuedStatus = '" + request.status + "', issuedDate = '" + request.approvedDate + "', issuedById = '" + request.employee_id + "'  where id = " + request.id + ";";
            List<DefaultResponseModel> _Employee = await Context.GetData<DefaultResponseModel>(updateQry).ConfigureAwait(false);

            return _Employee;
        }

        internal async Task<List<DefaultResponseModel>> PermitCancel(ApprovalModel request)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */
            string updateQry = "update permits set cancelReccomendations = '" + request.commnet + "', cancelRequestStatus = '" + request.status + "', cancelRequestDate = '" + request.approvedDate + "', cancelRequestById = '" + request.employee_id + "'  where id = " + request.id + ";";
            List<DefaultResponseModel> _Employee = await Context.GetData<DefaultResponseModel>(updateQry).ConfigureAwait(false);

            return _Employee;
        }
        internal async Task<int> UpdatePermit(UpdatePermitModel request)
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */

            string updatePermitQry = "update permits set facilityId = '"+request.facility_id+"' , startDate = '"+request.start_date+"', endDate = '"+request.end_date+"', description = '"+ request.description +"', jobId = '"+request.job_type_id+"', TBTId = '"+request.sop_type_id+"', issuedById = '"+request.issuer_id+"', approvedById = '"+request.approver_id+"', acceptedById = '"+request.user_id+"' where id = '"+request.permit_id +"';";
            await Context.ExecuteNonQry<int>(updatePermitQry).ConfigureAwait(false);
            int updatePrimaryKey = request.permit_id;
                foreach (var data in request.block_ids)
            {
                string updateFacilityQry = $"update permitblocks set block_id = {data} where ptw_id =  { request.permit_id } ;";
                await Context.ExecuteNonQry<int>(updateFacilityQry).ConfigureAwait(false);
            }

            foreach (var data in request.category_ids)
            {
                string updateFacilityQry = $"update permitassetlists set categoryId = { data } where ptwId = { request.permit_id } ;";
                await Context.ExecuteNonQry<int>(updateFacilityQry).ConfigureAwait(false);
            }

            if (request.is_isolation_required == true)
            {
                foreach (int data in request.isolated_category_ids)
                {
                    string updatePermitlotoAssets = $"update permitisolatedassetcategories set assetCategoryId = { data } where permitId ={ request.permit_id } ;";
                    await Context.ExecuteNonQry<int>(updatePermitlotoAssets).ConfigureAwait(false);
                }
            }

            foreach (var data in request.Loto_list)
            {
                string updatePermitlotoAssets = "update permitlotoassets set Loto_Asset_id = '"+data.id +"' , Loto_Key = '"+data.name+"' where PTW_id ='"+request.permit_id +"' ;";
                await Context.ExecuteNonQry<int>(updatePermitlotoAssets).ConfigureAwait(false);
            }

            foreach (var data in request.employee_list)
            {
                string updateyPermitEmpList = "update permitemployeelists set employeeId = '"+data.id+"', responsibility = '"+ data.name +"' where pwtId = '"+request.permit_id +"';";
                await Context.ExecuteNonQry<int>(updateyPermitEmpList).ConfigureAwait(false);
            }

            foreach (var data in request.safety_question_list)
            {
                string updateyPermitEmpList = "update permitsafetyquestions set safetyMeasureId = '"+data.id+"' ,                                    safetyMeasureValue = '"+ data.name +"' where permitId = '"+ request.permit_id +"' ;";
                await Context.ExecuteNonQry<int>(updateyPermitEmpList).ConfigureAwait(false);
            }
          
            return updatePrimaryKey;

        }

    }
}
