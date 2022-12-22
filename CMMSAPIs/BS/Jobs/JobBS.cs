using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Models.Jobs;

namespace CMMSAPIs.BS.Jobs
{
    public interface IJobBS
    {
        Task<List<CMJob>> GetJobList(int facility_id);
        Task<List<CMJob>> GetJobDetail(int job_id);
        Task<List<CMJob>> CreateNewJob();
        Task<List<CMJob>> UpdateJob();

    }

    public class JobBS : IJobBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JobBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMJob>> GetJobList(int facility_id)
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.GetJobList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJob>> GetJobDetail(int job_id)
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

        public async Task<List<CMJob>> CreateNewJob()
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.CreateNewJob();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJob>> UpdateJob()
        {
            try
            {
                using (var repos = new JobRepository(getDB))
                {
                    return await repos.UpdateJob();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
