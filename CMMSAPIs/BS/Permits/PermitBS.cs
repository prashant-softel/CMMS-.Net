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
        Task<CMDefaultResponse> CreatePermitType(CMCreatePermitType request, int userID);
        Task<CMDefaultResponse> UpdatePermitType(CMCreatePermitType request, int userID);
        Task<CMDefaultResponse> DeletePermitType(int id);
        Task<List<CMSafetyMeasurementQuestionList>> GetSafetyMeasurementQuestionList(int permit_type_id);
        Task<CMDefaultResponse> CreateSafetyMeasure(CMCreateSafetyMeasures request, int userID);
        Task<CMDefaultResponse> UpdateSafetyMeasure(CMCreateSafetyMeasures request, int userID);
        Task<CMDefaultResponse> DeleteSafetyMeasure(int id);
        Task<List<CMDefaultList>> GetJobTypeList(int facility_id);
        Task<CMDefaultResponse> CreateJobType(CMCreateJobType request, int userID);
        Task<CMDefaultResponse> UpdateJobType(CMCreateJobType request, int userID);
        Task<CMDefaultResponse> DeleteJobType(int id);
        Task<List<CMSOPList>> GetSOPList(int job_type_id);
        Task<CMDefaultResponse> CreateSOP(CMCreateSOP request);
        Task<CMDefaultResponse> UpdateSOP(CMCreateSOP request);
        Task<CMDefaultResponse> DeleteSOP(int id);

        /*
         * Permit Main End Points 
        */
        Task<CMDefaultResponse> CreatePermit(CMCreatePermit set, int userID);
        Task<List<CMPermitList>> GetPermitList(int facility_id, int userID, bool self_view);
        Task<CMPermitDetail> GetPermitDetails(int permit_id);    
        Task<CMDefaultResponse> PermitApprove(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitExtend(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitExtendApprove(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitExtendCancel(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitClose(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitReject(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitIssue(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitIssueReject(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitCancelRequest(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitCancelReject(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitCancelByApprover(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitCancelByHSE(CMApproval request, int userID);
        Task<CMDefaultResponse> PermitCancelByIssuer(CMApproval request, int userID);
        Task<CMDefaultResponse> UpdatePermit(CMUpdatePermit request, int userID);
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
        public async Task<CMDefaultResponse> CreatePermitType(CMCreatePermitType request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreatePermitType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdatePermitType(CMCreatePermitType request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.UpdatePermitType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeletePermitType(int id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.DeletePermitType(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMSafetyMeasurementQuestionList>> GetSafetyMeasurementQuestionList(int permit_type_id)
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

        public async Task<CMDefaultResponse> CreateSafetyMeasure(CMCreateSafetyMeasures request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreateSafetyMeasure(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateSafetyMeasure(CMCreateSafetyMeasures request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.UpdateSafetyMeasure(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteSafetyMeasure(int id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.DeleteSafetyMeasure(id);
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

        public async Task<CMDefaultResponse> CreateJobType(CMCreateJobType request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreateJobType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateJobType(CMCreateJobType request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.UpdateJobType(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteJobType(int id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.DeleteJobType(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMSOPList>> GetSOPList(int job_type_id)
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
        public async Task<CMDefaultResponse> CreateSOP(CMCreateSOP request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreateSOP(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdateSOP(CMCreateSOP request)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.UpdateSOP(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> DeleteSOP(int id)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.DeleteSOP(id);
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

        public async Task<List<CMPermitList>> GetPermitList(int facility_id, int userID, bool self_view)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.GetPermitList(facility_id, userID, self_view);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CMDefaultResponse> CreatePermit(CMCreatePermit request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreatePermit(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdatePermit(CMUpdatePermit request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.UpdatePermit(request, userID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMPermitDetail> GetPermitDetails(int permit_id)
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

        public async Task<CMDefaultResponse> PermitApprove(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitApprove(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> PermitExtend(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitExtend(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> PermitExtendApprove(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitExtendApprove(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> PermitExtendCancel(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitExtendCancel(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> PermitClose(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitClose(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitReject(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitReject(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitIssue(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitIssue(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> PermitIssueReject(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitIssueReject(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitCancelRequest(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancelRequest(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitCancelByApprover(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancelByApprover(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitCancelByHSE(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancelByHSE(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitCancelByIssuer(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancelByIssuer(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> PermitCancelReject(CMApproval request, int userID)
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancelReject(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}