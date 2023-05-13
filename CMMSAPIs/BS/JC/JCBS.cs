﻿using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.BS.JC
{
    public interface IJCBS
    {
        Task<List<CMJCList>> GetJCList(int facility_id);
        Task<List<CMJCDetail>> GetJCDetail(int jc_id);
        Task<CMDefaultResponse> CreateJC(int job_id);
        Task<CMDefaultResponse> UpdateJC(CMJCUpdate request, int userID);
        Task<CMDefaultResponse> CloseJC(CMJCClose request);
        Task<CMDefaultResponse> ApproveJC(CMJCApprove request);
        Task<CMDefaultResponse> RejectJC(CMJCReject request);
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

        public async Task<CMDefaultResponse> CreateJC(int job_id)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.CreateJC(job_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateJC(CMJCUpdate request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.UpdateJC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseJC(CMJCClose request)
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

        public async Task<CMDefaultResponse> ApproveJC(CMJCApprove request)
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

        public async Task<CMDefaultResponse> RejectJC(CMJCReject request)
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
