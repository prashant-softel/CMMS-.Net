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
        Task<List<DefaultListModel>> GetPermitTypeList(int facility_id);
        Task<List<DefaultListModel>> GetSafetyMeasurementQuestionList(int permit_type_id);
        Task<List<DefaultListModel>> GetJobTypeList(int facility_id);
        Task<List<DefaultListModel>> GetSOPList(int job_type_id);

        /*
         * Permit Main End Points 
        */
        Task<List<DefaultResponseModel>> CreatePermit(CreatePermitModel request);
        Task<List<PermitListModel>> GetPermitList(int facility_id);
        Task<List<PermitDetailModel>> GetPermitDetails(int permit_id);

        /*
         * Permit Issue/Approve/Reject/Cancel End Points
        */
        Task<List<DefaultResponseModel>> PermitApprove(ApprovalModel request);
        Task<List<DefaultResponseModel>> PermitReject(ApprovalModel request);
        Task<List<DefaultResponseModel>> PermitIssue(ApprovalModel request);
        Task<List<DefaultResponseModel>> PermitCancel(ApprovalModel request);
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

        public async Task<List<DefaultListModel>> GetPermitTypeList(int facility_id)
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

        public async Task<List<DefaultListModel>> GetSafetyMeasurementQuestionList(int permit_type_id)
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

        public async Task<List<DefaultListModel>> GetJobTypeList(int facility_id)
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

        public async Task<List<DefaultListModel>> GetSOPList(int job_type_id)
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

        public async Task<List<PermitListModel>> GetPermitList(int facility_id)
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

        public async Task<List<DefaultResponseModel>> CreatePermit(CreatePermitModel request)
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

        public async Task<List<PermitDetailModel>> GetPermitDetails(int permit_id)
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

        public async Task<List<DefaultResponseModel>> PermitApprove(ApprovalModel request)
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

        public async Task<List<DefaultResponseModel>> PermitReject(ApprovalModel request)
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

        public async Task<List<DefaultResponseModel>> PermitIssue(ApprovalModel request)
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

        public async Task<List<DefaultResponseModel>> PermitCancel(ApprovalModel request)
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
