﻿using System;
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
using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Repositories.Permits
{
    public class PermitRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public PermitRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        /* 
         * Permit Create Form Required End Points 
        */

        /*internal async Task<List<CMPermitDetail>> getPermitDetails(int permit_id)
        {
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, CONCAT(user5.firstName,' ',user5.lastName) as closedByName, ptw.cancelRequestDate as cancel_at " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "JOIN facilities as facilities  ON ptw.blockId = facilities.id and facilities.isBlock = 1 " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
              "LEFT JOIN users as user5 ON user5.id = ptw.completedById " +
              $"where ptw.id = { permit_id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            //return permitDetails[0];
            return permitDetails;
        }*/

        private static string Status(int statusID)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    statusName = "Permit Created";
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    statusName = "Permit Issued";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    statusName = "Permit Rejected By Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_APPROVE:
                    statusName = "Permit Approved";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    statusName = "Permit Rejected By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    statusName = "Permit Closed";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    statusName = "Permit Cancelled BY Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    statusName = "Permit Cancelled By HSE";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    statusName = "Permit Cancelled By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_EDIT:
                    statusName = "Permit Edited";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    statusName = "Requested for Permit Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    statusName = "Approved Extension for Permit";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    statusName = "Rejected Extension for Permit";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    statusName = "Permit Linked to Job";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    statusName = "Permit Linked to PM";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    statusName = "Permit Linked to Audit";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    statusName = "Permit Linked to HOTO";
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    statusName = "Permit Expired";
                    break;
                default:
                    statusName = "Invalid";
                    break;
            }
            return statusName;
        }

        internal async Task<List<CMDefaultList>> GetPermitTypeList(int facility_id)
        {
            /*
             * return permit_type_id, name from PermitTypeLists table for requsted facility_id 
            */
            string myQuery = $"SELECT id, title as name FROM permittypelists ";
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            myQuery += $"WHERE facilityId = { facility_id };";
            List<CMDefaultList> _PermitTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _PermitTypeList;
        }

        internal async Task<List<CMSafetyMeasurementQuestionList>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            /*
             * return id, title from PermitTypeSafetyMeasures table for requested permit_type_id
             * input 1 - checkbox, 2 - radio, 3 - text, 4 - Ok
            */
            Dictionary<int, string> InputDictionary = new Dictionary<int, string>
            {
                { 1, "Checkbox" },
                { 2, "Radio" },
                { 3, "Text" },
                { 4, "OK" }
            };
            string inputTypeOut = "CASE ";
            foreach (KeyValuePair<int, string> input in InputDictionary)
            {
                inputTypeOut += $"WHEN permitsaftymea.input = {input.Key} THEN '{input.Value}' ";
            }
            inputTypeOut += $"ELSE 'Invalid Input Type' END";
            string myQuery5 = $"SELECT permitsaftymea.id as id, permitsaftymea.title as name, permitsaftymea.input as inputID, { inputTypeOut } as inputName, ptw.title as permitType FROM permitsafetyquestions  as  permitsaftyques " +
                             "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                             "JOIN permittypelists as ptw ON ptw.id = permitsaftymea.permitTypeId ";
            if(permit_type_id > 0)
                myQuery5 += $"where ptw.id =  { permit_type_id } ";
            myQuery5 += "GROUP BY permitsaftyques.safetyMeasureId ORDER BY ptw.id ASC;";
            List<CMSafetyMeasurementQuestionList> _QuestionList = await Context.GetData<CMSafetyMeasurementQuestionList>(myQuery5).ConfigureAwait(false);
            return _QuestionList;
        }

        internal async Task<List<CMDefaultList>> GetJobTypeList(int facility_id)
        {
            /*
             * return id, title from PermitJobTypeList table for requested facility_id
            */
            string myQuery = $"SELECT id as id, title as name FROM permitjobtypelist ";
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            myQuery += $"WHERE facilityId =  { facility_id } ";
            List<CMDefaultList> _JobTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        internal async Task<List<CMSOPList>> GetSOPList(int job_type_id)
        {
            /*
             * return * from PermitTBTJobList table for requested job_type_id
            */
            string myQuery = $"SELECT tbtlist.id as id, tbtlist.title as name, jobtypes.title as jobTypeName " +
                                $"FROM permittbtjoblist as tbtlist " +
                                $"LEFT JOIN permitjobtypelist as jobtypes ON tbtlist.jobTypeId = jobtypes.id ";
            if (job_type_id > 0)
                myQuery += $"WHERE tbtlist.jobTypeId =  { job_type_id } ";
            myQuery += "ORDER BY jobtypes.id ASC, tbtlist.id ASC;";
            List<CMSOPList> _JobTypeList = await Context.GetData<CMSOPList>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        /*
         * Permit Main Feature End Points
        */

        internal async Task<List<CMPermitList>> GetPermitList(int facility_id, int userID, bool self_view)
        {
            /*
             * Return id as well as string value
             * Use Permits, Assets, AssetsCategory, Users table to fetch below fields
             * Permit id, site Permit No., Permit Type, Equipment Categories, Working Area/Equipment, Description, Permit requested by
             * Request Date/Time, Approved By, Approved Date/Time, Current Status(Approved, Rejected, closed).           
            */
            string statusSubQuery = "CASE ";
            for(int i = 121; i <= 138; i++)
            {
                statusSubQuery += $"WHEN ptw.status = {i} THEN '{Status(i)}' ";
            }
            statusSubQuery += $"ELSE '{Status(0)}' END";
            string myQuery = "SELECT " +
                                 $"ptw.id as permitId, ptw.status as ptwStatus, ptw.permitNumber as permit_site_no, permitType.id as permit_type, permitType.title as PermitTypeName, group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipment_categories, facilities.id as workingAreaId, facilities.name as workingAreaName, ptw.description as description, CONCAT(acceptedUser.firstName , ' ' , acceptedUser.lastName) as request_by_name, ptw.acceptedDate as request_datetime, CONCAT(issuedUser.firstName , ' ' , issuedUser.lastName) as issued_by_name, ptw.issuedDate as issue_datetime, CONCAT(approvedUser.firstName , ' ' , approvedUser.lastName) as approved_by_name, ptw.approvedDate as approved_datetime, {statusSubQuery} as currentStatus " +
                                 " FROM " +
                                        "permits as ptw " +
                                  "JOIN " +
                                        "facilities as facilities ON ptw.blockId = facilities.id " +
                                  "LEFT JOIN " +
                                        "permitlotoassets as loto ON ptw.id = loto.PTW_id " +
                                  "LEFT JOIN " +
                                        "assets ON assets.id = loto.Loto_Asset_id " + 
                                  "LEFT JOIN " +
                                        "assetcategories as asset_cat ON assets.categoryId = asset_cat.id " +
                                  "LEFT JOIN " +
                                         "permittypelists as permitType ON ptw.typeId = permitType.id " +
                                  "LEFT JOIN " +
                                         "jobs as job ON ptw.id = job.linkedPermit " +
                                  "LEFT JOIN " +
                                        "users as issuedUser ON issuedUser.id = ptw.issuedById " +
                                  "LEFT JOIN " +
                                        "users as approvedUser ON approvedUser.id = ptw.approvedById " +
                                  "LEFT JOIN " +
                                        "users as acceptedUser ON acceptedUser.id = ptw.acceptedById ";

            if (facility_id>0)
            {
                myQuery += $"WHERE ptw.facilityId = { facility_id } ";
                if (self_view)
                    myQuery += $"AND user.id = {userID} ";
            }
            myQuery += "GROUP BY ptw.id;";
            //$" WHERE ptw.facilityId = { facility_id } and user.id = { userID } ";
            List<CMPermitList> _PermitList = await Context.GetData<CMPermitList>(myQuery).ConfigureAwait(false);
            return _PermitList;
        }

        internal async Task<CMDefaultResponse> CreatePermit(CMCreatePermit request, int userID)
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
            string qryPermitBasic = "insert into permits(facilityId, blockId, startDate, endDate, title, description, jobTypeId, typeId, issuedById, approvedById, acceptedById, status, latitude, longitude) values" +
             $"({ request.facility_id }, { request.blockId }, '{ request.start_datetime.ToString("yyyy-MM-dd hh:mm:ss") }', '{ request.end_datetime.ToString("yyyy-MM-dd hh:mm:ss") }', '{request.title}', '{ request.description }', { request.job_type_id }, { request.typeId }, { request.issuer_id }, { request.approver_id }, {userID}, {(int)CMMS.CMMS_Status.PTW_CREATED}, {request.latitude}, {request.longitude}); ";
            await Context.ExecuteNonQry<int>(qryPermitBasic).ConfigureAwait(false);

            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, CONCAT(user5.firstName,' ',user5.lastName) as closedByName, ptw.cancelRequestDate as cancel_at " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
              "LEFT JOIN users as user5 ON user5.id = ptw.completedById  order by ptw.id desc limit 1";

            List<CMPermitDetail> permitPrimaryKey = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);
            int insertedId = permitPrimaryKey[0].insertedId;

            string ptwCodeQry = $"UPDATE permits SET code = 'PTW{insertedId}' WHERE id = {insertedId};";
            await Context.ExecuteNonQry<int>(ptwCodeQry).ConfigureAwait(false);

            // qry = "select id as insertedId from permits order by id desc limit 1";
            
            string qryPermitBlock = $"insert into permitblocks(ptw_id, block_id ) value ({ insertedId  }, { request.blockId })";
            await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);

            foreach (int data in request.category_ids)
            {
                string qryPermitCategory = $"insert into permitassetlists (ptwId, assetId ) value ({ insertedId  }, { data })";
                await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
            }

            if (request.is_isolation_required == true)
            {
                foreach (int data in request.isolated_category_ids)
                {
                    string qryPermitisolatedCategory = $"insert into permitisolatedassetcategories (permitId , assetCategoryId ) value ({ insertedId  },{ data })";
                    await Context.ExecuteNonQry<int>(qryPermitisolatedCategory).ConfigureAwait(false);
                }
            }

            foreach (var data in request.Loto_list)
            {
                string qryPermitlotoAssets = $"insert into permitlotoassets ( PTW_id, Loto_Asset_id, Loto_Key ) value ({ insertedId },{ data.Loto_id }, '{ data.Loto_Key }')";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            foreach (var data in request.employee_list)
            {
                string qryPermitEmpList = $"insert into permitemployeelists ( pwtId , employeeId , responsibility ) value ({ insertedId  },{ data.employeeId }, '{ data.responsibility }')";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
            }

            foreach (var data in request.safety_question_list)
            {
                string qryPermitSaftyQuestion = $"insert into permitsafetyquestions ( permitId , safetyMeasureId, safetyMeasureValue) value ({ insertedId  }, { data.safetyMeasureId }, '{ data.safetyMeasureValue }')";
                await Context.ExecuteNonQry<int>(qryPermitSaftyQuestion).ConfigureAwait(false);
            }
            //file_upload_form pending 

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, insertedId, 0, 0, "Permit Created", CMMS.CMMS_Status.PTW_CREATED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CREATED, permitPrimaryKey[0]);

            CMDefaultResponse response = new CMDefaultResponse(insertedId, CMMS.RETRUNSTATUS.SUCCESS, "Permit Created Successfully");

            return response;
        }

        internal async Task<CMPermitDetail> GetPermitDetails(int permit_id)
        {
            /*
             * Return id and string values which are stored in 
             * Permits, PermitBlocks, PermitIsolatedAssetCategories, PermitLOTOAssets, PermitEmployeeLists, PermitSafetyQuestions table
             * for request permit_id Join with below tables to get string value from
             * Assets, AssetsCategory, Facility, Users, PermitTypeSafetyMeasures, PermitTypeList, PermitJobTypeList, PermitTBTJobList
            */

            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, CONCAT(user5.firstName,' ',user5.lastName) as closedByName, ptw.cancelRequestDate as cancel_at " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
              "LEFT JOIN users as user5 ON user5.id = ptw.completedById " +
                $"where ptw.id = { permit_id }";
            List<CMPermitDetail> _PermitDetailsList = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            //get employee list
            string myQuery1 = "SELECT  CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList " +
                               "JOIN permits as ptw  ON ptw.id = ptwEmpList.pwtId " +
                              $"LEFT JOIN users as user ON user.id = ptwEmpList.employeeId where ptw.id = { permit_id }";
            List<CMEMPLIST> _EmpList = await Context.GetData<CMEMPLIST>(myQuery1).ConfigureAwait(false);

            //get isolation list
            string myQuery2 = $"SELECT asset_cat.id as IsolationAssetsCatID, asset_cat.name as IsolationAssetsCatName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id where ptwISOCat.permitId =  {permit_id} GROUP BY asset_cat.id ;";
            List<CMIsolationList> _IsolationList = await Context.GetData<CMIsolationList>(myQuery2).ConfigureAwait(false);

            //get loto
            string myQuery3 = "SELECT assets_cat.id as asset_id, assets_cat.name as asset_name, ptw.lockSrNo as locksrno FROM assetcategories as assets_cat " +
                               "JOIN assets on assets.categoryId = assets_cat.id " +
                               "JOIN permitlotoassets AS LOTOAssets on LOTOAssets.Loto_Asset_id = assets.id " +
                               "JOIN permits as ptw ON LOTOAssets.PTW_id=ptw.id " +
                               $"where ptw.id =  { permit_id }";
            List<CMLoto> _LotoList = await Context.GetData<CMLoto>(myQuery3).ConfigureAwait(false);

            //get upload file
            string myQuery4 = "SELECT PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory,PTWFiles.File_Size as fileSize,PTWFiles.status as status FROM fleximc_ptw_files AS PTWFiles " +
                               $"LEFT JOIN permits  as ptw on ptw.id = PTWFiles.PTW_id where ptw.id = { permit_id }";
            List<CMFileDetail> _UploadFileList = await Context.GetData<CMFileDetail>(myQuery4).ConfigureAwait(false);

            //get safty question
            string myQuery5 = "SELECT permitsaftymea.id as saftyQuestionId, permitsaftymea.title as SaftyQuestionName, permitsaftymea.input as input FROM permitsafetyquestions  as permitsaftyques " +
                               "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                               "JOIN permits as ptw ON ptw.typeId = permitsaftymea.permitTypeId " +
                               $"where ptw.id = { permit_id } GROUP BY saftyQuestionId";
            List<CMSaftyQuestion> _QuestionList = await Context.GetData<CMSaftyQuestion>(myQuery5).ConfigureAwait(false);

            //get Associated Job
            string myQuery6 = "SELECT job.id as JobId, jobCard.id as JobCardId, job.title as JobTitle , job.description as JobDes, job.createdAt as JobDate, job.status as JobStatus FROM jobs as job JOIN permits as ptw ON job.linkedPermit = ptw.id "+
            " LEFT JOIN fleximc_jc_files as jobCard ON jobCard.JC_id = job.id " +
              $"where ptw.id = { permit_id }";
            List<CMAssociatedList> _AssociatedJobList = await Context.GetData<CMAssociatedList>(myQuery6).ConfigureAwait(false);

            //get category list
            string myQuery7 = "SELECT assets_cat.name as equipmentCat FROM assetcategories as assets_cat " +
                               "JOIN assets on assets.categoryId = assets_cat.id " +
                               "JOIN permitassetlists AS assetList on assetList.assetId = assets.id " +
                               "JOIN permits as ptw ON assetList.ptwId=ptw.id " +
                               $"where ptw.id =  { permit_id }";
            List<CMCategory> _CategoryList = await Context.GetData<CMCategory>(myQuery7).ConfigureAwait(false);

            _PermitDetailsList[0].LstLoto = _LotoList;
            _PermitDetailsList[0].LstEmp = _EmpList;
            _PermitDetailsList[0].LstIsolation = _IsolationList;
            _PermitDetailsList[0].file_list = _UploadFileList;
            _PermitDetailsList[0].safety_question_list = _QuestionList;
            _PermitDetailsList[0].LstAssociatedJob = _AssociatedJobList;
            _PermitDetailsList[0].LstCategory = _CategoryList;

            return _PermitDetailsList[0];
        }

        /*         * Permit Issue/Approval/Rejection/Cancel End Points
        */
        internal async Task<CMDefaultResponse> PermitExtend(CMApproval request, int userID)
        {
            string updateQry = $"update permits set extendReason = '{ request.comment }', extendTime = '{ UtilsRepository.GetUTCTime() }', extendStatus = { (int)CMMS.CMMS_Status.PTW_EXTEND_REQUESTED }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
            "FROM permits as ptw " +
            "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
            "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
             "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
             "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
             "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
             "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
             $"where ptw.id = { request.id }";

             //Task<List<CMPermitDetail>> permitDetails = getPermitDetails(request.id);
            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, request.comment, CMMS.CMMS_Status.PTW_EXTEND_REQUESTED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EXTEND_REQUESTED, permitDetails[0]);
            CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit { permitDetails[0].insertedId } Extended");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitExtendApprove(CMApproval request, int userID)
        {
            string updateQry = $"update permits set extendStatus = '{ (int)CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE }', extendApproveTime = '{ UtilsRepository.GetUTCTime() }' where id = { request.id }";
            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
              $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

              await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, "Approve Permit Extend Request", CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE);

              CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE, permitDetails[0]);
              CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId } Extend Approved");
              return response;
        }

        internal async Task<CMDefaultResponse> PermitExtendCancel(CMApproval request, int userID)
        {
            string updateQry = $"update permits set extendStatus = {(int)CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED }, extendRejectReason = '{ request.comment }' where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
                $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);   
            
              await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, request.comment, CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED);

              CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED, permitDetails[0]);
              CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId } Extend Canceled");
              return response;
        }

        internal async Task<CMDefaultResponse> PermitIssue(CMApproval request, int userID)
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */

            string updateQry = $"update permits set issuedReccomendations = '{ request.comment }', issuedStatus = { (int) CMMS.CMMS_Status.PTW_ISSUED }, issuedDate = '{ UtilsRepository.GetUTCTime() }', issuedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
                $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);
           // return permitDetails[0];
               await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, request.comment, CMMS.CMMS_Status.PTW_ISSUED);

               CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_ISSUED, permitDetails[0]);
               CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId } Issued");
               return response;
        }

        internal async Task<CMDefaultResponse> PermitApprove(CMApproval request, int userID)
        {
            /*Update Permit Table reccomendationsByApprover, approvedStatus, approvedDate
                       * Return Message Approved successfully*/

            string updateQry = $"update permits set reccomendationsByApprover = '{ request.comment }', approvedStatus = { (int)CMMS.CMMS_Status.PTW_APPROVE }, approvedDate = '{ UtilsRepository.GetUTCTime() }', approvedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
                  "FROM permits as ptw " +
                  "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
                  "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
                  "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                  "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
                  "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
                  "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
                    $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, request.comment, CMMS.CMMS_Status.PTW_APPROVE);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_APPROVE, permitDetails[0]);
            CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId }  Approve");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitClose(CMApproval request, int userID)
        {

            string updateQry = $"update permits set completedDate = '{ UtilsRepository.GetUTCTime() }', completedStatus = { (int)CMMS.CMMS_Status.PTW_CLOSED }, completedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
                  "FROM permits as ptw " +
                  "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
                  "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
                  "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                  "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
                  "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
                  "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
                  $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, "Permit Close", CMMS.CMMS_Status.PTW_CLOSED);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CLOSED, permitDetails[0]);
            CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId } Close");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitReject(CMApproval request, int userID)
        {
            int userId = Utils.UtilsRepository.GetUserID();
            string updateQry = $"update permits set issuedReccomendations = '{ request.comment }', issuedStatus = { (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER }, issuedDate ='{ UtilsRepository.GetUTCTime() }', issuedById = { userId }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at " +
                 "FROM permits as ptw " +
                 "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
                 "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
                 "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                 "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
                 "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
                 "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
                   $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, request.comment, CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER, permitDetails[0]);
            CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId } Reject");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitCancel(CMApproval request, int userID)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */

            string updateQry = $"update permits set cancelReccomendations = '{ request.comment }', cancelRequestStatus = { (int)CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER }, cancelRequestDate = '{ UtilsRepository.GetUTCTime() }', cancelRequestById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at " +
                "FROM permits as ptw " +
                "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
                "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
                "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
                "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
                "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
                "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById "+
                 $"where ptw.id = { request.id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, permitDetails[0].insertedId, 0, 0, request.comment, CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER, permitDetails[0]);
            CMDefaultResponse response = new CMDefaultResponse(permitDetails[0].insertedId, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { permitDetails[0].insertedId } Cancel");
            return response;
        }

        internal async Task<int> UpdatePermit(CMUpdatePermit request, int userID)
        {
            string updatePermitQry = $"update permits set ";
            if (request.facility_id > 0)
                updatePermitQry += $"facilityId = { request.facility_id }, ";
            if (request.blockId > 0)
                updatePermitQry += $"blockId = { request.blockId }, ";
            if (request.start_date != null)
                updatePermitQry += $"startDate = '{ ((DateTime)request.start_date).ToString("yyyy-MM-dd") }', ";
            if (request.end_date != null)
                updatePermitQry += $"endDate = '{ ((DateTime)request.end_date).ToString("yyyy-MM-dd") }', ";
            if (request.description != null && request.description != "")
                updatePermitQry += $"description = '{ request.description }', ";
            if (request.job_type_id > 0)
                updatePermitQry += $"jobTypeId = { request.job_type_id }, ";
            if (request.typeId > 0)
                updatePermitQry += $"typeId = { request.typeId }, ";
            if (request.sop_type_id > 0)
                updatePermitQry += $"TBTId = { request.sop_type_id }, ";
            if (request.issuer_id > 0)
                updatePermitQry += $"issuedById = { request.issuer_id }, ";
            if (request.approver_id > 0)
                updatePermitQry += $"approvedById = { request.approver_id }, ";
            updatePermitQry += $"acceptedById = {userID} ";
            updatePermitQry += $"where id = { request.permit_id }; ";

            await Context.ExecuteNonQry<int>(updatePermitQry).ConfigureAwait(false);
            int updatePrimaryKey = request.permit_id;

            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as startDate, ptw.endDate as tillDate, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, facilities.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.description as description,CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, CONCAT(user3.firstName,' ',user3.lastName) as completedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, ptw.cancelRequestDate as cancel_at "+
             "FROM permits as ptw " +
             "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
             "LEFT JOIN facilities as facilities  ON ptw.blockId = facilities.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.completedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
            $"where ptw.id = { request.permit_id }";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);
            int insertedId = permitDetails[0].insertedId;

            if(request.block_ids.Count > 0)
            {
                string DeleteQry = $"delete from permitblocks where ptw_id = { request.permit_id };";
                await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

                foreach (var data in request.block_ids)
                {
                    string qryPermitBlock = $"insert into permitblocks(ptw_id, block_id ) value ({ updatePrimaryKey }, { data })";
                    await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);
                }
            }

            if(request.category_ids.Count > 0)
            {
                string DeleteQry1 = $"delete from permitassetlists where ptwId = {request.permit_id};";
                await Context.ExecuteNonQry<int>(DeleteQry1).ConfigureAwait(false);

                foreach (var data in request.category_ids)
                {
                    string qryPermitCategory = $"insert into permitassetlists (ptwId, assetId ) value ({ updatePrimaryKey }, { data })";
                    await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
                }
            }

            if (request.is_isolation_required != null)
            {
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
            }

            if(request.Loto_list.Count > 0)
            {
                string DeleteQry3 = $"delete from permitlotoassets where PTW_id = { request.permit_id };";
                await Context.ExecuteNonQry<int>(DeleteQry3).ConfigureAwait(false);
                foreach (var data in request.Loto_list)
                {
                    string qryPermitlotoAssets = $"insert into permitlotoassets ( PTW_id , Loto_Asset_id, Loto_Key ) value ({ updatePrimaryKey }, { data.Loto_id }, '{ data.Loto_Key }')";
                    await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
                }
            }

            if(request.employee_list.Count > 0)
            {
                string DeleteQry4 = $"delete from permitemployeelists where pwtId = { request.permit_id };";
                await Context.ExecuteNonQry<int>(DeleteQry4).ConfigureAwait(false);
                foreach (var data in request.employee_list)
                {
                    string qryPermitEmpList = $"insert into permitemployeelists ( pwtId , employeeId , responsibility ) value ({ updatePrimaryKey },{ data.employeeId }, '{ data.responsibility}')";
                    await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
                }
            }

            if(request.safety_question_list.Count > 0)
            {
                string DeleteQry5 = $"delete from permitsafetyquestions where permitId = { request.permit_id };";
                await Context.ExecuteNonQry<int>(DeleteQry5).ConfigureAwait(false);
                foreach (var data in request.safety_question_list)
                {
                    string qryPermitSaftyQuestion = $"insert into permitsafetyquestions ( permitId , safetyMeasureId, safetyMeasureValue) value ({ updatePrimaryKey }, { data.safetyMeasureId }, '{ data.safetyMeasureValue }')";
                    await Context.ExecuteNonQry<int>(qryPermitSaftyQuestion).ConfigureAwait(false);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, updatePrimaryKey, 0, 0, "Permit Updated", CMMS.CMMS_Status.PTW_EDIT);

            CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EDIT, permitDetails[0]);
            return updatePrimaryKey;

        }
    }
}