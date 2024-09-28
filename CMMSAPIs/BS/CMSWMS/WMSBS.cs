using CMMSAPIs.Helper;
using CMMSAPIs.Models.WMS;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.WMS;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.WMS
{
    public interface IWMSBS
    {

        Task<List<WMSDetails>> GetWMSlistofPlant(int PlantId);
        Task<List<WMSData>> GetWMSDataByDate(int WMSId, string Date);
        Task<List<WMSGraphData>> GetMinuteWMSGraphData(int WMSId, string Date);
        Task<List<WMSGraphData>> GetDailyWMSGraphData(int WMSId, string startDate, string endDate);
        Task<List<WMSGraphData>> GetMonthlyWMSGraphData(int WMSId, string startDate, string endDate);


    }
    public class WMSBS : IWMSBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public WMSBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

       
        public async Task<List<WMSDetails>> GetWMSlistofPlant(int PlantId)

        {
            try
            {
                using (var repos = new WMSRepository(getDB, _environment))
                {
                    return await repos.GetWMSlistofPlant(PlantId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<WMSData>> GetWMSDataByDate(int WMSId, string Date)
        {
            try
            {
                using (var repos = new WMSRepository(getDB, _environment))
                {
                    return await repos.GetWMSDataByDate(WMSId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async  Task<List<WMSGraphData>> GetMinuteWMSGraphData(int WMSId, string Date)

        {
            try
            {
                using (var repos = new WMSRepository(getDB, _environment))
                {
                    return await repos.GetMinuteWMSGraphData(WMSId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<WMSGraphData>> GetDailyWMSGraphData(int WMSId, string startDate, string endDate)
        {
            try
            {
                using (var repos = new WMSRepository(getDB, _environment))
                {
                    return await repos.GetDailyWMSGraphData(WMSId, startDate, endDate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }public async Task<List<WMSGraphData>> GetMonthlyWMSGraphData(int WMSId, string startDate, string endDate)
        {
            try
            {
                using (var repos = new WMSRepository(getDB, _environment))
                {
                    return await repos.GetMonthlyWMSGraphData(WMSId, startDate, endDate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
