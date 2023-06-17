using CMMSAPIs.Helper;
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
        Task<List<CMPlantStockOpening>> GetPlantStockReport(int plant_ID, DateTime StartDate, DateTime EndDate);
        Task<List<CMEmployeeStockReport>> GetEmployeeStockReport(int plant_ID, DateTime StartDate, DateTime EndDate);
        Task<List<CMFaultyMaterialReport>> GetFaultyMaterialReport(int plant_ID, DateTime StartDate, DateTime EndDate);
        Task<List<CMEmployeeTransactionReport>> GetEmployeeTransactionReport(int isAllEmployees, int plant_ID, DateTime StartDate, DateTime EndDate);
    }
    public class ReportsBS : ISMReportsBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public ReportsBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMPlantStockOpening>> GetPlantStockReport(int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetPlantStockReport(plant_ID, StartDate, EndDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMEmployeeStockReport>> GetEmployeeStockReport(int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetEmployeeStockReport(plant_ID, StartDate, EndDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<CMFaultyMaterialReport>> GetFaultyMaterialReport(int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetFaultyMaterialReport(plant_ID, StartDate, EndDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMEmployeeTransactionReport>> GetEmployeeTransactionReport(int isAllEmployees, int plant_ID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (var repos = new ReportsRepository(getDB))
                {
                    return await repos.GetEmployeeTransactionReport(isAllEmployees, plant_ID, StartDate, EndDate);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
