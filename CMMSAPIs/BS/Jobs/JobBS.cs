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
        Task<List<CMJobModel>> GetJobList(int facility_id, int userId);
        Task<List<CMJobView>> GetJobDetail(int job_id);
        Task<int> CreateNewJob(CMCreateJob request);
        Task<CMDefaultResponse> ReAssignJob(int job_id, int user_id, int changed_by);
        Task<CMDefaultResponse> CancelJob(int job_id, int user_id, string Cancelremark);
        Task<CMDefaultResponse> LinkToPTW(int job_id, int ptw_id);
    }

    public class JobBS : IJobBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JobBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMJobModel>> GetJobList(int facility_id, int userId)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.GetJobList(facility_id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJobView>> GetJobDetail(int job_id)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.GetJobDetail(job_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CreateNewJob(CMCreateJob request)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.CreateNewJob(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<CMDefaultResponse> ReAssignJob(int job_id, int user_id, int changed_by)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.ReAssignJob(job_id, user_id, changed_by);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CancelJob(int job_id, int user_id, string Cancelremark)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.CancelJob(job_id, user_id, Cancelremark);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> LinkToPTW(int job_id, int ptw_id)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.LinkToPTW(job_id, ptw_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
