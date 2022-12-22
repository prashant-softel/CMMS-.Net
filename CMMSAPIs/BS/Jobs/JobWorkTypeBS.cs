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
        Task<List<CMJobWorkType>> GetJobWorkTypeList();
        Task<List<CMJobWorkType>> CreateJobWorkType();
        Task<List<CMJobWorkType>> UpdateJobWorkType();
        Task<List<CMJobWorkType>> DeleteJobWorkType();

        /*
         * Tools Associated to Work Type CRUD Operation
        */
        Task<List<CMJobWorkTypeTool>> GetJobWorkTypeToolList();
        Task<List<CMMasterTool>> GetMasterToolList();
        Task<List<CMJobWorkTypeTool>> CreateJobWorkTypeTool();
        Task<List<CMJobWorkTypeTool>> UpdateJobWorkTypeTool();
        Task<List<CMJobWorkTypeTool>> DeleteJobWorkTypeTool();
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
        public async Task<List<CMJobWorkType>> GetJobWorkTypeList()
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

        public async Task<List<CMJobWorkType>> CreateJobWorkType()
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

        public async Task<List<CMJobWorkType>> UpdateJobWorkType()
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

        public async Task<List<CMJobWorkType>> DeleteJobWorkType()
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


        public async Task<List<CMJobWorkTypeTool>> GetJobWorkTypeToolList()
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

        public async Task<List<CMMasterTool>> GetMasterToolList()
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

        public async Task<List<CMJobWorkTypeTool>> CreateJobWorkTypeTool()
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

        public async Task<List<CMJobWorkTypeTool>> UpdateJobWorkTypeTool()
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

        public async Task<List<CMJobWorkTypeTool>> DeleteJobWorkTypeTool()
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
