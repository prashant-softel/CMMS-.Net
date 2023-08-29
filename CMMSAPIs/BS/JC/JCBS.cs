using CMMSAPIs.Helper;
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
        Task<List<CMJCList>> GetJCList(int facility_id, int userID, bool self_view);
        Task<List<CMJCListForJob>> GetJCListByJobId(int jobId);
        Task<List<CMJCDetail>> GetJCDetail(int jc_id);
        Task<CMDefaultResponse> CreateJC(int job_id, int userID);
        Task<CMDefaultResponse> UpdateJC(CMJCUpdate request, int userID);
        Task<CMDefaultResponse> CloseJC(CMJCClose request, int userID);
        Task<List<CMDefaultResponse>> ApproveJC(CMJCApprove request, int userID);
        Task<CMDefaultResponse> RejectJC(CMJCReject request, int userID);
        Task<CMDefaultResponse> StartJC(int jc_id, int userID);
        Task<CMDefaultResponse> CarryForwardJC(CMApproval request, int userID);
    }

    public class JCBS : IJCBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public JCBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMJCList>> GetJCList(int facility_id, int userID, bool self_view)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCList(facility_id,  userID,  self_view);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMJCListForJob>> GetJCListByJobId(int jobId)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.GetJCListByJobId(jobId);
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

        public async Task<CMDefaultResponse> StartJC(int jc_id, int userID)
        {
            try
            {
                using (var repos = new JCRepository(getDB))
                {
                    return await repos.StartJC(jc_id, userID);
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

    }
}
