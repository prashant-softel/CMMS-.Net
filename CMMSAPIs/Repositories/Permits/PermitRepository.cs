using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Repositories.Jobs;
using Google.Protobuf.WellKnownTypes;
using System.Collections;

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

        public static string Status(int statusID)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    statusName = "Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    statusName = "Issued";   
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    statusName = "Rejected By Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    statusName = "Approved";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    statusName = "Rejected By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    statusName = "Closed";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    statusName = "Cancelled BY Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    statusName = "Cancelled By HSE";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    statusName = "Cancelled By Approver";
                    break;
				case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    statusName = "Cancel Requested";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    statusName = "Cancel Request Rejected";
	                break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    statusName = "Cancel Approved";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    statusName = "Requested for Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    statusName = "Approved Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    statusName = "Rejected Extension";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    statusName = "Linked to Job";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    statusName = "Linked to PM";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    statusName = "Linked to Audit";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    statusName = "Linked to HOTO";
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    statusName = "Expired";
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    statusName = "Updated";
                    break;
                default:
                    statusName = "Invalid";
                    break;
            }
            return statusName;
        }

        public static string LongStatus(int statusID, CMPermitDetail permitObj)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    statusName = "Permit Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    statusName = "Permit Issued";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    statusName = "Permit Rejected By Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
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
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    statusName = "Cancel Requested for Permit";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    statusName = "Cancel Request Rejected for Permit";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    statusName = "Cancel Request Approved for Permit";
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
                case CMMS.CMMS_Status.PTW_UPDATED:
                    statusName = "Permit Updated";
                    break;
                default:
                    statusName = "Invalid";
                    break;
            }
            return statusName;
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
        internal async Task<List<CMDefaultList>> GetPermitTypeList(int facility_id)
        {
            /*
             * return permit_type_id, name from PermitTypeLists table for requsted facility_id 
            */
            string myQuery = $"SELECT id, title as name FROM permittypelists ";
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            myQuery += $"WHERE facilityId = { facility_id } and status = 1  ORDER BY id DESC;";
            List<CMDefaultList> _PermitTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _PermitTypeList;
        }
        internal async Task<CMDefaultResponse> CreatePermitType(CMCreatePermitType request, int userID)
        {
            string myQuery = "INSERT INTO permittypelists (title, description, facilityId, status, createdBy, createdAt) VALUES " +
                                $"('{request.title}', '{request.description}', {request.facilityId}, 1, {userID}, '{UtilsRepository.GetUTCTime()}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Permit Type added");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdatePermitType(CMCreatePermitType request, int userID)
        {
            if (request.id <= 0)
                throw new ArgumentException("Invalid ID");
            string updateQry = $"UPDATE permittypelists SET";
            if (request.title != null && request.title != "")
                updateQry += $" title = '{request.title}',";
            if (request.description != null && request.description != "")
                updateQry += $" description = '{request.description}',";
            if (request.facilityId > 0)
                updateQry += $" facilityId = {request.facilityId},";
            updateQry += $" updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Permit Type Updated");
            return response;
        }
        internal async Task<CMDefaultResponse> DeletePermitType(int id)
        {
            string deleteQry = $"UPDATE permittypelists SET status = 0 WHERE id = {id}";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Permit Type Deleted");
            return response;
        }
        internal async Task<List<CMSafetyMeasurementQuestionList>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            /*
             * return id, title from PermitTypeSafetyMeasures table for requested permit_type_id
             * input 1 - checkbox, 2 - radio, 3 - text, 4 - Ok
            */

            string inputTypeOut = "CASE ";
            foreach (CMMS.CMMS_Input input in System.Enum.GetValues(typeof(CMMS.CMMS_Input)))
            {
                inputTypeOut += $"WHEN permitsaftymea.input = {(int)input} THEN '{input}' ";
            }
            inputTypeOut += $"ELSE 'Invalid Input Type' END";
            string myQuery5 = $"SELECT distinct permitsaftymea.id as id, permitsaftymea.title as name, permitsaftymea.input as inputID, { inputTypeOut } as inputName, ptw.title as permitType" +
                $", permitsaftymea.discription, permitsaftymea.required as isRequired FROM permittypesafetymeasures as permitsaftymea " +
                             "LEFT JOIN permitsafetyquestions  as  permitsaftyques ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                             "LEFT JOIN permittypelists as ptw ON ptw.id = permitsaftymea.permitTypeId ";
            if (permit_type_id > 0)
                myQuery5 += $"where ptw.id =  { permit_type_id } ";
            //myQuery5 += "GROUP BY permitsaftyques.safetyMeasureId ORDER BY ptw.id ASC;";
            myQuery5 += "ORDER BY ptw.id ASC;";
            List<CMSafetyMeasurementQuestionList> _QuestionList = await Context.GetData<CMSafetyMeasurementQuestionList>(myQuery5).ConfigureAwait(false);
            return _QuestionList;
        }
        internal async Task<CMDefaultResponse> CreateSafetyMeasure(CMCreateSafetyMeasures request, int userID)
        {
            string myQuery = "INSERT INTO permittypesafetymeasures(title, discription, permitTypeId, input, status, required, createdAt, createdBy) VALUES " +
                                $"('{request.title}', '{request.description}', {request.permitType}, {(int)request.input}, 1, {(request.required==null?0:request.required)}, " +
                                $"'{UtilsRepository.GetUTCTime()}', {userID}); SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Safety Measure added");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateSafetyMeasure(CMCreateSafetyMeasures request, int userID)
        {
            if (request.id <= 0)
                throw new ArgumentException("Invalid ID");
            string updateQry = $"UPDATE permittypesafetymeasures SET";
            if (request.title != null && request.title != "")
                updateQry += $" title = '{request.title}',";
            if (request.description != null && request.description != "")
                updateQry += $" discription = '{request.description}',";
            if (request.permitType > 0)
                updateQry += $" permitTypeId = {request.permitType},";
            if (request.input > 0)
                updateQry += $" input = {(int)request.input},";
            if (request.required != null)
                updateQry += $" required = {request.required},";
            updateQry += $" updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Safety Measure Details Updated");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteSafetyMeasure(int id)
        {
            string deleteQry = $"UPDATE permittypesafetymeasures SET status = 0 WHERE id = {id}";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Safety Measure Deleted");
            return response;
        }
        internal async Task<List<CMDefaultList>> GetJobTypeList(int facility_id)
        {
            /*
             * return id, title from PermitJobTypeList table for requested facility_id
            */
            string myQuery = $"SELECT id as id, title as name FROM permitjobtypelist where status = 1  ";
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            myQuery += $" and facilityId =  { facility_id } ";
            List<CMDefaultList> _JobTypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }

        internal async Task<CMDefaultResponse> CreateJobType(CMCreateJobType request, int userID)
        {
            string myQuery = "INSERT INTO permitjobtypelist(title, description, status, facilityId, requireSOPJSA, createdAt, createdBy) VALUES " +
                                $"('{request.title}', '{request.description}', 1, {request.facilityId}, {(request.requires_SOP_JSA == null ? 0 : request.requires_SOP_JSA)}, " +
                                $"'{UtilsRepository.GetUTCTime()}', {userID}); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Job Type Created");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateJobType(CMCreateJobType request, int userID)
        {
            if (request.id <= 0)
                throw new ArgumentException("Invalid ID");
            string updateQry = $"UPDATE permitjobtypelist SET ";
            if (request.title != null && request.title != "")
                updateQry += $"title = '{request.title}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            if (request.facilityId > 0)
                updateQry += $"facilityId = {request.facilityId}, ";
            if (request.requires_SOP_JSA != null)
                updateQry += $"requireSOPJSA = {request.requires_SOP_JSA}, ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Job Type Details Updated");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteJobType(int id)
        {
            string deleteQry = $"UPDATE permitjobtypelist SET status = 0 WHERE id = {id}";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Job Type Deleted");
            return response;
        }
        internal async Task<List<CMSOPList>> GetSOPList(int job_type_id)
        {
            /*
             * return * from PermitTBTJobList table for requested job_type_id
            */
            string myQuery = $"SELECT tbtlist.id as id, tbtlist.title as name, tbtlist.description, jobtypes.title as jobTypeName, tbtlist.fileId as sop_file_id, tbtlist.fileName as sop_file_name, tbtlist.filePath as sop_file_path, sopcat.id as sop_file_cat_id, sopcat.name as sop_file_cat_name, tbtlist.JSAFileId as jsa_file_id, tbtlist.JSAFileName as jsa_file_name, tbtlist.JSAFilePath as jsa_file_path, jsacat.id as jsa_file_cat_id, jsacat.name as jsa_file_cat_name, tbtlist.TBTRemarks as tbt_remarks " +
                                $"FROM permittbtjoblist as tbtlist " +
                                $"LEFT JOIN permitjobtypelist as jobtypes ON tbtlist.jobTypeId = jobtypes.id " +
                                $"LEFT JOIN filecategory as sopcat ON tbtlist.fileCategoryId = sopcat.id " +
                                $"LEFT JOIN filecategory as jsacat ON tbtlist.JSAFileCategoryId = jsacat.id ";
            if (job_type_id > 0)
                myQuery += $"WHERE tbtlist.jobTypeId =  { job_type_id } and tbtlist.status = 1 ";
            myQuery += "ORDER BY jobtypes.id ASC, tbtlist.id ASC;";
            List<CMSOPList> _JobTypeList = await Context.GetData<CMSOPList>(myQuery).ConfigureAwait(false);
            return _JobTypeList;
        }
        internal async Task<CMDefaultResponse> CreateSOP(CMCreateSOP request)
        {
            string myQuery = "INSERT INTO permittbtjoblist (title, description, jobTypeId, fileId, fileDescription, TBTRemarks, JSAFileId, status ) " +
                                $"VALUES ('{request.title}', '{request.description}', {request.tbt_jobType}, {request.sop_fileId}, '{request.sop_file_desc}', " +
                                $"'{request.tbt_remarks}', {request.jsa_fileId}, 1); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            string fileInfoAdd = "UPDATE permittbtjoblist AS sop " +
                                    "JOIN uploadedfiles as sopfile ON sop.fileId = sopfile.id " +
                                    "JOIN uploadedfiles as jsafile ON sop.JSAFileId = jsafile.id " +
                                 "SET " +
                                    "sop.fileName = SUBSTRING_INDEX(sopfile.file_path, '\\\\', -1), " +
                                    "sop.filePath = sopfile.file_path, " +
                                    "sop.fileTypeId = sopfile.file_category, " +
                                    "sop.fileCategoryId = sopfile.file_category, " +
                                    "sop.fileNameChanged = SUBSTRING_INDEX(sopfile.file_path, '\\\\', -1), " +
                                    "sop.JSAFileCategoryId = jsafile.file_category, " +
                                    "sop.JSAFileName = SUBSTRING_INDEX(jsafile.file_path, '\\\\', -1), " +
                                    "sop.JSAFilePath = jsafile.file_path, " +
                                    "sop.JSAFileNameChanged = SUBSTRING_INDEX(jsafile.file_path, '\\\\', -1) " +
                                    $"WHERE sop.id = {id};";
            await Context.FetchData(fileInfoAdd).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "SOP Created");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateSOP(CMCreateSOP request)
        {
            if (request.id <= 0)
                throw new ArgumentException("Invalid ID");
            string updateQry = $"UPDATE permittbtjoblist SET ";
            if (request.title != null && request.title != "")
                updateQry += $"title = '{request.title}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            if (request.tbt_jobType > 0)
                updateQry += $"jobTypeId = {request.tbt_jobType}, ";
            if (request.sop_fileId > 0)
                updateQry += $"fileId = {request.sop_fileId}, ";
            if (request.sop_file_desc != null && request.sop_file_desc != "")
                updateQry += $"fileDescription = '{request.sop_file_desc}', ";
            if (request.tbt_remarks != null && request.tbt_remarks != "")
                updateQry += $"TBTremarks = '{request.tbt_remarks}', ";
            if (request.jsa_fileId > 0)
                updateQry += $"JSAFileId = {request.jsa_fileId}, ";
            updateQry = updateQry.Substring(0, updateQry.Length - 2);
            updateQry += $" WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            string fileInfoAdd = "UPDATE permittbtjoblist AS sop " +
                                    "JOIN uploadedfiles as sopfile ON sop.fileId = sopfile.id " +
                                    "JOIN uploadedfiles as jsafile ON sop.JSAFileId = jsafile.id " +
                                 "SET " +
                                    "sop.fileName = SUBSTRING_INDEX(sopfile.file_path, '\\\\', -1), " +
                                    "sop.filePath = sopfile.file_path, " +
                                    "sop.fileTypeId = sopfile.file_category, " +
                                    "sop.fileCategoryId = sopfile.file_category, " +
                                    "sop.fileNameChanged = SUBSTRING_INDEX(sopfile.file_path, '\\\\', -1), " +
                                    "sop.JSAFileCategoryId = jsafile.file_category, " +
                                    "sop.JSAFileName = SUBSTRING_INDEX(jsafile.file_path, '\\\\', -1), " +
                                    "sop.JSAFilePath = jsafile.file_path, " +
                                    "sop.JSAFileNameChanged = SUBSTRING_INDEX(jsafile.file_path, '\\\\', -1) " +
                                    $"WHERE sop.id = {request.id};";
            await Context.ExecuteNonQry<int>(fileInfoAdd).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "SOP Details Updated");
            return response;
        }
        /*
         * Permit Main Feature End Points
        */
        internal async Task<CMDefaultResponse> DeleteSOP(int id)
        {
            string deleteQry = $"UPDATE permittbtjoblist SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "SOP Deleted");
            return response;
        }
        internal async Task<List<CMPermitList>> GetPermitList(int facility_id, string startDate, string endDate,int userID, bool self_view, bool non_expired) //if current_time>end_time then status = expired
        {
            /*
             * Return id as well as string value
             * Use Permits, Assets, AssetsCategory, Users table to fetch below fields
             * Permit id, site Permit No., Permit Type, Equipment Categories, Working Area/Equipment, Description, Permit requested by
             * Request Date/Time, Approved By, Approved Date/Time, Current Status(Approved, Rejected, closed).           
            */
            var checkFilter = 0;
            string statusSubQuery = "CASE ";
            for (int i = (int)CMMS.CMMS_Status.PTW_CREATED; i <= (int)CMMS.CMMS_Status.PTW_EXPIRED; i++)
            {
                statusSubQuery += $"WHEN ptw.status = {i} THEN '{Status(i)}' ";
            }
            statusSubQuery += $"ELSE '{Status(0)}' END";
            string myQuery = "SELECT " +
                                 $"ptw.id as permitId, CASE when ptw.endDate < '{UtilsRepository.GetUTCTime()}' then 1 else 0 END as isExpired, ptw.code, ptw.status as ptwStatus, ptw.permitNumber as permit_site_no, permitType.id as permit_type, permitType.title as PermitTypeName, group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipment_categories, facilities.id as workingAreaId, facilities.name as workingAreaName, ptw.title as title, ptw.description as description, acceptedUser.id as request_by_id, CONCAT(acceptedUser.firstName , ' ' , acceptedUser.lastName) as request_by_name, ptw.acceptedDate as request_datetime, issuedUser.id as issued_by_id, CONCAT(issuedUser.firstName , ' ' , issuedUser.lastName) as issued_by_name, ptw.issuedDate as issue_datetime, approvedUser.id as approved_by_id, CONCAT(approvedUser.firstName , ' ' , approvedUser.lastName) as approved_by_name, ptw.approvedDate as approved_datetime, {statusSubQuery} as current_status_short " +
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
            if (facility_id > 0)
            {
                myQuery += $"WHERE ptw.facilityId = { facility_id } ";
                 checkFilter = 1;

                if (self_view)
                    myQuery += $"AND ( issuedUser.id = {userID} OR approvedUser.id = {userID} OR acceptedUser.id = {userID} ) ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            if (startDate?.Length > 0 && endDate?.Length > 0)
            {
                if (checkFilter == 0)
                {
                    myQuery += " where ";
                    checkFilter = 1;
                }
                else
                {
                    myQuery += " and ";
                }
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                if (DateTime.Compare(start, end) < 0)
                    myQuery += " DATE_FORMAT(ptw.acceptedDate,'%Y-%m-%d') BETWEEN \'" + startDate + "\' AND \'" + endDate + "\'";
            }
            if(non_expired == true)
            {
                if (checkFilter == 0)
                {
                    myQuery += " where ";
                    checkFilter = 1;
                }
                else
                {
                    myQuery += " and ";
                }

                myQuery += $" ptw.endDate > '{UtilsRepository.GetUTCTime()}' and ptw.status = {(int)CMMS.CMMS_Status.PTW_APPROVED} ";
            }

            myQuery += "GROUP BY ptw.id ORDER BY ptw.id DESC;";
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
            string qryPermitBasic = "insert into permits(facilityId, blockId, LOTOId, startDate, endDate, title, description, jobTypeId, typeId, TBTId, issuedById, approvedById, acceptedById, acceptedDate, status, status_updated_at, latitude, longitude) values" +
             $"({ request.facility_id }, { request.blockId },{request.lotoId},'{ request.start_datetime.ToString("yyyy-MM-dd HH:mm:ss") }', '{ request.end_datetime.ToString("yyyy-MM-dd HH:mm:ss") }', '{request.title}', '{ request.description }', { request.job_type_id }, { request.permitTypeId }, { request.sop_type_id }, { request.issuer_id }, { request.approver_id }, {userID}, '{UtilsRepository.GetUTCTime()}', {(int)CMMS.CMMS_Status.PTW_CREATED}, '{UtilsRepository.GetUTCTime()}', {request.latitude}, {request.longitude}); " +
             $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qryPermitBasic).ConfigureAwait(false);
            int insertedId = Convert.ToInt32(dt.Rows[0][0]);

            string myQuery = "SELECT ptw.id as insertedId, ptw.status as ptwStatus, ptw.startDate as start_datetime, ptw.endDate as end_datetime, facilities.id as facility_id, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, blocks.id as blockId, blocks.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.title as title, ptw.description as description, ptw.jobTypeId as job_type_id, jobType.title as job_type_name, ptw.TBTId as sop_type_id, sop.title as sop_type_name, user1.id as issuer_id, CONCAT(user1.firstName,' ',user1.lastName) as issuedByName, ptw.issuedDate as issue_at, user2.id as approver_id, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName, ptw.approvedDate as approve_at, user3.id as requester_id, CONCAT(user3.firstName,' ',user3.lastName) as requestedByName, ptw.completedDate as close_at, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName, CONCAT(user5.firstName,' ',user5.lastName) as closedByName, ptw.cancelRequestDate as cancel_at " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "LEFT JOIN permitjobtypelist as jobType ON ptw.jobTypeId = jobType.id " +
              "LEFT JOIN permittbtjoblist as sop ON ptw.TBTId = sop.id " +
              "JOIN facilities as facilities  ON ptw.facilityId = facilities.id " +
              "JOIN facilities as blocks  ON ptw.blockId = blocks.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById " +
              "LEFT JOIN users as user3 ON user3.id = ptw.acceptedById " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById " +
              "LEFT JOIN users as user5 ON user5.id = ptw.completedById order by ptw.id desc limit 1";

            List<CMPermitDetail> permitDetails = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            string ptwCodeQry = $"UPDATE permits SET code = CONCAT('PTW', id);";
            await Context.ExecuteNonQry<int>(ptwCodeQry).ConfigureAwait(false);

            // qry = "select id as insertedId from permits order by id desc limit 1";

            string qryPermitBlock = $"insert into permitblocks(ptw_id, block_id ) value ({ insertedId  }, { request.blockId })";
            await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);

            foreach (int data in request.category_ids)
            {
                string qryPermitCategory = $"insert into permitassetlists (ptwId, assetId ) value ({ insertedId  }, { data })";
                await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
            }

            if (request.is_grid_isolation_required == true)
            {
                string qryPermit1= $"Update permits set gridIsolation = 1 ,gridStartDate = '{request.grid_start_datetime.ToString("yyyy-MM-dd HH:mm:ss")}',gridStopDate = '{request.grid_stop_datetime.ToString("yyyy-MM-dd HH:mm:ss")}' ,gridRemark = '{request.grid_remark}' where id = {insertedId}";
                await Context.ExecuteNonQry<int>(qryPermit1).ConfigureAwait(false);
            }

            if (request.is_loto_required == true)
            {
                foreach (int data in request.isolated_category_ids)
                {
                    string qryPermitisolatedCategory = $"insert into permitisolatedassetcategories (permitId , assetCategoryId ) value ({ insertedId  },{ data })";
                    await Context.ExecuteNonQry<int>(qryPermitisolatedCategory).ConfigureAwait(false);
                }

                foreach (var data in request.Loto_list)
                {
                    string qryPermitlotoAssets = $"Update permits set lotoRequired = 1,lotoRemark = '{request.loto_remark}' where id = {insertedId} ;insert into permitlotoassets ( PTW_id, Loto_Asset_id, Loto_Key,lotoLockNo,emp_id ) value ({insertedId},{data.equipment_id}, '{data.Loto_Key}',{data.Loto_lock_number},{data.employee_id})";
                    await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
                }

            }
            string ids = "";
            if (request.is_physical_iso_required == true)
            {
                 ids += (request?.physical_iso_equip_ids?.Length > 0 ? " , physicalIsoEquips = '" + string.Join(" , ", request.physical_iso_equip_ids) + " '" : string.Empty);

                string qryPermit1 = $"Update permits set physicalIsolation = 1 ,physicalIsoRemark = '{request.physical_iso_remark}' {ids} where id = {insertedId}";
                await Context.ExecuteNonQry<int>(qryPermit1).ConfigureAwait(false);
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
            // file upload code
            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.PTW},module_ref_id={insertedId} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, insertedId, 0, 0, "Permit Created", CMMS.CMMS_Status.PTW_CREATED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CREATED,new[] { userID }, permitDetails[0]);

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
            string statusSubQuery = "CASE ";
            for (int i = 121; i <= 140; i++)
            {
                statusSubQuery += $"WHEN ptw.status = {i} THEN '{Status(i)}' ";
            }
            statusSubQuery += $"ELSE '{Status(0)}' END";
            if (permit_id <= 0)
                throw new ArgumentException("Invalid Permit ID");

            string myQuery = $"SELECT ptw.id as insertedId,CONCAT(userTBT.firstName,' ',userTBT.lastName) as TBT_Done_By,CONCAT('HFE/PTW/',ptw.id) as sitePermitNo,TBT_Done_At,CASE when ptw.endDate < '{UtilsRepository.GetUTCTime()}' and ptw.status = {(int)CMMS.CMMS_Status.PTW_APPROVED} then 1 else 0 END as isExpired, ptw.status as ptwStatus, {statusSubQuery} as current_status_short, ptw.startDate as start_datetime, ptw.endDate as end_datetime, facilities.id as facility_id, facilities.name as siteName, ptw.id as permitNo, ptw.permitNumber as sitePermitNo, permitType.id as permitTypeid, permitType.title as PermitTypeName, blocks.id as blockId, blocks.name as BlockName, ptw.permittedArea as permitArea, ptw.workingTime as workingTime, ptw.title as title, ptw.description as description, ptw.jobTypeId as job_type_id, jobType.title as job_type_name, ptw.TBTId as sop_type_id, sop.title as sop_type_name, user1.id as issuer_id, CONCAT(user1.firstName,' ',user1.lastName) as issuedByName,ud1.name as issuerDesignation,co1.name as issuerCompany, ptw.issuedDate as issue_at, user6.id as issueRejectedby_id, CONCAT(user6.firstName,' ',user6.lastName) as issueRejectedByName,co6.name as issueRejecterCompany,ud6.name as issueRejecterDesignation, ptw.rejectedDate as issueRejected_at, user2.id as approver_id, CONCAT(user2.firstName,' ',user2.lastName) as approvedByName,ud2.name as approverDesignation,co2.name as approverCompany, ptw.approvedDate as approve_at,user7.id as rejecter_id, CONCAT(user7.firstName,' ',user7.lastName) as rejectedByName,ud7.name as rejecterDesignation,co7.name as rejecterCompany, ptw.rejectedDate as rejected_at, user3.id as requester_id, CONCAT(user3.firstName,' ',user3.lastName) as requestedByName,ud3.name as requesterDesignation,co3.name as requesterCompany, ptw.completedDate as close_at, user4.id as cancelRequestby_id, CONCAT(user4.firstName,' ',user4.lastName) as cancelRequestByName,ud4.name as cancelRequestByDesignation,co4.name as cancelRequestByCompany,user8.id as cancelRequestApprovedby_id, CONCAT(user8.firstName,' ',user8.lastName) as cancelRequestApprovedByName,ud8.name as cancelRequestApprovedByDesignation,co8.name as cancelRequestApprovedByCompany, user9.id as cancelRequestRejectedby_id, CONCAT(user9.firstName,' ',user9.lastName) as cancelRequestRejectedByName, ud9.name as cancelRequestRejectedByDesignation,co9.name as cancelRequestRejectedByCompany,user5.id as closedby_id, CONCAT(user5.firstName,' ',user5.lastName) as closedByName, ud5.name as closedByDesignation,co5.name as closedByCompany,ptw.cancelRequestDate as cancel_at,ptw.gridIsolation as is_grid_isolation_required,gridStartDate as grid_start_datetime,gridStopDate  as grid_stop_datetime, gridRemark as grid_remark,physicalIsolation as is_physical_iso_required , physicalIsoRemark as physical_iso_remark,lotoRequired as is_loto_required, lotoRemark as loto_remark " +
              "FROM permits as ptw " +
              "LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId " +
              "LEFT JOIN permitjobtypelist as jobType ON ptw.jobTypeId = jobType.id " +
              "LEFT JOIN permittbtjoblist as sop ON ptw.TBTId = sop.id " +
              "JOIN facilities as facilities  ON ptw.facilityId = facilities.id " +
              "JOIN facilities as blocks  ON ptw.blockId = blocks.id " +
              "LEFT JOIN users as user1 ON user1.id = ptw.issuedById left join userroles as ud1 on  user1.roleId = ud1.id left join business as co1 on  user1.companyId = co1.id  " +
              "LEFT JOIN users as user2 ON user2.id = ptw.approvedById left join userroles as ud2 on  user2.roleId = ud2.id left join business as co2 on  user2.companyId = co2.id  " +
              "LEFT JOIN users as user3 ON user3.id = ptw.acceptedById left join userroles as ud3 on  user3.roleId = ud3.id left join business as co3 on  user3.companyId = co3.id  " +
              "LEFT JOIN users as user4 ON user4.id = ptw.cancelRequestById left join userroles as ud4 on  user4.roleId = ud4.id left join business as co4 on  user4.companyId = co4.id  " +
              "LEFT JOIN users as user5 ON user5.id = ptw.completedById left join userroles as ud5 on  user5.roleId = ud5.id left join business as co5 on  user5.companyId = co5.id  " +
              $"LEFT JOIN users as user6 ON user6.id = ptw.rejectedById and ptw.status = {(int)CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER} left join userroles as ud6 on  user6.roleId = ud6.id left join business as co6 on  user6.companyId = co6.id  " +
              $"LEFT JOIN users as user7 ON user7.id = ptw.rejectedById and ptw.status > {(int)CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER} left join userroles as ud7 on  user7.roleId = ud7.id left join business as co7 on  user7.companyId = co7.id  " +
              "LEFT JOIN users as user8 ON user8.id = ptw.cancelRequestApproveById left join userroles as ud8 on  user8.roleId = ud8.id left join business as co8 on  user8.companyId = co8.id " +
              "LEFT JOIN users as user9 ON user9.id = ptw.cancelRequestRejectById left join userroles as ud9 on  user9.roleId = ud9.id left join business as co9 on  user9.companyId = co9.id " +
              " LEFT JOIN users as userTBT ON userTBT.id = ptw.TBT_Done_By " +

                $"where ptw.id = {permit_id}";

            List<CMPermitDetail> _PermitDetailsList = await Context.GetData<CMPermitDetail>(myQuery).ConfigureAwait(false);

            if (_PermitDetailsList.Count == 0)
                throw new MissingMemberException($"Permit with ID {permit_id} not found");
            //get employee list
            string myQuery1 = "SELECT  CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList " +
                               "JOIN permits as ptw  ON ptw.id = ptwEmpList.pwtId " +
                              $"LEFT JOIN users as user ON user.id = ptwEmpList.employeeId where ptw.id = {permit_id}";
            List<CMEMPLIST> _EmpList = await Context.GetData<CMEMPLIST>(myQuery1).ConfigureAwait(false);

            string myQuery11 = $"SELECT physicalIsoEquips FROM permits where id = {permit_id}";
            DataTable dt = await Context.FetchData(myQuery11).ConfigureAwait(false);
            string id = Convert.ToString(dt.Rows[0][0]);

            if (id == "")
            {
                id = "0";
            }
            string myQuery10 = $"SELECT assets.id as id, assets.name as name FROM assets where id IN ({id}) ";                         
            List<CMDefaultList> _physical_iso_equips = await Context.GetData<CMDefaultList>(myQuery10).ConfigureAwait(false);

            //get isolation list
            string myQuery2 = $"SELECT asset_cat.id as IsolationAssetsCatID, asset_cat.name as IsolationAssetsCatName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id where ptwISOCat.permitId =  {permit_id} GROUP BY asset_cat.id ;";
            List<CMIsolationList> _IsolationList = await Context.GetData<CMIsolationList>(myQuery2).ConfigureAwait(false);

            //get loto
            string myQuery3 = "SELECT cat.name as equipment_cat, assets.name as equipment_name, CONCAT(emp.firstName,' ',emp.lastName) as employee_name,loto.Loto_key as Loto_Key,loto.lotoLockNo as Loto_lock_number FROM permitlotoassets as loto " +
                               "JOIN assets on assets.id = loto.Loto_Asset_id " +
                               "JOIN users AS emp on emp.id = loto.emp_id " +
                               "JOIN assetcategories as cat ON assets.categoryId = cat.id " +
                               $"where PTW_id =  {permit_id}";
            List<CMLotoListDetail> _LotoList = await Context.GetData<CMLotoListDetail>(myQuery3).ConfigureAwait(false);

            //get upload file
            //string myQuery4 = "SELECT PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory,PTWFiles.File_Size as fileSize,PTWFiles.status as status FROM st_ptw_files AS PTWFiles " +
            //                   $"LEFT JOIN permits  as ptw on ptw.id = PTWFiles.PTW_id where ptw.id = { permit_id }";

            string myQuery4 = "SELECT U.id, file_path as fileName, FC.name as fileCategory, U.File_Size as fileSize, U.status, '' as ptwFiles FROM uploadedfiles AS U " +
                              " LEFT JOIN permits  as ptw on ptw.id = U.module_ref_id Left join filecategory FC on FC.Id = U.file_category " +
                              " where ptw.id = " + permit_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.PTW + ";";

            List<CMFileDetail> _UploadFileList = await Context.GetData<CMFileDetail>(myQuery4).ConfigureAwait(false);

            //get safty question
            string myQuery5 = "SELECT permitsaftymea.id as saftyQuestionId, permitsaftymea.title as SaftyQuestionName, permitsaftymea.input as input FROM permitsafetyquestions  as permitsaftyques " +
                               "LEFT JOIN permittypesafetymeasures as permitsaftymea ON permitsaftyques.safetyMeasureId = permitsaftymea.id " +
                               "JOIN permits as ptw ON ptw.typeId = permitsaftymea.permitTypeId " +
                               $"where ptw.id = {permit_id} GROUP BY saftyQuestionId";



            List<CMSaftyQuestion> _QuestionList = await Context.GetData<CMSaftyQuestion>(myQuery5).ConfigureAwait(false);

            //get Associated Job
            string joblist = $"Select job.id as jobid, job.status as status, concat(user.firstname, ' ', user.lastname) as assignedto, job.title as title,  job.breakdowntime, job.linkedpermit as permitid, group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as equipmentcat, group_concat(distinct assets.name order by assets.id separator ', ') as equipment from jobs as job left join jobmappingassets as jobassets on job.id = jobassets.jobid left join assetcategories as asset_cat on asset_cat.id = jobassets.categoryid left join assets on assets.id = jobassets.assetid left join users as user on user.id = job.assignedid where job.linkedpermit = {permit_id} group by job.id; ";

            List<CMAssociatedList> _AssociatedJobList = await Context.GetData<CMAssociatedList>(joblist).ConfigureAwait(false);

            string closeQry = $"select `1`,`2`,`3`,`4`,closeOther from permits where id = {permit_id}  ";
            DataTable dt1 = await Context.FetchData(closeQry).ConfigureAwait(false);

            string conQry = $"select id, title as name from permitconditionmaster where type = 1 ";
            List<CMPermitConditions> closeConditions = await Context.GetData<CMPermitConditions>(conQry).ConfigureAwait(false);

            foreach (CMPermitConditions condition in closeConditions)
            {
                DataColumn column = dt1.Columns[condition.id.ToString()];

                if (column != null)
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        int? value = row.Field<int?>(column);

                        if (value.HasValue)
                        {
                            condition.value = value.Value;
                        }

                    }
                }

            }
            if (!string.IsNullOrEmpty(dt1.Rows[0]["closeOther"].ToString()))
            {
                CMPermitConditions other = new CMPermitConditions();
                other.value = 1;
                other.name = dt1.Rows[0]["closeOther"].ToString();
                closeConditions.Add(other);
            }
            string query1 = $"SELECT closeFile FROM permits WHERE id = {permit_id};";
            DataTable dt2 = await Context.FetchData(query1).ConfigureAwait(false);
            string fileIds = Convert.ToString(dt2.Rows[0][0]);

            if (fileIds == "")
            {
                fileIds = "0";
            }
            string files = $"SELECT id as fileId, file_path as path FROM uploadedFiles WHERE id IN ( {fileIds} );";
            List<files> closeFiles = await Context.GetData<files>(files).ConfigureAwait(false);


            _PermitDetailsList[0].closeDetails = new CMPermitConditionDetails();
            _PermitDetailsList[0].closeDetails.conditions = closeConditions;
            _PermitDetailsList[0].closeDetails.files = closeFiles;

            string cancleQry = $"select `5`,`6`,`7`,cancelOther from permits where id = {permit_id}  ";
            DataTable dt3 = await Context.FetchData(cancleQry).ConfigureAwait(false);

            string conQry2 = $"select id, title as name from permitconditionmaster where type = 2 ";
            List<CMPermitConditions> cancleConditions = await Context.GetData<CMPermitConditions>(conQry2).ConfigureAwait(false);

            foreach (CMPermitConditions condition in cancleConditions)
            {
                DataColumn column = dt3.Columns[condition.id.ToString()];

                if (column != null)
                {
                    foreach (DataRow row in dt3.Rows)
                    {
                        int? value = row.Field<int?>(column);

                        if (value.HasValue)
                        {
                            condition.value = value.Value;
                        }

                    }
                }

            }
            if (!string.IsNullOrEmpty(dt3.Rows[0]["cancelOther"].ToString()))
            {
                CMPermitConditions other = new CMPermitConditions();
                other.value = 1;
                other.name = dt3.Rows[0]["cancelOther"].ToString();
                cancleConditions.Add(other);
            }

            string query2 = $"SELECT cancelFile FROM permits WHERE id = {permit_id};";
            DataTable dt4 = await Context.FetchData(query2).ConfigureAwait(false);
            string fileIds2 = Convert.ToString(dt4.Rows[0][0]);

            if(fileIds2 == "")
            {
                fileIds2 = "0";
            }

            string files2 = $"SELECT id as fileId, file_path as path FROM uploadedFiles WHERE id IN ( {fileIds2} );";
            List<files> cancleFiles = await Context.GetData<files>(files2).ConfigureAwait(false);


            _PermitDetailsList[0].cancelDetails = new CMPermitConditionDetails();
            _PermitDetailsList[0].cancelDetails.conditions = cancleConditions;
            _PermitDetailsList[0].cancelDetails.files = cancleFiles;

            string extendQry = $"select `8`,`9`,extendOther from permits where id = {permit_id}  ";
            DataTable dt5 = await Context.FetchData(extendQry).ConfigureAwait(false);

            string conQry3 = $"select id, title as name from permitconditionmaster where type = 3 ";
            List<CMPermitConditions> extendConditions = await Context.GetData<CMPermitConditions>(conQry3).ConfigureAwait(false);

            foreach (CMPermitConditions condition in extendConditions)
            {
                DataColumn column = dt5.Columns[condition.id.ToString()];

                if (column != null)
                {
                    foreach (DataRow row in dt5.Rows)
                    {
                        int? value = row.Field<int?>(column);

                        if (value.HasValue)
                        {
                            condition.value = value.Value;
                        }

                    }
                }

            }
            if (!string.IsNullOrEmpty(dt5.Rows[0]["extendOther"].ToString()))
            {
                CMPermitConditions other = new CMPermitConditions();
                other.value = 1;
                other.name = dt5.Rows[0]["extendOther"].ToString();
                extendConditions.Add(other);
            }
            string query3 = $"SELECT closeFile FROM permits WHERE id = {permit_id};";
            DataTable dt7 = await Context.FetchData(query3).ConfigureAwait(false);
            string fileIds3 = Convert.ToString(dt7.Rows[0][0]);

            if (fileIds3 == "")
            {
                fileIds3 = "0";
            }
            string files3 = $"SELECT id as fileId, file_path as path FROM uploadedFiles WHERE id IN ( {fileIds3} );";
            List<files> extendFiles = await Context.GetData<files>(files3).ConfigureAwait(false);


            _PermitDetailsList[0].extendDetails = new CMPermitConditionDetails();
            _PermitDetailsList[0].extendDetails.conditions = extendConditions;
            _PermitDetailsList[0].extendDetails.files = extendFiles;

            foreach (var job in _AssociatedJobList)
            {

                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(job.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
                job.status_short = _shortStatus;
            }

            //get category list
            string myQuery7 = "SELECT assets_cat.name as equipmentCat FROM assetcategories as assets_cat " +
                               "JOIN assets on assets.categoryId = assets_cat.id " +
                               "JOIN permitassetlists AS assetList on assetList.assetId = assets.id " +
                               "JOIN permits as ptw ON assetList.ptwId=ptw.id " +
                               $"where ptw.id =  {permit_id}";
            List<CMCategory> _CategoryList = await Context.GetData<CMCategory>(myQuery7).ConfigureAwait(false);

            string myQuery8 = "SELECT assets_cat.id as catID FROM assetcategories as assets_cat " +
                               "JOIN assets on assets.categoryId = assets_cat.id " +
                               "JOIN permitassetlists AS assetList on assetList.assetId = assets.id " +
                               "JOIN permits as ptw ON assetList.ptwId=ptw.id " +
                               $"where ptw.id =  {permit_id}";
            DataTable catIDdt = await Context.FetchData(myQuery8).ConfigureAwait(false);
            List<int> _CategoryIDList = catIDdt.GetColumn<int>("catID");

            _PermitDetailsList[0].Loto_list = _LotoList;
            _PermitDetailsList[0].employee_list = _EmpList;
            _PermitDetailsList[0].LstIsolation = _IsolationList;
            _PermitDetailsList[0].file_list = _UploadFileList;
            _PermitDetailsList[0].safety_question_list = _QuestionList;
            _PermitDetailsList[0].LstAssociatedJobs = _AssociatedJobList;
            _PermitDetailsList[0].LstCategory = _CategoryList;
            _PermitDetailsList[0].category_ids = _CategoryIDList;
            _PermitDetailsList[0].physical_iso_equips = _physical_iso_equips;

            return _PermitDetailsList[0];
        }

        /*         * Permit Issue/Approval/Rejection/Cancel End Points
        */
        internal async Task<CMDefaultResponse> PermitExtend(CMPermitExtend request, int userID)
        {
            CMDefaultResponse response = new CMDefaultResponse();

            int extendMinutes = 240;

            if (request.extend_by_minutes > 0)
            {
                extendMinutes = request.extend_by_minutes;
            }

            if (extendMinutes < 720)
            {
                

                string fileIds = "";
                fileIds += (request?.fileIds?.Length > 0 ? " " + string.Join(" , ", request.fileIds) + " " : string.Empty);

                string updateQry = $"update permits set extendReason = '{request.comment}', extendByMinutes = '{extendMinutes}', extendTime = '{UtilsRepository.GetUTCTime()}',extendFile = '{fileIds}', extendStatus = 0, status = {(int)CMMS.CMMS_Status.PTW_EXTEND_REQUESTED} where id = {request.id}";

                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

                int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

                CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

                if (retValue > 0)
                {
                    retCode = CMMS.RETRUNSTATUS.SUCCESS;
                }

                CMPermitDetail permitDetails = await GetPermitDetails(request.id);

                string conditions = "";
                foreach (var column in request.conditionIds)
                {
                    conditions += $" `{column}` = 1, ";
                }

                conditions = conditions.Substring(0, conditions.Length - 2);

                string other = "";

                if (!string.IsNullOrEmpty(request.otherCondition))
                {
                    other = $" , extendOther = '{request.otherCondition}' ";
                }
                string qryCondition = $"update permits set {conditions} {other} where id = {request.id}";

                await Context.ExecuteNonQry<int>(qryCondition).ConfigureAwait(false);

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, $"Permit extension requested [{extendMinutes} Mins]. Reason:" + request.comment, CMMS.CMMS_Status.PTW_EXTEND_REQUESTED, userID);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EXTEND_REQUESTED, new[] { userID }, permitDetails);

                 response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit {request.id} Extension requested [{extendMinutes} Mins]. Reason:" + request.comment);
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit {request.id} Extension requested [{extendMinutes} Mins] : Permit cannot be extended for up to 12 hrs.");
            }
            
            return response;
        }

        internal async Task<CMDefaultResponse> PermitExtendApprove(CMApproval request, int userID)
        {
            string updateQry = $"update permits set extendStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE }, extendApproveTime = '{ UtilsRepository.GetUTCTime() }', endDate = ADDDATE(endDate, INTERVAL extendByMinutes MINUTE) where id = { request.id }";

            List<CMDefaultResp> _Employee = await Context.GetData<CMDefaultResp>(updateQry).ConfigureAwait(false);
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
              
            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, "Permit Extended for 4 hours", CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Extended for 4 hours");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitExtendReject(CMApproval request, int userID)
        {
            string updateQry = $"update permits set extendStatus = 0, status = { (int)CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED }, status_updated_at = '{UtilsRepository.GetUTCTime()}', extendRejectReason = '{ request.comment }' where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }


            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, "Permit Extension cancelled. Reason:"+request.comment, CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Extend Canceled. Reason:"+request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> PermitIssue(CMApproval request, int userID)
        {
            /*
             * Update Permit Table issuedReccomendations, issuedStatus, issuedDate
             * Return Message Issued successfully
            */

            string updateQry = $"update permits set issuedReccomendations = '{ request.comment }', issuedStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_ISSUED }, status_updated_at = '{UtilsRepository.GetUTCTime()}', issuedDate = '{ UtilsRepository.GetUTCTime() }', issuedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            // return permitDetails[0];
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PTW_ISSUED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_ISSUED, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Issued");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitApprove(CMApproval request, int userID)
        {
            /*Update Permit Table reccomendationsByApprover, approvedStatus, approvedDate
                       * Return Message Approved successfully*/

            string updateQry = $"update permits set reccomendationsByApprover = '{ request.comment }', approvedStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_APPROVED }, approvedDate = '{ UtilsRepository.GetUTCTime() }', status_updated_at = '{UtilsRepository.GetUTCTime()}', approvedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PTW_APPROVED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_APPROVED, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id }  Approved");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitClose(CMPermitApproval request, int userID)
        {
            string fileIds = "";
            fileIds +=  (request?.fileIds?.Length > 0 ? " " + string.Join(" , ", request.fileIds) + " " : string.Empty);

            string updateQry = $"update permits set completedDate = '{UtilsRepository.GetUTCTime()}',closeFile= '{fileIds}' , completedStatus = 1, status = {(int)CMMS.CMMS_Status.PTW_CLOSED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', completedById = {userID}  where id = {request.id}";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            string conditions = "";
            foreach (var column in request.conditionIds)
            {
                conditions += $" `{column}` = 1, ";
            }

            conditions = conditions.Substring(0, conditions.Length - 2);

            string other = "";

            if (!string.IsNullOrEmpty(request.otherCondition)){
                other = $" , closeOther = '{request.otherCondition}' ";
            }
            string qryCondition = $"update permits set {conditions} {other} where id = {request.id}";

            await Context.ExecuteNonQry<int>(qryCondition).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, "Permit Closed", CMMS.CMMS_Status.PTW_CLOSED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CLOSED, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  {request.id} Closed");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitIssueReject(CMApproval request, int userID)
        {
            string updateQry = $"update permits set rejectReason = '{ request.comment }', rejectStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER }, status_updated_at = '{UtilsRepository.GetUTCTime()}', rejectedDate ='{ UtilsRepository.GetUTCTime() }', rejectedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, "Permit Rejected by Issuer", CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Issue of Permit  { request.id } Rejected ");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitReject(CMApproval request, int userID)
        {
            string updateQry = $"update permits set rejectReason = '{ request.comment }', rejectStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER }, status_updated_at = '{UtilsRepository.GetUTCTime()}', rejectedDate ='{ UtilsRepository.GetUTCTime() }', rejectedById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, "Permit rejected by Approver. Reason :  " + request.comment, CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitCancelRequest(CMPermitApproval request, int userID)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */

            string fileIds = "";
            fileIds += (request?.fileIds?.Length > 0 ? " " + string.Join(" , ", request.fileIds) + " " : string.Empty);

            string updateQry = $"update permits set cancelReccomendations = '{request.comment}', cancelFile ='{fileIds}',cancelRequestStatus = 1, status = {(int)CMMS.CMMS_Status.PTW_CANCEL_REQUESTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', cancelRequestDate = '{UtilsRepository.GetUTCTime()}', cancelRequestById = {userID}  where id = {request.id}";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            string conditions = "";
            foreach (var column in request.conditionIds)
            {
                conditions += $" `{column}` = 1, ";
            }

            conditions = conditions.Substring(0, conditions.Length - 2);

            string other = "";

            if (!string.IsNullOrEmpty(request.otherCondition))
            {
                other = $" , cancelOther = '{request.otherCondition}' ";
            }
            string qryCondition = $"update permits set {conditions} {other} where id = {request.id}";

            await Context.ExecuteNonQry<int>(qryCondition).ConfigureAwait(false);


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PTW_CANCEL_REQUESTED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CANCEL_REQUESTED, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Cancel Requested for Permit  PTW{request.id}");
            return response;
        }


        internal async Task<CMDefaultResponse> PermitCancelByIssuer(CMApproval request, int userID)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */

            string updateQry = $"update permits set cancelRequestApproveStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER }, status_updated_at = '{UtilsRepository.GetUTCTime()}', cancelRequestApproveDate = '{ UtilsRepository.GetUTCTime() }', cancelRequestApproveById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Cancelled By Issuer {permitDetails.issuedByName}");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitCancelByApprover(CMApproval request, int userID)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */

            string updateQry = $"update permits set cancelRequestApproveStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER }, status_updated_at = '{UtilsRepository.GetUTCTime()}', cancelRequestApproveDate = '{ UtilsRepository.GetUTCTime() }', cancelRequestApproveById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Cancelled By Approver {permitDetails.approvedByName}");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitCancelByHSE(CMApproval request, int userID)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */

            string updateQry = $"update permits set cancelRequestApproveStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE }, status_updated_at = '{UtilsRepository.GetUTCTime()}', cancelRequestApproveDate = '{ UtilsRepository.GetUTCTime() }', cancelRequestApproveById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" Permit  { request.id } Cancelled By HSE");
            return response;
        }

        internal async Task<CMDefaultResponse> PermitCancelReject(CMApproval request, int userID)
        {
            /*
             * Update Permit Table 	cancelReccomendations, cancelRequestDate, cancelRequestStatus
             * Return Message Cancelled successfully
            */

            string updateQry = $"update permits set cancelRequestRejectReason = '{ request.comment }', cancelRequestRejectStatus = 1, status = { (int)CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED }, status_updated_at = '{UtilsRepository.GetUTCTime()}', cancelRequestRejectDate = '{ UtilsRepository.GetUTCTime() }', cancelRequestRejectById = { userID }  where id = { request.id }";
            int retValue = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retValue > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            CMPermitDetail permitDetails = await GetPermitDetails(request.id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.id, 0, 0, "Permit Cancel Request Rejected", CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED, new[] { userID }, permitDetails);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Cancel Request Rejected for Permit **PTW{ request.id }** ");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdatePermit(CMUpdatePermit request, int userID)
        {
            string requesterQuery = $"SELECT acceptedById FROM permits WHERE id = {request.permit_id};";
            DataTable requesterDt = await Context.FetchData(requesterQuery).ConfigureAwait(false);
            if (requesterDt.Rows.Count == 0)
                throw new MissingFieldException($"Permit 'PTW{request.permit_id}' not found");
            int requester = Convert.ToInt32(requesterDt.Rows[0][0]);
            if (requester != userID)
                throw new AccessViolationException("Only requester can update permit info");
            string updatePermitQry = $"update permits set ";
            if (request.facility_id > 0)
                updatePermitQry += $"facilityId = { request.facility_id }, ";
            if (request.blockId > 0)
                updatePermitQry += $"blockId = { request.blockId }, ";
            if (request.start_date != null)
                updatePermitQry += $"startDate = '{ ((DateTime)request.start_date).ToString("yyyy-MM-dd HH:mm:ss") }', ";
            if (request.end_date != null)
                updatePermitQry += $"endDate = '{ ((DateTime)request.end_date).ToString("yyyy-MM-dd HH:mm:ss") }', ";
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
            if (request.resubmit == true)
                updatePermitQry += $"status = {(int)CMMS.CMMS_Status.PTW_CREATED}, ";
	    updatePermitQry += $" TBT_Done_By = {request.TBT_Done_By}, TBT_Done_At = '{request.TBT_Done_At.ToString("yyyy-MM-dd HH:mm:ss")}', ";
            updatePermitQry = updatePermitQry.Substring(0, updatePermitQry.Length - 2);
            updatePermitQry += $" where id = { request.permit_id }; ";

            await Context.ExecuteNonQry<int>(updatePermitQry).ConfigureAwait(false);
            int updatePrimaryKey = request.permit_id;

            CMPermitDetail permitDetails = await GetPermitDetails(request.permit_id);

            if (request.block_ids != null)
            {
                if (request.block_ids.Count > 0)
                {
                    string DeleteQry = $"delete from permitblocks where ptw_id = { request.permit_id };";
                    await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

                    foreach (var data in request.block_ids)
                    {
                        string qryPermitBlock = $"insert into permitblocks(ptw_id, block_id ) value ({ updatePrimaryKey }, { data })";
                        await Context.ExecuteNonQry<int>(qryPermitBlock).ConfigureAwait(false);
                    }
                }
            }
            if (request.category_ids != null)
            {
                if (request.category_ids.Count > 0)
                {
                    string DeleteQry1 = $"delete from permitassetlists where ptwId = {request.permit_id};";
                    await Context.ExecuteNonQry<int>(DeleteQry1).ConfigureAwait(false);

                    foreach (var data in request.category_ids)
                    {
                        string qryPermitCategory = $"insert into permitassetlists (ptwId, assetId ) value ({ updatePrimaryKey }, { data })";
                        await Context.ExecuteNonQry<int>(qryPermitCategory).ConfigureAwait(false);
                    }
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

            if (request.Loto_list != null)
            {
                if (request.Loto_list.Count > 0)
                {
                    string DeleteQry3 = $"delete from permitlotoassets where PTW_id = { request.permit_id };";
                    await Context.ExecuteNonQry<int>(DeleteQry3).ConfigureAwait(false);
                    foreach (var data in request.Loto_list)
                    {
                        string qryPermitlotoAssets = $"insert into permitlotoassets ( PTW_id , Loto_Asset_id, Loto_Key ) value ({ updatePrimaryKey }, { data.Loto_id }, '{ data.Loto_Key }')";
                        await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
                    }
                }
            }

            if (request.employee_list != null)
            {
                if (request.employee_list.Count > 0)
                {
                    string DeleteQry4 = $"delete from permitemployeelists where pwtId = { request.permit_id };";
                    await Context.ExecuteNonQry<int>(DeleteQry4).ConfigureAwait(false);
                    foreach (var data in request.employee_list)
                    {
                        string qryPermitEmpList = $"insert into permitemployeelists ( pwtId , employeeId , responsibility ) value ({ updatePrimaryKey },{ data.employeeId }, '{ data.responsibility}')";
                        await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);
                    }
                }
            }

            if (request.safety_question_list != null)
            {
                if (request.safety_question_list.Count > 0)
                {
                    string DeleteQry5 = $"delete from permitsafetyquestions where permitId = { request.permit_id };";
                    await Context.ExecuteNonQry<int>(DeleteQry5).ConfigureAwait(false);
                    foreach (var data in request.safety_question_list)
                    {
                        string qryPermitSaftyQuestion = $"insert into permitsafetyquestions ( permitId , safetyMeasureId, safetyMeasureValue) value ({ updatePrimaryKey }, { data.safetyMeasureId }, '{ data.safetyMeasureValue }')";
                        await Context.ExecuteNonQry<int>(qryPermitSaftyQuestion).ConfigureAwait(false);
                    }
                }
            }
            CMDefaultResponse response = new CMDefaultResponse();
            if (request.resubmit == true)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.permit_id, 0, 0, "Permit Resubmitted for Approval", CMMS.CMMS_Status.PTW_CREATED,userID);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_CREATED, new[] { userID }, permitDetails);
                 response = new CMDefaultResponse(request.permit_id, CMMS.RETRUNSTATUS.SUCCESS, $"Permit Resubmitted for Approval");

            }
            else
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PTW, request.permit_id, 0, 0, "Permit Updated", CMMS.CMMS_Status.PTW_UPDATED,userID);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.PTW, CMMS.CMMS_Status.PTW_UPDATED, new[] { userID }, permitDetails);
                 response = new CMDefaultResponse(request.permit_id, CMMS.RETRUNSTATUS.SUCCESS, $"Permit Updated Successfully");

            }
            return response;

        }
        internal async Task<List<CMPermitConditions>> GetPermitConditionList(int permit_type_id, int isClose, int isCancle, int isExtend)
        {
            string filter = "";

            filter += (isClose > 0 && isCancle == 0 && isExtend == 0 ? " where type = 1 " : string.Empty);
            filter += (isCancle > 0 && isClose == 0 && isExtend == 0 ? " where type = 2 " : string.Empty);
            filter += (isExtend > 0 && isClose == 0 && isCancle == 0 ? " where type = 3 " : string.Empty);

            string myQuery5 = $"SELECT id, title as name FROM permitconditionmaster {filter}";

            //if (permit_type_id > 0)
            //    myQuery5 += $"where ptw.id =  {permit_type_id} ";

            List<CMPermitConditions> _conditionList = await Context.GetData<CMPermitConditions>(myQuery5).ConfigureAwait(false);
            return _conditionList;
        }

        internal async Task<List<CMDefaultList>> GetIsolationTypeList()
        {
            string myQuery = $"SELECT id as id, name as name FROM permitisolationtypes ";
            List<CMDefaultList> _TypeList = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _TypeList;
        }
    }
}
