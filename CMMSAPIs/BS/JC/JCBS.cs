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
        Task<List<JCListModel>> GetJCList(int facility_id);
        Task<List<JCDetailModel>> GetJCDetail(int jc_id);
        Task<List<DefaultResponseModel>> CreateJC(int job_id);
        Task<List<DefaultResponseModel>> UpdateJC(JCUpdateModel request);
        Task<List<DefaultResponseModel>> CloseJC(JCCloseModel request);
        Task<List<DefaultResponseModel>> ApproveJC(ApprovalModel request);
        Task<List<DefaultResponseModel>> RejectJC(ApprovalModel request);
    }

    public class JCBS : IJCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<JCListModel>> GetJCList(int facility_id)
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

        public async Task<List<JCDetailModel>> GetJCDetail(int jc_id)
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

        public async Task<List<DefaultResponseModel>> CreateJC(int job_id)
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

        public async Task<List<DefaultResponseModel>> UpdateJC(JCUpdateModel request)
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

        public async Task<List<DefaultResponseModel>> CloseJC(JCCloseModel request)
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

        public async Task<List<DefaultResponseModel>> ApproveJC(ApprovalModel request)
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

        public async Task<List<DefaultResponseModel>> RejectJC(ApprovalModel request)
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
