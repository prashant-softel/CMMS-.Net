using CMMSAPIs.Helper;
using CMMSAPIs.Models.Dashboard;
using CMMSAPIs.Models.Inverter;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Dashboard;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Dashboard
{
    public interface IDashboardBS
    {
     
        Task<PlantPerformanceDetails> GetPlantPerformanceDetails(int PlantId, string Date);
        Task<List<PowerGraphData>> GetPowerGraphDataByMinute(int PlantId, string Date);
        Task<List<EnergyGraphData>> GetEnegryGraphDataByMinute(int PlantId, string Date);
        Task<List<WeatherGraphData>> GetWeatherDataByMinute(int PlantId, string Date);
       

    }
    public class DashboardBS : IDashboardBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public DashboardBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

       
        public  async Task<PlantPerformanceDetails> GetPlantPerformanceDetails(int PlantId, string Date)

        {
            try
            {
                using (var repos = new DashboardRepository(getDB, _environment))
                {
                    return await repos.GetPlantPerformanceDetails(PlantId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PowerGraphData>> GetPowerGraphDataByMinute(int PlantId, string Date)

        {
            try
            {
                using (var repos = new DashboardRepository(getDB, _environment))
                {
                    return await repos.GetPowerGraphDataByMinute(PlantId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EnergyGraphData>> GetEnegryGraphDataByMinute(int PlantId, string Date)
        {
            try
            {
                using (var repos = new DashboardRepository(getDB, _environment))
                {
                    return await repos.GetEnegryGraphDataByMinute(PlantId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<WeatherGraphData>> GetWeatherDataByMinute(int PlantId, string Date)
        {
            try
            {
                using (var repos = new DashboardRepository(getDB, _environment))
                {
                    return await repos.GetWeatherDataByMinute(PlantId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
       
    }
}
