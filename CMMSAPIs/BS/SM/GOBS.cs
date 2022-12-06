using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models;

namespace CMMSAPIs.BS
{
    public interface IGOBS
    {
        Task<List<GO>> GetGOList();
        Task<List<GO>> GetAssetCodeDetails(int asset_code);
        Task<List<GO>> CreateGO();
        Task<List<GO>> UpdateGO();
        Task<List<GO>> DeleteGO();
        Task<List<GO>> WithdrawGO();
    }

    public class GOBS : IGOBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public GOBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<GO>> GetGOList()
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetGOList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GO>> GetAssetCodeDetails(int asset_code)
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.GetAssetCodeDetails(asset_code);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GO>> CreateGO()
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.CreateGO();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GO>> UpdateGO()
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.UpdateGO();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GO>> DeleteGO()
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.DeleteGO();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<GO>> WithdrawGO()
        {
            try
            {
                using (var repos = new GORepository(getDB))
                {
                    return await repos.WithdrawGO();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
