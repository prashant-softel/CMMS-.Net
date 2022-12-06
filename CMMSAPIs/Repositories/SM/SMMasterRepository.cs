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

        internal Task<List<SMMaster>> GetAssetTypeList()
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMaster>> AddAssetType()
        {
            /*
             * Add record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMaster>> UpdateAssetType()
        {
            /*
             * Update record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMaster>> DeleteAssetType()
        {
            /*
             * Delete record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMaster>> GetAssetCategoryList()
        {
            /*
             * Return id, name from SMItemCategory
            */
            return null;
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

        internal Task<List<SMMaster>> GetUnitMeasurementList()
        {
            /*
             * Return * from SMUnitMeasurement
            */
            return null;
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

        internal Task<List<SMMaster>> GetAssetMasterList()
        {
            /*
             * Return id, name, code, description, asset type, asset categroy, unit measurement, attached files  
             * from SMAssetMasters, SMAssetMasterFiles, SMUnitMeasurement, SMAssetTypes, SMAssetCategory
            */
            return null;
        }

        internal Task<List<SMMaster>> AddAssetMaster()
        {
            /*
             * Add record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<List<SMMaster>> UpdateAssetMaster()
        {
            /*
             * Update record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<List<SMMaster>> DeleteAssetMaster()
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }
    }
}
