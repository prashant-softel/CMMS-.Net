using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CMMSAPIs.BS.JC
{
    public interface IJCBS
    {
        Task<List<CMJCList>> GetJCList(int facility_id);
        Task<List<CMJCDetail>> GetJCDetail(int jc_id);
        Task<int> CreateJC(CMJCCreate request);
        Task<List<CMDefaultResponse>> UpdateJC(CMJCUpdate request);
        Task<List<CMDefaultResponse>> CloseJC(CMJCClose request);
        Task<List<CMDefaultResponse>> ApproveJC(CMApproval request);
        Task<List<CMDefaultResponse>> RejectJC(CMApproval request);
    }

    public class JCBS : IJCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMJCList>> GetJCList(int facility_id)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJCDetail>> GetJCDetail(int jc_id)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCDetail(jc_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CreateJC(CMJCCreate request)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.CreateJC(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> UpdateJC(CMJCUpdate request)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.UpdateJC(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> CloseJC(CMJCClose request)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.CloseJC(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> ApproveJC(CMApproval request)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.ApproveJC(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> RejectJC(CMApproval request)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.RejectJC(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
