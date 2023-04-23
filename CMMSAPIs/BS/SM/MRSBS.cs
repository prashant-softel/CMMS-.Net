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
    }

}
