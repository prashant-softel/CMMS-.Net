using CMMSAPIs.Helper;
using CMMSAPIs.Models.Grievance;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Grievance;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrievanceSummaryReport = CMMSAPIs.Models.Grievance.GrievanceSummaryReport;

namespace CMMSAPIs.BS.Grievance
{
    public interface IGrievanceBS
    {
        Task<List<CMGrievance>> GetGrievanceList(string facilityId, string startDate, string endDate, int selfView, string facilityTimeZone);
        Task<CMGrievance> GetGrievanceDetails(int id, int facilityId, string facilityTimeZone);
        Task<CMDefaultResponse> CreateGrievance(CMCreateGrievance request, int userID, string facilityId, string facilityTimeZone, int status);
        Task<CMDefaultResponse> UpdateGrievance(CMUpdateGrievance request, int userID, string facilityId, string facilityTimeZone);
        Task<CMDefaultResponse> DeleteGrievance(int id, int userID, string facilityId, string facilityTimezone);
        Task<CMDefaultResponse> CloseGrievance(CMGrievance request, int userID, string facilityId, string facilityTimezone);
        Task<List<GrievanceSummaryReport>> GrievanceSummaryReport(string facilityId, string fromDate, string toDate);

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

        public async Task<List<CMGrievance>> GetGrievanceList(string facilityId, string startDate, string endDate, int selfView, string facilityTime)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.GetGrievanceList(facilityId, startDate, endDate, selfView, facilityTime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<CMGrievance> GetGrievanceDetails(int id, int facilityId, string facilityTime)
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

        public async Task<CMDefaultResponse> CreateGrievance(CMCreateGrievance request, int userID, string facilityId, string facilityTime, int status)
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

        public async Task<CMDefaultResponse> UpdateGrievance(CMUpdateGrievance request, int userID, string facilityId, string facilityTime)
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

        public async Task<CMDefaultResponse> DeleteGrievance(int id, int userID, string facilityId, string facilityTime)
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

        public async Task<CMDefaultResponse> CloseGrievance(CMGrievance request, int userID, string facilityId, string facilityTime)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.CloseGrievance(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GrievanceSummaryReport>> GrievanceSummaryReport(string facilityId, string fromDate, string toDate)
        {
            try
            {
                using (var repos = new GrievanceRepository(getDB, _environment))
                {
                    return await repos.GrievanceSummaryReport(facilityId, fromDate, toDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }

}

