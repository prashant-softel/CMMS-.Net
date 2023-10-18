﻿using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.SM;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models;
using CMMSAPIs.Repositories;

namespace CMMSAPIs.BS.SM
{
    public interface ISMReportsBS
    {
        Task<List<CMPlantStockOpeningResponse>> GetPlantStockReport(string facility_id, DateTime StartDate, DateTime EndDate, string assetMasterIDs);
        Task<List<CMEmployeeStockReport>> GetEmployeeStockReport(int facility_id, int Emp_id, DateTime StartDate, DateTime EndDate, string itemID);
        Task<List<CMFaultyMaterialReport>> GetFaultyMaterialReport(string facility_id, string itemID, DateTime StartDate, DateTime EndDate);
        Task<List<CMEmployeeTransactionReport>> GetEmployeeTransactionReport(int isAllEmployees, string facility_id, int Emp_ID, DateTime StartDate, DateTime EndDate);
        Task<CMEmployeeStockList> GetEmployeeStock(int facility_ID, int emp_id);
        Task<List<CMEmployeeStockTransactionReport>> GetEmployeeStockTransactionReport(int facility_ID, int emp_id);
        Task<List<CMAssetMasterStockItems>> GetAssetMasterStockItems(int assetID);
        Task<List<CMPlantStockOpeningResponse>> GetStockReport(string facility_id, int actorTypeID,int actorID,  DateTime StartDate, DateTime EndDate, string assetMasterIDs);
    }
    public class ReportsBS : ISMReportsBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public ReportsBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMPlantStockOpeningResponse>> GetPlantStockReport(string facility_id, DateTime StartDate, DateTime EndDate, string assetMasterIDs)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetPlantStockReport(facility_id, StartDate, EndDate, assetMasterIDs);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMEmployeeStockReport>> GetEmployeeStockReport(int facility_id, int Emp_id, DateTime StartDate, DateTime EndDate, string itemID)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetEmployeeStockReport(facility_id, Emp_id, StartDate, EndDate, itemID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<CMFaultyMaterialReport>> GetFaultyMaterialReport(string facility_id, string itemID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetFaultyMaterialReport(facility_id, itemID, StartDate, EndDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMEmployeeTransactionReport>> GetEmployeeTransactionReport(int isAllEmployees, string facility_id, int Emp_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetEmployeeTransactionReport(isAllEmployees, facility_id, Emp_ID, StartDate, EndDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMEmployeeStockList> GetEmployeeStock(int facility_ID, int emp_id)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetEmployeeStock(facility_ID, emp_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMEmployeeStockTransactionReport>> GetEmployeeStockTransactionReport(int facility_ID, int emp_id)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetEmployeeStockTransactionReport(facility_ID, emp_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMAssetMasterStockItems>> GetAssetMasterStockItems(int assetID)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetAssetMasterStockItems(assetID);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMPlantStockOpeningResponse>> GetStockReport(string facility_id, int actorTypeID,int actorID, DateTime StartDate, DateTime EndDate, string assetMasterIDs)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetStockReport(facility_id, actorTypeID, actorID, StartDate, EndDate, assetMasterIDs);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
