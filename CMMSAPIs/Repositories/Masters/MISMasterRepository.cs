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
            String myqry = $"INSERT INTO BODYPARTS(sequence_no,id,bodyparts, description,status, Createdby,Createdat) VALUES " +
                                $"('{request.sequence_no}','{request.id}','{request.bodyparts}','{request.description}','{request.Status} ',  {UserId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "RECORD Added");
        }
        internal async Task<CMDefaultResponse> UpdateBodyParts(BODYPARTS request, int UserId)
        {
            string updateQry = "UPDATE BODYPARTS SET ";
            if (request.sequence_no >= 0)
                updateQry += $"Sequence_no = '{request.sequence_no}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedAt ='{UtilsRepository.GetUTCTime()}' , updatedBy = '{UserId}' WHERE Sequence_no = {request.sequence_no};";
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
        internal async Task<Responsibility> GetResponsibilityID(int id)
        {
            String getqry = $"Select * from   Responsibility where id =" + (id);
            List<Responsibility> Body = await Context.GetData<Responsibility>(getqry).ConfigureAwait(false);
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

        // Incident type CRUD 
        internal async Task<CMIncidentType> GetIncidentType(int id)
        {
            string myQuery = $"SELECT id, incidenttype,  status FROM incidenttype WHERE id = " + id+" and status=1;";
            List<CMIncidentType> result = await Context.GetData<CMIncidentType>(myQuery).ConfigureAwait(false);
            //Add history
            return result[0];
        }
        internal async Task<List<CMIncidentType>> GetIncidentTypeList()
        {
            string myQuery = "SELECT id, incidenttype FROM incidenttype WHERE status=1 ";
            List<CMIncidentType> result = await Context.GetData<CMIncidentType>(myQuery).ConfigureAwait(false);
            return result;
        }



        internal async Task<CMDefaultResponse> CreateIncidentType(CMIncidentType request, int userId)
        {
            string myQuery = $"INSERT INTO incidenttype(incidenttype,  status, addedBy, addedAt) VALUES " +
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


    }

}
