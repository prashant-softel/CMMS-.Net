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
        Task<List<CMDefaultResponse>> CreatePermit(CMCreatePermit request);
        Task<List<CMPermitList>> GetPermitList(int facility_id);
        Task<List<CMPermitDetail>> GetPermitDetails(int permit_id);

        /*
         * Permit Issue/Approve/Reject/Cancel End Points
        */
        Task<List<CMDefaultResponse>> PermitApprove(CMApproval request);
        Task<List<CMDefaultResponse>> PermitReject(CMApproval request);
        Task<List<CMDefaultResponse>> PermitIssue(CMApproval request);
        Task<List<CMDefaultResponse>> PermitCancel(CMApproval request);
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

        public async Task<List<CMPermitList>> GetPermitList(int facility_id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetPermitList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<CMDefaultResponse>> CreatePermit(CMCreatePermit request)
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

        public async Task<List<CMDefaultResponse>> PermitApprove(CMApproval request)
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

        public async Task<List<CMDefaultResponse>> PermitReject(CMApproval request)
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

        public async Task<List<CMDefaultResponse>> PermitIssue(CMApproval request)
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

        public async Task<List<CMDefaultResponse>> PermitCancel(CMApproval request)
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
