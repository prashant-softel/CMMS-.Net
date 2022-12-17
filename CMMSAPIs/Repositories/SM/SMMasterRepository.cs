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

        internal Task<List<SMMasterModel>> GetAssetTypeList()
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMasterModel>> AddAssetType()
        {
            /*
             * Add record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMasterModel>> UpdateAssetType()
        {
            /*
             * Update record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMasterModel>> DeleteAssetType()
        {
            /*
             * Delete record in SMAssetTypes
            */
            return null;
        }

        internal Task<List<SMMasterModel>> GetAssetCategoryList()
        {
            /*
             * Return id, name from SMItemCategory
            */
            return null;
        }

        internal Task<List<SMMasterModel>> AddAssetCategory()
        {
            /*
             * Add record in SMItemCategory
            */
            return null;
        }

        internal Task<List<SMMasterModel>> UpdateAssetCategory()
        {
            /*
             * Update record in SMItemCategory
            */
            return null;
        }

        internal Task<List<SMMasterModel>> DeleteAssetCategory()
        {
            /*
             * Delete record in SMItemCategory
            */
            return null;
        }

        internal Task<List<SMMasterModel>> GetUnitMeasurementList()
        {
            /*
             * Return * from SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<SMMasterModel>> AddUnitMeasurement()
        {
            /*
             * Add record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<SMMasterModel>> UpdateUnitMeasurement()
        {
            /*
             * Update record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<SMMasterModel>> DeleteUnitMeasurement()
        {
            /*
             * Delete record in SMUnitMeasurement
            */
            return null;
        }

        internal Task<List<SMMasterModel>> GetAssetMasterList()
        {
            /*
             * Return id, name, code, description, asset type, asset categroy, unit measurement, attached files  
             * from SMAssetMasters, SMAssetMasterFiles, SMUnitMeasurement, SMAssetTypes, SMAssetCategory
            */
            return null;
        }

        internal Task<List<SMMasterModel>> AddAssetMaster()
        {
            /*
             * Add record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<List<SMMasterModel>> UpdateAssetMaster()
        {
            /*
             * Update record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }

        internal Task<List<SMMasterModel>> DeleteAssetMaster()
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            return null;
        }
    }
}
