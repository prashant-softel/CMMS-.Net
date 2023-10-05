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
        Task<MISTypeObservation> GetTypeOfObservation(int type_id);
        Task<List<MISTypeObservation>> GetTypeOfObservationList();
        Task<CMDefaultResponse> AddTypeOfObservation(MISTypeObservation request, int userId);
        Task<CMDefaultResponse> UpdateTypeOfObservation(MISTypeObservation request, int userId);
        Task<CMDefaultResponse> DeleteTypeOfObservation(int id, int userId);
        Task<MISRiskType> GetRiskType(int risk_id);
        Task<List<MISRiskType>> GetRiskTypeList();
        Task<CMDefaultResponse> CreateRiskType(MISRiskType request, int userId);
        Task<CMDefaultResponse> UpdateRiskType(MISRiskType request, int userId);
        Task<CMDefaultResponse> DeleteRiskType(int id, int userId);
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


    }
}
