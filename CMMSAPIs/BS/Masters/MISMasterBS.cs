using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Masters;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Hosting;

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
        Task<Responsibility> GetResponsibilityID(int id);
        Task<CMDefaultResponse> CreateResponsibility(Responsibility request, int UserId);
        Task<CMDefaultResponse> UpdateResponsibility(Responsibility request, int UserId);
        Task<CMDefaultResponse> DeleteResponsibility(int id);
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
        public async Task<Responsibility> GetResponsibilityID(int id)
        {
            try
            {
                using (var repos = new MISMasterRepository(getDB))
                {
                    return await repos.GetResponsibilityID(id);
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

    }
}
