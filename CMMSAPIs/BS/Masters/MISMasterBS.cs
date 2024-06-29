using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.MISMasters
{
    public interface IMISMasterBS
    {
        Task<MISSourceOfObservation> GetSourceOfObservation(int source_id);
        Task<List<MISSourceOfObservation>> GetSourceOfObservationList();
        Task<CMDefaultResponse> AddSourceOfObservation(MISSourceOfObservation request, int userId);
        Task<CMDefaultResponse> UpdateSourceOfObservation(MISSourceOfObservation request, int userId);
        Task<CMDefaultResponse> DeleteSourceOfObservation(int id, int userId);
        Task<MISTypeObservation> GetTypeOfObservation(int id);
        Task<List<MISTypeObservation>> GetTypeOfObservationList();
        Task<CMDefaultResponse> AddTypeOfObservation(MISTypeObservation request, int userId);
        Task<CMDefaultResponse> UpdateTypeOfObservation(MISTypeObservation request, int userId);
        Task<CMDefaultResponse> DeleteTypeOfObservation(int id, int userId);
        Task<MISGrievanceType> GetGrievanceType(int id);
        Task<List<MISGrievanceType>> GetGrievanceTypeList();
        Task<CMDefaultResponse> AddGrievanceType(MISGrievanceType request, int userId);
        Task<CMDefaultResponse> UpdateGrievanceType(MISGrievanceType request, int userId);
        Task<CMDefaultResponse> DeleteGrievanceType(int id, int userId);
        Task<MISResolutionLevel> GetResolutionLevel(int id);
        Task<List<MISResolutionLevel>> GetResolutionLevelList();
        Task<CMDefaultResponse> AddResolutionLevel(MISResolutionLevel request, int userId);
        Task<CMDefaultResponse> UpdateResolutionLevel(MISResolutionLevel request, int userId);
        Task<CMDefaultResponse> DeleteResolutionLevel(int id, int userId);
        Task<MISRiskType> GetRiskType(int risk_id);
        Task<List<MISRiskType>> GetRiskTypeList();
        Task<CMDefaultResponse> CreateRiskType(MISRiskType request, int userId);
        Task<CMDefaultResponse> UpdateRiskType(MISRiskType request, int userId);
        Task<CMDefaultResponse> DeleteRiskType(int id, int userId);
        Task<MISCostType> GetCostType(int risk_id);
        Task<List<MISCostType>> GetCostTypeList();
        Task<CMDefaultResponse> CreateCostType(MISCostType request, int userId);
        Task<CMDefaultResponse> UpdateCostType(MISCostType request, int userId);
        Task<CMDefaultResponse> DeleteCostType(int id, int userId);
        Task<List<BODYPARTS>> GetBodyPartsList();
        Task<CMDefaultResponse> CreateBodyParts(BODYPARTS request, int UserId);
        Task<CMDefaultResponse> UpdateBodyParts(BODYPARTS request, int UserId);
        Task<CMDefaultResponse> DeleteBodyParts(int id, int UserId);
        Task<List<Responsibility>> GetResponsibilityList();
        Task<Responsibility> GetResponsibilityID(int id, string facilitytime);
        Task<CMDefaultResponse> CreateResponsibility(Responsibility request, int UserId);
        Task<CMDefaultResponse> UpdateResponsibility(Responsibility request, int UserId);
        Task<CMDefaultResponse> DeleteResponsibility(int id);
        Task<CMIncidentType> GetIncidentType(int id);
        Task<List<CMIncidentType>> GetIncidentTypeList(string facilitytime);
        Task<CMDefaultResponse> CreateIncidentType(CMIncidentType request, int userId);
        Task<CMDefaultResponse> UpdateIncidentType(CMIncidentType request, int userId);
        Task<CMDefaultResponse> DeleteIncidentType(int id, int userId);

        Task<CMGetMisWaterData> GetWaterDataById(int id, string facilitytime);
        Task<List<CMGetMisWaterData>> GetWaterDataList(DateTime fromDate, DateTime toDate, string facilitytime);
        Task<List<CMWaterDataReport>> GetWaterDataReport(DateTime fromDate, DateTime toDate);
        Task<CMDefaultResponse> CreateWaterData(CMMisWaterData request, int userId);
        Task<CMDefaultResponse> UpdateWaterData(CMMisWaterData request, int userId);
        Task<CMDefaultResponse> DeleteWaterData(int id, int userId);
        Task<List<CMGetMisWasteData>> GetWasteDataList(int facility_id, DateTime fromDate, DateTime toDate, int Hazardous, string facilitytimeZone);
        Task<CMDefaultResponse> CreateWasteData(CMWasteData request, int userID);
        Task<CMDefaultResponse> UpdateWasteData(CMWasteData request, int userID);
        Task<CMDefaultResponse> DeleteWasteData(int Id, int UserID);
        Task<CMWasteData> GetWasteDataByID(int Id);
        //changes
        Task<List<WaterDataType>> GetWaterType();
        Task<CMDefaultResponse> CreateWaterType(WaterDataType request, int userId);
        Task<CMDefaultResponse> UpdateWaterType(WaterDataType request, int userId);

        Task<CMDefaultResponse> DeleteWaterType(int Id, int userID);
        Task<List<WaterDataType>> GetWaterTypebyId(int id);

        //changes
        Task<List<WasteDataType>> GetWasteTypeByid(int Type);
        Task<List<WasteDataType>> GetWasteType();
        Task<CMDefaultResponse> CreateWasteType(WasteDataType request, int userId);
        Task<CMDefaultResponse> UpdateWasteType(WasteDataType request, int userId);
        Task<CMDefaultResponse> DeleteWasteType(int Id, int userID);


        Task<List<WaterDataResult>> GetWaterDataListMonthWise(DateTime fromDate, DateTime toDate, int facility_id);
        Task<List<CMWasteDataResult>> GetWasteDataListMonthWise(DateTime fromDate, DateTime toDate, int Hazardous, int facility_id);
        Task<List<WaterDataResult_Month>> GetWaterDataMonthDetail(int Month, int Year, int facility_id);
        Task<List<CMWasteDataResult_Month>> GetWasteDataMonthDetail(int Month, int Year, int Hazardous, int facility_id);
        Task<List<CMChecklistInspectionReport>> GetChecklistInspectionReport(string facility_id, int module_type, DateTime fromDate, DateTime toDate);
        Task<List<CMObservationReport>> GetObservationSheetReport(string facility_id, DateTime fromDate, DateTime toDate);
        Task<List<CMObservationSummary>> GetObservationSummaryReport(string facility_id, string fromDate, string toDate);
        Task<CMDefaultResponse> CloseObservation(CMApproval requset, int userId);
        Task<CMStatutoryCompliance> GetStatutoryComplianceMasterById(int id);
        Task<List<CMStatutoryCompliance>> GetStatutoryComplianceMasterList();
        Task<CMDefaultResponse> CreateStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId);
        Task<CMDefaultResponse> UpdateStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId);
        Task<CMDefaultResponse> DeleteStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId);
        Task<CMDefaultResponse> CreateStatutory(CMCreateStatutory request, int UserId);
        Task<CMDefaultResponse> UpdateStatutory(CMCreateStatutory request, int UserId);
        Task<List<CMStatutory>> GetStatutoryList(int facility_id, string start_date, string end_date);
        Task<List<CMStatutoryHistory>> GetStatutoryHistoryById(int compliance_id);
        Task<CMStatutory> GetStatutoryById(int id);
        Task<CMDefaultResponse> CreateStatusofAppliaction(MISTypeObservation request, int userID);
        Task<CMDefaultResponse> UpdateStatsofAppliaction(MISTypeObservation request, int userID);
        Task<List<MISTypeObservation>> GetStatsofAppliaction();
        Task<CMDefaultResponse> DeleteStatsofAppliaction(int id, int userID);
        Task<CMDefaultResponse> RejectStatutory(CMApprovals request, int userID);
        Task<CMDefaultResponse> ApproveStatutory(CMApprovals request, int userID);
        Task<CMDefaultResponse> CreateDocument(MISTypeObservation request, int userID);
        Task<List<MISTypeObservation>> GetDocument();
        Task<CMDefaultResponse> UpdateDocument(MISTypeObservation request, int userID);
        Task<CMDefaultResponse> DeleteDocument(int id, int userID);
        Task<CMObservationByIdList> GetObservationById(int observation_id);
        Task<List<CMObservation>> GetObservationList(int facility_Id, DateTime fromDate, DateTime toDate);
        Task<CMDefaultResponse> DeleteObservation(int id, int UserID);
        Task<CMDefaultResponse> UpdateObservation(CMObservation request, int UserID);
        Task<CMDefaultResponse> CreateObservation(CMObservation request, int UserID);
    }
    public class MISMasterBS : IMISMasterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;
        public MISMasterBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;

        }


        #region source of observation

        public async Task<MISSourceOfObservation> GetSourceOfObservation(int source_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetSourceOfObservation(source_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MISSourceOfObservation>> GetSourceOfObservationList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetSourceOfObservationList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddSourceOfObservation(MISSourceOfObservation request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.AddSourceOfObservation(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateSourceOfObservation(MISSourceOfObservation request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateSourceOfObservation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteSourceOfObservation(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteSourceOfObservation(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion  //source of observation


        #region type of observation
        public async Task<MISTypeObservation> GetTypeOfObservation(int type_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetTypeOfObservation(type_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MISTypeObservation>> GetTypeOfObservationList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetTypeOfObservationList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddTypeOfObservation(MISTypeObservation request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.AddTypeOfObservation(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateTypeOfObservation(MISTypeObservation request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateTypeOfObservation(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteTypeOfObservation(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteTypeOfObservation(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion  //type of observation

        #region Resolution level
        public async Task<MISResolutionLevel> GetResolutionLevel(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetResolutionLevel(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MISResolutionLevel>> GetResolutionLevelList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetResolutionLevelList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddResolutionLevel(MISResolutionLevel request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.AddResolutionLevel(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateResolutionLevel(MISResolutionLevel request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateResolutionLevel(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteResolutionLevel(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteResolutionLevel(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion  //Resolution leve

        #region GrievanceType
        public async Task<MISGrievanceType> GetGrievanceType(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetGrievanceType(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MISGrievanceType>> GetGrievanceTypeList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetGrievanceTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddGrievanceType(MISGrievanceType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.AddGrievanceType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateGrievanceType(MISGrievanceType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateGrievanceType(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteGrievanceType(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteGrievanceType(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion  //grievance type


        #region risk type
        public async Task<MISRiskType> GetRiskType(int risk_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetRiskType(risk_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MISRiskType>> GetRiskTypeList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetRiskTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateRiskType(MISRiskType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateRiskType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateRiskType(MISRiskType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateRiskType(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteRiskType(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteRiskType(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<MISCostType> GetCostType(int cost_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetCostType(cost_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MISCostType>> GetCostTypeList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetCostTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateCostType(MISCostType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateCostType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateCostType(MISCostType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateCostType(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteCostType(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteCostType(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion //risk type 


        /*
        public async Task<int> eQry(string qry)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.eQry(qry);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        */
        public async Task<List<BODYPARTS>> GetBodyPartsList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetBodyPartsList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateBodyParts(BODYPARTS requset, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                    return await repos.CreateBodyParts(requset, UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateBodyParts(BODYPARTS requset, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                    return await repos.UpdateBodyParts(requset, UserId);
            }
            catch
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteBodyParts(int id, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                    return await repos.DeleteBodyParts(id, UserId);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Responsibility>> GetResponsibilityList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetResponsibilityList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Responsibility> GetResponsibilityID(int id, string facilitytime)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetResponsibilityID(id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateResponsibility(Responsibility request, int UserID)
        {
            try
            {
                using (var response = new MISMasterRepository(getDB))
                    return await response.CreateResponsibility(request, UserID);
            }
            catch
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateResponsibility(Responsibility request, int UserID)
        {
            try
            {
                using (var response = new MISMasterRepository(getDB))
                    return await response.UpdateResponsibility(request, UserID);
            }
            catch
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteResponsibility(int id)
        {
            try
            {
                using (var response = new MISMasterRepository(getDB))
                    return await response.DeleteResponsibility(id);
            }
            catch
            {
                throw;
            }
        }


        // Incident type master
        public async Task<CMIncidentType> GetIncidentType(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetIncidentType(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMIncidentType>> GetIncidentTypeList(string facilitytime)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetIncidentTypeList(facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateIncidentType(CMIncidentType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateIncidentType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateIncidentType(CMIncidentType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateIncidentType(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteIncidentType(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteIncidentType(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // MIS Water data

        public async Task<CMGetMisWaterData> GetWaterDataById(int id, string facilitytime)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterDataById(id, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMGetMisWaterData>> GetWaterDataList(DateTime fromDate, DateTime toDate, string facilitytime)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterDataList(fromDate, toDate, facilitytime);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateWaterData(CMMisWaterData request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateWaterData(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateWaterData(CMMisWaterData request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateWaterData(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteWaterData(int id, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteWaterData(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMWaterDataReport>> GetWaterDataReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterDataReport(fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<WaterDataType>> GetWaterType()
        {
            try

            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateWaterType(WaterDataType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateWaterType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateWaterType(WaterDataType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateWaterType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteWaterType(int id, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteWaterType(id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<WaterDataType>> GetWaterTypebyId(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterTypebyId(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateWasteType(WasteDataType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateWasteType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateWasteType(WasteDataType request, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateWasteType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteWasteType(int id, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteWasteType(id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMGetMisWasteData>> GetWasteDataList(int facility_id, DateTime fromDate, DateTime toDate, int Hazardous, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWasteDataList(facility_id, fromDate, toDate, Hazardous, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMWasteData> GetWasteDataByID(int Id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWasteDataByID(Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateWasteData(CMWasteData request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateWasteData(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateWasteData(CMWasteData request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateWasteData(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteWasteData(int Id, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteWasteData(Id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<WasteDataType>> GetWasteType()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWasteType();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<WaterDataResult>> GetWaterDataListMonthWise(DateTime fromDate, DateTime toDate, int facility_id)

        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterDataListMonthWise(fromDate, toDate, facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMWasteDataResult>> GetWasteDataListMonthWise(DateTime fromDate, DateTime toDate, int Hazardous, int facility_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWasteDataListMonthWise(fromDate, toDate, Hazardous, facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<WaterDataResult_Month>> GetWaterDataMonthDetail(int Month, int Year, int facility_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWaterDataMonthDetail(Month, Year, facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMWasteDataResult_Month>> GetWasteDataMonthDetail(int Month, int Year, int Hazardous, int facility_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWasteDataMonthDetail(Month, Year, Hazardous, facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<WasteDataType>> GetWasteTypeByid(int Type)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetWasteTypeByid(Type);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMChecklistInspectionReport>> GetChecklistInspectionReport(string facility_id, int module_type, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetChecklistInspectionReport(facility_id, module_type, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMObservationReport>> GetObservationSheetReport(string facility_id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetObservationSheetReport(facility_id, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMObservationSummary>> GetObservationSummaryReport(string facility_id, string fromDate, string toDate)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetObservationSummaryReport(facility_id, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseObservation(CMApproval requset, int userId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CloseObservation(requset, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<CMStatutoryCompliance> GetStatutoryComplianceMasterById(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetStatutoryComplianceMasterById(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMStatutoryCompliance>> GetStatutoryComplianceMasterList()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetStatutoryComplianceMasterList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateStatutoryComplianceMaster(request, UserId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateStatutoryComplianceMaster(request, UserId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteStatutoryComplianceMaster(CMStatutoryCompliance request, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteStatutoryComplianceMaster(request, UserId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateStatutory(CMCreateStatutory request, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateStatutory(request, UserId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateStatutory(CMCreateStatutory request, int UserId)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateStatutory(request, UserId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMStatutory>> GetStatutoryList(int facility_id, string start_date, string end_date)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetStatutoryList(facility_id, start_date, end_date);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMStatutoryHistory>> GetStatutoryHistoryById(int compliance_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetStatutoryHistoryById(compliance_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMStatutory> GetStatutoryById(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetStatutoryById(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateStatusofAppliaction(MISTypeObservation request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateStatusofAppliaction(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateStatsofAppliaction(MISTypeObservation request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateStatsofAppliaction(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MISTypeObservation>> GetStatsofAppliaction()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetStatsofAppliaction();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteStatsofAppliaction(int id, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteStatsofAppliaction(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectStatutory(CMApprovals request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.RejectStatutory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveStatutory(CMApprovals request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.ApproveStatutory(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateDocument(MISTypeObservation request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateDocument(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MISTypeObservation>> GetDocument()
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetDocument();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateDocument(MISTypeObservation request, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateDocument(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteDocument(int id, int userID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteDocument(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CreateObservation(CMObservation request, int UserID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.CreateObservation(request, UserID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateObservation(CMObservation request, int UserID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.UpdateObservation(request, UserID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteObservation(int id, int UserID)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.DeleteObservation(id, UserID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMObservation>> GetObservationList(int facility_Id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetObservationList(facility_Id, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMObservationByIdList> GetObservationById(int observation_id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetObservationById(observation_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
