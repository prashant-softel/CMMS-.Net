using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models;

namespace CMMSAPIs.BS
{
    public interface IPermitBS
    {
        /*
         * Permit Create End Points
        */
        Task<List<Permit>> GetPermitTypeList(int facility_id);
        Task<List<Permit>> GetSafetyMeasurementQuestionList(int permit_type_id);
        Task<List<Permit>> GetJobTypeList(int facility_id);
        Task<List<Permit>> GetSOPList(int job_type_id);

        /*
         * Permit Main End Points 
        */
        Task<List<Permit>> CreatePermit();
        Task<List<Permit>> GetPermitList(int facility_id);
        Task<List<Permit>> GetPermitDetails(int permit_id);

        /*
         * Permit Issue/Approve/Reject/Cancel End Points
        */
        Task<List<Permit>> PermitApprove();
        Task<List<Permit>> PermitReject();
        Task<List<Permit>> PermitIssue();
        Task<List<Permit>> PermitCancel();
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

        public async Task<List<Permit>> GetPermitTypeList(int facility_id)
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

        public async Task<List<Permit>> GetSafetyMeasurementQuestionList(int permit_type_id)
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

        public async Task<List<Permit>> GetJobTypeList(int facility_id)
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

        public async Task<List<Permit>> GetSOPList(int job_type_id)
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

        public async Task<List<Permit>> GetPermitList(int facility_id)
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

        public async Task<List<Permit>> CreatePermit()
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.CreatePermit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Permit>> GetPermitDetails(int permit_id)
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

        public async Task<List<Permit>> PermitApprove()
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitApprove();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Permit>> PermitReject()
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitReject();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Permit>> PermitIssue()
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitIssue();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Permit>> PermitCancel()
        {
            try
            {
                using (var repos = new PermitRepository(getDB))
                {
                    return await repos.PermitCancel();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
