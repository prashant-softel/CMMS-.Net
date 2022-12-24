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
        Task<List<JobWorkTypeModel>> GetJobWorkTypeList();
        Task<List<JobWorkTypeModel>> CreateJobWorkType();
        Task<List<JobWorkTypeModel>> UpdateJobWorkType();
        Task<List<JobWorkTypeModel>> DeleteJobWorkType();

        /*
         * Tools Associated to Work Type CRUD Operation
        */
        Task<List<JobWorkTypeToolModel>> GetJobWorkTypeToolList();
        Task<List<MasterToolModel>> GetMasterToolList();
        Task<List<JobWorkTypeToolModel>> CreateJobWorkTypeTool();
        Task<List<JobWorkTypeToolModel>> UpdateJobWorkTypeTool();
        Task<List<JobWorkTypeToolModel>> DeleteJobWorkTypeTool();
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
        public async Task<List<JobWorkTypeModel>> GetJobWorkTypeList()
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

        public async Task<List<JobWorkTypeModel>> CreateJobWorkType()
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

        public async Task<List<JobWorkTypeModel>> UpdateJobWorkType()
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

        public async Task<List<JobWorkTypeModel>> DeleteJobWorkType()
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


        public async Task<List<JobWorkTypeToolModel>> GetJobWorkTypeToolList()
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

        public async Task<List<MasterToolModel>> GetMasterToolList()
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

        public async Task<List<JobWorkTypeToolModel>> CreateJobWorkTypeTool()
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

        public async Task<List<JobWorkTypeToolModel>> UpdateJobWorkTypeTool()
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

        public async Task<List<JobWorkTypeToolModel>> DeleteJobWorkTypeTool()
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
