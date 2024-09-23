using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inverter;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Inverter;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Inverter
{
    public interface IInverterBS
    {
     
        Task<List<InverterDetails>> GetAllInvertersDataByDate(int PlantId, string Date);
        Task<List<InverterDetails>> GetInvertersDataById(int InvId, string Date);
        Task<List<InverterGraphData>> GetInvGraphData(int PlantId, string Date);

    }
    public class InverterBS : IInverterBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public InverterBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

       
        public async Task<List<InverterDetails>> GetAllInvertersDataByDate(int PlantId, string Date)

        {
            try
            {
                using (var repos = new InverterRepository(getDB, _environment))
                {
                    return await repos.GetAllInvertersDataByDate(PlantId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<InverterDetails>> GetInvertersDataById(int InvId, string Date)

        {
            try
            {
                using (var repos = new InverterRepository(getDB, _environment))
                {
                    return await repos.GetInvertersDataById(InvId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<InverterGraphData>> GetInvGraphData(int PlantId, string Date)

        {
            try
            {
                using (var repos = new InverterRepository(getDB, _environment))
                {
                    return await repos.GetInvGraphData(PlantId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
