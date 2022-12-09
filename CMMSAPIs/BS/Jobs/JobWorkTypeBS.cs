using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Models.Jobs;

namespace CMMSAPIs.BS.Jobs
{
    public interface IJobWorkTypeBS
    {
        /*
         * Work Type CRUD Operation
        */
        Task<List<JobWorkType>> GetJobWorkTypeList();
        Task<List<JobWorkType>> CreateJobWorkType();
        Task<List<JobWorkType>> UpdateJobWorkType();
        Task<List<JobWorkType>> DeleteJobWorkType();

        /*
         * Tools Associated to Work Type CRUD Operation
        */
        Task<List<JobWorkTypeTool>> GetJobWorkTypeToolList();
        Task<List<MasterTool>> GetMasterToolList();
        Task<List<JobWorkTypeTool>> CreateJobWorkTypeTool();
        Task<List<JobWorkTypeTool>> UpdateJobWorkTypeTool();
        Task<List<JobWorkTypeTool>> DeleteJobWorkTypeTool();
    }

    public class JobWorkTypeBS : IJobWorkTypeBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JobWorkTypeBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        /*
         * Work Type CRUD Operation
        */
        public async Task<List<JobWorkType>> GetJobWorkTypeList()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetJobWorkTypeList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<JobWorkType>> CreateJobWorkType()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.CreateJobWorkType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<JobWorkType>> UpdateJobWorkType()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.UpdateJobWorkType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<JobWorkType>> DeleteJobWorkType()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.DeleteJobWorkType();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /*
         * Tools Associated to Work Type CRUD Operation
        */


        public async Task<List<JobWorkTypeTool>> GetJobWorkTypeToolList()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetJobWorkTypeToolList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MasterTool>> GetMasterToolList()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetMasterToolList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<JobWorkTypeTool>> CreateJobWorkTypeTool()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.CreateJobWorkTypeTool();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<JobWorkTypeTool>> UpdateJobWorkTypeTool()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.UpdateJobWorkTypeTool();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<JobWorkTypeTool>> DeleteJobWorkTypeTool()
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.DeleteJobWorkTypeTool();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
