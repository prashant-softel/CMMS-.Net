using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Utils;
using System.Data;
using CMMSAPIs.Models.Users;
using MySql.Data.MySqlClient;
using System.Transactions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Notifications;
using System.ComponentModel;

namespace CMMSAPIs.Repositories.SM
{
    public class ReportsRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public ReportsRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        public async Task<List<CMPlantStockOpening>> GetPlantStockReport(int plant_ID,DateTime StartDate, DateTime EndDate)
        {
            List<string> Asset_Item_Ids = new List<string>();
            List<CMPlantStockOpening> Asset_Item_Opening_Balance_details = new List<CMPlantStockOpening>();
            //Actors.Store
            string Plant_Stock_Opening_Details_query = $"SELECT fc.name as facilityName,fc.isBlock as Facility_Is_Block," +
                $"'' as Facility_Is_Block_of_name," +
                $"sm_trans.assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, " +
                $"sum(sm_trans.creditQty)-sum(sm_trans.debitQty) as Opening ,sum(sm_trans.creditQty) as inward, sum(sm_trans.debitQty) as outward " +
                $"FROM smtransition as sm_trans " +
                $"join smassetitems as a_item ON sm_trans.assetItemID = a_item.ID " +
                $"JOIN smassetmasters as a_master ON a_master.asset_code = a_item.asset_code " +
                $"LEFT JOIN facilities fc ON fc.id = a_item.plant_ID " +
                $"where sm_trans.actorType = {(int)CMMS.SM_Types.Store} and a_item.plant_ID = {plant_ID} " +
                $"and date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') BETWEEN '{ StartDate.ToString("yyyy-MM-dd")}' AND '{ EndDate.ToString("yyyy-MM-dd")}'  " +
                $"group by a_item.asset_code";
            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                string plant_name = "";
                if (Convert.ToInt32(item.Facility_Is_Block) == 0)
                {
                    plant_name = Convert.ToString(item.facilityName);
                }
                else
                {
                    plant_name = Convert.ToString(item.Facility_Is_Block_of_name);
                }

                Asset_Item_Ids.Add(Convert.ToString(item.assetItemID));

                CMPlantStockOpening openingBalance = new CMPlantStockOpening();
                openingBalance.plant_name = plant_name;
                openingBalance.Opening = item.Opening;
                openingBalance.assetItemID = item.assetItemID;
                openingBalance.asset_name = item.asset_name;
                openingBalance.asset_code = item.asset_code;
                openingBalance.asset_type_ID = item.asset_type_ID;
                openingBalance.inward = item.inward;
                openingBalance.outward = item.outward;

                Asset_Item_Opening_Balance_details.Add(openingBalance);
        
            }
            return Asset_Item_Opening_Balance_details;

        }
    }
}
