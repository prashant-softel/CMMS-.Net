using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Masters;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Masters
{
    public interface TrainingCourseBS
    {
        Task<CMDefaultResponse> CreateTrainingCourse();
        Task<CMDefaultResponse> GetCourseList();
        Task<CMDefaultResponse> UpdateCourseList();
        Task<CMDefaultResponse> CreateScheduleCourse();
        Task<CMDefaultResponse> GetScheduleCourse();
        Task<CMDefaultResponse> DeleteCourseList();
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

        public async Task<CMDefaultResponse> CreateTrainingCourse()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.CreateTrainingCourse();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> GetCourseList()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetCourseList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateCourseList()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.UpdateCourseList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteCourseList()
        {

            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.DeleteCourseList();

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
