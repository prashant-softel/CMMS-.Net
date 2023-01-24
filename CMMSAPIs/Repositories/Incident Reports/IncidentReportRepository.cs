using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Incident_Reports
{
    public class IncidentReportRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public IncidentReportRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<List<CMIncidentList>> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date)
        {
            /*
              
             * Fetch all the CMIncidentList model data from Incidents table and join other table to get string value for facility, user using there respctive tables
             * Update the string value of status from constant defined for incident report
             * Your code goes here
            */
            string filter = "(incident.facility_id ="+ facility_id + " AND incident.incident_datetime >= '" + start_date + "' AND incident.incident_datetime <= '" + end_date + "')";
            string selectqry = "SELECT incident.id as id,  incident.description as description , facilities.name as block_name, assets.name as equipment_name, incident.risk_level as risk_level, CONCAT(user.firstName + ' ' + user.lastName) as approved_by, incident.approved_at as approved_at, CONCAT(user1.firstName + ' ' + user1.lastName) as reported_by_name, incident.incident_datetime as created_at , incident.status as status FROM softel_cmms.incidents as incident JOIN softel_cmms.facilities AS facilities on facilities.id = incident.facility_id JOIN softel_cmms.assets as assets on incident.equipment_id = assets.id LEFT JOIN softel_cmms.users as user on incident.approved_by = user.id LEFT JOIN softel_cmms.users as user1 on incident.verified_by = user1.id where " + filter + ""; 
            List<CMIncidentList> getIncidentList = await Context.GetData<CMIncidentList>(selectqry).ConfigureAwait(false);
            return getIncidentList;
        }

        internal async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request)
        {
            /*
             * Table - Incidents
             * Need to insert all CMCreateIncidentReport model details in Incidents table
             * Add created log in history table
             * Your code goes here
            */

            //Add Log in history uncomment the below code
            string qryIncident = "INSERT INTO incidents" +
                                     "(" +
                                             "facility_id, block_id, equipment_id, risk_level, incident_datetime, victim_id, action_taken_by, action_taken_datetime, inverstigated_by, verified_by, risk_type, esi_applicability, legal_applicability, rca_required, damaged_cost, generation_loss, job_id, description, is_insurance_applicable, insurance_status, insurance_remark, approved_by" +
                                      ")" +
                                      "VALUES" +
                                     "(" +
                                            $"{ request.facility_id }, { request.block_id }, { request.equipment_id }, { request.risk_level }, '{ UtilsRepository.GetUTCTime() }',{request.victim_id}, {request.action_taken_by}, '{ UtilsRepository.GetUTCTime() }', {request.inverstigated_by}, {request.verified_by},{request.risk_type},{request.esi_applicability},{request.legal_applicability},{request.rca_required},{request.damaged_cost},{request.generation_loss},{request.job_id},'{request.description}',{request.is_insurance_applicable},{request.insurance_status},'{request.insurance_remark}', {request.approved_by}" +
                                     ")";

            int incident_id = await Context.ExecuteNonQry<int>(qryIncident).ConfigureAwait(false);
             await _utilsRepo.AddHistoryLog(Constant.INCIDENT, incident_id, 0, 0, "Added Incident Report", 0);
             await _utilsRepo.SendNotification(Constant.INCIDENT, 0, incident_id);

            CMDefaultResponse response = new CMDefaultResponse(incident_id, 200, "Added Incident Report");
            return response;
        }

        internal async Task<List<CMViewIncidentReport>> ViewIncidentReport(int id)
        {
            /*
             * Fetch all the CMViewIncidentReport model data from Incidents table
             * User - Users
             * Facility & Block - Facility
             * Equipment - Assets
             * status - define in constants file
             * 
             * Also fetch the history records from history table for this particular id
             * 
            */

            //string data = Constant.INCIDENT_RISK_LEVEL;



         string strRiskType= "foreach (var kvp in Constant.INCIDENT_RISK_TYPE)('Key: {0}, Value: {1}', kvp.Key, kvp.Value)";


            //string strRiskType = "IF(risk_type = '1', 'First-Aids Injury', 'Electric Short')";

            string myQuery = "SELECT " +
                               "incident.id as id, facilities.id as facility_id, facilities.name as facility_name, blockName.id as block_id, blockName.name as block_name, assets.id as equipment_id,  assets.name as equipment_name, incident.risk_level as risk_level ,IF(risk_level = '1','high',IF(risk_level ='2','medium','Low')) as risk_level_name, incident.incident_datetime as incident_datetime, user.id as victim_id, user.firstName as victim_name , user1.id as action_taken_by,  CONCAT(user1.firstName, user1.lastName) as action_taken_by_name, user2.id as inverstigated_by ,  CONCAT(user2.firstName, user2.lastName) as inverstigated_by_name , user3.id as verified_by ,CONCAT(user3.firstName, user3.lastName) as verified_by_name,IF(risk_type = 1 "+ strRiskType + "), IF(esi_applicability = '1', 'YES', 'NO') as esi_applicability_name, IF(legal_applicability = '1', 'YES', 'NO') as legal_applicability_name, IF(rca_required = '1', 'YES', 'NO') as rca_required_name, incident.damaged_cost AS damaged_cost, incident.generation_loss as generation_loss, job.id as job_id, job.title as job_name , job.description as description , IF(is_insurance_applicable = '1', 'YES', 'NO') as is_insurance_applicable_name, incident.insurance_status as insurance_status,  IF(incident.insurance_status ='1','YES','NO') as is_insurance_applicable_name, incident.insurance_remark as insurance_remark, user4.id as approved_by ,CONCAT(user4.firstName, user4.lastName) as approved_by_name, incident.status as status, incident.approved_at as approved_at " +
                               " FROM incidents as incident " +
                               "JOIN facilities AS facilities on facilities.id = incident.facility_id " +
                               "JOIN facilities AS blockName on blockName.id = incident.block_id  and blockName.isBlock = 1 " +
                               "JOIN assets as assets on incident.equipment_id = assets.id " +
                               "JOIN jobs AS job on incident.job_id = job.id " +
                               "LEFT JOIN users as user on incident.victim_id = user.id " +
                               "LEFT JOIN users as user1 on incident.action_taken_by = user1.id " +
                               "LEFT JOIN users as user2 on incident.inverstigated_by = user2.id  " +
                               "LEFT JOIN users as user3 on incident.verified_by = user3.id " +
                               "LEFT JOIN users as user4 on incident.approved_by = user4.id " +
                               " where incident.id = " + id;

            List<CMViewIncidentReport> _IncidentReportList = await Context.GetData<CMViewIncidentReport>(myQuery).ConfigureAwait(false);

            string myQuery1 = "SELECT history.moduleRefId as moduleRefId, history.moduleType as moduleType, history.comment as comment FROM history as history left join incidents as incident on incident.id = history.moduleRefId AND history.moduleType = " + Constant.INCIDENT + " where history.id = "+ _IncidentReportList[0].id;
            List<CMHistoryLIST> _historyList = await Context.GetData<CMHistoryLIST>(myQuery1).ConfigureAwait(false);

            _IncidentReportList[0].LstHistory = _historyList;

            return _IncidentReportList;
        }

        internal async Task<CMDefaultResponse> UpdateIncidentReport(CMCreateIncidentReport request)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */

            string updateqry = $"Update incidents" +
                                  $" set facility_id = { request.facility_id }, block_id={request.block_id}, equipment_id={ request.equipment_id }, risk_level= {request.risk_level}, incident_datetime='{ UtilsRepository.GetUTCTime() }',victim_id={request.victim_id},action_taken_by= {request.action_taken_by},action_taken_datetime = '{ UtilsRepository.GetUTCTime() }', inverstigated_by= {request.inverstigated_by}, verified_by={request.verified_by}, risk_type={request.risk_type},esi_applicability={request.esi_applicability},legal_applicability={request.legal_applicability},rca_required={request.rca_required},damaged_cost={request.damaged_cost},generation_loss={request.generation_loss},job_id= {request.job_id},description='{request.description}',is_insurance_applicable={request.is_insurance_applicable},insurance_status={request.insurance_status},insurance_remark= '{request.insurance_remark}',approved_by={request.approved_by}" +
                                $" where id =   { request.id}";

            await Context.ExecuteNonQry<int>(updateqry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(Constant.INCIDENT, request.id, 0, 0, "Update Incident Report", Constant.INCIDENT);
            await _utilsRepo.SendNotification(Constant.INCIDENT, Constant.JC_APPROVED, request.id);
            CMDefaultResponse response = new CMDefaultResponse(request.id, 200, "Updated Incident Report Successfully");

            return response;
        }


        internal async Task<CMDefaultResponse> ApproveIncidentReport(int id)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here ask status only
            */

            string approveQuery = $"Update incidents set status = {Constant.IR_APPROVED} where id = " + id;
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            CMLog _log = new CMLog();

            await _utilsRepo.AddHistoryLog(Constant.INCIDENT, id, 0, 0, "Approve incident Report", Constant.IR_APPROVED);
            await _utilsRepo.SendNotification(Constant.INCIDENT, Constant.IR_APPROVED, id);

            CMDefaultResponse response = new CMDefaultResponse(id, 200, "Approve incident Report");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectIncidentReport(CMApproveIncident request)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            string approveQuery = $"Update incidents set status = {Constant.IR_REJECTED} , reject_reccomendations = '{request.comment}'  where id = { request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            CMLog _log = new CMLog();

            await _utilsRepo.AddHistoryLog(Constant.INCIDENT, request.id, 0, 0, request.comment, Constant.IR_REJECTED);
            await _utilsRepo.SendNotification(Constant.INCIDENT, Constant.IR_REJECTED, request.id);

            CMDefaultResponse response = new CMDefaultResponse(request.id, 200, request.comment);
            return response;
        }


    }
}