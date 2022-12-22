﻿using System;
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

        internal async Task<List<DefaultListModel>> GetCountryList()
        {
            string myQuery = "SELECT id, name FROM Countries";
            List<DefaultListModel> _List = await Context.GetData<DefaultListModel>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<DefaultListModel>> GetStateList(int country_id)
        {
            string myQuery = "SELECT id, name FROM States WHERE country_id = "+country_id;
            List<DefaultListModel> _List = await Context.GetData<DefaultListModel>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<DefaultListModel>> GetCityList(int state_id)
        {
            string myQuery = "SELECT id, name FROM Cities WHERE state_id = " + state_id;
            List<DefaultListModel> _List = await Context.GetData<DefaultListModel>(myQuery).ConfigureAwait(false);
            return _List;
        }
        internal async Task<List<CurrencyModel>> GetCurrencyList()
        {
            string myQuery = "SELECT id,code, name FROM Currency";
            List<CurrencyModel> _List = await Context.GetData<CurrencyModel>(myQuery).ConfigureAwait(false);
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

        internal async Task<List<DefaultResponseModel>> AddLog(LogModel log)
        {
            /*
             * Insert the log model properties to History table
             * Return inserted id/log
            */
            return null;
        }

        internal async Task<List<LogModel>> GetLog(int module_type, int id)
        {
            /*
             * Fetch data from History table for requested module_type and id
             * Return Log
            */
            return null;
        }


    }
}
