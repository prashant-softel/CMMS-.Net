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

        internal Task<List<CMSMMaster>> GetAssetCategoryList()
        {
            /*
             * Return id, name from SMItemCategory
            */
            return null;
        }

        internal Task<List<CMSMMaster>> AddAssetCategory()
        {
            /*
             * Add record in SMItemCategory
            */
            return null;
        }

        internal Task<List<CMSMMaster>> UpdateAssetCategory()
        {
            /*
             * Update record in SMItemCategory
            */
            return null;
        }

        internal Task<List<CMSMMaster>> DeleteAssetCategory()
        {
            /*
             * Delete record in SMItemCategory
            */
            return null;
        }

        internal Task<List<CMSMMaster>> GetUnitMeasurementList()
        {
            /*
             * Return * from SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<CMSMMaster>> AddUnitMeasurement()
        {
            /*
             * Add record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<CMSMMaster>> UpdateUnitMeasurement()
        {
            /*
             * Update record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<CMSMMaster>> DeleteUnitMeasurement()
        {
            /*
             * Delete record in SMUnitMeasurement
            */
            return null;
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
