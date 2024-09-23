using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Models.Jobs;

namespace CMMSAPIs.BS.Jobs
{
    public interface IJobBS
    {
		Task<List<CMJobModel>> GetJobList(string facility_id, string startDate, string endDate, CMMS.CMMS_JobType jobType, bool selfView, int userId, string status,string facilitytime);
        Task<List<CMJobList>> GetJobListByPermitId(int permitId, string facilitytime);
        Task<CMJobView> GetJobDetails(int job_id,string facilitytime);
        Task<CMDefaultResponse> CreateNewJob(CMCreateJob request, int userId);
        Task<CMDefaultResponse> UpdateJob(CMCreateJob request, int userId);
        Task<CMDefaultResponse> ReAssignJob(int job_id, int assignedTo, int userId);
        Task<CMDefaultResponse> CancelJob(int job_id, int cancelledBy, string Cancelremark);
        //Task<CMDefaultResponse> DeleteJob(int job_id, int userId);
        Task<CMDefaultResponse> LinkToPTW(int job_id, int ptw_id, int userId);
    }

    public class JobBS : IJobBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JobBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMJobModel>> GetJobList(string facility_id, string startDate, string endDate, CMMS.CMMS_JobType jobType, bool selfView, int userId, string status, string facilitytime)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.GetJobList(facility_id, startDate, endDate, jobType, selfView, userId, status,facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJobList>> GetJobListByPermitId(int permitId, string facilitytime)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.GetJobListByPermitId(permitId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMJobView> GetJobDetails(int job_id, string facilitytime)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.GetJobDetails(job_id,facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateNewJob(CMCreateJob request, int userId)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.CreateNewJob(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        public async Task<CMDefaultResponse> UpdateJob(CMCreateJob request, int userId)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.UpdateJob(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ReAssignJob(int job_id, int assignedTo, int updatedBy)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.ReAssignJob(job_id, assignedTo, updatedBy);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CancelJob(int job_id, int cancelledBy, string cancelRemark)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.CancelJob(job_id, cancelledBy, cancelRemark);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*
        public async Task<CMDefaultResponse> DeleteJob(int job_id, int updatedBy)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.DeleteJob(job_id, updatedBy);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        */
        public async Task<CMDefaultResponse> LinkToPTW(int job_id, int ptw_id, int updatedBy)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.LinkToPTW(job_id, ptw_id, updatedBy);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
