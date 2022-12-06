using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models;

namespace CMMSAPIs.BS
{
    public interface IModuleBS
    {
        Task<List<Job>> GetModuleList();
    }

    public class ModuleBS : IModuleBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public ModuleBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        

        public async Task<List<Module>> GetJobList()
        {
            try
            {
                using (var repos = new ModuleRepository(getDB))
                {
                    return await repos.GetJobList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        
    }
}
