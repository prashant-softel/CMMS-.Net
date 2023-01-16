using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
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

        internal Task<List<CMSMMaster>> GetAssetTypeList()
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            return null;
        }

        internal Task<CMDefaultResponse> AddAssetType(CMSMMaster request)
        {
            /*
             * Add record in SMAssetTypes
            */
            return null;
        }

        internal Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request)
        {
            /*
             * Update record in SMAssetTypes
            */
            return null;
        }

        internal Task<CMDefaultResponse> DeleteAssetType(int id)
        {
            /*
             * Delete record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<CMSMMaster>> GetAssetCategoryList()
        {
            /*
             * Return id, name from SMItemCategory
            */
            return null;
        }

        internal Task<CMDefaultResponse> AddAssetCategory(CMSMMaster request)
        {
            /*
             * Add record in SMItemCategory
            */
            return null;
        }

        internal Task<CMDefaultResponse> UpdateAssetCategory(CMSMMaster request)
        {
            /*
             * Update record in SMItemCategory
            */
            return null;
        }

        internal Task<CMDefaultResponse> DeleteAssetCategory(int id)
        {
            /*
             * Delete record in SMItemCategory
            */
            return null;
        }

        internal Task<List<CMSMUnitMaster>> GetUnitMeasurementList()
        {
            /*
             * Return * from SMUnitMeasurement
            */
            return null;
        }

        internal Task<CMDefaultResponse> AddUnitMeasurement(CMSMUnitMaster request)
        {
            /*
             * Add record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<CMDefaultResponse> UpdateUnitMeasurement(CMSMUnitMaster request)
        {
            /*
             * Update record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<CMDefaultResponse> DeleteUnitMeasurement(int id)
        {
            /*
             * Delete record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<CMSMAssetMaster>> GetAssetMasterList()
        {
            /*
             * Return id, name, code, description, asset type, asset categroy, unit measurement, attached files  
             * from SMAssetMasters, SMAssetMasterFiles, SMUnitMeasurement, SMAssetTypes, SMAssetCategory
            */
            return null;
        }

        internal Task<CMDefaultResponse> AddAssetMaster(CMSMAssetMaster request)
        {
            /*
             * Add record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<CMDefaultResponse> UpdateAssetMaster(CMSMAssetMaster request)
        {
            /*
             * Update record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<CMDefaultResponse> DeleteAssetMaster(int id)
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        /* Stock List */

        internal Task<List<CMStock>> GetStockList(int facility_id)
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<CMDefaultResponse> UpdateStockList(List<CMStock> request)
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }
    }
}
