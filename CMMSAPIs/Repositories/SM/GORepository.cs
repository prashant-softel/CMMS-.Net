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

        
        internal Task<List<CMGO>> GetGOList()
        {
            /*
             * 
            */
            return null;
        }
        internal Task<List<CMGO>> GetAssetCodeDetails(int asset_code)
        {
            /*
             * 
            */
            return null;
        }

        internal Task<List<CMGO>> CreateGO()
        {
            return null;
        }
        internal Task<List<CMGO>> UpdateGO()
        {
            return null;
        }
        internal Task<List<CMGO>> DeleteGO()
        {
            return null;
        }
        internal Task<List<CMGO>> WithdrawGO()
        {
            return null;
        }
    }
}
