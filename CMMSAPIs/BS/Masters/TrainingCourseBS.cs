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
        Task<List<CMTrainingCourse>> GetCourseList(int facility_id, DateTime from_date, DateTime to_date);
        Task<List<CMTrainingCourse>> GetCourseDetailById(int id);
        Task<CMDefaultResponse> UpdateCourseList(CMTrainingCourse request, int userID);
        Task<CMDefaultResponse> DeleteCourseList(int id, int userID);
        Task<CMDefaultResponse> CreateScheduleCourse();
        Task<CMDefaultResponse> GetScheduleCourse();

        Task<CMDefaultResponse> ExecuteScheduleCourse();
        Task<List<CMTRAININGCATE>> GetTrainingCategorty();
        Task<CMDefaultResponse> CreateTrainingCategorty(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> UpdateTrainingCategorty(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> DeleteTrainingCategorty(int id, int userID);
        Task<List<CMTRAININGCATE>> GetTargetedGroupList();
        Task<CMDefaultResponse> CreateTargetedGroup(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> UpdateTargetedGroup(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> DeleteTargetedGroup(int id, int userID);
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

        public async Task<List<CMTrainingCourse>> GetCourseList(int facility_id, DateTime from_date, DateTime to_date)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetCourseList(facility_id, from_date, to_date);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMTrainingCourse>> GetCourseDetailById(int id)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetCourseDetailById(id);

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
        //

        public async Task<List<CMTRAININGCATE>> GetTrainingCategorty()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetTrainingCategorty();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMDefaultResponse> CreateTrainingCategorty(CMTRAININGCATE request, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.CreateTrainingCategorty(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMDefaultResponse> UpdateTrainingCategorty(CMTRAININGCATE request, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.UpdateTrainingCategorty(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteTrainingCategorty(int id, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.DeleteTrainingCategorty(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMTRAININGCATE>> GetTargetedGroupList()
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetTargetedGroupList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> CreateTargetedGroup(CMTRAININGCATE request, int userID)
        {

            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.CreateTargetedGroup(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> UpdateTargetedGroup(CMTRAININGCATE request, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.UpdateTargetedGroup(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteTargetedGroup(int id, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.DeleteTargetedGroup(id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

