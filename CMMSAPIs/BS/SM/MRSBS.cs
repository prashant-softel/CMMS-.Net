using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;

namespace CMMSAPIs.BS.SM
{
    public interface IMRSBS
    {
        Task<CMDefaultResponse> requestMRS(MRS request);        
        Task<List<MRS>> getMRSList(int plant_ID, int emp_id, DateTime toDate, DateTime fromDate);        
        Task<List<MRS>> getMRSItems(int ID);        
        Task<List<MRS>> getMRSItemsBeforeIssue(int ID);        
        Task<List<MRS>> getMRSItemsWithCode(int ID);        
        Task<List<MRS>> getMRSDetails(int ID);        
        Task<List<MRS>> getReturnDataByID(int ID);        
        Task<List<MRS>> getAssetTypeByItemID(int ItemID);
        Task<CMDefaultResponse> mrsReturn(MRS request);        
        Task<CMDefaultResponse> mrsApproval(MRS request);        
        Task<CMDefaultResponse> mrsReturnApproval(MRS request);        
    }
    public class MRSBS : IMRSBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public MRSBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<CMDefaultResponse> requestMRS(MRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.requestMRS(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MRS>> getMRSList(int plant_ID, int emp_id, DateTime toDate, DateTime fromDate)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSList(plant_ID, emp_id, toDate, fromDate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<MRS>> getMRSItems(int ID)
           {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItems(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<MRS>> getMRSItemsBeforeIssue(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItemsBeforeIssue(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MRS>> getMRSItemsWithCode(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSItemsWithCode(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MRS>> getMRSDetails(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getMRSDetails(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MRS>> getReturnDataByID(int ID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getReturnDataByID(ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MRS>> getAssetTypeByItemID(int ItemID)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.getAssetTypeByItemID(ItemID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> mrsReturn(MRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsReturn(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<CMDefaultResponse> mrsApproval(MRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsApproval(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<CMDefaultResponse> mrsReturnApproval(MRS request)
        {
            try
            {
                using (var repos = new MRSRepository(getDB))
                {
                    return await repos.mrsReturnApproval(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
