using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Http;
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
                                $"{(int)log.module_type}, {log.module_ref_id}, {(int)log.secondary_module_type}, {log.secondary_module_ref_id}," +
                                $"'{log.comment}', {(int)log.status}, '{log.latitude}', '{log.longitude}', {GetUserID()}, '{GetUTCTime()}'" +
                            ")";

            await Context.GetData<List<int>>(qry).ConfigureAwait(false);
            return null;
        }

        internal async Task<List<CMDefaultResponse>> AddHistoryLog(CMMS.CMMS_Modules module_type, int module_ref_id, CMMS.CMMS_Modules secondary_module_type, int secondary_module_ref_id, string comment, CMMS.CMMS_Status status, int userID=0, string current_latitude="", string current_longitude="")
        {

            string qry = "INSERT INTO History" +
                            "(" +
                                "moduleType, moduleRefId, secondaryModuleRefType, secondaryModuleRefId, comment, " +
                                "status, currentLatitude, currentLongitude, createdBy, createdAt" +
                            ")" +
                        "VALUES" +
                            "(" +
                                $"{(int)module_type}, {module_ref_id}, {(int)secondary_module_type}, {secondary_module_ref_id}," +
                                $"'{comment}', {(int)status}, '{current_latitude}', '{current_longitude}', {userID}, '{GetUTCTime()}'" +
                            ")";

            await Context.GetData<List<int>>(qry).ConfigureAwait(false);
            return null;
        }

        internal async Task<List<CMDefaultResponse>> SendNotification(int module_type, int status_event, int module_ref_id)
        {
            return null;
        }


        internal async Task<List<CMLog>> GetHistoryLog(int module_type, int id)
        {
            /*
             * Fetch data from History table for requested module_type and id
             * Return Log
            */
            string myQuery = "select history.Id as id, moduleType as module_type, moduleRefId as module_ref_id, secondaryModuleRefType as sec_module, secondaryModuleRefId as sec_ref_id, comment as comment, history.status as status, history.createdBy as created_by_id, CONCAT(created_user.firstName,' ',created_user.lastName) as created_by_name, history.createdAt as created_at, history.currentLatitude as current_latitude, history.currentLongitude as current_longitude from history join users as created_user on history.createdBy=created_user.id " +
                $"WHERE (moduleType = {module_type} or secondaryModuleRefType = {module_type}) AND (moduleRefId = {id} or secondaryModuleRefId = {id})";
            List<CMLog> _Log = await Context.GetData<CMLog>(myQuery).ConfigureAwait(false);
            return _Log;
        }

        // Return User ID
        internal static int GetUserID()
        {
            // Pending : Fetch User Id from Token
            return 232;
        }

        // Return UTC time
        internal static string GetUTCTime()
        {
            DateTime Utc = DateTime.UtcNow;
            return Utc.ToString("yyyy-MM-dd hh:mm:ss");
        }


    }
}
