using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.MC;
using CMMSAPIs.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.MC
{
    public interface IMCBS
    {
        public Task<List<CMMC>> GetCMList(CMMCFilter request);
        public Task<CMDefaultResponse> CreateMCPlan(CMMCPlan request);
        public Task<CMDefaultResponse> ApproveMCPlan(CMApproval request);
        public Task<CMDefaultResponse> RejectMCPlan(CMApproval request);
        public Task<CMDefaultResponse> StartMCExecution(int id);
        public Task<CMDefaultResponse> MCExecution(CMMCExecution request);
        public Task<CMDefaultResponse> CompleteMCExecution(CMMCExecution request);
        public Task<CMDefaultResponse> ApproveMCExecution(CMApproval request);
        public Task<CMDefaultResponse> RejectMCExecution(CMApproval request);
    }
    public class MCBS : IMCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public MCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMMC>> GetCMList(CMMCFilter request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.GetCMList(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateMCPlan(CMMCPlan request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.CreateMCPlan(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveMCPlan(CMApproval request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.ApproveMCPlan(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectMCPlan(CMApproval request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.RejectMCPlan(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartMCExecution(int id)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.StartMCExecution(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> MCExecution(CMMCExecution request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.MCExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CompleteMCExecution(CMMCExecution request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.CompleteMCExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveMCExecution(CMApproval request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.ApproveMCExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectMCExecution(CMApproval request)
        {
            try
            {
                using (var repos = new MCRepository(getDB))
                {
                    return await repos.RejectMCExecution(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
