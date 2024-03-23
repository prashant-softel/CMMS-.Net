using CMMSAPIs.Helper;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Models.Notifications;
using System.Data;
using System.Text;

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
            { 181, "Created-waiting for approval" },
            { 182, "Create-Rejected" },
            { 183, "Created" },
            { 184, "Investigation-waiting for approval" },
            { 185, "Investigation-Rejected" },
            { 186, "Investigation-Completed" },
            { 187, "Updated" },
            { 188, "Cancelled" },
        };
        private bool _IncidentReportDetails;

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.IR_CREATED_INITIAL:     
                    retValue = "Created-waiting for approval";
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:     
                    retValue = "Updated";
                    break;
                case CMMS.CMMS_Status.IR_APPROVED_INITIAL:     
                    retValue = "Created";
                    break;
                case CMMS.CMMS_Status.IR_REJECTED_INITIAL:     
                    retValue = "Create-Rejected";
                    break;
                case CMMS.CMMS_Status.IR_CREATED_INVESTIGATION:
                    retValue = "Investigation-waiting for approval";
                    break;
                case CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION:
                    retValue = "Investigation-Completed";
                    break;
                case CMMS.CMMS_Status.IR_REJECTED_INVESTIGATION:
                    retValue = "Investigation-Rejected";
                    break;
                case CMMS.CMMS_Status.IR_Cancel:
                    retValue = "Cancelled";
                    break;
                case CMMS.CMMS_Status.IR_Close:
                    retValue = "Closed";
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
                case CMMS.CMMS_Status.IR_CREATED_INITIAL:
                    retValue = String.Format("Incident Created-waiting for approval Report <{0}> Created by {1} ", IRObj.id, IRObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.IR_UPDATED:
                    retValue = String.Format("Incident Report <{0}> Updated by {1} ", IRObj.id, IRObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.IR_APPROVED_INITIAL:
                    retValue = String.Format("Incident Report <{0}> Created by {1} ", IRObj.id, IRObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.IR_REJECTED_INITIAL:
                    retValue = String.Format("Incident Report <{0}> Create-Rejected by {1} ", IRObj.id, IRObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.IR_CREATED_INVESTIGATION:
                    retValue = String.Format("Incident Investigation Report <{0}> Created by {1} ", IRObj.id, IRObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION:
                    retValue = String.Format("Incident Investigation Report <{0}> Approved by {1} ", IRObj.id, IRObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.IR_REJECTED_INVESTIGATION:
                    retValue = String.Format("Incident Investigation Report <{0}> Rejected by {1} ", IRObj.id, IRObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.IR_Cancel:
                    retValue = String.Format("Incident Report <{0}> Cancelled by {1} ", IRObj.id, IRObj.updated_by_name);
                    break;
                case CMMS.CMMS_Status.IR_Close:
                    retValue = String.Format("Incident Report <{0}> Closed by {1} ", IRObj.id, IRObj.updated_by_name);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMIncidentList>> GetIncidentList(int facility_id, DateTime start_date, DateTime end_date,string facilitytimeZone)
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

            string selectqry = $"SELECT incident.id as id,{severityName} as severity,  incident.description as description , facilities.name as facility_name,blockName.name as block_name, assets.name as equipment_name, incident.risk_level as risk_level, CONCAT(created_by.firstName ,' ' , created_by.lastName) as reported_by_name, incident.created_at as reported_at,CONCAT(user.firstName ,' ' , user.lastName) as approved_by, incident.approved_at as approved_at, CONCAT(user1.firstName , ' ' , user1.lastName) as reported_by_name , {statusOut} as status_short , incident.location_of_incident, incident_datetime, type_of_job,title,\r\nincident.status, incident.is_why_why_required, incident.is_investigation_required FROM incidents as incident left JOIN facilities AS facilities on facilities.id = incident.facility_id LEFT JOIN facilities AS blockName on blockName.id = incident.block_id  and blockName.isBlock = 1 LEFT JOIN assets as assets on incident.equipment_id = assets.id LEFT JOIN users as user on incident.approved_by = user.id LEFT JOIN users as created_by on incident.created_by = created_by.id LEFT JOIN users as user1 on incident.verified_by = user1.id where " + filter + " order by incident.id asc";

            List<CMIncidentList> getIncidentList = await Context.GetData<CMIncidentList>(selectqry).ConfigureAwait(false);
            foreach ( var getlist in getIncidentList)
            {
                if (getlist!=null && getlist.approved_at!=null)
                getlist.approved_at= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, getlist.approved_at);
                if (getlist!=null && getlist.incident_datetime!=null)
                    getlist.incident_datetime= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)getlist.incident_datetime);
                if (getlist!=null && getlist.reported_at!=null)
                    getlist.reported_at= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, getlist.reported_at);

            }
            return getIncidentList;
        }

        internal async Task<CMDefaultResponse> CreateIncidentReport(CMCreateIncidentReport request,int userId)
        {
           
            CMDefaultResponse response = new CMDefaultResponse();

            //string qryIncident = "INSERT INTO incidents" +
            //                         "(" +
            //                                 "facility_id, block_id, equipment_id,severity, risk_level, incident_datetime,victim_id, action_taken_by, action_taken_datetime, inverstigated_by, verified_by, risk_type, esi_applicability, legal_applicability, rca_required, damaged_cost, generation_loss, damaged_cost_curr_id, generation_loss_curr_id, title, description, is_insurance_applicable,insurance, insurance_status, insurance_remark, status,created_by,created_at, status_updated_at" +
            //                          ")" +
            //                          "VALUES" +
            //                         "(" +
            //                                $"{ request.facility_id }, { request.block_id }, { request.equipment_id },{request.severity_id}, { request.risk_level }, '{(request.incident_datetime).ToString("yyyy-MM-dd HH:mm:ss")}',{request.victim_id}, {request.action_taken_by}, '{(request.action_taken_datetime).ToString("yyyy-MM-dd HH:mm:ss")}', {request.inverstigated_by}, {request.verified_by},{request.risk_type},{request.esi_applicability},{request.legal_applicability},{request.rca_required},{request.damaged_cost},{request.generation_loss},{request.damaged_cost_curr_id},{request.generation_loss_curr_id},'{request.title}','{request.description}',{request.is_insurance_applicable},'{request.insurance}',{request.insurance_status},'{request.insurance_remark}',{(int)CMMS.CMMS_Status.IR_CREATED}, {userId},'{UtilsRepository.GetUTCTime()}','{UtilsRepository.GetUTCTime()}' " +
            //                         ") ; SELECT LAST_INSERT_ID()";

            string qryIncident = "INSERT INTO incidents" +
                     "(" +
                         "facility_id, block_id, equipment_id, severity, risk_level, incident_datetime, victim_id, action_taken_by, action_taken_datetime, inverstigated_by, verified_by, risk_type, esi_applicability, legal_applicability, rca_required, damaged_cost, generation_loss, damaged_cost_curr_id, generation_loss_curr_id, title, description, is_insurance_applicable, insurance, insurance_status, insurance_remark, status, created_by, created_at, status_updated_at, location_of_incident, type_of_job, is_activities_trained, is_person_authorized, instructions_given, safety_equipments, safe_procedure_observed, unsafe_condition_contributed, unsafe_act_cause, incidet_type_id,esi_applicability_remark,legal_applicability_remark,rca_required_remark,is_person_involved" +
                     ")" +
                     "VALUES" +
                     "(" +
                         $"{request.facility_id}, {request.block_id}, {request.equipment_id}, {request.severity_id}, {request.risk_level}, '{request.incident_datetime.ToString("yyyy-MM-dd HH:mm:ss")}', {request.victim_id}, {request.action_taken_by}, '{request.action_taken_datetime.ToString("yyyy-MM-dd HH:mm:ss")}', {request.inverstigated_by}, {request.verified_by}, {request.risk_type}, {request.esi_applicability}, {request.legal_applicability}, {request.rca_required}, {request.damaged_cost}, {request.generation_loss}, {request.damaged_cost_curr_id}, {request.generation_loss_curr_id}, '{request.title}', '{request.description}', {request.is_insurance_applicable}, '{request.insurance}', {request.insurance_status}, '{request.insurance_remark}', {(int)CMMS.CMMS_Status.IR_CREATED_INITIAL}, {userId}, '{UtilsRepository.GetUTCTime()}', '{UtilsRepository.GetUTCTime()}', '{request.location_of_incident}', '{request.type_of_job}', '{request.is_activities_trained}'," +
                         $" '{request.is_person_authorized}', '{request.instructions_given}', '{request.safety_equipments}', '{request.safe_procedure_observed}', '{request.unsafe_condition_contributed}', '{request.unsafe_act_cause}', {request.incidet_type_id}, '{request.esi_applicability_remark}','{request.legal_applicability_remark}','{request.rca_required_remark}','{request.is_person_involved}' " +
                     ") ; SELECT LAST_INSERT_ID()";


            DataTable dt2 = await Context.FetchData(qryIncident).ConfigureAwait(false);
            int incident_id = Convert.ToInt32(dt2.Rows[0][0]);

            if(request.injured_person != null && request.injured_person.Count > 0)
            {
                try
                {
                    string injured_Query = "INSERT INTO injured_person\r\n(\r\n incidents_id, person_id, person_type, age, sex, designation, address, name_contractor,\r\n  body_part_and_nature_of_injury, work_experience_years, plant_equipment_involved, location_of_incident\r\n)";
                    foreach(var item in  request.injured_person)
                    {
                        injured_Query = injured_Query + $" select   {incident_id}, '{ item.person_id}', { item.person_type}, { item.age}, { item.sex}, '{ item.designation}',\r\n  '{ item.address}', '{ item.name_contractor}', '{ item.body_part_and_nature_of_injury}', { item.work_experience_years},\r\n  '{ item.plant_equipment_involved}', '{ item.location_of_incident}' UNION ALL ";
                    }
                    injured_Query = injured_Query.TrimEnd("UNION ALL ".ToCharArray());
                    var injured_Query_result = await Context.ExecuteNonQry<int>(injured_Query).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.why_why_analysis != null && request.why_why_analysis.Count > 0)
            {
                try
                {
                    string why_why_IQuery = "INSERT INTO why_why_analysis (incidents_id, why, cause) ";
                    foreach(var item in request.why_why_analysis)
                    {
                        why_why_IQuery = why_why_IQuery + $" select {incident_id}, '{item.why}', '{item.cause}' UNION ALL ";
                    }
                    why_why_IQuery = why_why_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var why_why_Query_result = await Context.ExecuteNonQry<int>(why_why_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.root_cause != null && request.root_cause.Count > 0)
            {
                try
                {
                    string root_IQuery = "INSERT INTO root_cause (incidents_id, cause) ";
                    foreach (var item in request.root_cause)
                    {
                        root_IQuery = root_IQuery + $" select {incident_id}, '{item.cause}' UNION ALL ";
                    }
                    root_IQuery = root_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var root_Query_result = await Context.ExecuteNonQry<int>(root_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.immediate_correction != null && request.immediate_correction.Count > 0)
            {
                try
                {
                    string immediate_IQuery = "INSERT INTO immediate_correction (incidents_id, details) ";
                    foreach (var item in request.immediate_correction)
                    {
                        immediate_IQuery = immediate_IQuery + $" select {incident_id}, '{item.details}' UNION ALL ";
                    }
                    immediate_IQuery = immediate_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var immediate_Query_result = await Context.ExecuteNonQry<int>(immediate_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }


            if (request.proposed_action_plan != null && request.proposed_action_plan.Count > 0)
            {
                try
                {
                    string proposed_action_plan_IQuery = "INSERT INTO proposed_action_plan (incidents_id, actions_as_per_plan, responsibility, target_date, remarks) ";
                    foreach (var item in request.proposed_action_plan)
                    {
                        proposed_action_plan_IQuery = proposed_action_plan_IQuery + $" select {incident_id}, '{item.actions_as_per_plan}', '{item.responsibility}', '{item.target_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.remarks}' UNION ALL ";
                    }
                    proposed_action_plan_IQuery = proposed_action_plan_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var Query_result = await Context.ExecuteNonQry<int>(proposed_action_plan_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.investigation_team != null && request.investigation_team.Count > 0)
            {
                try
                {
                    string investigation_IQuery = "INSERT INTO investigation_team (incidents_id, person_id, person_type, designation, investigation_date,person_name) ";
                    foreach (var item in request.investigation_team)
                    {
                        investigation_IQuery = investigation_IQuery + $" select {incident_id}, '{item.person_id}', {item.person_type},'{item.designation}' ,'{item.investigation_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.person_name}' UNION ALL ";
                    }
                    investigation_IQuery = investigation_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var Query_result = await Context.ExecuteNonQry<int>(investigation_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (incident_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, incident_id, 0, 0, "Incident Report Created", CMMS.CMMS_Status.IR_CREATED_INITIAL,userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(incident_id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_CREATED_INITIAL, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(incident_id, CMMS.RETRUNSTATUS.SUCCESS, "Created Incident Report");
            }
            else{

                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateIncidentInvestigationReport(CMCreateIncidentReport request, int userId)
        {

            CMDefaultResponse response = new CMDefaultResponse();

            //string qryIncident = "INSERT INTO incidents" +
            //                         "(" +
            //                                 "facility_id, block_id, equipment_id,severity, risk_level, incident_datetime,victim_id, action_taken_by, action_taken_datetime, inverstigated_by, verified_by, risk_type, esi_applicability, legal_applicability, rca_required, damaged_cost, generation_loss, damaged_cost_curr_id, generation_loss_curr_id, title, description, is_insurance_applicable,insurance, insurance_status, insurance_remark, status,created_by,created_at, status_updated_at" +
            //                          ")" +
            //                          "VALUES" +
            //                         "(" +
            //                                $"{ request.facility_id }, { request.block_id }, { request.equipment_id },{request.severity_id}, { request.risk_level }, '{(request.incident_datetime).ToString("yyyy-MM-dd HH:mm:ss")}',{request.victim_id}, {request.action_taken_by}, '{(request.action_taken_datetime).ToString("yyyy-MM-dd HH:mm:ss")}', {request.inverstigated_by}, {request.verified_by},{request.risk_type},{request.esi_applicability},{request.legal_applicability},{request.rca_required},{request.damaged_cost},{request.generation_loss},{request.damaged_cost_curr_id},{request.generation_loss_curr_id},'{request.title}','{request.description}',{request.is_insurance_applicable},'{request.insurance}',{request.insurance_status},'{request.insurance_remark}',{(int)CMMS.CMMS_Status.IR_CREATED}, {userId},'{UtilsRepository.GetUTCTime()}','{UtilsRepository.GetUTCTime()}' " +
            //                         ") ; SELECT LAST_INSERT_ID()";

            //string qryIncident = "INSERT INTO incidents" +
            //         "(" +
            //             "facility_id, block_id, equipment_id, severity, risk_level, incident_datetime, victim_id, action_taken_by, action_taken_datetime, inverstigated_by, verified_by, risk_type, esi_applicability, legal_applicability, rca_required, damaged_cost, generation_loss, damaged_cost_curr_id, generation_loss_curr_id, title, description, is_insurance_applicable, insurance, insurance_status, insurance_remark, status, created_by, created_at, status_updated_at, location_of_incident, type_of_job, is_activities_trained, is_person_authorized, instructions_given, safety_equipments, safe_procedure_observed, unsafe_condition_contributed, unsafe_act_cause, incidet_type_id,esi_applicability_remark,legal_applicability_remark,rca_required_remark" +
            //         ")" +
            //         "VALUES" +
            //         "(" +
            //             $"{request.facility_id}, {request.block_id}, {request.equipment_id}, {request.severity_id}, {request.risk_level}, '{request.incident_datetime.ToString("yyyy-MM-dd HH:mm:ss")}', {request.victim_id}, {request.action_taken_by}, '{request.action_taken_datetime.ToString("yyyy-MM-dd HH:mm:ss")}', {request.inverstigated_by}, {request.verified_by}, {request.risk_type}, {request.esi_applicability}, {request.legal_applicability}, {request.rca_required}, {request.damaged_cost}, {request.generation_loss}, {request.damaged_cost_curr_id}, {request.generation_loss_curr_id}, '{request.title}', '{request.description}', {request.is_insurance_applicable}, '{request.insurance}', {request.insurance_status}, '{request.insurance_remark}', {(int)CMMS.CMMS_Status.IR_CREATED_INITIAL}, {userId}, '{UtilsRepository.GetUTCTime()}', '{UtilsRepository.GetUTCTime()}', '{request.location_of_incident}', '{request.type_of_job}', '{request.is_activities_trained}'," +
            //             $" '{request.is_person_authorized}', '{request.instructions_given}', '{request.safety_equipments}', '{request.safe_procedure_observed}', '{request.unsafe_condition_contributed}', '{request.unsafe_act_cause}', {request.incidet_type_id}, '{request.esi_applicability_remark}','{request.legal_applicability_remark}','{request.rca_required_remark}' " +
            //         ") ; SELECT LAST_INSERT_ID()";

            string qryIncident = " update incidents set status='"+(int)CMMS.CMMS_Status.IR_CREATED_INVESTIGATION+"', status_updated_at='"+ UtilsRepository.GetUTCTime() + "' where id= "+request.id+"";
            DataTable dt2 = await Context.FetchData(qryIncident).ConfigureAwait(false);
            int incident_id = request.id;

            if (request.injured_person != null && request.injured_person.Count > 0)
            {
                try
                {
                    string injured_Query = "INSERT INTO injured_person\r\n(\r\n incidents_id, person_id, person_type, age, sex, designation, address, name_contractor,\r\n  body_part_and_nature_of_injury, work_experience_years, plant_equipment_involved, location_of_incident\r\n)";
                    foreach (var item in request.injured_person)
                    {
                        injured_Query = injured_Query + $" select   {incident_id}, '{item.person_id}', {item.person_type}, {item.age}, {item.sex}, '{item.designation}',\r\n  '{item.address}', '{item.name_contractor}', '{item.body_part_and_nature_of_injury}', {item.work_experience_years},\r\n  '{item.plant_equipment_involved}', '{item.location_of_incident}' UNION ALL ";
                    }
                    injured_Query = injured_Query.TrimEnd("UNION ALL ".ToCharArray());
                    var injured_Query_result = await Context.ExecuteNonQry<int>(injured_Query).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.why_why_analysis != null && request.why_why_analysis.Count > 0)
            {
                try
                {
                    string why_why_IQuery = "INSERT INTO why_why_analysis (incidents_id, why, cause) ";
                    foreach (var item in request.why_why_analysis)
                    {
                        why_why_IQuery = why_why_IQuery + $" select {incident_id}, '{item.why}', '{item.cause}' UNION ALL ";
                    }
                    why_why_IQuery = why_why_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var why_why_Query_result = await Context.ExecuteNonQry<int>(why_why_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.root_cause != null && request.root_cause.Count > 0)
            {
                try
                {
                    string root_IQuery = "INSERT INTO root_cause (incidents_id, cause) ";
                    foreach (var item in request.root_cause)
                    {
                        root_IQuery = root_IQuery + $" select {incident_id}, '{item.cause}' UNION ALL ";
                    }
                    root_IQuery = root_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var root_Query_result = await Context.ExecuteNonQry<int>(root_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.immediate_correction != null && request.immediate_correction.Count > 0)
            {
                try
                {
                    string immediate_IQuery = "INSERT INTO immediate_correction (incidents_id, details) ";
                    foreach (var item in request.immediate_correction)
                    {
                        immediate_IQuery = immediate_IQuery + $" select {incident_id}, '{item.details}' UNION ALL ";
                    }
                    immediate_IQuery = immediate_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var immediate_Query_result = await Context.ExecuteNonQry<int>(immediate_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }


            if (request.proposed_action_plan != null && request.proposed_action_plan.Count > 0)
            {
                try
                {
                    string proposed_action_plan_IQuery = "INSERT INTO proposed_action_plan (incidents_id, actions_as_per_plan, responsibility, target_date, remarks,hse_remark,id_Status) ";
                    foreach (var item in request.proposed_action_plan)
                    {
                        proposed_action_plan_IQuery = proposed_action_plan_IQuery + $" select {incident_id}, '{item.actions_as_per_plan}', '{item.responsibility}', '{item.target_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.remarks}','{item.hse_remark}',{item.id_Status} UNION ALL ";
                    }
                    proposed_action_plan_IQuery = proposed_action_plan_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var Query_result = await Context.ExecuteNonQry<int>(proposed_action_plan_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (request.investigation_team != null && request.investigation_team.Count > 0)
            {
                try
                {
                    string investigation_IQuery = "INSERT INTO investigation_team (incidents_id, person_id, person_type, designation, investigation_date,person_name) ";
                    foreach (var item in request.investigation_team)
                    {
                        investigation_IQuery = investigation_IQuery + $" select {incident_id}, '{item.person_id}', {item.person_type},'{item.designation}' ,'{item.investigation_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.person_name}' UNION ALL ";
                    }
                    investigation_IQuery = investigation_IQuery.TrimEnd("UNION ALL ".ToCharArray());
                    var Query_result = await Context.ExecuteNonQry<int>(investigation_IQuery).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
                }
            }

            if (incident_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, incident_id, 0, 0, "Incident Report Created", CMMS.CMMS_Status.IR_CREATED_INITIAL, userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(incident_id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_CREATED_INITIAL, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(incident_id, CMMS.RETRUNSTATUS.SUCCESS, "Created Incident Investigation Report");
            }
            else
            {

                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report creation failed");
            }

            return response;
        }
        internal async Task<CMViewIncidentReport> GetIncidentDetailsReport(int id,string facilitytimeZone)
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
                strRiskType += $"If(risk_type = '{kvp.Value}', '{kvp.Key}',";
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
                               $"incident.id as id,incident.title, incident.description,facilities.id as facility_id, facilities.name as facility_name, blockName.id as block_id, blockName.name as block_name, assets.id as equipment_id,  assets.name as equipment_name,{severityName} as severity,incident.risk_level as risk_level ,IF(risk_level = '1','high',IF(risk_level ='2','medium','Low')) as risk_level_name, incident.incident_datetime as incident_datetime, incident.created_at as reporting_datetime,incident.action_taken_datetime ,user.id as victim_id, user.firstName as victim_name , user1.id as action_taken_by,  CONCAT(user1.firstName, user1.lastName) as action_taken_by_name, user2.id as inverstigated_by ,  CONCAT(user2.firstName, user2.lastName) as inverstigated_by_name , user3.id as verified_by ,CONCAT(user3.firstName, user3.lastName) as verified_by_name, incident.risk_type as risk_type, " + strRiskType + ",esi_applicability as esi_applicability,IF(esi_applicability = '1', 'YES', 'NO') as esi_applicability_name,legal_applicability as legal_applicability, IF(legal_applicability = '1', 'YES', 'NO') as legal_applicability_name, rca_required , IF(rca_required = '1', 'YES', 'NO') as rca_required_name, incident.damaged_cost AS damaged_cost, incident.generation_loss as generation_loss,incident.damaged_cost_curr_id, incident.generation_loss_curr_id, job.id as job_id, job.title as job_name , job.description as description , IF(is_insurance_applicable = '1', 'YES', 'NO') as is_insurance_applicable_name, incident.insurance_status as insurance_status, incident.insurance as insurance_name, incident.insurance_remark as insurance_remark, user4.id as approved_by ,CONCAT(user4.firstName, user4.lastName) as approved_by_name, CONCAT(user5.firstName, user5.lastName) as created_by_name,created_by as created_by_id, CONCAT(user6.firstName, user6.lastName) as updated_by_name, incident.status as status, incident.approved_at as approved_at,incident.reject_reccomendations as reject_comment " +
                               " ,esi_applicability_remark\r\n,legal_applicability_remark\r\n,location_of_incident\r\n,type_of_job\r\n,is_activities_trained\r\n,is_person_authorized\r\n,instructions_given\r\n,safety_equipments\r\n,safe_procedure_observed\r\n,unsafe_condition_contributed\r\n,unsafe_act_cause,incident.cancel_remarks, incident.is_person_involved, incident.approved_remarks, incident.is_why_why_required,incident.is_investigation_required FROM incidents as incident " +
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

                //_IncidentReportList[0].LstHistory = _historyList;

                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_IncidentReportList[0].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INCIDENT_REPORT, _Status);
                _IncidentReportList[0].status_short = _shortStatus;

                CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_IncidentReportList[0].status);
                string _longStatus = getLongStatus(CMMS.CMMS_Modules.INCIDENT_REPORT, _Status_long, _IncidentReportList[0]);
                _IncidentReportList[0].status_long = _longStatus;
            }
            else
            {
                return null;
            }
            _IncidentReportList[0].Injured_person = await Getinjured_person(id);
            _IncidentReportList[0].why_why_analysis = await Getwhy_why_analysis(id);
            _IncidentReportList[0].root_cause = await Getroot_cause(id);
            _IncidentReportList[0].immediate_correction = await Getimmediate_correction(id);
            _IncidentReportList[0].proposed_action_plan = await Getproposed_action_plan(id);
            _IncidentReportList[0].investigation_team = await Getinvestigation_team(id);
            foreach (var list in _IncidentReportList)
            {

                if(list.action_taken_datetime!=null && list!=null)

                list.action_taken_datetime= await _utilsRepo.ConvertToUTCDTC(facilitytimeZone,list.action_taken_datetime);

                if (list.approved_at != null && list != null)

                    list.approved_at= await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.approved_at);

                if (list.incident_datetime != null && list != null)
                    list.incident_datetime= await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.incident_datetime);

                if (list.reporting_datetime != null && list != null)

                    list.reporting_datetime = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.reporting_datetime);

               
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

            updateQry += $" updated_by = {userId}, update_at = '{UtilsRepository.GetUTCTime()}', " +
                $" location_of_incident = '{request.location_of_incident}', type_of_job= '{request.type_of_job}', is_activities_trained='{request.is_activities_trained}', " +
                $" is_person_authorized = '{request.is_person_authorized}', instructions_given = '{request.instructions_given}', safety_equipments = '{request.safety_equipments}', safe_procedure_observed='{request.safe_procedure_observed}' , " +
                $" unsafe_condition_contributed = '{request.unsafe_condition_contributed}', unsafe_act_cause= '{request.unsafe_act_cause}', esi_applicability_remark='{request.esi_applicability_remark}', legal_applicability_remark='{request.legal_applicability_remark}', rca_required_remark = '{request.rca_required_remark}', is_person_involved = '{request.is_person_involved}' WHERE id = {request.id};";

            //string updateqry = $"Update incidents" +
            //                      $" set facility_id = { request.facility_id }, block_id={request.block_id}, equipment_id={ request.equipment_id }, risk_level= {request.risk_level}, incident_datetime='{request.incident_datetime }',victim_id={request.victim_id},action_taken_by= {request.action_taken_by},action_taken_datetime = '{ request.action_taken_datetime }', inverstigated_by= {request.inverstigated_by}, verified_by={request.verified_by}, risk_type={request.risk_type},esi_applicability={request.esi_applicability},legal_applicability={request.legal_applicability},rca_required={request.rca_required},damaged_cost={request.damaged_cost},generation_loss={request.generation_loss},job_id= {request.job_id},title='{request.job_id}',description='{request.description}',is_insurance_applicable={request.is_insurance_applicable},insurance='{request.insurance_id}',insurance_status={request.insurance_status},insurance_remark= '{request.insurance_remark}',updated_by={userId},update_at= '{UtilsRepository.GetUTCTime()}'" +
            //                    $" where id =   { request.id}";

           int updateId= await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            int incident_id = request.id;

            if (request.injured_person != null && request.injured_person.Count > 0)
            {
                try
                {
               

                    
                    foreach (var item in request.injured_person)
                    {
                        StringBuilder injured_Query = new StringBuilder();
                        injured_Query.Append("UPDATE injured_person SET ");
                        injured_Query.Append("incidents_id = '" + incident_id + "', ");
                        injured_Query.Append("person_id = '" + item.person_id + "', ");
                        injured_Query.Append("person_type = " + item.person_type + ", ");
                        injured_Query.Append("age = " + item.age + ", ");
                        injured_Query.Append("sex = '" + item.sex + "', ");
                        injured_Query.Append("designation = '" + item.designation + "', ");
                        injured_Query.Append("address = '" + item.address + "', ");
                        injured_Query.Append("name_contractor = '" + item.name_contractor + "', ");
                        injured_Query.Append("body_part_and_nature_of_injury = '" + item.body_part_and_nature_of_injury + "', ");
                        injured_Query.Append("work_experience_years = " + item.work_experience_years + ", ");
                        injured_Query.Append("plant_equipment_involved = '" + item.plant_equipment_involved + "', ");
                        injured_Query.Append("location_of_incident = '" + item.location_of_incident + "' ");
                        injured_Query.Append("WHERE id = " + item.injured_item_id);
                        var injured_Query_result = await Context.ExecuteNonQry<int>(injured_Query.ToString()).ConfigureAwait(false);
                    }

    
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report update failed");
                }
            }

            if (request.why_why_analysis != null && request.why_why_analysis.Count > 0)
            {
                try
                {

                    /*  foreach (var item in request.why_why_analysis)
                      {
                          StringBuilder why_why_UQuery = new StringBuilder();
                          why_why_UQuery.Append("UPDATE why_why_analysis ");
                          why_why_UQuery.Append("SET why = '" + item.why + "', ");
                          why_why_UQuery.Append("incidents_id = '" + incident_id + "', ");
                          why_why_UQuery.Append("cause = '" +item.cause + "' ");
                          why_why_UQuery.Append("WHERE id = " + item.why_item_id);
                          var why_why_Query_result = await Context.ExecuteNonQry<int>(why_why_UQuery.ToString()).ConfigureAwait(false);
                      }*/
                    foreach (var item in request.why_why_analysis)
                    {
                        StringBuilder why_why_UQuery = new StringBuilder();
                        why_why_UQuery.Append("UPDATE why_why_analysis ");
                        why_why_UQuery.Append("SET why = '" + item.why + "', ");
                        why_why_UQuery.Append("incidents_id = '" + incident_id + "', ");
                        why_why_UQuery.Append("cause = '" + item.cause + "' ");
                        why_why_UQuery.Append("WHERE id = " + item.why_item_id);
                        var why_why_Query_result = await Context.ExecuteNonQry<int>(why_why_UQuery.ToString()).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report update failed");
                }
            }

            if (request.root_cause != null && request.root_cause.Count > 0)
            {
                try
                {
                    

                    foreach (var item in request.root_cause)
                    {
                        StringBuilder root_UQuery = new StringBuilder();
                        root_UQuery.Append("UPDATE root_cause ");
                        root_UQuery.Append("SET cause = '"+item.cause+"' ");
                        root_UQuery.Append("WHERE id = " + item.root_item_id);
                        var root_Query_result = await Context.ExecuteNonQry<int>(root_UQuery.ToString()).ConfigureAwait(false);
                    }                    
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report update failed");
                }
            }

            if (request.immediate_correction != null && request.immediate_correction.Count > 0)
            {
                try
                {
                
                    foreach (var item in request.immediate_correction)
                    {
                        StringBuilder immediate_UQuery = new StringBuilder();
                        immediate_UQuery.Append("UPDATE immediate_correction ");
                        immediate_UQuery.Append("SET details = '"+item.details+"' ");
                        immediate_UQuery.Append("WHERE id = " + item.ic_item_id);
                        var immediate_Query_result = await Context.ExecuteNonQry<int>(immediate_UQuery.ToString()).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report update failed");
                }
            }


            if (request.proposed_action_plan != null && request.proposed_action_plan.Count > 0)
            {
                try
                {

                    foreach (var item in request.proposed_action_plan)
                    {
                        StringBuilder proposedActionPlanUQuery = new StringBuilder();
                        proposedActionPlanUQuery.Append("UPDATE proposed_action_plan ");
                        proposedActionPlanUQuery.Append("SET actions_as_per_plan = '"+item.actions_as_per_plan+"', ");
                        proposedActionPlanUQuery.Append("responsibility = '"+item.responsibility+"', ");
                        proposedActionPlanUQuery.Append("target_date = '"+item.target_date.Value.ToString("yyyy-MM-dd HH:mm:ss")+"', ");
                        proposedActionPlanUQuery.Append("remarks = '"+item.remarks+"', ");
                        proposedActionPlanUQuery.Append("hse_remark = '" + item.hse_remark + "', ");
                        proposedActionPlanUQuery.Append("id_Status = " + item.id_Status + " ");
                        proposedActionPlanUQuery.Append("WHERE id = " + item.proposed_item_id);
                        var proposedActionPlanQuery_result = await Context.ExecuteNonQry<int>(proposedActionPlanUQuery.ToString()).ConfigureAwait(false);

                    }
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report update failed");
                }
            }
            if (request.investigation_team != null && request.investigation_team.Count > 0)
            {
                try
                {
                 
                    foreach (var item in request.investigation_team)
                    {
                        StringBuilder investigation_UQuery = new StringBuilder();
                        investigation_UQuery.Append("UPDATE investigation_team ");
                        investigation_UQuery.Append("SET person_id = '" + item.person_id + "', ");
                        investigation_UQuery.Append("person_type = '" + item.person_type + "', ");
                        investigation_UQuery.Append("person_name = '" + item.person_name + "', ");
                        investigation_UQuery.Append("investigation_date = '" + item.investigation_date.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
                        investigation_UQuery.Append("designation = '" + item.designation + "' ");
                        investigation_UQuery.Append("WHERE id = " + item.investigation_item_id);
                        var investigation_UQuery_result = await Context.ExecuteNonQry<int>(investigation_UQuery.ToString()).ConfigureAwait(false);

                    }
                }
                catch (Exception ex)
                {
                    return response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Incident Report update failed");
                }
            }
      
            if (updateId > 0)
            {

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, "Incident Report Updated", CMMS.CMMS_Status.IR_UPDATED,userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_UPDATED, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Updated Incident Report Successfully");

            }
            else
            {

                 response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, " Incident Report Update Failed");
            }

            return response;
        }


        internal async Task<CMDefaultResponse> ApproveIncidentReport(CMApproveIncident request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here ask status only
            */
            CMDefaultResponse response = new CMDefaultResponse();

            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_APPROVED_INITIAL},status_updated_at='{UtilsRepository.GetUTCTime()}',is_approved = 1,approved_by={userId},approved_at= '{UtilsRepository.GetUTCTime()}',approved_remarks='{request.comment}',is_why_why_required={request.is_why_why_required},is_investigation_required={request.is_investigation_required}  where id = " + request.id;
            int approve_id= await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (approve_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, request.comment, CMMS.CMMS_Status.IR_APPROVED_INITIAL, userId);

               CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_APPROVED_INITIAL,new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, " Incident Report Approved");
            }
            else
            {
                 response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, " Incident Report approval failed");
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
            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_REJECTED_INITIAL}, status_updated_at = '{UtilsRepository.GetUTCTime()}',is_approved = 2,approved_by={userId},approved_at= '{UtilsRepository.GetUTCTime()}',  reject_reccomendations = '{request.comment}'  where id = { request.id}";
            int reject_id= await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse();

            if (reject_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, "Incident Report Rejected", CMMS.CMMS_Status.IR_REJECTED_INITIAL, userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_REJECTED_INITIAL, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Report Rejected");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Incident Report approval failed");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> ApproveCreateIR(CMApproveIncident request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here ask status only
            */
            CMDefaultResponse response = new CMDefaultResponse();

            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION},status_updated_at='{UtilsRepository.GetUTCTime()}',is_approved = 1,approved_by={userId},approved_at= '{UtilsRepository.GetUTCTime()}'  where id = " + request.id;
            int approve_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (approve_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, request.comment, CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION,userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, " Incident Report Approved Investigation.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, " Incident Report approval failed");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> RejectCreateIR(CMApproveIncident request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_REJECTED_INVESTIGATION}, status_updated_at = '{UtilsRepository.GetUTCTime()}',is_approved = 2,approved_by={userId},approved_at= '{UtilsRepository.GetUTCTime()}',  reject_reccomendations = '{request.comment}'  where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse();

            if (reject_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, request.comment, CMMS.CMMS_Status.IR_REJECTED_INVESTIGATION, userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_REJECTED_INVESTIGATION, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Report Rejected Investigation.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Incident Report reject investigation failed.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> CancelIR(CMApproveIncident request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_Cancel}, status_updated_at = '{UtilsRepository.GetUTCTime()}', cancel_remarks = '{request.comment}'  where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse();

            if (reject_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, request.comment, CMMS.CMMS_Status.IR_Cancel, userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_Cancel, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Report Cancelled.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Incident Report cancel failed.");
            }

            return response;
        }

        internal async Task<List<CMInjured_person>> Getinjured_person(int incidents_id)
        {
            string selectqry = " SELECT\r\n    i.id as injured_item_id,\r\n    incidents_id,\r\n    person_id,\r\n     case when i.person_type =1 then concat(u.firstName, ' ', u.lastname)\r\n when i.person_type =2 then b.name\r\n else person_name\r\n end as person_name ,\r\n    person_type, case when person_type = 1 then 'Employee' when person_type = 2 then 'Contractor'  when person_type = 3 then 'Other' else '' end as person_type_name,\r\n    age,\r\n    sex,\r\n    designation,\r\n    i.address,\r\n    name_contractor,\r\n    body_part_and_nature_of_injury,\r\n    work_experience_years,\r\n    plant_equipment_involved,\r\n    location_of_incident, g.name as gender_name from injured_person i \r\nleft join  gender g on g.id = i.sex\r\nleft join users u on u.id = i.person_id\r\nleft join business b on b.id = i.person_id" +
                " where incidents_id = " + incidents_id + ";";
            List<CMInjured_person> result = await Context.GetData<CMInjured_person>(selectqry).ConfigureAwait(false);
            return result;
        }

        internal async Task<List<CMWhy_why_analysis>> Getwhy_why_analysis(int incidents_id)
        {
            string selectqry = "select id as why_item_id, incidents_id, why,cause from why_why_analysis where incidents_id = " + incidents_id + ";";
            List<CMWhy_why_analysis> result = await Context.GetData<CMWhy_why_analysis>(selectqry).ConfigureAwait(false);
            return result;
        }

        internal async Task<List<CMRoot_cause>> Getroot_cause(int incidents_id)
        {
            string selectqry = "select id as root_item_id, incidents_id, cause from root_cause where incidents_id = " + incidents_id + ";";
            List<CMRoot_cause> result = await Context.GetData<CMRoot_cause>(selectqry).ConfigureAwait(false);
            return result;
        }
        internal async Task<List<CMImmediate_correction>> Getimmediate_correction(int incidents_id)
        {
            string selectqry = "select id as ic_item_id, incidents_id, details from immediate_correction where incidents_id = " + incidents_id + ";";
            List<CMImmediate_correction> result = await Context.GetData<CMImmediate_correction>(selectqry).ConfigureAwait(false);
            return result;
        }

        internal async Task<List<CMProposed_action_plan>> Getproposed_action_plan(int incidents_id)
        {
            string selectqry = "select id as proposed_item_id,incidents_id,actions_as_per_plan,responsibility, target_date,remarks,hse_remark,id_Status from proposed_action_plan where incidents_id = " + incidents_id + ";";
            List<CMProposed_action_plan> result = await Context.GetData<CMProposed_action_plan>(selectqry).ConfigureAwait(false);

            return result;
        }
        internal async Task<List<CMInvestigation_team>> Getinvestigation_team(int incidents_id)
        {
            string selectqry = "select i.id as investigation_item_id, incidents_id,case when person_type = 1 then 'Employee' when person_type = 2 then 'Contractor' \r\n when person_type = 3 then 'Other' else '' end as person_type_name, person_id, person_type, designation, investigation_date,\r\n case when i.person_type =1 then concat(u.firstName, ' ', u.lastname)\r\n when i.person_type =2 then b.name\r\n else person_name\r\n end as person_name \r\n from investigation_team i\r\nleft join users u on u.id = i.person_id\r\nleft join business b on b.id = i.person_id  where incidents_id = " + incidents_id + ";";
            List<CMInvestigation_team> result = await Context.GetData<CMInvestigation_team>(selectqry).ConfigureAwait(false);
            return result;
        }
        internal async Task<CMDefaultResponse> CloseIR(CMApproveIncident request, int userId)
        {

            string approveQuery = $"Update incidents set status = {(int)CMMS.CMMS_Status.IR_Close}, status_updated_at = '{UtilsRepository.GetUTCTime()}', close_remarks = '{request.comment}'  where id = {request.id}";
            int close_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse();

            if (close_id > 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INCIDENT_REPORT, request.id, 0, 0, request.comment, CMMS.CMMS_Status.IR_Close, userId);

                CMViewIncidentReport _IncidentReportDetails = await GetIncidentDetailsReport(request.id,"");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INCIDENT_REPORT, CMMS.CMMS_Status.IR_Close, new[] { userId }, _IncidentReportDetails);

                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Report Closed.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Incident Report close failed.");
            }

            return response;
        }

    }
}
