using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
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
    public class MISMasterRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;

        public const string MA_Actual = "MA_Actual";
        public const string MA_Contractual = "MA_Contractual";
        public const string Internal_Grid = "Internal_Grid";
        public const string External_Grid = "External_Grid";
        private Dictionary<CMMS.CMMS_Modules, int> module_dict = new Dictionary<CMMS.CMMS_Modules, int>()
        {
            { CMMS.CMMS_Modules.DASHBOARD, 1 },
            { CMMS.CMMS_Modules.JOB, 2 },
            { CMMS.CMMS_Modules.PTW, 3 },
            { CMMS.CMMS_Modules.JOBCARD, 4 },
            { CMMS.CMMS_Modules.CHECKLIST_NUMBER, 5 },
            { CMMS.CMMS_Modules.CHECKPOINTS, 6 },
            { CMMS.CMMS_Modules.CHECKLIST_MAPPING, 7 },
            { CMMS.CMMS_Modules.PM_SCHEDULE, 8 },
            { CMMS.CMMS_Modules.PM_SCEHDULE_VIEW, 9 },
            { CMMS.CMMS_Modules.PM_EXECUTION, 10 },
            { CMMS.CMMS_Modules.PM_SCHEDULE_REPORT, 11 },
            { CMMS.CMMS_Modules.PM_SUMMARY, 12 },
            { CMMS.CMMS_Modules.SM_MASTER, 13 },
            { CMMS.CMMS_Modules.SM_GO, 14 },
            { CMMS.CMMS_Modules.SM_MRS, 15 },
            { CMMS.CMMS_Modules.SM_MRS_RETURN, 16 },
            { CMMS.CMMS_Modules.SM_S2S, 17 },
            { CMMS.CMMS_Modules.AUDIT_PLAN, 18 },
            { CMMS.CMMS_Modules.AUDIT_SCHEDULE, 19 },
            { CMMS.CMMS_Modules.AUDIT_SCEHDULE_VIEW, 20 },
            { CMMS.CMMS_Modules.AUDIT_EXECUTION, 21 },
            { CMMS.CMMS_Modules.AUDIT_SUMMARY, 22 },
            { CMMS.CMMS_Modules.HOTO_PLAN, 23 },
            { CMMS.CMMS_Modules.HOTO_SCHEDULE, 24 },
            { CMMS.CMMS_Modules.HOTO_SCEHDULE_VIEW, 25 },
            { CMMS.CMMS_Modules.HOTO_EXECUTION, 26 },
            { CMMS.CMMS_Modules.HOTO_SUMMARY, 27 },
            { CMMS.CMMS_Modules.INVENTORY, 28 },
            { CMMS.CMMS_Modules.WARRANTY_CLAIM, 30 },
            { CMMS.CMMS_Modules.CALIBRATION, 31 },
           // { CMMS.CMMS_Modules.MODULE_CLEANING, 32 },
            { CMMS.CMMS_Modules.VEGETATION_PLAN, 33 },
            {CMMS.CMMS_Modules.STATUTORY,34 }
        };
        public MISMasterRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }

        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* COST TYPE *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* RISK TYPE *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISRiskType> GetRiskType(int risk_id)
        {
            string myQuery = $"SELECT id, risktype as name, description, status FROM ir_risktype WHERE id = " + risk_id;
            List<MISRiskType> _RiskType = await Context.GetData<MISRiskType>(myQuery).ConfigureAwait(false);
            //Add history
            return _RiskType[0];
        }
        internal async Task<List<MISRiskType>> GetRiskTypeList()
        {
            string myQuery = "SELECT id, risktype as name, description FROM ir_risktype WHERE status=1 ";
            List<MISRiskType> _risktype = await Context.GetData<MISRiskType>(myQuery).ConfigureAwait(false);
            return _risktype;
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.STATUTORY_CREATED:
                    retValue = "Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.STATUTORY_RENEWD:
                    retValue = "Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.STATUTORY_UPDATED:
                    retValue = "Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.STATUTORY_APPROVED:
                    retValue = "Approved";
                    break;
                case CMMS.CMMS_Status.STATUTORY_REJECTED:
                    retValue = "Closed";
                    break;
                default:
                    retValue = "Unknown Status";
                    break;
            }
            return retValue;

        }
        public static string Status(int statusID)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.STATUTORY_CREATED:
                    statusName = "Created ";
                    break;
                case CMMS.CMMS_Status.STATUTORY_DELETED:
                    statusName = "Deleted ";
                    break;
                case CMMS.CMMS_Status.STATUTORY_UPDATED:
                    statusName = "Updated ";
                    break;
                case CMMS.CMMS_Status.STATUTORY_APPROVED:
                    statusName = "Approved";
                    break;
                case CMMS.CMMS_Status.STATUTORY_REJECTED:
                    statusName = "Rejected";
                    break;
                case CMMS.CMMS_Status.STATUTORY_RENEWD:
                    statusName = "Created";
                    break;
                default:
                    statusName = "Unknown Status";
                    break;
            }
            return statusName;
        }
        public static string Statusof(int statusID)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.OBSERVATION_CREATED:
                    statusName = "Created ";
                    break;
                case CMMS.CMMS_Status.OBSERVATION_DELETED:
                    statusName = "Deleted ";
                    break;
                case CMMS.CMMS_Status.UPDATED:
                    statusName = "Updated ";
                    break;
                case CMMS.CMMS_Status.OBSERVATION_CLOSED:
                    statusName = "Waiting-for-Approval";
                    break;
                case CMMS.CMMS_Status.OBSERVATION_ASSIGNED:
                    statusName = "Assigned";
                    break;
                case CMMS.CMMS_Status.OBSERVATION_APPROVED:
                    statusName = "closed-Approved";
                    break;
                case CMMS.CMMS_Status.OBSERVATION_REJECTED:
                    statusName = "closed-Rejected";
                    break;
                default:
                    statusName = "Unknown Status";
                    break;
            }
            return statusName;
        }
        internal async Task<CMDefaultResponse> CreateRiskType(MISRiskType request, int userId)
        {
            string myQuery = $"INSERT INTO ir_risktype(risktype, description, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}','{request.description} ', 1, {userId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Risk Type Added");
        }
        internal async Task<CMDefaultResponse> UpdateRiskType(MISRiskType request, int userID)
        {
            string updateQry = "UPDATE ir_risktype SET ";
            if (request.name != null && request.name != "")
                updateQry += $"risktype = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Risk Type Updated");
        }
        internal async Task<CMDefaultResponse> DeleteRiskType(int id, int userId)
        {
            string deleteQry = $"UPDATE ir_risktype " +
                $" SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Risk Type Deleted");
        }
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* SOURCE OF OBSERVATION *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISSourceOfObservation> GetSourceOfObservation(int source_id)
        {
            string myQuery = $"SELECT id, name, description, status FROM mis_m_observationsheet WHERE id = " + source_id;
            List<MISSourceOfObservation> _Sourceofobs = await Context.GetData<MISSourceOfObservation>(myQuery).ConfigureAwait(false);
            //Add history
            return _Sourceofobs[0];
        }

        internal async Task<List<MISSourceOfObservation>> GetSourceOfObservationList()
        {
            string myQuery = $"SELECT id, name, description FROM mis_m_observationsheet WHERE status = 1 ";
            List<MISSourceOfObservation> _Sourceofobs = await Context.GetData<MISSourceOfObservation>(myQuery).ConfigureAwait(false);
            //Add history
            return _Sourceofobs;
        }
        internal async Task<CMDefaultResponse> AddSourceOfObservation(MISSourceOfObservation request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into mis_m_observationsheet ( name, description, status , addedBy ,addedAt) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}');" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Source CMObservation Added");
        }

        internal async Task<CMDefaultResponse> UpdateSourceOfObservation(MISSourceOfObservation request, int userID)
        {
            string updateQry = "UPDATE mis_m_observationsheet SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}' ";
            updateQry += $" WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Source CMObservation Updated");
        }
        internal async Task<CMDefaultResponse> DeleteSourceOfObservation(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_observation SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Source CMObservation Deleted");
        }

        internal async Task<CMDefaultResponse> CloseObservation_old(CMApproval request, int userId)
        {
            string deleteQry = "";
            if (request.type == (int)CMMS.OBSERVATION_TYPE.PM_EXECUTION)
            {
                deleteQry = $"UPDATE pm_execution SET Observation_Status = {(int)CMMS_Status.OBSERVATION_CLOSED}, preventive_action = '{request.comment}'WHERE id = {request.id};";
                await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                System.Text.StringBuilder sb = new System.Text.StringBuilder("Observation Updated");
                if (request.comment.Length > 0)
                {
                    sb.Append(": " + request.comment);
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_EXECUTION, request.id, 0, 0, sb.ToString(), CMMS.CMMS_Status.OBSERVATION_CLOSED, userId); ;
            }
            else
            {
                deleteQry = $"UPDATE observations SET status_code = {(int)CMMS_Status.OBSERVATION_CLOSED}, closed_by = '{userId}' , closed_at='{UtilsRepository.GetUTCTime()}' , updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";

                await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

                System.Text.StringBuilder sb = new System.Text.StringBuilder("Observation Updated");
                if (request.comment.Length > 0)
                {
                    sb.Append(": " + request.comment);
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, request.id, 0, 0, sb.ToString(), CMMS.CMMS_Status.OBSERVATION_CLOSED, userId); ;
            }
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Observation {request.id} closed");
        }

        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* TYPE OF OBSERVATION *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISTypeObservation> GetTypeOfObservation(int type_id)
        {
            string myQuery = $"SELECT  m.id, m.name, m.description, m.id as risk_type_id, r.risktype, m.status FROM  mis_m_typeofobservation m LEFT JOIN  ir_risktype r ON  m.id = r.id WHERE m.id = " + type_id;
            List<MISTypeObservation> _Typeofobs = await Context.GetData<MISTypeObservation>(myQuery).ConfigureAwait(false);
            //Add history
            return _Typeofobs[0];
        }

        internal async Task<List<MISTypeObservation>> GetTypeOfObservationList()
        {

            string myQuery = $" SELECT  m.id, m.name, m.description,  m.id  as risk_type_id, r.risktype FROM mis_m_typeofobservation m LEFT JOIN  ir_risktype r ON   m.id = r.id WHERE m.status = 1; ";
            List<MISTypeObservation> _Sourceofobs = await Context.GetData<MISTypeObservation>(myQuery).ConfigureAwait(false);
            //Add history
            return _Sourceofobs;
        }
        internal async Task<CMDefaultResponse> AddTypeOfObservation(MISTypeObservation request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into mis_m_typeofobservation ( name, description, status , addedBy ,addedAt, risk_type_id) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}', {request.risk_type_id});" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Type CMObservation Added");
        }

        internal async Task<CMDefaultResponse> UpdateTypeOfObservation(MISTypeObservation request, int userID)
        {
            string updateQry = "UPDATE mis_m_typeofobservation SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            if (request.risk_type_id != null)
                updateQry += $"risk_type_id = '{request.risk_type_id}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Type CMObservation Updated");
        }
        internal async Task<CMDefaultResponse> DeleteTypeOfObservation(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_m_typeofobservation SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Type CMObservation Deleted");
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* GRIEVANCE TYPE *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISGrievanceType> GetGrievanceType(int id)
        {
            string myQuery = $"SELECT id, name, description, status FROM mis_m_grievancetype WHERE id = " + id;
            List<MISGrievanceType> _grivType = await Context.GetData<MISGrievanceType>(myQuery).ConfigureAwait(false);
            //Add history
            return _grivType[0];
        }

        internal async Task<List<MISGrievanceType>> GetGrievanceTypeList()
        {
            string myQuery = $"SELECT id, name, description FROM mis_m_grievancetype WHERE status = 1 ";
            List<MISGrievanceType> _grivType = await Context.GetData<MISGrievanceType>(myQuery).ConfigureAwait(false);
            //Add history
            return _grivType;

        }
        internal async Task<CMDefaultResponse> AddGrievanceType(MISGrievanceType request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into mis_m_grievancetype ( name, description, status , addedBy ,addedAt) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}');" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Greivance Type Added");
        }

        internal async Task<CMDefaultResponse> UpdateGrievanceType(MISGrievanceType request, int userID)
        {
            string updateQry = "UPDATE mis_m_grievancetype SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Greivance Type Updated");
        }
        internal async Task<CMDefaultResponse> DeleteGrievanceType(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_m_grievancetype SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Greivance Type Deleted");
        }

        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* ResolutionLevel *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISResolutionLevel> GetResolutionLevel(int id)
        {
            string myQuery = $"SELECT id, name, description, status FROM mis_m_resolutionlevel WHERE id = " + id;
            List<MISResolutionLevel> _ResLevel = await Context.GetData<MISResolutionLevel>(myQuery).ConfigureAwait(false);
            //Add history
            return _ResLevel[0];
        }

        internal async Task<List<MISResolutionLevel>> GetResolutionLevelList()
        {
            string myQuery = $"SELECT id, name, description FROM mis_m_resolutionlevel WHERE status = 1 ";
            List<MISResolutionLevel> _ResLevel = await Context.GetData<MISResolutionLevel>(myQuery).ConfigureAwait(false);
            //Add history
            return _ResLevel;

        }
        internal async Task<CMDefaultResponse> AddResolutionLevel(MISResolutionLevel request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into mis_m_resolutionlevel ( name, description, status , addedBy ,addedAt) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}');" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "ResLevel Added");
        }

        internal async Task<CMDefaultResponse> UpdateResolutionLevel(MISResolutionLevel request, int userID)
        {
            string updateQry = "UPDATE mis_m_resolutionlevel SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "ResLevel Updated");
        }
        internal async Task<CMDefaultResponse> DeleteResolutionLevel(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_m_resolutionlevel SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "_ResLevel Deleted");
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* COST TYPE  *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISCostType> GetCostType(int cost_id)
        {
            string myQuery = $"SELECT id, name, description, status FROM mis_m_costtype WHERE id = " + cost_id;
            List<MISCostType> _Typeofobs = await Context.GetData<MISCostType>(myQuery).ConfigureAwait(false);
            //Add history
            return _Typeofobs[0];
        }

        internal async Task<List<MISCostType>> GetCostTypeList()
        {
            string myQuery = $"SELECT id, name, description FROM mis_m_costtype WHERE status = 1 ";
            List<MISCostType> _Sourceofobs = await Context.GetData<MISCostType>(myQuery).ConfigureAwait(false);
            //Add history
            return _Sourceofobs;
        }
        internal async Task<CMDefaultResponse> CreateCostType(MISCostType request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into mis_m_costtype ( name, description, status , addedBy ,addedAt) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}');" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Cost Type Added");
        }

        internal async Task<CMDefaultResponse> UpdateCostType(MISCostType request, int userID)
        {
            string updateQry = "UPDATE mis_m_costtype SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Cost Type Updated");
        }
        internal async Task<CMDefaultResponse> DeleteCostType(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_m_costtype SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Cost Type Deleted");
        }
        internal async Task<List<BODYPARTS>> GetBodyPartsList()
        {
            string myQuery = "SELECT * FROM  bodyparts ";
            List<BODYPARTS> Data = await Context.GetData<BODYPARTS>(myQuery).ConfigureAwait(false);
            return Data;
        }

        internal async Task<CMDefaultResponse> CreateBodyParts(BODYPARTS request, int UserId)
        {
            String myqry = $"INSERT INTO BODYPARTS(sequence_no,id,name,description,status, Createdby,Createdat) VALUES " +
                                $"('{request.sequence_no}','{request.id}','{request.name}','{request.description}',1,  {UserId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "RECORD Added");
        }
        internal async Task<CMDefaultResponse> UpdateBodyParts(BODYPARTS request, int UserId)
        {
            string updateQry = "UPDATE BODYPARTS SET ";
            updateQry += $"name = '{request.name}', ";
            if (request.sequence_no >= 0)
                updateQry += $"Sequence_no = {request.sequence_no}, ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedAt ='{UtilsRepository.GetUTCTime()}' , updatedBy = '{UserId}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.sequence_no, CMMS.RETRUNSTATUS.SUCCESS, "Record Updated");
        }
        internal async Task<CMDefaultResponse> DeleteBodyParts(int id, int Userid)
        {
            string delqry = $"Delete From BODYPARTS where id={id};";
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "status deleted");
        }

        internal async Task<List<Responsibility>> GetResponsibilityList()
        {
            string getqry = $"Select id,name,description from Responsibility";
            List<Responsibility> Data = await Context.GetData<Responsibility>(getqry).ConfigureAwait(false);
            return Data;
        }
        //Master Of watertype
        internal async Task<List<WaterDataType>> GetWaterType()
        {

            string getqry = $"Select * from mis_watertype where  status=1;";
            List<WaterDataType> Data = await Context.GetData<WaterDataType>(getqry).ConfigureAwait(false);
            return Data;
        }
        internal async Task<List<WaterDataType>> GetWaterTypebyId(int id)
        {

            string getqry = $"Select facility_id,name, description,createdAt, updatedAt from mis_watertype where id=" + (id) + " and status=1;";
            List<WaterDataType> Data = await Context.GetData<WaterDataType>(getqry).ConfigureAwait(false);
            return Data;
        }
        internal async Task<CMDefaultResponse> CreateWaterType(WaterDataType request, int UserID)
        {

            string myQuery = $"INSERT INTO mis_watertype(facility_id,name,description,status,CreatedAt,CreatedBy,show_opening) VALUES " +
            $"('{request.facility_id}','{request.name} ','{request.description}',1,'{UtilsRepository.GetUTCTime()}','{UserID}',{request.show_opening});" +
            $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "WaterType  Created");
        }
        internal async Task<CMDefaultResponse> UpdateWaterType(WaterDataType request, int UserID)
        {
            string updateQry = "UPDATE mis_watertype  SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = '{UserID}', show_opening = {request.show_opening} WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WaterType updated");
        }
        internal async Task<CMDefaultResponse> DeleteWaterType(int id, int UserID)
        {
            string delqry = "UPDATE mis_watertype  SET status = 0 where id=" + (id);
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "WaterType deleted");

        }
        internal async Task<Responsibility> GetResponsibilityID(int id, string facilitytimeZone)
        {
            String getqry = $"Select * from   Responsibility where id =" + (id);
            List<Responsibility> Body = await Context.GetData<Responsibility>(getqry).ConfigureAwait(false);
            foreach (var body in Body)
            {
                if (body != null && body.CreatedAt != null)
                {
                    body.CreatedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, body.CreatedAt);
                }

            }

            return Body[0];
        }

        internal async Task<List<WasteDataType>> GetWasteType()
        {
            string delqry = "SELECT id,facility_id,name,Type,description,isHazardous,show_opening,createdAt,UpdatedAt  FROM mis_wastetype where status=1  ;";
            List<WasteDataType> Data = await Context.GetData<WasteDataType>(delqry).ConfigureAwait(false);
            return Data;
        }

        //changes
        internal async Task<CMDefaultResponse> CreateWasteType(WasteDataType request, int userId)
        {
            string myQuery = $"INSERT INTO mis_wastetype (facility_id,name,Type,description,status,CreatedAt,CreatedBy, show_opening, isHazardous) VALUES " +
            $"('{request.facility_id}','{request.name} ',{request.Type},'{request.description}',1,'{UtilsRepository.GetUTCTime()}','{userId}', {request.show_opening}, {request.isHazardous});" +
            $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "WasteType  Created");
        }
        internal async Task<CMDefaultResponse> DeleteWasteType(int id, int userID)
        {
            string delqry = "update mis_wastetype  set status=0  where id =" + (id);
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "WasteType deleted");
        }
        internal async Task<CMDefaultResponse> UpdateWasteType(WasteDataType request, int userId)
        {
            string updateQry = "UPDATE mis_wastetype  SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            if (request.Type != 0)
                updateQry += $"Type = '{request.Type}', ";
            updateQry += $"updatedAt = '{UtilsRepository.GetUTCTime()}', show_opening = {request.show_opening},isHazardous= {request.isHazardous} WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WasteType updated");
        }
        internal async Task<List<WasteDataType>> GetWasteTypeByid(int Type)
        {
            string delqry = "SELECT id,Type,facility_id,name,description,createdAt,UpdatedAt FROM mis_wastetype WHERE status=1 Type = " + Type + ";";
            List<WasteDataType> Data = await Context.GetData<WasteDataType>(delqry).ConfigureAwait(false);
            return Data;
        }
        internal async Task<CMDefaultResponse> CreateResponsibility(Responsibility request, int UserID)
        {
            string myQuery = $"INSERT INTO Responsibility(name, description, status, CreatedAt, CreatedBy) VALUES " +
            $"('{request.Name}','{request.Description} ',' Created', '{UtilsRepository.GetUTCTime()}', '{UserID}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";

            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Responsibility Added");
        }

        internal async Task<CMDefaultResponse> UpdateResponsibility(Responsibility request, int UserID)
        {
            string updateQry = "UPDATE Responsibility SET ";
            if (request.Name != null && request.Name != "")
                updateQry += $"name = '{request.Name}', ";
            if (request.Description != null && request.Description != "")
                updateQry += $"description = '{request.Description}', ";
            updateQry += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = '{UserID}' WHERE id = {request.Id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "Responsibility updated");
        }
        internal async Task<CMDefaultResponse> DeleteResponsibility(int id)
        {
            string delqry = "Delete from Responsibility where id =" + (id);
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Responsibility deleted");

        }
        // Incident type CRUD 
        internal async Task<CMIncidentType> GetIncidentType(int id)
        {
            string myQuery = $"SELECT id, incidenttype,  status FROM incidenttype WHERE id = " + id + " and status=1;";
            List<CMIncidentType> result = await Context.GetData<CMIncidentType>(myQuery).ConfigureAwait(false);
            //Add history
            return result[0];
        }
        internal async Task<List<CMIncidentType>> GetIncidentTypeList(string facilitytimeZone)
        {
            // string myQuery = "SELECT * FROM incidenttype WHERE status=1 ";
            string myQuery = "SELECT id,incidenttype,status,addedAt,addedBy,UpdatedAt,UpdatedBy FROM incidenttype WHERE status = 1";
            List<CMIncidentType> result = await Context.GetData<CMIncidentType>(myQuery).ConfigureAwait(false);
            foreach (var Result in result)
            {
                if (Result != null && Result.addedAt != null)
                    Result.addedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, Result.addedAt);
                if (Result != null && Result.updatedAt != null)
                    Result.updatedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, Result.updatedAt);
            }
            return result;
        }



        internal async Task<CMDefaultResponse> CreateIncidentType(CMIncidentType request, int userId)
        {
            string myQuery = $"INSERT INTO incidenttype(incidenttype,status, addedBy, addedAt) VALUES " +
                                $"('{request.incidenttype}', 1, {userId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Type Added.");
        }
        internal async Task<CMDefaultResponse> UpdateIncidentType(CMIncidentType request, int userID)
        {
            string updateQry = "UPDATE incidenttype SET ";
            if (request.incidenttype != null && request.incidenttype != "")
                updateQry += $"incidenttype = '{request.incidenttype}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Type Updated.");
        }
        internal async Task<CMDefaultResponse> DeleteIncidentType(int id, int userId)
        {
            string deleteQry = $"UPDATE incidenttype " +
                $" SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Incident Type Deleted.");
        }

        internal async Task<CMGetMisWaterData> GetWaterDataById(int id, string facilitytimeZone)
        {
            CMGetMisWaterData item = new CMGetMisWaterData();
            string myQuery = "SELECT M.id, M.plantId as facilityID, f.name as facilityName , date, waterTypeId, M.description, debitQty, creditQty, concat(added.firstname,' ',added.lastname)  as addedBy, addedAt,concat(updated.firstname,' ',updated.lastname)  as updatedBy, M.updatedAt FROM mis_waterdata M left join users added on added.id = M.addedBy " +
                " left join users updated on updated.id = M.updatedBy left join facilities f on f.id = M.plantId  where M.isActive = 1 and M.id = " + id + ";";
            List<CMGetMisWaterData> result = await Context.GetData<CMGetMisWaterData>(myQuery).ConfigureAwait(false);
            if (result.Count == 0)
            {
                return item;
            }
            foreach (var results in result)
            {
                if (results != null && results.AddedAt != null)
                    results.AddedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.AddedAt);
                if (results != null && results.Date != null)
                    results.Date = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, results.Date);
                if (results != null && results.UpdatedAt != null)
                    results.UpdatedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.UpdatedAt);
            }
            return result[0];
        }

        internal async Task<List<CMGetMisWaterData>> GetWaterDataList(DateTime fromDate, DateTime toDate, string facilitytimeZone)
        {
            string myQuery = "SELECT M.id, M.plantId as facilityID,f.name as facilityName, date, waterTypeId, M.description, debitQty, creditQty, concat(added.firstname,' ',added.lastname)  as addedBy, addedAt,concat(updated.firstname,' ',updated.lastname)  as updatedBy, M.updatedAt FROM mis_waterdata M left join users added on added.id = M.addedBy " +
                " left join users updated on updated.id = M.updatedBy left join facilities f on f.id = M.plantId  where isActive = 1 and date between '" + fromDate.ToString("yyyy-MM-dd") + "' and '" + toDate.ToString("yyyy-MM-dd") + "' ;";

            List<CMGetMisWaterData> result = await Context.GetData<CMGetMisWaterData>(myQuery).ConfigureAwait(false);
            foreach (var results in result)
            {
                if (results != null && results.AddedAt != null)
                    results.AddedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.AddedAt);
                if (results != null && results.Date != null)
                    results.Date = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, results.Date);
                if (results != null && results.UpdatedAt != null)
                    results.UpdatedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.UpdatedAt);
            }
            return result;
        }
        internal async Task<CMDefaultResponse> CreateWaterData(CMMisWaterData request, int userId)
        {
            int consumeType = 0;
            if (request.CreditQty > 0 && request.DebitQty > 0)
            {
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Credit and debit can not happen same time.");
            }

            if (request.CreditQty > 0 && request.DebitQty == 0)
            {
                consumeType = (int)CMMS.MISConsumptionTypes.Procurement;
            }
            if (request.CreditQty == 0 && request.DebitQty > 0)
            {
                consumeType = (int)CMMS.MISConsumptionTypes.Consumption;
            }
            string myQuery = $"INSERT INTO mis_waterdata(plantId, date, waterTypeId, description, debitQty, creditQty, addedBy, addedAt,consumeTypeId) VALUES " +
                             $"({request.facilityID}, '{request.Date.ToString("yyyy-MM-dd")}', {request.WaterTypeId}, '{request.Description}', {request.DebitQty}, {request.CreditQty}, {userId}, '{UtilsRepository.GetUTCTime()}',{consumeType}); " +
                             $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Water Data Added.");
        }

        internal async Task<CMDefaultResponse> UpdateWaterData(CMMisWaterData request, int userId)
        {
            string updateQry = "UPDATE mis_waterdata SET ";
            if (request.facilityID != null) updateQry += $"plantId = {request.facilityID}, ";
            if (request.Date != null) updateQry += $"date = '{request.Date.ToString("yyyy-MM-dd")}', ";
            if (request.WaterTypeId != null) updateQry += $"waterTypeId = {request.WaterTypeId}, ";
            if (request.Description != null) updateQry += $"description = '{request.Description}', ";
            if (request.DebitQty != null) updateQry += $"debitQty = {request.DebitQty}, ";
            if (request.CreditQty != null) updateQry += $"creditQty = {request.CreditQty}, ";
            updateQry += $"updatedBy = {userId}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.Id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "Water Data Updated.");
        }

        internal async Task<CMDefaultResponse> DeleteWaterData(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_waterdata SET isActive = 0 , updatedBy = {userId}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Water Data Deleted.");
        }

        internal async Task<List<CMWaterDataReport>> GetWaterDataReport(DateTime fromDate, DateTime toDate)
        {
            //string myQuery = "SELECT M.id, M.plantId,f.name as PlantName, date, waterTypeId, M.description,  debitQty, creditQty, concat(added.firstname,' ',added.lastname)  as addedBy, addedAt,concat(updated.firstname,' ',updated.lastname)  as updatedBy, M.updatedAt FROM mis_waterdata M left join users added on added.id = M.addedBy " +
            //    " left join users updated on updated.id = M.updatedBy left join facilities f on f.id = M.plantId  where isActive = 1 and date between '" + fromDate.ToString("yyyy-MM-dd") + "' and '" + toDate.ToString("yyyy-MM-dd") + "' ;";
            string myQuery = $"SELECT M.id, M.plantId as facilityID, IFNULL(f.name,'') as facilityName, date, waterTypeId, M.description, " +
                $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM mis_waterdata as ST  LEFT JOIN facilities fcc ON fcc.id = ST.plantId   " +
                $"where   ST.waterTypeId = M.waterTypeId and date_format(ST.date, '%Y-%m-%d') <= '{fromDate.ToString("yyyy-MM-dd")}'  group by ST.waterTypeID),0) Opening," +
                $" IFNULL((select sum(si.creditQty) from mis_waterdata si where si.waterTypeId = M.waterTypeId and  date_format(M.date, '%Y-%m-%d')\n BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}' ),0) as inward,    " +
                $" IFNULL((select sum(so.debitQty) from mis_waterdata so where so.waterTypeId = M.waterTypeId and date_format(M.date, '%Y-%m-%d')  BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}' ),0) as outward " +
                $" FROM mis_waterdata M left join users added on added.id = M.addedBy   left join users updated on updated.id = M.updatedBy left join facilities f on f.id = M.plantId  " +
                $" where isActive = 1 and date between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}' group by M.waterTypeId;";
            List<CMWaterDataReport> result = await Context.GetData<CMWaterDataReport>(myQuery).ConfigureAwait(false);
            if (result.Count != 0)
            {
                foreach (var item in result)
                {

                    item.balance = item.opening + item.inward - item.outward;
                }
            }
            return result;
        }

        internal async Task<List<CMGetMisWasteData>> GetWasteDataList(int facility_id, DateTime fromDate, DateTime toDate, int Hazardous, string facilitytimeZone)
        {
            string myQuery = "SELECT M.id, M.facilityId as facilityID,f.name as facilityName, date, waterTypeId, M.description, debitQty, creditQty, \nconcat(added.firstname,' ',added.lastname)  as addedBy, M.Created_At as addedAt,M.isHazardous " +
                " FROM waste_data M " +
                " left join mis_wastetype wt on wt.id = waterTypeId " +
                " left join users added on added.id = M.Created_By " +
                " left join facilities f on f.id = M.facilityId " +
                " where isActive = 1 and M.isHazardous = " + Hazardous + " and DATE_FORMAT(Created_At,'%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "';";

            List<CMGetMisWasteData> ListResult = await Context.GetData<CMGetMisWasteData>(myQuery).ConfigureAwait(false);
            foreach (var results in ListResult)
            {
                if (results != null && results.AddedAt != null)
                    results.AddedAt = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.AddedAt);
                if (results != null && results.Date != null)
                    results.Date = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, results.Date);

            }
            return ListResult;
        }

        internal async Task<CMWasteData> GetWasteDataByID(int Id)
        {
            string SelectQ = "select * from Waste_data where isActive = 1 and id = " + Id + "";
            List<CMWasteData> ListResult = await Context.GetData<CMWasteData>(SelectQ).ConfigureAwait(false);
            return ListResult[0];
        }

        internal async Task<CMDefaultResponse> CreateWasteData(CMWasteData request, int UserID)
        {
            CMDefaultResponse response = null;
            int InsertedValue = 0;
            //string insertQuery = $"INSERT INTO waste_data (" +
            //                     $"Solid_Waste, E_Waste, Battery_Waste, Solar_Module_Waste, " +
            //                     $"Haz_Waste_Oil, Haz_Waste_Grease, Haz_Solid_Waste, " +
            //                     $"Haz_Waste_Oil_Barrel_Generated, Solid_Waste_Disposed, " +
            //                     $"E_Waste_Disposed, Battery_Waste_Disposed, Solar_Module_Waste_Disposed, " +
            //                     $"Haz_Waste_Oil_Disposed, Haz_Waste_Grease_Disposed, " +
            //                     $"Haz_Solid_Waste_Disposed, Haz_Waste_Oil_Barrel_Disposed, " +
            //                     $"Created_By, Created_At" +
            //                     $") VALUES (" +
            //                     $"{request.Solid_Waste}, {request.E_Waste}, {request.Battery_Waste}, {request.Solar_Module_Waste}, " +
            //                     $"{request.Haz_Waste_Oil}, {request.Haz_Waste_Grease}, {request.Haz_Solid_Waste}, " +
            //                     $"{request.Haz_Waste_Oil_Barrel_Generated}, {request.Solid_Waste_Disposed}, " +
            //                     $"{request.E_Waste_Disposed}, {request.Battery_Waste_Disposed}, {request.Solar_Module_Waste_Disposed}, " +
            //                     $"{request.Haz_Waste_Oil_Disposed}, {request.Haz_Waste_Grease_Disposed}, " +
            //                     $"{request.Haz_Solid_Waste_Disposed}, {request.Haz_Waste_Oil_Barrel_Disposed}, " +
            //                     $"{UserID}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
            //                     $"); SELECT LAST_INSERT_ID();";
            int consumeType = 0;
            if (request.CreditQty > 0 && request.DebitQty > 0)
            {
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Credit and debit can not happen same time.");
            }

            if (request.CreditQty > 0 && request.DebitQty == 0)
            {
                consumeType = (int)CMMS.MISConsumptionTypes.Procurement;
            }
            if (request.CreditQty == 0 && request.DebitQty > 0)
            {
                consumeType = (int)CMMS.MISConsumptionTypes.Consumption;
            }
            int isHazardous = 0;
            string SelectQ = "select ifnull(isHazardous,0) isHazardous from mis_wastetype where ID = '" + request.wasteTypeId + "'";
            DataTable dt = await Context.FetchData(SelectQ).ConfigureAwait(false);
            if (dt.Rows.Count > 0)
            {
                isHazardous = Convert.ToInt32(dt.Rows[0][0]);
            }



            string insertQuery = $"INSERT INTO waste_data(facilityId, date, wasteTypeId, description, debitQty, creditQty, Created_By, Created_At,consumeTypeId, isHazardous) VALUES " +
                 $"({request.facilityID}, '{request.Date.ToString("yyyy-MM-dd")}', {request.wasteTypeId}, '{request.Description}', {request.DebitQty}, {request.CreditQty}, {UserID}, '{UtilsRepository.GetUTCTime()}',{consumeType},{isHazardous}); " +
                 $"SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(insertQuery).ConfigureAwait(false);
            InsertedValue = Convert.ToInt32(dt2.Rows[0][0]);
            response = new CMDefaultResponse(InsertedValue, CMMS.RETRUNSTATUS.SUCCESS, "Waste Data saved successfully.");
            return response;
        }


        internal async Task<CMDefaultResponse> UpdateWasteData(CMWasteData request, int UserID)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from waste_data where ID = '" + request.Id + "'";
            List<CMWasteData> WasteDataList = await Context.GetData<CMWasteData>(SelectQ).ConfigureAwait(false);

            if (WasteDataList != null && WasteDataList.Count > 0)
            {
                string updateQuery = $"UPDATE waste_data " +
                                    $"SET " +
                                    $"facilityId = {request.facilityID}, " +
                                    $"date = '{request.Date.ToString("yyyy-MM-dd")}', " +
                                    $"wasteTypeId = {request.wasteTypeId}, " +
                                    $"description = '{request.Description}', " +
                                    $"debitQty = {request.DebitQty}, " +
                                    $"creditQty = {request.CreditQty}, " +
                                    $"Modified_By = '{UserID}', " +
                                    $"Modified_At = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                                    $"WHERE id = {request.Id}";
                var result = await Context.ExecuteNonQry<int>(updateQuery);
                response = new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "Waste Data with id " + request.Id + " updated successfully.");
            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Waste Data does not exists to update.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteWasteData(int Id, int UserID)
        {
            CMDefaultResponse response = null;
            string SelectQ = "select id from Waste_data where ID = '" + Id + "'";
            List<CMWasteData> WasteDataList = await Context.GetData<CMWasteData>(SelectQ).ConfigureAwait(false);

            if (WasteDataList != null && WasteDataList.Count > 0)
            {
                string updateQuery = $"UPDATE Waste_data " +
                      $"SET " +
                      $"isActive = 0, " +
                      $"Modified_By = '{UserID}', " +
                      $"Modified_At = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                      $"WHERE id = {Id}";
                var result = await Context.ExecuteNonQry<int>(updateQuery);
                response = new CMDefaultResponse(Id, CMMS.RETRUNSTATUS.SUCCESS, "Waste Data with id " + Id + " deleted.");
            }
            else
            {
                response = new CMDefaultResponse(Id, CMMS.RETRUNSTATUS.FAILURE, "Waste Data does not exists to delete.");
            }

            return response;
        }

        internal async Task<List<WaterDataResult>> GetWaterDataListMonthWise(DateTime fromDate, DateTime toDate, int facility_id)
        {
            //     string SelectQ_MasterIDS = $" select distinct mis_waterdata.waterTypeId id, plantId as facility_id,fc.name facility_name,MONTHNAME(date) as month_name,YEAR(date) as year, " +
            //$" (select sum(creditQty)-sum(debitQty) from mis_waterdata where MONTH(date) < MONTH('" + fromDate.ToString("yyyy-MM-dd") + "')) as opening," +
            //$" sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type,show_opening" +
            //$" from mis_waterdata" +
            //$" LEFT JOIN facilities fc ON fc.id = mis_waterdata.plantId" +
            //$" LEFT JOIN mis_watertype mw on mw.id = mis_waterdata.waterTypeId" +
            //$" where isActive = 1 and mis_waterdata.plantId = {facility_id} and DATE_FORMAT(Date,'%Y-%m-%d') BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}'" +
            //$"  group by MONTH(date) , mis_waterdata.waterTypeId;";

            //     List<WasteDataType> Master_ID_Result = await Context.GetData<WasteDataType>(SelectQ_MasterIDS).ConfigureAwait(false);

            //show_opening
            string SelectQ = $" select distinct mis_waterdata.id,plantId as facility_id,fc.name facility_name,MONTHNAME(date) as month_name,Month(date) as month_id,YEAR(date) as year, " +
                $" (select sum(creditQty)-sum(debitQty) from mis_waterdata where MONTH(date) < MONTH('" + fromDate.ToString("yyyy-MM-dd") + "')) as opening," +
                $"sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type,mw.show_opening" +
                $" from mis_waterdata" +
                $" LEFT JOIN facilities fc ON fc.id = mis_waterdata.plantId" +
                $" LEFT JOIN mis_watertype mw on mw.id = mis_waterdata.waterTypeId" +
                $" where  mis_waterdata.plantId = {facility_id} and DATE_FORMAT(Date,'%Y-%m-%d') BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}'" +
                $" group by MONTH(month_id) , mis_waterdata.waterTypeId  " +
                $"  ;";
            //SelectQ = SelectQ + $" union all" +
            //    $"    select distinct plantId as facility_id,fc.name facility_name,'' as month_name," +
            //    $"  0000 as year, 0 as opening, " +
            //    $"  0 as procured_qty, 0 as consumed_qty, mw.name as water_type,show_opening " +
            //    $"  from mis_waterdata " +
            //    $"  LEFT JOIN facilities fc ON fc.id = mis_waterdata.plantId " +
            //    $"  LEFT JOIN mis_watertype mw on  mw.id not in ({string.Join(",", Master_ID_Result.Select(w => w.id).Distinct())})";

            string water_type_master_q = " select facility_id,fc.name facility_name, mis_watertype.name as water_type,mis_watertype.show_opening " +
                " from mis_watertype  LEFT JOIN facilities fc ON fc.id = mis_watertype.facility_id;";

            List<CMWaterDataMonthWise> Result_masterList = await Context.GetData<CMWaterDataMonthWise>(water_type_master_q).ConfigureAwait(false);
            List<CMWaterDataMonthWise> ListResult = await Context.GetData<CMWaterDataMonthWise>(SelectQ).ConfigureAwait(false);

            List<WaterDataResult> groupedResult = ListResult.GroupBy(r => new { r.facility_id, r.facility_name })
               .Select(group => new WaterDataResult
               {
                   facility_id = group.Key.facility_id,
                   facility_name = group.Key.facility_name,
                   period = group.Select(r => new { r.month_name, r.month_id, r.year })
                                .Distinct()
                                .Select(periodGroup => new FacilityPeriodData
                                {
                                    month_name = periodGroup.month_name,
                                    // month_id = periodGroup.month_id,
                                    month_id = (int)periodGroup.month_id, // Explicit cast to int
                                    year = (int)periodGroup.year, // Explicit cast to int
                                                                  // year = periodGroup.year,
                                    details = group.Where(g => g.month_name == periodGroup.month_name && g.year == periodGroup.year)
                                                  //.GroupBy(g => g.water_type)
                                                  .Select(g => new CMWaterDataMonthWiseDetails
                                                  {
                                                      water_id = g.id,
                                                      water_type = g.water_type,
                                                      opening_qty = g.opening,
                                                      procured_qty = g.procured_qty,
                                                      consumed_qty = g.consumed_qty,
                                                      closing_qty = g.opening + g.procured_qty - g.consumed_qty,
                                                      show_opening = g.show_opening
                                                  }).ToList()

                                }).ToList()
               }).ToList();

            List<CMWaterTypeMaster> water_types = new List<CMWaterTypeMaster>();
            foreach (var item in Result_masterList)
            {
                for (int i = 0; i < groupedResult.Count; i++)
                {
                    if (groupedResult[i].facility_id == item.facility_id)
                    {
                        CMWaterTypeMaster element = new CMWaterTypeMaster();
                        element.water_type = item.water_type;
                        element.show_opening = item.show_opening;
                        if (groupedResult[i].master_list == null)
                        {
                            groupedResult[i].master_list = water_types;
                        }
                        groupedResult[i].master_list.Add(element);
                    }
                }

            }



            return groupedResult;
        }

        internal async Task<List<CMWasteDataResult>> GetWasteDataListMonthWise(DateTime fromDate, DateTime toDate, int Hazardous, int facility_id)
        {
            string SelectQ = $" select distinct waste_data.id, facilityId as facility_id,fc.name facility_name,MONTHNAME(date) as month_name,Month(date) as month_id,YEAR(date) as year, " +
                $" (select sum(creditQty)-sum(debitQty) from waste_data where MONTH(date) < MONTH('" + fromDate.ToString("yyyy-MM-dd") + "')) as opening," +
                $" sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type, show_opening " +
                $" from waste_data" +
                $" LEFT JOIN facilities fc ON fc.id = waste_data.facilityId" +
                $" LEFT JOIN mis_wastetype mw on mw.id = waste_data.wasteTypeId" +
                $" where DATE_FORMAT(Date,'%Y-%m-%d') BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}'and  mw.isHazardous={Hazardous} and mw.status=1 " +
                $" group by MONTH(month_id) , waste_data.wasteTypeId  " +
                $"  ;";

            List<CMWaterDataMonthWise> ListResult = await Context.GetData<CMWaterDataMonthWise>(SelectQ).ConfigureAwait(false);

            string water_type_master_q = " select facility_id,fc.name facility_name, mis_wastetype.name as water_type,show_opening " +
                                         $" from mis_wastetype  LEFT JOIN facilities fc ON fc.id = mis_wastetype.facility_id where mis_wastetype.isHazardous={Hazardous} and mis_wastetype.status=1 ;";

            List<CMWaterDataMonthWise> Result_masterList = await Context.GetData<CMWaterDataMonthWise>(water_type_master_q).ConfigureAwait(false);


            List<CMWasteDataResult> groupedResult = ListResult.GroupBy(r => new { r.facility_id, r.facility_name })
               .Select(group => new CMWasteDataResult
               {
                   facility_id = group.Key.facility_id,
                   facility_name = group.Key.facility_name,
                   hazardous = Hazardous,
                   period = group.Select(r => new { r.month_name, r.month_id, r.year })
                                .Distinct()
                                .Select(periodGroup => new CMFacilityPeriodData_Waste
                                {
                                    month_name = periodGroup.month_name,
                                    month_id = (int)periodGroup.month_id, // Explicit cast to int
                                    year = (int)periodGroup.year, // Explicit cast to int
                                                                  // month_id = periodGroup.month_id,
                                                                  //  year = periodGroup.year,
                                    details = group.Where(g => g.month_name == periodGroup.month_name && g.year == periodGroup.year)
                                                  //.GroupBy(g => g.water_type)
                                                  .Select(g => new CMWasteDataMonthWiseDetails
                                                  {
                                                      waste_id = g.id,
                                                      waste_type = g.water_type,
                                                      opening = g.opening,
                                                      procured_qty = g.procured_qty,
                                                      consumed_qty = g.consumed_qty,
                                                      closing_qty = g.opening + g.procured_qty - g.consumed_qty,
                                                      show_opening = g.show_opening
                                                  }).ToList()
                                }).ToList()
               }).ToList();
            List<CMWasteTypeMaster> waste_types = new List<CMWasteTypeMaster>();
            foreach (var item in Result_masterList)
            {
                for (int i = 0; i < groupedResult.Count; i++)
                {
                    if (groupedResult[i].facility_id == item.facility_id)
                    {
                        CMWasteTypeMaster element = new CMWasteTypeMaster();
                        element.waste_type = item.water_type;
                        element.show_opening = item.show_opening;
                        if (groupedResult[i].master_list == null)
                        {
                            groupedResult[i].master_list = waste_types;
                        }
                        groupedResult[i].master_list.Add(element);
                    }
                }

            }
            return groupedResult;
        }

        internal async Task<List<WaterDataResult_Month>> GetWaterDataMonthDetail(int Month, int Year, int facility_id)
        {
            string SelectQ = $" select distinct mis_waterdata.waterTypeId,mis_waterdata.id as id, plantId as facility_id,fc.name facility_name,DATE_FORMAT(Date,'%Y-%m-%d') as date ,MONTHNAME(date) as months,YEAR(date) as year,"
                             + $" (select sum(creditQty) - sum(debitQty) from mis_waterdata where MONTH(date) < {Month}) as opening,"
                             + $" sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type,"
                             + $" consumeTypeId as consumeTypeId,"
                             + $" case when consumeTypeId = 1 then 'Procurement' when consumeTypeId = 2 then 'Consumption' else 'NA' end as TransactionType,"
                             + $" mis_waterdata.description as Description"
                             + $" from mis_waterdata"
                             + $" LEFT JOIN facilities fc ON fc.id = mis_waterdata.plantId"
                             + $" LEFT JOIN mis_watertype mw on mw.id = mis_waterdata.waterTypeId"
                             + $" where isActive = 1 and mis_waterdata.plantId = {facility_id} and MONTH(date) = {Month} and Year(date) = {Year} group by MONTH(date), mis_waterdata.waterTypeId; ";
            List<CMWaterDataMonthDetail> ListResult = await Context.GetData<CMWaterDataMonthDetail>(SelectQ).ConfigureAwait(false);
            if (ListResult != null)
            {
                for (int i = 0; i < ListResult.Count; i++)
                {
                    ListResult[i].closing_qty = ListResult[i].opening + ListResult[i].procured_qty - ListResult[i].consumed_qty;
                }
            }


            List<WaterDataResult_Month> groupedResult = ListResult.GroupBy(r => new { r.facility_id, r.facility_name, r.months, r.year })
    .Select(group => new WaterDataResult_Month
    {
        facility_id = group.Key.facility_id,
        facility_name = group.Key.facility_name,
        month = group.Key.months,
        year = group.Key.year,
        item_data = group.Select(r => new { r.water_type, r.opening, r.waterTypeId })
                        .Distinct()
                        .Select(periodGroup => new FacilityPeriodData_Month
                        {
                            water_type = periodGroup.water_type,
                            waterTypeId = periodGroup.waterTypeId,
                            opening = periodGroup.opening,
                            details = group.Where(g => g.water_type == periodGroup.water_type)
                                          .Select(g => new CMWaterDataMonthWiseDetails_Month
                                          {
                                              id = g.id,
                                              date = Convert.ToString(g.date),
                                              procured_qty = g.procured_qty,
                                              consumed_qty = g.consumed_qty,
                                              Description = g.Description,
                                              TransactionType = g.TransactionType
                                          }).ToList()
                        }).ToList()
    }).ToList();

            for (int i = 0; i < groupedResult[0].item_data.Count; i++)
            {
                string detail_Q = " select id, DATE_FORMAT(date,'%Y-%m-%d') date,mis_waterdata.description,creditQty as procured_qty,debitQty as consumed_qty," +
                      "  case when consumeTypeId = 1 then 'Procurement' when consumeTypeId = 2 then 'Consumption' else 'NA' end as TransactionType" +
                      " from mis_waterdata where waterTypeId = " + groupedResult[0].item_data[i].waterTypeId + " and isActive = 1 and mis_waterdata.plantId =  " + facility_id + " and MONTH(date) = " + Month + " and Year(date) = " + Year + " ;";


                groupedResult[0].item_data[i].details = await Context.GetData<CMWaterDataMonthWiseDetails_Month>(detail_Q).ConfigureAwait(false);
            }

            return groupedResult;
        }
        internal async Task<List<CMWasteDataResult_Month>> GetWasteDataMonthDetail(int Month, int Year, int Hazardous, int facility_id)
        {
            /* string SelectQ = $" select distinct waste_data.waterTypeId ,waste_data.id as id,Year(waste_data.date) as year,MONTH(waste_data.date) as months , facilityId as facility_id,fc.name facility_name,date ,"
                              + $" (select sum(creditQty) - sum(debitQty) from waste_data where MONTH(date) < {Month}) as opening,"
                              + $" sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type,"
                              + $" consumeTypeId as consumeTypeId,"
                              + $" case when consumeTypeId = 1 then 'Procurement' when consumeTypeId = 2 then 'Consumption' else 'NA' end as TransactionType,"
                              + $" waste_data.description as Description"
                              + $" from waste_data"
                              + $" LEFT JOIN facilities fc ON fc.id = waste_data.facilityId"
                              + $" LEFT JOIN mis_wastetype mw on mw.id = waste_data.waterTypeId"
                              + $" where waste_data.isHazardous = {Hazardous} and waste_data.facilityId = {facility_id} and MONTH(date) = {Month} and Year(date) = {Year} group by MONTH(date), waste_data.waterTypeId; ";*/
            string SelectQ = $" SELECT DISTINCT waste_data.wasteTypeId ,waste_data.id AS id,YEAR(waste_data.date) AS year,MONTHNAME(waste_data.date) AS months , facilityId AS facility_id,fc.name AS facility_name,date ,"
                    + $" (SELECT SUM(creditQty) - SUM(debitQty) FROM waste_data WHERE MONTH(date) < {Month}) AS opening,"
                    + $" SUM(creditQty) AS procured_qty, SUM(debitQty) AS consumed_qty, mw.name AS waste_type,"
                    + $" consumeTypeId AS consumeTypeId,"
                    + $" CASE WHEN consumeTypeId = 1 THEN 'Procurement' WHEN consumeTypeId = 2 THEN 'Consumption' ELSE 'NA' END AS TransactionType,"
                    + $" waste_data.description AS Description,MONTHNAME(date) as month_name,Month(date) as month_id,show_opening\r\n"
                    + $" FROM waste_data"
                    + $" LEFT JOIN facilities fc ON fc.id = waste_data.facilityId"
                    + $" LEFT JOIN mis_wastetype mw ON mw.id = waste_data.wasteTypeId"
                    + $" WHERE waste_data.isHazardous = {Hazardous} AND waste_data.facilityId = {facility_id} AND MONTH(date) = {Month} AND YEAR(date) = {Year} GROUP BY MONTH(date), waste_data.wasteTypeId;";


            // start
            List<CMWasteDataMonthDetail> ListResult_new = await Context.GetData<CMWasteDataMonthDetail>(SelectQ).ConfigureAwait(false);
            if (ListResult_new != null)
            {
                for (int i = 0; i < ListResult_new.Count; i++)
                {
                    ListResult_new[i].closing_qty = ListResult_new[i].opening + ListResult_new[i].procured_qty - ListResult_new[i].consumed_qty;
                }
            }
            //end

            List<CMWasteDataResult_Month> groupedResult_new = ListResult_new.GroupBy(r => new { r.facility_id, r.facility_name, r.months, r.year })
                .Select(group => new CMWasteDataResult_Month
                {
                    facility_id = group.Key.facility_id,
                    facility_name = group.Key.facility_name,
                    month = group.Key.months,
                    year = group.Key.year,
                    item_data = group.Select(r => new { r.wasteTypeId, r.waste_type, r.opening })
                            .Distinct()
                            .Select(periodGroup => new CMWasteFacilityPeriodData_Month
                            {
                                waste_type = periodGroup.waste_type,
                                wasteTypeId = periodGroup.wasteTypeId,
                                opening = periodGroup.opening,
                                details = group.Where(g => g.waste_type == periodGroup.waste_type)
                                              .Select(g => new CMWasteDataMonthWiseDetails_Month
                                              {
                                                  id = g.id,
                                                  date = g.date,
                                                  procured_qty = g.procured_qty,
                                                  consumed_qty = g.consumed_qty,
                                                  Description = g.Description,
                                                  TransactionType = g.TransactionType,
                                                  show_opening = g.show_opening,
                                              }).ToList()
                            }).ToList()
                }).ToList();
            if (groupedResult_new.Count > 0)
            {
                for (int i = 0; i < groupedResult_new[0].item_data.Count; i++)
                {
                    string detail_Q = " select waste_data.id,date,waste_data.description,creditQty as procured_qty,debitQty as consumed_qty," +
                          "  case when consumeTypeId = 1 then 'Procurement' when consumeTypeId = 2 then 'Consumption' else 'NA' end as TransactionType, show_opening" +
                          " from waste_data LEFT JOIN mis_wastetype mw ON mw.id = waste_data.wasteTypeId where wasteTypeId = " + groupedResult_new[0].item_data[i].wasteTypeId + " and waste_data.isHazardous = " + Hazardous + " AND waste_data.facilityId = " + facility_id + " AND MONTH(date) = " + Month + " AND YEAR(date) = " + Year + " ;";


                    groupedResult_new[0].item_data[i].details = await Context.GetData<CMWasteDataMonthWiseDetails_Month>(detail_Q).ConfigureAwait(false);
                }
            }
            return groupedResult_new;
        }

        internal async Task<List<CMChecklistInspectionReport>> GetChecklistInspectionReport(string facility_id, int module_type, DateTime fromDate, DateTime toDate)
        {
            string facilityQuery = $"SELECT DISTINCT f.id as facility_id, f.name AS facility_name FROM st_audit st LEFT JOIN facilities f ON f.id = st.facility_id WHERE st.facility_id IN ({facility_id});";
            List<CMChecklistInspectionReport> facilities = await Context.GetData<CMChecklistInspectionReport>(facilityQuery).ConfigureAwait(false);


            string checklistQuery = "SELECT  f.id as facility_id, f.name AS facility_name,st.id, checklist_number AS checklist_name, '' AS SOP_number, " +
                                    "frequency.name AS frequency, CASE WHEN is_ok = 0 THEN 'No' WHEN is_ok = 1 THEN 'Yes' ELSE 'NA' END AS inspection_status, " +
                                    "PM_Schedule_Observation_add_date AS date_of_inspection, MONTHNAME(PM_Schedule_Observation_add_date) AS month, " +
                                    "MONTH(PM_Schedule_Observation_add_date) AS month_id, YEAR(PM_Schedule_Observation_add_date) AS year_id, " +
                                    "CASE WHEN file_required = 0 THEN 'No' ELSE 'Yes' END AS checklist_attachment " +
                                    "FROM st_audit st " +
                                    "LEFT JOIN pm_task task ON task.plan_id = st.id " +
                                    "LEFT JOIN pm_execution pm_execution ON pm_execution.task_id = task.id " +
                                    "LEFT JOIN checklist_number checklist_number ON checklist_number.id = st.Checklist_id " +
                                    "LEFT JOIN frequency frequency ON frequency.id = st.Frequency " +
                                    "LEFT JOIN facilities f ON f.id = st.facility_id " +
                                    "WHERE st.facility_id IN (" + facility_id + ") AND st.module_type_id = " + module_type + " " +
                                    "AND DATE_FORMAT(PM_Schedule_Observation_add_date, '%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "' " +
                                    "GROUP BY st.id ORDER BY st.id DESC;";
            List<Checklist1> checklistData = await Context.GetData<Checklist1>(checklistQuery).ConfigureAwait(false);

            var groupedChecklistData = checklistData
                .GroupBy(cd => new { cd.month, cd.month_id, cd.year_id })
                .Select(g => new Checklist1
                {
                    month = g.Key.month,
                    month_id = g.Key.month_id,
                    year_id = g.Key.year_id,
                    Details = g.Select(cd => new ChecklistDetails
                    {
                        checklist_name = cd.checklist_name,
                        SOP_number = cd.SOP_number,
                        frequency = cd.frequency,
                        inspection_status = cd.inspection_status,
                        date_of_inspection = cd.date_of_inspection,
                        checklist_attachment = cd.checklist_attachment,
                        no_of_unsafe_observation = 0
                    }).ToList()
                }).ToList();

            var response = facilities.Select(facility => new CMChecklistInspectionReport
            {
                facility_id = facility.facility_id,
                facility_name = facility.facility_name,
                checklist = groupedChecklistData
            }).ToList();

            return response;

        }
        internal async Task<List<CMObservationReport>> GetObservationSheetReport(string facility_id, DateTime fromDate, DateTime toDate)
        {
            string myQuery = " select  distinct monthname(PM_Schedule_Observation_add_date) as month_of_observation,st.id, " +
                " PM_Schedule_Observation_add_date as date_of_observation, " +
                " concat(contarctorname.firstName, ' ', contarctorname.lastName) contractor_name," +
                " '' location_of_observation, '' source_of_observation,''risk_type, Description observation_description," +
                " '' corrective_action, concat(assignTo.firstName, ' ', assignTo.lastName) responsible_person," +
                "  started_at as target_date, '' action_taken, started_at closer_date, '' cost_type,'Closed' status," +
                "  '' timeline " +
                " from st_audit st" +
                " left join pm_task task on task.plan_id = st.id " +
                " left join pm_execution pm_execution on pm_execution.task_id = task.id" +
                " left join checklist_number checklist_number on checklist_number.id = st.Checklist_id " +
                " left join frequency frequency on frequency.id = st.Frequency" +
                " left join users contarctorname on contarctorname.id = pm_execution.PM_Schedule_Observation_added_by" +
                " left join users assignTo on assignTo.id = task.assigned_to" +
                " where st.facility_id in ( " + facility_id + ") and is_ok = 0 and DATE_FORMAT(PM_Schedule_Observation_add_date,'%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "' " +
                "  group by st.id  order by st.id desc;";
            List<CMObservationReport> response = await Context.GetData<CMObservationReport>(myQuery).ConfigureAwait(false);

            return response;
        }

        internal async Task<List<CMObservationSummary>> GetObservationSummaryReport(string facility_id, string fromDate, string toDate)
        {
            int createdCount = 0;
            int openCount = 0;
            int closeCount = 0;
            bool isOpen = false;
            bool isClosed = false;
            //CMObservationSummary of months
            Dictionary<int, CMObservationSummary> monthlyObservationSummary = new Dictionary<int, CMObservationSummary>();

            //
            //remove columns whose data is not required\
            // What is the need of short_Status
            string myQuery = "SELECT id, facility_id, contractor_name, risk_type_id, cost_type , date_of_observation,MONTHNAME(date_of_observation) AS month_name, " +
                 "type_of_observation, location_of_observation, source_of_observation, target_date, created_at, " +
                 "is_active, status_code, short_Status FROM observations " +
                 "WHERE facility_id in ( " + facility_id + ") AND " +
                 "DATE_FORMAT(date_of_observation,'%Y-%m-%d') BETWEEN '" + fromDate + "' AND '" + toDate + "'";

            List<CMObservationReport> response = await Context.GetData<CMObservationReport>(myQuery).ConfigureAwait(false);

            if (response != null)
            {
                foreach (var item in response)
                {
                    isOpen = false;
                    isClosed = false;
                    // int month_of_observation = Convert.ToInt32(item.date_of_observation); //convert to int

                    DateTime d_o_o = (DateTime)item.date_of_observation;
                    string date_of_observation = d_o_o.ToString("yyyy-MM-dd");
                    string strMonth = date_of_observation.Substring(5, 2);
                    int month = int.Parse(strMonth);
                    string mn = item.month_name;

                    string strYear = date_of_observation.Substring(0, 4);
                    int year = int.Parse(strYear);
                    DateTime date_of_observation_Date = DateTime.ParseExact(date_of_observation, "yyyy-MM-dd", null);

                    CMObservationSummary forMonth;


                    if (!monthlyObservationSummary.TryGetValue(month, out forMonth))
                    {

                        forMonth = new CMObservationSummary(month, year, mn);
                        monthlyObservationSummary.Add(month, forMonth);
                    }

                    //if (item.status_code == CMMS.CMMS_Status.OBSERVATION_CLOSED)

                    forMonth.created++;
                    if (item.status_code == (int)CMMS.CMMS_Status.OBSERVATION_CLOSED)
                    {
                        forMonth.closed++;
                        isClosed = true;
                    }
                    else
                    {
                        forMonth.open++;
                        isOpen = true;

                    }

                    if (item.type_of_observation == 1)
                    {
                        forMonth.unsafe_act++;
                    }
                    if (item.type_of_observation == 2)
                    {
                        forMonth.unsafe_condition++;
                    }
                    if (item.type_of_observation == 3)
                    {
                        forMonth.statutory_non_compliance++;
                    }

                    if (item.risk_type_id == 1)
                    {
                        forMonth.createdCount_Critical++;

                        if (isOpen == true)
                        {
                            forMonth.openCount_Critical++;
                        }

                    }

                    if (item.risk_type_id == 2)
                    {
                        forMonth.createdCount_Significant++;

                        if (isOpen == true)
                        {
                            forMonth.openCount_Significant++;
                        }
                        if (isOpen == true)
                        {
                            forMonth.closeCount_Significant++;
                        }

                    }
                    if (item.risk_type_id == 3)
                    {
                        forMonth.createdCount_Moderate++;

                        if (isOpen == true)
                        {
                            forMonth.openCount_Moderate++;
                        }
                        if (isOpen == true)
                        {
                            forMonth.closeCount_Moderate++;
                        }

                    }
                    if (date_of_observation_Date > item.target_date)
                    {
                        forMonth.target_count++;
                    }
                }
            }
            return monthlyObservationSummary.Values.ToList();
        }
        internal async Task<CMStatutoryCompliance> GetStatutoryComplianceMasterById(int id)
        {
            string myQuery = $" SELECT s.id,s.name,s.isActive, concat(users.firstName, ' ', users.lastName) Created_by, s.created_At FROM statutorycomliance s\n  left join users on users.id = s.Created_by WHERE s.id = {id} and s.isActive=1";
            List<CMStatutoryCompliance> data = await Context.GetData<CMStatutoryCompliance>(myQuery).ConfigureAwait(false);
            return data.FirstOrDefault();
        }

        internal async Task<List<CMStatutoryCompliance>> GetStatutoryComplianceMasterList()
        {
            string myQuery = " SELECT s.id,s.name,s.isActive, concat(users.firstName, ' ', users.lastName) Created_by, s.created_At FROM statutorycomliance s\n  left join users on users.id = s.Created_by where s.isActive = 1 ";
            List<CMStatutoryCompliance> data = await Context.GetData<CMStatutoryCompliance>(myQuery).ConfigureAwait(false);
            return data;
        }
        internal async Task<CMDefaultResponse> CreateStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId)
        {
            string myQuery = $"INSERT INTO statutorycomliance(name, isActive, Created_by, Created_at) VALUES " +
                             $"('{request.Name}',1, {UserId}, '{UtilsRepository.GetUTCTime()}'); " +
                             $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, id, 0, 0, "CMStatutory Compliance Added.", CMMS.CMMS_Status.STATUTORY_CREATED, UserId); ;

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "CMStatutory Compliance Added.");
        }
        internal async Task<CMDefaultResponse> UpdateStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId)
        {
            string updateQry = "UPDATE statutorycomliance SET ";
            if (!string.IsNullOrEmpty(request.Name))
                updateQry += $"name = '{request.Name}' ";
            updateQry += $"WHERE id = {request.Id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, request.Id, 0, 0, "CMStatutory Compliance Updated.", CMMS.CMMS_Status.STATUTORY_COMPILANCE_UPDATED, UserId); ;
            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "CMStatutory Compliance Updated.");
        }

        internal async Task<CMDefaultResponse> DeleteStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId)
        {
            string updateQry = "UPDATE statutorycomliance SET ";
            updateQry += $"isActive = 0 ";
            updateQry += $"WHERE id = {request.Id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, request.Id, 0, 0, "CMStatutory Compliance Updated.", CMMS.CMMS_Status.STATUTORY_COMPILANCE_DELETED, UserId); ;
            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "CMStatutory Compliance Deleted.");
        }
        internal async Task<CMDefaultResponse> CreateStatutory(CMCreateStatutory request, int UserId)
        {
            CMDefaultResponse response = new CMDefaultResponse();
            string renew_from = Convert.ToString(request.renew_from);
            if (request.renewflag == 0)
            {
                string myQuery = $"INSERT INTO statutory (compliance_id, facility_id, issue_date, expires_on, renewFlag, status_of_application, status, created_by, created_at, updated_by, updated_at, approved_by, approved_at, renew_from, renew_by,comment) VALUES " +
                 $"({request.compliance_id}, {request.facility_id}, '{request.issue_date.ToString("yyyy-MM-dd HH:mm")}', '{request.expires_on.ToString("yyyy-MM-dd HH:mm")}', {request.renewflag}, {request.status_of_aplication_id}, {(int)CMMS.CMMS_Status.STATUTORY_CREATED}, {UserId}, '{UtilsRepository.GetUTCTime()}', {UserId}, '{UtilsRepository.GetUTCTime()}', {UserId}, '{UtilsRepository.GetUTCTime()}', '{renew_from}', {(request.renew_from_id == null ? 0 : request.renew_from_id)},'{request.Comment}' );" +
                 $"SELECT LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
                int id = Convert.ToInt32(dt.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, id, 0, 0, "Statutory Created.", CMMS.CMMS_Status.STATUTORY_CREATED, UserId); ;
                response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Statutory  Added");
            }
            if (request.renewflag == 1)
            {
                string myQuery = $"INSERT INTO statutory (compliance_id,facility_id,issue_date, expires_on,renewFlag,status_of_application ,status, created_by, created_at, updated_by, updated_at,approved_by, " +
                    $"approved_at,renew_from, renew_by,renew_id,comment) VALUES " +
                                 $"({request.compliance_id},{request.facility_id}, '{request.issue_date.ToString("yyyy-MM-dd HH:mm")}', '{request.expires_on.ToString("yyyy-MM-dd HH:mm")}' , " +
                                 $"{request.renewflag},{request.status_of_aplication_id}," +
                                 $" {(int)CMMS.CMMS_Status.STATUTORY_RENEWD}, {UserId}, '{UtilsRepository.GetUTCTime()}',{UserId},'{UtilsRepository.GetUTCTime()}', {UserId},'{UtilsRepository.GetUTCTime()}' ," +
                                 $" '{renew_from}',{UserId},{(request.renew_from_id == null ? 0 : request.renew_from_id)},'{request.Comment}' ); " +
                                 $"SELECT LAST_INSERT_ID();";
                DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
                int id = Convert.ToInt32(dt.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, id, 0, 0, "Statutory Renewed.", CMMS.CMMS_Status.STATUTORY_CREATED, UserId); ;
                response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Statutory Renewed.");

            }
            return response;
        }

        internal async Task<List<CMStatutory>> GetStatutoryList(int facility_id, string start_date, string end_date)
        {

            string myQuery = $"SELECT s.id,s.Compliance_id,st.name as compilanceName,s.comment as description,s.renewFlag, s.facility_id, " +
                             $"s.Issue_date AS start_date, s.expires_on AS end_date, s.status as status_id ,s.created_by, CONCAT(uc.firstName, ' ', uc.lastName) AS createdByName, CONCAT(u.firstName, ' ', u.lastName) AS UpdatedByName ," +
                             $" CONCAT(us.firstName, ' ', us.lastName) AS ApprovedByName, s.created_at, s.updated_by, s.updated_at, s.renew_from as renew_date, s.renew_by , s.approved_by, s.approved_at, " +
                             $" DAY(expires_on) AS status, " +
                             $" YEAR(expires_on) AS expiry_year,DATEDIFF(expires_on, now()) AS daysLeft,TIMESTAMPDIFF(MONTH, now(), expires_on) AS validity_month, " +
                             $" CASE WHEN expires_on< Now() THEN 'inactive'  ELSE 'active'    END AS Activation_status " +
                             $" FROM statutory AS s LEFT JOIN users uc ON  s.created_by = uc.id " +
                             $" LEFT JOIN users u on s.updated_by = u.id" +
                             $" LEFT JOIN users us on s.approved_by = us.id " +
                             $"LEFT JOIN statutorycomliance as st on st.id = s.Compliance_id " +
                             $" where (facility_id ={facility_id} or s.created_at='{start_date}' or s.created_at='{end_date}') AND DATEDIFF(expires_on, NOW()) >=0 or s.renewFlag !=0;";                          /* AND  daysLeft!= 0 AND s.renewFlag!= 0;";*/
            List<CMStatutory> data = await Context.GetData<CMStatutory>(myQuery).ConfigureAwait(false);

            foreach (var item in data)
            {

                item.validity_month = item.validity_month < 0 ? 0 : item.validity_month;
                item.daysLeft = item.daysLeft < 0 ? 0 : item.daysLeft;

                string _shortStatus = Status(item.status_id);
                item.Current_status = _shortStatus;


            }
            foreach (var task in data)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(task.status_id);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.STATUTORY, _Status);
                task.status_short = _shortStatus;
            }


            return data;
        }

        internal async Task<CMStatutory> GetStatutoryById(int id)
        {
            // string myQuery = $"SELECT  Compliance_id,facility_id, Issue_date as start_date, expires_on as end_date, status, created_by, created_at, updated_by, updated_at, renew_from, renew_from_id, approved_by, approved_at  FROM statutory WHERE id = {id}";
            string myQuery = $"SELECT s.id,s.Compliance_id,st.name as compilanceName, s.facility_id, " +
                          $"s.Issue_date AS start_date, s.expires_on AS end_date,s.comment as description, s.status as status_id ,s.created_by, CONCAT(uc.firstName, ' ', uc.lastName) AS createdByName, CONCAT(u.firstName, ' ', u.lastName) AS UpdatedByName ," +
                          $" CONCAT(us.firstName, ' ', us.lastName) AS ApprovedByName, s.created_at, s.updated_by, s.updated_at, s.renew_from, s.renew_by, s.approved_by, s.approved_at, " +
                          $" DAY(expires_on) AS status, " +
                          $" YEAR(expires_on) AS expiry_year,DATEDIFF(expires_on, now()) AS daysLeft,TIMESTAMPDIFF(MONTH, now(), expires_on) AS validity_month, " +
                          $" CASE WHEN expires_on< Now() THEN 'inactive'  ELSE 'active'    END AS Activation_status, " +
                          $" sa.name AS status_of_application FROM statutory AS s LEFT JOIN users uc ON  s.created_by = uc.id " +
                          $" LEFT JOIN users u on s.updated_by = u.id" +
                          $" LEFT JOIN users us on s.approved_by = us.id " +

                          $" LEFT JOIN  status_of_appllication sa ON sa.id = s.status_of_application " +
                          $" LEFT JOIN statutorycomliance as st on st.id = s.Compliance_id " +
                          $" where s.id ={id} ;";
            List<CMStatutory> data = await Context.GetData<CMStatutory>(myQuery).ConfigureAwait(false);
            foreach (var item in data)
            {
                item.validity_month = item.validity_month < 0 ? 0 : item.validity_month;
                item.daysLeft = item.daysLeft < 0 ? 0 : item.daysLeft;

                string _shortStatus = Status(item.status_id);
                item.Current_status = _shortStatus;
            }
            foreach (var task in data)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(task.status_id);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.STATUTORY, _Status);
                task.status_short = _shortStatus;
            }
            return data.FirstOrDefault();
        }

        internal async Task<CMDefaultResponse> UpdateStatutory(CMCreateStatutory request, int UserId)
        {

            string updateQry = "UPDATE statutory SET ";
            if (request.compliance_id.HasValue)
                updateQry += $"compliance_id = {request.compliance_id.Value}, ";
            updateQry += $"issue_date = '{request.issue_date.ToString("yyyy-MM-dd HH:mm")}', ";
            updateQry += $"expires_on = '{request.expires_on.ToString("yyyy-MM-dd HH:mm")}', ";
            updateQry += $"status_of_application = '{request.status_of_aplication_id}', ";
            updateQry += $"renew_from = '{request.renew_from}', ";
            updateQry += $"renew_by = '{UserId}', ";
            updateQry += $"renew_id = '{request.renew_from_id}', ";
            updateQry += $"comment = '{request.Comment}', ";

            updateQry += $"status = {(int)CMMS.CMMS_Status.STATUTORY_UPDATED}, ";
            updateQry += $"updated_by ={UserId} ,";
            updateQry += $"updated_at ='{UtilsRepository.GetUTCTime()}'";
            updateQry = updateQry.TrimEnd(',', ' ');
            updateQry += $" WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, request.id, 0, 0, "Statutory Updated.", CMMS.CMMS_Status.STATUTORY_UPDATED, UserId);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Record Updated");
        }

        internal async Task<CMDefaultResponse> DeleteStatutory(int id)
        {
            string deleteQry = $"DELETE FROM statutory WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, id, 0, 0, "CMStatutory Deleted.", CMMS.CMMS_Status.STATUTORY_DELETED, 0);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Statutory Deleted");
        }
        internal async Task<List<CMStatutoryHistory>> GetStatutoryHistoryById(int compliance_id)
        {
            string myQuery = $"SELECT distinct s.id,Compliance_id,sc.name as compliance_name ,facility_id,f.name as facility_name, Issue_date as start_date, " +
                $" expires_on as end_date, s.status,concat(created.firstName, ' ', created.lastName)  created_by, s.created_at," +
                $" concat(updated.firstName, ' ', updated.lastName)  updated_by , s.updated_at, renew_from, s.renew_by, " +
                $" concat(approved.firstName, ' ', approved.lastName)  approved_by , s.approved_at, " +
                $" concat(rejected.firstName, ' ', rejected.lastName)  rejected_by ,  s.rejected_at " +
                $" FROM statutory s" +
                $" left join statutorycomliance sc on sc.id = s.Compliance_id " +
                $" left join users created on  created.id = s.created_by" +
                $" left join users updated on  updated.id = s.updated_by" +
                $" left join users approved on  approved.id = s.approved_by" +
                $" left join users rejected on  rejected.id = s.rejected_by" +
                $" left join facilities as f on  f.id = s.facility_id" +

                $" WHERE Compliance_id = {compliance_id};";
            List<CMStatutoryHistory> data = await Context.GetData<CMStatutoryHistory>(myQuery).ConfigureAwait(false);
            foreach (var item in data)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(item.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.STATUTORY, _Status);
                item.status_short = _shortStatus;
            }
            return data;
        }
        internal async Task<CMDefaultResponse> ApproveStatutory(CMApprovals request, int userID)
        {
            string approveQuery = $"Update statutory set status={(int)CMMS.CMMS_Status.STATUTORY_APPROVED}, approved_at = '{UtilsRepository.GetUTCTime()}', approved_by={userID},comment='{request.comment}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, request.id, 0, 0, request.comment, CMMS.CMMS_Status.STATUTORY_APPROVED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;
        }
        internal async Task<CMDefaultResponse> RejectStatutory(CMApprovals request, int userId)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string approveQuery = $"Update statutory set status = {(int)CMMS.CMMS_Status.STATUTORY_REJECTED} , " +
                $"comment = '{request.comment}', " +
                $"rejected_by = {userId}, rejected_at = '{UtilsRepository.GetUTCTime()}' " +
                $" where id = {request.id} ";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.STATUTORY, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "Statutory Rejected " : request.comment, CMMS.CMMS_Status.STATUTORY_REJECTED);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Statutory Rejected Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> CreateStatusofAppliaction(MISTypeObservation request)
        {
            String myqry = $"INSERT INTO status_of_appllication(id,name,description) VALUES " +
                               $"({request.id},'{request.name}','{request.description}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "RECORD Added");


        }

        internal async Task<CMDefaultResponse> UpdateStatsofAppliaction(MISTypeObservation request)
        {
            string updateQry = "UPDATE status_of_appllication SET ";
            updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}' ";
            updateQry += $" WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Record Updated");

        }

        internal async Task<List<MISTypeObservation>> GetStatsofAppliaction()
        {
            string myQuery = "SELECT * FROM  status_of_appllication ";
            List<MISTypeObservation> Data = await Context.GetData<MISTypeObservation>(myQuery).ConfigureAwait(false);
            return Data;
        }
        internal async Task<CMDefaultResponse> DeleteStatsofAppliaction(int id)
        {
            string delqry = $"Delete From status_of_appllication where id={id};";
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "status deleted");
        }

        //Documen
        //Master of Document
        internal async Task<CMDefaultResponse> CreateDocument(MISTypeObservation request)
        {
            String myqry = $"INSERT INTO document(name,description,status) VALUES " +
                               $"('{request.name}','{request.description}',1); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Document Added");


        }
        internal async Task<List<MISTypeObservation>> GetDocument()
        {
            string myQuery = "SELECT * FROM  document ";
            List<MISTypeObservation> Data = await Context.GetData<MISTypeObservation>(myQuery).ConfigureAwait(false);
            return Data;
        }

        internal async Task<CMDefaultResponse> UpdateDocument(MISTypeObservation request)
        {
            string updateQry = "UPDATE document SET ";
            updateQry += $"name = '{request.name}', ";

            if (!string.IsNullOrEmpty(request.description))
            {
                updateQry += $"description = '{request.description}', ";
            }

            updateQry += $"status = 1 ";
            updateQry += $"WHERE id = {request.id};";


            updateQry = updateQry.TrimEnd(',', ' ');

            try
            {
                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Document Updated");
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to update Document", ex);
            }
        }
        internal async Task<CMDefaultResponse> DeleteDocument(int id)
        {
            string delqry = $"Delete From document where id={id};";
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Document deleted");
        }

        internal async Task<CMDefaultResponse> CreateObservation(CMObservation request, int UserID)
        {
            CMDefaultResponse response = null;
            int insertedValue = 0;

            // Validate type_of_observation and risk_type_id
            bool isValidTypeOfObservation = request.type_of_observation == (int)ObservationType.Unsafe_Act ||
                                            request.type_of_observation == (int)ObservationType.Unsafe_Condition ||
                                            request.type_of_observation == (int)ObservationType.Statutory_Non_Compilance;

            bool isValidRiskTypeId = request.risk_type_id == (int)RiskType.Major ||
                                     request.risk_type_id == (int)RiskType.Significant ||
                                     request.risk_type_id == (int)RiskType.Moderate;

            // Collect errors
            List<string> errors = new List<string>();

            if (!isValidTypeOfObservation)
            {
                errors.Add($"Type of Observation <{request.type_of_observation}> is invalid. " +
                           $"It should be {(int)ObservationType.Unsafe_Act} <{ObservationType.Unsafe_Act}>, " +
                           $"{(int)ObservationType.Unsafe_Condition} <{ObservationType.Unsafe_Condition}>, " +
                           $"or {(int)ObservationType.Statutory_Non_Compilance} <{ObservationType.Statutory_Non_Compilance}>.");
            }
            if (!isValidRiskTypeId)
            {
                errors.Add($"Risk Type ID <{request.risk_type_id}> is invalid. " +
                           $"It should be {(int)RiskType.Major} <{RiskType.Major}>, " +
                           $"{(int)RiskType.Significant} <{RiskType.Significant}>, " +
                           $"or {(int)RiskType.Moderate} <{RiskType.Moderate}>.");
            }
            if (errors.Count > 0)
            {
                m_errorLog.SetError(string.Join(" ", errors));
                m_errorLog.SetError(string.Join(" ", errors));
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, string.Join(" ", errors));
            }

            try
            {
                string insertQuery = $"INSERT INTO observations(" +
                                     $"facility_id, risk_type_id, preventive_action, responsible_person, contact_number, cost_type, " +
                                     $"date_of_observation, type_of_observation, location_of_observation, source_of_observation, target_date, " +
                                     $"observation_description, created_at, created_by, is_active,status_code,action_taken) VALUES " +
                                     $"({request.facility_id},{request.risk_type_id}, '{request.preventive_action}', {request.assigned_to_id}, '{request.contact_number}', " +
                                     $"{request.cost_type}, '{request.date_of_observation:yyyy-MM-dd HH:mm:ss}', {request.type_of_observation}, '{request.location_of_observation}', " +
                                     $"{request.source_of_observation}, '{request.target_date:yyyy-MM-dd HH:mm:ss}', '{request.observation_description}', " +
                                     $"'{UtilsRepository.GetUTCTime()}', {UserID}, 1,{(int)CMMS.CMMS_Status.OBSERVATION_CREATED},'{request.action_taken}'); " +
                                     $"SELECT LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(insertQuery).ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    insertedValue = Convert.ToInt32(dt.Rows[0][0]);
                }
                if (request.uploadfileIds.Count > 0)
                {
                    foreach (int data in request.uploadfileIds)
                    {
                        string uploadfilkr = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.OBSERVATION}, module_ref_id={insertedValue} where id={data} ;";
                        await Context.ExecuteNonQry<int>(uploadfilkr).ConfigureAwait(false);
                    }
                }
                // Create history log if there is a comment
                System.Text.StringBuilder sb = new System.Text.StringBuilder("Observation Created");
                if (request.comment != null)
                {
                    sb.Append(": " + request.comment);
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, insertedValue, 0, 0, sb.ToString(), CMMS.CMMS_Status.OBSERVATION_CREATED, UserID);
                response = new CMDefaultResponse(insertedValue, CMMS.RETRUNSTATUS.SUCCESS, "Observation data saved successfully.");
            }
            catch (Exception ex)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Observation data failed to save.");
            }

            return response;
        }


        internal async Task<CMDefaultResponse> UpdateObservation(CMObservation request, int UserID)
        {
            CMDefaultResponse response = null;
            List<string> errors = new List<string>();
            bool isValidCostType = request.cost_type == (int)CMMS.CostType.Capex || request.cost_type == (int)CostType.Opex || request.cost_type == (int)CostType.Undefined;
            if (!isValidCostType)
            {
                errors.Add($"Cost Type ID <{request.cost_type}> is invalid. " +
                           $"It should be {(int)CostType.Capex} <{CostType.Capex}>, " +
                           $"or {(int)CostType.Opex} <{CostType.Opex}>, ");
            }
            if (errors.Count > 0)
            {
                m_errorLog.SetError(string.Join(" ", errors));
                m_errorLog.SetError(string.Join(" ", errors));
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, string.Join(" ", errors));
            }
            try
            {
                string updateQuery = $"UPDATE observations SET " +
                                     $"contractor_name = '{request.operator_name}', " +
                                     $"risk_type_id = {request.risk_type_id}, " +
                                     $"preventive_action = '{request.preventive_action}', " +
                                     $"responsible_person = {request.assigned_to_id}, " +
                                     $"contact_number = '{request.contact_number}', " +
                                     $"cost_type = {request.cost_type}, " +
                                     $"date_of_observation = '{request.date_of_observation:yyyy-MM-dd HH:mm:ss}', " +
                                     $"type_of_observation = '{request.type_of_observation}', " +
                                     $"location_of_observation = '{request.location_of_observation}', " +
                                     $"action_taken = '{request.action_taken}', " +
                                     $"source_of_observation = '{request.source_of_observation}', " +
                                     $"comment = '{request.comment}', " +
                                     $"target_date = '{request.target_date:yyyy-MM-dd HH:mm:ss}', " +
                                     $"observation_description = '{request.observation_description}', " +
                                     $"status_code={(int)CMMS.CMMS_Status.OBSERVATION_CREATED}," +
                                     $"updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                     $"updated_by = {UserID} " +
                                     $"WHERE id = {request.id};";
                await Context.ExecuteNonQry<int>(updateQuery).ConfigureAwait(false);

                if (request.uploadfileIds.Count > 0)
                {
                    foreach (int data in request.uploadfileIds)
                    {
                        string uploadfilkr = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.OBSERVATION}, module_ref_id={request.id} where id={data} ;";
                        await Context.ExecuteNonQry<int>(uploadfilkr).ConfigureAwait(false);
                    }
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder("Observation Updated");
                if (request.comment != null)
                {
                    sb.Append(": " + request.comment);
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, request.id, 0, 0, sb.ToString(), CMMS.CMMS_Status.UPDATED, UserID);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Observation data updated successfully.");
            }
            catch (Exception ex)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to update observation data.");
            }
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteObservation(int id, int UserID, string comment)
        {
            CMDefaultResponse response = null;
            try
            {
                string updateQuery = $"UPDATE observations SET " +
                                      $"status_code={(int)CMMS.CMMS_Status.OBSERVATION_DELETED}," +
                                      $"deleted_at = '{UtilsRepository.GetUTCTime()}'," +
                                      $"deleted_by = {UserID}," +
                                      $"delete_comment = '{comment}'," +
                                     $"is_active = 0 " +
                                     $"WHERE id = {id};";
                await Context.ExecuteNonQry<int>(updateQuery).ConfigureAwait(false);
                System.Text.StringBuilder sb = new System.Text.StringBuilder("Observation Deleted");
                if (comment.Length > 0)
                {
                    sb.Append(": " + comment);
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, id, 0, 0, sb.ToString(), CMMS.CMMS_Status.OBSERVATION_DELETED, UserID);
                response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "CMObservation data deleted successfully.");
            }
            catch (Exception ex)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to delete observation data.");
            }
            return response;
        }

        internal async Task<List<CMObservation>> GetObservationList(int facility_Id, DateTime fromDate, DateTime toDate)
        {
            string myQuery = "select observations.id,observations.facility_id,facilities.name facility_name,status_code,observations.short_status, " +
                " bs.name as operator_name , risk_type_id,ir_risktype.risktype as risk_type, preventive_action, responsible_person,bs.contactNumber as contact_number, cost_type, CASE WHEN cost_type=1 THEN 'Capex' WHEN cost_type=2 THEN 'Opex' ELSE 'Empty' END as Cost_name , " +
                " date_of_observation, type_of_observation, location_of_observation, source_of_observation, " +
                " monthname(observations.date_of_observation) as month_of_observation,observations.target_date as closer_date,observations.closed_at as closed_date, concat(createdBy.firstName, ' ', createdBy.lastName) as action_taken, " +
                " observations.preventive_action as  corrective_action, DATEDIFF(observations.target_date, observations.date_of_observation) AS remaining_days," +
                " observations.target_date, observation_description, created_at, concat(createdBy.firstName, ' ', createdBy.lastName) created_by,concat(responsible.firstName, ' ', responsible.lastName) as  assigned_to_name,assign_to as assigned_to_id,updated_by as updateid, created_by as createdid, " +
                " updated_at, concat(updatedBy.firstName, ' ', updatedBy.lastName) updated_by,mis_m_typeofobservation.name as type_of_observation_name, mssheet.name  as source_of_observation_name, " +
                "CASE " +
                "WHEN observations.closed_at IS NOT NULL AND observations.closed_at <= observations.target_date THEN 'In Time' " +
                "WHEN observations.closed_at IS NOT NULL AND observations.closed_at > observations.target_date THEN 'Out of Target Date' " +
                "ELSE 'Open' " +
                "END AS observation_status " +
                " from observations" +
                " left join ir_risktype ON observations.risk_type_id = ir_risktype.id" +
                " left join mis_m_typeofobservation ON observations.type_of_observation  = mis_m_typeofobservation.id " +
                " left join facilities ON observations.facility_id = facilities.id " +
                " left join mis_m_observationsheet as mssheet ON observations.source_of_observation  =mssheet.id " +
                " left join users createdBy on createdBy.id = observations.created_by" +
                " left join users updatedBy on updatedBy.id = observations.updated_by" +
                " left join business bs on bs.id = facilities.operatorId " +
                " left join users responsible  on responsible.id = observations.assign_to" +
                " where is_active = 1 and observations.facility_id = " + facility_Id + " and date_format(created_at, '%Y-%m-%d') between '" + fromDate.ToString("yyyy-MM-dd") + "' and '" + toDate.ToString("yyyy-MM-dd") + "' ;";
            List<CMObservation> Result = await Context.GetData<CMObservation>(myQuery).ConfigureAwait(false);


            string pmexecutionquery = "select pm_execution.id,pm_task.facility_id,facilities.name facility_name,  Observation_Status as status_code,    " +
                      "bs.name as operator_name, ckp.risk_type as risk_type_id,ir_risktype.risktype as risk_type,  preventive_action, " +
                      "concat(responsible.firstName, ' ', responsible.lastName) as  assigned_to_name,Observation_assign_to as assigned_to_id, bs.contactNumber as contact_number,   " +
                      "ckp.cost_type,CASE WHEN cost_type=1 THEN 'Capex' WHEN cost_type=2 THEN 'Opex' ELSE 'Empty' END as Cost_name ,  " +
                      "ckp.type_of_observation,bs.location as location_of_observation, " +
                      "ckp.check_list_id as source_of_observation,  monthname(pm_execution.PM_Schedule_Observation_add_date) as month_of_observation, " +
                      "pm_execution.PM_Schedule_Observation_add_date as date_of_observation,pm_execution.closed_at as closed_date,  " +
                      "concat(createdBy.firstName, ' ', createdBy.lastName) as action_taken,pm_execution.preventive_action as corrective_action,  " +
                      "DATEDIFF(pm_execution.Observation_target_date,pm_execution.PM_Schedule_Observation_add_date) AS remaining_days,  " +
                      "pm_execution.Observation_target_date as target_date,ckp.requirement as observation_description,ckp.created_at as created_at, " +
                      "concat(createdBy.firstName, ' ', createdBy.lastName) created_by,  pm_task.updated_at, " +
                      "concat(updatedBy.firstName, ' ', updatedBy.lastName) updated_by,mis_m_typeofobservation.name as type_of_observation_name,   " +
                      "cls.checklist_number as source_of_observation_name,  CASE WHEN pm_execution.PM_Schedule_Observation_add_date IS NOT NULL AND pm_execution.PM_Schedule_Observation_add_date <= pm_execution.PM_Schedule_Observation_add_date THEN 'In Time'  WHEN pm_execution.PM_Schedule_Observation_add_date IS NOT NULL AND pm_execution.PM_Schedule_Observation_add_date > pm_execution.Observation_target_date THEN 'Out of Target Date'  ELSE 'Open' END AS observation_status " +
                      "from pm_execution  left join pm_task  ON pm_task.id = pm_execution.task_id " +
                      " left join facilities ON pm_task.facility_id = facilities.id" +
                      " left join checkpoint as ckp ON ckp.id = pm_execution.Check_Point_id" +
                      " left join checklist_number as cls ON ckp.check_list_id = cls.id " +
                      "left join ir_risktype ON ckp.risk_type = ir_risktype.id " +
                      "left join mis_m_typeofobservation ON ckp.type_of_observation = mis_m_typeofobservation.id " +
                      " left join business bs on bs.id = facilities.operatorId " +
                      " left join users responsible  on responsible.id = pm_execution.Observation_assign_to" +
                      " left join users createdBy on createdBy.id = ckp.created_by left join users updatedBy  on updatedBy.id = pm_task.updated_by " +
                      " left join business on business.id = createdBy.companyId and business.type = 2  " +
                      $" where  pm_task.facility_id ={facility_Id} and  ckp.type_id=4  and date_format(PM_Schedule_Observation_add_date, '%Y-%m-%d') between '" + fromDate.ToString("yyyy-MM-dd") + "' and '" + toDate.ToString("yyyy-MM-dd") + "' ;";

            List<CMObservation> Result1 = await Context.GetData<CMObservation>(pmexecutionquery).ConfigureAwait(false);
            foreach (var task in Result)
            {
                string _shortStatus = Statusof(task.status_code);
                task.short_status = _shortStatus;
                task.check_point_type_id = 1;
            }
            foreach (var task in Result1)
            {
                string _shortStatus = Statusof(task.status_code);
                task.short_status = _shortStatus;
                task.check_point_type_id = 2;
            }
            foreach (var task1 in Result1)
            {
                task1.check_point_type_id = 2;
            }
            Result.AddRange(Result1);

            return Result;
        }
        internal async Task<CMObservationDetails> GetObservationDetails(int observation_id, int check_point_type_id)
        {
            List<CMObservationDetails> Result = null;

            if (check_point_type_id == 1)
            {
                string myQuery = "select observations.id,observations.facility_id,facilities.name facility_name,status_code,observations.short_status, " +
                    " business.name as operator_name, risk_type_id,ir_risktype.risktype as risk_type, preventive_action, responsible_person, business.contactNumber as contact_number, cost_type ,CASE WHEN cost_type=1 THEN 'Capex' WHEN cost_type=2 THEN 'Opex' ELSE 'Empty' END as Cost_name ,  " +
                    " date_of_observation, type_of_observation, location_of_observation, source_of_observation,mis.name as source_of_observation_name,  " +
                    " monthname(observations.date_of_observation) as month_of_observation,observations.target_date as closer_date, concat(createdBy.firstName, ' ', createdBy.lastName) as action_taken, updated_by as updateid, created_by as createdid ," +
                    " observations.preventive_action as corrective_action, DATEDIFF(observations.target_date, observations.date_of_observation) AS remaining_days," +
                    " target_date, observation_description, created_at, concat(createdBy.firstName, ' ', createdBy.lastName) created_by,concat(createdBy.firstName, ' ', createdBy.lastName) responsible_person_name, " +
                    " updated_at, concat(updatedBy.firstName, ' ', updatedBy.lastName) updated_by,mis_m_typeofobservation.name as type_of_observation_name ,concat(responsible1.firstName, ' ', responsible1.lastName) as assigned_to_name,assign_to as assigned_to_id, " +
                    "CASE " +
                    "WHEN observations.closed_at IS NOT NULL AND observations.closed_at <= observations.target_date THEN 'In Time' " +
                    "WHEN observations.closed_at IS NOT NULL AND observations.closed_at > observations.target_date THEN 'Out of Target Date' " +
                    "ELSE 'Open' " +
                    "END AS observation_status " +
                    " from observations" +
                    " left join ir_risktype ON observations.risk_type_id = ir_risktype.id" +
                    " left join mis_m_typeofobservation ON observations.type_of_observation  = mis_m_typeofobservation.id" +
                    " left join facilities ON observations.facility_id = facilities.id" +
                    " left join users createdBy on createdBy.id = observations.created_by" +
                    " left join mis_m_observationsheet as mis on mis.id = observations.source_of_observation " +
                    " left join users responsible on responsible.id = observations.responsible_person" +
                    " left join users updatedBy on updatedBy.id = observations.updated_by" +
                    " left join users responsible1 on responsible1.id = observations.assign_to" +
                    " left join business on business.id = facilities.operatorId " +
                    " where is_active = 1 and observations.id = " + observation_id + ";";

                Result = await Context.GetData<CMObservationDetails>(myQuery).ConfigureAwait(false);
            }
            else if (check_point_type_id == 2)
            {
                string pmexecutionquery = "select pm_execution.id,pm_task.facility_id,facilities.name facility_name,  Observation_Status as status_code,    " +
                      "business.name as operator_name, ckp.risk_type as risk_type_id,ir_risktype.risktype as risk_type,  preventive_action, " +
                      "business.contactPerson as responsible_person, " +
                      "business.contactNumber as contact_number, " +
                      "ckp.created_by as createdid, " +
                      "ckp.cost_type,CASE WHEN cost_type=1 THEN 'Capex' WHEN cost_type=2 THEN 'Opex' ELSE 'Empty' END as Cost_name ,  " +
                      "PM_Schedule_Observation_add_date as date_of_observation," +
                      "ckp.type_of_observation,business.location as location_of_observation, " +
                      "ckp.check_list_id as source_of_observation, " +
                      "pm_execution.PM_Schedule_Observation_add_date as closer_date,pm_execution.PM_Schedule_Observation_update_date as closed_date,  " +
                      "concat(createdBy.firstName, ' ', createdBy.lastName) as action_taken,pm_execution.preventive_action as corrective_action,  " +
                      "DATEDIFF(pm_execution.Observation_target_date,pm_execution.PM_Schedule_Observation_add_date) AS remaining_days,  " +
                      "pm_execution.Observation_target_date as target_date,ckp.requirement as observation_description,ckp.created_at as created_at, " +
                      "concat(createdBy.firstName, ' ', createdBy.lastName) created_by,  pm_task.updated_at, " +
                      "concat(updatedBy.firstName, ' ', updatedBy.lastName) updated_by,mis_m_typeofobservation.name as type_of_observation_name,   " +
                      "cls.checklist_number as source_of_observation_name,  CASE WHEN pm_execution.PM_Schedule_Observation_add_date IS NOT NULL AND pm_execution.PM_Schedule_Observation_add_date <= pm_execution.PM_Schedule_Observation_add_date THEN 'In Time'  WHEN pm_execution.PM_Schedule_Observation_add_date IS NOT NULL AND pm_execution.PM_Schedule_Observation_add_date > pm_execution.Observation_target_date THEN 'Out of Target Date'  ELSE 'Open' END AS observation_status " +
                      "from pm_execution  " +
                      " left join pm_task  ON pm_task.id = pm_execution.task_id " +
                      " left join facilities ON pm_task.facility_id = facilities.id" +
                      " left join checkpoint as ckp ON ckp.id = pm_execution.Check_Point_id" +
                      " left join checklist_number as cls ON ckp.check_list_id = cls.id " +
                      "left join ir_risktype ON ckp.risk_type = ir_risktype.id " +
                      "left join mis_m_typeofobservation ON ckp.type_of_observation = mis_m_typeofobservation.id " +
                      " left join users createdBy on createdBy.id = ckp.created_by left join users updatedBy  on updatedBy.id = pm_task.updated_by " +
                      " left join business on business.id = facilities.operatorId " +
                       "where pm_execution.id = " + observation_id + "; ";

                Result = await Context.GetData<CMObservationDetails>(pmexecutionquery).ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException("Invalid check_point_type_id: Must be either 1 or 2.");
            }

            foreach (var task in Result)
            {
                string _shortStatus = Statusof(task.status_code);
                task.short_status = _shortStatus;
            }

            string myQuery4 = "SELECT U.id, file_path as fileName, FC.name as fileCategory, U.File_Size as fileSize, U.status,U.description, '' as ptwFiles FROM uploadedfiles AS U " +
                " LEFT JOIN observations as observations on observations.id = U.module_ref_id Left join filecategory FC on FC.Id = U.file_category " +
                " where observations.id = " + observation_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.OBSERVATION + ";";

            List<CMFileDetailObservation> _UploadFileList = await Context.GetData<CMFileDetailObservation>(myQuery4).ConfigureAwait(false);
            Result[0].FileDetails = _UploadFileList;

            return Result[0];
        }

        public async Task<GetChecklistInspection> GetChecklistInspection()
        {
            var _GetChecklistInspection = new GetChecklistInspection
            {
                FacilityId = 1,
                FacilityName = "Bellary",
                InspectionData = new List<InspectionData>
                {
                    new InspectionData
                    {
                        MonthName = "April",
                        MonthId = 4,
                        Year = 2024,
                        CheckList = new List<CheckList>
                        {
                            new CheckList
                            {
                                ChecklistName = "Monitoring Checklist of Electrical",
                                SopNumber = "HFE/HSE/SOP-10/C-1",
                                Frequency = "Monthly",
                                InspectionStatus = "Yes",
                                DateOfInspection = "2023-09-18",
                                ChecklistAttachment = "No",
                                NoOfUnsafeObservation = 2
                            },
                            new CheckList
                            {
                                ChecklistName = "Monitoring Checklist of Electrical2",
                                SopNumber = "HFE/HSE/SOP-10/C-2",
                                Frequency = "Monthly",
                                InspectionStatus = "Yes",
                                DateOfInspection = "2023-09-18",
                                ChecklistAttachment = "No",
                                NoOfUnsafeObservation = 2
                            },
                            new CheckList
                            {
                                ChecklistName = "Monitoring Checklist of Electrical3",
                                SopNumber = "HFE/HSE/SOP-10/C-3",
                                Frequency = "Monthly",
                                InspectionStatus = "Yes",
                                DateOfInspection = "2023-09-18",
                                ChecklistAttachment = "No",
                                NoOfUnsafeObservation = 2
                            }
                        }
                    },
                    new InspectionData
                    {
                        MonthName = "May",
                        MonthId = 5,
                        Year = 2024,
                        CheckList = new List<CheckList>
                        {
                            new CheckList
                            {
                                ChecklistName = "Vehicle fitness Checklist",
                                SopNumber = "HFE/HSE/SOP-11/C-1",
                                Frequency = "Monthly",
                                InspectionStatus = "No",
                                DateOfInspection = "2023-09-18",
                                ChecklistAttachment = "yes",
                                NoOfUnsafeObservation = 51
                            },
                            new CheckList
                            {
                                ChecklistName = "Monitoring Checklist of Electrical2",
                                SopNumber = "HFE/HSE/SOP-10/C-2",
                                Frequency = "Monthly",
                                InspectionStatus = "Yes",
                                DateOfInspection = "2023-09-18",
                                ChecklistAttachment = "No",
                                NoOfUnsafeObservation = 2
                            },
                            new CheckList
                            {
                                ChecklistName = "Monitoring Checklist of Electrical3",
                                SopNumber = "HFE/HSE/SOP-10/C-3",
                                Frequency = "Monthly",
                                InspectionStatus = "Yes",
                                DateOfInspection = "2023-09-18",
                                ChecklistAttachment = "No",
                                NoOfUnsafeObservation = 2
                            }
                        }
                    }
                }
            };

            // Simulate async work
            // await Task.Delay(100);

            return _GetChecklistInspection;
        }

        internal async Task<CMDefaultResponse> uploadDocument(CMDocumentVersion request, int user_id)
        {
            CMDefaultResponse response = null;
            if (request.is_renew == 0)
            {
                string stmt = "select auto_id from document_version where doc_master_id = " + request.doc_master_id + " and trim(lower(sub_doc_name)) = '" + request.sub_doc_name + "';";
                DataTable dt_chk = await Context.FetchData(stmt).ConfigureAwait(false);
                if (dt_chk.Rows.Count > 0)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Document Version Already Added With Sub Document : " + request.sub_doc_name + "");
                }

                string myqry = $"INSERT INTO document_version(facility_id, doc_master_id, file_id, sub_doc_name, renew_date, created_by, created_at, remarks) VALUES " +
                               $"({request.facility_id},{request.doc_master_id}, {request.file_id}, '{request.sub_doc_name}', " +
                               $"{(request.renew_date.HasValue ? $"'{request.renew_date.Value.ToString("yyyy-MM-dd HH:mm")}'" : "NULL")}, " +
                               $"{user_id}, '{UtilsRepository.GetUTCTime()}', '{request.Remarks}'); " +
                               $"SELECT LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
                int id = Convert.ToInt32(dt.Rows[0][0]);
                response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Document Version Added");
            }
            else
            {
                //string stmt_update = $"update document_version set renew_date = '{request.renew_date.Value.ToString("yyyy-MM-dd")}' where auto_id = {request.docuemnt_id};";
                //await Context.ExecuteNonQry<int>(stmt_update).ConfigureAwait(false);
                string myqry1 = $"INSERT INTO document_version(facility_id, doc_master_id, file_id, sub_doc_name, renew_date, created_by, created_at, remarks) VALUES " +
                               $"({request.facility_id},{request.doc_master_id}, {request.file_id}, '{request.sub_doc_name}', " +
                               $"{(request.renew_date.HasValue ? $"'{request.renew_date.Value.ToString("yyyy-MM-dd HH:mm")}'" : "NULL")}, " +
                               $"{user_id}, '{UtilsRepository.GetUTCTime()}', '{request.Remarks}'); " +
                               $"SELECT LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
                int id = Convert.ToInt32(dt.Rows[0][0]);

                response = new CMDefaultResponse(request.docuemnt_id, CMMS.RETRUNSTATUS.SUCCESS, "Document Version Updated");

            }
            return response;
        }

        internal async Task<List<CMDocumentVersionList>> getDocuementList(int facility_id, string fromDate, string toDate)
        {
            string myQuery = "SELECT auto_id id, d.facility_id, f.name as facility_name, doc_master_id, " +
                              "dd.name doc_master_name, file_id,sub_doc_name, renew_date," +
                             " concat(u.firstName, ' ', u.lastName) created_by, d.created_at, remarks ," +
                             " CASE WHEN d.renew_date < Now() THEN '0'  ELSE '1'    END AS Activation_status, up.file_path,up.description " +
                             " FROM  document_version d  " +
                             " left join users u on u.id = d.created_by " +
                             " left join document dd on dd.id = d.doc_master_id " +
                             " left join uploadedfiles as up on up.id=d.file_id and module_type=0    " +
                             " left join facilities f on f.id = d.facility_id" +
                             $" where d.facility_id={facility_id} and  DATE(d.created_at)>='{fromDate}'  and DATE(d.created_at) <='{toDate}' ;";
            List<CMDocumentVersionList> Data = await Context.GetData<CMDocumentVersionList>(myQuery).ConfigureAwait(false);
            foreach (var item in Data)
            {
                int inactiv = Convert.ToInt32(item.Activation_status);
                item.Activation_status = inactiv;
            }

            return Data;
        }
        internal async Task<List<CMDocumentVersionList>> getDocuementListById(int id, string sub_doc_name, string fromDate, string toDate)
        {
            string myQuery = "SELECT auto_id id, doc_master_id,d.facility_id,f.name as facility_name,dd.name doc_master_name, up.file_path,up.description , file_id,sub_doc_name, renew_date, concat(u.firstName, ' ', u.lastName) created_by, d.created_at, remarks " +
                             " FROM  document_version d " +
                             " left join users u on u.id = d.created_by " +
                             " left join document dd on dd.id = d.doc_master_id" +
                             " left join facilities f on f.id = d.facility_id" +
                             " left join uploadedfiles as up on up.id=d.file_id and module_type=0    " +
                            $" where doc_master_id={id} and sub_doc_name='{sub_doc_name}' and " +
                            $"DATE(d.created_at)>='{fromDate}'  and DATE(d.created_at) <='{toDate}'";
            List<CMDocumentVersionList> Data = await Context.GetData<CMDocumentVersionList>(myQuery).ConfigureAwait(false);
            return Data;
        }
        Dictionary<int, string> MonthDictionary = new Dictionary<int, string>
        {
             {1, "January"},
             {2, "February"},
             {3, "March"},
             {4, "April"},
             {5, "May"},
             {6, "June"},
             {7, "July"},
             {8, "August"},
             {9, "September"},
             {10, "October"},
             {11, "November"},
             {12, "December"}
        };
        //Chages For Mis Health Data

        //Plantation Data
        internal async Task<CMDefaultResponse> CreateHealthData(OccupationalHealthData request, int userID)
        {

            string myqry1 = $"INSERT INTO MIS_OccupationalHealthData " +
                      $"(month_id,facility_id,year, NoOfHealthExamsOfNewJoiner, PeriodicTests, OccupationaIllnesses, Status, CreatedBy, CreatedAt) " +
                      $"VALUES " +
                      $"({request.month_id},{request.facility_id},{request.year}, {request.NoOfHealthExamsOfNewJoiner}, {request.PeriodicTests}, {request.OccupationalIllnesses},1 , " +
                      $"{userID}, '{UtilsRepository.GetUTCTime()}'); " +
                      $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            var response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Health Data Created");

            return response;
        }
        internal async Task<CMDefaultResponse> UpdateHealthData(OccupationalHealthData request, int userID)
        {

            string updateQry = "UPDATE MIS_OccupationalHealthData SET ";

            if (request.month_id != 0)
            {
                updateQry += $"month_id = {request.month_id}, ";
            }
            if (request.facility_id != 0)
            {
                updateQry += $"facility_id = {request.facility_id}, ";
            }
            if (request.year != 0)
            {
                updateQry += $"year = {request.year}, ";
            }
            if (request.NoOfHealthExamsOfNewJoiner >= 0)
            {
                updateQry += $"NoOfHealthExamsOfNewJoiner = {request.NoOfHealthExamsOfNewJoiner}, ";
            }
            if (request.PeriodicTests > 0)
            {
                updateQry += $"PeriodicTests = {request.PeriodicTests} , ";
            }
            if (request.OccupationalIllnesses > 0)
            {
                updateQry += $"OccupationaIllnesses = {request.OccupationalIllnesses}, ";
            }
            updateQry += $"UpdatedBy = {userID}," +
            $" UpdatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Health Data Updated");

        }

        internal async Task<CMDefaultResponse> DeleteHealthData(int id, int userID)
        {
            string deleteQry = $"DELETE FROM MIS_OccupationalHealthData WHERE id = {id};";

            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Health Data Deleted");
        }

        internal async Task<List<OccupationalHealthData>> GetHealthData()
        {

            string myQuery = "SELECT mis_o.id, mis_o.facility_id, mis_o.year, mis_o.month_id, mis_o.NoOfHealthExamsOfNewJoiner, " +
                            "mis_o.PeriodicTests, mis_o.OccupationaIllnesses AS OccupationalIllnesses, mis_o.Status, " +
                            "mis_o.CreatedBy, CONCAT(u.firstName, u.lastName) AS submited_by, mis_o.CreatedAt, " +
                            "mis_o.UpdatedBy, CONCAT(u.firstName, u.lastName) AS Updated_by_name, mis_o.UpdatedAt " +
                            "FROM MIS_OccupationalHealthData AS mis_o " +
                            "LEFT JOIN users AS u ON u.id = mis_o.CreatedBy " +
                            "LEFT JOIN facilities AS f ON f.id = mis_o.facility_id " +
                            "WHERE mis_o.Status = 1;";
            List<OccupationalHealthData> data = await Context.GetData<OccupationalHealthData>(myQuery).ConfigureAwait(false);
            Parallel.ForEach(data, item =>
            {
                if (MonthDictionary.TryGetValue(item.month_id, out string monthName))
                {
                    item.month_name = monthName;
                }
            });
            return data;
        }
        //Create Vsitor Data
        internal async Task<CMDefaultResponse> CreateVisitsAndNotices(VisitsAndNotices request, int userID)
        {

            string myqry1 = $"INSERT INTO mis_visitsandnotices " +
                            $"(month_id,facility_id,year, GovtAuthVisits, NoOfFineByThirdParty, NoOfShowCauseNoticesByThirdParty, " +
                            $"NoticesToContractor, AmountOfPenaltiesToContractors, AnyOther, Status, CreatedBy, CreatedAt) " +
                            $"VALUES " +
                            $"({request.month_id},{request.facility_id},{request.year}, {request.GovtAuthVisits}, {request.NoOfFineByThirdParty}, " +
                            $"{request.NoOfShowCauseNoticesByThirdParty}, {request.NoticesToContractor}, " +
                            $"{request.AmountOfPenaltiesToContractors}, {request.AnyOther}, 1, " +
                            $"{userID}, '{UtilsRepository.GetUTCTime()}'); " +
                            $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Visit Notice Created");
        }
        internal async Task<CMDefaultResponse> UpdateVisitsAndNotices(VisitsAndNotices request, int userID)
        {

            string updateQry = "UPDATE mis_visitsandnotices SET ";

            if (request.month_id != 0)
                updateQry += $"month_id = {request.month_id}, ";
            if (request.GovtAuthVisits > 0)
                updateQry += $"GovtAuthVisits = {request.GovtAuthVisits}, ";
            if (request.facility_id != 0)
                updateQry += $"facility_id = {request.facility_id}, ";
            if (request.year != 0)
                updateQry += $"year = {request.year}, ";
            if (request.NoOfFineByThirdParty > 0)
                updateQry += $"NoOfFineByThirdParty = {request.NoOfFineByThirdParty}, ";
            if (request.NoOfShowCauseNoticesByThirdParty > 0)
                updateQry += $"NoOfShowCauseNoticesByThirdParty = {request.NoOfShowCauseNoticesByThirdParty}, ";
            if (request.NoticesToContractor > 0)
                updateQry += $"NoticesToContractor = {request.NoticesToContractor}, ";
            if (request.AmountOfPenaltiesToContractors > 0)
                updateQry += $"AmountOfPenaltiesToContractors = {request.AmountOfPenaltiesToContractors}, ";
            if (request.AnyOther > 0)
                updateQry += $"AnyOther = {request.AnyOther}, ";

            updateQry += $"UpdatedBy = {userID}, UpdatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Visit  Notice Updated");
        }
        internal async Task<CMDefaultResponse> DeleteVisitsAndNotices(int id, int userID)
        {
            string deleteQry = $"DELETE FROM mis_visitsandnotices WHERE id = {id};";

            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Visit Notice Deleted");
        }
        internal async Task<List<VisitsAndNotices>> GetVisitsAndNotices()
        {
            string myQuery = "SELECT mis_v.id, mis_v.month_id, mis_v.facility_id, mis_v.year, " +
                 "mis_v.GovtAuthVisits, mis_v.NoOfFineByThirdParty, mis_v.NoOfShowCauseNoticesByThirdParty, " +
                 "mis_v.NoticesToContractor, mis_v.AmountOfPenaltiesToContractors, mis_v.AnyOther, mis_v.Status, " +
                 "mis_v.CreatedBy, CONCAT(u.firstName, u.lastName) AS submited_by, mis_v.CreatedAt, " +
                 "mis_v.UpdatedBy, CONCAT(u.firstName, u.lastName) AS Updated_by_name, mis_v.UpdatedAt " +
                 "FROM mis_visitsandnotices AS mis_v " +
                 "LEFT JOIN users AS u ON u.id = mis_v.CreatedBy " +
                 "WHERE mis_v.Status = 1;";
            List<VisitsAndNotices> data = await Context.GetData<VisitsAndNotices>(myQuery).ConfigureAwait(false);
            Parallel.ForEach(data, item =>
            {
                if (MonthDictionary.TryGetValue(item.month_id, out string monthName))
                {
                    item.month_name = monthName;
                }
            });
            return data;
        }
        //Fuel Consumption
        internal async Task<CMDefaultResponse> CreateFuelConsumption(FuelData request, int userID)
        {
            string myqry1 = $"INSERT INTO mis_fueldata (month_id,facility_id,year, DieselConsumedForVehicles, PetrolConsumedForVehicles, PetrolConsumedForGrassCuttingAndMovers, DieselConsumedAtSite, PetrolConsumedAtSite, Status, CreatedBy, CreatedAt) " +
                             $"VALUES " +
                             $"({request.month_id}, " +
                             $"{request.facility_id},{request.year}," +
                             $"{request.DieselConsumedForVehicles}, " +
                             $"{request.PetrolConsumedForVehicles}, " +
                             $"{request.PetrolConsumedForGrassCuttingAndMovers}, " +
                             $"{request.DieselConsumedAtSite}, " +
                             $"{request.PetrolConsumedAtSite},1, " +
                             $"{userID}, " +
                             $"'{UtilsRepository.GetUTCTime()}'); " +
                             $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Fuel Consumption Data Created");
        }
        internal async Task<CMDefaultResponse> UpdateFuelConsumption(FuelData request, int userID)
        {
            string updateQry = "UPDATE mis_fueldata SET ";
            updateQry += $"month_id = {request.month_id}, ";
            updateQry += $"DieselConsumedForVehicles = {request.DieselConsumedForVehicles}, ";
            updateQry += $"facility_id = {request.facility_id}, ";
            updateQry += $"year = {request.year}, ";
            updateQry += $"PetrolConsumedForVehicles = {request.PetrolConsumedForVehicles}, ";
            updateQry += $"PetrolConsumedForGrassCuttingAndMovers = {request.PetrolConsumedForGrassCuttingAndMovers}, ";
            updateQry += $"DieselConsumedAtSite = {request.DieselConsumedAtSite}, ";
            updateQry += $"PetrolConsumedAtSite = {request.PetrolConsumedAtSite}, ";
            updateQry += $"UpdatedBy = {userID}, ";
            updateQry += $"UpdatedAt = '{UtilsRepository.GetUTCTime()}' ";
            updateQry += $"WHERE id = {request.id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Fuel Consumption Data Updated");
        }
        internal async Task<CMDefaultResponse> DeleteFuelConsumption(int id)
        {
            string deleteQry = $"DELETE FROM mis_fueldata WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Fuel Consumption Data Deleted");
        }
        internal async Task<List<FuelData>> GetFuelConsumption()
        {

            string myQuery = "SELECT mis_f.id, mis_f.month_id, mis_f.facility_id, mis_f.year, " +
                             "mis_f.DieselConsumedForVehicles, mis_f.PetrolConsumedForVehicles, " +
                             "mis_f.PetrolConsumedForGrassCuttingAndMovers, mis_f.DieselConsumedAtSite, " +
                             "mis_f.PetrolConsumedAtSite, mis_f.CreatedBy, CONCAT(u.firstName, u.lastName) AS submited_by, " +
                             "mis_f.CreatedAt, mis_f.UpdatedBy, CONCAT(u.firstName, u.lastName) AS Updated_by_name, mis_f.UpdatedAt " +
                             "FROM mis_fueldata AS mis_f " +
                             "LEFT JOIN users AS u ON u.id = mis_f.CreatedBy " +
                             "WHERE mis_f.Status = 1 ;";
            List<FuelData> data = await Context.GetData<FuelData>(myQuery).ConfigureAwait(false);
            Parallel.ForEach(data, item =>
            {
                if (MonthDictionary.TryGetValue(item.month_id, out string monthName))
                {
                    item.month_name = monthName;
                }
            });
            return data;
        }

        //Plantation Data

        public async Task<CMDefaultResponse> CreatePlantationData(PlantationData request, int userID)
        {

            string myqry1 = $"INSERT INTO mis_plantationdata (month_id,facility_id,year, SaplingsPlanted, SaplingsSurvived, SaplingsDied, Status, CreatedBy, CreatedAt) " +
                            $"VALUES " +
                            $"({request.month_id},{request.facility_id},{request.year}, " +
                            $"{request.SaplingsPlanted}, " +
                            $"{request.SaplingsSurvived}, " +
                            $"{request.SaplingsDied},1, " +
                            $"{userID}, " +
                            $"'{UtilsRepository.GetUTCTime()}'); " +
                            $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Plantation Data Created");
        }

        public async Task<CMDefaultResponse> UpdatePlantationData(PlantationData request, int userID)
        {

            string updateQry = "UPDATE mis_plantationdata SET ";
            updateQry += $"month_id = {request.month_id}, ";
            updateQry += $"facility_id = {request.facility_id}, ";
            updateQry += $"year = {request.year}, ";
            updateQry += $"SaplingsPlanted = {request.SaplingsPlanted}, ";
            updateQry += $"SaplingsSurvived = {request.SaplingsSurvived}, ";
            updateQry += $"SaplingsDied = {request.SaplingsDied}, ";
            updateQry += $"UpdatedBy = {userID}, ";
            updateQry += $"UpdatedAt = '{UtilsRepository.GetUTCTime()}' ";
            updateQry += $"WHERE id = {request.id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Plantation Data Updated");
        }

        public async Task<CMDefaultResponse> DeletePlantationData(int id)
        {
            string deleteQry = $"DELETE FROM mis_plantationdata WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Plantation Data Deleted");
        }

        public async Task<List<PlantationData>> GetPlantationData()
        {
            string myQuery = "SELECT mis_p.id, mis_p.month_id, mis_p.facility_id, mis_p.year, mis_p.SaplingsPlanted, mis_p.SaplingsSurvived, " +
                             "mis_p.SaplingsDied, mis_p.CreatedBy, CONCAT(u.firstName, u.lastName) AS submited_by, mis_p.CreatedAt, " +
                             "mis_p.UpdatedBy, CONCAT(u.firstName, u.lastName) AS Updated_by_name, mis_p.UpdatedAt " +
                             "FROM mis_plantationdata AS mis_p " +
                             "LEFT JOIN users AS u ON u.id = mis_p.CreatedBy WHERE mis_p.Status = 1;";
            List<PlantationData> data = await Context.GetData<PlantationData>(myQuery).ConfigureAwait(false);
            Parallel.ForEach(data, item =>
            {
                if (MonthDictionary.TryGetValue(item.month_id, out string monthName))
                {
                    item.month_name = monthName;
                }
            });
            return data;
        }

        //Kizensdata
        public async Task<CMDefaultResponse> CreateKaizensData(KaizensData request, int userID)
        {

            string myqry1 = $"INSERT INTO mis_kaizensdata (month_id,facility_id,year, KaizensImplemented, CostForImplementation, CostSavedFromImplementation, Status, CreatedBy, CreatedAt) " +
                            $"VALUES " +
                            $"({request.month_id},{request.facility_id},{request.year}, " +
                            $"{request.KaizensImplemented}, " +
                            $"{request.CostForImplementation}, " +
                            $"{request.CostSavedFromImplementation},1, " +
                            $"{userID}, " +
                            $"'{UtilsRepository.GetUTCTime()}'); " +
                            $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Kaizens Data Created");
        }

        public async Task<CMDefaultResponse> UpdateKaizensData(KaizensData request, int userID)
        {

            string updateQry = "UPDATE mis_kaizensdata SET ";
            updateQry += $"month_id = {request.month_id}, ";
            updateQry += $"facility_id = {request.facility_id}, ";
            updateQry += $"year = {request.year}, ";
            updateQry += $"KaizensImplemented = {request.KaizensImplemented}, ";
            updateQry += $"CostForImplementation = {request.CostForImplementation}, ";
            updateQry += $"CostSavedFromImplementation = {request.CostSavedFromImplementation}, ";
            updateQry += $"UpdatedBy = {userID}, ";
            updateQry += $"UpdatedAt = '{UtilsRepository.GetUTCTime()}' ";
            updateQry += $"WHERE id = {request.id};";

            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Kaizens Data Updated");
        }

        public async Task<CMDefaultResponse> DeleteKaizensData(int id)
        {
            string deleteQry = $"DELETE FROM mis_kaizensdata WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Kaizens Data Deleted");
        }
        public async Task<List<KaizensData>> GetKaizensData()
        {
            string myQuery = "SELECT mis_k.id, mis_k.month_id, mis_k.facility_id, mis_k.year, mis_k.KaizensImplemented, mis_k.CostForImplementation," +
                             "mis_k.CostSavedFromImplementation, mis_k.CreatedBy, CONCAT(u.firstName, u.lastName) AS submited_by, " +
                             "mis_k.CreatedAt, mis_k.UpdatedBy, CONCAT(u.firstName, u.lastName) AS Updated_by_name, mis_k.UpdatedAt " +
                             "FROM mis_kaizensdata AS mis_k " +
                             "LEFT JOIN users AS u ON u.id = mis_k.CreatedBy " +
                             "LEFT JOIN facilities as f on f.id=mis_k.facility_id " +
                             " WHERE mis_k.Status = 1;";
            List<KaizensData> data = await Context.GetData<KaizensData>(myQuery).ConfigureAwait(false);
            Parallel.ForEach(data, item =>
            {
                if (MonthDictionary.TryGetValue(item.month_id, out string monthName))
                {
                    item.month_name = monthName;
                }
            });
            return data;
        }
        public async Task<List<CumalativeReport>> Cumulativereport(string facility_id, int module_id, string start_date, string end_date)
        {

            if (2 == module_id)
            {
                string myQueryJob = "SELECT fc.name as Site_name,  COUNT( js.createdBy > 0) AS Created, COUNT( jc.JC_End_By_id > 0) as Closed, " +
                                    " COUNT( jc.JC_End_By_id > 0) AS CardsEnded , " +
                                    "COUNT( js.cancelledBy) AS Cancelled , COUNT(jc.JC_Start_By_id = 0) AS NotStarted, " +
                                    "COUNT(jc.JC_End_By_id = 0 and jc.JC_Start_By_id = 0) AS Ongoing, " +
                                    "count(DATEDIFF(jc.JC_Date_Start, jc.JC_Date_Stop))  AS ClosedOnTime, " +
                                    "COUNT(per.extendStatus = 1 and  jc.JC_End_By_id > 0) AS ClosedWithExtension " +
                                    "FROM jobs AS js LEFT JOIN jobcards AS jc ON js.id = jc.jobId " +
                                    "LEFT JOIN facilities AS fc ON js.facilityId = fc.id " +
                                    "LEFT JOIN permits AS per ON jc.PTW_id =per.id " +
                                    $"Where js.facilityId in({facility_id}) or js.createdAt BETWEEN '{start_date}' and '{end_date}'  group by js.facilityId ;";
                List<CumalativeReport> data = await Context.GetData<CumalativeReport>(myQueryJob).ConfigureAwait(false);
                return data;
            }
            if (39 == module_id)
            {
                string myQueryJob = "SELECT fc.name as Site_name,  COUNT( js.created_by > 0) AS Created, COUNT( jc.closed_by > 0) as Closed, " +
                                    "COUNT(jc.cancelled_by) AS Cancelled, COUNT(jc.started_by = 0) AS NotStarted, " +
                                    "COUNT( jc.started_by = 0 and jc.closed_by = 0) AS Ongoing, " +
                                    "count(DATEDIFF(jc.closed_at, jc.started_at)) AS ClosedOnTime, " +
                                    "count( permit.extendStatus = 1) as  ClosedWithExtension " +
                                    "FROM  pm_plan AS js " +
                                    "LEFT JOIN pm_task AS jc ON js.id = jc.plan_id " +
                                    "LEFT join permits AS permit on jc.ptw_id = permit.id " +
                                    "LEFT JOIN facilities AS fc ON js.facility_id = fc.id " +
                                    $"Where jc.facility_id in({facility_id}) or js.plan_date BETWEEN '{start_date}' AND '{end_date}' group by jc.facility_Id;";
                List<CumalativeReport> data = await Context.GetData<CumalativeReport>(myQueryJob).ConfigureAwait(false);
                return data;
            }
            if (43 == module_id)
            {
                string myQueryJob = "SELECT f.name AS Site_name, " +
                                    "CASE WHEN mc.moduleType = 1 THEN 'Wet' " +
                                       "WHEN mc.moduleType = 2 THEN 'Dry' ELSE 'Robotic' END AS CleaningType, " +
                                       "sub1.TotalWaterUsed AS waterUsed, " +
                                       "SUM(css.moduleQuantity) AS scheduledQuantity, " +
                                       "sub2.no_of_cleaned AS actualQuantity, " +
                                       "CASE WHEN mc.abandonedById > 0 THEN 'yes' ELSE 'no' END AS Abandoned, " +
                                       "mc.reasonForAbandon AS remark, " +
                                       "(SUM(css.moduleQuantity) - sub2.no_of_cleaned) AS deviation, " +
                                       "CASE WHEN mc.abandonedAt IS NOT NULL THEN TIMESTAMPDIFF(MINUTE, mc.startDate,mc.endedAt) " +
                                       "ELSE TIMESTAMPDIFF(MINUTE,mc.startDate,mc.endedAt) END AS timeTaken " +
                                       "FROM cleaning_execution AS mc " +
                                       "LEFT JOIN cleaning_plan AS mp ON mp.planId = mc.planId " +
                                       "LEFT JOIN cleaning_execution_items AS css ON css.executionId = mc.id " +
                                       "LEFT JOIN (SELECT executionId, SUM(waterUsed) AS TotalWaterUsed " +
                                       "FROM cleaning_execution_schedules GROUP BY executionId) sub1 " +
                                       "ON mc.id = sub1.executionId " +
                                       "LEFT JOIN (SELECT executionId, SUM(moduleQuantity) AS no_of_cleaned " +
                                       "FROM cleaning_execution_items WHERE cleanedById > 0 GROUP BY executionId) sub2 " +
                                       "ON mc.id = sub2.executionId " +
                                       "LEFT JOIN Frequency AS freq ON freq.id = mp.frequencyId " +
                                       "LEFT JOIN facilities AS f ON f.id = mc.facilityId " +
                                       $"WHERE mc.facilityId IN ({facility_id}) AND mc.moduleType = 1 or mc.executionStartedAt BETWEEN '{start_date}' AND '{end_date}' group by mc.facilityId;";


                List<CumalativeReport> data = await Context.GetData<CumalativeReport>(myQueryJob).ConfigureAwait(false);
                return data;
            }
            if (44 == module_id)
            {
                string myQueryJob = "SELECT f.name AS Site_name, " +
                                   "CASE WHEN mc.moduleType = 1 THEN 'Wet' " +
                                      "WHEN mc.moduleType = 2 THEN 'Dry' ELSE 'Robotic' END AS CleaningType, " +
                                      "sub1.TotalWaterUsed AS WaterUsed, " +
                                      "SUM(css.area) AS ScheduledQuantity, " +
                                      "sub2.no_of_cleaned AS actualQuantity, " +
                                      "CASE WHEN mc.abandonedById > 0 THEN 'yes' ELSE 'no' END AS Abandoned, " +
                                      "mc.reasonForAbandon AS remark, " +
                                      "(SUM(css.area) - sub2.no_of_cleaned) AS deviation, " +
                                      "CASE WHEN mc.abandonedAt IS NOT NULL THEN TIMESTAMPDIFF(MINUTE,mc.startDate,mc.endedAt) " +
                                      "ELSE TIMESTAMPDIFF(MINUTE,mc.startDate,mc.endedAt) END AS timeTaken " +
                                      "FROM cleaning_execution AS mc " +
                                      "LEFT JOIN cleaning_plan AS mp ON mp.planId = mc.planId " +
                                      "LEFT JOIN cleaning_execution_items AS css ON css.executionId = mc.id " +
                                      "LEFT JOIN (SELECT executionId, SUM(waterUsed) AS TotalWaterUsed " +
                                      "FROM cleaning_execution_schedules GROUP BY executionId) sub1 " +
                                      "ON mc.id = sub1.executionId " +
                                      "LEFT JOIN (SELECT executionId, SUM(area) AS no_of_cleaned " +
                                      "FROM cleaning_execution_items WHERE cleanedById > 0 GROUP BY executionId) sub2 " +
                                      "ON mc.id = sub2.executionId " +
                                      "LEFT JOIN Frequency AS freq ON freq.id = mp.frequencyId " +
                                      "LEFT JOIN facilities AS f ON f.id = mc.facilityId " +
                                      $"WHERE mc.facilityId IN ({facility_id}) AND mc.moduleType = 2 or mc.executionStartedAt BETWEEN '{start_date}' AND '{end_date}' group by mc.facilityId;";
                List<CumalativeReport> data = await Context.GetData<CumalativeReport>(myQueryJob).ConfigureAwait(false);

                return data;

            }
            return null;

        }
        internal async Task<CMDefaultResponse> CloseObservation(CMApproval request, int userId, int check_point_type_id)
        {
            string deleteQry = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder("Observation Updated");
            if (request.comment.Length > 0)

                if (check_point_type_id == 1)
                {

                    sb.Append(": " + request.comment);
                    deleteQry = $"UPDATE observations SET status_code = {(int)CMMS_Status.OBSERVATION_CLOSED}, closed_by = '{userId}' , closed_at='{UtilsRepository.GetUTCTime()}' ,preventive_action = '{request.comment}'  WHERE id = {request.id};";

                    await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);

                    sb = new System.Text.StringBuilder("Observation Updated");
                    if (request.comment.Length > 0)
                    {
                        sb.Append(": " + request.comment);
                    }
                    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, request.id, 0, 0, sb.ToString(), CMMS.CMMS_Status.OBSERVATION_CLOSED, userId); ;



                }
                else if (check_point_type_id == 2)
                {
                    deleteQry = $"UPDATE pm_execution SET Observation_Status = {(int)CMMS_Status.OBSERVATION_CLOSED}, closed_by = '{userId}' , closed_at='{UtilsRepository.GetUTCTime()}', preventive_action = '{request.comment}'WHERE id = {request.id};";
                    await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                    sb = new System.Text.StringBuilder("Observation Updated");
                    if (request.comment.Length > 0)
                    {
                        sb.Append(": " + request.comment);
                    }
                    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_EXECUTION, request.id, 0, 0, sb.ToString(), CMMS.CMMS_Status.OBSERVATION_CLOSED, userId); ;
                }
                else
                {
                    throw new ArgumentException("Invalid Observation Type");
                }

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Observation {request.id} closed");
        }
        public async Task<CMDefaultResponse> AssingtoObservation(AssignToObservation request, int check_point_type_id, int userId)
        {
            string updateQry = "";

            if (check_point_type_id == 2)
            {
                updateQry = "UPDATE pm_execution SET ";
                updateQry += $"Observation_assign_to = {request.assigned_to_id}, ";
                updateQry += $"Observation_Status = {(int)CMMS.CMMS_Status.OBSERVATION_ASSIGNED}, ";
                updateQry += $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}', ";
                updateQry += $"Observation_target_date = {(request.target_date.HasValue ? $"'{request.target_date.Value.ToString("yyyy-MM-dd")}'" : "NULL")}, ";
                updateQry += $"preventive_action = '{request.preventive_action}', ";
                updateQry += $"PM_Schedule_Observation_update_by = '{userId}' ";
                updateQry += $"WHERE id = {request.id};";
                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            }
            else if (check_point_type_id == 1)
            {
                updateQry = "UPDATE observations SET ";
                updateQry += $"assign_to = {request.assigned_to_id}, ";
                updateQry += $"target_date = {(request.target_date.HasValue ? $"'{request.target_date.Value.ToString("yyyy-MM-dd")}'" : "NULL")}, ";
                updateQry += $"status_code = {(int)CMMS.CMMS_Status.OBSERVATION_ASSIGNED}, ";
                updateQry += $"contractor_name = '{request.contractor_name}', ";
                updateQry += $"risk_type_id = {request.risk_type_id}, ";
                updateQry += $"preventive_action = '{request.preventive_action}', ";
                updateQry += $"responsible_person = {request.assigned_to_id}, ";
                updateQry += $"contact_number = '{request.contact_number}', ";
                updateQry += $"cost_type = {request.cost_type}, ";
                updateQry += $"date_of_observation = {(request.date_of_observation.HasValue ? $"'{request.date_of_observation.Value.ToString("yyyy-MM-dd")}'" : "NULL")}, ";
                updateQry += $"type_of_observation = {request.type_of_observation}, ";  // Removed extra quotes for int value
                updateQry += $"location_of_observation = '{request.location_of_observation}', ";
                updateQry += $"action_taken = '{request.action_taken}', ";
                updateQry += $"source_of_observation = {request.source_of_observation}, ";
                updateQry += $"observation_description = '{request.observation_description}', ";
                updateQry += $"updated_at = '{UtilsRepository.GetUTCTime()}', ";
                updateQry += $"updated_by = '{userId}' ";
                updateQry += $"WHERE id = {request.id};";
                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            }

            else
            {
                throw new ArgumentException("Invalid Observation Type");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, request.id, 0, 0, request.observation_description, CMMS.CMMS_Status.ASSIGNED, request.user_id);

            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Assigned Observation ");
        }
        //EvaluateException
        internal async Task<CMDefaultResponse> CreateEvaluation(CMEvaluationCreate request, int user_id)
        {

            string addPlanQry = $"INSERT INTO evaluation_plan(plan_name, facility_id, frequency_id, plan_date, assigned_to, " +
                    $"status, created_by, created_at, remarks,status) VALUES " +
                    $"('{request.plan_name}', {request.facility_id}, {request.frequency_id}, " +
                    $"'{request.plan_date.ToString("yyyy-MM-dd")}', {request.assigned_to}, {user_id},'{UtilsRepository.GetUTCTime()}' , " +
                    $"'{request.remarks}',{(int)CMMS.CMMS_Status.EVAL_PLAN_CREATED}); " +
                    $"SELECT LAST_INSERT_ID();";
            DataTable dt3 = await Context.FetchData(addPlanQry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);


            string mapChecklistQry = "INSERT INTO evalution_auditmap(evalution_id, audit_id, weightage,comments,created_by,created_at) VALUES ";
            /*   foreach (var map in request.mapauditlist)
               {
                   mapChecklistQry += $"({id}, {map.audit_id}, {map.weightage}), ";
               }*/
            mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
            await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, id, 0, 0, "Eval Plan added", CMMS.CMMS_Status.EVAL_PLAN_CREATED, user_id);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, request.remarks);

            return null;
        }

        internal async Task<CMDefaultResponse> ApproveEvaluation(CMApproval request, int userID)
        {

            return null;
        }
        public async Task<List<CMEvaluationCreate>> GetEvaluationPlan(int id, int userID)
        {
            return null;
        }
        internal async Task<CMDefaultResponse> ApproveObservation(CMApproval request, int userID, string facilitytimeZone, int check_point_type_id)
        {
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.OBSERVATION;
            CMMS.CMMS_Status status = CMMS.CMMS_Status.OBSERVATION_APPROVED;
            if (check_point_type_id == 1)
            {

                string approveQuery = $"Update observations set status_code= {(int)status} ,approvedById={userID}, approveRemark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
                await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            }
            else if (check_point_type_id == 2)
            {
                string approveQuery = $"Update pm_execution set Observation_Status= {(int)status} ,approvedById={userID}, approveRemark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}', PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
                await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException("Invalid Observation Type");
            }

            //ADD REMARK TO HISTORY
            //add db col rejectRemark approveRemark
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, request.id, 0, 0, "Observation Approved", CMMS.CMMS_Status.OBSERVATION_APPROVED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Observation Approved");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectObservation(CMApproval request, int userID, string facilitytimeZone, int check_point_type_id)
        {
            CMMS.CMMS_Modules module = CMMS.CMMS_Modules.OBSERVATION;
            CMMS.CMMS_Status status = CMMS.CMMS_Status.OBSERVATION_REJECTED;

            if (check_point_type_id == 1)
            {
                string approveQuery = $"Update observations set status_code= {(int)status} ,rejectedById={userID}, rejectRemark='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}', status_updated_at = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
                await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            }
            else if (check_point_type_id == 2)
            {
                string approveQuery = $"Update pm_execution set Observation_Status = {(int)status} ,rejectedById={userID}, rejectRemark='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}', PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
                await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException("Invalid Observation Type");
            }
            //ADD REMARK TO HISTORY
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.OBSERVATION, request.id, 0, 0, "Observation Rejected", CMMS.CMMS_Status.OBSERVATION_REJECTED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Observation Rejected");
            return response;
        }

        internal async Task<List<ProjectDetails>> GetMisSummary(string year, int userID)
        {
            string facility = "Select f.name as sitename, f.state as state,f.city as district,bus1.name as contractor_name,bus.name AS contractor_site_incahrge_name,spv1.name as spv_name from facilities as f  left join business as bus on bus.id=f.ownerId left join business as bus1 on bus.id=f.operatorId left join spv as spv1 on spv1.id=f.spvId where parentId=0 group by f.id;";
            List<ProjectDetails> data = await Context.GetData<ProjectDetails>(facility).ConfigureAwait(false);

            string hfedat = "SELECT  CONCAT(YEAR(e.Date), '-', LPAD(MONTH(e.Date), 2, '0')) AS YearMonth,      COUNT( e.employee_id) AS AvgHFEEmployee,      COUNT(e.employee_id) AS ManDaysHFEEmployee,      COUNT(DISTINCT e.Date) AS ManHoursWorkedHFEEmployee,     COUNT(e.employee_id) * 8 AS ManHoursWorkedHFEEmployee FROM      employee_attendance AS e LEFT JOIN      facilities AS f ON f.id = e.facility_id LEFT JOIN      (SELECT DISTINCT Date FROM contractor_attendnace) AS cd ON cd.Date = e.Date     where e.present=1  GROUP BY      f.id,  YEAR(e.Date), MONTH(e.Date);";
            List<ManPowerData> hfedata = await Context.GetData<ManPowerData>(hfedat).ConfigureAwait(false);

            string occupationaldata = "select NoOfHealthExamsOfNewJoiner,PeriodicTests,OccupationaIllnesses from mis_occupationalhealthdata group by month_id; ";
            List<OccupationalHealthData> occupational_Helath = await Context.GetData<OccupationalHealthData>(occupationaldata).ConfigureAwait(false);

            string visitnotice = "SELECT GovtAuthVisits,NoOfFineByThirdParty,NoOfShowCauseNoticesByThirdParty,NoticesToContractor,NoticesToContractor, AmountOfPenaltiesToContractors,AnyOther FROM mis_visitsandnotices group by month_id;";
            List<VisitsAndNotices> Regulatory_visit_notice = await Context.GetData<VisitsAndNotices>(visitnotice).ConfigureAwait(false);

            List<ProjectDetails> projectDetailsList = new List<ProjectDetails>();

            foreach (var item in data)
            {
                ProjectDetails projectDetail = new ProjectDetails
                {
                    SpvName = item.SpvName,
                    sitename = item.sitename,
                    State = item.State,
                    District = item.District,
                    ContractorName = item.ContractorName,
                    ContractorSiteInChargeName = item.ContractorSiteInChargeName,
                    MonthlyData = hfedata,
                    healthDatas = occupational_Helath,
                    visitsAndNotices = Regulatory_visit_notice
                };
                projectDetailsList.Add(projectDetail);
            }
            return projectDetailsList;
        }
        internal async Task<List<EnviromentalSummary>> GeEnvironmentalSummary(string year, int userID)
        {
            string facility = "Select name as facilty_name from facilities where  parentId=0 group by id;";
            List<EnviromentalSummary> data = await Context.GetData<EnviromentalSummary>(facility).ConfigureAwait(false);
            string occupationaldata = "select NoOfHealthExamsOfNewJoiner,PeriodicTests,OccupationaIllnesses from mis_occupationalhealthdata group by month_id; ";
            List<OccupationalHealthData> occupational_Helath = await Context.GetData<OccupationalHealthData>(occupationaldata).ConfigureAwait(false);

            string visitnotice = "SELECT GovtAuthVisits,NoOfFineByThirdParty,NoOfShowCauseNoticesByThirdParty,NoticesToContractor,NoticesToContractor, AmountOfPenaltiesToContractors,AnyOther FROM mis_visitsandnotices group by month_id;";
            List<VisitsAndNotices> Regulatory_visit_notice = await Context.GetData<VisitsAndNotices>(visitnotice).ConfigureAwait(false);

            List<EnviromentalSummary> projectDetailsList = new List<EnviromentalSummary>();

            foreach (var item in data)
            {
                EnviromentalSummary projectDetail = new EnviromentalSummary
                {
                    healthDatas = occupational_Helath,
                    visitsAndNotices = Regulatory_visit_notice
                };
                projectDetailsList.Add(projectDetail);
            }
            return projectDetailsList;
        }

    }
}
