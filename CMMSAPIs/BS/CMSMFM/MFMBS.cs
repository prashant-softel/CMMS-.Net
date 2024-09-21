using CMMSAPIs.Helper;
using CMMSAPIs.Models.MFM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.MFM;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.MFM
{
    public interface IMFMBS
    {

        Task<List<MFMDetails>> GetMFMlistofPlant(int PlantId);
        Task<List<MFMGraphData>> GetMFMGraphData(int MFMId, string Date);

    }
    public class MFMBS : IMFMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public MFMBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

       
        public async Task<List<MFMDetails>> GetMFMlistofPlant(int PlantId)

        {
            try
            {
                using (var repos = new MFMRepository(getDB, _environment))
                {
                    return await repos.GetMFMlistofPlant(PlantId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MFMGraphData>> GetMFMGraphData(int MFMId, string Date)

        {
            try
            {
                using (var repos = new MFMRepository(getDB, _environment))
                {
                    return await repos.GetMFMGraphData(MFMId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
