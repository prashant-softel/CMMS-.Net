using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Repositories.Incident_Reports;
using CMMSAPIs.Repositories.WC;
using CMMSAPIs.Repositories.Calibration;
using CMMSAPIs.Repositories.Inventory;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Notifications;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

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
            { CMMS.CMMS_Modules.VEGETATION, 33 }
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
        internal async Task<List<WaterDataType>> GetWaterType(int facility_id)
        {

            string getqry = $"Select * from mis_watertype where facility_id=" + (facility_id)+" and status=1;";
            List<WaterDataType> Data = await Context.GetData<WaterDataType>(getqry).ConfigureAwait(false);
            return Data;
        }
        internal async Task<CMDefaultResponse>CreateWaterType(WaterDataType request, int UserID)
        {
            string myQuery = $"INSERT INTO mis_watertype(facility_id,name,description,status,CreatedAt,CreatedBy) VALUES " +
            $"('{request.facility_id}','{request.name} ','{request.description}',1,'{UtilsRepository.GetUTCTime()}','{UserID}');" +
            $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS,"WaterType  Created");
        }
             internal async Task<CMDefaultResponse> UpdateWaterType(WaterDataType request, int UserID)
        {
            string updateQry = "UPDATE mis_watertype  SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = '{UserID}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WaterType updated");
        }
        internal async Task<CMDefaultResponse> DeleteWaterType(int id,int UserID)
        {
            string delqry = "update  mis_watertype set status=0  where id =" + (id);
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

        internal async Task<List<WasteDataType>>GetWasteType( int facility_Id)
        {
            string delqry = "SELECT id,facility_id,name,wastetype,description,createdAt,UpdatedAt FROM mis_wastetype WHERE facility_id = " + facility_Id + " and status=1;";
           List< WasteDataType>Data= await Context.GetData<WasteDataType>(delqry).ConfigureAwait(false);
            return Data;
        }

        // Incident type CRUD 
        internal async Task<CMIncidentType> GetIncidentType(int id)
        {
            string myQuery = $"SELECT id, incidenttype,  status FROM incidenttype WHERE id = " + id+" and status=1;";
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
                if(Result!=null &&   Result.addedAt!=null)
                Result.addedAt = (DateTime) await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, Result.addedAt);
                if (Result != null && Result.updatedAt != null)
                    Result.updatedAt= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, Result.updatedAt);
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

        internal async Task<CMGetMisWaterData> GetWaterDataById(int id,string facilitytimeZone)
        {
            CMGetMisWaterData item = new CMGetMisWaterData();
            string myQuery = "SELECT M.id, M.plantId as facilityID, f.name as facilityName , date, waterTypeId, M.description, debitQty, creditQty, concat(added.firstname,' ',added.lastname)  as addedBy, addedAt,concat(updated.firstname,' ',updated.lastname)  as updatedBy, M.updatedAt FROM mis_waterdata M left join users added on added.id = M.addedBy " +
                " left join users updated on updated.id = M.updatedBy left join facilities f on f.id = M.plantId  where M.isActive = 1 and M.id = " + id+";";
            List<CMGetMisWaterData> result = await Context.GetData<CMGetMisWaterData>(myQuery).ConfigureAwait(false);
            if(result.Count == 0)
            {
                return item;
            }
            foreach ( var results in result)
            {
                if(results!=null && results.AddedAt!=null) 
                results.AddedAt= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.AddedAt);
                if (results != null && results.Date != null)
                    results.Date= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, results.Date);
                if (results != null && results.UpdatedAt != null)
                    results.UpdatedAt= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.UpdatedAt);
            }
            return result[0];
        }

        internal async Task<List<CMGetMisWaterData>> GetWaterDataList(DateTime fromDate, DateTime toDate, string facilitytimeZone)
        {
            string myQuery = "SELECT M.id, M.plantId as facilityID,f.name as facilityName, date, waterTypeId, M.description, debitQty, creditQty, concat(added.firstname,' ',added.lastname)  as addedBy, addedAt,concat(updated.firstname,' ',updated.lastname)  as updatedBy, M.updatedAt FROM mis_waterdata M left join users added on added.id = M.addedBy " +
                " left join users updated on updated.id = M.updatedBy left join facilities f on f.id = M.plantId  where isActive = 1 and date between '" + fromDate.ToString("yyyy-MM-dd") +"' and '"+ toDate.ToString("yyyy-MM-dd") + "' ;";

            List<CMGetMisWaterData> result = await Context.GetData<CMGetMisWaterData>(myQuery).ConfigureAwait(false);
            foreach(var results in result)
            {
                if (results != null && results.AddedAt != null)
                    results.AddedAt= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.AddedAt);
                if (results != null && results.Date != null)
                    results.Date= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, results.Date);
                if (results != null && results.UpdatedAt != null)
                    results.UpdatedAt= (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)results.UpdatedAt);
            }
            return result;
        }
        internal async Task<CMDefaultResponse> CreateWaterData(CMMisWaterData request, int userId)
        {
            string myQuery = $"INSERT INTO mis_waterdata(plantId, date, waterTypeId, description, debitQty, creditQty, addedBy, addedAt,consumeTypeId) VALUES " +
                             $"({request.facilityID}, '{request.Date.ToString("yyyy-MM-dd")}', {request.WaterTypeId}, '{request.Description}', {request.DebitQty}, {request.CreditQty}, {userId}, '{UtilsRepository.GetUTCTime()}',{request.consumeType}); " +
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

                internal async Task<List<CMWasteData>> GetWasteDataList(DateTime fromDate, DateTime toDate)
        {
            string SelectQ = "select * from Waste_data where isActive = 1 and DATE_FORMAT(Created_At,'%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "'";
            List<CMWasteData> ListResult = await Context.GetData<CMWasteData>(SelectQ).ConfigureAwait(false);
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
            string insertQuery = $"INSERT INTO waste_data (" +
                                 $"Solid_Waste, E_Waste, Battery_Waste, Solar_Module_Waste, " +
                                 $"Haz_Waste_Oil, Haz_Waste_Grease, Haz_Solid_Waste, " +
                                 $"Haz_Waste_Oil_Barrel_Generated, Solid_Waste_Disposed, " +
                                 $"E_Waste_Disposed, Battery_Waste_Disposed, Solar_Module_Waste_Disposed, " +
                                 $"Haz_Waste_Oil_Disposed, Haz_Waste_Grease_Disposed, " +
                                 $"Haz_Solid_Waste_Disposed, Haz_Waste_Oil_Barrel_Disposed, " +
                                 $"Created_By, Created_At" +
                                 $") VALUES (" +
                                 $"{request.Solid_Waste}, {request.E_Waste}, {request.Battery_Waste}, {request.Solar_Module_Waste}, " +
                                 $"{request.Haz_Waste_Oil}, {request.Haz_Waste_Grease}, {request.Haz_Solid_Waste}, " +
                                 $"{request.Haz_Waste_Oil_Barrel_Generated}, {request.Solid_Waste_Disposed}, " +
                                 $"{request.E_Waste_Disposed}, {request.Battery_Waste_Disposed}, {request.Solar_Module_Waste_Disposed}, " +
                                 $"{request.Haz_Waste_Oil_Disposed}, {request.Haz_Waste_Grease_Disposed}, " +
                                 $"{request.Haz_Solid_Waste_Disposed}, {request.Haz_Waste_Oil_Barrel_Disposed}, " +
                                 $"{UserID}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                                 $"); SELECT LAST_INSERT_ID();";
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
                                    $"Solid_Waste = {request.Solid_Waste}, " +
                                    $"E_Waste = {request.E_Waste}, " +
                                    $"Battery_Waste = {request.Battery_Waste}, " +
                                    $"Solar_Module_Waste = {request.Solar_Module_Waste}, " +
                                    $"Haz_Waste_Oil = {request.Haz_Waste_Oil}, " +
                                    $"Haz_Waste_Grease = {request.Haz_Waste_Grease}, " +
                                    $"Haz_Solid_Waste = {request.Haz_Solid_Waste}, " +
                                    $"Haz_Waste_Oil_Barrel_Generated = {request.Haz_Waste_Oil_Barrel_Generated}, " +
                                    $"Solid_Waste_Disposed = {request.Solid_Waste_Disposed}, " +
                                    $"E_Waste_Disposed = {request.E_Waste_Disposed}, " +
                                    $"Battery_Waste_Disposed = {request.Battery_Waste_Disposed}, " +
                                    $"Solar_Module_Waste_Disposed = {request.Solar_Module_Waste_Disposed}, " +
                                    $"Haz_Waste_Oil_Disposed = {request.Haz_Waste_Oil_Disposed}, " +
                                    $"Haz_Waste_Grease_Disposed = {request.Haz_Waste_Grease_Disposed}, " +
                                    $"Haz_Solid_Waste_Disposed = {request.Haz_Solid_Waste_Disposed}, " +
                                    $"Haz_Waste_Oil_Barrel_Disposed = {request.Haz_Waste_Oil_Barrel_Disposed}, " +
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
    }

}
