using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Models.Notifications;
using System.Data;

namespace CMMSAPIs.Repositories.Incident_Reports
{
    public class IncidentReportRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public IncidentReportRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>()
        {
            { 181, "Submitted" },
            { 182, "Approved" },
            { 183, "Rejected" },
            { 184, "Updated" },
        };
        private bool _IncidentReportDetails;

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.IR_CREATED:     
                    retValue = "Created";
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:     
                    retValue = "Updated";
                    break;
                case CMMS.CMMS_Status.IR_APPROVED:     
                    retValue = "Approved";
                    break;
                case CMMS.CMMS_Status.IR_REJECTED:     
                    retValue = "Rejected";
                    break;               
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }


        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMViewIncidentReport IRObj)
        {
            string retValue = "";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.IR_CREATED:
                    retValue = String.Format("Incident Report <{0}> Created by {1} ", IRObj.id, IRObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:
                    retValue = String.Format("Incident Report <{0}> Updated by {1} ", IRObj.id, IRObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.IR_APPROVED:
                    retValue = String.Format("Incident Report <{0}> Approved by {1} ", IRObj.id, IRObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.IR_REJECTED:
                    retValue = String.Format("Incident Report <{0}> Rejected by {1} ", IRObj.id, IRObj.approved_by_name);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMIncidentList>> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date)
        {
            /*              
             * Fetch all the CMIncidentList model data from Incidents table and join other table to get string value for facility, user using there respctive tables
             * Update the string value of status from constant defined for incident report
             * Your code goes here
            */
            string filter = "incident.facility_id =" + facility_id + " ";

            if (!start_date.IsNull() && !end_date.IsNull())
            {
                filter += "AND DATE(incident.incident_datetime) >= '" + start_date.ToString("yyyy-MM-dd") + "' AND DATE(incident.incident_datetime) <= '" + end_date.ToString("yyyy-MM-dd") + "'";
            }

            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN incident.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string severityName = "CASE ";
            foreach (KeyValuePair<string, int> severity in CMMS.INCIDENT_SEVERITY)
            {
                severityName += $"WHEN incident.severity = {severity.Value} THEN '{severity.Key}' ";
            }
            severityName += $"ELSE 'Invalid severity' END";

            string selectqry = $"SELECT incident.id as id,{severityName} as severity,  incident.description as description , facilities.name as facility_name,blockName.name as block_name, assets.name as equipment_name, incident.risk_level as risk_level, CONCAT(created_by.firstName ,' ' , created_by.lastName) as reported_by_name, incident.created_at as reported_at,CONCAT(user.firstName ,' ' , user.lastName) as approved_by, incident.approved_at as approved_at, CONCAT(user1.firstName , ' ' , user1.lastName) as reported_by_name , {statusOut} as status FROM incidents as incident left JOIN facilities AS facilities on facilities.id = incident.facility_id LEFT JOIN facilities AS blockName on blockName.id = incident.block_id  and blockName.isBlock = 1 JOIN assets as assets on incident.equipment_id = assets.id LEFT JOIN users as user on incident.approved_by = user.id LEFT JOIN users as created_by on incident.created_by = created_by.id LEFT JOIN users as user1 on incident.verified_by = user1.id where " + filter + " order by incident.id asc";

            List<CMIncidentList> getIncidentList = await Context.GetData<CMIncidentList>(selectqry).ConfigureAwait(false);
            return getIncidentList;
        }

        internal async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request,int userId)
        {
           
            CMDefaultResponse response = new CMDefaultResponse();

            string qryIncident = "INSERT INTO incidents" +
                                     "(" +
                                             "facility_id, block_id, equipment_id,severity, risk_level, incident_datetime,victim_id, action_taken_by, action_taken_datetime, inverstigated_by, verified_by, risk_type, esi_applicability, legal_applicability, rca_required, damaged_cost, generation_loss, damaged_cost_curr_id, generation_loss_curr_id, title, description, is_insurance_applicable,insurance, insurance_status, insurance_remark, status,created_by,created_at" +
                                      ")" +
                                      "VALUES" +
                                     "(" +
                                            $"{ request.facility_id }, { request.block_id }, { request.equipment_id },{request.severity_id}, { request.risk_level }, '{(request.incident_datetime).ToString("yyyy-MM-dd HH:mm:ss")}',{request.victim_id}, {request.action_taken_by}, '{(request.action_taken_datetime).ToString("yyyy-MM-dd HH:mm:ss")}', {request.inverstigated_by}, {request.verified_by},{request.risk_type},{request.esi_applicability},{request.legal_applicability},{request.rca_required},{request.damaged_cost},{request.generation_loss},{request.damaged_cost_curr_id},{request.generation_loss_curr_id},'{request.title}','{request.description}',{request.is_insurance_applicable},'{request.insurance}',{request.insurance_status},'{request.insurance_remark}',{(int)CMMS.CMMS_Status.IR_CREATED}, {userId},'{UtilsRepository.GetUTCTime()}'" +
                                     ") ; SELECT LAST_INSERT_ID()";

            DataTable dt2 = await Context.FetchData(qryIncident).ConfigureAwait(false);
            int incident_id = Convert.ToInt32(dt2.Rows[0][0]);
            
            if (incident_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, incident_id, 0, 0, "Incident Report Created", CMMS.CMMS_Status.IR_CREATED);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(incident_id);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_CREATED, _IncidentReportDetails);

                response = new CMDefaultResponse(incident_id, CMMS.RETRUNSTATUS.SUCCESS, "Created Incident Report");
            }
            else{

                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");

            }

            return response;
        }
        
        internal async Task<CMViewIncidentReport> GetIncidentDetailsReport(int id)
        {
            /*risk_type
             * Fetch all the CMViewIncidentReport model data from Incidents table
             * User - Users
             * Facility & Block - Facility
             * Equipment - Assets
             * status - define in constants file
             * 
             * Also fetch the history records from history table for this particular id
             * 
            */

            string strRiskType = "";
            string strParanthesis = "";
            
            foreach (var kvp in CMMS.INCIDENT_RISK_TYPE)
            {
                strRiskType += $"If(risk_type = '{kvp.Key}', '{kvp.Value}',";
                strParanthesis += ")";
            }

            string severityName = "CASE ";
            foreach (KeyValuePair<string, int> severity in CMMS.INCIDENT_SEVERITY)
            {
                severityName += $"WHEN incident.severity = {severity.Value} THEN '{severity.Key}' ";
            }
            severityName += $"ELSE 'Invalid severity' END";


            strRiskType = strRiskType.Substring(0, (strRiskType.Length - 1)) + ",''" + strParanthesis + " as risk_type_name ";


            string myQuery = $"SELECT " +
                               $"incident.id as id,incident.title, incident.description,facilities.id as facility_id, facilities.name as facility_name, blockName.id as block_id, blockName.name as block_name, assets.id as equipment_id,  assets.name as equipment_name,{severityName} as severity,incident.risk_level as risk_level ,IF(risk_level = '1','high',IF(risk_level ='2','medium','Low')) as risk_level_name, incident.incident_datetime as incident_datetime, incident.created_at as reporting_datetime,incident.action_taken_datetime ,user.id as victim_id, user.firstName as victim_name , user1.id as action_taken_by,  CONCAT(user1.firstName, user1.lastName) as action_taken_by_name, user2.id as inverstigated_by ,  CONCAT(user2.firstName, user2.lastName) as inverstigated_by_name , user3.id as verified_by ,CONCAT(user3.firstName, user3.lastName) as verified_by_name, incident.risk_type as risk_type, " + strRiskType + ", IF(esi_applicability = '1', 'YES', 'NO') as esi_applicability_name, IF(legal_applicability = '1', 'YES', 'NO') as legal_applicability_name, IF(rca_required = '1', 'YES', 'NO') as rca_required_name, incident.damaged_cost AS damaged_cost, incident.generation_loss as generation_loss,incident.damaged_cost_curr_id, incident.generation_loss_curr_id, job.id as job_id, job.title as job_name , job.description as description , IF(is_insurance_applicable = '1', 'YES', 'NO') as is_insurance_applicable_name, incident.insurance_status as insurance_status, incident.insurance as insurance_name, incident.insurance_remark as insurance_remark, user4.id as approved_by ,CONCAT(user4.firstName, user4.lastName) as approved_by_name, CONCAT(user5.firstName, user5.lastName) as created_by_name, CONCAT(user6.firstName, user6.lastName) as updated_by_name, incident.status as status, incident.approved_at as approved_at,incident.reject_reccomendations as reject_comment " +
                               " FROM incidents as incident " +
                               "LEFT JOIN facilities AS facilities on facilities.id = incident.facility_id " +
                               "LEFT JOIN facilities AS blockName on blockName.id = incident.block_id  and blockName.isBlock = 1 " +
                               "LEFT JOIN assets as assets on incident.equipment_id = assets.id " +
                               "LEFT JOIN jobs AS job on incident.job_id = job.id " +
                               "LEFT JOIN users as user on incident.victim_id = user.id " +
                               "LEFT JOIN users as user1 on incident.action_taken_by = user1.id " +
                               "LEFT JOIN users as user2 on incident.inverstigated_by = user2.id  " +
                               "LEFT JOIN users as user3 on incident.verified_by = user3.id " +
                               "LEFT JOIN users as user4 on incident.approved_by = user4.id " +
                               "LEFT JOIN users as user5 on incident.created_by = user5.id " +
                               "LEFT JOIN users as user6 on incident.updated_by = user6.id " +
                               " where incident.id = " + id;

            List<CMViewIncidentReport> _IncidentReportList = await Context.GetData<CMViewIncidentReport>(myQuery).ConfigureAwait(false);

            if (_IncidentReportList.Count > 0)
            {

                string myQuery1 = $"SELECT history.moduleRefId as moduleRefId, history.moduleType as moduleType, history.comment as comment FROM history as history left join incidents as incident on incident.id = history.moduleRefId AND history.moduleType = {(int)CMMS.CMMS_Modules.INCIDENT_REPORT} where incident.id= {id}";
                List<CMHistoryLIST> _historyList = await Context.GetData<CMHistoryLIST>(myQuery1).ConfigureAwait(false);

                _IncidentReportList[0].LstHistory = _historyList;

                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_IncidentReportList[0].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INCIDENT_REPORT, _Status);
                _IncidentReportList[0].status_short = _shortStatus;

                CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_IncidentReportList[0].status);
                string _longStatus = getLongStatus(CMMS.CMMS_Modules.INCIDENT_REPORT, _Status_long, _IncidentReportList[0]);
                _IncidentReportList[0].status_long = _longStatus;
            }

            return _IncidentReportList[0];
        }

        internal async Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            CMDefaultResponse response = new CMDefaultResponse();

            string updateQry = $"UPDATE incidents SET";
            if (request.facility_id > 0)
                updateQry += $" facility_id = {request.facility_id},";
            if (request.block_id > 0)
                updateQry += $" block_id = {request.block_id},";
            if (request.risk_level > 0)
                updateQry += $" equipment_id = {request.risk_level},";
            if (request.severity_id > 0)
                updateQry += $" severity = {request.severity_id},";
            if (request.equipment_id > 0)
                updateQry += $" equipment_id = {request.equipment_id},";
            if (request.incident_datetime != DateTime.Parse("01-01-0001 00:00:00"))
                updateQry += $" incident_datetime = '{(request.incident_datetime).ToString("yyyy-MM-dd HH:mm:ss")}',";
            if (request.action_taken_datetime != DateTime.Parse("01-01-0001 00:00:00"))
                updateQry += $" action_taken_datetime = '{(request.action_taken_datetime).ToString("yyyy-MM-dd HH:mm:ss")}',";
            if (request.description != null && request.description != "")
                updateQry += $" description = '{request.description}',";
            if (request.victim_id > 0)
                updateQry += $" victim_id = {request.victim_id},";
            if (request.action_taken_by > 0)
                updateQry += $" action_taken_by = {request.action_taken_by},";
            if (request.inverstigated_by > 0)
                updateQry += $" inverstigated_by = {request.inverstigated_by},";
            if (request.verified_by > 0)
                updateQry += $" verified_by = {request.verified_by},";
            if (request.risk_type > 0)
                updateQry += $" risk_type = {request.risk_type},";
            if (request.damaged_cost > 0)
                updateQry += $" damaged_cost = {request.damaged_cost},";
            if (request.generation_loss > 0)
                updateQry += $" generation_loss = {request.generation_loss},generation_loss_curr_id = {request.generation_loss_curr_id},";
            if (request.job_id > 0)
                updateQry += $" job_id = {request.job_id},";
            if (request.damaged_cost > 0)
                updateQry += $" damaged_cost = {request.damaged_cost},damaged_cost_curr_id = {request.damaged_cost_curr_id},";
            if (request.title != null || request.title != "")
                updateQry += $" title = '{request.title}',";
            if (request.insurance_id > 0)
                updateQry += $" insurance_id = {request.insurance_id},";
            if (request.insurance_status > 0)
                updateQry += $" insurance_status = {request.insurance_status},";
            if (request.damaged_cost > 0)
                updateQry += $" damaged_cost = {request.damaged_cost},";
            if (request.insurance_remark != null || request.insurance_remark != "")
                updateQry += $" insurance_remark = '{request.insurance_remark}',";

            updateQry += $"esi_applicability={request.esi_applicability},legal_applicability={request.legal_applicability},rca_required={request.rca_required},damaged_cost={request.damaged_cost},generation_loss={request.generation_loss},is_insurance_applicable={request.is_insurance_applicable},";

            updateQry += $" updated_by = {userId}, update_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";

            //string updateqry = $"Update incidents" +
            //                      $" set facility_id = { request.facility_id }, block_id={request.block_id}, equipment_id={ request.equipment_id }, risk_level= {request.risk_level}, incident_datetime='{request.incident_datetime }',victim_id={request.victim_id},action_taken_by= {request.action_taken_by},action_taken_datetime = '{ request.action_taken_datetime }', inverstigated_by= {request.inverstigated_by}, verified_by={request.verified_by}, risk_type={request.risk_type},esi_applicability={request.esi_applicability},legal_applicability={request.legal_applicability},rca_required={request.rca_required},damaged_cost={request.damaged_cost},generation_loss={request.generation_loss},job_id= {request.job_id},title='{request.job_id}',description='{request.description}',is_insurance_applicable={request.is_insurance_applicable},insurance='{request.insurance_id}',insurance_status={request.insurance_status},insurance_remark= '{request.insurance_remark}',updated_by={userId},update_at= '{UtilsRepository.GetUTCTime()}'" +
            //                    $" where id =   { request.id}";

           int updateId= await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);


            if (updateId > 0)
            {

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, "Incident Report Updated", CMMS.CMMS_Status.IR_UPDATED);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_UPDATED, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Updated Incident Report Successfully");

            }
            else
            {

                 response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, " Incident Report Update Failed");
            }

            return response;
        }


        internal async Task<CMDefaultResponse> ApproveIncidentReport(int incidentId, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here ask status only
            */
            CMDefaultResponse response = new CMDefaultResponse();

            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_APPROVED},is_approved = 1,approved_by={userId},approved_at= '{UtilsRepository.GetUTCTime()}'  where id = " + incidentId;
            int approve_id= await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (approve_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, incidentId, 0, 0, "Incident Report Approved", CMMS.CMMS_Status.IR_APPROVED);

               CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(incidentId);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_APPROVED, _IncidentReportDetails);

                response = new CMDefaultResponse(incidentId, CMMS.RETRUNSTATUS.SUCCESS, " Incident Report Approved");
            }
            else
            {
                 response = new CMDefaultResponse(incidentId, CMMS.RETRUNSTATUS.FAILURE, " Incident Report approval failed");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> RejectIncidentReport(CMApproveIncident request,int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_REJECTED} ,is_approved = 2,approved_by={userId},approved_at= '{UtilsRepository.GetUTCTime()}',  reject_reccomendations = '{request.comment}'  where id = { request.id}";
            int reject_id= await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse();

            if (reject_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, "Incident Report Rejected", CMMS.CMMS_Status.IR_REJECTED);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_REJECTED, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Report Rejected");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Incident Report approval failed");
            }

            return response;
        }


    }
}
