using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.MISEvaluation
{
    public interface IMISEvaluationBS
    {
        Task<List<CMEvaluation>> GetEvaluationList(string facility_Id, DateTime fromDate, DateTime toDate, int selfView, string facilitytimeZone);
        public Task<CMEvaluation> GetEvaluationDetails(int planId, string facilitytimeZone);
        Task<CMDefaultResponse> CreateEvaluationPlan(CMEvaluationCreate request, int userID);
        Task<CMDefaultResponse> UpdateEvaluationPlan(List<CMEvaluationUpdate> request, int userId, string facilitytimeZone);
        Task<CMDefaultResponse> ApproveEvaluationPlan(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectEvaluationPlan(CMApproval request, int userID);
        Task<CMDefaultResponse> DeleteEvaluationPlan(int id, int userID);


    }
    public class MISEvaluationBS : IMISEvaluationBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;
        public MISEvaluationBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;

        }


        #region source of observation


        public async Task<CMDefaultResponse> CreateEvaluationPlan(CMEvaluationCreate request, int userID)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.CreateEvaluationPlan(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateEvaluationPlan(List<CMEvaluationUpdate> request, int userId, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.UpdateEvaluationPlan(request, userId, facilitytimeZone);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<CMEvaluation>> GetEvaluationList(string facility_Id, DateTime fromDate, DateTime toDate, int selfView, string facilitytimeZone)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.GetEvaluationList(facility_Id, fromDate, toDate, facilitytimeZone);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMEvaluation> GetEvaluationDetails(int planId, string facilitytime)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.GetEvaluationDetails(planId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveEvaluationPlan(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.ApproveEvaluationPlan(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> RejectEvaluationPlan(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.RejectEvaluationPlan(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteEvaluationPlan(int id, int userID)
        {
            try
            {
                using (var repos = new MISEvaluationRepository(getDB))
                {
                    return await repos.DeleteEvaluationPlan(id, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}


#endregion