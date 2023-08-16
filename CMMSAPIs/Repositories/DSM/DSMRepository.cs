using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.DSM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;

namespace CMMSAPIs.Repositories.DSM
{
    public class DSMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public DSMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        
        public async Task<List<CMDSMData>> getDSMData(CMDSMFilter request)
        {
            string filter = "";

            filter += (request?.fy?.Length > 0 ? " AND fy IN ( '" + string.Join("' , '", request.fy) + "' )" :string.Empty);
            filter += (request?.month?.Length > 0 ? " AND month IN ( '" + string.Join("' , '", request.month) + "' )" : string.Empty);
            filter += (request?.stateId?.Length > 0 ? " AND sm.stateId IN (" + string.Join(",", request.stateId) + ")" : string.Empty);
            filter += (request?.spvId?.Length > 0 ? " AND sm.spvId IN (" + string.Join(",", request.spvId) + ")" : string.Empty);
            filter += (request?.siteId?.Length > 0 ? " AND sm.id IN (" + string.Join(",", request.siteId) + ")" : string.Empty);

            string qry = "select fy , month, dsm.site, spv.name as spv, states.name as state, category, dmsType, vendor as forcasterName ,dsmPenalty, actualKwh , scheduleKwh ,sum(dsmPenalty/actualKwh)*100 as dsmPer from dsm " +
                " left join site_master as sm on sm.site = dsm.site" +
                " left join spv on spv.id = sm.spvId" +
                " left join states on states.id = sm.stateId " +
                " where 1 " + filter + " group by fy, month, site";

            List<CMDSMData> data = await Context.GetData<CMDSMData>(qry).ConfigureAwait(false);

            return data;
        }
       
    }
}
