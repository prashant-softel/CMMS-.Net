using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Utils
{
    public interface IUtilsBS
    {
        Task<List<Util>> GetCountryList();
        Task<List<Util>> GetStateList(int country_id);
        Task<List<Util>> GetCityList(int state_id);
        Task<List<Currency>> GetCurrencyList();
        Task<List<TZone>> GetTimeZoneList();
        Task<List<DefaultResponse>> AddLog(Log log);
        Task<List<Log>> GetLog(int module_type, int id);

    }
    public class UtilsBS : IUtilsBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public UtilsBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<Util>> GetCountryList()
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetCountryList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Util>> GetStateList(int country_id)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetStateList(country_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Util>> GetCityList(int state_id)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetStateList(state_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Currency>> GetCurrencyList()
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetCurrencyList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TZone>> GetTimeZoneList()
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetTimeZoneList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<DefaultResponse>> AddLog(Log log)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.AddLog(log);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Log>> GetLog(int module_type, int id)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetLog(module_type, id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
