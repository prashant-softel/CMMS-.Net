using CMMSAPIs.Helper;
using CMMSAPIs.Models.Portfolio;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Portfolio;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Portfolio
{
    public interface IPorfolioBS
    {
        Task<List<CMSPlantList>> GetUserPlantsDetail(int UserId);
        Task<AllPlantsEnergy> GetUserPlantsEnergyDetails(int UserId, string Date);
      

    }
    public class PortfolioBS : IPorfolioBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public PortfolioBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

        public async Task<List<CMSPlantList>> GetUserPlantsDetail(int UserId)
        {
            try
            {
                using (var repos = new PortfolioRepository(getDB, _environment))
                {
                    return await repos.GetUserPlantsDetail(UserId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<AllPlantsEnergy> GetUserPlantsEnergyDetails(int UserId, string Date)

        {
            try
            {
                using (var repos = new PortfolioRepository(getDB, _environment))
                {
                    return await repos.GetUserPlantsEnergyDetails(UserId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
