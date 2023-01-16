using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Repositories.SM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.SM
{
    public interface ISMMRSBS
    {
        Task<List<CMMRS>> GetMRSList(int facility_id);
        Task<CMDefaultResponse> CreateMRS(CMCreateMRS request);
        Task<CMDefaultResponse> EditMRS(CMCreateMRS request);
        Task<CMDefaultResponse> ApproveMRS(CMApproveMRS request);
        Task<CMDefaultResponse> RejectMRS(CMRejectMRS request);

        Task<List<CMMRSReturn>> GetMRSReturnList(int facility_id);
        Task<CMDefaultResponse> CreateMRSReturn(CMCreateMRSReturn request);
        Task<CMDefaultResponse> EditMRSReturn(CMCreateMRSReturn request);
        Task<CMDefaultResponse> ApproveMRSReturn(CMApproveMRSReturn request);
        Task<CMDefaultResponse> RejectMRSReturn(CMRejectMRSReturn request);

        /* My Bucket List */

        Task<List<CMBucket>> GetBucketAssetList(int facility_id);
        Task<CMDefaultResponse> UpdateBucketAssetList(CMConsumeAssets request);
        Task<List<CMViewConsumeAssets>> ViewConsumeAssets(int id, int module_id);

    }
    public class SMMRSBS : ISMMRSBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public SMMRSBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMMRS>> GetMRSList(int facility_id)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.GetMRSList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateMRS(CMCreateMRS request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.CreateMRS(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EditMRS(CMCreateMRS request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.EditMRS(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveMRS(CMApproveMRS request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.ApproveMRS(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectMRS(CMRejectMRS request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.RejectMRS(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /* MRS Return */

        public async Task<List<CMMRSReturn>> GetMRSReturnList(int facility_id)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.GetMRSReturnList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateMRSReturn(CMCreateMRSReturn request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.CreateMRSReturn(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> EditMRSReturn(CMCreateMRSReturn request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.EditMRSReturn(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApproveMRSReturn(CMApproveMRSReturn request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.ApproveMRSReturn(request);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectMRSReturn(CMRejectMRSReturn request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.RejectMRSReturn(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* My Bucket List */

        public async Task<List<CMBucket>> GetBucketAssetList(int facility_id)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.GetBucketAssetList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateBucketAssetList(CMConsumeAssets request)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.UpdateBucketAssetList(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMViewConsumeAssets>> ViewConsumeAssets(int id, int module_id)
        {
            try
            {
                using (var repos = new SMMRSRepository(getDB))
                {
                    return await repos.ViewConsumeAssets(id, module_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
