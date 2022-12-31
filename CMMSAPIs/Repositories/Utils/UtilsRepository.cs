using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories.Utils
{
    public class UtilsRepository : GenericRepository
    {
        public UtilsRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMDefaultList>> GetCountryList()
        {
            string myQuery = "SELECT id, name FROM Countries";
            List<CMDefaultList> _List = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<CMDefaultList>> GetStateList(int country_id)
        {
            string myQuery = "SELECT id, name FROM States WHERE country_id = "+country_id;
            List<CMDefaultList> _List = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<CMDefaultList>> GetCityList(int state_id)
        {
            string myQuery = "SELECT id, name FROM Cities WHERE state_id = " + state_id;
            List<CMDefaultList> _List = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _List;
        }
        internal async Task<List<CMCurrency>> GetCurrencyList()
        {
            string myQuery = "SELECT id,code, name FROM Currency";
            List<CMCurrency> _List = await Context.GetData<CMCurrency>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<TZone>> GetTimeZoneList()
        {
            /*
             * Add offset, standard_name, display_name in List and return. Do test the list returning rigth timezone name and offset
             * If below code is not returning right timezone and offset search on google and implement this function
             * Return : list Of Timezone with offset
            */

            //foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
            //{
            //  offset = z.BaseUtcOffset ;
            //  standard_name = z.StandardName;
            //  display_name = z.DisplayName;
            //}


            return null;
        }

        internal async Task<List<CMDefaultResponse>> AddLog(CMLog log)
        {
            string qry = "INSERT INTO History" +
                            "(" +
                                "moduleType, moduleRefId, secondaryModuleRefType, secondaryModuleRefId, comment, " +
                                "status, currentLatitude, currentLongitude, createdBy, createdAt" +
                            ")" +
                        "VALUES" +
                            "(" +
                                $"{log.module_type}, {log.module_ref_id}, {log.secondary_module_type}, {log.secondary_module_ref_id}," +
                                $"'{log.comment}', {log.status}, '{log.current_latitude}', '{log.current_longitude}', {log.created_by}, {log.created_at}" +
                            ")";

            await Context.GetData<List<int>>(qry).ConfigureAwait(false);
            return null;
        }

        internal async Task<List<CMLog>> GetLog(int module_type, int id)
        {
            /*
             * Fetch data from History table for requested module_type and id
             * Return Log
            */
            return null;
        }


    }
}
