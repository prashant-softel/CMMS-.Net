using System;
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

        internal Task<List<CMSMMaster>> GetAssetTypeList()
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            return null;
        }

        internal Task<List<CMSMMaster>> AddAssetType()
        {
            /*
             * Add record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<CMSMMaster>> UpdateAssetType()
        {
            /*
             * Update record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<CMSMMaster>> DeleteAssetType()
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
