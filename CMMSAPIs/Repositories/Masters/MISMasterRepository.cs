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
            { CMMS.CMMS_Modules.VEGETATION, 33 },
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
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Source Observation Added");
        }

        internal async Task<CMDefaultResponse> UpdateSourceOfObservation(MISSourceOfObservation request, int userID)
        {
            string updateQry = "UPDATE mis_m_observationsheet SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Source Observation Updated");
        }
        internal async Task<CMDefaultResponse> DeleteSourceOfObservation(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_m_observationsheet SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Source Observation Deleted");
        }


        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        /************************************************* TYPE OF OBSERVATION *************************************************************/
        /***********************************************************************************************************************************/
        /***********************************************************************************************************************************/
        internal async Task<MISTypeObservation> GetTypeOfObservation(int type_id)
        {
            string myQuery = $"SELECT id, name, description, status FROM mis_m_typeofobservation WHERE id = " + type_id;
            List<MISTypeObservation> _Typeofobs = await Context.GetData<MISTypeObservation>(myQuery).ConfigureAwait(false);
            //Add history
            return _Typeofobs[0];
        }

        internal async Task<List<MISTypeObservation>> GetTypeOfObservationList()
        {
            string myQuery = $"SELECT id, name, description FROM mis_m_typeofobservation WHERE status = 1 ";
            List<MISTypeObservation> _Sourceofobs = await Context.GetData<MISTypeObservation>(myQuery).ConfigureAwait(false);
            //Add history
            return _Sourceofobs;
        }
        internal async Task<CMDefaultResponse> AddTypeOfObservation(MISTypeObservation request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into mis_m_typeofobservation ( name, description, status , addedBy ,addedAt) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}');" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Type Observation Added");
        }

        internal async Task<CMDefaultResponse> UpdateTypeOfObservation(MISTypeObservation request, int userID)
        {
            string updateQry = "UPDATE mis_m_typeofobservation SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Type Observation Updated");
        }
        internal async Task<CMDefaultResponse> DeleteTypeOfObservation(int id, int userId)
        {
            string deleteQry = $"UPDATE mis_m_typeofobservation SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            //Add history
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Type Observation Deleted");
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
                string detail_Q = " select id, DATE_FORMAT(date,'%Y-%m-%d') date,description,creditQty as procured_qty,debitQty as consumed_qty," +
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
                    string detail_Q = " select mw.id,date,mw.description,creditQty as procured_qty,debitQty as consumed_qty," +
                          "  case when consumeTypeId = 1 then 'Procurement' when consumeTypeId = 2 then 'Consumption' else 'NA' end as TransactionType, show_opening" +
                          " from waste_data LEFT JOIN mis_wastetype mw ON mw.id = waste_data.wasteTypeId where wasteTypeId = " + groupedResult_new[0].item_data[i].wasteTypeId + " and waste_data.isHazardous = " + Hazardous + " AND waste_data.facilityId = " + facility_id + " AND MONTH(date) = " + Month + " AND YEAR(date) = " + Year + " ;";


                    groupedResult_new[0].item_data[i].details = await Context.GetData<CMWasteDataMonthWiseDetails_Month>(detail_Q).ConfigureAwait(false);
                }
            }
            return groupedResult_new;
        }

        internal async Task<List<CMChecklistInspectionReport>> GetChecklistInspectionReport(string facility_id, int module_type, DateTime fromDate, DateTime toDate)
        {
            string myQuery = " select distinct  st.id, checklist_number as checklist_name , '' SOP_number, " +
                " frequency.name as frequency, case when  is_ok = 0 then 'No' when is_ok = 1 then 'Yes' else 'NA' end as inspection_status, " +
                " PM_Schedule_Observation_add_date as date_of_inspection, monthname(PM_Schedule_Observation_add_date) as month, " +
                " case when file_required = 0 then 'No' else 'Yes' end  as checklist_attachment" +
                " from st_audit st " +
                " left join pm_task task on task.plan_id = st.id " +
                " left join pm_execution pm_execution on pm_execution.task_id = task.id" +
                " left join checklist_number checklist_number on checklist_number.id = st.Checklist_id " +
                " left join frequency frequency on frequency.id = st.Frequency" +
                " where st.facility_id in ( " + facility_id + ") and st.module_type_id = " + module_type + " and DATE_FORMAT(PM_Schedule_Observation_add_date,'%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'" +
                "  group by st.id order by st.id desc; ";
            List<CMChecklistInspectionReport> response = await Context.GetData<CMChecklistInspectionReport>(myQuery).ConfigureAwait(false);
            return response;
        }


        internal async Task<List<CMObservationReport>> GetObservationSheetReport(string facility_id, DateTime fromDate, DateTime toDate)
        {
            string myQuery = " select st.id, distinct monthname(PM_Schedule_Observation_add_date) as month_of_observation, " +
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

            string myQuery = $"SELECT s.id,s.Compliance_id,st.name as compilanceName,s.comment as description, s.facility_id, " +
                             $"s.Issue_date AS start_date, s.expires_on AS end_date, s.status as status_id ,s.created_by, CONCAT(uc.firstName, ' ', uc.lastName) AS createdByName, CONCAT(u.firstName, ' ', u.lastName) AS UpdatedByName ," +
                             $" CONCAT(us.firstName, ' ', us.lastName) AS ApprovedByName, s.created_at, s.updated_by, s.updated_at, s.renew_from as renew_date, s.renew_by , s.approved_by, s.approved_at, " +
                             $" DAY(expires_on) AS status, " +
                             $" YEAR(expires_on) AS expiry_year,DATEDIFF(expires_on, now()) AS daysLeft,TIMESTAMPDIFF(MONTH, now(), expires_on) AS validity_month, " +
                             $" CASE WHEN expires_on< Now() THEN 'inactive'  ELSE 'active'    END AS Activation_status " +
                             $" FROM statutory AS s LEFT JOIN users uc ON  s.created_by = uc.id " +
                             $" LEFT JOIN users u on s.updated_by = u.id" +
                             $" LEFT JOIN users us on s.approved_by = us.id " +
                             $"LEFT JOIN statutorycomliance as st on st.id = s.Compliance_id " +
                             $" where facility_id ={facility_id} or s.created_at={start_date} or s.created_at={end_date} ;";
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
                          $" CASE WHEN expires_on< Now() THEN 'inactive'  ELSE 'active'    END AS Activation_status " +
                          $" FROM statutory AS s LEFT JOIN users uc ON  s.created_by = uc.id " +
                          $" LEFT JOIN users u on s.updated_by = u.id" +
                          $" LEFT JOIN users us on s.approved_by = us.id " +
                          $"LEFT JOIN statutorycomliance as st on st.id = s.Compliance_id " +
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
            string myQuery = $"SELECT distinct s.id,Compliance_id,sc.name ,facility_id, Issue_date as start_date, " +
                $" expires_on as end_date, s.status,concat(created.firstName, ' ', created.lastName)  created_by, s.created_at," +
                $" concat(updated.firstName, ' ', updated.lastName)  updated_by , s.updated_at, renew_from, renew_from_id, " +
                $" concat(approved.firstName, ' ', approved.lastName)  approved_by , s.approved_at, " +
                $" concat(rejected.firstName, ' ', rejected.lastName)  rejected_by ,   s.rejected_at " +
                $" FROM statutory s" +
                $" left join statutorycomliance sc on sc.id = s.Compliance_id " +
                $" left join users created on  created.id = s.created_by" +
                $" left join users updated on  updated.id = s.updated_by" +
                $" left join users approved on  approved.id = s.approved_by" +
                $" left join users rejected on  rejected.id = s.rejected_by" +
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
                updateQry += $"description = '{request.description}', ";
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
    }

}
