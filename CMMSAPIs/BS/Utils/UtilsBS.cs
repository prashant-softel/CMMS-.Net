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
        Task<List<CMDefaultList>> GetCountryList();
        Task<List<CMDefaultList>> GetStateList(int country_id);
        Task<List<CMDefaultList>> GetCityList(int state_id);
        Task<List<CMCurrency>> GetCurrencyList();
        // Task<List<double>> GetConversionRate(int currency_id_from, int currency_id_to);
        Task<List<TZone>> GetTimeZoneList();
        // Task<List<TZone>> GetTimeZone(int facility_id);
        //Changes
        Task<DateTime> Contvertime(int facility_id, DateTime datetime);
        Task<List<CMDefaultResponse>> AddLog(CMLog log);
        Task<List<CMLog>> GetHistoryLog(CMMS.CMMS_Modules module_type, int id,string facilitytime);
    }
    public class UtilsBS : IUtilsBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public UtilsBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMDefaultList>> GetCountryList()
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

        public async Task<List<CMDefaultList>> GetStateList(int country_id)
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

        public async Task<List<CMDefaultList>> GetCityList(int state_id)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetCityList(state_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMCurrency>> GetCurrencyList()
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

        //public async Task<List<double>> GetConversionRate(int currency_id_from, int currency_id_to)
        //{
        //    try
        //    {
        //        using (var repos = new UtilsRepository(getDB))
        //        {
        //            return await repos.GetConversionRate(currency_id_from, currency_id_to);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

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

       
        //public async Task<List<TZone>> GetTimeZone(int facility_id)
        //{
        //    try
        //    {
        //        using (var repos = new UtilsRepository(getDB))
        //        {
        //            return await repos.GetTimeZone(facility_id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        public async Task<List<CMDefaultResponse>> AddLog(CMLog log)
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

        public async Task<List<CMLog>> GetHistoryLog(CMMS.CMMS_Modules module_type, int id, string facilitytime)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.GetHistoryLog(module_type, id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Changes
        async Task<DateTime> IUtilsBS.Contvertime(int facility_id, DateTime datetime)
        {
            try
            {
                using (var repos = new UtilsRepository(getDB))
                {
                    return await repos.Contvertime( facility_id,datetime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}