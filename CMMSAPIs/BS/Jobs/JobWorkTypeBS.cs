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
        Task<List<CMJobWorkType>> GetJobWorkTypeList(string categoryIds);
        Task<CMDefaultResponse> CreateJobWorkType(CMADDJobWorkType request, int userID);
        Task<CMDefaultResponse> UpdateJobWorkType(CMADDJobWorkType request, int userID);
        Task<CMDefaultResponse> DeleteJobWorkType(int id);

        /*
         * Tools Associated to Work Type CRUD Operation
        */
        Task<List<CMJobWorkTypeTool>> GetJobWorkTypeToolList(int jobId);
        Task<List<CMMasterTool>> GetMasterToolList(string worktypeIds );
        Task<CMDefaultResponse> CreateMasterTool(CMADDJobWorkTypeTool request, int userID);
        Task<CMDefaultResponse> UpdateMasterTool(CMADDJobWorkTypeTool request , int userID);
        Task<CMDefaultResponse> DeleteMasterTool(int id);
        Task<CMDefaultResponse> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request);
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
        public async Task<List<CMJobWorkType>> GetJobWorkTypeList(string categoryIds)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetJobWorkTypeList(categoryIds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateJobWorkType(CMADDJobWorkType request, int userID)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.CreateJobWorkType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateJobWorkType(CMADDJobWorkType request, int userID)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.UpdateJobWorkType(request, userID);
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

        public async Task<List<CMMasterTool>> GetMasterToolList(string worktypeIds)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.GetMasterToolList(worktypeIds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateMasterTool(string name, int userID)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.CreateMasterTool(name, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateMasterTool(CMADDJobWorkTypeTool request, int userID)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.UpdateMasterTool( request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteMasterTool(int id)
        {
            try
            {
                using (var repos = new JobWorkTypeRepository(getDB))
                {
                    return await repos.DeleteMasterTool(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request)
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
