using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Masters
{
    public interface AttendeceBS
    {
        Task<CMDefaultResponse> CreateAttendance(CMCreateAttendence requset, int userID);
        Task<object> GetAttendanceByDetails(int facility_id, DateTime date);

        Task<List<CMGetAttendenceList>> GetAttendanceList(int facility_id, int year);
        Task<object> GetAttendanceByDetailsByMonth(int facility_id, DateTime from_date, DateTime to_date);
        Task<CMDefaultResponse> UpdateAttendance(CMCreateAttendence requset, int userID);
    }
    public class _AttendeceBS : AttendeceBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;
        public _AttendeceBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;

        }
        public async Task<CMDefaultResponse> CreateAttendance(CMCreateAttendence requset, int userID)
        {
            try
            {
                using (var repos = new AttendenceRepository(getDB))
                {
                    return await repos.CreateAttendance(requset, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> GetAttendanceByDetails(int facility_id, DateTime date)
        {

            try
            {
                using (var repos = new AttendenceRepository(getDB))
                {
                    return await repos.GetAttendanceByDetails(facility_id, date);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<CMGetAttendenceList>> GetAttendanceList(int facility_id, int year)

        {
            try
            {
                using (var repos = new AttendenceRepository(getDB))
                {
                    return await repos.GetAttendanceList(facility_id, year);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> GetAttendanceByDetailsByMonth(int facility_id, DateTime from_date, DateTime to_date)
        {
            try
            {
                using (var repos = new AttendenceRepository(getDB))
                {
                    return await repos.GetAttendanceByDetailsByMonth(facility_id, from_date, to_date);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateAttendance(CMCreateAttendence requset, int userID)
        {
            try
            {
                using (var repos = new AttendenceRepository(getDB))
                {
                    return await repos.UpdateAttendance(requset, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}