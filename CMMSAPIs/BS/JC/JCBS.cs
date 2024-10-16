using CMMSAPIs.Helper;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.JC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.JC
{
    public interface IJCBS
    {
        Task<List<CMJCList>> GetJCList(string facility_id, int userID, bool self_view, string start_date, string end_date, string facilitytime);
        Task<List<CMJCListForJob>> GetJCListByJobId(int jobId, string facilitytime);
        Task<List<CMJCDetail>> GetJCDetail(int jc_id, string facilitytime);
        Task<CMDefaultResponse> CreateJC(int job_id, int userID);
        Task<CMDefaultResponse> UpdateJC(CMJCUpdate request, int userID);
        Task<CMDefaultResponse> CloseJC(CMJCClose request, int userID);
        Task<List<CMDefaultResponse>> ApproveJC(CMJCApprove request, int userID);
        Task<CMDefaultResponse> RejectJC(CMJCReject request, int userID);
        Task<CMDefaultResponse> StartJC(CMJCRequest request, int userID, string facilitytime);
        Task<CMDefaultResponse> CarryForwardJC(CMApproval request, int userID);
        Task<CMDefaultResponse> RejectJCCF(CMJCReject request, int userID);
        Task<CMDefaultResponse> ApproveJCCF(CMJCApprove request, int userID);
    }

    public class JCBS : IJCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMJCList>> GetJCList(string facility_id, int userID, bool self_view, string start_date, string end_date, string facilitytime)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCList(facility_id, userID, self_view, start_date, end_date, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJCListForJob>> GetJCListByJobId(int jobId, string facilitytime)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCListByJobId(jobId, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJCDetail>> GetJCDetail(int jc_id, string facilitytime)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCDetail(jc_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateJC(int job_id, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.CreateJC(job_id, userID);
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

        public async Task<CMDefaultResponse> CloseJC(CMJCClose request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.CloseJC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> ApproveJC(CMJCApprove request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.ApproveJC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectJC(CMJCReject request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.RejectJC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> StartJC(CMJCRequest request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.StartJC(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> CarryForwardJC(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.CarryForwardJC(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> ApproveJCCF(CMJCApprove request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.ApproveJCCF(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectJCCF(CMJCReject request, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.RejectJCCF(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
