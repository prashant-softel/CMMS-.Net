using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMMSAPIs.Repositories;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models;

namespace CMMSAPIs.BS
{
    public interface IGOBS
    {
        Task<List<CMGOList>> GetGOList();
        Task<CMAssetDetail> GetAssetCodeDetails(int asset_code);
        Task<CMDefaultResponse> CreateGO();
        Task<CMDefaultResponse> UpdateGO();
        Task<CMDefaultResponse> DeleteGO();
        Task<CMDefaultResponse> WithdrawGO();
    }

    public class GOBS : IGOBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public GOBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMGOList>> GetGOList()
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

        public async Task<CMAssetDetail> GetAssetCodeDetails(int asset_code)
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

        public async Task<CMDefaultResponse> CreateGO()
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

        public async Task<CMDefaultResponse> UpdateGO()
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

        public async Task<CMDefaultResponse> DeleteGO()
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
        public async Task<CMDefaultResponse> WithdrawGO()
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
