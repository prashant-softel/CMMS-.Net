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

namespace CMMSAPIs.Repositories.SM
{
    public class SMMasterRepository : GenericRepository
    {
        public SMMasterRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMSMMaster>> GetAssetTypeList()
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            var myQuery = "SELECT * FROM SM_Asset_Type WHERE flag = 1";
            
            List<CMSMMaster> _checkList = await Context.GetData<CMSMMaster>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID)
        {
            /*
             * Add record in SMAssetTypes
            */
            string mainQuery = $"INSERT INTO smassettypes (asset_type,flag) VALUES (" +request.asset_type+",1); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse  response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset type added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID)
        {
            /*
             * Update record in SMAssetTypes
            */
            string mainQuery = $"UPDATE smassettypes SET asset_type = " + request.asset_type + " where ID = "+request.ID+"";
            await Context.ExecuteNonQry<int>(mainQuery); 
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset type updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteAssetType(int Id, int userID)
        {
            /*
             * Delete record in SMAssetTypes
            */
            string mainQuery = $"UPDATE smassettypes SET flag = 0 where ID = " + Id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset type deleted successfully.");
            return response;
        }

        internal async Task<List<ItemCategory>> GetAssetCategoryList()
        {
            /*
             * Return id, name from SMItemCategory
            */

            var myQuery = "SELECT * FROM SMItemCategory WHERE flag = 1";

            List<ItemCategory> _List = await Context.GetData<ItemCategory>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddAssetCategory(ItemCategory request, int userID)
        {
            /*
             * Add record in SMItemCategory
            */
            string mainQuery = $"INSERT INTO SMItemCategory (cat_name,flag) VALUES ('" + request.cat_name + "',1); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset category added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAssetCategory(ItemCategory request, int userID)
        {
            /*
             * Update record in SMItemCategory
            */
            string mainQuery = $"UPDATE SMItemCategory SET cat_name = '"+request.cat_name+"' where ID = " + request.ID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement deleted.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteAssetCategory(int acID, int userID)
        {
            /*
             * Delete record in SMItemCategory
            */
            string mainQuery = $"UPDATE SMItemCategory SET flag = 0 where ID = " + acID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement deleted.");
            return response;
        }

        internal async Task<List<UnitMeasurement>> GetUnitMeasurementList()
        {

            var myQuery = "SELECT * FROM smunitmeasurement WHERE flag = 1";
            List<UnitMeasurement> _List = await Context.GetData<UnitMeasurement>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddUnitMeasurement(UnitMeasurement request,int userID)
        {
            string mainQuery = $"INSERT INTO smunitmeasurement (name,flag,decimal_status, spare_multi_selection) VALUES ('"+request.name+"',1,"+request.decimal_status+", "+request.spare_multi_selection+"); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateUnitMeasurement(UnitMeasurement request, int userID)
        {
            string mainQuery = $"UPDATE smunitmeasurement SET name = '"+request.name+"',decimal_status="+request.decimal_status+", spare_multi_selection = "+request.spare_multi_selection+" WHERE ID = "+request.ID+"";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID)
        {
            string mainQuery = $"UPDATE smunitmeasurement SET flag = 0 where ID = " + umID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement deleted.");
            return response;
        }

        internal Task<List<CMSMMaster>> GetAssetMasterList()
        {
            /*
             * Return id, name, code, description, asset type, asset categroy, unit measurement, attached files  
             * from SMAssetMasters, SMAssetMasterFiles, SMUnitMeasurement, SMAssetTypes, SMAssetCategory
            */
            return null;
        }

        internal Task<List<CMSMMaster>> AddAssetMaster()
        {
            /*
             * Add record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<List<CMSMMaster>> UpdateAssetMaster()
        {
            /*
             * Update record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<List<CMSMMaster>> DeleteAssetMaster()
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }
    }
}
