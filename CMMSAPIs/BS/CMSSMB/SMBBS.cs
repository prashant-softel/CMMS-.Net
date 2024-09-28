using CMMSAPIs.Helper;
using CMMSAPIs.Models.SMB;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.SMB;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.SMB
{
    public interface ISMBBS
    {

        Task<List<SMBs>> GetSMBlistofPlant(int PlantId);
        Task<List<SMBDetails>> GetSMBDataByDate(int SMBId, string Date);

    }
    public class SMBBS : ISMBBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public SMBBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

       
        public async Task<List<SMBs>> GetSMBlistofPlant(int PlantId)

        {
            try
            {
                using (var repos = new SMBRepository(getDB, _environment))
                {
                    return await repos.GetSMBlistofPlant(PlantId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<SMBDetails>> GetSMBDataByDate(int SMBId, string Date)

        {
            try
            {
                using (var repos = new SMBRepository(getDB, _environment))
                {
                    return await repos.GetSMBDataByDate(SMBId, Date);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
