using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.Masters
{
    public class MISEvaluationRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;


        public MISEvaluationRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }

        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>()
        {
            { (int)CMMS.CMMS_Status.EVAL_PLAN_CREATED, "Created" },
            { (int)CMMS.CMMS_Status.EVAL_PLAN_UPDATED, "Updated" },
            { (int)CMMS.CMMS_Status.EVAL_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.EVAL_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.EVAL_PLAN_DELETED, "Deleted" }
        };

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMEvaluation evalObj)
        {
            string retValue = "";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.EVAL_PLAN_CREATED:
                    retValue = String.Format("EVAL{0} Created by {1}", evalObj.id, evalObj.createdByName);
                    break;
                case CMMS.CMMS_Status.EVAL_PLAN_UPDATED:
                    retValue = String.Format("EVAL{0} Updated by {1}", evalObj.id, evalObj.updatedByName);
                    break;
                case CMMS.CMMS_Status.EVAL_PLAN_APPROVED:
                    retValue = String.Format("EVAL{0} Approved by {1}", evalObj.id, evalObj.approvedByName);
                    break;
                case CMMS.CMMS_Status.EVAL_PLAN_REJECTED:
                    retValue = String.Format("EVAL{0} Rejected by {1}", evalObj.id, evalObj.rejectedByName);
                    break;
                case CMMS.CMMS_Status.EVAL_PLAN_DELETED:
                    retValue = String.Format("EVAL{0} Deleted by {1}", evalObj.id, evalObj.deletedByName);
                    break;
                default:
                    retValue = String.Format(" No status for EVAL{0}", evalObj.id); ;
                    break;
            }
            return retValue;

        }


        
        internal async Task<CMDefaultResponse> CreateEvaluationPlan(CMEvaluationCreate request, int user_id)
        {

            string addPlanQry = $"INSERT INTO evaluation_plan(plan_name, facility_id, frequency_id, plan_date, assigned_to, " +
                    $"created_by, created_at, remarks, status, status_updated_at) VALUES " +
                    $"('{request.plan_name}', {request.facility_id}, {request.frequency_id}, " +
                    $"'{request.plan_date.ToString("yyyy-MM-dd")}', {request.assigned_to}, {user_id},'{UtilsRepository.GetUTCTime()}' , " +
                    $"'{request.remarks}',{(int)CMMS.CMMS_Status.EVAL_PLAN_CREATED}, '{UtilsRepository.GetUTCTime()}'); " +
                    $"SELECT LAST_INSERT_ID();";
            DataTable dt3 = await Context.FetchData(addPlanQry).ConfigureAwait(false);
            int evaluationPlanId = Convert.ToInt32(dt3.Rows[0][0]);

            string mapChecklistQry = "INSERT INTO evalution_checklist_map(evalution_plan_id, checklist_id, weightage,comments,created_by,created_at) VALUES ";
            foreach (var map in request.map_checklist)
            {
                mapChecklistQry += $"({evaluationPlanId}, {map.checklist_id}, {map.weightage},'{map.comment}',{user_id},'{UtilsRepository.GetUTCTime()}'), ";
            }

            // Remove the trailing comma and space, and append the semicolon
            if (mapChecklistQry.Length > 0)
            {
                mapChecklistQry = mapChecklistQry.TrimEnd(',', ' ') + ";";
            }

            // Execute the query
            await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.EVAL_PLAN, evaluationPlanId, 0, 0, "Eval Plan added", CMMS.CMMS_Status.EVAL_PLAN_CREATED, user_id);
            return new CMDefaultResponse(evaluationPlanId, CMMS.RETRUNSTATUS.SUCCESS, request.remarks);
        }


        internal async Task<List<CMEvaluation>> GetEvaluationList(string facility_Id, DateTime fromDate, DateTime toDate, string facilitytimeZone)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN ep.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery = $"SELECT ep.id, ep.plan_Name, ep.facility_id, ep.frequency_id, ep.plan_date, ep.assigned_to, ep.status, ep.created_at, ep.approved_At, ep.rejected_at, " +
                $" ep.remarks, ep.status, facilityidName.Name AS facilityidName, {statusOut} AS status_short," +
                $" ep.updated_by AS updatedById, ep.updated_at, ep.approved_by AS approvedById, ep.approved_at, ep.rejected_by AS rejectedById, ep.rejected_at, ep.deleted_by AS deletedById, ep.deleted_at, " +
                $"CONCAT(createdByUser.firstName, ' ', createdByUser.lastName) AS createdByName, CONCAT(updatedByUser.firstName, ' ', updatedByUser.lastName) AS updatedByName, " +
                $"CONCAT(approvedByUser.firstName, ' ', approvedByUser.lastName) AS approvedByName, CONCAT(rejectedByUser.firstName, ' ', rejectedByUser.lastName) AS rejectedByName, CONCAT(deletedByUser.firstName, ' ', deletedByUser.lastName) AS deletedByName, " +
                $"GROUP_CONCAT(CONCAT(sta.Audit_Title) SEPARATOR ', ') AS audit_details " +  // Collecting audit details
                $"FROM evaluation_plan AS ep " +
                $"LEFT JOIN facilities AS facilityidName ON facilityidName.id = ep.facility_id " +
                $"LEFT JOIN users AS createdByUser ON createdByUser.id = ep.created_by " +
                $"LEFT JOIN users AS updatedByUser ON updatedByUser.id = ep.updated_by " +
                $"LEFT JOIN users AS approvedByUser ON approvedByUser.id = ep.approved_by " +
                $"LEFT JOIN users AS rejectedByUser ON rejectedByUser.id = ep.rejected_by " +
                $"LEFT JOIN users AS deletedByUser ON deletedByUser.id = ep.deleted_by " +
                $"LEFT JOIN evalution_auditmap AS ea ON ea.evalution_id = ep.id " +
                $"LEFT JOIN st_audit AS sta ON sta.id = ea.audit_id " + // Join with audit table
                $"WHERE ep.facility_id IN(" + facility_Id + ")  AND ep.Deleted = 0 "+
                $"AND DATE_FORMAT(ep.created_at, '%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "' " +
                $"GROUP BY ep.id;";
            List<CMEvaluation> Result = await Context.GetData<CMEvaluation>(myQuery).ConfigureAwait(false);




            return Result;
        }

        internal async Task<CMEvaluation> GetEvaluationDetails(int id, string facilitytimeZone)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN ep.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string planQuery = $"SELECT ep.id, ep.plan_Name, ep.facility_id, ep.frequency_id, ep.plan_date, ep.assigned_to, ep.status, ep.created_at, ep.approved_At, ep.rejected_at, " +
                $" ep.remarks, ep.status, facilityidName.Name AS facilityidName, {statusOut} AS status_short," +
                $" ep.updated_by AS updatedById, ep.updated_at, ep.approved_by AS approvedById, ep.approved_at, ep.rejected_by AS rejectedById, ep.rejected_at, ep.deleted_by AS deletedById, ep.deleted_at, " +
                $"CONCAT(createdByUser.firstName, ' ', createdByUser.lastName) AS createdByName, CONCAT(updatedByUser.firstName, ' ', updatedByUser.lastName) AS updatedByName, " +
                $"CONCAT(approvedByUser.firstName, ' ', approvedByUser.lastName) AS approvedByName, CONCAT(rejectedByUser.firstName, ' ', rejectedByUser.lastName) AS rejectedByName, CONCAT(deletedByUser.firstName, ' ', deletedByUser.lastName) AS deletedByName " +
                $"FROM evaluation_plan AS ep " +
                $"LEFT JOIN facilities AS facilityidName ON facilityidName.id = ep.facility_id " +
                $"LEFT JOIN users AS createdByUser ON createdByUser.id = ep.created_by " +
                $"LEFT JOIN users AS updatedByUser ON updatedByUser.id = ep.updated_by " +
                $"LEFT JOIN users AS approvedByUser ON approvedByUser.id = ep.approved_by " +
                $"LEFT JOIN users AS rejectedByUser ON rejectedByUser.id = ep.rejected_by " +
                $"LEFT JOIN users AS deletedByUser ON deletedByUser.id = ep.deleted_by " +
                $"where ep.id = {id};";


            List<CMEvaluation> _ViewEvaluation = await Context.GetData<CMEvaluation>(planQuery).ConfigureAwait(false);


            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewEvaluation[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.EVAL_PLAN, _Status_long, _ViewEvaluation[0]);
            _ViewEvaluation[0].status_long = _longStatus;

            string auditQuery = $"SELECT ea.id, ea.evalution_id, ea.audit_id, ea.weightage, ea.comments AS comment FROM evalution_auditmap AS ea WHERE ea.evalution_id = {id};";
            List<CMEvaluationAudit> _ViewAudit = await Context.GetData<CMEvaluationAudit>(auditQuery).ConfigureAwait(false);

            // Assign the fetched audit list to the audit_list property
            _ViewEvaluation[0].audit_list = _ViewAudit;
            return _ViewEvaluation[0];
        }

        internal async Task<CMDefaultResponse> UpdateEvaluationPlan(List<CMEvaluationUpdate> requests, int userId, string facilitytimeZone)
        {

            //int cleaningType1;
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.EVAL_PLAN;
            CMDefaultResponse response = null;

            foreach (CMEvaluationUpdate request in requests)
            {
                int rid = request.id;

                string Query = "UPDATE evaluation_plan SET ";
                if (!string.IsNullOrEmpty(request.plan_name))
                    Query += $"plan_name = '{request.plan_name}', ";
                if (request.frequency_id > 0)
                    Query += $"frequency_id = {request.frequency_id}, ";
                if (request.assigned_to > 0)
                    Query += $"assigned_to = {request.assigned_to}, ";
                if (!string.IsNullOrEmpty(request.remarks))
                    Query += $"remarks = '{request.remarks}', ";
                if (request.plan_date != null && request.plan_date != DateTime.MinValue)
                {
                    Query += $"plan_date = '{request.plan_date.ToString("yyyy-MM-dd HH:mm:ss")}', ";
                }

                // Add updated_at and updated_by
                Query += $"updated_at = '{UtilsRepository.GetUTCTime()}', updated_by = {userId} WHERE id = {request.id};";

                await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);

                await _utilsRepo.AddHistoryLog(module, rid, 0, 0, request.remarks, CMMS.CMMS_Status.UPDATED, userId);


                string auditListQry = $"UPDATE evalution_auditmap SET ";
                if (request.map_checklist != null && request.map_checklist.Count > 0)
                {
                    foreach (var audit in request.map_checklist)
                    {
                        // Construct the UPDATE query for each audit record
                        if (audit.weightage > 0)
                            auditListQry += $"weightage = {audit.weightage}, ";
                        if (!string.IsNullOrEmpty(audit.comment))
                            auditListQry += $"comments = '{audit.comment?.Replace("'", "''") ?? ""}' ";
                        else
                        {
                            if (auditListQry.Length > 0)
                            {
                                auditListQry = auditListQry.TrimEnd(',', ' ') + ";";
                            }
                        }
                        auditListQry += $"WHERE id = {audit.id}; ";
                        
                        await Context.ExecuteNonQry<int>(auditListQry).ConfigureAwait(false);

                    }
                }


                // Set the response after the last request
                response = new CMDefaultResponse(rid, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully");
            }

            // Return the response after all requests are processed
            return response ?? new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "No plans were updated");
        }
    
        internal async Task<CMDefaultResponse> ApproveEvaluationPlan(CMApproval request, int userID)
        {
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.EVAL_PLAN;
            CMMS.CMMS_Status status = CMMS.CMMS_Status.EVAL_PLAN_APPROVED;

            string approveQuery = $"Update evaluation_plan set status = {(int)status} ,approved_by={userID}, approveRemark='{request.comment}', approved_at='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            //ADD REMARK TO HISTORY
            //add db col rejectRemark approveRemark
            //see if comment is sent, else set comment "Evaludation Plan rejected"

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.EVAL_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.EVAL_PLAN_APPROVED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Evaluation Approved");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectEvaluationPlan(CMApproval request, int userID)
        {
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.EVAL_PLAN;
            CMMS.CMMS_Status status = CMMS.CMMS_Status.EVAL_PLAN_REJECTED;

            string approveQuery = $"Update evaluation_plan set status = {(int)status} ,rejected_by={userID}, rejectRemark='{request.comment}', rejected_at='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            //ADD REMARK TO HISTORY
            //see if comment is sent, else set comment "Evaludation Plan rejected"
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.EVAL_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.EVAL_PLAN_REJECTED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Evaluation Plan Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteEvaluationPlan(int ID, int userID)
        {
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.EVAL_PLAN;
            int status = (int)CMMS.CMMS_Status.EVAL_PLAN_DELETED;

            // Update query to set deleted_by_id and status
            string deleteQuery = $"UPDATE evaluation_plan SET deleted_by = {userID}, status = {status}, Deleted = {userID}, deleted_at='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {ID};";
            await Context.ExecuteNonQry<int>(deleteQuery).ConfigureAwait(false);

            // Log the history for the deletion
            await _utilsRepo.AddHistoryLog(module, ID, 0, 0, "Evaluation Plan Deleted", (CMMS.CMMS_Status)status, userID);

            // Return response indicating success
            CMDefaultResponse response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.SUCCESS, $"Evaluation Plan Deleted");
            return response;
        }

    }
}
