using CMMSAPIs.Helper;
using CMMSAPIs.Repositories.Grievance;
using CMMSAPIs.Models.Grievance;
using CMMSAPIs.Models.Utils;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using CMMSAPIs.Models.Calibration;
using Ubiety.Dns.Core;

namespace CMMSAPIs.BS.Grievance
{
    public interface IGrievanceBS
    {
        Task<List<CMGrievance>> GetGrievanceList(string facilityId, string status, string startDate, string endDate, int selfView, int userID);
        Task<CMGrievance> GetGrievanceDetails(int id);
        Task<CMDefaultResponse> CreateGrievance(CMCreateGrievance request, int userID);
        Task<CMDefaultResponse> UpdateGrievance(CMUpdateGrievance request, int userID);
        Task<CMDefaultResponse> DeleteGrievance(int id, int userID);
    }
    public class GrievanceBS : IGrievanceBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;
        public GrievanceBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

        public async Task<List<CMGrievance>> GetGrievanceList(string facilityId, string status, string startDate, string endDate, int selfView, int userID)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.GetGrievanceList(facilityId, status, startDate, endDate, selfView, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      

        public async Task<CMGrievance> GetGrievanceDetails(int id)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.GetGrievanceDetails(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateGrievance(CMCreateGrievance request, int userID)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.CreateGrievance(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateGrievance(CMUpdateGrievance request, int userID)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.UpdateGrievance(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteGrievance(int id, int userID)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.DeleteGrievance(id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        }

    }

