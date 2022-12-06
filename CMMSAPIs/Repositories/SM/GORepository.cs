using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories
{
    public class GORepository : GenericRepository
    {
        public GORepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        
        internal Task<List<GO>> GetGOList()
        {
            /*
             * 
            */
            return null;
        }
        internal Task<List<GO>> GetAssetCodeDetails(int asset_code)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<List<GO>> CreateGO()
        {
            return null;
        }
        internal Task<List<GO>> UpdateGO()
        {
            return null;
        }
        internal Task<List<GO>> DeleteGO()
        {
            return null;
        }
        internal Task<List<GO>> WithdrawGO()
        {
            return null;
        }
    }
}
