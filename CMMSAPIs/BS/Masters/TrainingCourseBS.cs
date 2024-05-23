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
    public interface TrainingCourseBS
    {
        Task<CMDefaultResponse> CreateTrainingCourse(CMTrainingCourse request, int userID);
        Task<List<CMTrainingCourse>> GetCourseList(int facility_id);
        Task<CMDefaultResponse> UpdateCourseList(CMTrainingCourse request, int userID);
        Task<CMDefaultResponse> DeleteCourseList(int id, int userID);
        Task<CMDefaultResponse> CreateScheduleCourse();
        Task<CMDefaultResponse> GetScheduleCourse();

        Task<CMDefaultResponse> ExecuteScheduleCourse();
    }
    public class Traningbs : TrainingCourseBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;
        public Traningbs(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;

        }

        public async Task<CMDefaultResponse> CreateTrainingCourse(CMTrainingCourse request, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.CreateTrainingCourse(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMTrainingCourse>> GetCourseList(int facility_id)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetCourseList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateCourseList(CMTrainingCourse request, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.UpdateCourseList(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteCourseList(int id, int userID)
        {

            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.DeleteCourseList(id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateScheduleCourse()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.CreateScheduleCourse();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> GetScheduleCourse()
        {

            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetScheduleCourse();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ExecuteScheduleCourse()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.ExecuteScheduleCourse();
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
