﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories.SM
{
    public class SMMasterRepository : GenericRepository
    {
        public SMMasterRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<SMMaster>> GetAssetTypeList()
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            string myQuery = "SELECT ID, asset_type as title FROM SMAssetTypes";
            List<SMMaster> _AssetTypeList = await Context.GetData<SMMaster>(myQuery).ConfigureAwait(false);
            return _AssetTypeList;
        }

        internal async Task<List<SMMaster>> AddAssetType()
        {
            //dont implement now
            /*
             * Add record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMaster>> UpdateAssetType()
        {
            //dont implement now
            /*
             * Update record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMaster>> DeleteAssetType()
        {
            //dont implement now
            /*
             * Delete record in SMAssetTypes
            */
            return null;
        }

        internal async Task<List<SMMaster>> GetAssetCategoryList()
        {
            /*
             * Return id, name from SMItemCategory
            */
            string myQuery = "SELECT ID, cat_name as title FROM smitemcategory ";
            List<SMMaster> _AssetCategoryList = await Context.GetData<SMMaster>(myQuery).ConfigureAwait(false);
            return _AssetCategoryList;
        }

        internal Task<List<SMMaster>> AddAssetCategory()
        {
            /*
             * Add record in SMItemCategory
            */
            return null;
        }

        internal Task<List<SMMaster>> UpdateAssetCategory()
        {
            /*
             * Update record in SMItemCategory
            */
            return null;
        }

        internal Task<List<SMMaster>> DeleteAssetCategory()
        {
            /*
             * Delete record in SMItemCategory
            */
            return null;
        }

        internal async Task<List<SMMaster>> GetUnitMeasurementList()
        {
            /*
             * Return * from SMUnitMeasurement
            */
            string myQuery = "SELECT ID, name as title FROM smunitmeasurement";
            List<SMMaster> _UnitMeasurementList = await Context.GetData<SMMaster>(myQuery).ConfigureAwait(false);
            return _UnitMeasurementList;
        }

        internal Task<List<SMMaster>> AddUnitMeasurement()
        {
            /*
             * Add record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<SMMaster>> UpdateUnitMeasurement()
        {
            /*
             * Update record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<SMMaster>> DeleteUnitMeasurement()
        {
            /*
             * Delete record in SMUnitMeasurement
            */
            return null;
        }

        internal async Task<List<SMAssetMaster>> GetAssetMasterList()
        {
            /*
             * Return id, name, code, description, asset type, asset categroy, unit measurement, attached files  
             * from SMAssetMasters, SMAssetMasterFiles, SMUnitMeasurement, SMAssetTypes, SMAssetCategory
            */
            string myQuery = "SELECT " +
                                    "am.asset_code as assetsCode,am.asset_name as assetName,at.asset_type as assetType ,ic.cat_name as assetCat,am.description as description,um.name as unitMeasurement," +
                                    "IF(am.approval_required = '1', 'YES', 'NO') as approvalRequired " +
                                   "FROM " +
                                    "smassetmasters as am " +
                             "JOIN " +
                                    "smassettypes as at ON at.ID = am.asset_type_ID " +
                             "JOIN " +
                                    "smitemcategory as ic ON ic.ID = am.item_category_ID " +
                             "JOIN " +
                                    "smunitmeasurement as um ON um.ID = am.unit_of_measurement " +
                             "WHERE " +
                                    " am.flag = 1" ;
            List<SMAssetMaster> _Employee = await Context.GetData<SMAssetMaster>(myQuery).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<int> AddAssetMaster(SMAssetMaster request)
        {

            /*implement
             * Add record in SMAssetMasters and SMAssetMasterFiles
            */
            string qry = "insert into smassetmasters (asset_code, asset_name, asset_type_ID, item_category_ID, description, unit_of_measurement, approval_required) values " +
                     "('"+ request.assetsCode + "', '"+ request.assetName + "', '"+ request.assetType + "', '"+ request.assetCat + "', '"+ request.description + "', '"+ request.unitMeasurement + "', '"+ request.approvalRequired + "')";           
            
            
            return await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);

        }


        internal async Task<int> UpdateAssetMaster(SMAssetMaster request)
        {
            /*
             * Update record in SMAssetMasters and SMAssetMasterFiles
            */
            string updateQry = "update smassetmasters set asset_code = '" + request.assetsCode + "', asset_name = '" + request.assetName + "', asset_type_ID = " + request.assetType + ", item_category_ID = " + request.assetCat + " , description = '" + request.description + "' , unit_of_measurement = " + request.unitMeasurement + ", approval_required = " + request.approvalRequired + " where ID = " + request.id + ";";

            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
        }

        internal async Task<int> DeleteAssetMaster(SMAssetMaster request)
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            string updateQry = "delete from smassetmasters where ID = " + request.id + ";";

            return await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
        }
    }
}
