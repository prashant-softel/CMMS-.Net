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
        Task<CMDefaultResponse> CreateScheduleCourse(TrainingSchedule request, int userID);
        Task<List<GETSCHEDULE>> GetScheduleCourseList(int facility_id, DateTime from_date, DateTime to_date);
        Task<List<GETSCHEDULEDETAIL>> GetScheduleCourseDetail(int schedule_id);

        Task<CMDefaultResponse> ExecuteScheduleCourse(GETSCHEDULEDETAIL requset);
        Task<List<CMTRAININGCATE>> GetTrainingCategorty();
        Task<List<CMTrainingSummary>> GetTrainingReportByCategory(int facility_id, DateTime fromDate, DateTime toDate);
        Task<CMDefaultResponse> CreateTrainingCategorty(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> UpdateTrainingCategorty(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> DeleteTrainingCategorty(int id, int userID);
        Task<List<CMTRAININGCATE>> GetTargetedGroupList();
        Task<CMDefaultResponse> CreateTargetedGroup(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> UpdateTargetedGroup(CMTRAININGCATE request, int userID);
        Task<CMDefaultResponse> DeleteTargetedGroup(int id, int userID);
        Task<CMDefaultResponse> ApproveScheduleCourse(CMApproval request, int userid);
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

        public async Task<CMDefaultResponse> CreateScheduleCourse(TrainingSchedule request, int userID)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.CreateScheduleCourse(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GETSCHEDULE>> GetScheduleCourseList(int facility_id, DateTime from_date, DateTime to_date)
        {

            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetScheduleCourseList(facility_id, from_date, to_date);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ExecuteScheduleCourse(GETSCHEDULEDETAIL requset)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.ExecuteScheduleCourse(requset);
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

        public async Task<List<CMTrainingSummary>> GetTrainingReportByCategory(int facility_id, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetTrainingReportByCategory(facility_id, fromDate, toDate);

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

        public async Task<List<GETSCHEDULEDETAIL>> GetScheduleCourseDetail(int schedule_id)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.GetScheduleCourseDetail(schedule_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveScheduleCourse(CMApproval request, int userid)
        {
            try
            {
                using (var repos = new TrainingRepository(getDB))
                {
                    return await repos.ApproveScheduleCourse(request, userid);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

