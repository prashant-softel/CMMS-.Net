using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Utils;


namespace CMMSAPIs.Repositories
{
    public class GORepository : GenericRepository
    {
        public GORepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        
        internal Task<List<CMGOList>> GetGOList()
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMAssetDetail> GetAssetCodeDetails(int asset_code)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<CMDefaultResponse> CreateGO()
        {
            return null;
        }
        internal Task<CMDefaultResponse> UpdateGO()
        {
            return null;
        }
        internal Task<CMDefaultResponse> DeleteGO()
        {
            return null;
        }
        internal Task<CMDefaultResponse> WithdrawGO()
        {
            return null;
        }
    }
}
