using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.BS.Jobs
{
    public interface IJobWorkTypeBS
    {
        /*
         * Work Type CRUD Operation
        */
        Task<List<CMJobWorkType>> GetJobWorkTypeList();
        Task<int> CreateJobWorkType(CMADDJobWorkType request);
        Task<CMDefaultResponse> UpdateJobWorkType(CMUpdateJobWorkType request);
        Task<CMDefaultResponse> DeleteJobWorkType(int id);

        /*
         * Tools Associated to Work Type CRUD Operation
        */
        Task<List<CMJobWorkTypeTool>> GetJobWorkTypeToolList(int jobId);
        Task<List<CMMasterTool>> GetMasterToolList(int id);
        Task<int> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request);
        Task<CMDefaultResponse> UpdateJobWorkTypeTool(CMUpdateJobWorkTypeTool request);
        Task<CMDefaultResponse> DeleteJobWorkTypeTool(int id);
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

        public async Task<int> CreateJobWorkType(CMADDJobWorkType request)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.CreateJobWorkType(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateJobWorkType(CMUpdateJobWorkType request)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.UpdateJobWorkType(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteJobWorkType(int id)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.DeleteJobWorkType(id);
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


        public async Task<List<CMJobWorkTypeTool>> GetJobWorkTypeToolList(int jobId)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetJobWorkTypeToolList(jobId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMasterTool>> GetMasterToolList(int id)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetMasterToolList(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.CreateJobWorkTypeTool(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateJobWorkTypeTool(CMUpdateJobWorkTypeTool request)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.UpdateJobWorkTypeTool(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteJobWorkTypeTool(int id)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.DeleteJobWorkTypeTool(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
