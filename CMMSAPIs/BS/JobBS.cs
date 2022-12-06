﻿using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models;

namespace CMMSAPIs.BS
{
    public interface IJobBS
    {
        Task<List<Job>> GetJobList(int facility_id);
        Task<List<Job>> GetJobDetail(int job_id);
        Task<List<Job>> CreateNewJob();
        Task<List<Job>> UpdateJob();

    }

    public class JobBS : IJobBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JobBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<Job>> GetJobList(int facility_id)
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

        public async Task<List<Job>> GetJobDetail(int job_id)
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

        public async Task<List<Job>> CreateNewJob()
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

        public async Task<List<Job>> UpdateJob()
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
