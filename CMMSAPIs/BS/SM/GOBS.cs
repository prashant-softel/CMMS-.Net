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
        Task<List<CMGO>> GetGOList();
        Task<List<CMGO>> GetAssetCodeDetails(int asset_code);
        Task<List<CMGO>> CreateGO();
        Task<List<CMGO>> UpdateGO();
        Task<List<CMGO>> DeleteGO();
        Task<List<CMGO>> WithdrawGO();
    }

    public class GOBS : IGOBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public GOBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMGO>> GetGOList()
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

        public async Task<List<CMGO>> GetAssetCodeDetails(int asset_code)
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

        public async Task<List<CMGO>> CreateGO()
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

        public async Task<List<CMGO>> UpdateGO()
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

        public async Task<List<CMGO>> DeleteGO()
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
        public async Task<List<CMGO>> WithdrawGO()
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
