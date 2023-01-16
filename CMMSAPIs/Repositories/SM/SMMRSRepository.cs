using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.SM;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMMSAPIs.Repositories.SM
{
    public class SMMRSRepository : GenericRepository
    {
        public SMMRSRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }


        internal Task<List<CMMRS>> GetMRSList(int facility_id)
        {
            /*
             * 
            */
            return null;
        }
        

        internal Task<CMDefaultResponse> CreateMRS(CMCreateMRS request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> EditMRS(CMCreateMRS request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> ApproveMRS(CMApproveMRS request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> RejectMRS(CMRejectMRS request)
        {
            /*
             * 
            */
            return null;
        }

        /* MRS Return */

        internal Task<List<CMMRSReturn>> GetMRSReturnList(int facility_id)
        {
            /*
             * 
            */
            return null;
        }


        internal Task<CMDefaultResponse> CreateMRSReturn(CMCreateMRSReturn request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> EditMRSReturn(CMCreateMRSReturn request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> ApproveMRSReturn(CMApproveMRSReturn request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> RejectMRSReturn(CMRejectMRSReturn request)
        {
            /*
             * 
            */
            return null;
        }

        /* My Bucket List */

        internal Task<List<CMBucket>> GetBucketAssetList(int facility_id)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> UpdateBucketAssetList(CMConsumeAssets request)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<List<CMViewConsumeAssets>> ViewConsumeAssets(int id, int module_id)
        {
            /*
             * 
            */
            return null;
        }

    }
}
