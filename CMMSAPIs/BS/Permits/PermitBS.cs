using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.BS.Permits
{
    public interface IPermitBS
    {
        /*
         * Permit Create End Points
        */
        Task<List<CMDefaultList>> GetPermitTypeList(int facility_id);
        Task<List<CMDefaultList>> GetSafetyMeasurementQuestionList(int permit_type_id);
        Task<List<CMDefaultList>> GetJobTypeList(int facility_id);
        Task<List<CMDefaultList>> GetSOPList(int job_type_id);

        /*
         * Permit Main End Points 
        */
        Task<int> CreatePermit(CMCreatePermit set);
        Task<List<CMPermitList>> GetPermitList(int facility_id, int userID);
        Task<List<CMPermitDetail>> GetPermitDetails(int permit_id);    
        Task<List<CMDefaultResp>> PermitApprove(CMApproval request);
        Task<List<CMDefaultResp>> PermitExtend(CMApproval request);
        Task<List<CMDefaultResp>> PermitExtendApprove(CMApproval request);
        Task<List<CMDefaultResp>> PermitExtendCancel(CMApproval request);
        Task<List<CMDefaultResp>> PermitClose(CMApproval request);
        Task<List<CMDefaultResp>> PermitReject(CMApproval request);
        Task<List<CMDefaultResp>> PermitIssue(CMApproval request);
        Task<List<CMDefaultResp>> PermitCancel(CMApproval request);
        Task<int> UpdatePermit(CMUpdatePermit request);
    }

    public class PermitBS : IPermitBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public PermitBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        /* 
         * Permit Create Form Required End Points 
        */

        public async Task<List<CMDefaultList>> GetPermitTypeList(int facility_id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetPermitTypeList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultList>> GetSafetyMeasurementQuestionList(int permit_type_id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetSafetyMeasurementQuestionList(permit_type_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultList>> GetJobTypeList(int facility_id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetJobTypeList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultList>> GetSOPList(int job_type_id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetSOPList(job_type_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Permit Main Feature End Points
        */

        public async Task<List<CMPermitList>> GetPermitList(int facility_id, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetPermitList(facility_id, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> CreatePermit(CMCreatePermit request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreatePermit(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdatePermit(CMUpdatePermit request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.UpdatePermit(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMPermitDetail>> GetPermitDetails(int permit_id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetPermitDetails(permit_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
         * Permit Issue/Approval/Rejection/Cancel End Points
        */

        public async Task<List<CMDefaultResp>> PermitApprove(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitApprove(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResp>> PermitExtend(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitExtend(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResp>> PermitExtendApprove(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitExtendApprove(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResp>> PermitExtendCancel(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitExtendCancel(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMDefaultResp>> PermitClose(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitClose(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResp>> PermitReject(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitReject(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResp>> PermitIssue(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitIssue(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResp>> PermitCancel(CMApproval request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancel(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}